using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using BaseX;
using CodeX;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.LogiX.Data;
using FrooxEngine.LogiX.Input;
using FrooxEngine.LogiX.ProgramFlow;
using FrooxEngine.UIX;
using HarmonyLib;
using NeosModLoader;

namespace LogixVisualCustomizer
{
    public class LogixVisualCustomizer : NeosMod
    {
        public static ModConfiguration Config;
        private static readonly float4 defaultSlices = new float4(0, 0, 1, 1);
        private static readonly MethodInfo onGenerateVisualPatch = typeof(TextFieldPatch).GetMethod(nameof(TextFieldPatch.OnGenerateVisualPrefix), AccessTools.all);

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<float4> BackgroundHorizontalSlicesKey = new ModConfigurationKey<float4>("BackgroundHorizontalSlices", "Positions for start and end of bottom, as well as top slices for the background sprite. Middle is implicit.", () => new float4(0, .5f, .5f, 1));

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<string> BackgroundSpriteUriKey = new ModConfigurationKey<string>("BackgroundSpriteUri", "Uri of the sprite for the background. Leave empty for default square.", () => "neosdb:///1e64bbda2fb62373fd3b82ae4f96a60daebaff81d690c96bbe03d10871221209.png");

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<float4> BackgroundVerticalSlicesKey = new ModConfigurationKey<float4>("BackgroundVerticalSlices", "Positions for start and end of left, as well as right slices for the background sprite. Middle is implicit.", () => new float4(0, .5f, .5f, 1));

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<float4> BorderHorizontalSlicesKey = new ModConfigurationKey<float4>("BorderHorizontalSlicesKey", "Positions for start and end of bottom, as well as top slices for the border sprite. Middle is implicit.", () => new float4(0, .5f, .5f, 1));

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<string> BorderSpriteUriKey = new ModConfigurationKey<string>("BorderSpriteUri", "Uri of the sprite for the border. Leave empty to remove border.", () => "neosdb:///518299baeefe744aa609c9b2c77c5930b6593c051b38eba116ff9177e8200a4f.png");

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<float4> BorderVerticalSlicesKey = new ModConfigurationKey<float4>("BorderVerticalSlices", "Positions for start and end of left, as well as right slices for the border sprite. Middle is implicit.", () => new float4(0, .5f, .5f, 1));

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> EnableCustomLogixVisualsKey = new ModConfigurationKey<bool>("EnableCustomLogixVisuals", "Use the custom layout and settings here to generate LogiX visuals.", () => true);

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> EnableLeftNullButtonKey = new ModConfigurationKey<bool>("EnableLeftNullButton", "Swap the null Button on some inputs to the left.", () => true);

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<color> InputBackgroundColorKey = new ModConfigurationKey<color>("InputBackgroundColor", "Color of the Node's Input's background.", () => new color(.26f));

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<float> InputBackgroundScaleKey = new ModConfigurationKey<float>("InputBackgroundScale", "Scale for the Node's Input's background sprites.", () => .03f);

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<color> InputBorderColorKey = new ModConfigurationKey<color>("InputBorderColor", "Color of the Node's Input's border.", () => new color(.16f));

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<float> InputBorderScaleKey = new ModConfigurationKey<float>("InputBorderScale", "Scale for the Node's Input's borders sprites.", () => .03f);

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<color> NodeBackgroundColorKey = new ModConfigurationKey<color>("NodeBackgroundColor", "Color of the Node's background.", () => new color(.22f));

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<float> NodeBackgroundScaleKey = new ModConfigurationKey<float>("NodeBackgroundScale", "Scale for the Node's background sprites.", () => .03f);

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<color> NodeBorderColorKey = new ModConfigurationKey<color>("NodeBorderColor", "Color of the Node's border.", () => new color(.54f));

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<float> NodeBorderScaleKey = new ModConfigurationKey<float>("NodeBorderScale", "Scale for the Node's borders sprite.", () => .03f);

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<color> TextColorKey = new ModConfigurationKey<color>("TextColor", "Color of text on the Node.", () => new color(.95f));

        public override string Author => "Banane9";
        public override string Link => "https://github.com/Banane9/NeosLogixVisualCustomizer";
        public override string Name => "LogixVisualCustomizer";
        public override string Version => "1.0.0";
        internal static float4 BackgroundHorizontalSlices => UseBackground ? Config.GetValue(BackgroundHorizontalSlicesKey) : defaultSlices;
        internal static Uri BackgroundSpriteUri => UseBackground ? new Uri(Config.GetValue(BackgroundSpriteUriKey)) : null;
        internal static float4 BackgroundVerticalSlices => UseBackground ? Config.GetValue(BackgroundVerticalSlicesKey) : defaultSlices;
        internal static float4 BorderHorizontalSlices => UseBorder ? Config.GetValue(BorderHorizontalSlicesKey) : defaultSlices;
        internal static Uri BorderSpriteUri => UseBorder ? new Uri(Config.GetValue(BorderSpriteUriKey)) : null;
        internal static float4 BorderVerticalSlices => UseBorder ? Config.GetValue(BorderVerticalSlicesKey) : defaultSlices;
        internal static float4 BottomBackgroundBorders => Slices.GetBottomBorders(BackgroundVerticalSlices, BackgroundHorizontalSlices);
        internal static Rect BottomBackgroundRect => Slices.GetBottomRect(BackgroundVerticalSlices, BackgroundHorizontalSlices);
        internal static float4 BottomBorderBorders => Slices.GetBottomBorders(BorderVerticalSlices, BorderHorizontalSlices);
        internal static Rect BottomBorderRect => Slices.GetBottomRect(BorderVerticalSlices, BorderHorizontalSlices);
        internal static bool EnableCustomLogixVisuals => Config.GetValue(EnableCustomLogixVisualsKey);
        internal static bool EnableLeftNullButton => Config.GetValue(EnableLeftNullButtonKey);
        internal static float4 FullBackgroundBorders => Slices.GetFullBorders(BackgroundVerticalSlices, BackgroundHorizontalSlices);
        internal static Rect FullBackgroundRect => Slices.GetFullRect(BackgroundVerticalSlices, BackgroundHorizontalSlices);
        internal static float4 FullBorderBorders => Slices.GetFullBorders(BorderVerticalSlices, BorderHorizontalSlices);
        internal static Rect FullBorderRect => Slices.GetFullRect(BorderVerticalSlices, BorderHorizontalSlices);
        internal static float4 HorizontalMiddleBackgroundBorders => Slices.GetHorizontalMiddleBorders(BackgroundHorizontalSlices);
        internal static Rect HorizontalMiddleBackgroundRect => Slices.GetHorizontalMiddleRect(BackgroundVerticalSlices, BackgroundHorizontalSlices);
        internal static float4 HorizontalMiddleBorderBorders => Slices.GetHorizontalMiddleBorders(BorderHorizontalSlices);
        internal static Rect HorizontalMiddleBorderRect => Slices.GetHorizontalMiddleRect(BorderVerticalSlices, BorderHorizontalSlices);
        internal static color InputBackgroundColor => Config.GetValue(InputBackgroundColorKey);
        internal static float InputBackgroundScale => Config.GetValue(InputBackgroundScaleKey);
        internal static color InputBorderColor => Config.GetValue(InputBorderColorKey);
        internal static float InputBorderScale => Config.GetValue(InputBorderScaleKey);
        internal static float4 LeftBackgroundBorders => Slices.GetLeftBorders(BackgroundVerticalSlices, BackgroundHorizontalSlices);
        internal static Rect LeftBackgroundRect => Slices.GetLeftRect(BackgroundVerticalSlices, BackgroundHorizontalSlices);
        internal static float4 LeftBorderBorders => Slices.GetLeftBorders(BorderVerticalSlices, BorderHorizontalSlices);
        internal static Rect LeftBorderRect => Slices.GetLeftRect(BorderVerticalSlices, BorderHorizontalSlices);
        internal static color NodeBackgroundColor => Config.GetValue(NodeBackgroundColorKey);
        internal static float NodeBackgroundScale => Config.GetValue(NodeBackgroundScaleKey);
        internal static color NodeBorderColor => Config.GetValue(NodeBorderColorKey);
        internal static float NodeBorderScale => Config.GetValue(NodeBorderScaleKey);
        internal static float4 RightBackgroundBorders => Slices.GetRightBorders(BackgroundVerticalSlices, BackgroundHorizontalSlices);
        internal static Rect RightBackgroundRect => Slices.GetRightRect(BackgroundVerticalSlices, BackgroundHorizontalSlices);
        internal static float4 RightBorderBorders => Slices.GetRightBorders(BorderVerticalSlices, BorderHorizontalSlices);
        internal static Rect RightBorderRect => Slices.GetRightRect(BorderVerticalSlices, BorderHorizontalSlices);
        internal static color TextColor => Config.GetValue(TextColorKey);
        internal static float4 TopBackgroundBorders => Slices.GetTopBorders(BackgroundVerticalSlices, BackgroundHorizontalSlices);
        internal static Rect TopBackgroundRect => Slices.GetTopRect(BackgroundVerticalSlices, BackgroundHorizontalSlices);
        internal static float4 TopBorderBorders => Slices.GetTopBorders(BorderVerticalSlices, BorderHorizontalSlices);
        internal static Rect TopBorderRect => Slices.GetTopRect(BorderVerticalSlices, BorderHorizontalSlices);
        internal static bool UseBackground => !string.IsNullOrWhiteSpace(Config.GetValue(BackgroundSpriteUriKey));
        internal static bool UseBorder => !string.IsNullOrWhiteSpace(Config.GetValue(BorderSpriteUriKey));
        internal static float4 VerticalMiddleBackgroundBorders => Slices.GetVerticalMiddleBorders(BackgroundVerticalSlices);
        internal static Rect VerticalMiddleBackgroundRect => Slices.GetVerticalMiddleRect(BackgroundVerticalSlices, BackgroundHorizontalSlices);
        internal static float4 VerticalMiddleBorderBorders => Slices.GetVerticalMiddleBorders(BorderVerticalSlices);
        internal static Rect VerticalMiddleBorderRect => Slices.GetVerticalMiddleRect(BorderVerticalSlices, BorderHorizontalSlices);

        public override void OnEngineInit()
        {
            Harmony harmony = new Harmony($"{Author}.{Name}");
            Config = GetConfiguration();
            Config.Save(true);
            harmony.PatchAll();
            patchTextFieldInputs(harmony);
        }

        private static void patchTextFieldInputs(Harmony harmony)
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
    }
}