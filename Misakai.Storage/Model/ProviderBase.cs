using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime;
using System.Collections.Specialized;

namespace Misakai.Storage
{
    /// <summary>
    /// This class provides a base implementation for the extensible provider model.
    /// </summary>
    public abstract class Provider : DisposableObject
    {
        // Fields
        private string fName;
        private string fDescription;
        private bool fInitialized;

        /// <summary>
        /// Initializes a new instance of the System.Configuration.Provider.ProviderBase
        /// class.
        /// </summary>
        protected Provider()
        {

        }

        /// <summary>
        /// Initializes the provider.
        /// </summary>
        /// <param name="name">The friendly name of the provider.</param>
        /// <param name="config">A collection of the name/value pairs representing the provider-specific attributes
        /// specified in the configuration for this provider.
        /// </param>
        public virtual void Initialize(string name, NameValueCollection config)
        {
            lock (this)
            {
                if (this.fInitialized)
                    throw new InvalidOperationException("Provider have already been initialized.");
                this.fInitialized = true;
            }

            if (name == null)
                throw new ArgumentNullException("name");

            if (name.Length == 0)
                throw new ArgumentException("The supplied provider name is null or empty.", "name");
            
            this.fName = name;
            if (config != null)
            {
                this.fDescription = config["description"];
                config.Remove("description");
            }
        }

        /// <summary>
        /// Gets a brief, friendly description suitable for display in administrative
        /// tools or other user interfaces.
        /// </summary>
        public virtual string Description
        {
            get
            {
                if (!string.IsNullOrEmpty(this.fDescription))
                    return this.fDescription;
                
                return this.Name;
            }
        }

        /// <summary>
        /// Gets the friendly name used to refer to the provider during configuration.
        /// </summary>
        public virtual string Name
        {
            get { return this.fName; }
        }
    }

 

}
