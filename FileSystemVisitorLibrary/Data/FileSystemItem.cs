using System;
using System.IO;

namespace FileSystemVisitorLibrary.Data
{
    public enum FileSystemItemType
    {
        File,
        Directory
    }

    /// <summary>
    /// A class represent short informaition about file or directory
    /// </summary>
    public class FileSystemItem
    {
        /// <summary>
        /// Initializes a new instance of the FileSystemItem class.
        /// </summary>
        public FileSystemItem()
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the FileSystemItem class on the specified path.
        /// </summary>
        /// <param name="systemItem"> A string specifying the path on which to create the FileSystemItem. </param>
        public FileSystemItem(string systemItem)
        {
            var fileSystemInfo =
                Directory.Exists(systemItem) ? (FileSystemInfo) new DirectoryInfo(systemItem) : new FileInfo(systemItem);
            Type = (fileSystemInfo.Attributes & FileAttributes.Directory) == FileAttributes.Directory
                ? FileSystemItemType.Directory
                : FileSystemItemType.File;
            Name = fileSystemInfo.Name;
            FullName = fileSystemInfo.FullName;
            Date = fileSystemInfo.CreationTime;
        }

        public FileSystemItemType Type { get; set; }
        public string Name { get; set; }

        /// <summary>
        ///  Gets the full path of the directory or file.
        /// </summary>
        public string FullName { get; private set; }
        public DateTime Date { get; set; }
    }
}
