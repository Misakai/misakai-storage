using System;

namespace Misakai.Storage
{
    /// <summary>
    /// Represents a pluggable logger.
    /// </summary>
    public static class Log
    {
        public static event LogExceptionDelegate Error;

        /// <summary>
        /// Writes the error to the log.
        /// </summary>
        /// <param name="ex">The exception to write</param>
        public static void Write(Exception ex)
        {
            if (Error != null)
                Error(ex);
        }
    }



    public delegate void LogExceptionDelegate(Exception ex);
}
