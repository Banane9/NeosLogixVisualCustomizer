using BaseX;
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

            var backgroundSlot = root.FindInChildren("Image");

            var background = backgroundSlot.GetComponent<Image>();
            background.Tint.OverrideWith(SettingOverrides.NodeBackgroundColor);
            background.Sprite.Target = root.GetNodeBackgroundProvider();

            var borderSlot = backgroundSlot.AddSlot("Border");
            borderSlot.OrderOffset = -1;

            var borderImage = borderSlot.AttachComponent<Image>();
            borderImage.Tint.OverrideWith(SettingOverrides.NodeBorderColor);
            borderImage.Sprite.Target = root.GetNodeBorderProvider();

            backgroundSlot.ForeachComponentInChildren<Text>(VisualCustomizing.CustomizeLabel, cacheItems: true);

            foreach (var connector in backgroundSlot.Children.Where(child => child.Name == "Image").Select(child => child.GetComponent<Image>()))
                connector.Tint.Value = connector.Tint.Value.SetA(1).AddValueHDR(.1f);
        }
    }
}