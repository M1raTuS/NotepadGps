using NotepadGps.Services.Profile;
using NotepadGps.Services.Settings;
using System.Linq;
using System.Threading.Tasks;

namespace NotepadGps.Services.Autorization
{
    public class AutorizationService : IAutorizationService
    {
        private readonly IProfileService _profile;
        private readonly ISettingsService _settingsService;

        public AutorizationService(
            IProfileService profile,
            ISettingsService settingsService)
        {
            _profile = profile;
            _settingsService = settingsService;
        }

        #region -- IAutorizationService implementation --

        public bool IsAutorized
        {
            get => _settingsService.CurrentUser != -1;
        }

        private bool isMailTrue;
        public bool IsMailTrue()
        {
            return isMailTrue;
        }

        private bool isPasswordTrue;
        public bool IsPasswordTrue()
        {
            return isPasswordTrue;
        }

        public async Task<bool> TryToAuthorizeAsync(string email, string password)
        {
            var isAuthorized = false;
            isMailTrue = false;

            var user = await _profile.GetAllUserListAsync();
            var foundUser = user.Where(x => x.Email == email && x.Password == password)?.FirstOrDefault();
            var foundEmail = user.Where(x => x.Email == email)?.FirstOrDefault();

            if (foundUser != null)
            {
                _settingsService.CurrentUser = foundUser.Id;
                isAuthorized = true;
            }

            if (foundEmail != null)
            {
                isMailTrue = true;
            }

            return isAuthorized;
        }

        public int GetAutorizedUserId
        {
            get => _settingsService.CurrentUser;
        }

        public void Unautorize()
        { 
           _settingsService.CurrentUser = -1;
        }

        #endregion
    }
}
