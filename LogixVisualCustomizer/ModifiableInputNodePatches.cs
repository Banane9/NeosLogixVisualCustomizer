using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.LogiX.Utility;
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
    internal static class ModifiableInputNodePatches
    {
        [HarmonyTargetMethods]
        public static IEnumerable<MethodBase> TargetMethods()
        {
            return LogixVisualCustomizer.GenerateGenericMethodTargets(
                LogixVisualCustomizer.neosPrimitiveTypes,
                "OnGenerateVisual",
                typeof(DualInputOperator<>),
                typeof(IndexOfFirstMatch<>));
        }

        [HarmonyPostfix]
        private static void OnGenerateVisualPostfix(Slot root)
        {
            var horizontal = root.GetComponentInChildren<HorizontalLayout>();
            horizontal.PaddingTop.Value = 3;
            horizontal.PaddingBottom.Value = 3;
            horizontal.PaddingLeft.Value = 4;
            horizontal.PaddingRight.Value = 4;

            root.GetComponentsInChildren<Button>(LogixVisualCustomizer.ButtonFilter).ToArray().CustomizeHorizontal();
        }
    }
}