namespace NotepadGps.Services.Autorization
{
    public interface IAutorizationService
    {
        bool IsAutorized { get; set; }
        void Authorizate(string email, string password);
    }
}
