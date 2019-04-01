using System.Globalization;
using System.Threading;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Sanus.Helpers
{
    public class Settings
    {
        public static string LanguageSettings
        {
            get => AppSettings.GetValueOrDefault(LANGUAGE_SETTINGS_KEY, LanguageSettingsDefault);
            set
            {
                AppSettings.AddOrUpdateValue(LANGUAGE_SETTINGS_KEY, value);
                if (!string.IsNullOrEmpty(value))
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo(value);
                    CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(value);
                }
            }
        }
        private static ISettings AppSettings => CrossSettings.Current;

        #region Setting Constants
        private const string LANGUAGE_SETTINGS_KEY = "language_settings_key";
        private static readonly string LanguageSettingsDefault = null;
        #endregion

        public static long Unit
        {
            get => AppSettings.GetValueOrDefault(UNIT_SETTINGS_KEY, UnitSettingsDefault);
            set => AppSettings.AddOrUpdateValue(UNIT_SETTINGS_KEY, value);
        }

        private const string UNIT_SETTINGS_KEY = "unit_settings_key";
        private static readonly long UnitSettingsDefault = 0;
    }
}
