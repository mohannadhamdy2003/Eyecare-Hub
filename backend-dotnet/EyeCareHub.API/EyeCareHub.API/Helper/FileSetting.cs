using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace EyeCareHub.API.Helper
{
    public class FileSetting
    {
        public static async Task<string> UploadFileAsync(IFormFile file,string BaseFolder ,string FolderName, HttpContext httpContext)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("No file uploaded!");
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".pdf" };
            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
            {
                throw new ArgumentException("Invalid file type. Only JPG, PNG, and PDF are allowed!"); 
            }

            var FolderPath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/{BaseFolder}", FolderName);

            if (!Directory.Exists(FolderPath))
            {
                Directory.CreateDirectory(FolderPath);
            }

            var FileName = $"{Guid.NewGuid()}{Path.GetFileName(file.FileName)}";
            var FilePath = Path.Combine(FolderPath, FileName);

            using (var fs = new FileStream(FilePath, FileMode.Create))
            {
                await file.CopyToAsync(fs); 
            }

            var baseUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}"; /// https:///localhost:5001
            var fileUrl = $"{BaseFolder}/{FolderName}/{FileName}";

            return fileUrl;
        }

        public static bool DeleteFile(string filePath)
        {
            try
            {
                var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/", filePath);
                

                if (!File.Exists(FilePath))
                {
                    throw new FileNotFoundException("Not file!");
                }

                File.Delete(FilePath);
                return true; 
            }
            catch (Exception ex)
            {
                return false; 
            }
        }

        //public static Task<string> UpdateFileAsync()

    }
}
