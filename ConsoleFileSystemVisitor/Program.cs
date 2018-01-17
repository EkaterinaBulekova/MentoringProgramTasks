using System;
using FileSystemVisitorLibrary;


namespace ConsoleFileSistemWalker
{
    class Program
    {
        static int count = 0;
        static void Main(string[] args)
        {
            var visitor = new FileSystemVisitor();
            visitor.Started += start_reaction;
            visitor.Finished += finish_reaction;
            visitor.Found += find_reaction;

            
            foreach (var item in visitor.FileSystemVisit("d:/MyProject"))
            {
                Console.WriteLine($"{item.Name} - {item.Type} - {item.Date}");
            }

            Console.ReadLine();
        }

        static void start_reaction(object sender, SystemVisitorEventArgs e)
        {
            Console.WriteLine("Start !!!");
        }

        static void finish_reaction(object sender, SystemVisitorEventArgs e)
        {
            Console.WriteLine("Finish !!!");
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
