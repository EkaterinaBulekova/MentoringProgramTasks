using FileSystemVisitorLibrary.Data;
using System;
using System.Collections.Generic;
using System.IO;

namespace FileSystemVisitorLibrary
{
    public class SystemWalkerEventArgs : EventArgs
    {
        public FileSystemItem Entity { get; set; }
    }

    public class FileSystemVisitor
    {

        protected virtual void OnStart(SystemWalkerEventArgs e)
        {
            Started?.Invoke(this, e);
        }

        public event EventHandler<SystemWalkerEventArgs> Finished;

        protected virtual void OnFinish(SystemWalkerEventArgs e)
        {
            Finished?.Invoke(this, e);
        }

        public event EventHandler<SystemWalkerEventArgs> Found;

        protected virtual void OnFound(SystemWalkerEventArgs e)
        {
            Found?.Invoke(this, e);
        }

        public event EventHandler<SystemWalkerEventArgs> Started;

        public IEnumerable<FileSystemItem> FileSystemVisit(string rootPath)
        {
            if (Directory.Exists(rootPath))
            {
                SystemWalkerEventArgs args = new SystemWalkerEventArgs();
 
                OnStart(null);
                foreach (var item in DyrectoryVisit(rootPath))
                {
                    args.Entity = item;
                    OnFound(args);
                    yield return item;
                }
                OnFinish(null);
            }
        }

        public static IEnumerable<FileSystemItem> DyrectoryVisit(string rootPath)
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
    }

}
