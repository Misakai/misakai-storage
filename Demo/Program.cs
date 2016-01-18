using Misakai.Storage;
using System;

namespace Demo
{
    class Program
    {
        static readonly DemoContext Context = new DemoContext();
        static readonly ActorProvider Actors = new ActorProvider();

        static void Main(string[] args)
        {
            // Transaction
            using (var uow = EntityContext.Acquire())
            {
                // Using the provider for queries
                var result = Actors.GetByKey(7);
                if (result.Success)
                {
                    // Change something
                    var actor = result.Value;
                    actor.Bio = "Hello";

                    // Commit
                    uow.SaveChanges();
                }
            }

            Console.ReadKey();
        }
    }
}
