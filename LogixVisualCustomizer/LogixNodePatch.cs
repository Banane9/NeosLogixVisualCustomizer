using FrooxEngine;
using FrooxEngine.LogiX;
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
        [HarmonyPostfix]
        [HarmonyPatch("GenerateUI")]
        private static void GenerateUIPostfix(Slot root)
        {
            if (!LogixVisualCustomizer.EnableCustomLogixVisuals)
                return;

            var background = root[0][0].GetComponent<Image>();
            background.Tint.Value = LogixVisualCustomizer.NodeBackgroundColor;
            background.Sprite.Target = root.GetBackgroundProvider();

            var border = root.Find("Image").AddSlot("Border");
            border.OrderOffset = -1;

            var borderImage = border.AttachComponent<Image>();
            borderImage.Tint.Value = LogixVisualCustomizer.NodeBorderColor;
            borderImage.Sprite.Target = root.GetNodeBorderProvider();
        }
    }
}