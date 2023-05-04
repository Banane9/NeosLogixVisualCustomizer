using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.LogiX.Data;
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
    // Have to reimplement the label on the node, as the generic type stamping from patching otherwise has every reference type labeled as object
    // Also add null button for variable name
    internal static class DynamicVariableInputPatch
    {
        [HarmonyPatch]
        internal static class Label
        {
            [HarmonyPrefix]
            private static bool LabelGetterPrefix(LogixNode __instance, ref string __result)
            {
                __result = $"Dynamic {__instance.GetType().GenericTypeArguments[0].GetNiceName("<", ">")} Variable";

                return false;
            }

            [HarmonyTargetMethods]
            private static IEnumerable<MethodBase> TargetMethods()
            {
                return LogixVisualCustomizer.GenerateGenericMethodTargets(
                    LogixVisualCustomizer.NeosPrimitiveAndEnumTypes,
                    "get_Label",
                    typeof(DynamicVariableInput<>));
            }
        }

        [HarmonyPatch]
        internal static class OnGenerateVisual
        {
            [HarmonyPrefix]
            private static bool OnGenerateVisualPrefix(LogixNode __instance, Slot root)
            {
                var instance = Traverse.Create(__instance);

                var memberEditor = root.AttachComponent<PrimitiveMemberEditor>();
                memberEditor.Continuous.Value = true;

                memberEditor._target.Target = instance.Field<Sync<string>>("_variableName").Value;

                var builder = instance.Method("GenerateUI", root, 384f, 76f).GetValue<UIBuilder>();

                root.GetComponentInChildren<LayoutElement>().FlexibleHeight.Value = 1;

                var vertical = root.GetComponentInChildren<VerticalLayout>();
                vertical.ForceExpandHeight.Value = false;
                vertical.Spacing.Value = 4;

                builder.Style.MinHeight = 32;
                builder.Panel();

                var horizontal = builder.HorizontalLayout(4, 0, 8, 0, 8);
                horizontal.ForceExpandWidth.Value = false;

                builder.Style.MinWidth = 32;
                var nullButton = builder.Button("∅", memberEditor.OnReset);
                memberEditor._resetButton.Target = nullButton;

                builder.Style.FlexibleWidth = 1;
                var nameInput = builder.TextField("", true, null, false);
                memberEditor._textEditor.Target = nameInput.Editor;
                memberEditor._textDrive.Target = nameInput.Text.Content;

                new[] { nullButton, nameInput.Slot.GetComponent<Button>() }.CustomizeHorizontal();

                return false;
            }

            [HarmonyTargetMethods]
            private static IEnumerable<MethodBase> TargetMethods()
            {
                return LogixVisualCustomizer.GenerateGenericMethodTargets(
                    LogixVisualCustomizer.NeosPrimitiveAndEnumTypes,
                    nameof(DynamicVariableInput<dummy>.OnGenerateVisual),
                    typeof(DynamicVariableInput<>));
            }
        }
    }
}