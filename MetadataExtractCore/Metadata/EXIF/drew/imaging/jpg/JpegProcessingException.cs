using System;
using com.drew.lang;

namespace com.drew.imaging.jpg
{
	/// <summary>
	/// Represents a JpegProcessing exception
	/// </summary>
    [Serializable]
	public class JpegProcessingException : CompoundException
	{
		/// <summary>
		/// Constructor of the object
		/// </summary>
		/// <param name="aMessage">The error aMessage</param>
		public JpegProcessingException(string aMessage) : base(aMessage)
		{
		}

		/// <summary>
		/// Constructor of the object
		/// </summary>
		/// <param name="aMessage">The error aMessage</param>
		/// <param name="aCause">The aCause of the exception</param>
		public JpegProcessingException(string aMessage, Exception aCause) : base(aMessage, aCause)
		{
		}

		/// <summary>
		/// Constructor of the object
		/// </summary>
		/// <param name="aCause">The aCause of the exception</param>
		public JpegProcessingException(Exception aCause) : base(aCause)
		{
		}
	}
}
