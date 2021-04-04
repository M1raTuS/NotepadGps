using NotepadGps.Models;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;

namespace NotepadGps.ViewModel
{
    public class BaseViewModel : BindableBase, IInitialize, INavigationAware, IDisposable
    {
        public UserModel _user;
        public MapPinModel _mapPin;


        public ObservableCollection<UserModel> User;

        #region -Public properties-

        private ObservableCollection<UserModel> users;
        public ObservableCollection<UserModel> Users
        {
            get => users;
            set => SetProperty(ref users, value);
        }


        private ObservableCollection<MapPinModel> mapPin;
        public ObservableCollection<MapPinModel> MapPin
        {
            get => mapPin;
            set => SetProperty(ref mapPin, value);
        }

        #endregion
        public void Dispose()
        {
        }

        public virtual void Initialize(INavigationParameters parameters)
        {
            
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
            
        }
    }
}
