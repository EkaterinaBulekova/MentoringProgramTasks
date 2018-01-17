using System;
using FileSystemVisitorLibrary;


namespace ConsoleFileSistemWalker
{
    class Program
    {
        static void Main(string[] args)
        {
            var visitor = new FileSystemVisitor();
            visitor.Started += start_reaction;
            visitor.Finished += finish_reaction;
            visitor.Found += find_reaction;


            foreach (var item in visitor.FileSystemVisit("d:"))
            {
                Console.WriteLine($"{item.Name} - {item.Type} - {item.Date}");
            }

            Console.ReadLine();
        }

        static void start_reaction(object sender, SystemWalkerEventArgs e)
        {
            Console.WriteLine("Start !!!");
        }

        static void finish_reaction(object sender, SystemWalkerEventArgs e)
        {
            Console.WriteLine("Finish !!!");
        }

        static void find_reaction(object sender, SystemWalkerEventArgs e)
        {
            Console.WriteLine("Found {0}!!!", e.Entity.Name);
        }
    }
}
