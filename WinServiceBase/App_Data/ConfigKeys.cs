using System.Configuration;

namespace WinServiceBase
{
    public static class ConfigKeys
    {
        #region Basic Time Logger Settings
        public static bool BasicTimeLogger => bool.Parse( GetConfigKey( "BasicTimeLogger" ) );

        public static int BasicTimeLoggerFrequency => int.Parse( GetConfigKey( "BasicTimeLoggerFrequency" ) );
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
