using NotepadGps.Enum;
using NotepadGps.Resource.Style;
using NotepadGps.Services.Settings;
using Prism.Navigation;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace NotepadGps.ViewModel
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly ISettingsService _settingsService;

        public SettingsViewModel(
            INavigationService navigationService,
            ISettingsService settingsService)
            : base(navigationService)
        {
            _settingsService = settingsService;
        }

        private bool _isSwitchToggled;
        public bool IsSwitchToggled
        {
            get 
            {
                if (_settingsService.SelectedTheme == 1)
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

        void SetTheme(bool status)
        {
            if (status)
            {
                _settingsService.SelectedTheme = 1;
            }
            else
            {
                _settingsService.SelectedTheme = 0;
            }  
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(IsSwitchToggled))
            {
                _settingsService.LoadTheme();
            }
        }
    }
}
