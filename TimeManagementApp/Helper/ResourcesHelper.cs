using Microsoft.Windows.ApplicationModel.Resources;

namespace TimeManagementApp.Helper
{
    public static class ResourcesHelper
    {
        private static readonly ResourceLoader _resourceLoader = new();
        public static string GetLocalized(this string resourceKey) => _resourceLoader.GetString(resourceKey);

        public static string GetLanguage()
        {
            string language = Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride[..2];
            return language;
        }
    }
}
