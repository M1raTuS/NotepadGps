using NotepadGps.Models;
using NotepadGps.Resource;
using NotepadGps.Services.Autentification;
using NotepadGps.Services.Profile;
using NotepadGps.Services.Validation;
using Prism.Navigation;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotepadGps.ViewModel
{
    public class SignUpPwCheckViewModel : BaseViewModel
    {
        private readonly IProfileService _profileService;
        private readonly IAutentificationService _autentificationService;

        public SignUpPwCheckViewModel(
            INavigationService navigationService,
            IProfileService profileService,
            IAutentificationService autentificationService)
            : base(navigationService)
        {
            _profileService = profileService;
            _autentificationService = autentificationService;
        }

        #region -- Public properties --

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }
        
        private string _passwordError;
        public string PasswordError
        {
            get => _passwordError;
            set => SetProperty(ref _passwordError, value);
        }

        private string _confirmPasswordError;
        public string ConfirmPasswordError
        {
            get => _confirmPasswordError;
            set => SetProperty(ref _confirmPasswordError, value);
        }

        private bool _isPasswordErrorVisible;
        public bool IsPasswordErrorVisible
        {
            get => _isPasswordErrorVisible;
            set => SetProperty(ref _isPasswordErrorVisible, value);
        }

        private bool _isConfirmPasswordErrorVisible;
        public bool IsConfirmPasswordErrorVisible
        {
            get => _isConfirmPasswordErrorVisible;
            set => SetProperty(ref _isConfirmPasswordErrorVisible, value);
        }

        public ICommand SignUpCommand => new Command(OnSignUpCommandAsync);

        #endregion

        #region -- Overrides --

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.TryGetValue(nameof(UserModel), out UserModel user))
            {
                Name = user.Name;
                Email = user.Email;
            }
        }

        #endregion

        #region -- Private helpers --     

        private async void OnSignUpCommandAsync(object obj)
        {
            var PasswordValidation = Validator.StringValid(Password, Validator.Password);

            IsPasswordErrorVisible = false;
            IsConfirmPasswordErrorVisible = false;

            if (Password == ConfirmPassword)
            {
                //if (!PasswordValidation)
                //{
                //    IsPasswordErrorVisible = true;
                //    PasswordError = StringResource.PasswordAlert;
                //}
                //else
                //{
                    var user = new UserModel()
                    {
                        Name = Name,
                        Email = Email,
                        Password = Password
                    };

                    await _profileService.SaveUserAsync(user);
                    await NavigationService.GoBackToRootAsync();
                //}
            }
            else
            {
                IsConfirmPasswordErrorVisible = true;
                ConfirmPasswordError = StringResource.ConfirmPasswordAlert;
            }
        }

        private bool CanSignIn()
        {
            return !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(Email);
        }

        #endregion

    }

}

