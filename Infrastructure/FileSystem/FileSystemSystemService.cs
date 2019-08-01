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

        public FileSystemSystemService(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<long> SaveFiles(ICollection<IFormFile> files)
        {
            var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, "temp");

            foreach (var file in files)
            {
                if (file.Length <= 0) continue;

                using (var stream = new FileStream($"{filePath}/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}", FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }

            return files.Sum(f => f.Length);
        }

        public async Task<string> SaveFile(IFormFile file)
        {
            var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, "files/temp");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            using (var stream = new FileStream($"{filePath}/{fileName}", FileMode.Create))
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
    }
}
