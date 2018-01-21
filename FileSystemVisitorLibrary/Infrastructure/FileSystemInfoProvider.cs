using System.Collections.Generic;
using System.IO;
using System.Linq;
using FileSystemVisitorLibrary.Data;

namespace FileSystemVisitorLibrary.Infrastructure
{
    public class FileSystemInfoProvider: IFileSystemInfoProvider
    {
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
                        foreach (var item in GetFileSystemItems(entity.FullName))
                        {
                            yield return item;
                        }
                    }
                }
            }
        }

    }
}
