using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misakai.Storage
{
    /// <summary>
    /// Represents the entity framework context.
    /// </summary>
    public abstract class EntityContext : DbContext
    {
        #region Constructor


        /// <summary>
        /// Constructs a new context.
        /// </summary>
        /// <param name="database">The database name to connect to.</param>
        /// <param name="password">The password.</param>
        /// <param name="port">The server port to connect to.</param>
        /// <param name="server">The server endpoint.</param>
        /// <param name="user">The user name</param>
        public EntityContext(string server, int port, string database, string user, string password) :base(
           String.Format("Server={0};Port={1};Database={2};User Id={3};Password={4};", server, port, database, user, password)
            )
        {
            //Database.SetInitializer<MyDbContext>(null);
        }
        #endregion

        #region Static Members
        /// <summary>
        /// Gets the scope factory.
        /// </summary>
        public static readonly DbContextScopeFactory Default = new DbContextScopeFactory();

        /// <summary>
        /// Acquire a new scope.
        /// </summary>
        /// <returns></returns>
        public static IDbContextScope Acquire()
        {
            return Default.Create(DbContextScopeOption.JoinExisting);
        }

        /// <summary>
        /// Acquire a new scope.
        /// </summary>
        /// <returns></returns>
        public static IDbContextReadOnlyScope AcquireReadOnly()
        {
            return Default.CreateReadOnly();
        }
        #endregion

        /// <summary>
        /// Saves the changes into the context.
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            // Get the tracked entitites
            var entities = ChangeTracker
                .Entries()
                .Where(x => x.Entity is Entity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            // Set created/updated properties
            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    try
                    {
                        var e = ((Entity)entity.Entity);
                        e.CreatedDate = e.UpdatedDate = DateTime.UtcNow;
                    }
                    catch(Exception ex)
                    {
                        // Log the exception
                        Log.Write(ex);
                    }
                }

                if (entity.State == EntityState.Modified)
                {
                    try
                    {
                        var e = ((Entity)entity.Entity);
                        e.UpdatedDate = DateTime.UtcNow;
                    }
                    catch (Exception ex)
                    {
                        // Log the exception
                        Log.Write(ex);
                    }
                }
            }

            // Call the base 
            return base.SaveChanges();
        }
    }
}
