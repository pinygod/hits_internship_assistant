namespace HitsInternshipAssistant.Services
{
    public class FileUploadsService
    {
        private static readonly HashSet<string> AllowedExtensions = new() { ".jpg", ".jpeg", ".png", ".gif" };
        private static readonly string AttachmentsFolder = "attachments";


        private readonly IWebHostEnvironment hostingEnvironment;

        public FileUploadsService(IWebHostEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        public async Task<string> UploadImageAsync(IFormFile image)
        {
            var fileName = Path.GetFileName(image.FileName);
            var fileExt = Path.GetExtension(fileName);
            if (!AllowedExtensions.Contains(fileExt))
            {
                throw new ArgumentException("This file type is prohibited");
            }

            return await UploadFileAsync(image);
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            var filePath = Path.Combine(hostingEnvironment.WebRootPath, AttachmentsFolder, file.FileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return filePath;
        }
    }
}
