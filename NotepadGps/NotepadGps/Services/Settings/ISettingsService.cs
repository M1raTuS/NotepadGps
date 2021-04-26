using NotepadGps.Enum;

namespace NotepadGps.Services.Settings
{
    public interface ISettingsService
    {
        int CurrentUser { get; set; }
        int SelectedTheme { get; set; }
        void LoadTheme();
    }
}
