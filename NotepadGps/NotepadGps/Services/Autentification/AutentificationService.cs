using NotepadGps.Models;
using NotepadGps.Services.Repository;
using NotepadGps.ViewModel;
using System.Collections.ObjectModel;

namespace NotepadGps.Services.Autentification
{
    public class AutentificationService : BaseViewModel, IAutentificationService
    {
        private readonly IRepository _repository;

        public AutentificationService(IRepository repository)
        {
            _repository = repository;
        }

        private int _getCurrentId;
        public int GetCurrentId
        {
            get => _getCurrentId;
            set => SetProperty(ref _getCurrentId, value);
        }

        public void LoadProfile()
        {
            var user = _repository.GetAll<UserModel>();
            User = new ObservableCollection<UserModel>(user);
        }

        public bool CheckEmail(string email)
        {
            LoadProfile();

            foreach (var item in User)
            {
                if (item.Email == email.ToString())
                {
                    return true;
                }
            }

            return false;
        }
    }
}
