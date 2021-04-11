using NotepadGps.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotepadGps.Services.Profile
{
    public interface IProfileService
    {
        Task<List<UserModel>> GetUserListByIdAsync();
        Task<List<UserModel>> GetAllUserListAsync();
        Task SaveUserAsync(UserModel user);
        List<UserModel> GetAllUserList();
    }
}
