using System;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace UserManagement.PL.Helpers
{
    public static class DocumentSettings
    {
        public static string UploadFile(IFormFile file, string folderName)
        {
            if(file is not null)
            {
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", folderName);
            
                string FileName = $"{Guid.NewGuid()}{file.FileName}";

                string FilePath = Path.Combine(folderPath, FileName);
            
                using var fs = new FileStream(FilePath, FileMode.Create);

                file.CopyTo(fs);
                
                return FileName;
            }
            else
                return null;
            

            
        }
        public static void DeleteFile(string fileName, string folderName) {
            if (fileName is not null && folderName is not null)
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", folderName, fileName);
                if(File.Exists(filePath) )
                    File.Delete(filePath);
            }
            
        }
        
    }
}
