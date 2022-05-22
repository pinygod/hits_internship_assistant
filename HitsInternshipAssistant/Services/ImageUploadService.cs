namespace HitsInternshipAssistant.Services
{
    public class ImageUploadService
    {
        private static readonly HashSet<string> AllowedExtensions = new() { ".jpg", ".jpeg", ".png", ".gif" };
        private static readonly string AttachmentsFolder = "attachments";


        private readonly IWebHostEnvironment hostingEnvironment;

        public ImageUploadService(IWebHostEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        public async Task<string> UploadAsync(IFormFile image)
        {
            var fileName = Path.GetFileName(image.FileName);
            var fileExt = Path.GetExtension(fileName);
            if (AllowedExtensions.Contains(fileExt))
            {
                throw new ArgumentException("This file type is prohibited");
            }

            var imagePath = Path.Combine(hostingEnvironment.WebRootPath, AttachmentsFolder, image.FileName);
            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            return imagePath;
        }
    }
}
