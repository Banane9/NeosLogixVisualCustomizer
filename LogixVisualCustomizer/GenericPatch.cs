using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogixVisualCustomizer
{
    public abstract class GenericPatch<TPatch, TPatched>
        where TPatch : GenericPatch<TPatch, TPatched>
    {
        private static readonly Dictionary<string, Dictionary<Type, HashSet<Action<TPatched>>>>[] genericPatchedMethods =
            new Dictionary<string, Dictionary<Type, HashSet<Action<TPatched>>>>[2]
            {
                new Dictionary<string, Dictionary<Type, HashSet<Action<TPatched>>>>(),
                new Dictionary<string, Dictionary<Type, HashSet<Action<TPatched>>>>()
            };

        public static void CallPatches(PatchPosition position, string method, TPatched instance)
        {
            var type = instance.GetType();
            type = type.IsGenericType ? type.GetGenericTypeDefinition() : type;

            if (!genericPatchedMethods[(int)position].TryGetValue(method, out var methodPatches)
                || !methodPatches.TryGetValue(type, out var patches))
                return;

            foreach (var patch in patches)
                patch(instance);
        }

        public static void RegisterPatch(PatchPosition position, string method, Type genericDerivedType, Action<TPatched> patch)
        {
            genericDerivedType = genericDerivedType.IsGenericType ?
                genericDerivedType.GetGenericTypeDefinition() : genericDerivedType;

            if (!genericPatchedMethods[(int)position].TryGetValue(method, out var methodPatches))
            {
                methodPatches = new Dictionary<Type, HashSet<Action<TPatched>>>();
                genericPatchedMethods[(int)position].Add(method, methodPatches);
            }

            if (!methodPatches.TryGetValue(genericDerivedType, out var patches))
            {
                patches = new HashSet<Action<TPatched>>();
                methodPatches.Add(genericDerivedType, patches);
            }

            patches.Add(patch);
        }

        public static bool RemovePatch(PatchPosition position, string method, Type genericDerivedType, Action<TPatched> patch)
        {
            genericDerivedType = genericDerivedType.IsGenericType ?
                genericDerivedType.GetGenericTypeDefinition() : genericDerivedType;

            if (!genericPatchedMethods[(int)position].TryGetValue(method, out var methodPatches)
                || !methodPatches.TryGetValue(genericDerivedType, out var patches))
                return false;

            if (!patches.Remove(patch))
                return false;

            if (patches.Count == 0)
                methodPatches.Remove(genericDerivedType);

            return true;
        }

        public static bool RemovePatch(Action<TPatched> patch)
        {
            var flag = false;

            foreach (var methodPositionPatches in genericPatchedMethods)
            {
                foreach (var methodPatches in methodPositionPatches)
                {
                    foreach (var patches in methodPatches.Value)
                    {
                        flag = true;
                        patches.Value.Remove(patch);
                    }

                    foreach (var emptyType in methodPatches.Value.Where(kv => kv.Value.Count == 0).Select(kv => kv.Key))
                        methodPatches.Value.Remove(emptyType);
                }
            }

            return flag;
        }
    }

    public enum PatchPosition
    {
        Prefix = 0,
        Postfix = 1
    }
}