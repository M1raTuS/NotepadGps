﻿using SQLite;

namespace NotepadGps.Models
{
    public class MapPinModel : IEntityBase
    {
        [PrimaryKey, AutoIncrement, Column("Id")]
        public int Id { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool Chosen { get; set; }
        public int UserId { get; set; }
        public string ImgPath { get; set; }
    }
}
