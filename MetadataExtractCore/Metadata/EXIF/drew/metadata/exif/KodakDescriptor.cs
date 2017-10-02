namespace com.drew.metadata.exif
{
	/// <summary>
	/// Tag descriptor for Kodak
    /// 
    /// Thanks to David Carson for the initial version of this class.
	/// </summary>

	public class KodakDescriptor : AbstractTagDescriptor 
	{
		/// <summary>
		/// Constructor of the object
		/// </summary>
		/// <param name="directory">a directory</param>
        public KodakDescriptor(AbstractDirectory directory)
            : base(directory)
		{
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
        public override string GetDescription(int aTagType)
        {
            return base.directory.GetString(aTagType);
        }
	}
}
