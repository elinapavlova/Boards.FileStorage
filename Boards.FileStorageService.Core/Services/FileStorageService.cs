using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

            var result = await UploadFiles(files);
            return result;
        }

        public async Task<FileResultDto> GetFile(Uri uri)
        {
            var name = uri.AbsolutePath.Split('/').LastOrDefault();
            if (name == null)
                return null;
            
            var path = Directory.GetFiles(_basePath, name, SearchOption.AllDirectories).FirstOrDefault();
            if (path == null)
                return null;
            
            var contentType = FileExtensions.Extensions.Keys
                .FirstOrDefault(f => f.Equals(Path.GetExtension(name)));

            await using var filestream = new FileStream(path, FileMode.Open, FileAccess.Read);
            var bytes = new byte[filestream.Length];
            await filestream.ReadAsync(bytes.AsMemory(0, (int) filestream.Length));

            var file = new FileResultDto
            {
                Bytes = bytes,
                ContentType = contentType,
                Url = uri
            };
            return file;
        }

        public async Task<ICollection<FileResultDto>> GetByThreadId(Guid id)
        {
            var files = _fileRepository.Get(f => f.ThreadId == id && f.MessageId == null);
            var result = new Collection<FileResultDto>();
            foreach (var uri in files.Select(file => CreateUrl(file.Path)))
                result.Add(await GetFile(uri));

            return result;
        }
        
        public async Task<ICollection<FileResultDto>> GetByMessageId(Guid id)
        {
            var files = _fileRepository.Get(f => f.MessageId == id);
            var result = new Collection<FileResultDto>();
            foreach (var uri in files.Select(file => CreateUrl(file.Path)))
                result.Add(await GetFile(uri));

            return result;
        }

        private async Task<ICollection<FileResponseDto>> UploadFiles(IFormFileCollection files)
        {
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