using NotepadGps.Models;
using NotepadGps.Services.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotepadGps.Services.Map
{
    public class MapPinService : IMapPinService
    {
        private readonly IRepositoryService _repository;

        public MapPinService(
            IRepositoryService repository)
        {
            _repository = repository;
        }

        #region -- IMapPinService implementation --

        public async Task DeleteMapPinAsync(MapPinModel mapPin)
        {
            await _repository.DeleteAsync(mapPin);
        }

        public async Task<List<MapPinModel>> GetMapPinListByIdAsync(int id)
        {
            var mapPin = new List<MapPinModel>();
            var list = await _repository.FindAsync<MapPinModel>(c => c.UserId == id);

            if (list.Count > 0)
            {
                mapPin = list;
            }

            return mapPin;
        }

        public List<MapPinModel> GetMapPinListById(int id)
        {
            var mapPin = new List<MapPinModel>();
            var list = _repository.Find<MapPinModel>(c => c.UserId == id);

            if (list.Count > 0)
            {
                mapPin = list;
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

        #endregion

    }
}
