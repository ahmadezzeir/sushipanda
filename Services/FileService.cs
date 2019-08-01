using System;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using Infrastructure.FileSystem;
using Microsoft.AspNetCore.Http;
using Services.Abstractions;
using Services.Interfaces;
using File = Domain.Models.File;

namespace Services
{
    public class FileService : ServiceBaseSql, IFileService
    {
        private readonly IFileSystemService _fileSystemService;

        public FileService(IMapper mapper, IComponentContext scope, IFileSystemService fileSystemService) : base(mapper, scope)
        {
            _fileSystemService = fileSystemService;
        }

        public async Task<Guid> SaveFileAsync(IFormFile formFile)
        {
            var name = await _fileSystemService.SaveFile(formFile);

            var file = new File
            {
                Caption = formFile.Name,
                Name = name
            };

            var repository = UnitOfWork.Repository<File>();
            await repository.AddAsync(file);
            await UnitOfWork.CommitAsync();

            return file.Id;
        }
    }
}