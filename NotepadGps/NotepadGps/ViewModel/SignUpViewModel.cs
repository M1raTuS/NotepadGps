using NotepadGps.Models;
using NotepadGps.Services.Profile;
using NotepadGps.Services.Repository;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotepadGps.ViewModel
{
    public class SignUpViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IProfileService _profile;

        public SignUpViewModel(INavigationService navigationService,
                                IProfileService profile)
        {
            _navigationService = navigationService;
            _profile = profile;


            User = new ObservableCollection<UserModel>();
        }


        #region -Public properties-

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

        #region -Methods-
        private async void AddUserAsync(object obj)
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

        private bool CanSignIn()
        {
            if (!String.IsNullOrEmpty(Name) && !String.IsNullOrEmpty(Password) && !String.IsNullOrEmpty(Email))
            {
                return true;
            }
            return false;
        }

        #endregion

        #region -Overrides-

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

            //var _user = await _repository.GetAllAsync<UserModel>();
            // User = new ObservableCollection<UserModel>(_user);
        }

        #endregion
    }
}
