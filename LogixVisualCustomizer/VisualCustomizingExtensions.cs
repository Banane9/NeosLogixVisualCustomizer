using BaseX;
using FrooxEngine;
using FrooxEngine.UIX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogixVisualCustomizer
{
    internal static class VisualCustomizingExtensions
    {
        public static void Customize(this Button[] buttons)
        {
            for (var i = 0; i < buttons.Length; ++i)
            {
                var button = buttons[i];

                button.GetInputProviders(i, buttons.Length, out var inputBackground, out var inputBorder);
                button.Customize(inputBackground, inputBorder);
            }
        }

        public static void Customize(this Button button, SpriteProvider inputBackground, SpriteProvider inputBorder)
        {
            button.BaseColor.Value = LogixVisualCustomizer.InputBackgroundColor;
            button.ColorDrivers[0].TintColorMode.Value = InteractionElement.ColorMode.Multiply;

            var buttonSlot = button.Slot;

            var buttonImage = buttonSlot.GetComponent<Image>();
            buttonImage.Sprite.Target = inputBackground;

            var borderSlot = buttonSlot.AddSlot("Border");
            borderSlot.OrderOffset = -1;

            var borderImage = borderSlot.AttachComponent<Image>();
            borderImage.Tint.Value = LogixVisualCustomizer.InputBorderColor;
            borderImage.Sprite.Target = inputBorder;

            var buttonText = buttonSlot.GetComponentInChildren<Text>();
            buttonText.Color.Value = LogixVisualCustomizer.TextColor;

            var textRect = buttonText.RectTransform;
            textRect.AnchorMin.Value = new float2(0.1f, 0.1f);
            textRect.AnchorMax.Value = new float2(0.9f, 0.9f);
        }
    }
}