using System;

namespace Core.Dto.File
{
    public class FileResponseDto
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Extension { get; set; }
        public DateTime DateCreated { get; set; }

    }
}