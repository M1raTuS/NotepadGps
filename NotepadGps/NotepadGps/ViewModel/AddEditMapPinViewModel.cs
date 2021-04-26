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
using System.Collections.ObjectModel;
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

        private bool _isListViewShow;
        public bool IsListViewShow
        {
            get => _isListViewShow;
            set => SetProperty(ref _isListViewShow, value);

        }

        private ObservableCollection<MapPinModel> mapPins;
        public ObservableCollection<MapPinModel> MapPins
        {
            get => mapPins;
            set => SetProperty(ref mapPins, value);
        }

        private ObservableCollection<ImageModel> _listImg;
        public ObservableCollection<ImageModel> ListImg
        {
            get => _listImg;
            set => SetProperty(ref _listImg, value);
        }

        public ICommand AddButtonCommand => new Command(OnAddButtonCommandAsync);
        public ICommand PictureButtonCommand => new Command(OnPictureButtonCommand);
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
            bool isCanSave = !string.IsNullOrWhiteSpace(Title) && !string.IsNullOrWhiteSpace(Longitude)
                                                               && !string.IsNullOrWhiteSpace(Latitude)
                                                               && !string.IsNullOrWhiteSpace(Description);

            if (isCanSave)
            {
                var mapPin = new MapPinModel()
                {
                    Id = Id,
                    UserId = _autorizationService.GetAutorizedUserId,
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
                UserDialogs.Instance.Alert(StringResource.FieldsAlert, StringResource.Alert, StringResource.Ok);
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
            bool isCanSave = !string.IsNullOrWhiteSpace(Title) && !string.IsNullOrWhiteSpace(Longitude)
                                                                  && !string.IsNullOrWhiteSpace(Latitude)
                                                                  && !string.IsNullOrWhiteSpace(Description);

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
                UserDialogs.Instance.Alert(StringResource.FieldsAlert, StringResource.Alert, StringResource.Ok);
            }
        }

        private async void OpenGalery()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();

            if (status != PermissionStatus.Granted)
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage))
                {
                   
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

                        ListImg = new ObservableCollection<ImageModel> { image };
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
            //var status = await CrossPermissions.Current.CheckPermissionStatusAsync<CameraPermission>();
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync<MediaLibraryPermission>();

            if (status != PermissionStatus.Granted)
            {

                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.MediaLibrary))
                {
                    UserDialogs.Instance.Alert(StringResource.MediaAlert, StringResource.Alert, StringResource.Ok);
                }

                status = await CrossPermissions.Current.RequestPermissionAsync<CameraPermission>();

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

                        ListImg = new ObservableCollection<ImageModel> { image };
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

        #endregion

    }
}
