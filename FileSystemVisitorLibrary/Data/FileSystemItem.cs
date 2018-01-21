using System;
using System.IO;

namespace FileSystemVisitorLibrary.Data
{
    /// <summary>
    /// Represents enumeration type of FileSystemItem - Directory or File
    /// </summary>
    public enum FileSystemItemType
    {
        /// <summary>
        /// If FileSystemItem is file
        /// </summary>
        File,

        /// <summary>
        /// If FileSystemItem is Directory
        /// </summary>
        Directory
    }

    /// <summary>
    /// A class represent short information about file or directory
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
            var fileSystemInfo = Directory.Exists(systemItem)
                ? (FileSystemInfo)new DirectoryInfo(systemItem)
                : new FileInfo(systemItem);
            Type = (fileSystemInfo.Attributes & FileAttributes.Directory) == FileAttributes.Directory
                ? FileSystemItemType.Directory
                : FileSystemItemType.File;
            Name = fileSystemInfo.Name;
            FullName = fileSystemInfo.FullName;
            Date = fileSystemInfo.CreationTime;
        }

        /// <summary>
        ///  Gets or sets the type of the item - directory or file.
        /// </summary>
        public FileSystemItemType Type { get; set; }

        /// <summary>
        ///  Gets or sets the name of the directory or file.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///  Gets or sets the full path of the directory or file.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        ///  Gets or sets the creating date of the directory or file.
        /// </summary>
        public DateTime Date { get; set; }
    }
}
