using SQLite;

namespace NotepadGps.Models
{
    public class ProfileModel : IEntityBase
    {

        [PrimaryKey, AutoIncrement, Column("Id")]
        public int Id { get; set; }
        public string Description { get; set; }
    }
}
