using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.LogiX.Input;
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
    internal static class ButtonInputPatches
    {
        [HarmonyPostfix]
        private static void OnGenerateVisualPostfix(Slot root)
        {
            var buttons = root.GetComponentsInChildren<Button>(LogixVisualCustomizer.ButtonFilter).ToArray();

            buttons.CustomizeVertical();
        }

        [HarmonyTargetMethods]
        private static IEnumerable<MethodBase> TargetMethods()
        {
            return new[] { typeof(BoolInput), typeof(Bool2Input), typeof(Bool3Input), typeof(Bool4Input), typeof(ImpulseInput) }
                .Select(t => t.GetMethod("OnGenerateVisual", AccessTools.allDeclared));
        }
    }
}