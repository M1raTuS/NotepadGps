using NotepadGps.Models;
using NotepadGps.Services.Autentification;
using NotepadGps.View;
using Prism.Navigation;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotepadGps.ViewModel
{
    public class SignUpViewModel : BaseViewModel
    {
        private readonly IAutentificationService _autentificationService;

        public SignUpViewModel(
            INavigationService navigationService,
            IAutentificationService autentificationService)
            : base(navigationService)
        {
            _autentificationService = autentificationService;
        }

        #region -- Public properties --

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _nameError;
        public string NameError
        {
            get => _nameError;
            set => SetProperty(ref _nameError, value);
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

        private bool _isNameErrorVisible;
        public bool IsNameErrorVisible
        {
            get => _isNameErrorVisible;
            set => SetProperty(ref _isNameErrorVisible, value);
        }

        public ICommand NextPageCommand => new Command(OnNextPageCommand);
        public ICommand BackCommand => new Command(OnBackCommandAsync);

        #endregion

        #region -- Private helpers --     

        private async void OnNextPageCommand(object obj)
        {
            //var NamelValidation = Validator.StringValid(Name, Validator.Name);
            //var EmailValidation = Validator.StringValid(Email, Validator.Email);
            //var isEmailExist = await _autentificationService.CheckEmailAsync(Email);

            //IsEmailErrorVisible = false;
            //IsNameErrorVisible = false;

            //if (!NamelValidation)
            //{
            //    IsNameErrorVisible = true;
            //    NameError = StringResource.NameAlert;
            //}
            //else if (!EmailValidation)
            //{
            //    IsEmailErrorVisible = true;
            //    EmailError = StringResource.MailAlert;
            //}
            //else if (isEmailExist)
            //{
            //    IsEmailErrorVisible = true;
            //    EmailError = StringResource.MailConflict;
            //}
            //else
            //{
            var user = new UserModel()
            {
                Name = Name,
                Email = Email
            };

            var nav = new NavigationParameters();
            nav.Add(nameof(UserModel), user);

            await NavigationService.NavigateAsync(nameof(SignUpPwCheckView), nav);
            //await NavigationService.NavigateAsync(nameof(SignUpPwCheckView));
            //}
        }

        private async void OnBackCommandAsync()
        {
            await NavigationService.GoBackAsync();
        }

        #endregion
    }
}
