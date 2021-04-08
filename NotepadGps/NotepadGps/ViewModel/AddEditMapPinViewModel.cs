using Acr.UserDialogs;
using NotepadGps.Models;
using NotepadGps.Services.Autorization;
using NotepadGps.Services.Profile;
using Prism.Navigation;
using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace NotepadGps.ViewModel
{
    public class AddEditMapPinViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IAutorizationService _autorization;
        private readonly IProfileService _profile;

        public AddEditMapPinViewModel(INavigationService navigationService,
                                      IAutorizationService autorization,
                                      IProfileService profile)
        {
            _navigationService = navigationService;
            _autorization = autorization;
            _profile = profile;
        }

        #region -Public Properties-

        public ICommand AddButtonCommand => new Command(SaveCommand);
        public ICommand MapClickedCommand => new Command<Position>(MapClicked);

       

        private int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _longitude;
        public string Longitude
        {
            get => _longitude;
            set => SetProperty(ref _longitude, value);
        }

        private string _latitude;
        public string Latitude
        {
            get => _latitude;
            set => SetProperty(ref _latitude, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);

        }

        #endregion

        #region -Methods-

        private async void SaveCommand()
        {
            try
            {
                if (CanSave())
                {
                    var mapPin = new MapPinModel()
                    {
                        Id = Id,
                        UserId = _autorization.GetCurrentId,
                        Title = Title,
                        Longitude = Convert.ToDouble(Longitude),
                        Latitude = Convert.ToDouble(Latitude),
                        Description = Description
                    };

                    await _profile.SaveMapPinAsync(mapPin);
                    await _navigationService.GoBackAsync();

                }
                else
                {
                    UserDialogs.Instance.Alert("Заполните поля Title, Longitude и Latitude", "Alert", "Ok");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private bool CanSave()
        {
            if (!String.IsNullOrEmpty(Title) && !String.IsNullOrEmpty(Longitude) && !String.IsNullOrEmpty(Latitude))
            {
                return true;
            }
            return false;
        }

        private void MapClicked(Position position)
        {
            Latitude = "";
            Longitude = "";

            Latitude = position.Latitude.ToString();
            Longitude = position.Longitude.ToString();
        }

        #endregion

        #region -Overrides-
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.TryGetValue(nameof(MapPinModel), out MapPinModel mapPin))
            {
                //mapPinModel = mapPin;
            }
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
        }
        #endregion
        
    }
}
