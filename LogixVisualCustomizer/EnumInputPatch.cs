using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.LogiX.Actions;
using FrooxEngine.LogiX.Cast;
using FrooxEngine.LogiX.Input;
using FrooxEngine.LogiX.Operators;
using FrooxEngine.LogiX.ProgramFlow;
using FrooxEngine.LogiX.WorldModel;
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
    [HarmonyPatch]
    internal static class EnumInputPatch
    {
        private static readonly Type castType = typeof(CastClass<,>);
        private static readonly Dictionary<Type, string[]> enumValueCache = new Dictionary<Type, string[]>();

        private static readonly Type fireOnChangeType = typeof(FireOnChange<>);
        private static readonly string NextValueMethod = "NextValue";

        private static readonly Type objectType = typeof(object);
        private static readonly string PreviousValueMethod = "PreviousValue";

        [HarmonyTargetMethods]
        public static IEnumerable<MethodBase> TargetMethods()
        {
            return LogixVisualCustomizer.GenerateGenericMethodTargets(
                AccessTools.GetTypesFromAssembly(typeof(EnumInput<>).Assembly).Concat(AccessTools.GetTypesFromAssembly(typeof(float4).Assembly)).Where(type => type.IsEnum && !type.IsNested),
                "OnGenerateVisual",
                typeof(EnumInput<>));
        }

        private static void addScrollPositioningLogix(ScrollRect scrollRect, LogixNode enumInput, Type type)
        {
            var contentRoot = scrollRect.Slot;
            var valueField = enumInput.TryGetField("Value");

            var valueToObjectCast = contentRoot.AttachComponent(castType.MakeGenericType(type, objectType));
            ((ISyncRef)valueToObjectCast.TryGetField("In")).Target = valueField;

            var valueToString = contentRoot.AttachComponent<ToString_Object>();
            valueToString.V.Target = (IElementContent<object>)valueToObjectCast;

            var contentRootRef = contentRoot.AttachComponent<ReferenceNode<Slot>>();
            contentRootRef.RefTarget.Target = contentRoot;

            var findValueSlot = contentRoot.AttachComponent<FindChildByName>();
            findValueSlot.Instance.Target = contentRootRef;
            findValueSlot.Name.Target = valueToString;

            var valueSlotIndex = contentRoot.AttachComponent<IndexOfChild>();
            valueSlotIndex.Instance.Target = findValueSlot.FoundChild;

            var contentRootChildren = contentRoot.AttachComponent<ChildrenCount>();
            contentRootChildren.Instance.Target = contentRootRef;

            var contentRootChildrenDec = contentRoot.AttachComponent<Dec_Int>();
            contentRootChildrenDec.A.Target = contentRootChildren;

            var valueSlotIndexCast = contentRoot.AttachComponent<Cast_int_To_float>();
            valueSlotIndexCast.In.Target = valueSlotIndex;

            var contentRootChildrenDecCast = contentRoot.AttachComponent<Cast_int_To_float>();
            contentRootChildrenDecCast.In.Target = contentRootChildrenDec;

            var scrollRectYOffset = contentRoot.AttachComponent<Div_Float>();
            scrollRectYOffset.A.Target = valueSlotIndexCast;
            scrollRectYOffset.B.Target = contentRootChildrenDecCast;

            var scrollRectOffsetPack = contentRoot.AttachComponent<Construct_Float2>();
            scrollRectOffsetPack.Y.Target = scrollRectYOffset;

            var scrollRectOffsetRef = contentRoot.AttachComponent<ReferenceNode<IValue<float2>>>();
            scrollRectOffsetRef.RefTarget.Target = scrollRect.NormalizedPosition;

            var writeScrollRectOffset = contentRoot.AttachComponent<WriteValueNode<float2>>();
            writeScrollRectOffset.Value.Target = scrollRectOffsetPack;
            writeScrollRectOffset.Target.Target = scrollRectOffsetRef;

            var valueFireOnChange = contentRoot.AttachComponent(fireOnChangeType.MakeGenericType(type));
            ((ISyncRef)valueFireOnChange.TryGetField("Value")).Target = valueField;
            ((Impulse)valueFireOnChange.TryGetField("Pulse")).Target = writeScrollRectOffset.Write;
        }

        private static ButtonEventHandler getEventHandler(this LogixNode enumInput, string method)
        {
            return AccessTools.MethodDelegate<ButtonEventHandler>(enumInput.GetType().GetMethod(method, AccessTools.allDeclared), enumInput);
        }

        [HarmonyPrefix]
        private static bool OnGenerateVisualPrefix(LogixNode __instance, Slot root)
        {
            var traverse = Traverse.Create(__instance);
            var type = __instance.GetType().GetGenericArguments()[0];

            var builder = traverse.Method("GenerateUI", root, 256f, 40f).GetValue<UIBuilder>();

            builder.HorizontalLayout(4, 0, 4, 0, 0, null);

            builder.Style.MinWidth = 32;
            builder.Style.PreferredHeight = 32;
            builder.Button("<<", __instance.getEventHandler(PreviousValueMethod)).Customize();

            builder.Style.FlexibleWidth = 1;
            var scrollRect = builder.ScrollArea();

            builder.FitContent(SizeFit.Disabled, SizeFit.PreferredSize);
            builder.VerticalLayout(4);

            if (!enumValueCache.TryGetValue(type, out var values))
            {
                var enumValues = Enum.GetValues(type);
                values = new string[enumValues.Length];

                for (var i = 0; i < enumValues.Length; ++i)
                    values[i] = enumValues.GetValue(i).ToString();

                enumValueCache.Add(type, values);
            }

            foreach (var value in values)
            {
                var text = builder.Text(value);
                text.CustomizeDisplay();
                text.Slot.Name = value;
            }

            addScrollPositioningLogix(scrollRect, __instance, type);

            //SyncRef<Text> valueDisplay = this._valueDisplay;
            //localeString = "---";
            //valueDisplay.Target = uibuilder3.Text(localeString, true, null, false, null);

            builder.NestOut();

            builder.Style.FlexibleWidth = -1f;
            builder.Button(">>", __instance.getEventHandler(NextValueMethod)).Customize();

            return false;
        }
    }
}