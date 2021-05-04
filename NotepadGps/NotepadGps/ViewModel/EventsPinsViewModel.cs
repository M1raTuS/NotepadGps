using Prism.Navigation;
using System.Collections.ObjectModel;

namespace NotepadGps.ViewModel
{
    public class EventsPinsViewModel : BaseViewModel
    {
        public EventsPinsViewModel(
            INavigationService navigationService)
            : base(navigationService)
        {
            eventList.Add(new AddEditMapPinViewModel(navigationService,null,null,null,null,null));
            eventList.Add(new EventsViewModel(navigationService,null,null));
        }

        private ObservableCollection<BaseViewModel> eventList;
        public ObservableCollection<BaseViewModel> EventList
        {
            get => eventList;
            set => SetProperty(ref eventList, value);
        }
    }
}