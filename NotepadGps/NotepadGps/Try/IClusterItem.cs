using Xamarin.Forms.GoogleMaps;

namespace NotepadGps.Try
{
    interface IClusterItem
    {
        Position getPosition();


        string getTitle();


        string getDescription();
    }
}

