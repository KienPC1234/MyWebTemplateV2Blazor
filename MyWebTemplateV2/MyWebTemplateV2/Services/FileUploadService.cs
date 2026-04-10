using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Components.Forms;
using System.IO;

namespace MyWebTemplateV2.Services
{
    public class FileUploadService
    {
        private readonly IWebHostEnvironment _environment;

        public FileUploadService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> UploadFileAsync(IBrowserFile file, string subFolder)
        {
            var uploads = Path.Combine(_environment.WebRootPath, "uploads", subFolder);
            if (!Directory.Exists(uploads))
            {
                Directory.CreateDirectory(uploads);
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.Name);
            var filePath = Path.Combine(uploads, fileName);

            using var stream = file.OpenReadStream(maxAllowedSize: 1024 * 1024 * 50); // 50MB
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await stream.CopyToAsync(fileStream);

            return $"/uploads/{subFolder}/{fileName}";
        }
    }
}
