using System.ComponentModel;

namespace TimeManagementApp.Helper
{
    public class NavigationMenuHelper : INotifyPropertyChanged
    {
        private bool _isNavigationMenuVisible = true;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsNavigationMenuVisible
        {
            get => _isNavigationMenuVisible;
            set
            {
                if (_isNavigationMenuVisible != value)
                {
                    _isNavigationMenuVisible = value;
                    OnPropertyChanged(nameof(IsNavigationMenuVisible));
                }
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
