using Android.Content;
using Android.Database;
using Android.Provider;
using NotepadGps.Services.Calendar;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(NotepadGps.Droid.CalendarService))]
namespace NotepadGps.Droid
{
    public class CalendarService : ICalendarService
    {
        public async Task<bool> AddEventToCalendar()
        {
            var calendarsId = GetCalendarList();
            foreach (var item in calendarsId)
            {

            }

            return await Task.FromResult(true);
        }

        private List<string> GetCalendarList()
        {
            List<string> calendarList = new List<string>();

            var calendarsUri = CalendarContract.Calendars.ContentUri;

            string[] calendarsProjection = {
                    CalendarContract.Calendars.InterfaceConsts.Id,
                    CalendarContract.Calendars.InterfaceConsts.CalendarDisplayName,
                    CalendarContract.Calendars.InterfaceConsts.AccountName
            };

            var loader = new CursorLoader(MainActivity.Instance, calendarsUri, calendarsProjection, null, null, null);
            var cursor = (ICursor)loader.LoadInBackground();

            while (cursor.MoveToNext())
            {
                string id = cursor.GetString(cursor.GetColumnIndex(CalendarContract.Calendars.InterfaceConsts.Id));
                calendarList.Add(id);
            }

            return calendarList;
        }
    }
}