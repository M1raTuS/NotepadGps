using NotepadGps.Models;
using System.Collections;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace NotepadGps.Controls
{
    public class CustomMap : Map
    {

        public CustomMap()
        {
            PinsSource = new ObservableCollection<MapPinModel>();
        }

        #region -- Public properties --

        public ObservableCollection<MapPinModel> PinsSource
        {
            get => (ObservableCollection<MapPinModel>)GetValue(PinsSourceProperty);
            set => SetValue(PinsSourceProperty, value);
        }

        public static readonly BindableProperty PinsSourceProperty = BindableProperty.Create(
                                                                     propertyName: nameof(PinsSource),
                                                                     returnType: typeof(ObservableCollection<MapPinModel>),
                                                                     declaringType: typeof(CustomMap),
                                                                     propertyChanged: PinsSourcePropertyChanged);

        public static readonly BindableProperty MapSpanProperty = BindableProperty.Create(
                                                                  propertyName: nameof(MapSpan),
                                                                  returnType: typeof(MapSpan),
                                                                  declaringType: typeof(CustomMap),
                                                                  propertyChanged: MapSpanPropertyChanged);

        public static BindableProperty CurrentCameraPositionProperty = BindableProperty.Create(
                                                                       propertyName: nameof(CurrentCameraPosition),
                                                                       returnType: typeof(CameraPosition),
                                                                       declaringType: typeof(CustomMap),
                                                                       propertyChanged: CurrentCameraPositionPropertyChanged);

        public MapSpan MapSpan
        {
            get { return (MapSpan)GetValue(MapSpanProperty); }
            set { SetValue(MapSpanProperty, value); }
        }

        public CameraPosition CurrentCameraPosition
        {
            get { return (CameraPosition)GetValue(CurrentCameraPositionProperty); }
            set { SetValue(CurrentCameraPositionProperty, value); }
        }

        private static void CurrentCameraPositionPropertyChanged(BindableObject bindable, object oldvalue, object newValue)
        {
            if (newValue != null)
            {
                CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition((CameraPosition)newValue);

                if (bindable is CustomMap map)
                {
                    map.InitialCameraUpdate = cameraUpdate;
                }

                ((CustomMap)bindable).MoveCamera(cameraUpdate);
            }
        }

        #endregion

        #region -- Private properties --

        private static void PinsSourcePropertyChanged(BindableObject bindable, object oldvalue, object newValue)
        {
            var bindableMap = (CustomMap)bindable;
            var newPinsSource = (IEnumerable)newValue;

            if (bindableMap != null && newPinsSource != null)
            {
                bindableMap.Pins.Clear();
                foreach (MapPinModel pin in newPinsSource)
                {

                    var pins = new Pin
                    {
                        Label = pin.Title,
                        Address = pin.Description,
                        Type = PinType.SearchResult,
                        Position = new Position(pin.Latitude, pin.Longitude),
                        IsVisible = pin.IsChosen
                    };

                    bindableMap.Pins.Add(pins);
                }
            }
        }

        private static void MapSpanPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var thisInstance = (CustomMap)bindable;
            var newMapSpan = (MapSpan)newValue;

            thisInstance?.MoveToRegion(newMapSpan);
        }

        #endregion

    }
}

