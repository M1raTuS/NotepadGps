using NotepadGps.Models;
using NotepadGps.Services.Autentification;
using NotepadGps.Services.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotepadGps.Services.Profile
{
    public class ProfileService : IProfileService
    {
        private readonly IRepository _repository;
        private readonly IAutentificationService _autentification;

        public ProfileService(IRepository repository,
                              IAutentificationService autentification)
        {
            _repository = repository;
            _autentification = autentification;
        }

        public async Task<List<UserModel>> GetAllUserListAsync()
        {
            var users = new List<UserModel>();
            var list = await _repository.GetAllAsync<UserModel>();
            if (list.Count > 0)
            {
                users.AddRange(list);
            }
            return users;
        }

        public List<UserModel> GetAllUserList()
        {
            var users = new List<UserModel>();
            var list = _repository.GetAll<UserModel>();
            if (list.Count > 0)
            {
                users.AddRange(list);
            }
            return users;
        }

        public async Task<List<UserModel>> GetUserListByIdAsync()
        {
            var users = new List<UserModel>();
            var Id = _autentification.GetCurrentId;
            var list = await _repository.FindAsync<UserModel>(c => c.Id == Id);
            if (list.Count > 0)
            {
                users.AddRange(list);
            }
            return users;
        }

        public async Task SaveUserAsync(UserModel user)
        {
            await _repository.InsertAsync(user);
        }
    }
}
