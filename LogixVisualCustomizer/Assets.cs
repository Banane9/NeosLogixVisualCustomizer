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
        public static IAssetProvider<ITexture2D> GetBackgroundTexture(this World world)
        {
            const string key = "Logix_CustomBackground_Texture";

            if (LogixVisualCustomizer.UseBackground)
                return world.getOrCreateTexture(key, LogixVisualCustomizer.BackgroundSpriteUri);
            else
                return world.GetSolidBackgroundTexture();
        }

        public static IAssetProvider<ITexture2D> GetBackgroundTexture(this Worker worker)
        {
            return worker.World.GetBackgroundTexture();
        }

        public static IAssetProvider<ITexture2D> GetBorderTexture(this Worker worker)
        {
            return worker.World.GetBorderTexture();
        }

        public static IAssetProvider<ITexture2D> GetBorderTexture(this World world)
        {
            const string key = "Logix_CustomBorder_Texture";

            if (LogixVisualCustomizer.UseBorder)
                return world.getOrCreateTexture(key, LogixVisualCustomizer.BorderSpriteUri);
            else
                return world.GetHiddenBorderTexture();
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
            const string key = "LogixCustomizerAssets";

            if (world.AssetsSlot.Find(key) is Slot slot)
                return slot;

            slot = world.AssetsSlot.AddSlot(key);
            slot.AttachComponent<AssetOptimizationBlock>();

            return slot;
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
                LogixVisualCustomizer.HorizontalMiddleBackgroundBorders,
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
                LogixVisualCustomizer.HorizontalMiddleBorderBorders,
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
                LogixVisualCustomizer.VerticalMiddleBackgroundRect,
                LogixVisualCustomizer.VerticalMiddleBackgroundBorders,
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
                LogixVisualCustomizer.VerticalMiddleBorderRect,
                LogixVisualCustomizer.VerticalMiddleBorderBorders,
                LogixVisualCustomizer.InputBorderScale);
        }

        private static void ensureSettings(this SpriteProvider sprite, IAssetProvider<ITexture2D> defaultTexture, IAssetProvider<ITexture2D> localTexture, Rect localRect, float4 localBorders, float localScale)
        {
            var textureOverride = sprite.Texture.GetUserOverride(true);
            textureOverride.CreateOverrideOnWrite.Value = true;
            textureOverride.Default.Target = defaultTexture;
            textureOverride.SetOverride(sprite.World.LocalUser, localTexture);

            var rectOverride = sprite.Rect.GetUserOverride(true);
            rectOverride.CreateOverrideOnWrite.Value = true;
            rectOverride.Default.Value = new Rect(0, 0, 1, 1);
            rectOverride.SetOverride(sprite.World.LocalUser, localRect);

            var bordersOverride = sprite.Borders.GetUserOverride(true);
            bordersOverride.CreateOverrideOnWrite.Value = true;
            bordersOverride.Default.Value = new float4(.5f, .5f, .5f, .5f);
            bordersOverride.SetOverride(sprite.World.LocalUser, localBorders);

            var scaleOverride = sprite.Scale.GetUserOverride(true);
            scaleOverride.CreateOverrideOnWrite.Value = true;
            scaleOverride.Default.Value = 1;
            scaleOverride.SetOverride(sprite.World.LocalUser, localScale);
        }

        private static void ensureSettings(this StaticTexture2D texture, Uri source)
        {
            texture.WrapModeU.Value = TextureWrapMode.Clamp;
            texture.WrapModeV.Value = TextureWrapMode.Clamp;
            texture.FilterMode.Value = TextureFilterMode.Anisotropic;

            var urlOverride = texture.URL.GetUserOverride(true);
            urlOverride.CreateOverrideOnWrite.Value = true;
            urlOverride.SetOverride(texture.World.LocalUser, source);
        }

        private static SolidColorTexture getOrCreateSolidColorTexture(this World world, string key, color color)
        {
            if (world.KeyOwner(key) is SolidColorTexture texture)
                return texture;

            texture = world.GetCustomizerAssets().AttachComponent<SolidColorTexture>();
            texture.WrapModeU.Value = TextureWrapMode.Repeat;
            texture.WrapModeV.Value = TextureWrapMode.Repeat;
            texture.FilterMode.Value = TextureFilterMode.Point;
            texture.Color.Value = color;

            world.GetCustomizerAssets().AttachComponent<AssetLoader<ITexture2D>>().Asset.Target = texture;

            texture.AssignKey(key);

            return texture;
        }

        private static SpriteProvider getOrCreateSpriteProvider(this World world, string key, IAssetProvider<ITexture2D> defaultTexture, IAssetProvider<ITexture2D> localTexture, Rect localRect, float4 localBorders, float localScale)
        {
            if (world.KeyOwner(key) is SpriteProvider sprite)
            {
                sprite.ensureSettings(defaultTexture, localTexture, localRect, localBorders, localScale);

                return sprite;
            }

            sprite = world.GetCustomizerAssets().AttachComponent<SpriteProvider>();
            sprite.ensureSettings(defaultTexture, localTexture, localRect, localBorders, localScale);

            sprite.AssignKey(key);

            return sprite;
        }

        private static StaticTexture2D getOrCreateTexture(this World world, string key, Uri source = null)
        {
            if (world.KeyOwner(key) is StaticTexture2D texture)
            {
                texture.ensureSettings(source);

                return texture;
            }

            texture = world.GetCustomizerAssets().AttachComponent<StaticTexture2D>();
            texture.ensureSettings(source);

            world.GetCustomizerAssets().AttachComponent<AssetLoader<ITexture2D>>().Asset.Target = texture;

            texture.AssignKey(key);

            return texture;
        }
    }
}