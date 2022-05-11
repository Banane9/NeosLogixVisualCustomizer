using FrooxEngine;
using FrooxEngine.LogiX.Display;
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
    internal static class DisplayPatches
    {
        [HarmonyPostfix]
        private static void OnGenerateVisualPostfix(Slot root)
        {
            root.ForeachComponentInChildren<Text>(VisualCustomizing.CustomizeDisplay);
        }

        [HarmonyTargetMethods]
        private static IEnumerable<MethodBase> TargetMethods()
        {
            return AccessTools.GetTypesFromAssembly(typeof(Display_Dummy).Assembly)
                .Where(t => t.Namespace == "FrooxEngine.LogiX.Display")
                .Select(t => t.GetMethod("OnGenerateVisual", AccessTools.allDeclared))
                .Where(m => m != null); // Dummy doesn't have one
        }
    }
}