using NotepadGps.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotepadGps.Services.Map
{
    public interface IMapPinService
    {
        Task<List<MapPinModel>> GetMapPinListByIdAsync(int id);
        List<MapPinModel> GetMapPinListById(int id);
        Task SaveMapPinAsync(MapPinModel mapPin);
        Task UpdateMapPinAsync(MapPinModel mapPin);
        Task DeleteMapPinAsync(MapPinModel mapPin);
    }
}
