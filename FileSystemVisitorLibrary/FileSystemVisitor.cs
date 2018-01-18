using FileSystemVisitorLibrary.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileSystemVisitorLibrary
{
    public class SystemVisitorEventArgs : EventArgs
    {
        public int Count { get; set; }
        public bool IsStop { get; set; }
        public bool IsSkip { get; set; }
        public FileSystemItem FoundItem { get; set; }
    }

    public class FileSystemVisitor : IEnumerable<FileSystemItem>
    {
        private string _path;
        private int _level;
        private int _count;
        private bool _isStop;
        private bool _isSkip;
        private Func<FileSystemItem, bool> _filter;

        public FileSystemVisitor(string path, Func<FileSystemItem, bool> filter = null)
        {
            _count = 0;
            _level = 0;
            _path = path;
            _filter = filter;
        }

        public event EventHandler<SystemVisitorEventArgs> Finish;
        public event EventHandler<SystemVisitorEventArgs> Start;
        public event EventHandler<SystemVisitorEventArgs> FileFound;
        public event EventHandler<SystemVisitorEventArgs> DirectoryFound;
        public event EventHandler<SystemVisitorEventArgs> FilteredFileFound;
        public event EventHandler<SystemVisitorEventArgs> FilteredDirectoryFound;

        protected virtual void OnStart(SystemVisitorEventArgs e)
        {
            Start?.Invoke(this, e);
        }

        protected virtual void OnFinish(SystemVisitorEventArgs e) => Finish?.Invoke(this, e);

        protected virtual void OnFileFound(SystemVisitorEventArgs e)
        {
            FileFound?.Invoke(this, e);
            _isStop = e.IsStop;
            _isSkip = e.IsSkip;
        }

        protected virtual void OnDirectoryFound(SystemVisitorEventArgs e)
        {
            DirectoryFound?.Invoke(this, e);
            _isStop = e.IsStop;
            _isSkip = e.IsSkip;
        }

        protected virtual void OnFilteredFileFound(SystemVisitorEventArgs e)
        {
            FilteredFileFound?.Invoke(this, e);
            _isStop = e.IsStop;
            _isSkip = e.IsSkip;
        }

        protected virtual void OnFilteredDirectoryFound(SystemVisitorEventArgs e)
        {
            FilteredDirectoryFound?.Invoke(this, e);
            _isStop = e.IsStop;
            _isSkip = e.IsSkip;
        }

        private IEnumerable<FileSystemItem> GetFileSystemEnumerable()
        {
            OnStart(null);
            var filteredFiles = GetFileSystemItems().Where(_filter).ToList();
            foreach (var fileSystemItem in GetFileSystemItems())
            {
                var isFile = fileSystemItem.Type == FileSystemItemType.File;
                var e = new  SystemVisitorEventArgs{ Count = _count, IsStop = false, IsSkip = false, FoundItem = fileSystemItem};

                if (isFile)
                {
                    OnFileFound(e);
                }
                else
                {
                    OnDirectoryFound(e);
                }
                 
                if (filteredFiles.Contains(fileSystemItem))
                {
                    if (isFile)
                    {
                        OnFilteredFileFound(e);
                    }
                    else
                    {
                        OnFilteredDirectoryFound(e);
                    }

                    yield return fileSystemItem;
                }
            }
            OnFinish(null);
        }


        private IEnumerable<FileSystemItem> GetFileSystemItems()
        {
            //if(_level == 0)
            //{
            //    OnStart(null);
            //}

            //_level++;
            if (Directory.Exists(_path))
            {
                _count++;
                DirectoryInfo dir = new DirectoryInfo(_path);
                yield return new FileSystemItem()
                {
                    Name = dir.Name,
                    Type = FileSystemItemType.Directory,
                    Date = dir.LastWriteTime
                };
                string[] files;
                try
                {
                    files = Directory.GetFiles(_path);
                }
                catch
                {
                    files = null;
                }

                if (files != null && files.Length > 0)
                    foreach (string f in files)
                    {
                        FileInfo file = new FileInfo(f);
                        yield return new FileSystemItem()
                        {
                            Name = file.Name,
                            Type = FileSystemItemType.File,
                            Date = file.LastWriteTime
                        };
                    }

                string[] dirs;
                try
                {
                    dirs = Directory.GetDirectories(_path);
                }
                catch
                {
                    dirs = null;
                }

                if (dirs != null && dirs.Length > 0)
                  foreach (var directory in dirs)
                  {
                      _path = directory;
                      foreach (var item in GetFileSystemEnumerable())
                      {
                          yield return item;
                      }
                  }
            }

            //_level--;
            //if (_level == 0)
            //{
            //    OnFinish(null);
            //}
        }

        public IEnumerator<FileSystemItem> GetEnumerator()
        {
            return GetFileSystemEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetFileSystemEnumerable()?.GetEnumerator() ?? throw new InvalidOperationException();
        }
    }
}
