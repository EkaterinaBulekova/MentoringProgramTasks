using System;
using System.Collections.Generic;
using System.IO;

namespace FileSystemVisitorLibrary.Data
{
    public enum FileSystemItemType
    {
        File,
        Directory
    }

    public class FileSystemItem
    {
        public FileSystemItem(FileSystemInfo fsInfo)
        {
            Type = ((fsInfo.Attributes & FileAttributes.Directory) == FileAttributes.Directory) ? FileSystemItemType.Directory : FileSystemItemType.File;
            Name = fsInfo.FullName;
            Date = fsInfo.CreationTime;
        }

        public FileSystemItemType Type { get; set; }        

        public string Name { get; set; }

        public DateTime Date { get; set; }
    }
}
