using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public IEnumerable<FileModel> GetByMessageId(Guid id)
        {
            return _context
                .Set<FileModel>()
                .AsNoTracking()
                .AsEnumerable()
                .Where(f => f.MessageId == id)
                .ToList();
        }

        public IEnumerable<FileModel> GetByThreadId(Guid id)
        {
            return _context
                .Set<FileModel>()
                .AsNoTracking()
                .AsEnumerable()
                .Where(f => f.ThreadId == id && f.MessageId == null)
                .ToList();
        }

        public async Task<FileModel> GetById(Guid id)
        {
            return await _context.Set<FileModel>().FindAsync(id);
        }
    }
}