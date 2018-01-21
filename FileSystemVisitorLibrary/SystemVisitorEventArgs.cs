using System;
using FileSystemVisitorLibrary.Data;

namespace FileSystemVisitorLibrary
{
    /// <summary>
    /// Provides a values to use for events that do not include event data.
    /// </summary>
    public class SystemVisitorEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether flag to stop
        /// </summary>
        public bool IsStop { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether flag to skip
        /// </summary>
        public bool IsSkip { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether FileSysteItem instance that fire event
        /// </summary>
        public FileSystemItem FoundItem { get; set; }
    }
}
