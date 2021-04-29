using Acr.UserDialogs;
using NotepadGps.Models;
using NotepadGps.Resource;
using NotepadGps.Services.Autorization;
using NotepadGps.Services.Image;
using NotepadGps.Services.Map;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace NotepadGps.ViewModel
{
    public class NotifyViewModel : BaseViewModel
    {
        private readonly IEventService _eventService;
        private readonly IMapPinService _mapPinService;
        private readonly IAutorizationService _autorizationService;

        public NotifyViewModel(
            INavigationService navigationService,
            IMapPinService mapPinService,
            IEventService eventService,
            IAutorizationService autorizationService)
            : base(navigationService)
        {
            _mapPinService = mapPinService;
            _eventService = eventService;
            _autorizationService = autorizationService;

            MapPinLoadAsync();
        }

        #region -- Public properties --

        private ObservableCollection<MapPinModel> mapPin;
        public ObservableCollection<MapPinModel> MapPin
        {
            get => mapPin;
            set => SetProperty(ref mapPin, value);
        }

        private ObservableCollection<EventModel> eventList;
        public ObservableCollection<EventModel> EventList
        {
            get => eventList;
            set => SetProperty(ref eventList, value);
        }

        private DateTime _selectedDate = DateTime.Now.AddSeconds(10);
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                SetProperty(ref _selectedDate, value);

                if (_selectedDate.AddSeconds(10) < DateTime.Now)
                {
                    UserDialogs.Instance.Alert(StringResource.AlertDate, StringResource.Alert, StringResource.Ok);
                    SetProperty(ref _selectedDate, DateTime.Now.AddSeconds(10));
                }
            }
        }

        private TimeSpan _selectedTime = DateTime.Now.TimeOfDay;
        public TimeSpan SelectedTime
        {
            get => _selectedTime;
            set
            {
                SetProperty(ref _selectedTime, value);

                if (_selectedTime.Add(new TimeSpan(0, 0, 20)) < DateTime.Now.TimeOfDay)
                {
                    UserDialogs.Instance.Alert(StringResource.AlertTime, StringResource.Alert, StringResource.Ok);
                    SetProperty(ref _selectedTime, DateTime.Now.TimeOfDay);
                }
            }
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private MapStyle _mapTheme;
        public MapStyle MapTheme
        {
            get => _mapTheme;
            set => SetProperty(ref _mapTheme, value);
        }

        public ICommand SaveCommand => new Command(SaveLocalNotification);
        public ICommand BackCommand => new Command(OnBackCommand);

        #endregion

        #region -- Private helpers --

        private async Task<ObservableCollection<MapPinModel>> MapPinLoadAsync()
        {
            var mapPin = await _mapPinService.GetMapPinListByIdAsync(_autorizationService.GetAutorizedUserId);
            MapPin = new ObservableCollection<MapPinModel>(mapPin);
            return MapPin;
        }

        private async void SaveLocalNotification()
        {
            if (!string.IsNullOrWhiteSpace(Title) && !string.IsNullOrWhiteSpace(SelectedDate.ToString()) && !string.IsNullOrWhiteSpace(SelectedTime.ToString()))
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync<CalendarPermission>();

                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Calendar))
                    {
                       
                    }

                    status = await CrossPermissions.Current.RequestPermissionAsync<CalendarPermission>();
                }
                
                if (status == PermissionStatus.Granted)
                {
                    await AddedEvent();
                }
                else if(status != PermissionStatus.Unknown)
                {
                    CrossPermissions.Current.OpenAppSettings();
                }
            }
            else
            {
                UserDialogs.Instance.Alert(StringResource.AlertAll, StringResource.Alert, StringResource.Ok);
            }
        }

        private async Task AddedEvent()
        {
            await DependencyService.Get<Services.Calendar.ICalendarService>().AddEventToCalendar(Title, SelectedDate, SelectedTime);

            var events = new EventModel
            {
                UserId = _autorizationService.GetAutorizedUserId,
                Title = Title,
                Date = SelectedDate,
                Time = SelectedTime
            };

            await _eventService.SaveEventAsync(events);
            await NavigationService.GoBackAsync();
        }

        private void OnBackCommand()
        {
            NavigationService.GoBackAsync();
        }

    }

    #endregion

}
