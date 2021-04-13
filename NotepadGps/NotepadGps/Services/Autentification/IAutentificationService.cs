namespace NotepadGps.Services.Autentification
{
    public interface IAutentificationService
    {
        int GetCurrentId { get; set; }
        bool CheckEmail(string email);
    }
}
