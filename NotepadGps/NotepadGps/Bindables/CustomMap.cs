using NotepadGps.Models;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace NotepadGps.Bindables
{
    public class CustomMap : Map
    {

        public CustomMap()
        {
            PinsSource = new ObservableCollection<MapPinModel>();
            PinsSource.CollectionChanged += PinsSourceOnCollectionChanged;
        }

        #region -- Map --

        public ObservableCollection<MapPinModel> PinsSource
        {
            get { return (ObservableCollection<MapPinModel>)GetValue(PinsSourceProperty); }
            set { SetValue(PinsSourceProperty, value); }
        }

        public static readonly BindableProperty PinsSourceProperty = BindableProperty.Create(
                                                         propertyName: nameof(PinsSource),
                                                         returnType: typeof(ObservableCollection<MapPinModel>),
                                                         declaringType: typeof(CustomMap),
                                                         defaultValue: null,
                                                         defaultBindingMode: BindingMode.TwoWay,
                                                         validateValue: null,
                                                         propertyChanged: PinsSourcePropertyChanged);

        private static void PinsSourcePropertyChanged(BindableObject bindable, object oldvalue, object newValue)
        {
            var bindableMap = bindable as CustomMap;
            var newPinsSource = newValue as IEnumerable;

            if (bindableMap != null && newPinsSource != null)
            {
                bindableMap.Pins.Clear();
                try
                {
                    foreach (MapPinModel pin in newPinsSource)
                    {
                        var pins = new Pin
                        {
                            Label = pin.Title,
                            Address = pin.Description,
                            Type = PinType.SearchResult,
                            Position = new Position(pin.Latitude, pin.Longitude),
                            IsVisible = pin.Chosen
                        };
                        bindableMap.Pins.Add(pins);
                    }
                }
                catch (System.Exception e)
                {
                    System.Console.WriteLine(e);
                }
            }
        }

        private void PinsSourceOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdatePinsSource(this, sender as IEnumerable<Pin>);
        }

        private void MapPinsSourceOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateMapPinsSource(this, sender as IEnumerable<MapPinModel>);
        }

        private static void UpdatePinsSource(Map bindableMap, IEnumerable<Pin> newSource)
        {
            bindableMap.Pins.Clear();
            foreach (var pin in newSource)
                bindableMap.Pins.Add(pin);
        }

        private static void UpdateMapPinsSource(Map bindableMap, IEnumerable<MapPinModel> newSource)
        {
            bindableMap.Pins.Clear();
            foreach (var pin in newSource)
                bindableMap.Pins.Add(new Pin
                {
                    Label = pin.Title,
                    Position = new Position(pin.Latitude, pin.Longitude)
                });
        }

        #endregion

        #region -- MapSpan --

        public MapSpan MapSpan
        {
            get { return (MapSpan)GetValue(MapSpanProperty); }
            set { SetValue(MapSpanProperty, value); }
        }

        public static readonly BindableProperty MapSpanProperty = BindableProperty.Create(
                                                         propertyName: nameof(MapSpan),
                                                         returnType: typeof(MapSpan),
                                                         declaringType: typeof(CustomMap),
                                                         defaultValue: null,
                                                         defaultBindingMode: BindingMode.TwoWay,
                                                         validateValue: null,
                                                         propertyChanged: MapSpanPropertyChanged);

        private static void MapSpanPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var thisInstance = bindable as CustomMap;
            var newMapSpan = newValue as MapSpan;

            thisInstance?.MoveToRegion(newMapSpan);
        }

        #endregion

        #region --Camera--

        public static BindableProperty CurrentCameraPositionProperty = BindableProperty.Create(
                                                       propertyName: nameof(CurrentCameraPosition),
                                                       returnType: typeof(CameraPosition),
                                                       declaringType: typeof(CustomMap),
                                                       defaultValue: null,
                                                       propertyChanged: CurrentCameraPositionPropertyChanged);

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
                (bindable as CustomMap).MoveCamera(cameraUpdate);
            }
        }

        #endregion


    }
}

