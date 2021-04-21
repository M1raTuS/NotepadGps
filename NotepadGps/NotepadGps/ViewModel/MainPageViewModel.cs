using NotepadGps.Services.Autorization;
using NotepadGps.Services.Profile;
using NotepadGps.ViewModel;
using Prism.Navigation;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotepadGps.View
{
    public class MainPageViewModel : BaseViewModel
    {
        private readonly IAutorizationService _autorization;
        private readonly IProfileService _profileService;

        public MainPageViewModel(
            INavigationService navigationService,
            IAutorizationService autorization,
            IProfileService profileService)
            : base(navigationService)
        {
            _autorization = autorization;
            _profileService = profileService;
        }

        #region -- Public properties --

        public ICommand LogInCommand => new Command(OnLogInCommandAsync);
        public ICommand LogOutCommand => new Command(OnLogOutCommandAsync);

        #endregion

        #region -- Private helpers --        

        private async void OnLogInCommandAsync()
        {
            await NavigationService.NavigateAsync(nameof(SignInView));
        }

        private async void OnLogOutCommandAsync()
        {
            await NavigationService.NavigateAsync(nameof(MainPageView));
        }

        #endregion
    }
}

