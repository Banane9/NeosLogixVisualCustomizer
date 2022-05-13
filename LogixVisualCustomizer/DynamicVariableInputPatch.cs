using FrooxEngine;
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
    [HarmonyPatch]
    internal static class DynamicVariableInputPatch
    {
        [HarmonyTargetMethods]
        public static IEnumerable<MethodBase> TargetMethods()
        {
            return LogixVisualCustomizer.GenerateGenericMethodTargets(
                LogixVisualCustomizer.neosPrimitiveAndEnumTypes,
                "OnGenerateVisual",
                typeof(DynamicVariableInput<>));
        }

        [HarmonyPostfix]
        private static void OnGenerateVisualPostfix(Slot root)
        {
            root.GetComponentInChildren<Text>().CustomizeDisplay();

            root.GetComponentsInChildren<Button>(LogixVisualCustomizer.ButtonFilter).ToArray().CustomizeVertical();
        }
    }
}