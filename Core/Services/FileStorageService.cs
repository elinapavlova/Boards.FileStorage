using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Common.Error;
using Common.Result;
using Core.Dto.File;
using Core.Options;
using Microsoft.AspNetCore.Http;

namespace Core.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IMapper _mapper;
        private readonly string _basePath;
        private readonly Dictionary<string, string> _catalogues;

        public FileStorageService
        (
            IMapper mapper, 
            FileStorageOptions options
        )
        {
            _mapper = mapper;
            _basePath = options.BasePath;
            _catalogues = options.CataloguesName;
        }
        
        public async Task<ResultContainer<ICollection<FileResponseDto>>> Upload(IFormFileCollection files)
        {
            ResultContainer<ICollection<FileResponseDto>> result;

            // Если есть файлы с неподдерживаемым типом
            if (files.Any(f => !ContentType.ContentTypes.ContentType.Contains(f.ContentType)))
            {
                result = CreateBadResult(files);
                return result;
            }

            result = await UploadFiles(files);
            return result;
        }
        
        private ResultContainer<ICollection<FileResponseDto>> CreateBadResult(IFormFileCollection files)
        {
            var result = new ResultContainer<ICollection<FileResponseDto>>
            {
                Data = new List<FileResponseDto>()
            };
            
            foreach (var file in files)
            {
                // Если у файла неподдерживаемый тип
                if (!ContentType.ContentTypes.ContentType.Contains(file.ContentType))
                {
                    var invalidFile = _mapper.Map<FileResponseDto>(file);
                    result.Data.Add(invalidFile);
                    break;
                }
                
                result.Data.Add(_mapper.Map<FileResponseDto>(file));
            }
            
            result.ErrorType = ErrorType.BadRequest;
            
            return result;
        }
        
        private async Task<ResultContainer<ICollection<FileResponseDto>>> UploadFiles(IFormFileCollection files)
        {
            var result = new ResultContainer<ICollection<FileResponseDto>>
            {
                Data = new List<FileResponseDto>()
            };
            
            foreach (var file in files)
            {
                var absolutePath = CreateAbsolutePath(file.FileName);
                
                await using var fileStream = File.Create(absolutePath);
                await file.CopyToAsync(fileStream);
                
                var fileResponse = _mapper.Map<FileResponseDto>(file);
                fileResponse.DateCreated = DateTime.Now;
                fileResponse.Path = absolutePath;
                fileResponse.Extension = Path.GetExtension(file.FileName);
                result.Data.Add(_mapper.Map<FileResponseDto>(fileResponse));
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
                case ".wav" :
                    _catalogues.TryGetValue("Audio",  out catalogue);
                    absolutePath = Path.Combine(_basePath, catalogue, Guid.NewGuid() + extension);
                    break;
                case ".txt" :
                    _catalogues.TryGetValue("Text",  out catalogue);
                    absolutePath = Path.Combine(_basePath, catalogue, Guid.NewGuid() + extension);
                    break;
                case ".png" :
                case ".jpg" :
                    _catalogues.TryGetValue("Images",  out catalogue);
                    absolutePath = Path.Combine(_basePath, catalogue, Guid.NewGuid() + extension);
                    break;
            }

            return absolutePath;
        }
    }
}