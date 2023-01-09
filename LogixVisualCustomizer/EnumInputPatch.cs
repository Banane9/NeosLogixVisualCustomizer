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
        private static readonly Dictionary<Type, string[]> enumValueCache = new Dictionary<Type, string[]>();

        private static readonly Type fireOnChangeType = typeof(FireOnChange<>);
        private static readonly Type floatType = typeof(float);
        private static readonly Type intType = typeof(int);
        private static readonly string NextValueMethod = "NextValue";

        private static readonly Type objectType = typeof(object);
        private static readonly string PreviousValueMethod = "PreviousValue";

        private static void addScrollPositioningLogix(ScrollRect scrollRect, LogixNode enumInput, Type type)
        {
            var contentRoot = enumInput.Slot;
            var valueField = enumInput.TryGetField("Value");

            var valueToObjectCast = contentRoot.AttachCastNode(type, objectType, false);
            ((ISyncRef)valueToObjectCast.TryGetField("In")).Target = valueField;

            var valueToString = contentRoot.AttachComponent<ToString_Object>();
            valueToString.V.Target = (IElementContent<object>)valueToObjectCast;

            var contentRootRef = contentRoot.AttachComponent<ReferenceNode<Slot>>();
            contentRootRef.RefTarget.Target = scrollRect.Slot;

            var findValueSlot = contentRoot.AttachComponent<FindChildByName>();
            findValueSlot.Instance.Target = contentRootRef;
            findValueSlot.Name.Target = valueToString;

            var valueSlotIndex = contentRoot.AttachComponent<IndexOfChild>();
            valueSlotIndex.Instance.Target = findValueSlot.FoundChild;

            var contentRootChildren = contentRoot.AttachComponent<ChildrenCount>();
            contentRootChildren.Instance.Target = contentRootRef;

            var contentRootChildrenDec = contentRoot.AttachComponent<Dec_Int>();
            contentRootChildrenDec.A.Target = contentRootChildren;

            var valueSlotIndexCast = contentRoot.AttachCastNode(intType, floatType, false);
            valueSlotIndexCast.In.Target = valueSlotIndex;

            var contentRootChildrenDecCast = contentRoot.AttachCastNode(intType, floatType, false);
            contentRootChildrenDecCast.In.Target = contentRootChildrenDec;

            var scrollRectYOffset = contentRoot.AttachComponent<Div_Float>();
            scrollRectYOffset.A.Target = (IElementContent<float>)valueSlotIndexCast;
            scrollRectYOffset.B.Target = (IElementContent<float>)contentRootChildrenDecCast;

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

        private static void GenerateUIPostfix(LogixNode instance)
        {
            instance.ActiveVisual.GetComponentsInChildren<Button>(LogixVisualCustomizer.ButtonFilter).ForEach(VisualCustomizing.Customize);
            instance.ActiveVisual.GetComponentsInChildren<Text>(text => text.Slot.Parent.GetComponent<Button>() == null).ForEach(VisualCustomizing.CustomizeDisplay);

            return;

            var traverse = Traverse.Create(instance);
            var type = instance.GetType().GetGenericArguments()[0];

            var builder = traverse.Method("GenerateUI", instance.ActiveVisual, 256f, 40f).GetValue<UIBuilder>();

            builder.HorizontalLayout(4, 0, 4, 0, 0, null);

            builder.Style.MinWidth = 32;
            builder.Style.PreferredHeight = 32;
            builder.Button("<<", instance.getEventHandler(PreviousValueMethod)).Customize();

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

            addScrollPositioningLogix(scrollRect, instance, type);

            //SyncRef<Text> valueDisplay = this._valueDisplay;
            //localeString = "---";
            //valueDisplay.Target = uibuilder3.Text(localeString, true, null, false, null);

            builder.NestOut();

            builder.Style.FlexibleWidth = -1f;
            builder.Button(">>", instance.getEventHandler(NextValueMethod)).Customize();

            return;
        }

        private static ButtonEventHandler getEventHandler(this LogixNode enumInput, string method)
        {
            return AccessTools.MethodDelegate<ButtonEventHandler>(enumInput.GetType().GetMethod(method, AccessTools.allDeclared), enumInput);
        }

        [HarmonyPrepare]
        private static bool Initialize()
        {
            LogixNodePatch.RegisterPatch(PatchPosition.Postfix, "GenerateUI", typeof(EnumInput<>), GenerateUIPostfix);
            return false;
        }
    }
}