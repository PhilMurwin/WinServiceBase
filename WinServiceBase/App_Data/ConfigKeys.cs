using System;
using System.Configuration;

namespace WinServiceBase
{
    public static class ConfigKeys
    {
        #region Basic Time Logger Settings
        public static bool BasicTimeLogger
        {
            get
            {
                bool boolParse;
                Boolean.TryParse( GetConfigKey( "BasicTimeLogger" ), out boolParse );
                return boolParse;
            }
        }
        #endregion Basic Time Logger Settings

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
