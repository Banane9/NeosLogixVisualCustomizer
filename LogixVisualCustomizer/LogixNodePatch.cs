using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.LogiX.Cast;
using FrooxEngine.UIX;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogixVisualCustomizer
{
    [HarmonyPatch(typeof(LogixNode))]
    internal static class LogixNodePatch
    {
        private static readonly Type valueRelayType = typeof(RelayNode<>);

        [HarmonyPostfix]
        [HarmonyPatch("GenerateUI")]
        private static void GenerateUIPostfix(LogixNode __instance, Slot root)
        {
            var backgroundSlot = root.FindInChildren("Image");
            var background = backgroundSlot.GetComponent<Image>();

            foreach (var connector in backgroundSlot.Children.Where(child => child.Name == "Image").Select(child => child.GetComponent<Image>()))
                connector.Tint.Value = connector.Tint.Value.SetA(1).AddValueHDR(.1f);

            if (!__instance.Enabled)
                background.Tint.Value = __instance.NodeErrorBackground.SetA(1);
            else if (__instance.NodeBackground != LogixNode.DEFAULT_NODE_BACKGROUND)
                background.Tint.Value = __instance.NodeBackground.SetA(1);
            else
                background.Tint.DriveFromSharedSetting(LogixVisualCustomizer.NodeBackgroundColorKey, LogixVisualCustomizer.Config);

            var type = __instance.GetType();
            if (__instance is ImpulseRelay || __instance is ICastNode || (type.IsGenericType && type.GetGenericTypeDefinition() == valueRelayType))
                return;

            background.Sprite.Target = root.GetNodeBackgroundProvider();

            var borderSlot = backgroundSlot.AddSlot("Border");
            borderSlot.OrderOffset = -1;

            var borderImage = borderSlot.AttachComponent<Image>();
            borderImage.Tint.DriveFromSharedSetting(LogixVisualCustomizer.NodeBorderColorKey, LogixVisualCustomizer.Config);
            borderImage.Sprite.Target = root.GetNodeBorderProvider();

            backgroundSlot.ForeachComponentInChildren<Text>(VisualCustomizing.CustomizeLabel, cacheItems: true);
        }
    }
}