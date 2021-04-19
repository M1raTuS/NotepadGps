using NotepadGps.Services.Autorization;
using NotepadGps.View;
using Prism.Navigation;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotepadGps.ViewModel
{
    public class MainListViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IAutorizationService _autorizationService;

        public MainListViewModel(
            INavigationService navigationService,
            IAutorizationService autorizationService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            _autorizationService = autorizationService;
        }

        #region -- Private helpers --        

        public ICommand LogOutTapCommand => new Command(OnLogOutCommandAsync);

        private async void OnLogOutCommandAsync()
        {
            _autorizationService.Unautorize();
            await _navigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(SignInView)}");
        }

        #endregion
    }
}
