using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Services.Interfaces
{
    public interface IFileService
    {
        Task<Guid> SaveFileAsync(IFormFile formFile);
    }
}