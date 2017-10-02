using System;
using com.drew.lang;

namespace com.utils.bundle
{
	/// <summary>
	/// This class represents a missing resource exception.
    ///
    /// Used by ResourveBundle class.
	/// </summary>

    [Serializable]
    public class MissingResourceException : CompoundException
	{
		/// <summary>
		/// Constructor of the object
		/// </summary>
		/// <param name="aMessage">The error aMessage</param>
		public MissingResourceException(string message) : base(message)
		{
		}

		/// <summary>
		/// Constructor of the object
		/// </summary>
		/// <param name="aMessage">The error aMessage</param>
		/// <param name="aCause">The aCause of the exception</param>
		public MissingResourceException(string message, Exception cause) : base(message, cause)
		{
		}

		/// <summary>
		/// Constructor of the object
		/// </summary>
		/// <param name="aCause">The aCause of the exception</param>
        public MissingResourceException(Exception cause)
            : base(cause)
		{
		}
	}
}
