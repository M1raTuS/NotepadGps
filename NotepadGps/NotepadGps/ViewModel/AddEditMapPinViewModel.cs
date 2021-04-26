using Acr.UserDialogs;
using NotepadGps.Models;
using NotepadGps.Resource;
using NotepadGps.Services.Autentification;
using NotepadGps.Services.Autorization;
using NotepadGps.Services.Image;
using NotepadGps.Services.Map;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace NotepadGps.ViewModel
{
    public class AddEditMapPinViewModel : BaseViewModel
    {
        private const string imgString = "ic_like_blue";

        private readonly IAutorizationService _autorizationService;
        private readonly IMapPinService _mapPinService;
        private readonly IImageService _imageService;
        private readonly IAutentificationService _autentificationService;

        public AddEditMapPinViewModel(
            INavigationService navigationService,
            IAutorizationService autorizationService,
            IMapPinService mapPinService,
            IImageService imageService,
            IAutentificationService autentificationService)
            : base(navigationService)
        {
            _autorizationService = autorizationService;
            _mapPinService = mapPinService;
            _imageService = imageService;
            _autentificationService = autentificationService;

            LabelError = StringResource.LabelError;
            DescriptionError = StringResource.DescriptionError;
            LongitudeError = StringResource.LongitudeError;
            LatitudeError = StringResource.LatitudeError;
        }

        #region -- Public properties --

        private int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _label;
        public string Label
        {
            get => _label;
            set => SetProperty(ref _label, value);
        }

        private string _labelError;
        public string LabelError
        {
            get => _labelError;
            set => SetProperty(ref _labelError, value);
        }

        private string _longitude;
        public string Longitude
        {
            get => _longitude;
            set => SetProperty(ref _longitude, value);
        }

        private string _longitudeError;
        public string LongitudeError
        {
            get => _longitudeError;
            set => SetProperty(ref _longitudeError, value);
        }

        private string _latitude;
        public string Latitude
        {
            get => _latitude;
            set => SetProperty(ref _latitude, value);
        }

        private string _latitudeError;
        public string LatitudeError
        {
            get => _latitudeError;
            set => SetProperty(ref _latitudeError, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private string _descriptionError;
        public string DescriptionError
        {
            get => _descriptionError;
            set => SetProperty(ref _descriptionError, value);
        }

        private string _imagePins;
        public string ImagePins
        {
            get => _imagePins;
            set
            {
                var strArray = value.Split('/');
                SetProperty(ref _imagePins, strArray.Last());
            }
        }

        private bool _isListViewShow;
        public bool IsListViewShow
        {
            get => _isListViewShow;
            set => SetProperty(ref _isListViewShow, value);
        }

        private bool _isLabelErrorVisible;
        public bool IsLabelErrorVisible
        {
            get => _isLabelErrorVisible;
            set => SetProperty(ref _isLabelErrorVisible, value);
        }

        private bool _isDescriptionErrorVisible;
        public bool IsDescriptionErrorVisible
        {
            get => _isDescriptionErrorVisible;
            set => SetProperty(ref _isDescriptionErrorVisible, value);
        }

        private bool _isLongitudeErrorVisible;
        public bool IsLongitudeErrorVisible
        {
            get => _isLongitudeErrorVisible;
            set => SetProperty(ref _isLongitudeErrorVisible, value);
        }

        private bool _isLatitudeErrorVisible;
        public bool IsLatitudeErrorVisible
        {
            get => _isLatitudeErrorVisible;
            set => SetProperty(ref _isLatitudeErrorVisible, value);
        }

        private ObservableCollection<MapPinModel> mapPins;
        public ObservableCollection<MapPinModel> MapPins
        {
            get => mapPins;
            set => SetProperty(ref mapPins, value);
        }

        private ObservableCollection<ImageModel> _listImg = new ObservableCollection<ImageModel>();
        public ObservableCollection<ImageModel> ListImg
        {
            get => _listImg;
        }

        private ObservableCollection<ImageModel> _tempImg = new ObservableCollection<ImageModel>();
        public ObservableCollection<ImageModel> TempImg
        {
            get => _tempImg;
        }

        public ICommand AddButtonCommand => new Command(OnAddButtonCommandAsync);
        public ICommand PictureButtonCommand => new Command(OnPictureButtonCommand);
        public ICommand MapClickedCommand => new Command<Position>(OnMapClickedCommand);
        public ICommand OnClick => new Command(OnClicks);

        #endregion

        #region -- Overrides --

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue(nameof(MapPinModel), out MapPinModel mapPin))
            {
                Id = mapPin.Id;
                Label = mapPin.Title;
                Latitude = mapPin.Latitude.ToString();
                Longitude = mapPin.Longitude.ToString();
                Description = mapPin.Description;
            }
        }

        #endregion

        #region -- Private helpers --        

        private async void OnAddButtonCommandAsync()
        {
            bool isCanSave = !string.IsNullOrWhiteSpace(Label) &&
                             !string.IsNullOrWhiteSpace(Longitude) &&
                             !string.IsNullOrWhiteSpace(Latitude) &&
                             !string.IsNullOrWhiteSpace(Description);

            IsLabelErrorVisible = false;
            IsLongitudeErrorVisible = false;
            IsLatitudeErrorVisible = false;
            IsDescriptionErrorVisible = false;

            if (isCanSave)
            {
                var mapPin = new MapPinModel()
                {
                    Id = Id,
                    UserId = _autorizationService.GetAutorizedUserId,
                    Title = Label,
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
                    await AddImgAsync();
                    await NavigationService.GoBackAsync();
                }
                else
                {
                    var nav = new NavigationParameters();
                    nav.Add(nameof(MapPinModel), mapPin);

                    await _mapPinService.SaveMapPinAsync(mapPin);
                    await AddImgAsync();
                    await NavigationService.GoBackAsync();
                }
            }
            else
            {
                ErrorMessageVisible();
            }
        }

        private void ErrorMessageVisible()
        {
            if (string.IsNullOrWhiteSpace(Label))
            {
                IsLabelErrorVisible = true;
            }

            if (string.IsNullOrWhiteSpace(Longitude))
            {
                IsLongitudeErrorVisible = true;
            }

            if (string.IsNullOrWhiteSpace(Latitude))
            {
                IsLatitudeErrorVisible = true;
            }

            if (string.IsNullOrWhiteSpace(Description))
            {
                IsDescriptionErrorVisible = true;
            }
        }

        private async Task AddImgAsync()
        {
            if (ListImg?.Count > 0)
            {
                foreach (var item in ListImg)
                {
                    await _imageService.SaveMapPinAsync(item);
                }
            }
        }

        private void OnPictureButtonCommand()
        {
            bool isCanSave = !string.IsNullOrWhiteSpace(Label) &&
                             !string.IsNullOrWhiteSpace(Longitude) &&
                             !string.IsNullOrWhiteSpace(Latitude) &&
                             !string.IsNullOrWhiteSpace(Description);

            IsLabelErrorVisible = false;
            IsLongitudeErrorVisible = false;
            IsLatitudeErrorVisible = false;
            IsDescriptionErrorVisible = false;

            if (isCanSave)
            {
                var file = new ActionSheetConfig()
                .SetTitle(StringResource.ImgSourceChoose)
                .Add(StringResource.Gallery, () => OpenGalery())
                .Add(StringResource.Camera, () => OpenCamera())
                .SetCancel();

                UserDialogs.Instance.ActionSheet(file);
            }
            else
            {
                ErrorMessageVisible();
            }
        }

        private async void OpenGalery()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();

            if (status != PermissionStatus.Granted)
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage))
                {
<<<<<<< HEAD
=======
                   
>>>>>>> e6a746682176846e9d2060160bbc2195675fcd5f
                }

                status = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();

            }

            if (status == PermissionStatus.Granted)
            {
                if (CrossMedia.Current.IsPickPhotoSupported)
                {
                    MediaFile img = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions());

                    if (img != null)
                    {
                        var image = new ImageModel()
                        {
                            UserId = _autorizationService.GetAutorizedUserId,
                            Latitude = Latitude,
                            Longitude = Longitude,
                            ImagePins = img.Path
                        };

                        var imgd = image;
                        imgd.ImagePins = imgd.ImagePins.Split('/').Last();
                        ListImg.Add(imgd);
                    }
                }
            }
            else if (status != PermissionStatus.Unknown)
            {
                CrossPermissions.Current.OpenAppSettings();
            }
        }

        private async void OpenCamera()
        {
<<<<<<< HEAD
=======
            //var status = await CrossPermissions.Current.CheckPermissionStatusAsync<CameraPermission>();
>>>>>>> e6a746682176846e9d2060160bbc2195675fcd5f
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync<MediaLibraryPermission>();

            if (status != PermissionStatus.Granted)
            {

                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.MediaLibrary))
                {

                }

                status = await CrossPermissions.Current.RequestPermissionAsync<MediaLibraryPermission>();

            }

            if (status == PermissionStatus.Granted)
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
                        var image = new ImageModel()
                        {
                            UserId = _autorizationService.GetAutorizedUserId,
                            Latitude = Latitude,
                            Longitude = Longitude,
                            ImagePins = img.Path
                        };

                        var imgd = image;
                        imgd.ImagePins = imgd.ImagePins.Split('/').Last();
                        ListImg.Add(imgd);
                    }
                }
            }
            else if (status != PermissionStatus.Unknown)
            {
                CrossPermissions.Current.OpenAppSettings();
            }

        }

        private void OnMapClickedCommand(Position position)
        {
            Latitude = "";
            Longitude = "";

            MapPinModel Pin = new MapPinModel
            {
                Title = "test",
                Latitude = position.Latitude,
                Longitude = position.Longitude,
                IsChosen = true
            };

            MapPins = new ObservableCollection<MapPinModel> { Pin };

            Latitude = string.Format("{0:f8}", position.Latitude);
            Longitude = string.Format("{0:f8}", position.Longitude);
        }

        private void OnClicks(object obj)
        {
            ListImg.Remove((ImageModel)obj);
        }

        #endregion

    }
}
