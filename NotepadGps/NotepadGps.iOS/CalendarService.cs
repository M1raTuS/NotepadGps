using EventKit;
using Foundation;
using NotepadGps.Services.Calendar;
using System;
using System.Threading.Tasks;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(NotepadGps.iOS.CalendarService))]
namespace NotepadGps.iOS
{
    public class CalendarService : ICalendarService
    {
        protected EKEventStore eventStore;

        public CalendarService()
        {
            eventStore = new EKEventStore();
        }

        public async Task<bool> AddEventToCalendar(string eventTitle, DateTime dateTime, TimeSpan timeSpan)
        {
            var granted = await eventStore.RequestAccessAsync(EKEntityType.Event);

            if (granted.Item1)
            {
                EKEvent newEvent = EKEvent.FromStore(eventStore);
                newEvent.StartDate = DateTimeToNSDate(new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds));
                newEvent.EndDate = DateTimeToNSDate(new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, timeSpan.Hours + 1, timeSpan.Minutes, timeSpan.Seconds));
                newEvent.Title = eventTitle;
                newEvent.Calendar = eventStore.DefaultCalendarForNewEvents;

                newEvent.AddAlarm(EKAlarm.FromDate(newEvent.StartDate.AddSeconds(-3600)));

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
