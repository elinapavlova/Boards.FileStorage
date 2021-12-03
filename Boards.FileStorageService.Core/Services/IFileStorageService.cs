using System.Collections.Generic;
using System.Threading.Tasks;
using Boards.FileStorageService.Core.Dto.File;
using Common.Result;
using Microsoft.AspNetCore.Http;

namespace Boards.FileStorageService.Core.Services
{
    public interface IFileStorageService
    {
        Task<ResultContainer<ICollection<FileResponseDto>>> Upload(IFormFileCollection files);
    }
}