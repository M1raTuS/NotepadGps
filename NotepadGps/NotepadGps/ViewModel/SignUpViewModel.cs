using Acr.UserDialogs;
using NotepadGps.Models;
using NotepadGps.Resource;
using NotepadGps.Services.Autentification;
using NotepadGps.Services.Profile;
using NotepadGps.Services.Validation;
using Prism.Navigation;
using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotepadGps.ViewModel
{
    public class SignUpViewModel : BaseViewModel
    {
        private readonly IProfileService _profileService;
        private readonly IAutentificationService _autentificationService;

        public SignUpViewModel(
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

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private bool _buttonEnabled;
        public bool ButtonEnabled
        {
            get => _buttonEnabled;
            set => SetProperty(ref _buttonEnabled, value);
        }

        public ICommand AddCommand => new Command(OnAddCommandAsync);

        #endregion

        #region -- Overrides --

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(Name) ||
                args.PropertyName == nameof(Password) ||
                args.PropertyName == nameof(Email))
            {
                ButtonEnabled = CanSignIn();
            }
        }

        #endregion

        #region -- Private helpers --     

        private async void OnAddCommandAsync(object obj)
        {
            var NamelValidation = Validator.StringValid(Name, Validator.Name);
            var EmailValidation = Validator.StringValid(Email, Validator.Email);
            var PasswordValidation = Validator.StringValid(Password, Validator.Password);
            var isEmailExist = await _autentificationService.CheckEmailAsync(Email);

            //if (!NamelValidation)
            //{
            //    UserDialogs.Instance.Alert(StringResource.NameAlert, StringResource.Alert, StringResource.Ok);
            //}
            //else if (!EmailValidation)
            //{
            //    UserDialogs.Instance.Alert(StringResource.MailAlert, StringResource.Alert, StringResource.Ok);
            //}
            //else if (!PasswordValidation)
            //{
            //    UserDialogs.Instance.Alert(StringResource.PasswordAlert, StringResource.Alert, StringResource.Ok);
            //}
            //else if (isEmailExist)
            //{
            //    UserDialogs.Instance.Alert(StringResource.MailConflict,StringResource.Alert,StringResource.Ok);
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
                await NavigationService.GoBackAsync();
            //}
        }

        private bool CanSignIn()
        {
            return !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(Email); 
        }

        #endregion

    }
}
