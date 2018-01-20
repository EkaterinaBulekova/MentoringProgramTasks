using System;
using FileSystemVisitorLibrary;
using FileSystemVisitorLibrary.Data;

namespace ConsoleFileSystemVisitor
{
    internal class Program
    {
        private static void Main()
        {
            var stringForSkip = "vs";
            var stringForStop = "sssln";
            var visitor = new FileSystemVisitor2("d:/MyProject", (x) => x.Date <= new DateTime(2018, 01, 18) && x.Type == FileSystemItemType.File);
            visitor.Start += (o, eventArgs) => Console.WriteLine("Start !!!");
            visitor.Finish += (o, eventArgs) => Console.WriteLine("Finish !!!");
            visitor.FileFound += (o, eventArgs) => Console.WriteLine($"File {eventArgs.FoundItem.Name} found!!!");
            visitor.DirectoryFound += (o, eventArgs) => Console.WriteLine($"Directory {eventArgs.FoundItem.Name} found!!!");
            visitor.FilteredFileFound += (o, eventArgs) =>
            {
                Console.WriteLine($"Filtered File {eventArgs.FoundItem.Name} found!!!");
                eventArgs.IsSkip = false;
                eventArgs.IsStop = false;
                if (eventArgs.FoundItem.Name.Contains(stringForSkip))
                    eventArgs.IsSkip = true;
                if (eventArgs.FoundItem.Name.Contains(stringForStop))
                    eventArgs.IsSkip = true;
            };
            visitor.FilteredDirectoryFound += (o, eventArgs) =>
            {
                Console.WriteLine($"Filtered Directory {eventArgs.FoundItem.Name} found!!!");
                eventArgs.IsSkip = false;
                eventArgs.IsStop = false;
                if (eventArgs.FoundItem.Name.Contains(stringForSkip))
                    eventArgs.IsSkip = true;
                if (eventArgs.FoundItem.Name.Contains(stringForStop))
                    eventArgs.IsSkip = true;
            };

            foreach (var item in visitor)
            {
                Console.WriteLine($"{item.Name} - {item.Type} - {item.Date}");
            }

            Console.ReadKey();
        }
    }
}
