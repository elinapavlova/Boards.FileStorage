using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Result;
using Boards.FileStorageService.Core.Dto.File;
using Boards.FileStorageService.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Boards.FileStorageService.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("/api/[controller]/[action]")]
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
        [HttpPost]
        public async Task<ResultContainer<ICollection<FileResponseDto>>> Upload(IFormFileCollection files)
            => await _fileStorageService.Upload(files);
    }
}