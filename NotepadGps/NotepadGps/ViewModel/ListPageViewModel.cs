using NotepadGps.Models;
using NotepadGps.Services.Map;
using NotepadGps.View;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotepadGps.ViewModel
{
    public class ListPageViewModel : BaseViewModel
    {
        private readonly IMapPinService _mapPinService;

        public ListPageViewModel(
            INavigationService navigationService,
            IMapPinService mapPinService)
            : base(navigationService)
        {
            _mapPinService = mapPinService;
        }

        #region -- Public properties --

        private ObservableCollection<MapPinModel> _mapPins;
        public ObservableCollection<MapPinModel> MapPins
        {
            get => _mapPins;
            set => SetProperty(ref _mapPins, value);
        }

        private ObservableCollection<MapPinModel> _selectedItem;
        public ObservableCollection<MapPinModel> SelectedItem //TODO: remove
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
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

        private bool _chosen;
        public bool Chosen //TODO: is
        {
            get => _labelVisible;
            set => SetProperty(ref _chosen, value);
        }

        public ICommand AddMapPinFloatingButtonCommand => new Command(AddMapPinFloatingButton);
        public ICommand SelectedCommand => new Command(SelectedPin); //TODO: OnCommandNameAsync
        public ICommand EditContext => new Command(EditContextMenu);
        public ICommand DeleteContext => new Command(DeleteContextMenu);
        public ICommand CheckedPinCommand => new Command<MapPinModel>(OnCheckedPinCommand);

        #endregion

        #region -- Private helpers --        

        private async void AddMapPinFloatingButton()
        {
            await NavigationService.NavigateAsync(nameof(AddEditMapPinView));
        }

        private async void SelectedPin(object pin)
        {
            var nav = new NavigationParameters();
            nav.Add(nameof(MapPinModel), (MapPinModel)pin);

            await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/MainListView?selectedTab=MapsPage", nav);
        }

        private async void EditContextMenu(object obj)
        {
            var nav = new NavigationParameters();
            nav.Add(nameof(MapPinModel), (MapPinModel)obj);

            await NavigationService.NavigateAsync(nameof(AddEditMapPinView), nav, false, true);
        }

        private async void DeleteContextMenu(object obj)
        {
            if (await Application.Current.MainPage.DisplayAlert("Alert", "Подтверждаете ли вы удаление?", "Ok", "Cancel"))//TODO: remove
            {
                await _mapPinService.DeleteMapPinAsync((MapPinModel)obj);

                MapPinLoad();
            }
        }

        private async void OnCheckedPinCommand(MapPinModel mapPin)
        {
            if (mapPin.Chosen)
            {
                mapPin.ImgPath = "EmptyStar.png"; //TODO: constants
                mapPin.Chosen = false;
            }
            else
            {
                mapPin.ImgPath = "FullStar.png";
                mapPin.Chosen = true;
            }

            await _mapPinService.UpdateMapPinAsync(mapPin);

            MapPinLoad();
        }

        private void MapPinLoad()
        {
            var mapPin = _mapPinService.GetMapPinListById();
            MapPin = new ObservableCollection<MapPinModel>(mapPin);
        }

        #endregion

        #region -- Overrides --

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
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
                MapPinLoad();

                if (!(SearchPin.Length == 0 || string.IsNullOrEmpty(SearchPin)))
                {
                    try //TODO: to service
                    {
                        var pin = MapPin.Where(x => x.Title.ToLower().Contains(SearchPin.ToLower()) ||
                        x.Latitude.ToString().ToLower().Contains(SearchPin.ToLower()) ||
                        x.Longitude.ToString().ToLower().Contains(SearchPin.ToLower()) ||
                        x.Description.ToLower().Contains(SearchPin.ToLower()));
                        MapPin = new ObservableCollection<MapPinModel>(pin);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                    }
                }
            }
        }

        public override void Initialize(INavigationParameters parameters)
        {
            MapPinLoad(); //TODO: rework
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            MapPinLoad();
        }

        #endregion
    }
}
