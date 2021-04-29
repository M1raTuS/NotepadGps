using Xamarin.Essentials;

namespace NotepadGps.Services.Settings
{
    public class SettingsService : ISettingsService
    {
        #region -- ISettingsService implementation --

        public int CurrentUser
        {
            get => Preferences.Get(nameof(CurrentUser), -1);
            set => Preferences.Set(nameof(CurrentUser), value);
        }

        #endregion
    }
}
