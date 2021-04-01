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

        public int GetCurrentUserId()
        {
            return GetCurrentId;
        }
    }
}
