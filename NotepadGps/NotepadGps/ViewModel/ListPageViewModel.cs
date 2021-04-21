using NotepadGps.Models;
using NotepadGps.Resource;
using NotepadGps.Services.Autorization;
using NotepadGps.Services.Map;
using NotepadGps.View;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotepadGps.ViewModel
{
    public class ListPageViewModel : BaseViewModel
    {
        private const string ImgFull = "ic_like_blue";
        private const string ImgEmpty = "ic_like_gray";

        private readonly IMapPinService _mapPinService;
        private readonly IAutorizationService _autorizationService;

        public ListPageViewModel(
            INavigationService navigationService,
            IMapPinService mapPinService,
            IAutorizationService autorizationService)
            : base(navigationService)
        {
            _mapPinService = mapPinService;
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

        private bool _labelVisible;
        public bool LabelVisible
        {
            get => _labelVisible;
            set => SetProperty(ref _labelVisible, value);
        }

        private string _searchPin;
        public string SearchPin
        {
            get => _searchPin;
            set => SetProperty(ref _searchPin, value);
        }

        private bool _isChosen;
        public bool IsChosen
        {
            get => _labelVisible;
            set => SetProperty(ref _isChosen, value);
        }

        public ICommand AddMapPinFloatingButtonCommand => new Command(AddMapPinFloatingButtonAsync);
        public ICommand SelectedCommand => new Command(OnSelectedCommandAsync);
        public ICommand EditContext => new Command(EditContextMenuAsync);
        public ICommand DeleteContext => new Command(DeleteContextMenuAsync); 
        public ICommand CheckedPinCommand => new Command<MapPinModel>(OnCheckedPinCommandAsync);
        public ICommand AddEvents => new Command<MapPinModel>(OnAddEvents);

        #endregion

        #region -- Private helpers --        

        private async void AddMapPinFloatingButtonAsync()
        {
            await NavigationService.NavigateAsync(nameof(AddEditMapPinView));
        }

        private async void OnSelectedCommandAsync(object pin)
        {
            var nav = new NavigationParameters();
            nav.Add(nameof(MapPinModel), (MapPinModel)pin);

            await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/MainListView?selectedTab=MapsPage", nav);
        }

        private async void EditContextMenuAsync(object obj)
        {
            var nav = new NavigationParameters();
            nav.Add(nameof(MapPinModel), (MapPinModel)obj);

            await NavigationService.NavigateAsync(nameof(AddEditMapPinView), nav, false, true);
        }

        private async void DeleteContextMenuAsync(object obj)
        {
            bool del = await Application.Current.MainPage.DisplayAlert(StringResource.Alert, StringResource.DeleteAccept, StringResource.Ok, StringResource.Cancel);

            if (del)
            {
                await _mapPinService.DeleteMapPinAsync((MapPinModel)obj);

                await MapPinLoadAsync();
            }
        }

        private async void OnCheckedPinCommandAsync(MapPinModel mapPin)
        {
            if (mapPin.IsChosen)
            {
                mapPin.ImgPath = ImgEmpty;
                mapPin.IsChosen = false;
            }
            else
            {
                mapPin.ImgPath = ImgFull;
                mapPin.IsChosen = true;
            }

            await _mapPinService.UpdateMapPinAsync(mapPin);

            await MapPinLoadAsync();
        }

        private async void OnAddEvents(object obj)
        {
            var nav = new NavigationParameters();
            nav.Add(nameof(MapPinModel), (MapPinModel)obj);

            await NavigationService.NavigateAsync(nameof(NotifyPageView), nav);
        }

        private async Task<ObservableCollection<MapPinModel>> MapPinLoadAsync()
        {
            var mapPin = await _mapPinService.GetMapPinListByIdAsync(_autorizationService.GetAutorizedUserId);
            MapPin = new ObservableCollection<MapPinModel>(mapPin);
            return MapPin;
        }

        #endregion

        #region -- Overrides --

        protected async override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            if (args.PropertyName == nameof(MapPin))
            {
                LabelVisible = false;

                if (MapPin.Count < 1)
                {
                    LabelVisible = true;
                }
            }

            if (args.PropertyName == nameof(SearchPin))
            {
                await MapPinLoadAsync();

                if (!(SearchPin.Length == 0 || string.IsNullOrEmpty(SearchPin)))
                {
                    var pin = MapPin.Where(x => x.Title.ToLower().Contains(SearchPin.ToLower()) || //TODO: to service
                    x.Latitude.ToString().ToLower().Contains(SearchPin.ToLower()) ||
                    x.Longitude.ToString().ToLower().Contains(SearchPin.ToLower()) ||
                    x.Description.ToLower().Contains(SearchPin.ToLower()));
                    MapPin = new ObservableCollection<MapPinModel>(pin);
                }
            }
        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            await MapPinLoadAsync();
        }

        protected async override void RaiseIsActiveChanged()
        {
            base.RaiseIsActiveChanged();
        }

        #endregion
    }
}
