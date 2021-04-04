using NotepadGps.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotepadGps.Services.Profile
{

    public interface IProfileService
    {
        Task<List<MapPinModel>> GetMapPinListByIdAsync();
        Task<List<UserModel>> GetUserListByIdAsync();
        Task<List<UserModel>> GetAllUserListAsync();
        Task SaveMapPinAsync(MapPinModel mapPin);
        Task UpdateMapPinAsync(MapPinModel mapPin);
        Task DeleteMapPinAsync(MapPinModel mapPin); 
        Task SaveUserAsync(UserModel user);
        List<UserModel> GetAllUserList();
        List<MapPinModel> GetMapPinListById();
    }
}
