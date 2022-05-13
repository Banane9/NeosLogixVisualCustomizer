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
    internal static class SettingOverrides
    {
        public const string InputBackgroundColor = "InputBackgroundColor";
        public const string InputBorderColor = "InputBorderColor";
        public const string NodeBackgroundColor = "NodeBackgroundColor";
        public const string NodeBorderColor = "NodeBorderColor";
        public const string TextColor = "TextColor";

        private static readonly Type colorType = typeof(color);

        public static void OverrideWith<T>(this Sync<T> field, string setting)
        {
            field.GetMultiDriver(setting).Drives.Add().ForceLink(field);
        }

        private static void ensureUserOverride<T>(this Sync<T> field, string setting)
        {
            var vuo = field.GetUserOverride(true);

            vuo.Default.Value = setting.getDefault<T>();
            vuo.CreateOverrideOnWrite.Value = true;
            vuo.SetOverride(field.World.LocalUser, setting.getValue<T>());
        }

        private static color getColor(this string setting)
        {
            switch (setting)
            {
                case InputBackgroundColor: return LogixVisualCustomizer.InputBackgroundColor;
                case InputBorderColor: return LogixVisualCustomizer.InputBorderColor;
                case NodeBackgroundColor: return LogixVisualCustomizer.NodeBackgroundColor;
                case NodeBorderColor: return LogixVisualCustomizer.NodeBorderColor;
                case TextColor: return LogixVisualCustomizer.TextColor;
                default: throw new ArgumentOutOfRangeException(nameof(setting), "Unsupported Setting Name");
            }
        }

        private static T getDefault<T>(this string setting)
        {
            if (typeof(T) == colorType)
                return (T)(object)setting.getDefaultColor();
            else
                return default;
        }

        private static color getDefaultColor(this string setting)
        {
            switch (setting)
            {
                case InputBackgroundColor: return color.White;
                case InputBorderColor: return new color(1, 0);
                case NodeBackgroundColor: return LogixNode.DEFAULT_NODE_BACKGROUND;
                case NodeBorderColor: return new color(1, 0);
                case TextColor: return color.Black;
                default: throw new ArgumentOutOfRangeException(nameof(setting), "Unsupported Setting Name");
            }
        }

        private static ValueMultiDriver<T> GetMultiDriver<T>(this Sync<T> field, string setting)
        {
            var key = $"Logix_{setting}_ValueMultiDriver";

            if (field.World.KeyOwner(key) is ValueMultiDriver<T> multiDriver)
            {
                multiDriver.Value.ensureUserOverride(setting);
                multiDriver.trimDriveList();

                return multiDriver;
            }

            multiDriver = field.Worker.GetCustomizerAssets().AttachComponent<ValueMultiDriver<T>>();
            multiDriver.Value.ensureUserOverride(setting);

            multiDriver.AssignKey(key);

            return multiDriver;
        }

        private static T getValue<T>(this string setting)
        {
            if (typeof(T) == colorType)
                return (T)(object)setting.getColor();
            else
                return default;
        }

        private static void trimDriveList<T>(this ValueMultiDriver<T> multiDriver)
        {
            multiDriver.Drives.RemoveAll(drive => drive.Target == null);
        }
    }
}