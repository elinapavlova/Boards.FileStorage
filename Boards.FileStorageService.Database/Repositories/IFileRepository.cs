using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Boards.FileStorageService.Database.Models;

namespace Boards.FileStorageService.Database.Repositories
{
    public interface IFileRepository
    {
        IEnumerable<FileModel> GetByMessageId(Guid id);
        IEnumerable<FileModel> GetByThreadId(Guid id);
        Task<FileModel> GetById(Guid id);
    }
}