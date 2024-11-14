using Windows.Storage;

namespace TimeManagementApp.Helper
{
    public static class StorageHelper
    {
        private static readonly ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        public static void SaveSetting(string key, string value)
        {
            localSettings.Values[key] = value;
        }

        public static string GetSetting(string key)
        {
            return localSettings.Values[key] as string;
        }
    }
}
