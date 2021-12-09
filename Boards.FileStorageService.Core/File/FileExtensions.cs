using System.Collections.Generic;

namespace Boards.FileStorageService.Core.File
{
    public static class FileExtensions
    {
        public static readonly IList<string> Extensions = new List<string>
        {
            ".mp4",
            ".png",
            ".jpg",
            ".jpeg"
        };
    }
}