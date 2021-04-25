using NotepadGps.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotepadGps.Services.Image
{
    public interface IEventService
    {
        Task<List<EventModel>> GetEventListByIdAsync(int id);
        Task SaveEventAsync(EventModel eventModel);
    }
}
