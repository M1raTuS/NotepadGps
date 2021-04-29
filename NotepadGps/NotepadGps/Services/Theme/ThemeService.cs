using NotepadGps.Resource.Style;
using NotepadGps.Services.Settings;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace NotepadGps.Services.Theme
{
    public class ThemeService : IThemeService
    {
        private readonly ISettingsService _settingsService;

        public ThemeService(
            ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        #region -- IThemeService implementation --

        public int SelectedTheme
        {
            get => Preferences.Get(nameof(SelectedTheme), 0);
            set => Preferences.Set(nameof(SelectedTheme), value);
        }

        public void LoadTheme()
        {
            SelectTheme();
        }

        private void SelectTheme()
        {
            ICollection<ResourceDictionary> dictionary = Application.Current.Resources.MergedDictionaries;

            if (dictionary != null)
            {
                dictionary.Clear();

                if (_settingsService.CurrentUser != -1)
                {
                    switch (SelectedTheme)
                    {
                        case 1:
                            dictionary.Add(new DarkTheme());
                            break;

                        case 0:
                        default:
                            dictionary.Add(new LightTheme());
                            break;
                    }
                }
                else
                {
                    SelectedTheme = 0;
                }
            }
        }

        public MapStyle ShowMapTheme(string fileName)
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream(fileName);
            string styleFile;

            using (var reader = new StreamReader(stream))
            {
                styleFile = reader.ReadToEnd();
            }

            MapStyle mapStyle = null;
            mapStyle = MapStyle.FromJson(styleFile);

            return mapStyle;
        }

        #endregion
    }
}
