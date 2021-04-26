using NotepadGps.Services.Autorization;
using NotepadGps.Services.Map;
using NotepadGps.View;
using Prism.Navigation;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotepadGps.ViewModel
{
    public class MainListViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IAutorizationService _autorizationService;
        private readonly IMapPinService _mapPinService;

        public MainListViewModel(
            INavigationService navigationService,
            IMapPinService mapPinService,
            IAutorizationService autorizationService)
            : base(navigationService)
        {
            _mapPinService = mapPinService;
            _navigationService = navigationService;
            _autorizationService = autorizationService;
        }

        #region -- Public properties --

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        public ICommand LogOutTapCommand => new Command(OnLogOutCommandAsync); 
        public ICommand OpenSettings => new Command(OnOpenSettingsAsync);

        #endregion

        #region -- Overrides --   

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(SearchText))
            {
                MessagingCenter.Send(this, "SearchTextChanged", SearchText);
            }
        }

        #endregion

        #region -- Private helpers --        

        private async void OnLogOutCommandAsync()
        {
            _autorizationService.Unautorize();
            await _navigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(MainPageView)}");
        }

        private async void OnOpenSettingsAsync()
        {
            await _navigationService.NavigateAsync(nameof(SettingsView));
        }
        #endregion


    }
}
