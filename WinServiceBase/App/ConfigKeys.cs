using System.Configuration;

namespace WinServiceBase.App
{
    public static class ConfigKeys
    {
        #region Basic Time Logger Settings
        public static bool BasicTimeLogger => bool.Parse( GetConfigKey( "BasicTimeLogger" ) );
        public static int BasicTimeLoggerFrequency => int.Parse( GetConfigKey( "BasicTimeLoggerFrequency" ) );
        #endregion Basic Time Logger Settings

        #region DelayedProcess Settings
        public static bool DelayedProcess => bool.Parse(GetConfigKey("DelayedProcess"));
        public static int DelayedProcessFrequency => int.Parse(GetConfigKey("DelayedProcessFrequency"));
        #endregion DelayedProcess Settings

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
