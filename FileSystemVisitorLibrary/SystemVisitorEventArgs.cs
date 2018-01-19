using System;
using FileSystemVisitorLibrary.Data;

namespace FileSystemVisitorLibrary
{
    public class SystemVisitorEventArgs : EventArgs
    {
        public bool IsStop { get; set; }
        public bool IsSkip { get; set; }
        public FileSystemItem FoundItem { get; set; }
    }

}
