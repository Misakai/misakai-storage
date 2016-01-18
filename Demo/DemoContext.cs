using Misakai.Storage;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    /// <summary>
    /// Represents the entity framework context.
    /// </summary>
    public class DemoContext : EntityContext
    {
        /// <summary>
        /// Constructs a new context.
        /// </summary>
        public DemoContext() : base("HOSTNAME", 3000, "DATABASE", "USERNAME", "PASSWORD") { }

        /// <summary>
        /// The storage model for the actors.
        /// </summary>
        public DbSet<Actor> Actors { get; set; }


    }
}
