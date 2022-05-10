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
        private static ModConfigurationKey<string> BackgroundSpriteUriKey = new ModConfigurationKey<string>("BackgroundSpriteUri", "Uri of the sprite for the background. Leave empty for default square.", () => "neosdb:///1e64bbda2fb62373fd3b82ae4f96a60daebaff81d690c96bbe03d10871221209.png");

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<string> BorderSpriteUriKey = new ModConfigurationKey<string>("BorderSpriteUri", "Uri of the sprite for the border. Leave empty to remove border.", () => "neosdb:///518299baeefe744aa609c9b2c77c5930b6593c051b38eba116ff9177e8200a4f.png");

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> EnableCustomLogixVisualsKey = new ModConfigurationKey<bool>("EnableCustomLogixVisuals", "Use the custom layout and settings here to generate LogiX visuals.", () => true);

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<bool> EnableLeftNullButtonKey = new ModConfigurationKey<bool>("EnableLeftNullButton", "Swap the null Button on some inputs to the left.", () => true);

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<float4> FullBackgroundBordersKey = new ModConfigurationKey<float4>("FullBackgroundBorders", "Borders for the Node's background sprite with all corners.", () => new float4(.5f, .5f, .5f, .5f));

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<Rect> FullBackgroundRectKey = new ModConfigurationKey<Rect>("FullBackgroundRect", "Rect for the Node's background sprite with all corners.", () => new Rect(0, 0, 1, 1));

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<float4> FullBorderBordersKey = new ModConfigurationKey<float4>("FullBorderBorders", "Borders for the Node's borders sprite with all corners.", () => new float4(.5f, .5f, .5f, .5f));

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<Rect> FullBorderRectKey = new ModConfigurationKey<Rect>("FullBorderRect", "Rect for the Node's borders sprite with all corners.", () => new Rect(0, 0, 1, 1));

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<color> InputBackgroundColorKey = new ModConfigurationKey<color>("InputBackgroundColor", "Color of the Node's Input's background.", () => new color(.26f));

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<float> InputBackgroundScaleKey = new ModConfigurationKey<float>("InputBackgroundScale", "Scale for the Node's Input's background sprites.", () => .03f);

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<color> InputBorderColorKey = new ModConfigurationKey<color>("InputBorderColor", "Color of the Node's Input's border.", () => new color(.16f));

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<float> InputBorderScaleKey = new ModConfigurationKey<float>("InputBorderScale", "Scale for the Node's Input's borders sprites.", () => .03f);

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<float4> LeftBackgroundBordersKey = new ModConfigurationKey<float4>("LeftBackgroundBorders", "Borders for the Node's background sprite with only the left corners.", () => new float4(1, .5f, 0, .5f));

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<Rect> LeftBackgroundRectKey = new ModConfigurationKey<Rect>("LeftBackgroundRect", "Rect for the Node's background sprite with only the left corners.", () => new Rect(0, 0, .5f, 1));

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<float4> LeftBorderBordersKey = new ModConfigurationKey<float4>("LeftBorderBorders", "Borders for the Node's borders sprite with only the left corners.", () => new float4(1, .5f, 0, .5f));

        [AutoRegisterConfigKey]
        private static ModConfigurationKey<Rect> LeftBorderRectKey = new ModConfigurationKey<Rect>("LeftBorderRect", "Rect for the Node's borders sprite with only the left corners.", () => new Rect(0, 0, .5f, 1));

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
        internal static Uri BackgroundSpriteUri => new Uri(Config.GetValue(BackgroundSpriteUriKey));
        internal static Uri BorderSpriteUri => new Uri(Config.GetValue(BorderSpriteUriKey));
        internal static bool EnableCustomLogixVisuals => Config.GetValue(EnableCustomLogixVisualsKey);
        internal static bool EnableLeftNullButton => Config.GetValue(EnableLeftNullButtonKey);
        internal static float4 FullBackgroundBorders => Config.GetValue(FullBackgroundBordersKey);
        internal static Rect FullBackgroundRect => Config.GetValue(FullBackgroundRectKey);
        internal static float4 FullBorderBorders => Config.GetValue(FullBorderBordersKey);
        internal static Rect FullBorderRect => Config.GetValue(FullBorderRectKey);
        internal static color InputBackgroundColor => Config.GetValue(InputBackgroundColorKey);
        internal static float InputBackgroundScale => Config.GetValue(InputBackgroundScaleKey);
        internal static color InputBorderColor => Config.GetValue(InputBorderColorKey);
        internal static float InputBorderScale => Config.GetValue(InputBorderScaleKey);
        internal static float4 LeftBackgroundBorders => Config.GetValue(LeftBackgroundBordersKey);
        internal static Rect LeftBackgroundRect => Config.GetValue(LeftBackgroundRectKey);
        internal static float4 LeftBorderBorders => Config.GetValue(LeftBorderBordersKey);
        internal static Rect LeftBorderRect => Config.GetValue(LeftBorderRectKey);
        internal static color NodeBackgroundColor => Config.GetValue(NodeBackgroundColorKey);
        internal static float NodeBackgroundScale => Config.GetValue(NodeBackgroundScaleKey);
        internal static color NodeBorderColor => Config.GetValue(NodeBorderColorKey);
        internal static float NodeBorderScale => Config.GetValue(NodeBorderScaleKey);
        internal static color TextColor => Config.GetValue(TextColorKey);
        internal static bool UseBackground => !string.IsNullOrWhiteSpace(Config.GetValue(BackgroundSpriteUriKey));
        internal static bool UseBorder => !string.IsNullOrWhiteSpace(Config.GetValue(BorderSpriteUriKey));

        public override void OnEngineInit()
        {
            Harmony harmony = new Harmony($"{Author}.{Name}");
            Config = GetConfiguration();
            Config.OnThisConfigurationChanged += ConfigurationChanged;
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

        private void ConfigurationChanged(ConfigurationChangedEvent configurationChangedEvent)
        {
            foreach (var world in Engine.Current.WorldManager.Worlds)
                world.UpdateCustomizerAssets();
        }
    }
}