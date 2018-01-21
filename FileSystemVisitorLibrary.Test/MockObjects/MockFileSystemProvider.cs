using System;
using System.Collections.Generic;
using FileSystemVisitorLibrary.Data;
using FileSystemVisitorLibrary.Infrastructure;

namespace FileSystemVisitorLibrary.Test.MockObjects
{
    public class MockFileSystemProvider : IFileSystemInfoProvider
    {
        public IEnumerable<FileSystemItem> GetFileSystemItems(string path)
        {
            var fileSystemItems = new List<FileSystemItem>
            {
                new FileSystemItem
                {
                    Date = new DateTime(2017, 1, 1),
                    Name = "FileName1",
                    Type = FileSystemItemType.File
                },
                new FileSystemItem
                {
                    Date = new DateTime(2017, 1, 2),
                    Name = "DirectoryName1",
                    Type = FileSystemItemType.Directory
                },
                new FileSystemItem
                {
                    Date = new DateTime(2017, 1, 1),
                    Name = "FileName2",
                    Type = FileSystemItemType.File
                },
                new FileSystemItem
                {
                    Date = new DateTime(2017, 1, 3),
                    Name = "FileName3",
                    Type = FileSystemItemType.File
                },
                new FileSystemItem
                {
                    Date = new DateTime(2017, 1, 1),
                    Name = "DirectoryName2",
                    Type = FileSystemItemType.Directory
                }
            };
            return fileSystemItems;
        }
    }
}
