using SQLite;
using System;

namespace NotepadGps.Models
{
    public class EventModel : IEntityBase
    {
        [PrimaryKey, AutoIncrement, Column("Id")]
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public int UserId { get; set; }
    }
}
