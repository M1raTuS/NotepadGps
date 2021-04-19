using EventKit;
using Foundation;
using NotepadGps.Services.Calendar;
using System;
using System.Threading.Tasks;
using UIKit;

namespace NotepadGps.iOS
{
    public class CalendarService : ICalendarService
    {
        protected EKEventStore eventStore;

        public CalendarService()
        {
            eventStore = new EKEventStore();
        }

        public async Task<bool> AddEventToCalendar(string eventTitle, string eventDescription)
        {
            var granted = await eventStore.RequestAccessAsync(EKEntityType.Event);
            if (granted.Item1)
            {
                EKEvent newEvent = EKEvent.FromStore(eventStore);
                newEvent.StartDate = DateTimeToNSDate(DateTime.Now);
                newEvent.EndDate = DateTimeToNSDate(DateTime.Now.AddHours(1));
                newEvent.Title = eventTitle;
                newEvent.Notes = eventDescription;
                newEvent.Calendar = eventStore.DefaultCalendarForNewEvents;
                NSError e;
                eventStore.SaveEvent(newEvent, EKSpan.ThisEvent, out e);

                return true;
            }
            else
            {
                new UIAlertView("Access Denied", "User Denied Access to Calendar Data", null, "ok", null).Show();

                return false;
            }
        }

        public DateTime NSDateToDateTime(NSDate date)
        {
            double secs = date.SecondsSinceReferenceDate;
            {
                if (secs < -63113904000)
                {
                    return DateTime.MinValue;
                }

                if (secs > 252423993599)
                {
                    return DateTime.MaxValue;
                }

                return (DateTime)date;
            }
        }

        public NSDate DateTimeToNSDate(DateTime date)
        {
            if (date.Kind == DateTimeKind.Unspecified)
            {
                date = DateTime.SpecifyKind(date, DateTimeKind.Local);
            }

            return (NSDate)date;
        }
    }

}
