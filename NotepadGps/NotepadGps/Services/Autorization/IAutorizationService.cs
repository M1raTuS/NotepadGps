namespace NotepadGps.Services.Autorization
{
    public interface IAutorizationService
    {
        bool IsAutorized { get; set; }
        int GetCurrentId { get; set; }
    }
}
