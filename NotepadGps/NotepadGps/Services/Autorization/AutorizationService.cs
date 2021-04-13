using NotepadGps.Services.Autentification;
using NotepadGps.Services.Profile;
using NotepadGps.Services.Settings;
using NotepadGps.ViewModel;
using System.Linq;

namespace NotepadGps.Services.Autorization
{
    public class AutorizationService : BaseViewModel, IAutorizationService
    {

        private readonly IProfileService _profile;
        private readonly IAutentificationService _autentification;
        private readonly ISettingsService _settings;

        public AutorizationService(IProfileService profile,
                                      ISettingsService settings,
                                      IAutentificationService autentification)
        {
            _profile = profile;
            _settings = settings;
            _autentification = autentification;

            _autentification.GetCurrentId = _settings.CurrentUser;
        }

        private bool _isAutorized;
        public bool IsAutorized
        {
            get => _isAutorized;
            set => SetProperty(ref _isAutorized, value);
        }

        public void Authorizate(string email, string password)
        {
            IsAutorized = false;

            var user = _profile.GetAllUserList();
            user = user.Where(x => x.Email == email && x.Password == password).ToList();

            if (user != null && user.Count > 0)
            {
                foreach (var item in user)
                {
                    _autentification.GetCurrentId = item.Id;
                }

                _settings.CurrentUser = _autentification.GetCurrentId;
                IsAutorized = true;
            }
        }
    }
}
