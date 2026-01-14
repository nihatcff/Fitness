using System.Threading.Tasks;

namespace Fitness.Helpers
{
    public static class ExtensionMethods
    {
        public static bool CheckSize(this IFormFile file, int mb)
        {
            return file.Length < mb * 1024 * 1024;
        }
        public static bool CheckType(this IFormFile file, string type)
        {
            return file.ContentType.Contains(type);
        }

        public static async Task<string> UploadFileAsync(this IFormFile file, string folderpath)
        {
            string uniqueFolderName = Guid.NewGuid().ToString() + file.FileName;
            string path = Path.Combine(folderpath, uniqueFolderName);
            using FileStream stream = new(path, FileMode.Create);

            await file.CopyToAsync(stream);
                       

            return uniqueFolderName;

        }

        public static void DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
