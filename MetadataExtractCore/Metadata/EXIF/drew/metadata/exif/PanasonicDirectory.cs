namespace com.drew.metadata.exif
{
    /// <summary>
    /// The panasonic directory class.
    /// </summary>

	public class PanasonicDirectory : AbstractDirectory 
	{
        public const int TAG_PANASONIC_QUALITY_MODE = 0x0001;
        public const int TAG_PANASONIC_VERSION = 0x0002;

        /// <summary>
        ///  1 = On
        ///  2 = Off
        /// </summary>
        public const int TAG_PANASONIC_MACRO_MODE = 0x001C;

        /// <summary>
        ///  1 = Normal
        ///  2 = Portrait
        ///  9 = Macro 
        /// </summary>
        public const int TAG_PANASONIC_RECORD_MODE = 0x001F;
        public const int TAG_PANASONIC_PRINT_IMAGE_MATCHING_INFO = 0x0E00;

		/// <summary>
		/// Constructor of the object.
		/// </summary>
        public PanasonicDirectory()
            : base("PanasonicMarkernote")
		{
			this.SetDescriptor(new PanasonicDescriptor(this));
		}
	}
}
