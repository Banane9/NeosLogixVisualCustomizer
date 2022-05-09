using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX.Input;
using FrooxEngine.UIX;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogixVisualCustomizer
{
    [HarmonyPatch(typeof(BoolInput))]
    internal static class BoolInputPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch("OnGenerateVisual")]
        private static void OnGenerateVisualPostfix(Slot root)
        {
            var textRect = root.GetComponentInChildren<Text>().RectTransform;
            textRect.AnchorMin.Value = new float2(0.1f, 0.1f);
            textRect.AnchorMax.Value = new float2(0.9f, 0.9f);
        }
    }
}