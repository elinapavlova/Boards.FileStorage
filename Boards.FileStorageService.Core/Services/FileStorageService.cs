using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Boards.FileStorageService.Core.Options;
using Boards.Auth.Common.Options;
using Boards.FileStorageService.Core.Dto.File;
using Boards.FileStorageService.Core.File;
using Boards.FileStorageService.Database.Repositories;
using Microsoft.AspNetCore.Http;

namespace Boards.FileStorageService.Core.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly string _basePath;
        private readonly Dictionary<string, string> _catalogues;
        private readonly Uri _baseUri;
        private readonly IFileRepository _fileRepository;

        public FileStorageService
        (
            FileStorageOptions options,
            AppOptions appOptions,
             IFileRepository fileRepository
        )
        {
            _basePath = options.BasePath;
            _catalogues = options.CataloguesName;
            _baseUri = new Uri(appOptions.Urls);
            _fileRepository = fileRepository;
        }
        
        public async Task<ICollection<FileResponseDto>> Upload(IFormFileCollection files)
        {
            if (files.Any(file => !FileExtensions.Extensions.ContainsKey(Path.GetExtension(file.FileName))))
                return null;

            var result = new List<FileResponseDto>();
            
            foreach (var file in files)
            {
                var absolutePath = CreateAbsolutePath(file.FileName);
                
                await using var fileStream = System.IO.File.Create(absolutePath);
                await file.CopyToAsync(fileStream);

                var newfile = new FileResponseDto
                {
                    Name = file.FileName,
                    Path = absolutePath,
                    Extension = Path.GetExtension(absolutePath),
                    Url = CreateUrl(absolutePath)
                };
                result.Add(newfile);
            }
            return result;
        }

        public async Task<FileResultDto> GetById(Guid id)
        {
            var file = await _fileRepository.GetById(id);
            if (file == null)
                return null;
            
            var name = Path.GetFileName(file.Path);
            if (name == null)
                return null;
            
            var contentType = FileExtensions.Extensions
                .FirstOrDefault(f => f.Key.Equals(file.Extension))
                .Value;

            var filestream = new FileStream(file.Path, FileMode.Open, FileAccess.Read);
            var result = new FileResultDto
            {
                Stream = filestream,
                ContentType = contentType,
                FileName = name
            };
            return result;
        }

        private string CreateAbsolutePath(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            var absolutePath = "";
            
            switch (extension)
            {
                case ".mp4" :
                    _catalogues.TryGetValue("Video",  out var catalogue);
                    absolutePath = Path.Combine(_basePath, catalogue, Guid.NewGuid() + extension);
                    break;
                case ".png" :
                case ".jpg" :
                case ".jpeg":
                    _catalogues.TryGetValue("Images",  out catalogue);
                    absolutePath = Path.Combine(_basePath, catalogue, Guid.NewGuid() + extension);
                    break;
            }

            return absolutePath;
        }

        private Uri CreateUrl(string path)
        {
            var name = Path.GetFileName(path);
            var catalogue = Directory.GetParent(path);
            var pathToFile = Path.Combine("api/v1/", catalogue.Parent.Name, catalogue.Name, name);
            var url = new Uri(_baseUri, pathToFile);
            return url;
        }
    }
}