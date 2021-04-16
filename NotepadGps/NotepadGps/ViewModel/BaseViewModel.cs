﻿using NotepadGps.Models;
using Prism;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;

namespace NotepadGps.ViewModel
{
    public class BaseViewModel : BindableBase, IInitialize, INavigationAware, IActiveAware
    {
        public BaseViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        #region -- Public properties --

        private ObservableCollection<UserModel> user; //TODO: remove
        public ObservableCollection<UserModel> User
        {
            get => user;
            set => SetProperty(ref user, value);
        }

        private ObservableCollection<MapPinModel> mapPin;
        public ObservableCollection<MapPinModel> MapPin
        {
            get => mapPin;
            set => SetProperty(ref mapPin, value);
        }

        public event EventHandler IsActiveChanged;

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value, RaiseIsActiveChanged);
        }

        public INavigationService NavigationService { get;  }

        #endregion

        #region -- Initialize implementation --

        public virtual void Initialize(INavigationParameters parameters)
        {

        }

        #endregion

        #region -- OnNavigatedFrom implementation --

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        #endregion

        #region -- OnNavigatedTo implementation --

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        #endregion

        #region -- RaiseIsActiveChanged implementation --

        protected virtual void RaiseIsActiveChanged()
        {
            IsActiveChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
