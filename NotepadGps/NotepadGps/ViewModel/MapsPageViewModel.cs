using Acr.UserDialogs;
using NotepadGps.Models;
using NotepadGps.Services.Profile;
using NotepadGps.Services.Settings;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace NotepadGps.ViewModel
{
    public class MapsPageViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IProfileService _profile;
        private readonly ISettingsService _settingsService;

        public MapsPageViewModel(INavigationService navigationService,
                                  IProfileService profile,
                                  ISettingsService settingsService)
        {
            _navigationService = navigationService;
            _profile = profile;
            _settingsService = settingsService;
           // Load();
        }

        #region -Public properties-

        private CameraPosition _currentCameraPosition;
        public CameraPosition CurrentCameraPosition
        {
            get => _currentCameraPosition;
            set => SetProperty(ref _currentCameraPosition, value);
        }

        public ICommand OnMapClicked => new Command(MapClicked);
        public ICommand FindMyLocationCommand => new Command(FindMyLocationAsync);
        #endregion

        #region -Methods-

        private void Load()
        {
            var mapPin = _profile.GetMapPinListById();
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
            //var map = new Map();
            //map.MapClicked += OnMapClickeded;
        }
        void OnMapClickeded(object sender, MapClickedEventArgs e)
        {
            // Debug.WriteLine($"MapClick: {e.Position.Latitude}, {e.Position.Longitude}");
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




        private string _customFrameLable;
        public string CustomFrameLabel
        {
            get => _customFrameLable;

            set => SetProperty(ref _customFrameLable, value);
        }

        private string _customFrameDescriptionLabel;
        public string CustomFrameDescriptionLabel
        {
            get => _customFrameDescriptionLabel;

            set => SetProperty(ref _customFrameDescriptionLabel, value);
        }

        private string _customFrameLatitude;
        public string CustomFrameLatitude
        {
            get => _customFrameLatitude;

            set => SetProperty(ref _customFrameLatitude, value);
        }

        private string _customframeLongitude;
        public string CustomFrameLongitudeLabel
        {
            get => _customframeLongitude;

            set => SetProperty(ref _customframeLongitude, value);
        }

        private bool _showedFrameData;
        public bool ShowedFrameData
        {
            get => _showedFrameData;

            set => SetProperty(ref _showedFrameData, value);
        }

        public ICommand PinClickedCommand => new Command<Pin>(PinClicked);

        async private void PinClicked(Pin pin)
        {
            var items = _profile.GetMapPinListById();
            var tappedPin = items.FirstOrDefault(x => x.Title == pin.Label);

            if (tappedPin != null)
            {
                CustomFrameLabel = tappedPin.Title;
                CustomFrameDescriptionLabel = tappedPin.Description;
                CustomFrameLatitude = tappedPin.Latitude.ToString();
                CustomFrameLongitudeLabel = tappedPin.Longitude.ToString();
                ShowedFrameData = true;
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("error", "pin=null", "Ok", null);
            }
        }
    }
}

