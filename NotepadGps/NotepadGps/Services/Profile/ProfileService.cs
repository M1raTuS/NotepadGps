using NotepadGps.Models;
using NotepadGps.Services.Autorization;
using NotepadGps.Services.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotepadGps.Services.Profile
{

    public class ProfileService : IProfileService
    {
        private readonly IRepository _repository;
        private readonly IAutorizationService _autorization;

        public ProfileService(IRepository repository,
                              IAutorizationService autorization)
        {
            _repository = repository;
            _autorization = autorization;
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
            var Id = _autorization.GetCurrentId;
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
