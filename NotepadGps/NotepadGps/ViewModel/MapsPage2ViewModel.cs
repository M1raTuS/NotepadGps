using NotepadGps.Models;
using NotepadGps.Services.Profile;
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
    public class MapsPage2ViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IProfileService _profile;

        public MapsPage2ViewModel(INavigationService navigationService,
                                 IProfileService profile)
        {
            _navigationService = navigationService;
            _profile = profile;

            Load();
        }

        #region -Public properties-

        private ObservableCollection<MapPinModel> _mapPins;
        public ObservableCollection<MapPinModel> MapPins
        {
            get => _mapPins;
            set => SetProperty(ref _mapPins, value);
        }

        private ObservableCollection<MapPinModel> _selectedItem;
        public ObservableCollection<MapPinModel> SelectedItem
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

        private bool _listEnabled;
        public bool ListEnabled
        {
            get => _listEnabled;
            set => SetProperty(ref _listEnabled, value);
        }

        private string _searchPin;
        public string SearchPin
        {
            get => _searchPin;
            set => SetProperty(ref _searchPin, value);
        }

        public ICommand AddMapPinFloatingButtonCommand => new Command(AddMapPinFloatingButton);
        public ICommand FindPinCommand => new Command(FindPin);


        #endregion

        #region -Methods-

        private async void AddMapPinFloatingButton()
        {
            await _navigationService.NavigateAsync(nameof(AddEditMapPinView));
        }

        private void FindPin()
        {
            if (SelectedItem == null)
            {
                SelectedItem = new ObservableCollection<MapPinModel>(MapPin);
            }
            else
            {
                MapPin = new ObservableCollection<MapPinModel>(SelectedItem);
            }
            
            try
            {
                var pin = MapPin.Where(x => x.Latitude.ToString() == SearchPin || x.Longitude.ToString() == SearchPin || x.Description == SearchPin);
                MapPin = new ObservableCollection<MapPinModel>(pin);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);   
            }
            
        }

        private void Load()
        {
            var mapPin = _profile.GetMapPinListById();
            MapPin = new ObservableCollection<MapPinModel>(mapPin);
        }


        #endregion

        #region -Overrides-
        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            if (args.PropertyName == nameof(MapPin))
            {
                if (MapPin.Count > 0)
                {
                    LabelVisible = false;
                    ListEnabled = true;
                }
                else
                {
                    LabelVisible = true;
                    ListEnabled = false;
                }
            }
            if (args.PropertyName == nameof(SearchPin))
            {
                if (SearchPin.Length == 0)
                {
                    Load();
                }
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            Load();
        }

        #endregion
    }
}
