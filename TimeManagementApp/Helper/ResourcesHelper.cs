using Microsoft.Windows.ApplicationModel.Resources;

namespace TimeManagementApp.Helper
{
    public static class ResourcesHelper
    {
        private static readonly ResourceLoader _resourceLoader = new();
        public static string GetLocalized(this string resourceKey) => _resourceLoader.GetString(resourceKey);
    }
}
