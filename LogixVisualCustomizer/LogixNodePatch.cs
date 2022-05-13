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
        private static readonly Type impulseRelayType = typeof(ImpulseRelay);
        private static readonly Type valueRelayType = typeof(RelayNode<>);

        [HarmonyPostfix]
        [HarmonyPatch("GenerateUI")]
        private static void GenerateUIPostfix(LogixNode __instance, Slot root)
        {
            var backgroundSlot = root.FindInChildren("Image");

            var background = backgroundSlot.GetComponent<Image>();
            background.Sprite.Target = root.GetNodeBackgroundProvider();

            if (!__instance.Enabled)
                background.Tint.Value = __instance.NodeErrorBackground.SetA(1);
            else if (__instance.NodeBackground != LogixNode.DEFAULT_NODE_BACKGROUND)
                background.Tint.Value = __instance.NodeBackground.SetA(1);
            else
                background.Tint.OverrideWith(SettingOverrides.NodeBackgroundColor);

            var type = __instance.GetType();
            if (type == impulseRelayType || (type.IsGenericType && type.GetGenericTypeDefinition() == valueRelayType))
                return;

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