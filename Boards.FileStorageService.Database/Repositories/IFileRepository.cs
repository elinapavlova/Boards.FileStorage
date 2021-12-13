using System;
using System.Threading.Tasks;
using Boards.FileStorageService.Database.Models;

namespace Boards.FileStorageService.Database.Repositories
{
    public interface IFileRepository
    {
        Task<FileModel> GetById(Guid id);
    }
}