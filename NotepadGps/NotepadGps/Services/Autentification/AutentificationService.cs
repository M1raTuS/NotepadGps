using NotepadGps.Models;
using NotepadGps.Services.Repository;
using System.Threading.Tasks;

namespace NotepadGps.Services.Autentification
{
    public class AutentificationService : IAutentificationService
    {
        private readonly IRepositoryService _repository;

        public AutentificationService(IRepositoryService repository)
        {
            _repository = repository;
        }

        #region -- IAutentificationService implementation --

        public async Task<bool> CheckEmailAsync(string email)
        {
            var isEmailExist = false;
            var users = await _repository.GetAllAsync<UserModel>();

            foreach (var item in users)
            {
                if (item.Email == email.ToString())
                {
                    isEmailExist = true;
                }
            }

            return isEmailExist;
        }

        #endregion
    }
}
