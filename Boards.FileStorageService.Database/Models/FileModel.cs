using System;
using Boards.Auth.Common.Base;

namespace Boards.FileStorageService.Database.Models
{
    public class FileModel : BaseModel
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Extension { get; set; }
        public Guid ThreadId { get; set; }
        public Guid? MessageId { get; set; }
    }
}