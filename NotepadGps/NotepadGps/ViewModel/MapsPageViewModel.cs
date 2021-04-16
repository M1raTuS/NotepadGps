using NotepadGps.Models;
using NotepadGps.Services.Map;
using NotepadGps.Services.Settings;
using NotepadGps.View;
using Plugin.Permissions;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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

        public MapsPageViewModel(
            INavigationService navigationService,
            IMapPinService mapPinService,
            ISettingsService settingsService)
            : base(navigationService)
        {
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
        public bool ListViewIsVisible //TODo: is
        {
            get => _listViewIsVisible;
            set => SetProperty(ref _listViewIsVisible, value);
        }

        private int _rowHeight;
        public int RowHeight //TODO: remove
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

        private async Task MapPinLoadAsync()
        {
            var mapPin = await _mapPinService.GetMapPinListByIdAsync();

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

        private async void OnPinClickedCommand(Pin pin)//TODO: async
        {
            PinsCheck();

            ListViewIsVisible = false;
            var nav = new NavigationParameters();
            nav.Add(nameof(Pin), pin);

            await NavigationService.NavigateAsync(nameof(PopUpView), nav, true, true);
        }

        private void PinsCheck()//TODO: rename
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

        protected async override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(SearchText))
            {
                await MapPinLoadAsync();
                ListViewIsVisible = false;
                RowHeight = 0;

                if (!(SearchText.Length == 0 || String.IsNullOrEmpty(SearchText)))
                {
                    ListViewIsVisible = true;

                    try
                    {
                        MapPins = MapPin;
                        var data = MapPins.Where(x => x.Title.ToLower().Contains(SearchText.ToLower()) || //TODO: to service
                        x.Latitude.ToString().ToLower().Contains(SearchText.ToLower()) ||
                        x.Longitude.ToString().ToLower().Contains(SearchText.ToLower()) ||
                        x.Description.ToLower().Contains(SearchText.ToLower()));
                        MapPins = new ObservableCollection<MapPinModel>(data);

                        switch (MapPins.Count) //TODO: remove
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

        protected async override void RaiseIsActiveChanged()
        {
            base.RaiseIsActiveChanged();

            await MapPinLoadAsync();
        }

        #endregion

        #region MyRegion

        public ICommand SaveCommand => new Command(SaveLocalNotification);




        //DateTime _selectedDate = DateTime.Today;
        //public DateTime SelectedDate
        //{
        //    get => _selectedDate;
        //    set
        //    {
        //        if (_selectedDate < DateTime.Now)
        //        {
        //            UserDialogs.Instance.Alert("Нельзя чтобы дата была меньше текущей даты", "Alert", "Ok");
        //        }
        //        else { SetProperty(ref _selectedDate, value); }
        //    }
        //}

        //TimeSpan _selectedTime = DateTime.Now.TimeOfDay;
        //public TimeSpan SelectedTime
        //{
        //    get => _selectedTime;
        //    set => SetProperty(ref _selectedTime, value);
        //}

        //string _messageText;
        //public string MessageText
        //{
        //    get => _messageText;
        //    set => SetProperty(ref _messageText, value);
        //}

        private async void SaveLocalNotification()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync<CalendarPermission>();
            if (status != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
            {
                status = await CrossPermissions.Current.RequestPermissionAsync<CalendarPermission>();

                if (status == Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                {
                    await DependencyService.Get<NotepadGps.Services.Calendar.ICalendarService>().AddEventToCalendar();
                }
            }
            else
            {
                await DependencyService.Get<NotepadGps.Services.Calendar.ICalendarService>().AddEventToCalendar();
            }



            //    var date = (SelectedDate.Date.Month.ToString("00") + "-" + SelectedDate.Date.Day.ToString("00") + "-" + SelectedDate.Date.Year.ToString());
            //    var time = Convert.ToDateTime(SelectedTime.ToString()).ToString("HH:mm");
            //    var dateTime = date + " " + time;
            //    var selectedDateTime = DateTime.ParseExact(dateTime, "MM-dd-yyyy HH:mm", CultureInfo.InvariantCulture);
            //    if (!string.IsNullOrEmpty(MessageText))
            //    {
            //        DependencyService.Get<ILocalNotificationService>().Cancel(0);
            //        DependencyService.Get<ILocalNotificationService>().LocalNotification("Local Notification", MessageText, 0, selectedDateTime);
            //        App.Current.MainPage.DisplayAlert("LocalNotificationDemo", "Notification details saved successfully ", "Ok");
            //    }
            //    else
            //    {
            //        App.Current.MainPage.DisplayAlert("LocalNotificationDemo", "Please enter meassage", "OK");
            //    }
        }

            //protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action onChanged = null)
            //{
            //    if (EqualityComparer<T>.Default.Equals(backingStore, value))
            //    { return false; }

            //    backingStore = value;
            //    onChanged?.Invoke();
            //    OnPropertyChanged(propertyName);
            //    return true;
            //}

            //public event PropertyChangedEventHandler PropertyChanged;
            //protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
            //{
            //    var changed = PropertyChanged;
            //    if (changed == null)
            //        return;
            //    changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
            //}
        }
    #endregion
}


