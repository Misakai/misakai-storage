using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    class Program
    {
        static readonly DemoContext Context = new DemoContext();

        static void Main(string[] args)
        {

            foreach(var actor in Context.Actors)
            {
                Console.WriteLine("Actor: " + actor.Name);
            }

            Console.ReadKey();
        }
    }
}
