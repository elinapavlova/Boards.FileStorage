using System.IO;

namespace Boards.FileStorageService.Core.Dto.File
{
    public class FileResultDto
    {
        public FileStream Stream { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
    }
}