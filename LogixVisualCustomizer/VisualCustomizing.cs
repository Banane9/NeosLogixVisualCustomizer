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
    internal static class VisualCustomizing
    {
        public static Slot AddBorder(this Slot root, SpriteProvider sprite, string tintSetting, long orderOffset = -1)
        {
            var borderSlot = root.AddSlot("Border");
            borderSlot.OrderOffset = orderOffset;

            var borderImage = borderSlot.AttachComponent<Image>();
            borderImage.Tint.OverrideWith(tintSetting);
            borderImage.Sprite.Target = sprite;

            return borderSlot;
        }

        public static void Customize(this Button button, SpriteProvider inputBackground, SpriteProvider inputBorder)
        {
            button.BaseColor.OverrideWith(SettingOverrides.InputBackgroundColor);
            button.ColorDrivers[0].TintColorMode.Value = InteractionElement.ColorMode.Multiply;

            var buttonSlot = button.Slot;

            var buttonImage = buttonSlot.GetComponent<Image>();
            buttonImage.Sprite.Target = inputBackground;

            buttonSlot.AddBorder(inputBorder, SettingOverrides.InputBorderColor);

            var buttonText = buttonSlot.GetComponentInChildren<Text>();
            buttonText.Color.OverrideWith(SettingOverrides.TextColor);

            var textRect = buttonText.RectTransform;
            textRect.AnchorMin.Value = new float2(0.1f, 0.1f);
            textRect.AnchorMax.Value = new float2(0.9f, 0.9f);
        }

        public static void CustomizeDisplay(this Text text)
        {
            text.Color.OverrideWith(SettingOverrides.TextColor);
        }

        public static void CustomizeHorizontal(this Button[] buttons)
        {
            for (var i = 0; i < buttons.Length; ++i)
            {
                var button = buttons[i];

                button.GetHorizontalInputProviders(i, buttons.Length, out var inputBackground, out var inputBorder);
                button.Customize(inputBackground, inputBorder);
            }
        }

        public static void CustomizeLabel(this Text text)
        {
            var textPadding = text.Slot.Parent.AddSlot("TextPadding");
            textPadding.AttachComponent<LayoutElement>();
            text.Slot.Parent = textPadding;

            text.Color.OverrideWith(SettingOverrides.TextColor);
            text.RectTransform.OffsetMin.Value = new float2(8, 0);
            text.RectTransform.OffsetMax.Value = new float2(-8, 0);
        }

        public static void CustomizeVertical(this Button[] buttons)
        {
            for (var i = 0; i < buttons.Length; ++i)
            {
                var button = buttons[i];

                button.GetVerticalInputProviders(i, buttons.Length, out var inputBackground, out var inputBorder);
                button.Customize(inputBackground, inputBorder);
            }
        }
    }
}