using Image.API.Interfaces;
using MetadataExtractor.Formats.Exif;
using MetadataExtractor;
using SixLabors.ImageSharp.Formats.Webp;
using Directory = System.IO.Directory;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Image.API.DTOs.Enums;
using Image.API.DTOs;
using StatusCodes = Image.API.DTOs.StatusCodes;

namespace Image.API.Services
{

    public static class AllowedImageFormats
    {
        public static readonly HashSet<string> Formats = new() { ".jpg", ".jpeg", ".png", ".webp" };
    }

    public class ImageService : IImageService
    {
        private readonly string _storagePath;

        public ImageService()
        {
            _storagePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            if (!Directory.Exists(_storagePath))
                Directory.CreateDirectory(_storagePath);
        }

        public async Task<BaseCommandResponse<List<object>>> ProcessUploadedImages(List<IFormFile> files)
        {
            var results = new List<object>();

            foreach (var file in files)
            {
                if (file.Length > 2 * 1024 * 1024)
                {
                    return new BaseCommandResponse<List<object>>
                    {
                        IsSuccess = false,
                        Message = "File size exceeds 2MB.",
                        ResponseData = null,
                        Errors = new List<Errors> { new Errors { Key = (int)StatusCodes.InvalidFileExceed, Value = "File size exceeds 2MB." } },
                        StatusCode = (int)StatusCodes.BadRequest,
                    };
                }

                var ext = Path.GetExtension(file.FileName).ToLower();
                if (!AllowedImageFormats.Formats.Contains(ext))
                {
                    return new BaseCommandResponse<List<object>>
                    {
                        IsSuccess = false,
                        Message = "Invalid file format.",
                        ResponseData = null,
                        Errors = new List<Errors> { new Errors { Key = (int)StatusCodes.InvalidFileFormat, Value = "Invalid file format." } },
                        StatusCode = (int)StatusCodes.BadRequest,
                    };
                }

                var imageId = Guid.NewGuid().ToString();

                // Store file in MemoryStream to allow multiple reads
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                // Extract metadata
                var metadata = ExtractMetadata(memoryStream);
                var metadataPath = Path.Combine(_storagePath, "metadata", $"{imageId}.json");
                Directory.CreateDirectory(Path.GetDirectoryName(metadataPath)!);
                await File.WriteAllTextAsync(metadataPath, metadata);

                // Reset stream position before loading the image
                memoryStream.Position = 0;
                using var image = SixLabors.ImageSharp.Image.Load(memoryStream);

                // Save original image
                var originalPath = Path.Combine(_storagePath, "original", $"{imageId}.webp");
                Directory.CreateDirectory(Path.GetDirectoryName(originalPath)!);
                image.Save(originalPath, new WebpEncoder());

                // Resize & Save images for all defined sizes
                foreach (ImageSize size in Enum.GetValues(typeof(ImageSize)))
                {
                    var resizedPath = Path.Combine(_storagePath, size.ToString().ToLower(), $"{imageId}.webp");
                    Directory.CreateDirectory(Path.GetDirectoryName(resizedPath)!);

                    using var resizedImage = image.Clone(ctx => ctx.Resize((int)size, 0));
                    resizedImage.Save(resizedPath, new WebpEncoder());
                }

                results.Add(new { id = imageId });
            }

            return new BaseCommandResponse<List<object>> { IsSuccess = true, Message = "Images uploaded successfully.", ResponseData = results, Errors = null, StatusCode = (int)StatusCodes.Success };
        }

        public BaseCommandResponse<Stream> GetResizedImage(string id, string size)
        {
            if (!Enum.TryParse(typeof(ImageSize), size, true, out var sizeEnum))
            {
                return new BaseCommandResponse<Stream>
                {
                    IsSuccess = false,
                    Message = "Invalid size.",
                    ResponseData = null,
                    Errors = new List<Errors> { new Errors { Key = (int)StatusCodes.InvalidSize, Value = "Invalid size." } },
                    StatusCode = (int)StatusCodes.BadRequest,
                };
            }

            var path = Path.Combine(_storagePath, size.ToLower(), $"{id}.webp");
            if (File.Exists(path))
            {
                return new BaseCommandResponse<Stream> { IsSuccess = true, Message = "Image found.", ResponseData = File.OpenRead(path), Errors = null, StatusCode = (int)StatusCodes.Success };
            }

            return new BaseCommandResponse<Stream> { IsSuccess = false, Message = "Image not found.", ResponseData = null, Errors = null, StatusCode = (int)StatusCodes.NotFound };
        }
        public BaseCommandResponse<object> GetImageMetadata(string id)
        {
            var path = Path.Combine(_storagePath, "metadata", $"{id}.json");

            if (File.Exists(path))
            {
                var jsonData = File.ReadAllText(path);
                var metadataObject = System.Text.Json.JsonSerializer.Deserialize<object>(jsonData);

                return new BaseCommandResponse<object>
                {
                    IsSuccess = true,
                    Message = "Metadata found.",
                    ResponseData = metadataObject,
                    Errors = null,
                    StatusCode = (int)StatusCodes.Success
                };
            }

            return new BaseCommandResponse<object>
            {
                IsSuccess = false,
                Message = "Metadata not found.",
                ResponseData = null,
                Errors = null,
                StatusCode = (int)StatusCodes.NotFound
            };
        }


        #region Private Methods
        //private string ExtractMetadata(Stream stream)
        //{
        //    stream.Position = 0; // Reset stream before reading metadata

        //    var metadata = ImageMetadataReader.ReadMetadata(stream);
        //    var metadataDict = new Dictionary<string, object>();

        //    foreach (var directory in metadata)
        //    {
        //        foreach (var tag in directory.Tags)
        //        {
        //            // Store metadata in a dictionary
        //            metadataDict[$"{directory.Name} - {tag.Name}"] = tag.Description ?? "Unknown";
        //        }
        //    }

        //    // Convert dictionary to JSON
        //    return System.Text.Json.JsonSerializer.Serialize(metadataDict, new System.Text.Json.JsonSerializerOptions
        //    {
        //        WriteIndented = true // Pretty-print JSON
        //    });
        //}

        private string ExtractMetadata(Stream stream)
        {
            stream.Position = 0; // Reset stream before reading metadata

            var metadata = ImageMetadataReader.ReadMetadata(stream);
            var structuredMetadata = new Dictionary<string, object>();

            foreach (var directory in metadata)
            {
                var directoryData = new Dictionary<string, object>();

                foreach (var tag in directory.Tags)
                {
                    directoryData[tag.Name] = tag.Description ?? "Unknown";
                }

                if (directoryData.Count > 0)
                {
                    structuredMetadata[directory.Name] = directoryData;
                }
            }

            // Convert to formatted JSON for readability
            return System.Text.Json.JsonSerializer.Serialize(structuredMetadata, new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true // Pretty-print JSON
            });
        }

        #endregion


    }
}
