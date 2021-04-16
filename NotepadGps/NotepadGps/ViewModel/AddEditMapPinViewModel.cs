using Acr.UserDialogs;
using NotepadGps.Models;
using NotepadGps.Services.Autentification;
using NotepadGps.Services.Autorization;
using NotepadGps.Services.Map;
using Prism.Navigation;
using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace NotepadGps.ViewModel
{
    public class AddEditMapPinViewModel : BaseViewModel
    {
        private readonly IAutorizationService _autorization;
        private readonly IMapPinService _mapPinService;
        private readonly IAutentificationService _autentification;

        public AddEditMapPinViewModel(
            INavigationService navigationService,
            IAutorizationService autorization,
            IMapPinService mapPinService,
            IAutentificationService autentification)
            : base (navigationService)
        {
            _autorization = autorization;
            _mapPinService = mapPinService;
            _autentification = autentification;
        }

        #region -- Public properties --

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

        public ICommand AddButtonCommand => new Command(SaveCommand);//TODO: ONCommandNameAsync
        public ICommand MapClickedCommand => new Command<Position>(OnMapClickedCommand);

        #endregion

        #region -- Private helpers --        

        private async void SaveCommand()
        {
            try
            {
                if (CanSave())//TODO: move up
                {
                    var mapPin = new MapPinModel()
                    {
                        Id = Id,
                        UserId = _autentification.GetCurrentId, //TODO: rework
                        Title = Title,
                        Longitude = Convert.ToDouble(Longitude),
                        Latitude = Convert.ToDouble(Latitude),
                        Description = Description,
                        Chosen = true,
                        ImgPath = "FullStar.png"
                    };
                    if (Id > 0)
                    {
                        var nav = new NavigationParameters();
                        nav.Add(nameof(MapPinModel), mapPin);

                        await _mapPinService.UpdateMapPinAsync(mapPin);
                        await NavigationService.GoBackAsync();
                    }
                    else
                    {
                        var nav = new NavigationParameters();
                        nav.Add(nameof(MapPinModel), mapPin);

                        await _mapPinService.SaveMapPinAsync(mapPin);
                        await NavigationService.GoBackAsync();
                    }
                }
                else
                {
                    UserDialogs.Instance.Alert("Заполните поля Title, Longitude, Latitude и Description", "Alert", "Ok");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private bool CanSave()
        {
            return !string.IsNullOrEmpty(Title) && !String.IsNullOrEmpty(Longitude) && !String.IsNullOrEmpty(Latitude) && !String.IsNullOrEmpty(Description);
        }

        private void OnMapClickedCommand(Position position)
        {
            Latitude = "";
            Longitude = "";

            Latitude = position.Latitude.ToString();
            Longitude = position.Longitude.ToString();
        }

        #endregion

        #region -- Overrides --

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue(nameof(MapPinModel), out MapPinModel mapPin))
            {
                Id = mapPin.Id;
                Title = mapPin.Title;
                Latitude = mapPin.Latitude.ToString();
                Longitude = mapPin.Longitude.ToString();
                Description = mapPin.Description;
            }
        }

        #endregion
    }
}
