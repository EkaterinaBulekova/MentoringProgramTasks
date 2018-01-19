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
        public int SkippedCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsStop { get; set; }
        public bool IsSkip { get; set; }
        public FileSystemItem FoundItem { get; set; }
    }

    public class FileSystemVisitor : IEnumerable<FileSystemItem>
    {
        private string _path;
        private int _level;
        private int _filteredCount;
        private int _skippedCount;
        private bool _isStop;
        private bool _isSkip;
        private Func<FileSystemItem, bool> _filter;

        public FileSystemVisitor(string path, Func<FileSystemItem, bool> filter = null)
        {
            _filteredCount = 0;
            _skippedCount = 0;
            _level = 0;
            _path = path;
            _filter = filter != null ? filter : (_) => true;
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
            if (_level == 0)
            {
                OnStart(null);
            }

            _level++;
            if (Directory.Exists(_path))
            {
                List<FileSystemItem> entities = new List<FileSystemItem>();
                try
                {
                    entities = Directory.GetFileSystemEntries(_path).Select(_ => Directory.Exists(_) ? new FileSystemItem(new DirectoryInfo(_)) : new FileSystemItem(new FileInfo(_))).ToList();
                }
                catch
                {
                }

                var filtered = entities.Where(_filter).ToList();
                foreach (var entity in entities)
                {
                   
                    _filteredCount++;
                    var isFile = entity.Type == FileSystemItemType.File;
                    var args = new SystemVisitorEventArgs { FilteredCount = _filteredCount, SkippedCount = _skippedCount, IsStop = false, IsSkip = false, FoundItem = entity };
                    if (isFile)
                    {
                        OnFileFound(args);
                            if (_isStop) yield break;
                    }
                    else
                    {
                        OnDirectoryFound(args);
                            if (_isStop) yield break;
                    }
                    if (filtered.Contains(entity)&& !_isSkip)
                    {
                        if (isFile)
                        {
                            OnFilteredFileFound(args);
                            if (_isStop) yield break;
                            yield return entity;
                        }
                        else
                        {
                            OnFilteredDirectoryFound(args);
                            if (_isStop) yield break;
                            yield return entity;
                            _path = entity.Name;
                            foreach (var item in GetFileSystemEnumerable())
                            {
                                yield return item;
                            }
                        }
                    }
                    else
                    {
                        if (_isSkip) _skippedCount++;
                        _filteredCount--;
                    }
                }
            }

            _level--;
            if (_level == 0)
            {
                OnFinish(null);
            }
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
