namespace com.drew.metadata.jpeg
{
	/// <summary>
	/// Tag descriptor for Jpeg
	/// </summary>

	public class JpegDescriptor : AbstractTagDescriptor 
	{
		/// <summary>
		/// Constructor of the object
		/// </summary>
		/// <param name="directory">a directory</param>
		public JpegDescriptor(AbstractDirectory directory) : base(directory)
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
				case JpegDirectory.TAG_JPEG_COMPONENT_DATA_1 :
					return GetComponentDataDescription(0);
				case JpegDirectory.TAG_JPEG_COMPONENT_DATA_2 :
					return GetComponentDataDescription(1);
				case JpegDirectory.TAG_JPEG_COMPONENT_DATA_3 :
					return GetComponentDataDescription(2);
				case JpegDirectory.TAG_JPEG_COMPONENT_DATA_4 :
					return GetComponentDataDescription(3);
				case JpegDirectory.TAG_JPEG_DATA_PRECISION :
					return GetDataPrecisionDescription();
				case JpegDirectory.TAG_JPEG_IMAGE_HEIGHT :
					return GetImageHeightDescription();
				case JpegDirectory.TAG_JPEG_IMAGE_WIDTH :
					return GetImageWidthDescription();
				default :
					return base.directory.GetString(tagType);
			}
		}

		/// <summary>
		/// Gets the image width description
		/// </summary>
		/// <returns>the image width description</returns>
		public string GetImageWidthDescription() 
		{
            return BUNDLE["PIXELS", base.directory.GetString(JpegDirectory.TAG_JPEG_IMAGE_WIDTH)];
		}

		/// <summary>
		/// Gets the image height description
		/// </summary>
		/// <returns>the image height description</returns>
		public string GetImageHeightDescription() 
		{
            return BUNDLE["PIXELS", base.directory.GetString(JpegDirectory.TAG_JPEG_IMAGE_HEIGHT)];
		}

		/// <summary>
		/// Gets the Data Precision description
		/// </summary>
		/// <returns>the Data Precision description</returns>
		public string GetDataPrecisionDescription() 
		{
            return BUNDLE["BITS", base.directory.GetString(JpegDirectory.TAG_JPEG_DATA_PRECISION)];
		}

		/// <summary>
		/// Gets the Component Data description
		/// </summary>
		/// <param name="componentNumber">the component number</param>
		/// <returns>the Component Data description</returns>
		public string GetComponentDataDescription(int componentNumber) 
		{
			JpegComponent component =
                ((JpegDirectory)base.directory).GetComponent(componentNumber);
			if (component == null) 
			{
				throw new MetadataException("No Jpeg component exists with number " + componentNumber);
			}

			// {0} component: Quantization table {1}, Sampling factors {2} horiz/{3} vert
			string[] tab = new string[] {component.GetComponentName(), 
											component.QuantizationTableNumber.ToString(),
											component.HorizontalSamplingFactor.ToString(),
											component.VerticalSamplingFactor.ToString()};

			return BUNDLE["COMPONENT_DATA", tab];
		}
	}
}