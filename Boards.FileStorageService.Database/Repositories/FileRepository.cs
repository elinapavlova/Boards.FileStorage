using System;
using System.Collections.Generic;
using System.Linq;
using Boards.FileStorageService.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Boards.FileStorageService.Database.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly AppDbContext _context;
        
        public FileRepository(AppDbContext context)
        {
            _context = context;
        }
        
        public IEnumerable<FileModel> Get(Func<FileModel, bool> predicate)
        {
            return _context
                .Set<FileModel>()
                .AsNoTracking()
                .AsEnumerable()
                .Where(predicate)
                .ToList();
        }
    }
}