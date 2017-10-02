using System;

namespace com.drew.metadata.exif
{
	/// <summary>
	/// The exception type raised during reading of Exif data in the instance of unexpected data conditions.
	/// </summary>

    [Serializable]
	public class ExifProcessingException : MetadataException
	{
		/// <summary>
		/// Constructor of the object
		/// </summary>
		/// <param name="aMessage">The error aMessage</param>
		public ExifProcessingException(string message) : base(message)
		{
		}

		/// <summary>
		/// Constructor of the object
		/// </summary>
		/// <param name="aMessage">The error aMessage</param>
		/// <param name="aCause">The aCause of the exception</param>
		public ExifProcessingException(string message, Exception cause) : base(message, cause)
		{
		}

		/// <summary>
		/// Constructor of the object
		/// </summary>
		/// <param name="aCause">The aCause of the exception</param>
		public ExifProcessingException(Exception cause) : base(cause)
		{
		}
	}
}