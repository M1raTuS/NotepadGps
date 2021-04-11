using NotepadGps.Services.Settings;
using NotepadGps.ViewModel;

namespace NotepadGps.Services.Autorization
{
    public class AutorizationService : BaseViewModel, IAutorizationService
    {
        private readonly ISettingsService _settings;
        public AutorizationService(ISettingsService settings)
        {
            _settings = settings;

            _getCurrentId = _settings.CurrentUser;
        }
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
    }
}
