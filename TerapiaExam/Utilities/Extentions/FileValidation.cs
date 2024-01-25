﻿namespace TerapiaExam.Utilities.Extentions
{
    public static class FileValidation
    {
        public static bool ValidateType(this IFormFile file, string type = "image/")
        {
            if(file.ContentType.Contains(type))
            {
                return true;
            }
            return false;
        }
        public static bool ValidateSize(this IFormFile file, int limitKb = 2)
        {
            if (file.Length<=limitKb*1024*1024)
            {
                return true;
            }
            return false;
        }

        private static string CreatePath(string root, string fileName, params string[] folders)
        {
            string path = root;
            foreach (var folder in folders)
            {
                path = Path.Combine(path, folder);
            }
            path = Path.Combine(path, fileName);
            return path;
        }

        public static async Task<string>CreateFileAsync(this IFormFile file, string root, params string[] folders)
        {
            string extension = Path.GetExtension(file.FileName);
            string fileName = $"{Guid.NewGuid()}{extension}";
            string path = CreatePath(root, fileName, folders);
            using(FileStream fs = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(fs);
            }
            return fileName;
        }

        public static void DeleteFile(this string fileName, string root, params string[] folders)
        {
            string path = CreatePath(root, fileName, folders);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

    }
}
