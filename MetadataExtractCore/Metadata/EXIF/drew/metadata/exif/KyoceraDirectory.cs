namespace com.drew.metadata.exif
{
	/// <summary>
	/// The GPS Directory class
	/// </summary>

	public class KyoceraDirectory : AbstractDirectory 
	{
        public const int TAG_KYOCERA_PROPRIETARY_THUMBNAIL = 0x0001;
        public const int TAG_KYOCERA_PRINT_IMAGE_MATCHING_INFO = 0x0E00;

		/// <summary>
		/// Constructor of the object.
		/// </summary>
        public KyoceraDirectory()
            : base("KyoceraMarkernote")
		{
            this.SetDescriptor(new KyoceraDescriptor(this));
		}
	} 
}
