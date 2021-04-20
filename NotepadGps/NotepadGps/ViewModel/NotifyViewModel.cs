using Acr.UserDialogs;
using NotepadGps.Resource;
using Plugin.Permissions;
using Prism.Navigation;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotepadGps.ViewModel
{
    public class NotifyViewModel : BaseViewModel
    {
        public NotifyViewModel(
            INavigationService navigationService)
            : base(navigationService)
        {

        }

        #region -- Overrides --

        public async override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
        }

        #endregion

        #region MyRegion

        public ICommand SaveCommand => new Command(SaveLocalNotification);

        DateTime _selectedDate = DateTime.Today;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (_selectedDate < DateTime.Now)
                {
                    UserDialogs.Instance.Alert(StringResource.AlertDate, StringResource.Alert, StringResource.Ok);
                }
                else { SetProperty(ref _selectedDate, value); }
            }
        }

        TimeSpan _selectedTime = DateTime.Now.TimeOfDay;
        public TimeSpan SelectedTime
        {
            get => _selectedTime;
            set => SetProperty(ref _selectedTime, value);
        }

        string _messageText;
        public string MessageText
        {
            get => _messageText;
            set => SetProperty(ref _messageText, value);
        }

        private async void SaveLocalNotification()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync<CalendarPermission>();

            if (status != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
            {
                status = await CrossPermissions.Current.RequestPermissionAsync<CalendarPermission>();

                if (status == Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                {
                    await DependencyService.Get<NotepadGps.Services.Calendar.ICalendarService>().AddEventToCalendar(MessageText, MessageText);
                }
            }
            else
            {
                await DependencyService.Get<NotepadGps.Services.Calendar.ICalendarService>().AddEventToCalendar(MessageText, MessageText);
            }
        }
    }
    #endregion

}
