using Microsoft.AspNetCore.Http;

namespace Bloggy.Shared.Extension
{
    public static class ValidateFileExtension
    {
        public static bool IsImage(this IFormFile UploadedFile)
        {
            if (UploadedFile == null)
            {
                return true;
            }

            if (!string.Equals(UploadedFile.ContentType, "image/jpg", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(UploadedFile.ContentType, "image/jpeg", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(UploadedFile.ContentType, "image/pjpeg", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(UploadedFile.ContentType, "image/gif", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(UploadedFile.ContentType, "image/x-png", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(UploadedFile.ContentType, "image/png", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            var postedFileExtension = Path.GetExtension(UploadedFile.FileName);
            if (!string.Equals(postedFileExtension, ".jpg", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(postedFileExtension, ".png", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(postedFileExtension, ".gif", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(postedFileExtension, ".jpeg", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }
    }
}
