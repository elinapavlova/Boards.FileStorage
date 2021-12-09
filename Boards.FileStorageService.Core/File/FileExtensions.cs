using System.Collections.Generic;

namespace Boards.FileStorageService.Core.File
{
    public static class FileExtensions
    {
        public static readonly Dictionary<string, string> Extensions = new Dictionary<string, string>
        {
            {".mp4", "video/mp4"},
            {".png", "image/png"},
            {".jpg", "image/jpeg"},
            {".jpeg", "image/jpeg"}
        };
    }
}