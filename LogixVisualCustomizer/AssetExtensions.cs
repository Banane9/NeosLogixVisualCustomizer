using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogixVisualCustomizer
{
    internal static class Assets
    {
        public static StaticTexture2D GetBackgroundTexture(this World world)
        {
            var key = $"Logix_CustomBackground_Texture_{world.LocalUser.UserID}";
            if (world.KeyOwner(key) is StaticTexture2D customBackgroundTexture)
                return customBackgroundTexture;

            customBackgroundTexture = world.GetCustomizerAssets().AttachComponent<StaticTexture2D>();
            customBackgroundTexture.URL.Value = LogixVisualCustomizer.BackgroundSpriteUri;
            customBackgroundTexture.WrapModeU.Value = TextureWrapMode.Clamp;
            customBackgroundTexture.WrapModeV.Value = TextureWrapMode.Clamp;
            customBackgroundTexture.FilterMode.Value = TextureFilterMode.Anisotropic;

            customBackgroundTexture.AssignKey(key);

            return customBackgroundTexture;
        }

        public static StaticTexture2D GetBackgroundTexture(this Worker worker)
        {
            return worker.World.GetBackgroundTexture();
        }

        public static StaticTexture2D GetBorderTexture(this Worker worker)
        {
            return worker.World.GetBorderTexture();
        }

        public static StaticTexture2D GetBorderTexture(this World world)
        {
            var key = $"Logix_CustomBorder_Texture_{world.LocalUser.UserID}";
            if (world.KeyOwner(key) is StaticTexture2D customBorderTexture)
                return customBorderTexture;

            customBorderTexture = world.GetCustomizerAssets().AttachComponent<StaticTexture2D>();
            customBorderTexture.URL.Value = LogixVisualCustomizer.BorderSpriteUri;
            customBorderTexture.WrapModeU.Value = TextureWrapMode.Clamp;
            customBorderTexture.WrapModeV.Value = TextureWrapMode.Clamp;
            customBorderTexture.FilterMode.Value = TextureFilterMode.Anisotropic;

            customBorderTexture.AssignKey(key);

            return customBorderTexture;
        }

        public static Slot GetCustomizerAssets(this Worker worker)
        {
            return worker.World.GetCustomizerAssets();
        }

        public static Slot GetCustomizerAssets(this World world)
        {
            return world.AssetsSlot.FindOrAdd("LogixCustomizerAssets");
        }

        public static SpriteProvider GetFullInputBackgroundProvider(this Worker worker)
        {
            return worker.World.GetFullInputBackgroundProvider();
        }

        public static SpriteProvider GetFullInputBackgroundProvider(this World world)
        {
            const string key = "Logix_FullInputBackground_SpriteProvider";
            if (world.KeyOwner(key) is SpriteProvider backgroundSprite)
                return backgroundSprite;

            backgroundSprite = world.GetCustomizerAssets().AttachComponent<SpriteProvider>();

            var textureOverride = backgroundSprite.Texture.GetUserOverride(true);
            textureOverride.CreateOverrideOnWrite.Value = true;
            textureOverride.Default.Target = world.GetSolidBackgroundTexture();
            textureOverride.SetOverride(world.LocalUser, world.GetBackgroundTexture());

            var rectOverride = backgroundSprite.Rect.GetUserOverride(true);
            rectOverride.CreateOverrideOnWrite.Value = true;
            rectOverride.SetOverride(world.LocalUser, LogixVisualCustomizer.FullBackgroundRect);

            var bordersOverride = backgroundSprite.Borders.GetUserOverride(true);
            bordersOverride.CreateOverrideOnWrite.Value = true;
            bordersOverride.SetOverride(world.LocalUser, LogixVisualCustomizer.FullBackgroundBorders);

            var scaleOverride = backgroundSprite.Scale.GetUserOverride(true);
            scaleOverride.CreateOverrideOnWrite.Value = true;
            scaleOverride.SetOverride(world.LocalUser, LogixVisualCustomizer.InputBackgroundScale);

            backgroundSprite.AssignKey(key);

            return backgroundSprite;
        }

        public static SpriteProvider GetFullInputBorderProvider(this Worker worker)
        {
            return worker.World.GetFullInputBorderProvider();
        }

        public static SpriteProvider GetFullInputBorderProvider(this World world)
        {
            const string key = "Logix_FullInputBorder_SpriteProvider";
            if (world.KeyOwner(key) is SpriteProvider borderSprite)
                return borderSprite;

            borderSprite = world.GetCustomizerAssets().AttachComponent<SpriteProvider>();

            var textureOverride = borderSprite.Texture.GetUserOverride(true);
            textureOverride.CreateOverrideOnWrite.Value = true;
            textureOverride.Default.Target = world.GetHiddenBorderTexture();
            textureOverride.SetOverride(world.LocalUser, world.GetBorderTexture());

            var rectOverride = borderSprite.Rect.GetUserOverride(true);
            rectOverride.CreateOverrideOnWrite.Value = true;
            rectOverride.SetOverride(world.LocalUser, LogixVisualCustomizer.FullBorderRect);

            var bordersOverride = borderSprite.Borders.GetUserOverride(true);
            bordersOverride.CreateOverrideOnWrite.Value = true;
            bordersOverride.SetOverride(world.LocalUser, LogixVisualCustomizer.FullBorderBorders);

            var scaleOverride = borderSprite.Scale.GetUserOverride(true);
            scaleOverride.CreateOverrideOnWrite.Value = true;
            scaleOverride.SetOverride(world.LocalUser, LogixVisualCustomizer.InputBorderScale);

            borderSprite.AssignKey(key);

            return borderSprite;
        }

        public static SolidColorTexture GetHiddenBorderTexture(this Worker worker)
        {
            return worker.World.GetHiddenBorderTexture();
        }

        public static SolidColorTexture GetHiddenBorderTexture(this World world)
        {
            const string key = "Logix_HiddenBorder_Texture";
            if (world.KeyOwner(key) is SolidColorTexture hiddenBorderTexture)
                return hiddenBorderTexture;

            hiddenBorderTexture = world.GetCustomizerAssets().AttachComponent<SolidColorTexture>();
            hiddenBorderTexture.Color.Value = new color(1, 0);

            hiddenBorderTexture.AssignKey(key);

            return hiddenBorderTexture;
        }

        public static SpriteProvider GetLeftInputBackgroundProvider(this Worker worker)
        {
            return worker.World.GetLeftInputBackgroundProvider();
        }

        public static SpriteProvider GetLeftInputBackgroundProvider(this World world)
        {
            const string key = "Logix_LeftInputBackground_SpriteProvider";
            if (world.KeyOwner(key) is SpriteProvider backgroundSprite)
                return backgroundSprite;

            backgroundSprite = world.GetCustomizerAssets().AttachComponent<SpriteProvider>();

            var textureOverride = backgroundSprite.Texture.GetUserOverride(true);
            textureOverride.CreateOverrideOnWrite.Value = true;
            textureOverride.Default.Target = world.GetSolidBackgroundTexture();
            textureOverride.SetOverride(world.LocalUser, world.GetBackgroundTexture());

            var rectOverride = backgroundSprite.Rect.GetUserOverride(true);
            rectOverride.CreateOverrideOnWrite.Value = true;
            rectOverride.SetOverride(world.LocalUser, LogixVisualCustomizer.LeftBackgroundRect);

            var bordersOverride = backgroundSprite.Borders.GetUserOverride(true);
            bordersOverride.CreateOverrideOnWrite.Value = true;
            bordersOverride.SetOverride(world.LocalUser, LogixVisualCustomizer.LeftBackgroundBorders);

            var scaleOverride = backgroundSprite.Scale.GetUserOverride(true);
            scaleOverride.CreateOverrideOnWrite.Value = true;
            scaleOverride.SetOverride(world.LocalUser, LogixVisualCustomizer.InputBackgroundScale);

            backgroundSprite.AssignKey(key);

            return backgroundSprite;
        }

        public static SpriteProvider GetLeftInputBorderProvider(this Worker worker)
        {
            return worker.World.GetLeftInputBorderProvider();
        }

        public static SpriteProvider GetLeftInputBorderProvider(this World world)
        {
            const string key = "Logix_LeftInputBorder_SpriteProvider";
            if (world.KeyOwner(key) is SpriteProvider borderSprite)
                return borderSprite;

            borderSprite = world.GetCustomizerAssets().AttachComponent<SpriteProvider>();

            var textureOverride = borderSprite.Texture.GetUserOverride(true);
            textureOverride.CreateOverrideOnWrite.Value = true;
            textureOverride.Default.Target = world.GetHiddenBorderTexture();
            textureOverride.SetOverride(world.LocalUser, world.GetBorderTexture());

            var rectOverride = borderSprite.Rect.GetUserOverride(true);
            rectOverride.CreateOverrideOnWrite.Value = true;
            rectOverride.SetOverride(world.LocalUser, LogixVisualCustomizer.LeftBorderRect);

            var bordersOverride = borderSprite.Borders.GetUserOverride(true);
            bordersOverride.CreateOverrideOnWrite.Value = true;
            bordersOverride.SetOverride(world.LocalUser, LogixVisualCustomizer.LeftBorderBorders);

            var scaleOverride = borderSprite.Scale.GetUserOverride(true);
            scaleOverride.CreateOverrideOnWrite.Value = true;
            scaleOverride.SetOverride(world.LocalUser, LogixVisualCustomizer.InputBorderScale);

            borderSprite.AssignKey(key);

            return borderSprite;
        }

        public static SpriteProvider GetNodeBackgroundProvider(this Worker worker)
        {
            return worker.World.GetNodeBackgroundProvider();
        }

        public static SpriteProvider GetNodeBackgroundProvider(this World world)
        {
            const string key = "Logix_NodeBackground_SpriteProvider";
            if (world.KeyOwner(key) is SpriteProvider backgroundSprite)
                return backgroundSprite;

            backgroundSprite = world.GetCustomizerAssets().AttachComponent<SpriteProvider>();

            var textureOverride = backgroundSprite.Texture.GetUserOverride(true);
            textureOverride.CreateOverrideOnWrite.Value = true;
            textureOverride.Default.Target = world.GetSolidBackgroundTexture();
            textureOverride.SetOverride(world.LocalUser, world.GetBackgroundTexture());

            var rectOverride = backgroundSprite.Rect.GetUserOverride(true);
            rectOverride.CreateOverrideOnWrite.Value = true;
            rectOverride.SetOverride(world.LocalUser, LogixVisualCustomizer.FullBackgroundRect);

            var bordersOverride = backgroundSprite.Borders.GetUserOverride(true);
            bordersOverride.CreateOverrideOnWrite.Value = true;
            bordersOverride.SetOverride(world.LocalUser, LogixVisualCustomizer.FullBackgroundBorders);

            var scaleOverride = backgroundSprite.Scale.GetUserOverride(true);
            scaleOverride.CreateOverrideOnWrite.Value = true;
            scaleOverride.SetOverride(world.LocalUser, LogixVisualCustomizer.NodeBackgroundScale);

            backgroundSprite.AssignKey(key);

            return backgroundSprite;
        }

        public static SpriteProvider GetNodeBorderProvider(this Worker worker)
        {
            return worker.World.GetNodeBorderProvider();
        }

        public static SpriteProvider GetNodeBorderProvider(this World world)
        {
            const string key = "Logix_NodeBorder_SpriteProvider";
            if (world.KeyOwner(key) is SpriteProvider borderSprite)
                return borderSprite;

            borderSprite = world.GetCustomizerAssets().AttachComponent<SpriteProvider>();

            var textureOverride = borderSprite.Texture.GetUserOverride(true);
            textureOverride.CreateOverrideOnWrite.Value = true;
            textureOverride.Default.Target = world.GetHiddenBorderTexture();
            textureOverride.SetOverride(world.LocalUser, world.GetBorderTexture());

            var rectOverride = borderSprite.Rect.GetUserOverride(true);
            rectOverride.CreateOverrideOnWrite.Value = true;
            rectOverride.SetOverride(world.LocalUser, LogixVisualCustomizer.FullBorderRect);

            var bordersOverride = borderSprite.Borders.GetUserOverride(true);
            bordersOverride.CreateOverrideOnWrite.Value = true;
            bordersOverride.SetOverride(world.LocalUser, LogixVisualCustomizer.FullBorderBorders);

            var scaleOverride = borderSprite.Scale.GetUserOverride(true);
            scaleOverride.CreateOverrideOnWrite.Value = true;
            scaleOverride.SetOverride(world.LocalUser, LogixVisualCustomizer.NodeBorderScale);

            borderSprite.AssignKey(key);

            return borderSprite;
        }

        public static SolidColorTexture GetSolidBackgroundTexture(this Worker worker)
        {
            return worker.World.GetSolidBackgroundTexture();
        }

        public static SolidColorTexture GetSolidBackgroundTexture(this World world)
        {
            const string key = "Logix_DefaultBackground_Texture";
            if (world.KeyOwner(key) is SolidColorTexture backgroundTexture)
                return backgroundTexture;

            backgroundTexture = world.GetCustomizerAssets().AttachComponent<SolidColorTexture>();
            backgroundTexture.Color.Value = LogixNode.DEFAULT_NODE_BACKGROUND;

            backgroundTexture.AssignKey(key);

            return backgroundTexture;
        }

        public static void UpdateCustomizerAssets(this Worker worker)
        {
            worker.World.UpdateCustomizerAssets();
        }

        public static void UpdateCustomizerAssets(this World world)
        {
            IAssetProvider<ITexture2D> borderTexture = world.GetHiddenBorderTexture();

            if (LogixVisualCustomizer.UseBorder)
            {
                var border = world.GetBorderTexture();
                border.URL.Value = LogixVisualCustomizer.BorderSpriteUri;
                borderTexture = border;
            }

            var nodeBorderSprite = world.GetNodeBorderProvider();
            nodeBorderSprite.Texture.Target = borderTexture;
            nodeBorderSprite.Rect.Value = LogixVisualCustomizer.FullBorderRect;
            nodeBorderSprite.Borders.Value = LogixVisualCustomizer.FullBorderBorders;
            nodeBorderSprite.Scale.Value = LogixVisualCustomizer.NodeBorderScale;

            var fullBorderSprite = world.GetFullInputBorderProvider();
            fullBorderSprite.Texture.Target = borderTexture;
            fullBorderSprite.Rect.Value = LogixVisualCustomizer.FullBorderRect;
            fullBorderSprite.Borders.Value = LogixVisualCustomizer.FullBorderBorders;
            fullBorderSprite.Scale.Value = LogixVisualCustomizer.InputBorderScale;

            var leftBordersSprite = world.GetLeftInputBorderProvider();
            leftBordersSprite.Texture.Target = borderTexture;
            leftBordersSprite.Rect.Value = LogixVisualCustomizer.LeftBorderRect;
            leftBordersSprite.Borders.Value = LogixVisualCustomizer.LeftBorderBorders;
            leftBordersSprite.Scale.Value = LogixVisualCustomizer.NodeBorderScale;

            IAssetProvider<ITexture2D> backgroundTexture = world.GetSolidBackgroundTexture();

            if (LogixVisualCustomizer.UseBackground)
            {
                var background = world.GetBackgroundTexture();
                background.URL.Value = LogixVisualCustomizer.BackgroundSpriteUri;
                backgroundTexture = background;
            }

            var nodeBackgroundSprite = world.GetNodeBackgroundProvider();
            nodeBackgroundSprite.Texture.Target = backgroundTexture;
            nodeBackgroundSprite.Rect.Value = LogixVisualCustomizer.FullBackgroundRect;
            nodeBackgroundSprite.Borders.Value = LogixVisualCustomizer.FullBackgroundBorders;
            nodeBackgroundSprite.Scale.Value = LogixVisualCustomizer.NodeBackgroundScale;

            var fullBackgroundSprite = world.GetFullInputBackgroundProvider();
            fullBackgroundSprite.Texture.Target = backgroundTexture;
            fullBackgroundSprite.Rect.Value = LogixVisualCustomizer.FullBackgroundRect;
            fullBackgroundSprite.Borders.Value = LogixVisualCustomizer.FullBackgroundBorders;
            fullBackgroundSprite.Scale.Value = LogixVisualCustomizer.InputBorderScale;

            var leftBackgroundSprite = world.GetLeftInputBackgroundProvider();
            leftBackgroundSprite.Texture.Target = backgroundTexture;
            leftBackgroundSprite.Rect.Value = LogixVisualCustomizer.LeftBackgroundRect;
            leftBackgroundSprite.Borders.Value = LogixVisualCustomizer.LeftBackgroundBorders;
            leftBackgroundSprite.Scale.Value = LogixVisualCustomizer.InputBorderScale;
        }

        //private static SpriteProvider setUpSpriteProvider(string key, IAssetProvider<ITexture2D> defaultTexture, IAssetProvider<ITexture2D> localTexture, Rect localRect, float4 localBorders, float localScale)
        //{
        //}
    }
}