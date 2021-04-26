using NotepadGps.Resource.Style;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NotepadGps.Services.Settings
{
    class SettingsService : ISettingsService
    {
        public int CurrentUser
        {
            get => Preferences.Get(nameof(CurrentUser), -1);
            set => Preferences.Set(nameof(CurrentUser), value);
        }

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

                if (CurrentUser != -1)
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
    }
}
