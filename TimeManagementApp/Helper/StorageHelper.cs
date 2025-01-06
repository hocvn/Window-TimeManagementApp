using Windows.Storage;

namespace TimeManagementApp.Helper
{
    /// <summary>   
    /// This class is used to save and retrieve settings from local storage.
    /// </summary>

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

        public static void RemoveSetting(string key)
        {
            localSettings.Values.Remove(key);
        }

        public static bool ContainsSetting(string key)
        {
            return localSettings.Values.ContainsKey(key);
        }

        public static void ClearAllSettings()
        {
            localSettings.Values.Clear();
        }
    }
}
