using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FileSystemVisitorLibrary.Data;
using FileSystemVisitorLibrary.Infrastructure;

namespace FileSystemVisitorLibrary
{
    /// <summary>
    /// Supports iteration over a collection of the FileSystemItem type. 
    /// Also fires the events and allow skip or stop iteration. 
    /// </summary>
    public class FileSystemVisitor : IEnumerable<FileSystemItem>
    {
        private readonly IFileSystemInfoProvider _fileSystemProvaider;
        private readonly Func<FileSystemItem, bool> _filter;
        private readonly string _path;
        private bool _isStop;
        private bool _isSkip;

        /// <summary>
        /// Initializes a new instance of the FileSystemVisitor class on the specified path.
        /// </summary>
        /// <param name="provider"> A IFileSystemInfoProvaider that alows get collection of FileSystemItems. </param>
        /// <param name="path"> A string specifying the path on which to create the DirectoryInfo. </param>
        /// <param name="filter"> A delegete which reference to filtering method </param>
        public FileSystemVisitor(IFileSystemInfoProvider provider, string path, Func<FileSystemItem, bool> filter = null)
        {
            _fileSystemProvaider = provider;
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

        /// <summary>
        /// Returns an enumerator that iterates throw the collection of FileSystemItem  
        /// </summary>
        /// <returns>IEnumerator of FileSystemItem type</returns>
        public IEnumerator<FileSystemItem> GetEnumerator()
        {
            return GetFileSystemEnumerable(_fileSystemProvaider.GetFileSystemItems(_path)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected virtual void OnStart(SystemVisitorEventArgs e) => Start?.Invoke(this, e);

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

        private IEnumerable<FileSystemItem> GetFileSystemEnumerable(IEnumerable<FileSystemItem> entities)
        {
            OnStart(null);
            var fileSystemItems = entities.ToList();
            var filteredEntities = fileSystemItems.Where(_filter).ToList();
            foreach (var entity in fileSystemItems)
            {
                var isFile = entity.Type == FileSystemItemType.File;
                var args = new SystemVisitorEventArgs
                {
                    IsStop = false,
                    IsSkip = false,
                    FoundItem = entity
                };
                if (isFile)
                    OnFileFound(args);
                else
                    OnDirectoryFound(args);
                if (!filteredEntities.Contains(entity)) continue;
                if (isFile)
                    OnFilteredFileFound(args);
                else
                    OnFilteredDirectoryFound(args);
                if (_isStop) yield break;
                if (!_isSkip) yield return entity;
            }

            OnFinish(null);
        }
    }
}
