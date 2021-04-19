using NotepadGps.Models;
using NotepadGps.Services.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotepadGps.Services.Profile
{
    public class ProfileService : IProfileService
    {
        private readonly IRepositoryService _repositoryService;

        public ProfileService(
            IRepositoryService repositoryService)
        {
            _repositoryService = repositoryService;
        }

        #region -- IProfileService implementation --

        public async Task<List<UserModel>> GetAllUserListAsync()
        {
            var users = new List<UserModel>();
            var list = await _repositoryService.GetAllAsync<UserModel>();

            if (list.Count > 0)
            {
                users.AddRange(list);
            }

            return users;
        }

        public List<UserModel> GetAllUserList()
        {
            var users = new List<UserModel>();
            var list = _repositoryService.GetAll<UserModel>();

            if (list.Count > 0)
            {
                users.AddRange(list);
            }

            return users;
        }

        public async Task<List<UserModel>> GetUserListByIdAsync(int id)
        {
            var users = new List<UserModel>();
            var list = await _repositoryService.FindAsync<UserModel>(c => c.Id == id);

            if (list.Count > 0)
            {
                users.AddRange(list);
            }

            return users;
        }

        public async Task<UserModel> GetUserByIdAsync(int id)
        {
            var user = await _repositoryService.FindUserAsync<UserModel>(c => c.Id == id);
            return user;
        }

        public async Task SaveUserAsync(UserModel user)
        {
            await _repositoryService.InsertAsync(user);
        }

        #endregion

    }
}
