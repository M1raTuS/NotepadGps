using Xamarin.Forms.GoogleMaps;

namespace NotepadGps.Services.Theme
{
    public interface IThemeService
    {
        int SelectedTheme { get; set; }
        void LoadTheme();
        MapStyle ShowMapTheme(string fileName);
    }
}
