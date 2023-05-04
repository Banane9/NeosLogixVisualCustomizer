using FrooxEngine;
using FrooxEngine.LogiX;
using HarmonyLib;
using NeosModLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogixVisualCustomizer
{
    [HarmonyPatch(typeof(ConnectionWire))]
    internal static class ConnectionWirePatches
    {
        private static User GetAllocatingUser(IWorldElement worldElement)
        {
            worldElement.ReferenceID.ExtractIDs(out var position, out var user);
            var userByAllocationID = worldElement.World.GetUserByAllocationID(user);

            if (userByAllocationID == null || position < userByAllocationID.AllocationIDStart)
                return null;

            return userByAllocationID;
        }

        [HarmonyPostfix]
        [HarmonyPatch("OnChanges")]
        private static void OnChangesPostfix(Sync<bool> ___TempWire, bool __state)
        {
            ___TempWire.Value = __state;
        }

        [HarmonyPrefix]
        [HarmonyPatch("OnChanges")]
        private static void OnChangesPrefix(ConnectionWire __instance, Sync<bool> ___TempWire, SyncRef<IWorldElement> ___SetupSource, Sync<string> ___SetupMethod, ref bool __state)
        {
            __state = ___TempWire.Value;

            var allocatingUser = GetAllocatingUser(__instance);
            var correctUser = allocatingUser == __instance.LocalUser || (allocatingUser == null && __instance.LocalUser.IsHost);

            var worldElement = __instance.InputField.Target?.Target;
            var text = (__instance.InputField.Target as ISyncDelegate)?.MethodName;

            ___TempWire.Value = !correctUser && !__state && (worldElement != ___SetupSource.Target || text != ___SetupMethod.Value || __instance.TargetSlot.Target == null) && worldElement != null;

            if (!__state && ___TempWire.Value)
                NeosMod.Msg($"Blocking wire setup attempt from: {allocatingUser.UserName}");
        }
    }
}