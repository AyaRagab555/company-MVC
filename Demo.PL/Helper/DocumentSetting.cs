using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Demo.PL.Helper
{
    public static class DocumentSetting
    {
        public static string UploadFile(IFormFile file , string FolderName)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", FolderName);


            var fileName = $"{Guid.NewGuid()} {Path.GetFileName(file.FileName)}";

            var filePath = Path.Combine(folderPath, fileName);

            using var fileStream = new FileStream(filePath , FileMode.Create);

            file.CopyTo(fileStream);

            return fileName;
        }

        public static void Delete(string folderName , string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot" ,folderName, fileName);
            if(File.Exists(filePath))
                 File.Delete(filePath); 
        }
    }
}
