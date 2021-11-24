using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace FreeMote.Tools.Viewer
{
    /// <summary>
    /// 用户注册表项
    /// </summary>
    public static class UserRegistryKey
    {
        #region properties

        private const string parentKey = "software";
        private const string ApplicationTopKey = "EmoteViewer";

        private static RegistryKey TopKey;

        #endregion

        static UserRegistryKey()
        {
            // var key = GetSubKey(ApplicationTopKey);
            // TopKey = key ?? Registry.CurrentUser.CreateSubKey($"{parentKey}\\{ApplicationTopKey}", true);
            TopKey = Registry.CurrentUser.CreateSubKey($"{parentKey}\\{ApplicationTopKey}", true);
        }

        #region Public Method

        public static void SetValue(string key, object value)
        {
            TopKey.SetValue(key, value);
        }

        public static object GetValue(string key)
        {
            return TopKey.GetValue(key);
        }

        public static void SetDefaultValue(object value)
        {
            SetValue("", value);
        }

        public static object GetDefaultValue()
        {
            return GetValue("");
        }

        public static void SetString(string key, string value)
        {
            SetValue(key, value);
        }

        public static string GetString(string key)
        {
            var value = GetValue(key);
            return value == null ? "" : value.ToString();
        }

        public static void SetInt(string key, int value)
        {
            SetValue(key, value);
        }

        public static int GetInt(string key)
        {
            var value = GetValue(key);
            return value == null ? -1 : (int) value;
        }

        public static void OnApplicationExit()
        {
            TopKey.Close();
        }

        #endregion

        #region Private Method

        private static RegistryKey GetSubKey(string subKey)
        {
            var key = Registry.CurrentUser.OpenSubKey(parentKey);
            var names = key.GetSubKeyNames();
            foreach (var name in names)
            {
                if (subKey.Equals(name))
                {
                    key.Close();
                    return Registry.CurrentUser.OpenSubKey($"{parentKey}\\{subKey}", true);
                }
            }

            return null;
        }

        #endregion
    }
}
