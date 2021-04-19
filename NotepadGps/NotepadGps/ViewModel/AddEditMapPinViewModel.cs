using Acr.UserDialogs;
using NotepadGps.Models;
using NotepadGps.Resource;
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
        private const string imgString = "FullStar.png";

        private readonly IAutorizationService _autorization;
        private readonly IMapPinService _mapPinService;
        private readonly IAutentificationService _autentification;

        public AddEditMapPinViewModel(
            INavigationService navigationService,
            IAutorizationService autorization,
            IMapPinService mapPinService,
            IAutentificationService autentification)
            : base(navigationService)
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

        public ICommand AddButtonCommand => new Command(OnAddButtonCommandAsync);
        public ICommand MapClickedCommand => new Command<Position>(OnMapClickedCommand);

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

        #region -- Private helpers --        

        private async void OnAddButtonCommandAsync()
        {
            bool isCanSave = !string.IsNullOrWhiteSpace(Title) && !string.IsNullOrWhiteSpace(Longitude) && !string.IsNullOrWhiteSpace(Latitude) && !string.IsNullOrWhiteSpace(Description);

            if (isCanSave)
            {
                var mapPin = new MapPinModel()
                {
                    Id = Id,
                    UserId = _autorization.GetAutorizedUserId,
                    Title = Title,
                    Longitude = Convert.ToDouble(Longitude),
                    Latitude = Convert.ToDouble(Latitude),
                    Description = Description,
                    IsChosen = true,
                    ImgPath = imgString
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
                UserDialogs.Instance.Alert(StringResource.FieldsAlert, StringResource.Alert, StringResource.Ok);
            }
        }

        private void OnMapClickedCommand(Position position)
        {
            Latitude = "";
            Longitude = "";

            Latitude = position.Latitude.ToString();
            Longitude = position.Longitude.ToString();
        }

        #endregion

    }
}
