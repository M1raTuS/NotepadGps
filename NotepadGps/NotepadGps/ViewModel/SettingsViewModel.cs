using NotepadGps.Services.Theme;
using Prism.Navigation;
using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotepadGps.ViewModel
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly IThemeService _themeService;

        public SettingsViewModel(
            INavigationService navigationService,
            IThemeService themeService)
            : base(navigationService)
        {
            _themeService = themeService;
        }

        #region -- Public properties --

        private bool _isSwitchToggled;
        public bool IsSwitchToggled
        {
            get
            {
                if (_themeService.SelectedTheme == 1)
                {
                    return true;
                }

                return _isSwitchToggled;
            }

            set
            {
                SetTheme(value);
                SetProperty(ref _isSwitchToggled, value);
            }
        }

        public ICommand BackCommand => new Command(OnBackCommand);

        #endregion

        #region -- Overrides --

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(IsSwitchToggled))
            {
                _themeService.LoadTheme();
            }
        }

        #endregion

        #region -- Private helpers --

        void SetTheme(bool status)
        {
            if (status)
            {
                _themeService.SelectedTheme = 1;
            }
            else
            {
                _themeService.SelectedTheme = 0;
            }
        }

        private void OnBackCommand()
        {
            NavigationService.GoBackAsync();
        }

        #endregion
    }
}
