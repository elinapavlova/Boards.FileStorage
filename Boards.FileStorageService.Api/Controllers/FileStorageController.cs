using System;
using System.Collections.Generic;
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
        /// Get file by url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [HttpPost("Get-File")]
        [AllowAnonymous]
        public async Task<FileResultDto> GetFile(Uri url)
            => await _fileStorageService.GetFile(url);

        /// <summary>
        /// Get files by thread Id
        /// </summary>
        /// <returns></returns>
        [HttpPost("By-Thread-Id/{id:guid}")]
        [AllowAnonymous]
        public async Task<ICollection<FileResultDto>> GetByThreadId(Guid id)
            => await _fileStorageService.GetByThreadId(id);
        
        /// <summary>
        /// Get files by message Id
        /// </summary>
        /// <returns></returns>
        [HttpPost("By-Message-Id/{id:guid}")]
        [AllowAnonymous]
        public async Task<ICollection<FileResultDto>> GetByMessageId(Guid id)
            => await _fileStorageService.GetByMessageId(id);
    }
}