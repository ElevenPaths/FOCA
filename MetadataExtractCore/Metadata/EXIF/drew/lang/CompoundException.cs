using System;

namespace com.drew.lang
{
	/// <summary>
	/// This is Compound exception
	/// </summary>
    [Serializable]
	public class CompoundException : Exception
	{
		/// <summary>
		/// Constructor of the object
		/// </summary>
		/// <param name="aMessage">The error aMessage</param>
		public CompoundException(string aMessage) : base(aMessage)
		{
		}

		/// <summary>
		/// Constructor of the object
		/// </summary>
		/// <param name="aMessage">The error aMessage</param>
		/// <param name="aCause">The aCause of the exception</param>
		public CompoundException(string aMessage, Exception aCause) : base(aMessage, aCause)
		{
		}

		/// <summary>
		/// Constructor of the object
		/// </summary>
		/// <param name="aCause">The aCause of the exception</param>
		public CompoundException(Exception aCause) : base(null, aCause)
		{
		}
	}
}
