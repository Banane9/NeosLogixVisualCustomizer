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
        internal static bool OnGenerateVisualPrefix(LogixNode __instance, Slot root)
        {
            if (!LogixVisualCustomizer.EnableCustomLogixVisuals)
                return true;

            var traverse = Traverse.Create(__instance);
            var instanceType = __instance.GetType();
            LogixVisualCustomizer.Msg("GenerateVisual for " + __instance.GetType());

            var minWidth = (float)traverse.Property("MinWidth").GetValue();
            var fields = (int)traverse.Property("Fields").GetValue();

            var builder = (UIBuilder)traverse.Method("GenerateUI", root, minWidth, 4f * (fields + 1) + 32f * fields).GetValue();

            if ((bool)traverse.Property("NullButton").GetValue())
            {
                builder.HorizontalLayout(10, 0, LogixVisualCustomizer.EnableLeftNullButton ? Alignment.MiddleLeft : Alignment.MiddleRight);
                builder.Style.MinHeight = 32f;
                builder.Style.MinWidth = 48f;

                builder.Button("∅", AccessTools.MethodDelegate<ButtonEventHandler>(__instance.GetType().BaseType.BaseType.GetMethod("OnSetNull", AccessTools.all), __instance));
            }

            builder.VerticalLayout(4f, 0f, null);
            builder.Style.MinHeight = 32f;
            builder.Style.FlexibleWidth = 1f;

            for (int i = 0; i < fields; i++)
            {
                traverse.Method("GenerateTextField", builder, i).GetValue();
            }

            return false;
        }
    }
}