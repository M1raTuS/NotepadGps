using NotepadGps.Models;
using NotepadGps.Services.Autentification;
using NotepadGps.Services.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotepadGps.Services.Image
{
    public class ImageService : IImageService
    {
        private readonly IRepository _repository;
        private readonly IAutentificationService _autentification;

        public ImageService(IRepository repository,
                            IAutentificationService autentification)
        {
            _repository = repository;
            _autentification = autentification;
        }
        public new List<ImageModel> GetImageListById()
        {
            var image = new List<ImageModel>();
            // var Id = _autentification.CurentUserId;
            var id = 1; //TODO: rework
            var list =  _repository.Find<ImageModel>(c => c.UserId == id);
            if (list.Count > 0)
            {
                image.AddRange(list);
            }
            return image;
        }

        public async Task SaveMapPinAsync(ImageModel imageModel)
        {
            await _repository.InsertAsync(imageModel);
        }
    }
}
