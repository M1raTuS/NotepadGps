using NotepadGps.Services.Settings;
using NotepadGps.View;
using Prism.Navigation;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotepadGps.ViewModel
{
    public class MainListViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly ISettingsService _settingsService;

        public MainListViewModel(
            INavigationService navigationService,
            ISettingsService settingsService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            _settingsService = settingsService;
        }

        #region -- Private helpers --        

        public ICommand LogOutTapCommand => new Command(OnLogOutCommandAsync);

        private async void OnLogOutCommandAsync()
        {
            _settingsService.CurrentUser = -1; //TODO: to authorization service
            await _navigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(SignInView)}");
        }

        #endregion
    }
}
