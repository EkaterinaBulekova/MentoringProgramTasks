using System.Collections.Generic;
using System.IO;
using System.Linq;
using FileSystemVisitorLibrary.Data;

namespace FileSystemVisitorLibrary.Infrastructure
{
    /// <summary>
    /// Provide work with File System tree
    /// </summary>
    public class FileSystemInfoProvider : IFileSystemInfoProvider
    {
        /// <summary>
        /// Iterates File System tree  starts on certain path and returns Enumerable of FileSystemItem
        /// </summary>
        /// <param name="path"> A string specifying the path on which to create the FileSystemItem. </param>
        /// <returns> IEnumerable of FileSystemItem </returns>
        public IEnumerable<FileSystemItem> GetFileSystemItems(string path)
        {
            if (Directory.Exists(path))
            {
                List<FileSystemItem> directoryItems = new List<FileSystemItem>();
                try
                {
                    directoryItems = Directory.GetFileSystemEntries(path).Select(_ => new FileSystemItem(_)).ToList();
                }
                catch
                {
                    // ignored
                }

                foreach (var entity in directoryItems)
                {
                    var isFile = entity.Type == FileSystemItemType.File;
                    yield return entity;
                    if (!isFile)
                    {
                        foreach (var item in this.GetFileSystemItems(entity.FullName))
                        {
                            yield return item;
                        }
                    }
                }
            }
        }
    }
}
