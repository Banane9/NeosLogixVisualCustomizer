using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogixVisualCustomizer
{
    [HarmonyPatch(typeof(LogixHelper))]
    internal static class LogixHelperPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(LogixHelper.GetConnectorSprite))]
        private static bool GetConnectorSpritePrefix(World world, int dimensions, bool isOutput, bool isImpulse, ref SpriteProvider __result)
        {
            string key = $"LogixCustomizer_ConnectorSprite_{(isImpulse ? "Impulse" : "Value")}_{dimensions}_{(isOutput ? "Output" : "Input")}";

            __result = world.GetSharedComponentOrCreate(key, delegate (SpriteProvider sprite)
            {
                sprite.Texture.Target = LogixHelper.GetConnectorTexture(world, dimensions, isOutput, isImpulse);

                sprite.Rect.Value = isImpulse || isOutput ? new Rect(0f, 0f, 1f, 1f) : new Rect(1, 0, 1, 1);
            }, 1, false, false, world.GetCustomizerAssets);

            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(LogixHelper.GetConnectorTexture))]
        private static bool GetConnectorTexturePrefix(World world, int dimensions, bool isOutput, bool isImpulse, ref StaticTexture2D __result)
        {
            var key = $"LogixCustomizer_ConnectorTexture_{(isImpulse ? "Impulse" : "Value")}_{dimensions}_{(isOutput ? "Output" : "Input")}";

            __result = world.GetSharedComponentOrCreate(key, delegate (StaticTexture2D tex)
            {
                tex.URL.Value = getConnectorUri(dimensions, isOutput, isImpulse);
                tex.FilterMode.Value = TextureFilterMode.Anisotropic;

                if (isImpulse)
                {
                    tex.WrapModeU.Value = TextureWrapMode.Clamp;
                    tex.WrapModeV.Value = TextureWrapMode.Clamp;
                }
                else
                {
                    tex.WrapModeU.Value = TextureWrapMode.MirrorOnce;
                    tex.WrapModeV.Value = TextureWrapMode.Clamp;
                }
            }, 1, false, false, world.GetCustomizerAssets);

            return false;
        }

        private static Uri getConnectorUri(int dimensions, bool isOutput, bool isImpulse)
        {
            if (isImpulse)
                return isOutput ? NeosAssets.Graphics.LogiX.Connectors.ImpulseOut : NeosAssets.Graphics.LogiX.Connectors.ImpulseIn;

            switch (dimensions)
            {
                case 1:
                    return NeosAssets.Graphics.LogiX.Connectors.Value1D;

                case 2:
                    return NeosAssets.Graphics.LogiX.Connectors.Value2D;

                case 3:
                    return NeosAssets.Graphics.LogiX.Connectors.Value3D;

                case 4:
                    return NeosAssets.Graphics.LogiX.Connectors.Value4D;

                default:
                    throw new Exception("Invalid number of dimensions for value type");
            }
        }
    }
}