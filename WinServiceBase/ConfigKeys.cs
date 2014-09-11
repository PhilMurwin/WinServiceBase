using System;
using System.Configuration;

namespace WinServiceBase
{
    public static class ConfigKeys
    {
        #region Windows Event Logger Settings
        public static bool WindowsEventLogger
        {
            get
            {
                bool boolParse;
                Boolean.TryParse( GetConfigKey( "WindowsEventLogger" ), out boolParse );
                return boolParse;
            }
        }
        #endregion Windows Event Logger Settings

        /// <summary>
        /// Helper method for getting config keys
        /// <para>Refreshes the appSettings section of the config before retrieving a setting</para>
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        public static string GetConfigKey( string setting )
        {
            ConfigurationManager.RefreshSection( "appSettings" );
            return ConfigurationManager.AppSettings[setting];
        }
    }
}
