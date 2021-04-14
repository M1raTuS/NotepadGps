using Acr.UserDialogs;
using NotepadGps.Models;
using NotepadGps.Services.Autentification;
using NotepadGps.Services.Image;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace NotepadGps.ViewModel
{
    public class PopUpViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IImageService _imageService;
        private readonly IAutentificationService _autentification;

        public PopUpViewModel(INavigationService navigationService,
                              IAutentificationService autentification,
                              IImageService imageService)
        {
            _navigationService = navigationService;
            _autentification = autentification;
            _imageService = imageService;
        }

        #region -- Public properties --

        private ObservableCollection<ImageModel> _imageList;
        public ObservableCollection<ImageModel> ImageList
        {
            get => _imageList;
            set => SetProperty(ref _imageList, value);
        }

        private string _id;
        public string Id
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

        private string _latitude;
        public string Latitude
        {
            get => _latitude;
            set => SetProperty(ref _latitude, value);
        }

        private string _longitude;
        public string Longitude
        {
            get => _longitude;
            set => SetProperty(ref _longitude, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private string _imagePins;
        public string ImagePins
        {
            get => _imagePins;
            set => SetProperty(ref _imagePins, value);
        }

        public ICommand TapCommand => new Command(TapCommands);
        public ICommand ImageCommand => new Command(OnImageCommand);

        #endregion

        #region -- Private helpers --

        private void TapCommands()
        {
            NavigationParameters nav = new NavigationParameters();
            _navigationService.GoBackAsync(nav, true, true);
        }

        private void ImageLoad()
        {
            var image = _imageService.GetImageListById();
            ImageList = new ObservableCollection<ImageModel>(image);
        }

        private void OnImageCommand()
        {
            var file = new ActionSheetConfig()
                .SetTitle("Choose your action")
                .Add("Camera", () => OpenCamera())
                .Add("Gallery", () => OpenGalery())
                .SetCancel();

            UserDialogs.Instance.ActionSheet(file);
        }

        private async void OpenGalery()
        {
            try
            {
                if (CrossMedia.Current.IsPickPhotoSupported)
                {
                    MediaFile img = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions());
                    if (img != null)
                    {
                        var image = new ImageModel()
                        {
                            UserId = _autentification.GetCurrentId,
                            Latitude = Latitude,
                            Longitude = Longitude,
                            ImagePins = img.Path
                        };


                        await _imageService.SaveMapPinAsync(image);
                    }
                    ImageLoad();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private async void OpenCamera()
        {
            try
            {
                if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsPickPhotoSupported)
                {
                    MediaFile img = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        PhotoSize = PhotoSize.Medium,
                        SaveToAlbum = true,
                        SaveMetaData = true,
                        Directory = "temp",
                        MaxWidthHeight = 1500,
                        CompressionQuality = 75,
                        RotateImage = Device.RuntimePlatform == Device.Android ? true : false,
                        Name = $"{DateTime.Now}.jpg"
                    });
                    if (img != null)
                    {
                        ImagePins = img.Path;
                    }

                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        #endregion

        #region -- Overrides --

        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters.TryGetValue(nameof(Pin), out Pin pin))
            {
                Title = pin.Label;
                Latitude = pin.Position.Latitude.ToString();
                Longitude = pin.Position.Longitude.ToString();
                Description = pin.Address is null ? "{null}" : pin.Address;
            }

            ImageLoad();

            try
            {
                var img = ImageList.Where(x => x.Latitude.ToString() == Latitude.ToString() &&
                x.Longitude.ToString() == Longitude.ToString());

                ImageList = new ObservableCollection<ImageModel>(img);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        #endregion
    }
}
