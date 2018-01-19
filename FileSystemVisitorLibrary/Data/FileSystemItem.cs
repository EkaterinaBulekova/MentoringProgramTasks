using System;
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

        /// <summary>
        /// Initializes a new instance of the FileSystemItem class on the specified path.
        /// </summary>
        /// <param name="systemItem"> A string specifying the path on which to create the FileSystemItem. </param>
        public FileSystemItem(string systemItem)
        {
            var fileSystemInfoInfo =
                Directory.Exists(systemItem) ? (FileSystemInfo) new DirectoryInfo(systemItem) : new FileInfo(systemItem);
            Type = (fileSystemInfoInfo.Attributes & FileAttributes.Directory) == FileAttributes.Directory
                ? FileSystemItemType.Directory
                : FileSystemItemType.File;
            Name = fileSystemInfoInfo.FullName;
            Date = fileSystemInfoInfo.CreationTime;
        }

        public FileSystemItemType Type { get; set; }        

        public string Name { get; set; }

        public DateTime Date { get; set; }
    }
}
