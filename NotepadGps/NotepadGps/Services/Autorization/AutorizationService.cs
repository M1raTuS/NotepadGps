using NotepadGps.ViewModel;

namespace NotepadGps.Services.Autorization
{
    public class AutorizationService : BaseViewModel, IAutorizationService
    {
        private int _getCurrentId;
        public int GetCurrentId
        {
            get => _getCurrentId;
            set => SetProperty(ref _getCurrentId, value);
        }

        private bool _isAutorized;

        public bool IsAutorized
        {
            get => _isAutorized;
            set => SetProperty(ref _isAutorized, value);
        }

        public int GetCurrentUserId()
        {
            return GetCurrentId;
        }
    }
}
