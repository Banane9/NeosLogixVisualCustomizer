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
    internal static class TextFieldPatch
    {
        private static readonly MethodInfo onGenerateVisualPatch = typeof(TextFieldPatch).GetMethod(nameof(TextFieldPatch.OnGenerateVisualPrefix), AccessTools.all);
        private static readonly Dictionary<Type, MethodInfo> onSetNullMethods = new Dictionary<Type, MethodInfo>();
        private static readonly Type textFieldNodeBaseType = typeof(TextFieldNodeBase<>);

        public static void Patch(Harmony harmony)
        {
            foreach (var type in LogixVisualCustomizer.NeosPrimitiveTypes)
            {
                var createdType = textFieldNodeBaseType.MakeGenericType(type);
                var methodInfo = createdType.GetMethod("OnGenerateVisual", AccessTools.allDeclared);

                harmony.Patch(methodInfo, new HarmonyMethod(onGenerateVisualPatch.MakeGenericMethod(type)));
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch("OnGenerateVisual")]
        internal static bool OnGenerateVisualPrefix<T>(TextFieldNodeBase<T> __instance, Slot root)
        {
            var leftNull = LogixVisualCustomizer.EnableLeftNullButton;

            var minWidth = __instance.MinWidth;
            var fields = __instance.Fields;

            var builder = __instance.GenerateUI(root, minWidth, 4f * (fields + 1) + 32f * fields);

            var inputBackground = leftNull ? root.GetLeftInputBackgroundProvider() : root.GetHorizontalMiddleInputBackgroundProvider();
            var inputBorder = leftNull ? root.GetLeftInputBorderProvider() : root.GetHorizontalMiddleInputBorderProvider();

            if (__instance.NullButton)
            {
                builder.HorizontalLayout(4, 0);
                builder.Style.MinHeight = 32;
                builder.Style.MinWidth = 32;

                var button = builder.Button("∅", __instance.OnSetNull);
                button.Slot.OrderOffset = leftNull ? -1 : 1;
                button.Customize(inputBackground, inputBorder);

                inputBackground = leftNull ? root.GetHorizontalMiddleInputBackgroundProvider() : root.GetLeftInputBackgroundProvider();
                inputBorder = leftNull ? root.GetHorizontalMiddleInputBorderProvider() : root.GetLeftInputBorderProvider();
            }

            var vertical = builder.VerticalLayout(4, 0, null).Slot;
            builder.Style.MinHeight = 32;
            builder.Style.FlexibleWidth = 1;

            for (int i = 0; i < fields; i++)
            {
                __instance.GenerateTextField(builder, i);
            }

            var buttons = vertical.GetComponentsInChildren<Button>(LogixVisualCustomizer.ButtonFilter).ToArray();

            if (buttons.Length > 1)
                buttons.CustomizeVertical();
            else
                buttons[0].Customize(inputBackground, inputBorder);

            return false;
        }

        private static ButtonEventHandler getOnSetNull<T>(this TextFieldNodeBase<T> instance)
        {
            var type = instance.GetType();

            if (!onSetNullMethods.TryGetValue(type, out var method))
            {
                while (!type.IsGenericType || type.GetGenericTypeDefinition() != textFieldNodeBaseType)
                    type = type.BaseType;

                method = type.GetMethod("OnSetNull", AccessTools.allDeclared);
                onSetNullMethods.Add(instance.GetType(), method);
            }

            return AccessTools.MethodDelegate<ButtonEventHandler>(method, instance);
        }
    }
}