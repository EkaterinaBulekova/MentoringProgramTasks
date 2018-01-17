using System;
using System.Collections.Generic;
using System.Text;

namespace FileSystemVisitorLibrary.Data
{
    public enum FileSystemItemType
    {
        File,
        Directory
    }

    public class FileSystemItem
    {
        public FileSystemItemType Type { get; set; }        

        public string Name { get; set; }

        public DateTime Date { get; set; }
    }
}
