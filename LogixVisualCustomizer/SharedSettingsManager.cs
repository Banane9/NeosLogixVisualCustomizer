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

        private static readonly HashSet<string> removeOverrideOnNaturalDefaultSharedKeys = new HashSet<string>();

        public static void AddDrive<T>(this ValueMultiDriver<T> multiDriver, IField<T> field)
        {
            multiDriver.Drives.Add().Target = field;
        }

        public static void AddDrive<T>(this ReferenceMultiDriver<T> multiDriver, SyncRef<T> field) where T : class, IWorldElement
        {
            multiDriver.Drives.Add().Target = field;
        }

        public static void DriveFromSharedSetting<T>(this IField<T> field, ModConfigurationKey<T> configurationKey, ModConfiguration config = null)
        {
            field.World.GetSharedMultiDriver(configurationKey, config).AddDrive(field);
        }

        public static T GetSharedDefault<T>(this ModConfigurationKey<T> configurationKey)
        {
            return DefaultManager<T>.GetDefault(configurationKey);
        }

        public static ValueMultiDriver<T> GetSharedMultiDriver<T>(this World world, ModConfigurationKey<T> configurationKey, ModConfiguration config = null)
        {
            var key = configurationKey.getWorldKey("MultiDriver");

            if (!(world.KeyOwner(key) is ValueMultiDriver<T> multiDriver))
                multiDriver = world.createSharedComponent<ValueMultiDriver<T>>(key);

            world.GetSharedUserOverride(configurationKey, config).Target.Target = multiDriver.Value;
            multiDriver.trimDriveList();

            return multiDriver;
        }

        public static ValueUserOverride<T> GetSharedUserOverride<T>(this World world, ModConfigurationKey<T> configurationKey, ModConfiguration config = null)
        {
            var key = configurationKey.getWorldKey("VUO");

            if (!(world.KeyOwner(key) is ValueUserOverride<T> vuo))
            {
                vuo = world.createSharedComponent<ValueUserOverride<T>>(key);

                vuo.Default.Value = DefaultManager<T>.GetDefault(configurationKey);
                vuo.Default.OnValueChange += field => DefaultManager<T>.SetDefault(configurationKey, field);
            }

            vuo.CreateOverrideOnWrite.Value = true;

            if (config != null)
            {
                var value = config.GetValue(configurationKey);

                if (Equals(value, default(T)))
                    vuo.RemoveOverride(world.LocalUser);
                else
                    vuo.SetOverride(world.LocalUser, config.GetValue(configurationKey));
            }

            return vuo;
        }

        public static T GetSharedValue<T>(this World world, ModConfigurationKey<T> configurationKey, User user)
        {
            var vuo = world.GetSharedUserOverride(configurationKey);

            return vuo.getOverrides().FirstOrDefault(o => o.User.Target == user)?.Value
                ?? vuo.Default.Value;
        }

        public static T GetSharedValue<T>(this World world, ModConfigurationKey<T> configurationKey)
        {
            return world.GetSharedValue(configurationKey, world.LocalUser);
        }

        public static IEnumerable<User> GetSharingUsers<T>(this World world, ModConfigurationKey<T> configurationKey)
        {
            var vuo = world.GetSharedUserOverride(configurationKey);

            return vuo.getOverrides().Select(o => o.User.Target);
        }

        public static bool HasSharedSetting<T>(this World world, ModConfigurationKey<T> configurationKey)
        {
            return world.IsKeyInUse(configurationKey.getWorldKey("VUO"));
        }

        public static void SetSharedDefault<T>(this ModConfigurationKey<T> configurationKey, T newDefault, bool removeOverrideOnNaturalDefault = true)
        {
            if (removeOverrideOnNaturalDefault)
                removeOverrideOnNaturalDefaultSharedKeys.Add(configurationKey.getSharedKey());

            DefaultManager<T>.SetDefault(configurationKey, newDefault);
        }

        public static void SetSharedValue<T>(this World world, ModConfigurationKey<T> configurationKey, T value)
        {
            var vuo = world.GetSharedUserOverride(configurationKey);

            if (Equals(value, default(T)) && removeOverrideOnNaturalDefaultSharedKeys.Contains(configurationKey.getSharedKey()))
                vuo.RemoveOverride(world.LocalUser);
            else
                vuo.SetOverride(world.LocalUser, value);
        }

        public static void SetSharedValue<T>(this World world, ModConfigurationKey<T> configurationKey, ModConfiguration config)
        {
            world.GetSharedUserOverride(configurationKey, config);
        }

        private static T createSharedComponent<T>(this World world, string key) where T : Component, new()
        {
            var component = world.AssetsSlot.FindOrAdd(AssetsSlotName).AttachComponent<T>();
            component.AssignKey(key);

            return component;
        }

        private static IEnumerable<ValueUserOverride<T>.Override> getOverrides<T>(this ValueUserOverride<T> vuo)
        {
            return ((SyncBag<ValueUserOverride<T>.Override>)vuo.TryGetField("_overrides")).Values;
        }

        private static string getSharedKey<T>(this ModConfigurationKey<T> configurationKey)
        {
            return $"SharedSettings_{configurationKey.ValueType().Name}_{configurationKey.Name}";
        }

        private static string getWorldKey<T>(this ModConfigurationKey<T> configurationKey, string component)
        {
            return $"{configurationKey.getSharedKey()}_{component}";
        }

        private static void trimDriveList<T>(this ValueMultiDriver<T> multiDriver)
        {
            multiDriver.Drives.RemoveAll(drive => drive.Target == null);
        }

        /// <summary>
        /// This handles the shared defaults and capsules subscribing to the any configuration changed event while being able to pass the generic parameter to the handling method.
        /// </summary>
        /// <typeparam name="T">The type of the config keys.</typeparam>
        private static class DefaultManager<T>
        {
            private static readonly Dictionary<string, T> sharedDefaults = new Dictionary<string, T>();

            static DefaultManager()
            {
                ModConfiguration.OnAnyConfigurationChanged += onAnyConfigurationChanged;
            }

            public static T GetDefault(ModConfigurationKey<T> configurationKey)
            {
                var sharedKey = configurationKey.getSharedKey();

                if (sharedDefaults.TryGetValue(sharedKey, out var value))
                {
                    return value;
                }
                else if (configurationKey.TryComputeDefaultTyped(out var keyDefault))
                {
                    sharedDefaults.Add(sharedKey, keyDefault);
                    return keyDefault;
                }
                else
                {
                    sharedDefaults.Add(sharedKey, default);
                    return default;
                }
            }

            public static bool HasDefault(ModConfigurationKey<T> configurationKey)
            {
                return sharedDefaults.ContainsKey(configurationKey.getSharedKey());
            }

            public static void SetDefault(ModConfigurationKey<T> configurationKey, T newDefault)
            {
                var sharedKey = configurationKey.getSharedKey();

                if (sharedDefaults.TryGetValue(sharedKey, out var oldDefault) && newDefault.Equals(oldDefault))
                    return;

                sharedDefaults[sharedKey] = newDefault;

                // WorldManager may be null when defaults are set in static constructor of a mod
                if (Engine.Current.WorldManager != null)
                    foreach (var world in Engine.Current.WorldManager.Worlds.Where(world => world.HasSharedSetting(configurationKey)))
                        GetSharedUserOverride(world, configurationKey).Default.Value = newDefault;
            }

            private static void onAnyConfigurationChanged(ConfigurationChangedEvent configurationChangedEvent)
            {
                // Make sure it's the right type
                // If it has a default, it's a setting that was used for sharing
                if (Engine.Current.WorldManager != null && configurationChangedEvent.Key is ModConfigurationKey<T> configurationKey && HasDefault(configurationKey))
                    foreach (var world in Engine.Current.WorldManager.Worlds.Where(world => world.HasSharedSetting(configurationKey)))
                        world.RunSynchronously(() => world.SetSharedValue(configurationKey, configurationChangedEvent.Config));
            }
        }
    }
}