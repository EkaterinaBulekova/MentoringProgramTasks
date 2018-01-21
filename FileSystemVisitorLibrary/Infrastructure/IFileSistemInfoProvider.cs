using FileSystemVisitorLibrary.Data;
using System.Collections.Generic;

namespace FileSystemVisitorLibrary.Infrastructure
{
    public interface IFileSystemInfoProvider
    {
        IEnumerable<FileSystemItem> GetFileSystemItems(string path);
    }
}
