using NotepadGps.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotepadGps.Services.Profile
{

    public interface IProfileService
    {
        Task<List<ProfileModel>> GetProfileListByIdAsync();
        Task<List<UserModel>> GetAllUserListAsync();
        Task SaveProfileAsync(ProfileModel profile);
        Task UpdateProfileAsync(ProfileModel profile);
        Task DeleteProfileAsync(ProfileModel profile); 
        Task SaveUserAsync(UserModel user); 
    }
}
