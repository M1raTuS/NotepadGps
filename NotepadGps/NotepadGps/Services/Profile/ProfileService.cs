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
        private readonly IAutorizationService _autorizationService;

        public ProfileService(IRepository repository,
                              IAutorizationService autorizationService)
        {
            _repository = repository;
            _autorizationService = autorizationService;
        }
        public async Task DeleteProfileAsync(ProfileModel profile)
        {
            await _repository.DeleteAsync(profile);
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

        public async Task<List<ProfileModel>> GetProfileListByIdAsync()
        {
            var users = new List<ProfileModel>();
            var Id = _autorizationService.GetCurrentUserId();
            var list = await _repository.FindAsync<ProfileModel>(c => c.Id == Id);
            if (list.Count > 0)
            {
                users.AddRange(list);
            }
            return users;
        }

        public async Task SaveProfileAsync(ProfileModel profile)
        {
            await _repository.InsertAsync(profile);
        }
        public async Task SaveUserAsync(UserModel user)
        {
            await _repository.InsertAsync(user);
        }
        public async Task UpdateProfileAsync(ProfileModel profile)
        {
            await _repository.UpdateAsync(profile);
        }
    }
}
