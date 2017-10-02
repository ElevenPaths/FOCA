using System;
using com.utils.bundle;

namespace com.drew.metadata
{
	/// <summary>
	/// This abstract class represent the mother class of all tag descriptor.
	/// </summary>
	[Serializable]

	public abstract class AbstractTagDescriptor 
	{
        /// <summary>
        /// Contains all commons words.
        /// </summary>
        protected static readonly IResourceBundle BUNDLE = ResourceBundleFactory.CreateDefaultBundle("Commons");

		protected AbstractDirectory directory;

		/// <summary>
		/// Constructor of the object
		/// </summary>
		/// <param name="aDirectory">a directory</param>
		public AbstractTagDescriptor(AbstractDirectory aDirectory) : base()
		{
            this.directory = aDirectory;
		}

		/// <summary>
		/// Returns a descriptive value of the the specified tag for this image. 
		/// Where possible, known values will be substituted here in place of the raw tokens actually 
		/// kept in the Exif segment. 
		/// If no substitution is available, the value provided by GetString(int) will be returned.
		/// This and GetString(int) are the only 'get' methods that won't throw an exception.
		/// </summary>
		/// <param name="aTagType">the tag to find a description for</param>
		/// <returns>a description of the image'str value for the specified tag, or null if the tag hasn't been defined.</returns>
		public abstract string GetDescription(int aTagType);
	}
}
