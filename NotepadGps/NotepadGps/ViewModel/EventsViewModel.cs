using NotepadGps.Models;
using NotepadGps.Services.Autorization;
using NotepadGps.Services.Image;
using NotepadGps.View;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotepadGps.ViewModel
{
    public class EventsViewModel : BaseViewModel
    {
        private readonly IEventService _eventService;
        private readonly IAutorizationService _autorizationService;

        public EventsViewModel(
            INavigationService navigationService,
            IEventService eventService,
            IAutorizationService autorizationService)
            : base(navigationService)
        {
            _eventService = eventService;
            _autorizationService = autorizationService;
        }

        #region -- Public properties --

        private ObservableCollection<EventModel> eventList;
        public ObservableCollection<EventModel> EventList
        {
            get => eventList;
            set => SetProperty(ref eventList, value);
        }

        private bool _isLabelVisible;
        public bool IsLabelVisible
        {
            get => _isLabelVisible;
            set => SetProperty(ref _isLabelVisible, value);
        }

        private bool _isListVisible;
        public bool IsListVisible
        {
            get => _isListVisible;
            set => SetProperty(ref _isListVisible, value);
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private DateTime _date;
        public DateTime Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }

        private TimeSpan _time;
        public TimeSpan Time
        {
            get => _time;
            set => SetProperty(ref _time, value);
        }

        public ICommand AddEventCommand => new Command(OnAddEventCommandAsync);

        #endregion

        #region -- Ovverides --

        protected async override void RaiseIsActiveChanged()
        {
            base.RaiseIsActiveChanged();

            if (IsActive)
            {
                await EventListLoadAsync();
            }
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(EventList))
            {
                IsLabelVisible = false;
                IsListVisible = true;

                if (EventList.Count < 1)
                {
                    IsLabelVisible = true;
                    IsListVisible = false;
                }
            }
        }

        #endregion

        #region -- Private helpers --

        private async Task<ObservableCollection<EventModel>> EventListLoadAsync()
        {
            var events = await _eventService.GetEventListByIdAsync(_autorizationService.GetAutorizedUserId);
            EventList = new ObservableCollection<EventModel>(events);
            return EventList;
        }

        private async void OnAddEventCommandAsync()
        {
            await NavigationService.NavigateAsync(nameof(NotifyPageView));
        }

        #endregion
    }
}
