using NotepadGps.Models;
using NotepadGps.Services.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotepadGps.Services.Image
{
    public class ImageService : IImageService
    {
        private readonly IRepositoryService _repositoryService;

        public ImageService(
            IRepositoryService repositoryService)
        {
            _repositoryService = repositoryService;
        }

        #region -- IImageService implementation --

        public async Task<List<ImageModel>> GetImageListByIdAsync(int id)
        {
            var image = new List<ImageModel>();
            var list = await _repositoryService.FindAsync<ImageModel>(c => c.UserId == id);

            if (list.Count > 0)
            {
                image.AddRange(list);
            }

            return image;
        }

        public async Task SaveMapPinAsync(ImageModel imageModel)
        {
            await _repositoryService.InsertAsync(imageModel);
        }

        public async Task<List<ImageModel>> FindImgAsync(string lat, string lon)
        {
            var img = new List<ImageModel>();
            var list = await _repositoryService.FindAsync<ImageModel>(
                x => x.Latitude == lat && 
                     x.Longitude == lon);

            if (list.Count > 0)
            {
                img.AddRange(list);
            }

            return img;
        }

        #endregion

    }
}
