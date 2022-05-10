using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
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
    internal static class BoolInputPatches
    {
        [HarmonyPostfix]
        [HarmonyPatch("OnGenerateVisual")]
        private static void OnGenerateVisualPostfix(Slot root)
        {
            var buttons = root.GetComponentsInChildren<Button>().ToArray();

            SpriteProvider inputBackground;
            SpriteProvider inputBorder;

            for (var i = 0; i < buttons.Length; ++i)
            {
                //if (i == 0)
                //{
                //    if (buttons.Length == 1)
                //    {
                //        inputBackground = root.GetLeftInputBackgroundProvider();
                //        inputBorder = root.GetLeftInputBorderProvider();
                //    }
                //    else
                //    {
                //        inputBackground = root.GetTopInputBackgroundProvider();
                //        inputBorder = root.GetTopInputBorderProvider();
                //    }
                //}
                //else if (i == buttons.Length - 1)
                //{
                //    inputBackground = root.GetBottomInputBackgroundProvider();
                //    inputBorder = root.GetBottomInputBorderProvider();
                //}
                //else
                //{
                //    inputBackground = root.GetMiddleInputBackgroundProvider();
                //    inputBorder = root.GetMiddleInputBorderProvider();
                //}

                var button = buttons[i];
                button.BaseColor.Value = LogixVisualCustomizer.InputBackgroundColor;
                button.ColorDrivers[0].TintColorMode.Value = InteractionElement.ColorMode.Multiply;

                var buttonSlot = button.Slot;

                var buttonImage = buttonSlot.GetComponent<Image>();
                buttonImage.Sprite.Target = root.GetLeftInputBackgroundProvider();

                var borderSlot = buttonSlot.AddSlot("Border");
                borderSlot.OrderOffset = -1;

                var borderImage = borderSlot.AttachComponent<Image>();
                borderImage.Tint.Value = LogixVisualCustomizer.InputBorderColor;
                borderImage.Sprite.Target = root.GetLeftInputBorderProvider();

                var buttonText = buttonSlot.GetComponentInChildren<Text>();
                buttonText.Color.Value = LogixVisualCustomizer.TextColor;

                var textRect = buttonText.RectTransform;
                textRect.AnchorMin.Value = new float2(0.1f, 0.1f);
                textRect.AnchorMax.Value = new float2(0.9f, 0.9f);
            }
        }
    }
}