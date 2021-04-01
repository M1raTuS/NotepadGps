using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace NotepadGps.ViewModel
{
    public class MapsPage1ViewModel : BaseViewModel
    {
        public ICommand OnMapClicked => new Command(MapClicked);


        private void MapClicked()
        {
            Map map = new Map();
            map.MapClicked += OnMapClickeded;
        }
        void OnMapClickeded(object sender, MapClickedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"MapClick: {e.Position.Latitude}, {e.Position.Longitude}");
        }

        
    }
}
