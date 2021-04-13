using Xamarin.Essentials;

namespace NotepadGps.Services.Settings
{
    class SettingsService : ISettingsService
    {
        public int CurrentUser
        {
            get => Preferences.Get(nameof(CurrentUser), -1);
            set => Preferences.Set(nameof(CurrentUser), value);
        }
    }
}
