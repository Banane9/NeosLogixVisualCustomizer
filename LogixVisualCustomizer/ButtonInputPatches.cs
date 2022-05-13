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
        private static readonly Type impulseInputType = typeof(ImpulseInput);

        [HarmonyPostfix]
        private static void OnGenerateVisualPostfix(LogixNode __instance, Slot root)
        {
            var buttons = root.GetComponentsInChildren<Button>(LogixVisualCustomizer.ButtonFilter).ToArray();

            if (__instance.GetType() == impulseInputType)
            {
                var inputBackground = root.GetFullInputBackgroundProvider();
                var inputBorder = root.GetFullInputBorderProvider();

                buttons[0].Customize(inputBackground, inputBorder);

                return;
            }

            buttons.CustomizeVertical();
        }

        [HarmonyTargetMethods]
        private static IEnumerable<MethodBase> TargetMethods()
        {
            return new[] { typeof(BoolInput), typeof(Bool2Input), typeof(Bool3Input), typeof(Bool4Input), impulseInputType }
                .Select(t => t.GetMethod("OnGenerateVisual", AccessTools.allDeclared));
        }
    }
}