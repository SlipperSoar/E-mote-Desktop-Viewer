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
        private static RegistryKey TopKey
        {
            get
            {
                var key = GetSubKey(ApplicationTopKey);
                if (key != null)
                {
                    return key;
                }
                else
                {
                    return Registry.CurrentUser.CreateSubKey($"{parentKey}\\{ApplicationTopKey}", true);
                }
            }
        }

        #endregion

        static UserRegistryKey()
        {

        }

        #region Public Method

        public static void SetString(string key, string value)
        {
            TopKey.SetValue(key, value);
            TopKey.Close();
        }

        public static string GetString(string key)
        {
            var value = TopKey.GetValue(key);
            return value == null ? "" : value.ToString();
        }

        #endregion

        #region Private Method

        private static RegistryKey GetSubKey(string subKey)
        {
            var names = Registry.CurrentUser.OpenSubKey(parentKey).GetSubKeyNames();
            foreach (var name in names)
            {
                if (subKey.Equals(name))
                {
                    return Registry.CurrentUser.OpenSubKey($"{parentKey}\\{subKey}", true);
                }
            }

            return null;
        }

        #endregion
    }
}
