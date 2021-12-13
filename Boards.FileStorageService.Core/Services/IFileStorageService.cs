using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Boards.FileStorageService.Core.Dto.File;
using Microsoft.AspNetCore.Http;

namespace Boards.FileStorageService.Core.Services
{
    public interface IFileStorageService
    {
        Task<ICollection<FileResponseDto>> Upload(IFormFileCollection files);
        Task<FileResultDto> GetById(Guid id);
        Task<ICollection<FileResultDto>> GetByThreadId(Guid id);
        Task<ICollection<FileResultDto>> GetByMessageId(Guid id);
    }
}