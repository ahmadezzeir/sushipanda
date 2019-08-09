using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.FileSystem;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Files
{
    public class FileSystemSystemService : IFileSystemService
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly string _filePath;

        public FileSystemSystemService(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _filePath = Path.Combine(_hostingEnvironment.ContentRootPath, "files");
        }

        public async Task<long> SaveFiles(ICollection<IFormFile> files)
        {
            foreach (var file in files)
            {
                if (file.Length <= 0) continue;

                using (var stream = new FileStream($"{_filePath}/temp/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}", FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }

            return files.Sum(f => f.Length);
        }

        public async Task<string> SaveFile(IFormFile file)
        {
            var tempFilePath = $"{_filePath}/temp";

            if (!Directory.Exists(tempFilePath))
            {
                Directory.CreateDirectory(tempFilePath);
            }
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            using (var stream = new FileStream($"{tempFilePath}/{fileName}", FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }

        public async Task<byte[]> GetFile(string path, string fileName)
        {
            var fullPath = Path.Combine(path, fileName);
            var content = await File.ReadAllBytesAsync(fullPath);
            return content;
        }

        public async Task MoveFileAsync(string fileName, string folder)
        {
            var oldFilePath = $"{_filePath}/temp/{fileName}";
            var newFilePath = $"{_filePath}/{folder}";

            if (!File.Exists(oldFilePath))
            {
                throw new FileNotFoundException($"Cannot find file: {oldFilePath}");
            }

            if (!Directory.Exists(newFilePath))
            {
                Directory.CreateDirectory(newFilePath);
            }

            using (Stream source = File.Open(oldFilePath, FileMode.Open))
            {
                using (var stream = new FileStream($"{newFilePath}/{fileName}", FileMode.Create, FileAccess.Write))
                {
                    await source.CopyToAsync(stream);
                }
            }
        }
    }
}
