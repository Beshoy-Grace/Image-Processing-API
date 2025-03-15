using Image.API.DTOs;

namespace Image.API.Interfaces
{
    public interface IImageService
    {
        Task<BaseCommandResponse<List<object>>> ProcessUploadedImages(List<IFormFile> files);
        BaseCommandResponse<Stream> GetResizedImage(string id, string size);
        BaseCommandResponse<object> GetImageMetadata(string id);
    }
}
