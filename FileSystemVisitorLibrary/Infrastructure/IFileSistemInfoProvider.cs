using FileSystemVisitorLibrary.Data;
using System.Collections.Generic;

namespace FileSystemVisitorLibrary.Infrastructure
{
    /// <summary>
    /// The interface provide work with File System
    /// </summary>
    public interface IFileSystemInfoProvider
    {
        /// <summary>
        /// Iterates File System entities
        /// </summary>
        /// <param name="path">A string specifying the path on which to start iteration.</param>
        /// <returns>Inumerable of FileSystemItem</returns>
        IEnumerable<FileSystemItem> GetFileSystemItems(string path);
    }
}
