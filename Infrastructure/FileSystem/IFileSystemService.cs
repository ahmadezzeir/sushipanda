using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.FileSystem
{
    public interface IFileSystemService
    {
        Task<long> SaveFiles(ICollection<IFormFile> files);

        Task<string> SaveFile(IFormFile file);

        Task<byte[]> GetFile(string path, string fileName);

        Task MoveFileAsync(string fileName, string folder);
    }
}