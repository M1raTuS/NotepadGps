using Acr.UserDialogs;
using NotepadGps.Models;
using NotepadGps.Services.Autentification;
using NotepadGps.Services.Profile;
using NotepadGps.Services.Validation;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotepadGps.ViewModel
{
    public class SignUpViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IProfileService _profile;
        private readonly IAutentificationService _autentification;

        public SignUpViewModel(INavigationService navigationService,
                               IProfileService profile,
                               IAutentificationService autentification)
        {
            _navigationService = navigationService;
            _profile = profile;
            _autentification = autentification;

            User = new ObservableCollection<UserModel>();
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

        public ICommand AddCommand => new Command(AddUserAsync);

        #endregion

        #region -- Private helpers --     

        private async void AddUserAsync(object obj)
        {
            var NamelValidation = Validator.StringValid(Name, Validator.Name);
            var EmailValidation = Validator.StringValid(Email, Validator.Email);
            var PasswordValidation = Validator.StringValid(Password, Validator.Password);

            if (!NamelValidation)
            {
                UserDialogs.Instance.Alert("Имя должно быть не менее 4 и не более 16 символов. Имя не должно начинаться с цифер", "Alert", "Ok");
            }
            else if (!EmailValidation)
            {
                UserDialogs.Instance.Alert("Введите корректный почтовый адрес", "Alert", "Ok");
            }
            else if (!PasswordValidation)
            {
                UserDialogs.Instance.Alert("Пароль должен быть не менее 8 и не более 16 символов. Пароль должен содержать минимум одну заглавную букву, одну строчную букву и одну цифру", "Alert", "Ok");
            }
            else if (_autentification.CheckEmail(Email))
            {
                UserDialogs.Instance.Alert("Эта почта уже занята", "Alert", "Ok");
            }
            else
            {
                var user = new UserModel()
                {
                    Name = Name,
                    Email = Email,
                    Password = Password
                };

                await _profile.SaveUserAsync(user);
                await _navigationService.GoBackAsync();
            }
        }

        private bool CanSignIn()
        {
            if (!String.IsNullOrEmpty(Name) && !String.IsNullOrEmpty(Password) && !String.IsNullOrEmpty(Email))
            {
                return true;
            }
            return false;
        }

        #endregion

        #region -- Overrides --

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(Name) ||
                args.PropertyName == nameof(Password) ||
                args.PropertyName == nameof(Email))
            {
                if (CanSignIn())
                {
                    ButtonEnabled = true;
                }
                else
                {
                    ButtonEnabled = false;
                }
            }
        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            var _user = await _profile.GetAllUserListAsync();
            User = new ObservableCollection<UserModel>(_user);
        }

        #endregion
    }
}
