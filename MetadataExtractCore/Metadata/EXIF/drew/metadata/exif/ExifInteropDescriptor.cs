namespace com.drew.metadata.exif
{
	/// <summary>
	/// Tag descriptor for almost every images
	/// </summary>

	public class ExifInteropDescriptor : AbstractTagDescriptor 
	{
		/// <summary>
		/// Constructor of the object
		/// </summary>
		/// <param name="base.directory">a base.directory</param>
		public ExifInteropDescriptor(AbstractDirectory aDirectory) : base(aDirectory)
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
		public override string GetDescription(int tagType)  
		{
			switch(tagType) 
			{
				case ExifInteropDirectory.TAG_INTEROP_INDEX:
					return GetInteropIndexDescription();
				case ExifInteropDirectory.TAG_INTEROP_VERSION:
					return GetInteropVersionDescription();
				default:
					return base.directory.GetString(tagType);
			}
		}

		/// <summary>
		/// Returns the Interop Version Description. 
		/// </summary>
		/// <returns>the Interop Version Description.</returns>
		private string GetInteropVersionDescription()  
		{
			if (!base.directory.ContainsTag(ExifInteropDirectory.TAG_INTEROP_VERSION))
				return null;
			int[] ints =
				base.directory.GetIntArray(ExifInteropDirectory.TAG_INTEROP_VERSION);
			return ExifDescriptor.ConvertBytesToVersionString(ints);
		}

		/// <summary>
		/// Returns the Interop index Description. 
		/// </summary>
		/// <returns>the Interop index Description.</returns>
		private string GetInteropIndexDescription() 
		{
			if (!base.directory.ContainsTag(ExifInteropDirectory.TAG_INTEROP_INDEX))
				return null;
			string interopIndex =
				base.directory.GetString(ExifInteropDirectory.TAG_INTEROP_INDEX).Trim();
			if ("R98".Equals(interopIndex.ToUpper())) 
			{
				return BUNDLE["RECOMMENDED_EXIF_INTEROPERABILITY"];
			} 
			else 
			{
				return BUNDLE["UNKNOWN", interopIndex.ToString()];
			}
		}
	}
}
