using Acr.UserDialogs;
using NotepadGps.Models;
using NotepadGps.Resource;
using NotepadGps.Services.Autorization;
using NotepadGps.Services.Image;
using NotepadGps.View;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace NotepadGps.ViewModel
{
    public class PopUpViewModel : BaseViewModel
    {
        private readonly IImageService _imageService;
        private readonly IAutorizationService _autorizationService;

        public PopUpViewModel(
            INavigationService navigationService,
            IAutorizationService autorizationService,
            IImageService imageService)
            : base(navigationService)
        {
            _autorizationService = autorizationService;
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

        private bool _isImgVisible;
        public bool IsImgVisible
        {
            get => _isImgVisible;
            set => SetProperty(ref _isImgVisible, value);
        }

        private bool _isCarouselVisible;
        public bool IsCarouselVisible
        {
            get => _isCarouselVisible;
            set => SetProperty(ref _isCarouselVisible, value);
        }

        public ICommand TapCommand => new Command(OnTapCommandAsync);
        public ICommand ImageCommand => new Command(OnImageCommand); 
        public ICommand NotifyCommand => new Command<Pin>(OnNotifyCommand); 

        #endregion

        #region -- Overrides --

        public async override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            if (parameters.TryGetValue(nameof(Pin), out Pin pin))
            {
                Title = pin.Label;
                Latitude = pin.Position.Latitude.ToString();
                Longitude = pin.Position.Longitude.ToString();
                Description = pin.Address;
            }

            await ImageLoadAsync();

            SearchImgAsync();
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(ImageList))
            {
                if (ImageList.Count < 1 || ImageList == null)
                {
                    IsImgVisible = true;
                    IsCarouselVisible = false;
                }
                else
                {
                    IsImgVisible = false;
                    IsCarouselVisible = true;
                }
            }
        }

        #endregion

        #region -- Private helpers --

        private async void OnTapCommandAsync()
        {
            await NavigationService.GoBackAsync(new NavigationParameters(), true, true);
        }

        private async Task<ObservableCollection<ImageModel>> ImageLoadAsync()
        {
            var image = await _imageService.GetImageListByIdAsync(_autorizationService.GetAutorizedUserId);
            ImageList = new ObservableCollection<ImageModel>(image);
            return ImageList;
        }

        private void OnImageCommand()
        {
            var file = new ActionSheetConfig()
                .SetTitle(StringResource.ImgSourceChoose)
                .Add(StringResource.Camera, () => OpenCamera())
                .Add(StringResource.Gallery, () => OpenGalery())
                .SetCancel();

            UserDialogs.Instance.ActionSheet(file); //TODO: move to baseViewModel
        }

        private async void OnNotifyCommand(Pin pin)
        {
            await NavigationService.NavigateAsync(nameof(NotifyPageView));
        }

        private async void OpenGalery()
        {
            if (CrossMedia.Current.IsPickPhotoSupported) //TODO: move to service
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

                    await _imageService.SaveMapPinAsync(image);
                }

                await ImageLoadAsync();

                SearchImgAsync();
            }
        }

        private async void OpenCamera()
        {
            if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsPickPhotoSupported) //TODO: move t service
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

                    await _imageService.SaveMapPinAsync(image);
                }

                await ImageLoadAsync();
                SearchImgAsync();
            }
        }

        private async void SearchImgAsync()
        {
            if (ImageList != null)
            {
                var img = await _imageService.FindImgAsync(Latitude, Longitude);
                ImageList = new ObservableCollection<ImageModel>(img);
            }
        }

        #endregion
    }
}
