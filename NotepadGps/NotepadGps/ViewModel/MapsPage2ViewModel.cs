using NotepadGps.Models;
using NotepadGps.Services.Profile;
using NotepadGps.View;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

        private MapPinModel _selectedItem;
        public MapPinModel SelectedItem
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

        public ICommand AddMapPinFloatingButtonCommand => new Command(AddMapPinFloatingButton);

        #endregion

        #region -Methods-

        private async void AddMapPinFloatingButton()
        {
            await _navigationService.NavigateAsync(nameof(AddEditMapPinView));
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
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue(nameof(UserModel), out UserModel user))
            {
                Load();
            }
        }

        #endregion
    }
}
