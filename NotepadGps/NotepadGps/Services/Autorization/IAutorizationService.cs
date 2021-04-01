namespace NotepadGps.Services.Autorization
{
    public interface IAutorizationService
    {

        int GetCurrentId { get; set; }
        int GetCurrentUserId();
    }
}
