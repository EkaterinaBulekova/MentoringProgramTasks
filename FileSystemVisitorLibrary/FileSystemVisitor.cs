using FileSystemVisitorLibrary.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace FileSystemVisitorLibrary
{
    public class SystemVisitorEventArgs : EventArgs
    {
        public bool IsStop { get; set; }
        public bool IsSkip { get; set; }
        public FileSystemItem FoundItem { get; set; }
    }

    public class FileSystemVisitor : IEnumerable
    {
        private readonly string _path;
        private int _count;
        private bool _isStop;
        private bool _isSkip;
        private Func<FileSystemItem, bool> _filter;

        public FileSystemVisitor() => _filter = ((item) => true);

        public FileSystemVisitor(Func<FileSystemItem, bool> filter, string path)
        {
            _path = path;
            _filter = filter;
        }


        protected virtual void OnStart(SystemVisitorEventArgs e)
        {
            Started?.Invoke(this, e);
        }

        public event EventHandler<SystemVisitorEventArgs> Finished;

        protected virtual void OnFinish(SystemVisitorEventArgs e)
        {
            Finished?.Invoke(this, e);
        }

        public event EventHandler<SystemVisitorEventArgs> Found;

        protected virtual void OnFound(SystemVisitorEventArgs e)
        {
            Found?.Invoke(this, e);
        }

        public event EventHandler<SystemVisitorEventArgs> Started;

        private IEnumerable<FileSystemItem> GetFileSystemEnumerable()
        {
            if (Directory.Exists(_path))
            {
                SystemVisitorEventArgs args = new SystemVisitorEventArgs();
                OnStart(null);
                foreach (var item in DyrectoryVisit(_path))
                {
                    if (!args.IsStop)
                    {
                        args.FoundItem = item;
                        OnFound(args);
                        yield return item;
                    }
                    else
                    {
                        break;
                    }
                }
                OnFinish(null);
            }
        }

        private IEnumerable<FileSystemItem> DyrectoryVisit(string rootPath)
        {
            if (Directory.Exists(rootPath))
            {

                DirectoryInfo dir = new DirectoryInfo(rootPath);
                yield return new FileSystemItem()
                {
                    Name = dir.Name,
                    Type = FileSystemItemType.Directory,
                    Date = dir.LastWriteTime
                };
                string[] files;
                try
                {
                    files = Directory.GetFiles(rootPath);
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
                    dirs = Directory.GetDirectories(rootPath);
                }
                catch
                {
                    dirs = null;
                }

                if (dirs != null && dirs.Length > 0)
                  foreach (var directory in dirs)
                  {
                      foreach (var item in DyrectoryVisit(directory))
                      {
                          yield return item;
                      }
                  }
            }
        }

        public IEnumerator GetEnumerator()
        {
            return GetFileSystemEnumerable().GetEnumerator();
        }
    }
}
