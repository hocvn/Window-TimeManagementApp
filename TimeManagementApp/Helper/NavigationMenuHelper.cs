using System;

namespace TimeManagementApp.Helper
{
    public class NavigationMenuHelper
    {
        private bool _isNavigationMenuVisible = true;

        public event EventHandler NavigationMenuVisibilityChanged;

        public bool IsNavigationMenuVisible
        {
            get => _isNavigationMenuVisible;
            set
            {
                if (_isNavigationMenuVisible != value)
                {
                    _isNavigationMenuVisible = value;
                    OnNavigationMenuVisibilityChanged();
                }
            }
        }

        protected virtual void OnNavigationMenuVisibilityChanged()
        {
            NavigationMenuVisibilityChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
