using System.Collections.Generic;

namespace Boards.FileStorageService.Core.ContentType
{
    public static class ContentTypes
    {
        public static readonly IList<string> ContentType = new List<string>
        {
            "image/jpeg",
            "image/png",
            "text/plain",
            "audio/wav",
            "video/mp4"
        };
    }
}