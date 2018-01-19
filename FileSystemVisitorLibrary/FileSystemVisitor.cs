using FileSystemVisitorLibrary.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileSystemVisitorLibrary
{
    public class FileSystemVisitor : IEnumerable<FileSystemItem>
    {
        private readonly string _path;
        private int _level;
        private bool _isStop;
        private bool _isSkip;
        private readonly Func<FileSystemItem, bool> _filter;

        /// <summary>
        /// Initializes a new instance of the FileSystemVisitor class on the specified
        //     path.
        /// </summary>
        /// <param name="path"> A string specifying the path on which to create the DirectoryInfo. </param>
        /// <param name="filter"> A delegete which reference to filtering method </param>
        public FileSystemVisitor(string path, Func<FileSystemItem, bool> filter = null)
        {
            _level = 0;
            _path = path;
            _filter = filter ?? (_ => true);
        }
        /// <summary>
        /// Represents the method that will handle an event Finish when the event provides data.
        /// </summary>
        public event EventHandler<SystemVisitorEventArgs> Finish;
       
        /// <summary>
        /// Represents the method that will handle an event Start when the event provides data.
        /// </summary>
        public event EventHandler<SystemVisitorEventArgs> Start;
        
        /// <summary>
        /// Represents the method that will handle an event FileFound when the event provides data.
        /// </summary>
        public event EventHandler<SystemVisitorEventArgs> FileFound;
        /// <summary>
        /// Represents the method that will handle an event DirectoryFound when the event provides data.
        /// </summary>
        public event EventHandler<SystemVisitorEventArgs> DirectoryFound;
        /// <summary>
        /// Represents the method that will handle an event FilteredFileFound when the event provides data.
        /// </summary>
        public event EventHandler<SystemVisitorEventArgs> FilteredFileFound;
        /// <summary>
        /// Represents the method that will handle an event FilteredDirectoryFound when the event provides data.
        /// </summary>
        public event EventHandler<SystemVisitorEventArgs> FilteredDirectoryFound;

        protected virtual void OnStart(SystemVisitorEventArgs e)
        {
            Start?.Invoke(this, e);
        }

        protected virtual void OnFinish(SystemVisitorEventArgs e) => Finish?.Invoke(this, e);

        protected virtual void OnFileFound(SystemVisitorEventArgs e) => FileFound?.Invoke(this, e);

        protected virtual void OnDirectoryFound(SystemVisitorEventArgs e) => DirectoryFound?.Invoke(this, e);

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

        private IEnumerable<FileSystemItem> GetFileSystemEnumerable(string path)
        {
            if (_level == 0)
            {
                OnStart(null);
            }

            _level++;
            if (Directory.Exists(path))
            {
                List<FileSystemItem> entities = new List<FileSystemItem>();
                try
                {
                    entities = Directory.GetFileSystemEntries(path).Select(_ => new FileSystemItem(_)).ToList();
                }
                catch
                {
                    // ignored
                }

                var filtered = entities.Where(_filter).ToList();
                foreach (var entity in entities)
                {
                   
                    var isFile = entity.Type == FileSystemItemType.File;
                    var args = new SystemVisitorEventArgs
                    {
                        IsStop = false,
                        IsSkip = false,
                        FoundItem = entity
                    };
                    if (isFile)
                    {
                        OnFileFound(args);
                    }
                    else
                    {
                        OnDirectoryFound(args);
                    }

                    if (filtered.Contains(entity))
                    {
                        if (isFile)
                        {
                            OnFilteredFileFound(args);
                            if (_isStop)
                            {
                                yield break;
                            }

                            if (!_isSkip)
                            {
                                yield return entity;
                            }
                        }
                        else
                        {
                            OnFilteredDirectoryFound(args);
                            if (_isStop)
                            {
                                yield break;
                            }

                            if (!_isSkip)
                            {
                                yield return entity;
                                foreach (var item in GetFileSystemEnumerable(entity.Name))
                                {
                                    yield return item;
                                }
                            }
                        }
                    }
                }
            }

            _level--;
            if (_level == 0)
            {
                OnFinish(null);
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates throw the collection of FileSystemItem  
        /// </summary>
        /// <returns></returns>
        public IEnumerator<FileSystemItem> GetEnumerator()
        {
            return GetFileSystemEnumerable(_path).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}
