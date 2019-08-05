using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Services.Interfaces
{
    public interface IFileService
    {
        Task<(Guid id, string name)> SaveFileAsync(IFormFile formFile);
    }
}