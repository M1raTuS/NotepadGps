using NotepadGps.Models;
using Prism.Navigation;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotepadGps.ViewModel
{
    public class PhotoCheckViewModel : BaseViewModel
    {
        public PhotoCheckViewModel(
            INavigationService navigationService)
            : base(navigationService)
        {
        }

        #region -- Public properties --

        private string _imageModel;
        public string ImageModel
        {
            get => _imageModel;
            set => SetProperty(ref _imageModel, value);
        }

        public ICommand BackCommand => new Command(OnBackCommandAsync);

        #endregion

        #region -- Overrides --

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            if (parameters.TryGetValue(nameof(ImageModel), out ImageModel img))
            {
                ImageModel = img.ImagePins;
            }
        }

        #endregion

        #region -- Private helpers --

        private void OnBackCommandAsync()
        {
            NavigationService.GoBackAsync();
        }

        #endregion
    }
}
