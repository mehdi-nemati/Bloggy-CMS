using Microsoft.AspNetCore.Http;

namespace Bloggy.Service.UploadService
{
    public interface IUploadService
    {
        Task<string> UploadFile(IFormFile file, string directory);
        bool DeleteFileFromStorage(string Imagename);
    }
}
