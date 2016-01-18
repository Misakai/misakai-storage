namespace System
{
    /// <summary>
    /// Represents an object that implements IDisposable contract.
    /// </summary>
    public abstract class DisposableObject : IDisposable
    {
        #region IDisposable Members
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or
        /// resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // the object is actually going to die.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the ByteSTream class and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"> 
        /// If set to true, release both managed and unmanaged resources, othewise release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {

        }

        /// <summary>
        /// Finalizer for the recyclable object.
        /// </summary>
        ~DisposableObject()
        {
            // The object is actually going to die.
            Dispose(false);
        }
        #endregion
    }

}
