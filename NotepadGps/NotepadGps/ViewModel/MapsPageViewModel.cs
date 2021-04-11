using NotepadGps.Models;
using NotepadGps.Services.Map;
using NotepadGps.Services.Settings;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace NotepadGps.ViewModel
{
    public class MapsPageViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IMapPinService _mapPinService;
        private readonly ISettingsService _settingsService;

        public MapsPageViewModel(INavigationService navigationService,
                                  IMapPinService mapPinService,
                                  ISettingsService settingsService)
        {
            _navigationService = navigationService;
            _mapPinService = mapPinService;
            _settingsService = settingsService;
        }

        #region -Public properties-

        private CameraPosition _currentCameraPosition;
        public CameraPosition CurrentCameraPosition
        {
            get => _currentCameraPosition;
            set => SetProperty(ref _currentCameraPosition, value);
        }

        public ICommand FindMyLocationCommand => new Command(FindMyLocationAsync);
        #endregion

        #region -Methods-

        private void Load()
        {
            var mapPin = _mapPinService.GetMapPinListById();
            MapPin = new ObservableCollection<MapPinModel>(mapPin);
        }

        private async void FindMyLocationAsync()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location == null)
                {
                    location = await Geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.Medium,
                        Timeout = TimeSpan.FromSeconds(10)
                    });
                    CurrentCameraPosition = new CameraPosition(new Position(location.Latitude, location.Longitude), 18.0);
                }
                CurrentCameraPosition = new CameraPosition(new Position(location.Latitude, location.Longitude), 18.0);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        #endregion

        #region -Overrides-
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.TryGetValue(nameof(MapPinModel), out MapPinModel pin))
            {
                CurrentCameraPosition = new CameraPosition(new Position(pin.Latitude, pin.Longitude), 12.0);
            }
        }

        protected override void RaiseIsActiveChanged()
        {
            base.RaiseIsActiveChanged();
            Load();
        }

        #endregion

    }
}

