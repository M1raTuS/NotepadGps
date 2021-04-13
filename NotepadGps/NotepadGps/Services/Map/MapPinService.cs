using NotepadGps.Models;
using NotepadGps.Services.Autentification;
using NotepadGps.Services.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotepadGps.Services.Map
{
    public class MapPinService : IMapPinService
    {
        private readonly IRepository _repository;
        private readonly IAutentificationService _autentification;

        public MapPinService(IRepository repository,
                             IAutentificationService autentification)
        {
            _repository = repository;
            _autentification = autentification;
        }

        public async Task DeleteMapPinAsync(MapPinModel mapPin)
        {
            await _repository.DeleteAsync(mapPin);
        }

        public async Task<List<MapPinModel>> GetMapPinListByIdAsync()
        {
            var mapPin = new List<MapPinModel>();
            var Id = _autentification.GetCurrentId;
            var list = await _repository.FindAsync<MapPinModel>(c => c.UserId == Id);
            if (list.Count > 0)
            {
                mapPin.AddRange(list);
            }
            return mapPin;
        }

        public List<MapPinModel> GetMapPinListById()
        {
            var mapPin = new List<MapPinModel>();
            var Id = _autentification.GetCurrentId;
            var list = _repository.Find<MapPinModel>(c => c.UserId == Id);
            if (list.Count > 0)
            {
                mapPin.AddRange(list);
            }
            return mapPin;
        }

        public async Task SaveMapPinAsync(MapPinModel mapPin)
        {
            await _repository.InsertAsync(mapPin);
        }

        public async Task UpdateMapPinAsync(MapPinModel mapPin)
        {
            await _repository.UpdateAsync(mapPin);
        }
    }
}
