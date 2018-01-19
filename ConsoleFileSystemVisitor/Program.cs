using System;
using FileSystemVisitorLibrary;

namespace ConsoleFileSystemVisitor
{
    class Program
    {
        static void Main(string[] args)
        {
            var _skipLimit = 2;
//            var _skipStart = 10;
            var _stopLimit = 5;
            var visitor = new FileSystemVisitor("d:/MyProject");//, (x) => x.Date >= new DateTime(2018, 01, 18));
            visitor.Start += (o, eventArgs) => Console.WriteLine("Start !!!");
            visitor.Finish += (o, eventArgs) => Console.WriteLine("Finish !!!");
            visitor.FileFound += (o, eventArgs) => Console.WriteLine($"File {eventArgs.FoundItem.Name} found!!!", eventArgs.IsSkip=(eventArgs.SkippedCount < _skipLimit)?true:false,  eventArgs.IsStop=(eventArgs.FilteredCount >= _stopLimit)?true:false);
            visitor.DirectoryFound += (o, eventArgs) => Console.WriteLine($"Directory {eventArgs.FoundItem.Name} found!!!", eventArgs.IsSkip = (eventArgs.SkippedCount < _skipLimit) ? true : false, eventArgs.IsStop = (eventArgs.FilteredCount >= _stopLimit) ? true : false);
            visitor.FilteredFileFound += (o, eventArgs) => Console.WriteLine($"Filtered File {eventArgs.FoundItem.Name} found!!!", eventArgs.IsSkip = (eventArgs.SkippedCount < _skipLimit) ? true : false, eventArgs.IsStop = (eventArgs.FilteredCount >= _stopLimit) ? true : false);
            visitor.FilteredDirectoryFound += (o, eventArgs) => Console.WriteLine($"Filtered Directory {eventArgs.FoundItem.Name} found!!!", eventArgs.IsSkip = (eventArgs.SkippedCount < _skipLimit) ? true : false, eventArgs.IsStop = (eventArgs.FilteredCount >= _stopLimit) ? true : false);

            foreach (var item in visitor)
            {
                Console.WriteLine($"{item.Name} - {item.Type} - {item.Date}");
            }

            Console.ReadKey();
        }
    }
}
