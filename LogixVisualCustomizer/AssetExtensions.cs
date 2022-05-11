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
            const string key = "Logix_CustomBackground_Texture";

            return world.getOrCreateTexture(key, LogixVisualCustomizer.BackgroundSpriteUri);
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
            const string key = "Logix_CustomBorder_Texture";

            return world.getOrCreateTexture(key, LogixVisualCustomizer.BorderSpriteUri);
        }

        public static SpriteProvider GetBottomInputBackgroundProvider(this Worker worker)
        {
            return worker.World.GetBottomInputBackgroundProvider();
        }

        public static SpriteProvider GetBottomInputBackgroundProvider(this World world)
        {
            const string key = "Logix_BottomInputBackground_SpriteProvider";

            return world.getOrCreateSpriteProvider(key,
                world.GetSolidBackgroundTexture(), world.GetBackgroundTexture(),
                LogixVisualCustomizer.BottomBackgroundRect,
                LogixVisualCustomizer.BottomBackgroundBorders,
                LogixVisualCustomizer.InputBackgroundScale);
        }

        public static SpriteProvider GetBottomInputBorderProvider(this Worker worker)
        {
            return worker.World.GetBottomInputBorderProvider();
        }

        public static SpriteProvider GetBottomInputBorderProvider(this World world)
        {
            const string key = "Logix_BottomInputBorder_SpriteProvider";

            return world.getOrCreateSpriteProvider(key,
                world.GetHiddenBorderTexture(), world.GetBorderTexture(),
                LogixVisualCustomizer.BottomBorderRect,
                LogixVisualCustomizer.BottomBorderBorders,
                LogixVisualCustomizer.InputBorderScale);
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

            return world.getOrCreateSpriteProvider(key,
                world.GetSolidBackgroundTexture(), world.GetBackgroundTexture(),
                LogixVisualCustomizer.FullBackgroundRect,
                LogixVisualCustomizer.FullBackgroundBorders,
                LogixVisualCustomizer.InputBackgroundScale);
        }

        public static SpriteProvider GetFullInputBorderProvider(this Worker worker)
        {
            return worker.World.GetFullInputBorderProvider();
        }

        public static SpriteProvider GetFullInputBorderProvider(this World world)
        {
            const string key = "Logix_FullInputBorder_SpriteProvider";

            return world.getOrCreateSpriteProvider(key,
                world.GetHiddenBorderTexture(), world.GetBorderTexture(),
                LogixVisualCustomizer.FullBorderRect,
                LogixVisualCustomizer.FullBorderBorders,
                LogixVisualCustomizer.InputBorderScale);
        }

        public static SolidColorTexture GetHiddenBorderTexture(this Worker worker)
        {
            return worker.World.GetHiddenBorderTexture();
        }

        public static SolidColorTexture GetHiddenBorderTexture(this World world)
        {
            const string key = "Logix_HiddenBorder_Texture";

            return world.getOrCreateSolidColorTexture(key, new color(1, 0));
        }

        public static SpriteProvider GetHorizontalMiddleInputBackgroundProvider(this World world)
        {
            const string key = "Logix_HorizontalMiddleInputBackground_SpriteProvider";

            return world.getOrCreateSpriteProvider(key,
                world.GetSolidBackgroundTexture(), world.GetBackgroundTexture(),
                LogixVisualCustomizer.HorizontalMiddleBackgroundRect,
                new float4(0),
                LogixVisualCustomizer.InputBackgroundScale);
        }

        public static SpriteProvider GetHorizontalMiddleInputBackgroundProvider(this Worker worker)
        {
            return worker.World.GetHorizontalMiddleInputBackgroundProvider();
        }

        public static SpriteProvider GetHorizontalMiddleInputBorderProvider(this Worker worker)
        {
            return worker.World.GetHorizontalMiddleInputBorderProvider();
        }

        public static SpriteProvider GetHorizontalMiddleInputBorderProvider(this World world)
        {
            const string key = "Logix_HorizontalMiddleInputBorder_SpriteProvider";

            return world.getOrCreateSpriteProvider(key,
                world.GetHiddenBorderTexture(), world.GetBorderTexture(),
                LogixVisualCustomizer.HorizontalMiddleBorderRect,
                new float4(0),
                LogixVisualCustomizer.InputBorderScale);
        }

        public static void GetInputProviders(this Worker worker, int index, int inputs, out SpriteProvider inputBackground, out SpriteProvider inputBorder)
        {
            worker.World.GetInputProviders(index, inputs, out inputBackground, out inputBorder);
        }

        public static void GetInputProviders(this World world, int index, int total, out SpriteProvider inputBackground, out SpriteProvider inputBorder)
        {
            if (index == 0)
            {
                if (total == 1)
                {
                    inputBackground = world.GetLeftInputBackgroundProvider();
                    inputBorder = world.GetLeftInputBorderProvider();
                }
                else
                {
                    inputBackground = world.GetTopInputBackgroundProvider();
                    inputBorder = world.GetTopInputBorderProvider();
                }
            }
            else if (index == total - 1)
            {
                inputBackground = world.GetBottomInputBackgroundProvider();
                inputBorder = world.GetBottomInputBorderProvider();
            }
            else
            {
                inputBackground = world.GetVerticleMiddleInputBackgroundProvider();
                inputBorder = world.GetVerticleMiddleInputBorderProvider();
            }
        }

        public static SpriteProvider GetLeftInputBackgroundProvider(this Worker worker)
        {
            return worker.World.GetLeftInputBackgroundProvider();
        }

        public static SpriteProvider GetLeftInputBackgroundProvider(this World world)
        {
            const string key = "Logix_LeftInputBackground_SpriteProvider";

            return world.getOrCreateSpriteProvider(key,
                world.GetSolidBackgroundTexture(), world.GetBackgroundTexture(),
                LogixVisualCustomizer.LeftBackgroundRect,
                LogixVisualCustomizer.LeftBackgroundBorders,
                LogixVisualCustomizer.InputBackgroundScale);
        }

        public static SpriteProvider GetLeftInputBorderProvider(this Worker worker)
        {
            return worker.World.GetLeftInputBorderProvider();
        }

        public static SpriteProvider GetLeftInputBorderProvider(this World world)
        {
            const string key = "Logix_LeftInputBorder_SpriteProvider";

            return world.getOrCreateSpriteProvider(key,
                world.GetHiddenBorderTexture(), world.GetBorderTexture(),
                LogixVisualCustomizer.LeftBorderRect,
                LogixVisualCustomizer.LeftBorderBorders,
                LogixVisualCustomizer.InputBorderScale);
        }

        public static SpriteProvider GetNodeBackgroundProvider(this Worker worker)
        {
            return worker.World.GetNodeBackgroundProvider();
        }

        public static SpriteProvider GetNodeBackgroundProvider(this World world)
        {
            const string key = "Logix_NodeBackground_SpriteProvider";

            return world.getOrCreateSpriteProvider(key,
                world.GetSolidBackgroundTexture(), world.GetBackgroundTexture(),
                LogixVisualCustomizer.FullBackgroundRect,
                LogixVisualCustomizer.FullBackgroundBorders,
                LogixVisualCustomizer.NodeBackgroundScale);
        }

        public static SpriteProvider GetNodeBorderProvider(this Worker worker)
        {
            return worker.World.GetNodeBorderProvider();
        }

        public static SpriteProvider GetNodeBorderProvider(this World world)
        {
            const string key = "Logix_NodeBorder_SpriteProvider";

            return world.getOrCreateSpriteProvider(key,
                world.GetHiddenBorderTexture(), world.GetBorderTexture(),
                LogixVisualCustomizer.FullBorderRect,
                LogixVisualCustomizer.FullBorderBorders,
                LogixVisualCustomizer.NodeBackgroundScale);
        }

        public static SolidColorTexture GetSolidBackgroundTexture(this Worker worker)
        {
            return worker.World.GetSolidBackgroundTexture();
        }

        public static SolidColorTexture GetSolidBackgroundTexture(this World world)
        {
            const string key = "Logix_DefaultBackground_Texture";

            return world.getOrCreateSolidColorTexture(key, new color(1));
        }

        public static SpriteProvider GetTopInputBackgroundProvider(this Worker worker)
        {
            return worker.World.GetTopInputBackgroundProvider();
        }

        public static SpriteProvider GetTopInputBackgroundProvider(this World world)
        {
            const string key = "Logix_TopInputBackground_SpriteProvider";

            return world.getOrCreateSpriteProvider(key,
                world.GetSolidBackgroundTexture(), world.GetBackgroundTexture(),
                LogixVisualCustomizer.TopBackgroundRect,
                LogixVisualCustomizer.TopBackgroundBorders,
                LogixVisualCustomizer.InputBackgroundScale);
        }

        public static SpriteProvider GetTopInputBorderProvider(this Worker worker)
        {
            return worker.World.GetTopInputBorderProvider();
        }

        public static SpriteProvider GetTopInputBorderProvider(this World world)
        {
            const string key = "Logix_TopInputBorder_SpriteProvider";

            return world.getOrCreateSpriteProvider(key,
                world.GetHiddenBorderTexture(), world.GetBorderTexture(),
                LogixVisualCustomizer.TopBorderRect,
                LogixVisualCustomizer.TopBorderBorders,
                LogixVisualCustomizer.InputBorderScale);
        }

        public static SpriteProvider GetVerticleMiddleInputBackgroundProvider(this World world)
        {
            const string key = "Logix_VerticleMiddleInputBackground_SpriteProvider";

            return world.getOrCreateSpriteProvider(key,
                world.GetSolidBackgroundTexture(), world.GetBackgroundTexture(),
                LogixVisualCustomizer.VerticleMiddleBackgroundRect,
                new float4(0),
                LogixVisualCustomizer.InputBackgroundScale);
        }

        public static SpriteProvider GetVerticleMiddleInputBackgroundProvider(this Worker worker)
        {
            return worker.World.GetVerticleMiddleInputBackgroundProvider();
        }

        public static SpriteProvider GetVerticleMiddleInputBorderProvider(this Worker worker)
        {
            return worker.World.GetVerticleMiddleInputBorderProvider();
        }

        public static SpriteProvider GetVerticleMiddleInputBorderProvider(this World world)
        {
            const string key = "Logix_VerticleMiddleInputBorder_SpriteProvider";

            return world.getOrCreateSpriteProvider(key,
                world.GetHiddenBorderTexture(), world.GetBorderTexture(),
                LogixVisualCustomizer.VerticleMiddleBorderRect,
                new float4(0),
                LogixVisualCustomizer.InputBorderScale);
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

        private static SolidColorTexture getOrCreateSolidColorTexture(this World world, string key, color color)
        {
            if (world.KeyOwner(key) is SolidColorTexture texture)
                return texture;

            texture = world.GetCustomizerAssets().AttachComponent<SolidColorTexture>();
            texture.Color.Value = color;

            world.GetCustomizerAssets().AttachComponent<AssetLoader<ITexture2D>>().Asset.Target = texture;

            texture.AssignKey(key);

            return texture;
        }

        private static SpriteProvider getOrCreateSpriteProvider(this World world, string key, IAssetProvider<ITexture2D> defaultTexture, IAssetProvider<ITexture2D> localTexture, Rect localRect, float4 localBorders, float localScale)
        {
            if (world.KeyOwner(key) is SpriteProvider sprite)
                return sprite;

            sprite = world.GetCustomizerAssets().AttachComponent<SpriteProvider>();

            var textureOverride = sprite.Texture.GetUserOverride(true);
            textureOverride.CreateOverrideOnWrite.Value = true;
            textureOverride.Default.Target = defaultTexture;
            textureOverride.SetOverride(world.LocalUser, localTexture);

            var rectOverride = sprite.Rect.GetUserOverride(true);
            rectOverride.CreateOverrideOnWrite.Value = true;
            rectOverride.SetOverride(world.LocalUser, localRect);

            var bordersOverride = sprite.Borders.GetUserOverride(true);
            bordersOverride.CreateOverrideOnWrite.Value = true;
            bordersOverride.SetOverride(world.LocalUser, localBorders);

            var scaleOverride = sprite.Scale.GetUserOverride(true);
            scaleOverride.CreateOverrideOnWrite.Value = true;
            scaleOverride.SetOverride(world.LocalUser, localScale);

            sprite.AssignKey(key);

            return sprite;
        }

        private static StaticTexture2D getOrCreateTexture(this World world, string key, Uri source = null)
        {
            if (world.KeyOwner(key) is StaticTexture2D texture)
                return texture;

            texture = world.GetCustomizerAssets().AttachComponent<StaticTexture2D>();
            texture.WrapModeU.Value = TextureWrapMode.Clamp;
            texture.WrapModeV.Value = TextureWrapMode.Clamp;
            texture.FilterMode.Value = TextureFilterMode.Anisotropic;

            var urlOverride = texture.URL.GetUserOverride(true);
            urlOverride.CreateOverrideOnWrite.Value = true;
            urlOverride.SetOverride(world.LocalUser, source);

            world.GetCustomizerAssets().AttachComponent<AssetLoader<ITexture2D>>().Asset.Target = texture;

            texture.AssignKey(key);

            return texture;
        }
    }
}