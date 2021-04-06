using Acr.UserDialogs;
using NotepadGps.Models;
using NotepadGps.Services.Profile;
using NotepadGps.Services.Settings;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace NotepadGps.ViewModel
{
    public class MapsPage1ViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IProfileService _profile;
        private readonly ISettingsService _settingsService;

        public MapsPage1ViewModel(INavigationService navigationService,
                                  IProfileService profile,
                                  ISettingsService settingsService)
        {
            _navigationService = navigationService;
            _profile = profile;
            _settingsService = settingsService;


            Load();
        }

        private void Load()
        {
            var mapPin = _profile.GetMapPinListById();
            MapPin = new ObservableCollection<MapPinModel>(mapPin);
        }

        public ICommand OnMapClicked => new Command(MapClicked);

        public void PinSelected(object param)
        {
            var pin = param as Pin;

            if (pin != null)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    UserDialogs.Instance.Alert(pin.Label);
                });
            }
        }

        private void MapClicked()
        {
            var map = new Map();
            map.MapClicked += OnMapClickeded;
        }
        void OnMapClickeded(object sender, MapClickedEventArgs e)
        {
            Debug.WriteLine($"MapClick: {e.Position.Latitude}, {e.Position.Longitude}");
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }

        protected override void RaiseIsActiveChanged()
        {
            base.RaiseIsActiveChanged();
            Load();
        }
    }
}

