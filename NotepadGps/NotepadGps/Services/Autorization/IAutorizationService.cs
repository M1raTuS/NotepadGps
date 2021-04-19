using System.Threading.Tasks;

namespace NotepadGps.Services.Autorization
{
    public interface IAutorizationService
    {
        bool IsAutorized { get; }
        Task<bool> TryToAuthorizeAsync(string email, string password);
        int GetAutorizedUserId { get; }
        void Unautorize();
    }
}
