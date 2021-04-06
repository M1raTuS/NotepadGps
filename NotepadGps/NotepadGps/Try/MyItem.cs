using NotepadGps.Try;
using System;
using Xamarin.Forms.Maps;

namespace NotepadGps
{
    public class MyItem : IClusterItem
    {
        private Position LatLng;
        private string title;
        private string description;

        public MyItem(double lat, double lng, string title, string description)
        {
            LatLng = new Position(lat, lng);
            this.title = title;
            this.description = description;
        }


        public  Position getPosition()
        {
            return LatLng;
        }


        public  String getTitle()
        {
            return title;
        }


        public  string getDescription()
        {
            return description;
        }
    }
}
