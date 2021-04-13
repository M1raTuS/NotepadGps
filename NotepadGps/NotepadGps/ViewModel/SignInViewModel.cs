﻿using Acr.UserDialogs;
using NotepadGps.Models;
using NotepadGps.Services.Autentification;
using NotepadGps.Services.Autorization;
using NotepadGps.Services.Profile;
using NotepadGps.View;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotepadGps.ViewModel
{
    public class SignInViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IAutorizationService _autorization;
        private readonly IProfileService _profileService;
        private readonly IAutentificationService _autentification;

        public SignInViewModel(INavigationService navigationService,
                               IAutorizationService autorization,
                               IProfileService profileService,
                               IAutentificationService autentification)
        {
            _navigationService = navigationService;
            _autorization = autorization;
            _profileService = profileService;
            _autentification = autentification;
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

        public ICommand SignInCommand => new Command(SignInUser);
        public ICommand SignUpCommand => new Command(SignUpUser);

        #endregion

        #region -- Private helpers --        

        private async void SignInUser()
        {
            _autorization.Authorizate(Email, Password);

            if (_autorization.IsAutorized)
            {
                await _navigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(MainListView)}");
            }
            else
            {
                UserDialogs.Instance.Alert("Invalid email or password!", "Alert", "Ok");
                Password = "";
            }
        }

        async void SignUpUser()
        {
            await _navigationService.NavigateAsync($"{nameof(SignUpView)}");
        }

        private bool CanSignIn()
        {
            if (!String.IsNullOrEmpty(Email) && !String.IsNullOrEmpty(Password))
            {
                return true;
            }
            return false;
        }

        private async void MapPinLoad()
        {
            var _user = await _profileService.GetAllUserListAsync();
            User = new ObservableCollection<UserModel>(_user);
        }

        #endregion

        #region -- Overrides --

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(Email) ||
                args.PropertyName == nameof(Password))
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

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            MapPinLoad();
        }

        #endregion
    }
}
