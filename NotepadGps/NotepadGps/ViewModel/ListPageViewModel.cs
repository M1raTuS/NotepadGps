﻿using NotepadGps.Models;
using NotepadGps.Services.Map;
using NotepadGps.View;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotepadGps.ViewModel
{
    public class ListPageViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IMapPinService _mapPinService;

        public ListPageViewModel(INavigationService navigationService,
                                 IMapPinService mapPinService)
        {
            _navigationService = navigationService;
            _mapPinService = mapPinService;

            Load();
        }

        #region -Public properties-

        private ObservableCollection<MapPinModel> _mapPins;
        public ObservableCollection<MapPinModel> MapPins
        {
            get => _mapPins;
            set => SetProperty(ref _mapPins, value);
        }

        private ObservableCollection<MapPinModel> _selectedItem;
        public ObservableCollection<MapPinModel> SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        private bool _labelVisible;
        public bool LabelVisible
        {
            get => _labelVisible;
            set => SetProperty(ref _labelVisible, value);
        }

        private bool _listEnabled;
        public bool ListEnabled
        {
            get => _listEnabled;
            set => SetProperty(ref _listEnabled, value);
        }

        private string _searchPin;
        public string SearchPin
        {
            get => _searchPin;
            set => SetProperty(ref _searchPin, value);
        }

        private bool _chosen;
        public bool Chosen
        {
            get => _labelVisible;
            set => SetProperty(ref _chosen, value);
        }

        public ICommand AddMapPinFloatingButtonCommand => new Command(AddMapPinFloatingButton);
        public ICommand SelectedCommand => new Command(SelectedPin);
        public ICommand EditContext => new Command(EditContextMenu);
        public ICommand DeleteContext => new Command(DeleteContextMenu);


        #endregion

        #region -Methods-

        private async void AddMapPinFloatingButton()
        {
            await _navigationService.NavigateAsync(nameof(AddEditMapPinView));
        }

        private async void SelectedPin(object pin)
        {
            var nav = new NavigationParameters();
            nav.Add(nameof(MapPinModel), (MapPinModel)pin);

            await _navigationService.NavigateAsync(nameof(MapsPage), nav, false, true);

        }

        private async void EditContextMenu(object obj)
        {
            var nav = new NavigationParameters();
            nav.Add(nameof(MapPinModel), (MapPinModel)obj);

            await _navigationService.NavigateAsync(nameof(AddEditMapPinView), nav, false, true);
        }

        private async void DeleteContextMenu(object obj)
        {
            if (await Application.Current.MainPage.DisplayAlert("Alert", "Подтверждаете ли вы удаление?", "Ok", "Cancel"))
            {
                await _mapPinService.DeleteMapPinAsync((MapPinModel)obj);

                Load();
            }
        }

        private void Load()
        {
            var mapPin = _mapPinService.GetMapPinListById();
            MapPin = new ObservableCollection<MapPinModel>(mapPin);
        }

        #endregion

        #region -Overrides-

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            if (args.PropertyName == nameof(MapPin))
            {
                if (MapPin.Count > 0)
                {
                    LabelVisible = false;
                    ListEnabled = true;
                }
                else
                {
                    LabelVisible = true;
                    ListEnabled = false;
                }
            }

            if (args.PropertyName == nameof(SearchPin))
            {
                Load();

                if (!(SearchPin.Length == 0 || String.IsNullOrEmpty(SearchPin)))
                {
                    try
                    {
                        var pin = MapPin.Where(x => x.Title.ToLower().Contains(SearchPin.ToLower()) || x.Latitude.ToString().ToLower().Contains(SearchPin.ToLower()) || x.Longitude.ToString().ToLower().Contains(SearchPin.ToLower()));
                        MapPin = new ObservableCollection<MapPinModel>(pin);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                    }
                }
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            Load();
        }

        #endregion
    }
}
