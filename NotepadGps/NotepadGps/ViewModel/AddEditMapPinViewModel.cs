using Acr.UserDialogs;
using NotepadGps.Models;
using NotepadGps.Services.Autorization;
using NotepadGps.Services.Profile;
using NotepadGps.Services.Repository;
using Prism.Navigation;
using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

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

        #region -Property-

        public ICommand AddButtonCommand => new Command(SaveCommand);


        private MapPinModel _mapPinModel;
        public MapPinModel mapPinModel
        {
            get => _mapPinModel;
            set => SetProperty(ref _mapPinModel, value);
        }
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

        private double _longitude;

        public double Longitude
        {
            get => _longitude;
            set => SetProperty(ref _longitude, value);
        }

        private double _latitude;

        public double Latitude
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
                        Longitude = Longitude,
                        Latitude = Latitude,
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
            if (!String.IsNullOrEmpty(Title) && !Double.IsNaN(Longitude) && !Double.IsNaN(Latitude))
            {
                return true;
            }
            return false;
        }

        #endregion

        #region -Overrides-
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.TryGetValue(nameof(MapPinModel), out MapPinModel mapPin))
            {
                mapPinModel = mapPin;
            }
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
        }
        #endregion
    }
}
