using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Boards.FileStorageService.Core.Dto.File;
using Boards.FileStorageService.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Boards.FileStorageService.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("/api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class FileStorageController : ControllerBase
    {
        private readonly IFileStorageService _fileStorageService;
        
        public FileStorageController(IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }
        
        /// <summary>
        /// Upload files to storage
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost("Upload")]
        [AllowAnonymous]
        public async Task<ICollection<FileResponseDto>> Upload(IFormFileCollection files)
            => await _fileStorageService.Upload(files);

        /// <summary>
        /// Get file by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<FileResult> GetFileById(Guid id)
        {
            var result = await _fileStorageService.GetById(id);
            return File(result.Stream, result.ContentType);
        }

        /// <summary>
        /// Get files by thread Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("Thread/{id:guid}/Files")]
        [AllowAnonymous]
        public async Task<Collection<FileResult>> GetByThreadId(Guid id)
        {
            var files = await _fileStorageService.GetByThreadId(id);
            var result = new Collection<FileResult>();
            
            foreach(var file in files)
                result.Add(File(file.Stream, file.ContentType, file.FileName));
            return result;
        }
        
        /// <summary>
        /// Get files by message Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("Message/{id:guid}/Files")]
        [AllowAnonymous]
        public async Task<ICollection<FileResult>> GetByMessageId(Guid id)
        {
            var files = await _fileStorageService.GetByMessageId(id);
            var result = new Collection<FileResult>();
            
            foreach(var file in files)
                result.Add(File(file.Stream, file.ContentType, file.FileName));
            return result;
        }
    }
}