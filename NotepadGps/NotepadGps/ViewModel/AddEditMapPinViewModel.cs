using Acr.UserDialogs;
using NotepadGps.Models;
using NotepadGps.Services.Autorization;
using NotepadGps.Services.Profile;
using NotepadGps.Services.Repository;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

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

        private bool _chosen;
        public bool Chosen
        {
            get => _chosen;
            set => SetProperty(ref _chosen, value);

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
                        Chosen = Chosen,
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
        public ICommand OnMapClicked => new Command(MapClicked);

        private async void MapClicked()
        {

            var location = await Geolocation.GetLastKnownLocationAsync();
            if (location == null)
            {
                location = await Geolocation.GetLocationAsync(new GeolocationRequest
            {
                DesiredAccuracy=GeolocationAccuracy.Medium,
                Timeout = TimeSpan.FromSeconds(30)
            });
            }

           var fg = location.Latitude;
           var jh = location.Longitude;


            Position pos = new Position();
            Xamarin.Forms.Maps.Map m = new Xamarin.Forms.Maps.Map();
           var td = m.X;
           var q = pos.Latitude;
           var t = pos.Longitude;
        }
        public class TapEventArgs : EventArgs
        {
            public Position Position { get; set; }
        }
    }
}
