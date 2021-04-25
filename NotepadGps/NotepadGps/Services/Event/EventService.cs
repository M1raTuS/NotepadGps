using NotepadGps.Models;
using NotepadGps.Services.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotepadGps.Services.Image
{
    public class EventService : IEventService
    {
        private readonly IRepositoryService _repositoryService;

        public EventService(
            IRepositoryService repositoryService)
        {
            _repositoryService = repositoryService;
        }

        #region -- IImageService implementation --

        public async Task<List<EventModel>> GetEventListByIdAsync(int id)
        {
            var events = new List<EventModel>();
            var list = await _repositoryService.FindAsync<EventModel>(c => c.UserId == id);

            if (list.Count > 0)
            {
                events = list;
            }

            return events;
        }

        public async Task SaveEventAsync(EventModel eventModel)
        {
            await _repositoryService.InsertAsync(eventModel);
        }

        #endregion

    }
}
