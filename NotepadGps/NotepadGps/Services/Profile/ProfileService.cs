using NotepadGps.Models;
using NotepadGps.Services.Autorization;
using NotepadGps.Services.Repository;
using NotepadGps.Services.Settings;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotepadGps.Services.Profile
{

    public class ProfileService : IProfileService
    {
        private readonly IRepository _repository;
        private readonly ISettingsService _settingsService;

        public ProfileService(IRepository repository,
                              ISettingsService settingsService)
        {
            _repository = repository;
            _settingsService = settingsService;
        }
        public async Task DeleteMapPinAsync(MapPinModel mapPin)
        {
            await _repository.DeleteAsync(mapPin);
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
        public async Task<List<MapPinModel>> GetMapPinListByIdAsync()
        {
            var mapPin = new List<MapPinModel>();
            var Id = _settingsService.CurrentUser;
            var list = await _repository.FindAsync<MapPinModel>(c => c.UserId == Id);
            if (list.Count > 0)
            {
                mapPin.AddRange(list);
            }
            return mapPin;
        }
        public  List<MapPinModel> GetMapPinListById()
        {
            var mapPin = new List<MapPinModel>();
            var Id = _settingsService.CurrentUser;
            var list =  _repository.Find<MapPinModel>(c => c.UserId == Id);
            if (list.Count > 0)
            {
                mapPin.AddRange(list);
            }
            return mapPin;
        }

        public async Task<List<UserModel>> GetUserListByIdAsync()
        {
            var users = new List<UserModel>();
            var Id = _settingsService.CurrentUser;
            var list = await _repository.FindAsync<UserModel>(c => c.Id == Id);
            if (list.Count > 0)
            {
                users.AddRange(list);
            }
            return users;
        }

        public async Task SaveMapPinAsync(MapPinModel mapPin)
        {
            await _repository.InsertAsync(mapPin);
        }
        public async Task SaveUserAsync(UserModel user)
        {
            await _repository.InsertAsync(user);
        }
        public async Task UpdateMapPinAsync(MapPinModel mapPin)
        {
            await _repository.UpdateAsync(mapPin);
        }
    }
}
