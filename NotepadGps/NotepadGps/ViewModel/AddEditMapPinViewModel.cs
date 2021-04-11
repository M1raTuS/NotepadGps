using Acr.UserDialogs;
using NotepadGps.Models;
using NotepadGps.Services.Autorization;
using NotepadGps.Services.Map;
using Prism.Navigation;
using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace NotepadGps.ViewModel
{
    public class AddEditMapPinViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IAutorizationService _autorization;
        private readonly IMapPinService _mapPinService;

        public AddEditMapPinViewModel(INavigationService navigationService,
                                      IAutorizationService autorization,
                                      IMapPinService mapPinService)
        {
            _navigationService = navigationService;
            _autorization = autorization;
            _mapPinService = mapPinService;
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
                        Description = Description,
                        IsFavorite = true,
                        ImgPath = "FullStar.jpg"
                    };
                    if (Id > 0)
                    {
                        var nav = new NavigationParameters();
                        nav.Add(nameof(MapPinModel), mapPin);

                        await _mapPinService.UpdateMapPinAsync(mapPin);
                        await _navigationService.GoBackAsync();
                    }
                    else
                    {
                        var nav = new NavigationParameters();
                        nav.Add(nameof(MapPinModel), mapPin);

                        await _mapPinService.SaveMapPinAsync(mapPin);
                        await _navigationService.GoBackAsync();
                    }

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

        #region --Overrides--

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
