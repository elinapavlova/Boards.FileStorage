using Boards.FileStorageService.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Boards.FileStorageService.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<FileModel> Files { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<FileModel>(file =>
            {
                file.Property(f => f.Name).IsRequired().HasMaxLength(30);
                file.Property(f => f.Path).IsRequired().HasMaxLength(100);
                file.Property(f => f.Extension).IsRequired().HasMaxLength(5);
                file.Property(f => f.DateCreated).IsRequired();
            });
        }
    }
}