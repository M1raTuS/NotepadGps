using SQLite;

namespace NotepadGps.Models
{
    public class ImageModel : IEntityBase
    {
        [PrimaryKey, AutoIncrement, Column("Id")]
        public int Id { get; set; }
        public string ImagePins { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public int UserId { get; set; }
        public string ImgShortPath { get; set; }
    }
}
