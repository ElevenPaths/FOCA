using System;

namespace com.drew.metadata.iptc
{
	/// <summary>
	/// The exception type raised during reading of Iptc data in the instance of unexpected data conditions.
	/// </summary>

    [Serializable]
	public class IptcProcessingException : MetadataException
	{
		/// <summary>
		/// Constructor of the object
		/// </summary>
		/// <param name="aMessage">The error aMessage</param>
        public IptcProcessingException(string aMessage)
            : base(aMessage)
		{
		}

		/// <summary>
		/// Constructor of the object
		/// </summary>
		/// <param name="aMessage">The error aMessage</param>
		/// <param name="aCause">The aCause of the exception</param>
        public IptcProcessingException(string aMessage, Exception aCause)
            : base(aMessage, aCause)
		{
		}

		/// <summary>
		/// Constructor of the object
		/// </summary>
		/// <param name="aCause">The aCause of the exception</param>
        public IptcProcessingException(Exception aCause)
            : base(aCause.Message, aCause)
		{
		}
	}
}