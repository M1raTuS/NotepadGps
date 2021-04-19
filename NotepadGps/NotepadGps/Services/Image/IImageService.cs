using NotepadGps.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotepadGps.Services.Image
{
    public interface IImageService
    {
        Task<List<ImageModel>> GetImageListByIdAsync(int id);
        Task SaveMapPinAsync(ImageModel imageModel);
        Task<List<ImageModel>> FindImgAsync(string lat, string lon);
    }
}
