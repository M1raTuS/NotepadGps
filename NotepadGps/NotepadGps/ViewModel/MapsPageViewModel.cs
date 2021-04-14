using NotepadGps.Models;
using NotepadGps.Services.Map;
using NotepadGps.Services.Settings;
using NotepadGps.View;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

        #region -- Public properties --

        private CameraPosition _currentCameraPosition;
        public CameraPosition CurrentCameraPosition
        {
            get => _currentCameraPosition;
            set => SetProperty(ref _currentCameraPosition, value);
        }

        private ObservableCollection<MapPinModel> _mapPins;
        public ObservableCollection<MapPinModel> MapPins
        {
            get => _mapPins;
            set => SetProperty(ref _mapPins, value);
        }

        private MapSpan _mapSpan;
        public MapSpan MapSpan
        {
            get => _mapSpan;
            set => SetProperty(ref _mapSpan, value);
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        private bool _listViewIsVisible;
        public bool ListViewIsVisible
        {
            get => _listViewIsVisible;
            set => SetProperty(ref _listViewIsVisible, value);
        }

        private int _rowHeight;
        public int RowHeight
        {
            get => _rowHeight;
            set => SetProperty(ref _rowHeight, value);
        }

        public ICommand FindMyLocationCommand => new Command(FindMyLocationAsync);
        public ICommand PinClickedCommand => new Command<Pin>(OnPinClickedCommand);
        public ICommand SelectedListViewCommand => new Command<MapPinModel>(OnSelectedListViewCommand);
        public ICommand MapClickedCommand => new Command(OnMapClickedCommand);

        #endregion

        #region -- Private helpers --        

        private void MapPinLoad()
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
                    MapSpan = new MapSpan(new Position(location.Latitude, location.Longitude), 1, 1);
                    CurrentCameraPosition = new CameraPosition(new Position(location.Latitude, location.Longitude), 18.0);
                }
                MapSpan = new MapSpan(new Position(location.Latitude, location.Longitude), 1, 1);
                CurrentCameraPosition = new CameraPosition(new Position(location.Latitude, location.Longitude), 18.0);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private async void OnPinClickedCommand(Pin pin)
        {
            PinsCheck();

            ListViewIsVisible = false;
            var nav = new NavigationParameters();
            nav.Add(nameof(Pin), pin);

            await _navigationService.NavigateAsync(nameof(PopUpView), nav, true, true);
        }

        private void PinsCheck()
        {
            if (MapPins != null)
            {
                MapPins.Clear();
            }
        }

        private void OnMapClickedCommand()
        {
            PinsCheck();
            ListViewIsVisible = false;
        }

        private void OnSelectedListViewCommand(MapPinModel mapPin)
        {
            if (mapPin != null)
            {
                Pin pin = new Pin
                {
                    Label = mapPin.Description,
                    Position = new Position(Convert.ToDouble(mapPin.Latitude), Convert.ToDouble(mapPin.Longitude))
                };

                CurrentCameraPosition = new CameraPosition(pin.Position, 15);
                PinsCheck();
                ListViewIsVisible = false;
            }
        }

        #endregion

        #region -- Overrides --

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.TryGetValue(nameof(MapPinModel), out MapPinModel pin))
            {
                CurrentCameraPosition = new CameraPosition(new Position(pin.Latitude, pin.Longitude), 12.0);
            }
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            if (args.PropertyName == nameof(SearchText))
            {
                MapPinLoad();
                ListViewIsVisible = false;
                RowHeight = 0;

                if (!(SearchText.Length == 0 || String.IsNullOrEmpty(SearchText)))
                {
                    ListViewIsVisible = true;

                    try
                    {
                        MapPins = MapPin;
                        var data = MapPins.Where(x => x.Title.ToLower().Contains(SearchText.ToLower()) ||
                        x.Latitude.ToString().ToLower().Contains(SearchText.ToLower()) ||
                        x.Longitude.ToString().ToLower().Contains(SearchText.ToLower()) ||
                        x.Description.ToLower().Contains(SearchText.ToLower()));
                        MapPins = new ObservableCollection<MapPinModel>(data);

                        switch (MapPins.Count)
                        {
                            case 1:
                                RowHeight = 45;
                                break;
                            case 2:
                                RowHeight = 90;
                                break; ;
                            default:
                                RowHeight = 135;
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                    }
                }
            }
        }

        protected override void RaiseIsActiveChanged()
        {
            base.RaiseIsActiveChanged();
            MapPinLoad();
        }

        #endregion
    }
}

