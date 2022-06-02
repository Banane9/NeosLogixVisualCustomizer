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
        [HarmonyPatch("Label", MethodType.Getter)]
        private static bool LabelGetterPrefix(ref string __result)
        {
            __result = "Audio Clip";

            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch("OnGenerateVisual")]
        private static bool OnGenerateVisualPrefix(AudioClipInput __instance, Slot root, AssetRef<AudioClip> ___Clip)
        {
            var builder = Traverse.Create(__instance).Method("GenerateUI", root, 128, 72).GetValue<UIBuilder>();

            var refEditor = root.AttachComponent<RefEditor>();
            var editor = Traverse.Create(refEditor);

            builder.Panel();

            var button = builder.Button("");
            button.Customize();
            button.Pressed.Target = AccessTools.MethodDelegate<ButtonEventHandler>(LogixVisualCustomizer.RefEditorRemovePressed, refEditor);
            button.Released.Target = AccessTools.MethodDelegate<ButtonEventHandler>(LogixVisualCustomizer.RefEditorSetReference, refEditor);

            var padding = button.RectTransform;
            padding.OffsetMin.Value = new float2(4, 0);
            padding.OffsetMax.Value = new float2(-4, 0);

            editor.Field<RelayRef<ISyncRef>>("_targetRef").Value.Target = ___Clip;
            editor.Field<FieldDrive<string>>("_textDrive").Value.Target = button.Slot.GetComponentInChildren<Text>().Content;

            return false;
        }
    }
}