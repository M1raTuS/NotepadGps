﻿using Android.Content;
using Android.Database;
using Android.Provider;
using Java.Util;
using NotepadGps.Services.Calendar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(NotepadGps.Droid.CalendarService))]
namespace NotepadGps.Droid
{
    public class CalendarService : ICalendarService
    {
        private const string timeZone = "UTC";

        public async Task<bool> AddEventToCalendar(string eventTitle, DateTime dateTime, TimeSpan timeSpan)
        {
            var calendarsId = GetCalendarList();
            var newTime = timeSpan.Add(new TimeSpan(1, 0, 0));

            foreach (var calendarId in calendarsId)
            {
                ContentValues eventValues = new ContentValues();

                eventValues.Put(CalendarContract.Events.InterfaceConsts.CalendarId, calendarId);
                eventValues.Put(CalendarContract.Events.InterfaceConsts.Title, eventTitle);
                eventValues.Put(CalendarContract.Events.InterfaceConsts.Description, eventTitle);
                eventValues.Put(CalendarContract.Events.InterfaceConsts.Dtstart, GetDateTimeMS(dateTime.Year, dateTime.Month, dateTime.Day,
                                                                                              timeSpan.Hours, timeSpan.Minutes));
                eventValues.Put(CalendarContract.Events.InterfaceConsts.Dtend, GetDateTimeMS(dateTime.Year, dateTime.Month, dateTime.Day,
                                                                                              newTime.Hours, timeSpan.Minutes));
                eventValues.Put(CalendarContract.Events.InterfaceConsts.EventTimezone, timeZone);
                eventValues.Put(CalendarContract.Events.InterfaceConsts.EventEndTimezone, timeZone);

                MainActivity.Instance.ContentResolver.Insert(CalendarContract.Events.ContentUri, eventValues);
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

        private long GetDateTimeMS(int year, int month, int day, int hr, int min)
        {
            Calendar c = Calendar.GetInstance(Java.Util.TimeZone.GetTimeZone(TimeZoneInfo.Local.Id));

            c.Set(CalendarField.DayOfMonth, day);
            c.Set(CalendarField.HourOfDay, hr);
            c.Set(CalendarField.Minute, min);
            c.Set(CalendarField.Month, month - 1);
            c.Set(CalendarField.Year, year);

            return c.TimeInMillis;
        }
    }
}