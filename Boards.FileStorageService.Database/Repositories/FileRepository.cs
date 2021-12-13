using System;
using System.Threading.Tasks;
using Boards.FileStorageService.Database.Models;

namespace Boards.FileStorageService.Database.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly AppDbContext _context;
        
        public FileRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<FileModel> GetById(Guid id)
        {
            return await _context.Set<FileModel>().FindAsync(id);
        }
    }
}