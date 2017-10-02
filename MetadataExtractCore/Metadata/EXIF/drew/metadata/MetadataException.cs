using System;
using com.drew.lang;

namespace com.drew.metadata
{
	/// <summary>
	/// This class represents a Metadata exception
	/// </summary>

    [Serializable]
	public class MetadataException : CompoundException
	{
		/// <summary>
		/// Constructor of the object
		/// </summary>
		/// <param name="aMessage">The error aMessage</param>
		public MetadataException(string aMessage) : base(aMessage)
		{
		}

		/// <summary>
		/// Constructor of the object
		/// </summary>
		/// <param name="aMessage">The error aMessage</param>
		/// <param name="aCause">The aCause of the exception</param>
		public MetadataException(string aMessage, Exception aCause) : base(aMessage, aCause)
		{
		}

		/// <summary>
		/// Constructor of the object
		/// </summary>
		/// <param name="aCause">The aCause of the exception</param>
        public MetadataException(Exception aCause)
            : base(aCause.Message, aCause)
		{
		}
	}
}
