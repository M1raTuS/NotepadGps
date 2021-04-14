using NotepadGps.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotepadGps.Services.Image
{
    public interface IImageService
    {
        List<ImageModel> GetImageListById();
        Task SaveMapPinAsync(ImageModel imageModel);
    }
}
