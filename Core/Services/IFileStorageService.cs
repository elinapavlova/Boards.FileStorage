using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Result;
using Core.Dto.File;
using Microsoft.AspNetCore.Http;

namespace Core.Services
{
    public interface IFileStorageService
    {
        Task<ResultContainer<ICollection<FileResponseDto>>> Upload(IFormFileCollection files);
    }
}