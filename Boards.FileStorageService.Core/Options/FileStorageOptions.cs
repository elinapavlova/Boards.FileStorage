﻿using System.Collections.Generic;

namespace Boards.FileStorageService.Core.Options
{
    public class FileStorageOptions
    {
        public const string FileStorage = "FileStorageOptions";
        public string BasePath { get; set; }
        public Dictionary<string, string> CataloguesName { get; set; }
    }
}