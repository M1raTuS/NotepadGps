using NotepadGps.Models;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace NotepadGps.Controls
{
    public class CustomMap : Map
    {

        public CustomMap()
        {
            PinsSource = new ObservableCollection<MapPinModel>();
            PinsSource.CollectionChanged += PinsSourceOnCollectionChanged;
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
                ((CustomMap)bindable).MoveCamera(cameraUpdate);
            }
        }

        #endregion

        #region -- Private properties --

        private static void PinsSourcePropertyChanged(BindableObject bindable, object oldvalue, object newValue) //TODO: onproperty changed
        {
            var bindableMap = (CustomMap)bindable;
            var newPinsSource = newValue as IEnumerable;

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
            {
                bindableMap.Pins.Add(pin);
            }
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

        private static void MapSpanPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var thisInstance = bindable as CustomMap;
            var newMapSpan = newValue as MapSpan;

            thisInstance?.MoveToRegion(newMapSpan);
        }

        #endregion
        //public void OnPropertyChanged(PropertyChangedEventArgs args)
        //{
        //    if (args.PropertyName == nameof(PinsSourceProperty))
        //    {

        //    }
        //}

    }
}

