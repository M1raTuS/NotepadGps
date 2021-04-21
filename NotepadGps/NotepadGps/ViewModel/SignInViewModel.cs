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

        private string _passwordError;
        public string PasswordError
        {
            get => _passwordError;
            set => SetProperty(ref _passwordError, value);
        }

        private string _emailError;
        public string EmailError
        {
            get => _emailError;
            set => SetProperty(ref _emailError, value);
        }

        private bool _isEmailErrorVisible;
        public bool IsEmailErrorVisible
        {
            get => _isEmailErrorVisible;
            set => SetProperty(ref _isEmailErrorVisible, value);
        }

        private bool _isPasswordErrorVisible;
        public bool IsPasswordErrorVisible
        {
            get => _isPasswordErrorVisible;
            set => SetProperty(ref _isPasswordErrorVisible, value);
        }

        public ICommand LogInCommand => new Command(OnLogInCommandAsync);

        #endregion

        #region -- Private helpers --        

        private async void OnLogInCommandAsync()
        {
            IsEmailErrorVisible = false;
            IsPasswordErrorVisible = false;

            var isAuthorized = await _autorization.TryToAuthorizeAsync(Email, Password);

            if (isAuthorized)
            {
                await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(MainListView)}");
            }
            else
            {
                if (!_autorization.IsMailTrue())
                {
                    EmailError = StringResource.SignInMailAlert;
                    IsEmailErrorVisible = true;
                }

                if (!_autorization.IsPasswordTrue())
                {
                    PasswordError = StringResource.SignInPasswordAlert;
                    IsPasswordErrorVisible = true;
                }

                Password = string.Empty;
            }
        }

        private async void OnSignUpCommandAsync()
        {
            await NavigationService.NavigateAsync(nameof(SignUpView));
        }

        private bool CanSignIn()
        {
            return !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Password);//todo: test 
        }

        #endregion
    }
}
