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
    //[HarmonyPatch]
    internal static class TextFieldPatch
    {
        //private static readonly Type textFieldNodeBaseType = typeof();

        //[HarmonyPrefix]
        //[HarmonyPatch("GenerateUI")]
        //private static bool GenerateUIPrefix(LogixNode __instance)
        //{
        //    if (!LogixVisualCustomizer.EnableCustomLogixVisuals)
        //        return true;

        //    return true;
        //}

        [HarmonyPrefix]
        [HarmonyPatch("OnGenerateVisual")]
        internal static bool OnGenerateVisualPrefix<T>(TextFieldNodeBase<T> __instance, Slot root)
        {
            if (!LogixVisualCustomizer.EnableCustomLogixVisuals)
                return true;

            var traverse = Traverse.Create(__instance);
            var leftNull = LogixVisualCustomizer.EnableLeftNullButton;

            var minWidth = traverse.Property<float>("MinWidth").Value;
            var fields = traverse.Property<int>("Fields").Value;

            var builder = (UIBuilder)traverse.Method("GenerateUI", root, minWidth, 4f * (fields + 1) + 32f * fields).GetValue();

            var inputBackground = leftNull ? root.GetLeftInputBackgroundProvider() : root.GetHorizontalMiddleInputBackgroundProvider();
            var inputBorder = leftNull ? root.GetLeftInputBorderProvider() : root.GetHorizontalMiddleInputBorderProvider();

            if (traverse.Property<bool>("NullButton").Value)
            {
                builder.HorizontalLayout(4, 0);
                builder.Style.MinHeight = 32f;
                builder.Style.MinWidth = 48f;

                var button = builder.Button("∅", AccessTools.MethodDelegate<ButtonEventHandler>(__instance.GetType().BaseType.BaseType.GetMethod("OnSetNull", AccessTools.all), __instance));
                button.Slot.OrderOffset = leftNull ? -1 : 1;
                button.Customize(inputBackground, inputBorder);

                inputBackground = leftNull ? root.GetHorizontalMiddleInputBackgroundProvider() : root.GetLeftInputBackgroundProvider();
                inputBorder = leftNull ? root.GetHorizontalMiddleInputBorderProvider() : root.GetLeftInputBorderProvider();
            }

            var vertical = builder.VerticalLayout(4, 0, null).Slot;
            builder.Style.MinHeight = 32f;
            builder.Style.FlexibleWidth = 1f;

            for (int i = 0; i < fields; i++)
            {
                traverse.Method("GenerateTextField", builder, i).GetValue();
            }

            var buttons = vertical.GetComponentsInChildren<Button>().ToArray();

            if (buttons.Length > 1)
                buttons.Customize();
            else
                buttons[0].Customize(inputBackground, inputBorder);

            return false;
        }
    }
}