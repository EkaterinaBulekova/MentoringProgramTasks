using System;
using FileSystemVisitorLibrary;

namespace ConsoleFileSystemVisitor
{
    class Program
    {
        private static int count = 0;
        static void Main(string[] args)
        {
            var _skip = 5;
            var _stop = 20;
            var visitor = new FileSystemVisitor("d:/MyProject", (x) => x.Date >= new DateTime(2018, 01, 18));
            visitor.Started += (o, eventArgs) => Console.WriteLine("Start !!!");
            visitor.Finished += (o, eventArgs) => Console.WriteLine("Finish !!!");
            visitor.Found += (o, eventArgs) =>
            {
                if (eventArgs.IsSkip)
                {

                }
                Console.WriteLine($"Found - {eventArgs.FoundItem.Name} !!!");
            };



            foreach (var item in visitor)
            {
                Console.WriteLine($"{item.Name} - {item.Type} - {item.Date}");
            }

            Console.ReadKey();
        }

 
        static void find_reaction(object sender, SystemVisitorEventArgs e)
        {
            if (count <= 10)
            {
                Console.WriteLine("Found {0}!!!", e.FoundItem.Name);
                count++;
            }
            else
            {
                e.IsStop = true;
            }
        }
    }
}
