using NotepadGps.Services.Settings;
using NotepadGps.View;
using Prism.Navigation;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotepadGps.ViewModel
{
    public class MainListViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly ISettingsService _settingsService;

        public MainListViewModel(INavigationService navigationService,
                                 ISettingsService settingsService)
        {
            _navigationService = navigationService;
            _settingsService = settingsService;
        }

        #region -Methods-

        public ICommand LogOutTapCommand => new Command(OnLogOutCommandAsync);


        private async void OnLogOutCommandAsync()
        {
            _settingsService.CurrentUser = -1;
            await _navigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(SignInView)}");
        }

        #endregion
    }
}
