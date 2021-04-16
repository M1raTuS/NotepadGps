using System.Threading.Tasks;

namespace NotepadGps.Services.Autentification
{
    public interface IAutentificationService
    {
        Task<bool> CheckEmailAsync(string email);
    }
}
