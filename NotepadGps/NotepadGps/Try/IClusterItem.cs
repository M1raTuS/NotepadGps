using Xamarin.Forms.Maps;

namespace NotepadGps.Try
{
    interface IClusterItem
    {
        Position getPosition();


        string getTitle();


        string getDescription();
    }
}

