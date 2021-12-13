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
    }
}