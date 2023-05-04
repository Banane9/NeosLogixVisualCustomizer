using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX.Audio;
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
    [HarmonyPatch(typeof(AudioClipInput))]
    internal static class AudioClipInputPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(AudioClipInput.Label), MethodType.Getter)]
        private static bool LabelGetterPrefix(ref string __result)
        {
            __result = "Audio Clip";

            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(AudioClipInput.OnGenerateVisual))]
        private static bool OnGenerateVisualPrefix(AudioClipInput __instance, Slot root, AssetRef<AudioClip> ___Clip)
        {
            var builder = __instance.GenerateUI(root, 128, 72);

            var refEditor = root.AttachComponent<RefEditor>();

            builder.Panel();

            var button = builder.Button("");
            button.Customize();
            button.Pressed.Target = refEditor.RemovePressed;
            button.Released.Target = refEditor.SetReference;

            var padding = button.RectTransform;
            padding.OffsetMin.Value = new float2(4, 0);
            padding.OffsetMax.Value = new float2(-4, 0);

            refEditor._targetRef.Target = ___Clip;
            refEditor._textDrive.Target = button.Slot.GetComponentInChildren<Text>().Content;

            return false;
        }
    }
}