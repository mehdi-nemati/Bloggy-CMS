using Bloggy.Shared.Config;
using Microsoft.AspNetCore.Http;

namespace Bloggy.Service.UploadService
{
    public class UploadService : IUploadService, ISingletonDependency
    {
        public async Task<string> UploadFile(IFormFile file, string directory)
        {
            if (BloggyConst.DoNotChange)
                return "/admin/images/bloggy-logo.jpg";

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName).ToLower();
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/" + directory);
            var filePath = Path.Combine(folderPath, fileName);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            using (var stream = File.Create(filePath))
            {
                await file.CopyToAsync(stream);
            }

            return $"/{directory}/{fileName}";
        }

        /// <summary>
        /// Delete uploaded file from wwwroot
        /// </summary>
        public bool DeleteFileFromStorage(string Imagename)
        {
            if (BloggyConst.DoNotChange)
                return true;

            if (string.IsNullOrEmpty(Imagename)) return false;

            var FilePath = Directory.GetCurrentDirectory() + "/wwwroot/" + Imagename;
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
                return true;
            }
            return false;
        }
    }
}
