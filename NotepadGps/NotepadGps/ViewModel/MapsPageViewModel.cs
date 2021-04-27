using NotepadGps.Models;
using NotepadGps.Services.Autorization;
using NotepadGps.Services.Map;
using NotepadGps.Services.Settings;
using NotepadGps.View;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace NotepadGps.ViewModel
{
    public class MapsPageViewModel : BaseViewModel
    {
        private readonly IMapPinService _mapPinService;
        private readonly ISettingsService _settingsService;
        private readonly IAutorizationService _autorizationService;

        public MapsPageViewModel(
            INavigationService navigationService,
            IMapPinService mapPinService,
            ISettingsService settingsService,
            IAutorizationService autorizationService)
            : base(navigationService)
        {
            _mapPinService = mapPinService;
            _settingsService = settingsService;
            _autorizationService = autorizationService;

        }

        #region -- Public properties --

        private CameraPosition _currentCameraPosition;
        public CameraPosition CurrentCameraPosition
        {
            get => _currentCameraPosition;
            set => SetProperty(ref _currentCameraPosition, value);
        }

        private ObservableCollection<MapPinModel> mapPin;
        public ObservableCollection<MapPinModel> MapPin
        {
            get => mapPin;
            set => SetProperty(ref mapPin, value);
        }

        private ObservableCollection<MapPinModel> _pinSearchList;
        public ObservableCollection<MapPinModel> PinSearchList
        {
            get => _pinSearchList;
            set => SetProperty(ref _pinSearchList, value);
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

        private bool _isListViewIsVisible;
        public bool IsListViewIsVisible
        {
            get => _isListViewIsVisible;
            set => SetProperty(ref _isListViewIsVisible, value);
        }

        private int _listViewHeight; 
        public int ListViewHeight
        {
            get => _listViewHeight;
            set => SetProperty(ref _listViewHeight, value);
        }

        public ICommand FindMyLocationCommand => new Command(OnFindMyLocationAsync);
        public ICommand PinClickedCommand => new Command<Pin>(OnPinClickedCommandAsync);
        public ICommand SelectedListViewCommand => new Command<MapPinModel>(OnSelectedListViewCommand);
        public ICommand MapClickedCommand => new Command(OnMapClickedCommand);
        public ICommand LogOutTapCommand => new Command(OnLogOutCommandAsync);

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

        protected async override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(SearchText))
            {
                if (IsActive)
                {
                    await MapPinLoadAsync();

                    IsListViewIsVisible = false;
                    ListViewHeight = 0;

                    if (!string.IsNullOrWhiteSpace(SearchText))
                    {
                        IsListViewIsVisible = true;

                        PinSearchList = MapPin;

                        var data = PinSearchList.Where(x => x.Title.ToLower().Contains(SearchText.ToLower()) ||
                        x.Latitude.ToString().ToLower().Contains(SearchText.ToLower()) ||
                        x.Longitude.ToString().ToLower().Contains(SearchText.ToLower()) ||
                        x.Description.ToLower().Contains(SearchText.ToLower()));

                        PinSearchList = new ObservableCollection<MapPinModel>(data);

                        if (PinSearchList.Count == 0)
                        {
                            IsListViewIsVisible = false;
                        }
                        else if (PinSearchList.Count > 3)
                        {
                            ListViewHeight = 135;
                        }
                        else
                        {
                            ListViewHeight = 45 * PinSearchList.Count;
                        }
                    }
                }
            }
        }

        protected async override void RaiseIsActiveChanged()
        {
            base.RaiseIsActiveChanged();

            if (IsActive)
            {
                await MapPinLoadAsync();

                MessagingCenter.Subscribe<MainListViewModel, string>(this, "SearchTextChanged", (obj, e) =>
                {
                    SearchText = e;
                });
            }
            else
            {
                MessagingCenter.Unsubscribe<MainListViewModel>(this, "SearchTextChanged");
            }

            IsListViewIsVisible = false;
        }

        #endregion

        #region -- Private helpers --    

        private async void OnLogOutCommandAsync()
        {
            _autorizationService.Unautorize();
            await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(MainPageView)}");
        }

        private async Task<ObservableCollection<MapPinModel>> MapPinLoadAsync()
        {
            var mapPin = await _mapPinService.GetMapPinListByIdAsync(_autorizationService.GetAutorizedUserId);
            MapPin = new ObservableCollection<MapPinModel>(mapPin);

            return MapPin;
        }

        private async void OnFindMyLocationAsync()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync<LocationPermission>();

            if (status != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                {
                }

                status = await CrossPermissions.Current.RequestPermissionAsync<LocationPermission>();

            }
            else if (status == Plugin.Permissions.Abstractions.PermissionStatus.Granted)
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
                catch
                {
                }
            }
            else if (status != Plugin.Permissions.Abstractions.PermissionStatus.Unknown)
            {
                CrossPermissions.Current.OpenAppSettings();
            }
        }

        private async void OnPinClickedCommandAsync(Pin pin)
        {
            MapPinsCheck();

            IsListViewIsVisible = false;

            var nav = new NavigationParameters();
            nav.Add(nameof(Pin), pin);

            await NavigationService.NavigateAsync(nameof(PopUpView), nav, true, false);
        }

        private void MapPinsCheck()
        {
            if (PinSearchList != null)
            {
                PinSearchList.Clear();
            }
        }

        private void OnMapClickedCommand()
        {
            MapPinsCheck();
            IsListViewIsVisible = false;
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
                MapPinsCheck();
                IsListViewIsVisible = false;
            }
        }

        #endregion
    }
}


