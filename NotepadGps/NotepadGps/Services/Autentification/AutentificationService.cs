using NotepadGps.Services.Autorization;
using NotepadGps.Services.Profile;
using NotepadGps.Services.Settings;
using System;
using System.Linq;

namespace NotepadGps.Services.Autentification
{
    public class AutentificationService : IAutentificationService
    {
        private readonly IProfileService _profile;
        private readonly IAutorizationService _autorization;
        private readonly ISettingsService _settings;

        public AutentificationService(IProfileService profile,
                                      IAutorizationService autorization,
                                      ISettingsService settings)
        {
            _profile = profile;
            _autorization = autorization;
            _settings = settings;
        }
        public void Authorizate(string email, string password)
        {
            _autorization.IsAutorized = false;

            var user = _profile.GetAllUserList();
            user = user.Where(x => x.Email == email && x.Password == password).ToList();
            
            if (user != null && user.Count > 0)
            {
                foreach (var item in user)
                {
                    _autorization.GetCurrentId =  item.Id;
                }
                
                _settings.CurrentUser = _autorization.GetCurrentId;
                _autorization.IsAutorized= true;
            }
        }
    }
}
