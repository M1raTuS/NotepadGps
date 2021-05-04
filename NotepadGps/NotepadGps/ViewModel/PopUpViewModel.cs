using NotepadGps.Models;
using NotepadGps.Services.Autorization;
using NotepadGps.Services.Image;
using NotepadGps.View;
using Prism.Navigation;
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

        private int _frameHeight;
        public int FrameHeight
        {
            get => _frameHeight;
            set => SetProperty(ref _frameHeight, value);
        }

        public ICommand TapCommand => new Command(OnTapCommandAsync);
        public ICommand ImageTappedCommand => new Command(OnImageTappedCommandAsync);

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
                if (ImageList?.Count < 1)
                {
                    IsCarouselVisible = false;
                    FrameHeight = 150;
                }
                else
                {
                    IsCarouselVisible = true;
                    FrameHeight = 250;
                }
            }
        }

        #endregion

        #region -- Private helpers --

        private async void OnTapCommandAsync()
        {
            await NavigationService.GoBackAsync(new NavigationParameters(), true, false);
        }

        private async void OnImageTappedCommandAsync()
        {
            var nav = new NavigationParameters();
            //nav.Add(nameof(ImageModel), (ImageModel)img);

            await NavigationService.NavigateAsync(nameof(PhotoCheckView), nav, false, false);
        }

        private async Task<ObservableCollection<ImageModel>> ImageLoadAsync()
        {
            var image = await _imageService.GetImageListByIdAsync(_autorizationService.GetAutorizedUserId);
            ImageList = new ObservableCollection<ImageModel>(image);
            return ImageList;
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
