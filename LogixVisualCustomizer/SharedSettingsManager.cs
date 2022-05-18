using FrooxEngine;
using HarmonyLib;
using NeosModLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogixVisualCustomizer
{
    internal static class SharedSettingsManager
    {
        public static readonly string AssetsSlotName = "SharedSettings";

        static SharedSettingsManager()
        {
            ModConfiguration.OnAnyConfigurationChanged += onAnyConfigurationChanged;
        }

        public static void DriveFromSharedSetting<T>(this IField<T> field, ModConfigurationKey<T> configurationKey)
        {
            field.World.getMultiDriver(configurationKey).Drives.Add().Target = field;
        }

        public static T GetSharedValue<T>(this World world, ModConfigurationKey<T> configurationKey, User user)
        {
            var vuo = world.getUserOverride(configurationKey);

            return vuo.getOverrides().FirstOrDefault(o => o.User.Target == user)?.Value
                ?? vuo.Default.Value;
        }

        public static T GetSharedValue<T>(this World world, ModConfigurationKey<T> configurationKey)
        {
            return world.GetSharedValue(configurationKey, world.LocalUser);
        }

        public static IEnumerable<User> GetSharingUsers<T>(this World world, ModConfigurationKey<T> configurationKey)
        {
            var vuo = world.getUserOverride(configurationKey);

            return vuo.getOverrides().Select(o => o.User.Target);
        }

        public static bool HasSharedSetting(this World world, ModConfigurationKey configurationKey)
        {
            return world.IsKeyInUse(configurationKey.getWorldKey("VUO"));
        }

        private static T createSharedComponent<T>(this World world, string key) where T : Component, new()
        {
            var component = world.AssetsSlot.FindOrAdd(AssetsSlotName).AttachComponent<T>();
            component.AssignKey(key);

            return component;
        }

        private static ValueMultiDriver<T> getMultiDriver<T>(this World world, ModConfigurationKey<T> configurationKey)
        {
            var key = configurationKey.getWorldKey("MultiDriver");

            if (!(world.KeyOwner(key) is ValueMultiDriver<T> multiDriver))
                multiDriver = world.createSharedComponent<ValueMultiDriver<T>>(key);

            world.getUserOverride(configurationKey).Target.Target = multiDriver.Value;
            multiDriver.trimDriveList();

            return multiDriver;
        }

        private static IEnumerable<ValueUserOverride<T>.Override> getOverrides<T>(this ValueUserOverride<T> vuo)
        {
            return Traverse.Create(vuo).Field<SyncBag<ValueUserOverride<T>.Override>>("_overrides").Value.Values;
        }

        private static ValueUserOverride<T> getUserOverride<T>(this World world, ModConfigurationKey<T> configurationKey)
        {
            var key = configurationKey.getWorldKey("VUO");

            if (!(world.KeyOwner(key) is ValueUserOverride<T> vuo))
            {
                vuo = world.createSharedComponent<ValueUserOverride<T>>(key);

                configurationKey.TryComputeDefaultTyped(out var defaultValue);
                vuo.Default.Value = defaultValue;
            }

            vuo.CreateOverrideOnWrite.Value = true;

            return vuo;
        }

        private static string getWorldKey(this ModConfigurationKey configurationKey, string type)
        {
            return $"SharedSettings_{configurationKey.ValueType().Name}_{configurationKey.Name}_{type}";
        }

        private static void onAnyConfigurationChanged(ConfigurationChangedEvent configurationChangedEvent)
        {
            foreach (var world in Engine.Current.WorldManager.Worlds.Where(world => world.HasSharedSetting(configurationChangedEvent.Key)))
            {
                world.getUserOverride(configurationChangedEvent.Key);
            }
        }

        private static void trimDriveList<T>(this ValueMultiDriver<T> multiDriver)
        {
            multiDriver.Drives.RemoveAll(drive => drive.Target == null);
        }
    }
}