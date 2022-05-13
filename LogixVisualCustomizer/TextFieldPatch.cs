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

        public static void Patch(Harmony harmony)
        {
            var baseType = typeof(TextFieldNodeBase<>);
            var genericTypes = Traverse.Create(typeof(GenericTypes)).Field<Type[]>("neosPrimitives").Value
                .Where(type => type.Name != "String")
                .AddItem(typeof(object));

            foreach (var type in genericTypes)
            {
                var createdType = baseType.MakeGenericType(type);
                var methodInfo = createdType.GetMethod("OnGenerateVisual", AccessTools.allDeclared);

                harmony.Patch(methodInfo, new HarmonyMethod(onGenerateVisualPatch.MakeGenericMethod(type)));
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch("OnGenerateVisual")]
        internal static bool OnGenerateVisualPrefix<T>(TextFieldNodeBase<T> __instance, Slot root)
        {
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

            var buttons = vertical.GetComponentsInChildren<Button>(LogixVisualCustomizer.ButtonFilter).ToArray();

            if (buttons.Length > 1)
                buttons.CustomizeVertical();
            else
                buttons[0].Customize(inputBackground, inputBorder);

            return false;
        }
    }
}