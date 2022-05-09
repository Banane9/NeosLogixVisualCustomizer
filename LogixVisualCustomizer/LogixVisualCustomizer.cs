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
        private static readonly MethodInfo onGenerateVisualPatch = typeof(TextFieldPatch).GetMethod(nameof(TextFieldPatch.OnGenerateVisualPrefix), AccessTools.all);

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<string> BackgroundSpriteUriKey = new ModConfigurationKey<string>("BackgroundSpriteUri", "Uri of the sprite for the background. Leave empty for default square.", () => new Uri("neosdb:///427a01c03424b86b4b8ffba936e4eb6cbf4be4d6773fa1f45ec004cfb526d016.png"));

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<string> BorderSpriteUriKey = new ModConfigurationKey<string>("BorderSpriteUri", "Uri of the sprite for the border. Leave empty to remove border.", () => new Uri("neosdb:///5428bc486550ce28de2cd928936062662d841f027a70f8cb38b3723daf03496f.png"));

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> EnableCustomLogixVisualsKey = new ModConfigurationKey<bool>("EnableCustomLogixVisuals", "Use the custom layout and settings here to generate LogiX visuals.", () => true);

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> EnableLeftNullButtonKey = new ModConfigurationKey<bool>("EnableLeftNullButton", "Swap the null Button on some inputs to the left.", () => true);

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<float4> FullBorderBordersKey = new ModConfigurationKey<float4>("FullBorderBorders", "Borders for the Node's borders sprite with all corners.", () => new float4(.5f, .5f, .5f, .5f));

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<Rect> FullBorderRectKey = new ModConfigurationKey<Rect>("FullBorderRect", "Rect for the Node's borders sprite with all corners.", () => new Rect(0, 0, 1, 1));

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<color> InputBackgroundColorKey = new ModConfigurationKey<color>("InputBackgroundColor", "Color of the Node's Input's background.", () => new color(.45f, 0.8f));

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<color> InputBorderColorKey = new ModConfigurationKey<color>("InputBorderColor", "Color of the Node's Input's border.", () => new color(.16f));

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<float> InputBorderScaleKey = new ModConfigurationKey<float>("InputBorderScale", "Scale for the Node's Input's borders sprites.", () => .03f);

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<float4> LeftBorderBordersKey = new ModConfigurationKey<float4>("LeftBorderBorders", "Borders for the Node's borders sprite with only the left corners.", () => new float4(1, .5f, 0, .5f));

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<Rect> LeftBorderRectKey = new ModConfigurationKey<Rect>("LeftBorderRect", "Rect for the Node's borders sprite with only the left corners.", () => new Rect(0, 0, .5f, 1));

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<color> NodeBackgroundColorKey = new ModConfigurationKey<color>("NodeBackgroundColor", "Color of the Node's background.", () => new color(.45f, 0.8f));

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<color> NodeBorderColorKey = new ModConfigurationKey<color>("NodeBorderColor", "Color of the Node's border.", () => new color(.45f));

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<float> NodeBorderScaleKey = new ModConfigurationKey<float>("NodeBorderScale", "Scale for the Node's borders sprite.", () => .03f);

        public override string Author => "Banane9";
        public override string Link => "https://github.com/Banane9/NeosLogixVisualCustomizer";
        public override string Name => "LogixVisualCustomizer";
        public override string Version => "1.0.0";
        internal static Uri BackgroundSpriteUri => new Uri(Config.GetValue(BackgroundSpriteUriKey));
        internal static Uri BorderSpriteUri => new Uri(Config.GetValue(BorderSpriteUriKey));
        internal static bool EnableCustomLogixVisuals => Config.GetValue(EnableCustomLogixVisualsKey);
        internal static bool EnableLeftNullButton => Config.GetValue(EnableLeftNullButtonKey);
        internal static float4 FullBorderBorders => Config.GetValue(FullBorderBordersKey);
        internal static Rect FullBorderRect => Config.GetValue(FullBorderRectKey);
        internal static color InputBackgroundColor => Config.GetValue(InputBackgroundColorKey);
        internal static color InputBorderColor => Config.GetValue(InputBorderColorKey);
        internal static float InputBorderScale => Config.GetValue(InputBorderScaleKey);
        internal static float4 LeftBorderBorders => Config.GetValue(LeftBorderBordersKey);
        internal static Rect LeftBorderRect => Config.GetValue(LeftBorderRectKey);
        internal static color NodeBackgroundColor => Config.GetValue(NodeBackgroundColorKey);
        internal static color NodeBorderColor => Config.GetValue(NodeBorderColorKey);
        internal static float NodeBorderScale => Config.GetValue(NodeBorderScaleKey);
        internal static bool UseBackground => !string.IsNullOrWhiteSpace(Config.GetValue(BackgroundSpriteUriKey));
        internal static bool UseBorder => !string.IsNullOrWhiteSpace(Config.GetValue(BorderSpriteUriKey));

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

                harmony.Patch(methodInfo, new HarmonyMethod(onGenerateVisualPatch));
            }
        }
    }
}