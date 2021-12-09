using System;
using System.Collections.Generic;
using Boards.FileStorageService.Database.Models;

namespace Boards.FileStorageService.Database.Repositories
{
    public interface IFileRepository
    {
        IEnumerable<FileModel> Get(Func<FileModel, bool> predicate);
    }
}