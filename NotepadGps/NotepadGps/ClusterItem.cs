using Xamarin.Forms.GoogleMaps;

namespace NotepadGps.ViewModel
{
    public class ClusterItem
    {
        public Position Position { get; set; }

        public ClusterItem(double lat, double lng)
        {
            Position = new Position(lat, lng);
        }
    }
}