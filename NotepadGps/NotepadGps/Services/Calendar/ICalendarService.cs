using System.Threading.Tasks;

namespace NotepadGps.Services.Calendar
{
    public interface ICalendarService
    {
        Task<bool> AddEventToCalendar(string eventTitle, string eventDescription);
    }
}
