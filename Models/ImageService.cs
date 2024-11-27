using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;

namespace WebApplication23.Models
{
    public class ImageService
    {
        private readonly ComputerVisionClient _computerVisionClient;
        private readonly string _uploadPath;

        public ImageService(IConfiguration configuration)
        {
            var endpoint = configuration["ComputerVision:Endpoint"];
            var key = configuration["ComputerVision:SubscriptionKey"];
            _computerVisionClient = new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
            {
                Endpoint = endpoint
            };

            _uploadPath = configuration["ImageUpload:UploadPath"];

            if (string.IsNullOrEmpty(_uploadPath))
            {
                throw new ArgumentNullException(nameof(_uploadPath), "Upload path is not configured.");
            }

            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }
        }

        public async Task<string> GetImageDescriptionAsync(string imagePath)
        {
            using (var stream = File.OpenRead(imagePath))
            {
                var descriptionResult = await _computerVisionClient.DescribeImageInStreamAsync(stream);
                return descriptionResult.Captions.FirstOrDefault()?.Text ?? "No description available";
            }
        }

        public string SaveImage(IFormFile image)
        {
            var fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(image.FileName);
            var filePath = Path.Combine(_uploadPath, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                image.CopyTo(fileStream);
            }

            return filePath;
        }
    }


}
