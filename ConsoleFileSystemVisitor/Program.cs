using System;
using FileSystemVisitorLibrary;
using FileSystemVisitorLibrary.Infrastructure;

namespace ConsoleFileSystemVisitor
{
    internal class Program
    {
        private static void Main()
        {
            var stringForSkip = "level2";
            var stringForStop = "file2";
            var fileSystemProvider = new FileSystemInfoProvider();
            var visitor = new FileSystemVisitor(fileSystemProvider, "d:/Test", entity => entity.Name.Contains("l2"));
            visitor.Start += (o, eventArgs) => Console.WriteLine("Start !!!");
            visitor.Finish += (o, eventArgs) => Console.WriteLine("Finish !!!");
            visitor.FileFound += (o, eventArgs) => Console.WriteLine($"File {eventArgs.FoundItem.Name} found!!!");
            visitor.DirectoryFound += (o, eventArgs) => Console.WriteLine($"Directory {eventArgs.FoundItem.Name} found!!!");
            visitor.FilteredFileFound += (o, eventArgs) =>
            {
                Console.WriteLine($"Filtered File {eventArgs.FoundItem.Name} found!!!");
                eventArgs.IsSkip = eventArgs.FoundItem.Name.Contains(stringForSkip);
                eventArgs.IsStop = eventArgs.FoundItem.Name.Contains(stringForStop);
            };
            visitor.FilteredDirectoryFound += (o, eventArgs) =>
            {
                Console.WriteLine($"Filtered Directory {eventArgs.FoundItem.Name} found!!!");
                eventArgs.IsSkip = eventArgs.FoundItem.Name.Contains(stringForSkip);
                eventArgs.IsStop = eventArgs.FoundItem.Name.Contains(stringForStop);
            };

            foreach (var item in visitor)
            {
                Console.WriteLine($"{item.Name} - {item.Type} - {item.Date}");
            }

            Console.ReadKey();
        }
    }
}
