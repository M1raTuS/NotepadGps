using Acr.UserDialogs;
using NotepadGps.Resource;
using NotepadGps.Services.Autorization;
using NotepadGps.Services.Profile;
using NotepadGps.View;
using Prism.Navigation;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotepadGps.ViewModel
{
    public class SignInViewModel : BaseViewModel
    {
        private readonly IAutorizationService _autorization;
        private readonly IProfileService _profileService;

        public SignInViewModel(
            INavigationService navigationService,
            IAutorizationService autorization,
            IProfileService profileService)
            : base(navigationService)
        {
            _autorization = autorization;
            _profileService = profileService;
        }

        #region -- Public properties --

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private bool _buttonEnabled;
        public bool ButtonEnabled
        {
            get => _buttonEnabled;
            set => SetProperty(ref _buttonEnabled, value);
        }

        public ICommand SignInCommand => new Command(OnSignInCommandAsync);
        public ICommand SignUpCommand => new Command(OnSignUpCommandAsync);

        #endregion

        #region -- Overrides --

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(Email) || args.PropertyName == nameof(Password))
            {
                var isNotEmpty = CanSignIn();

                if (isNotEmpty)
                {
                    ButtonEnabled = true;
                }
                else
                {
                    ButtonEnabled = false;
                }
            }
        }

        #endregion

        #region -- Private helpers --        

        private async void OnSignInCommandAsync()
        {
            var isAuthorized = await _autorization.TryToAuthorizeAsync(Email, Password);

            if (isAuthorized)
            {
                await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(MainListView)}");
            }
            else
            {
                UserDialogs.Instance.Alert(StringResource.MailPasswordAlert, StringResource.Alert, StringResource.Ok);
                Password = string.Empty;
            }
        }

        private async void OnSignUpCommandAsync()
        {
            await NavigationService.NavigateAsync($"{nameof(SignUpView)}");
        }

        private bool CanSignIn()
        {
            return !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Password);
        }

        #endregion
    }
}
