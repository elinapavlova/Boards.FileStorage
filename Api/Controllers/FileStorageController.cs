using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Result;
using Core.Dto.File;
using Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("/api/[controller]")]
    [ApiController]
    public class FileStorageController : ControllerBase
    {
        private readonly IFileStorageService _fileStorageService;
        
        public FileStorageController(IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }
        
        [HttpPost("Upload")]
        public async Task<ResultContainer<ICollection<FileResponseDto>>> Upload(IFormFileCollection files)
            => await _fileStorageService.Upload(files);
    }
}