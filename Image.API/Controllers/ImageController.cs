using Image.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Image.API.Controllers
{
    [ApiController]
    [Route("api/images")]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImages(List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
                return BadRequest("No files uploaded.");

            var result = await _imageService.ProcessUploadedImages(files);
            return Ok(result);
        }

        [HttpGet("download/{id}/{size}")]
        public IActionResult DownloadImage(string id, string size)
        {
            var fileStream = _imageService.GetResizedImage(id, size);
            if (!fileStream.IsSuccess)
                return NotFound(fileStream);

            return File(fileStream.ResponseData, "image/webp");
        }

        [HttpGet("metadata/{id}")]
        public IActionResult GetMetadata(string id)
        {
            var metadata = _imageService.GetImageMetadata(id);
            if (!metadata.IsSuccess)
                return NotFound(metadata);
            return Ok(metadata);
        }
    }

}
