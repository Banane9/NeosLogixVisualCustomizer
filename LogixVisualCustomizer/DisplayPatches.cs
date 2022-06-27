using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.LogiX.Color;
using FrooxEngine.LogiX.Display;
using FrooxEngine.LogiX.Input;
using FrooxEngine.LogiX.Operators;
using FrooxEngine.UIX;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LogixVisualCustomizer
{
    [HarmonyPatch]
    internal static class DisplayPatches
    {
        private static readonly Type colorType = typeof(Display_Color);

        private static void createAlphaRemovalLogix(Slot root, Sync<color> source, Sync<color> target)
        {
            var value = root.AttachComponent<FloatInput>();
            value.CurrentValue = 1;

            var setA = root.AttachComponent<ColorSetAlpha>();
            setA.Color.Target = source;
            setA.Value.Target = Traverse.Create(value).Field<Sync<float>>("_value").Value;

            var driver = root.AttachComponent<DriverNode<color>>();
            driver.Source.Target = setA;
            driver.DriveTarget.Target = target;
        }

        [HarmonyPostfix]
        private static void OnGenerateVisualPostfix(LogixNode __instance, Slot root)
        {
            var type = __instance.GetType();
            root.ForeachComponentInChildren<Text>(VisualCustomizing.CustomizeDisplay);

            // Only create special display for color
            if (type != colorType)
                return;

            var colorDisplayRoot = root.GetComponentInChildren<HorizontalLayout>().Slot;
            var alphaColorImage = colorDisplayRoot.GetComponentInChildren<Image>();

            var builder = new UIBuilder(colorDisplayRoot);

            builder.Style.MinWidth = 64;
            var colorPanelRoot = builder.Panel().Slot;
            colorPanelRoot.OrderOffset = -1;
            builder.Style.MinWidth = -1;

            var vLayout = builder.VerticalLayout();
            vLayout.RectTransform.OffsetMin.Value = new float2(2, 10);
            vLayout.RectTransform.OffsetMax.Value = new float2(-2, -10);

            var mask = vLayout.Slot.AttachComponent<Mask>();
            mask.Slot.GetComponent<Image>().Sprite.Target = root.GetFullInputBackgroundProvider();

            var noAlphaImage = builder.Image();
            createAlphaRemovalLogix(root, alphaColorImage.Tint, noAlphaImage.Tint);

            var checkers = builder.Image(NeosAssets.Common.Backgrounds.TransparentLight64);
            alphaColorImage.Slot.Parent = checkers.Slot;

            var borderTransform = colorPanelRoot.AddBorder(root.GetFullInputBorderProvider(), LogixVisualCustomizer.InputBorderColorKey, 1).GetComponent<RectTransform>();
            borderTransform.OffsetMin.Value = new float2(0, 8);
            borderTransform.OffsetMax.Value = new float2(0, -8);
        }

        [HarmonyTargetMethods]
        private static IEnumerable<MethodBase> TargetMethods()
        {
            return AccessTools.GetTypesFromAssembly(typeof(Display_Dummy).Assembly)
                .Where(t => t.Namespace == "FrooxEngine.LogiX.Display")
                .Select(t => t.GetMethod("OnGenerateVisual", AccessTools.allDeclared))
                .Where(m => m != null); // Dummy doesn't have one
        }
    }
}