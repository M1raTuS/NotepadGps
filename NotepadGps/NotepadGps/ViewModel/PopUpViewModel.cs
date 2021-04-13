using Prism.Navigation;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace NotepadGps.ViewModel
{
    public class PopUpViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;

        public PopUpViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        #region -- Public properties --

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _latitude;
        public string Latitude
        {
            get => _latitude;
            set => SetProperty(ref _latitude, value);
        }

        private string _longitude;
        public string Longitude
        {
            get => _longitude;
            set => SetProperty(ref _longitude, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public ICommand TapCommand => new Command(TapCommands);

        #endregion

        #region -- Private helpers --

        private void TapCommands()
        {
            NavigationParameters nav = new NavigationParameters();
            _navigationService.GoBackAsync(nav, true, true);
        }

        #endregion

        #region -- Overrides --

        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters.TryGetValue(nameof(Pin), out Pin pin))
            {
                Title = pin.Label;
                Latitude = pin.Position.Latitude.ToString();
                Longitude = pin.Position.Longitude.ToString();
                Description = pin.Address is null ? "{null}" : pin.Address;
            }
        }

        #endregion
    }
}
