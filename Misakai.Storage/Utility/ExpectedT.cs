namespace System
{
    /// <summary>
    /// Represents an expected return value, or an error. This is used for a high-performance
    /// exception propagation.
    /// </summary>
    /// <typeparam name="T">The type of the value we expect.</typeparam>
    public struct Expected<T>
    {
        #region Constructors
        /// <summary>
        /// Constructs a new expected value.
        /// </summary>
        /// <param name="error">The error to construct for.</param>
        public Expected(Exception error)
        {
            this.Value = default(T);
            this.Error = error;
        }

        /// <summary>
        /// Constructs a new expected value.
        /// </summary>
        /// <param name="value">The value to construct for.</param>
        public Expected(T value)
        {
            this.Value = value;
            this.Error = null;
        }

        /// <summary>
        /// Whether this has no error.
        /// </summary>
        public bool Success
        {
            get { return this.Error == null; }
        }

        /// <summary>
        /// Whether this has an error or not.
        /// </summary>
        public bool Failure
        {
            get { return this.Error != null; }
        }

        /// <summary>
        /// Whether this has a value or not.
        /// </summary>
        public bool HasValue
        {
            get  { return !this.Value.Equals(default(T)); }
        }

        /// <summary>
        /// Whether this has no error and no value
        /// </summary>
        public bool IsEmpty
        {
            get { return this.Error == null && this.Value.Equals(default(T)); }
        }

        /// <summary>
        /// Gets the value of this expected.
        /// </summary>
        public readonly T Value;

        /// <summary>
        /// Gets the error of this expected.
        /// </summary>
        public readonly Exception Error;
        #endregion

        /// <summary>
        /// Gets whether this expected's value is equal or not to another value.
        /// </summary>
        /// <param name="other">The other value.</param>
        /// <returns>Whether they are equal or not.</returns>
        public override bool Equals(object other)
        {
            if (!Success) 
                return other == null;

            if (other == null) 
                return false;
            return this.Value.Equals(other);
        }

        /// <summary>
        /// Gets the hash code for this expected value. Only based on the value
        /// and not on the error.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Success 
                ? this.Value.GetHashCode() 
                : 0;
        }

        /// <summary>
        /// Converts the expected to a string representation.
        /// </summary>
        /// <returns>The string representation of the expected.</returns>
        public override string ToString()
        {
            return Success 
                ? this.Value.ToString() 
                : this.Error.ToString();
        }

        /// <summary>
        /// Implicitly converts a value to the expected
        /// </summary>
        /// <param name="value">The value to convert to.</param>
        /// <returns>Converted expected.</returns>
        public static implicit operator Expected<T>(T value)
        {
            return new Expected<T>(value);
        }

        /// <summary>
        /// Implicitly converts an exception to the expected
        /// </summary>
        /// <param name="error">The error to convert to.</param>
        /// <returns>Converted expected.</returns>
        public static implicit operator Expected<T>(Exception error)
        {
            return new Expected<T>(error);
        }

        /// <summary>
        /// Implicitly converts the expected to a value.
        /// </summary>
        /// <param name="value">The expected to convert.</param>
        /// <returns>Converted value.</returns>
        public static explicit operator T(Expected<T> value)
        {
            return value.Value;
        }

        /// <summary>
        /// Implicitly converts the expected to an error.
        /// </summary>
        /// <param name="value">The expected to convert.</param>
        /// <returns>Converted error.</returns>
        public static explicit operator Exception(Expected<T> value)
        {
            return value.Error;
        }

    }

}
