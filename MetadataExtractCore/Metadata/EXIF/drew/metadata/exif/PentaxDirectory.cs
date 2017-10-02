namespace com.drew.metadata.exif
{
    /// <summary>
    /// The pentax directory class.
    /// </summary>

	public class PentaxDirectory : AbstractDirectory 
	{
        /// <summary>
        ///  0 = Auto
        ///  1 = Night-scene
        ///  2 = Manual
        ///  4 = Multiple
        /// </summary>
        public const int TAG_PENTAX_CAPTURE_MODE = 0x0001;

        /// <summary>
        ///  0 = Good
        ///  1 = Better
        ///  2 = Best
        /// </summary>
        public const int TAG_PENTAX_QUALITY_LEVEL = 0x0002;

        /// <summary>
        ///  2 = Custom
        ///  3 = Auto
        /// </summary>
        public const int TAG_PENTAX_FOCUS_MODE = 0x0003;

        /// <summary>
        ///  1 = Auto
        ///  2 = Flash on
        ///  4 = Flash off
        ///  6 = Red-eye Reduction
        /// </summary>
        public const int TAG_PENTAX_FLASH_MODE = 0x0004;

        /// <summary>
        ///  0 = Auto
        ///  1 = Daylight
        ///  2 = Shade
        ///  3 = Tungsten
        ///  4 = Fluorescent
        ///  5 = Manual
        /// </summary>
        public const int TAG_PENTAX_WHITE_BALANCE = 0x0007;

        /// <summary>
        ///  (0 = Off)
        /// </summary>
        public const int TAG_PENTAX_DIGITAL_ZOOM = 0x000A;

        /// <summary>
        ///  0 = Normal
        ///  1 = Soft
        ///  2 = Hard
        /// </summary>
        public const int TAG_PENTAX_SHARPNESS = 0x000B;

        /// <summary>
        ///  0 = Normal
        ///  1 = Low
        ///  2 = High
        /// </summary>
        public const int TAG_PENTAX_CONTRAST = 0x000C;

        /// <summary>
        ///  0 = Normal
        ///  1 = Low
        ///  2 = High
        /// </summary>
        public const int TAG_PENTAX_SATURATION = 0x000D;

        /// <summary>
        ///  10 = ISO 100
        ///  16 = ISO 200
        ///  100 = ISO 100
        ///  200 = ISO 200
        /// </summary>
        public const int TAG_PENTAX_ISO_SPEED = 0x0014;

        /// <summary>
        ///  1 = Normal
        ///  2 = Black & White
        ///  3 = Sepia
        /// </summary>
        public const int TAG_PENTAX_COLOR = 0x0017;

        /// <summary>
        ///  See Print Image Matching for specification.
        ///  http://www.ozhiker.com/electronics/pjmt/jpeg_info/pim.html
        /// </summary>
        public const int TAG_PENTAX_PRINT_IMAGE_MATCHING_INFO = 0x0E00;

        /// <summary>
        ///  (String).
        /// </summary>
        public const int TAG_PENTAX_TIME_ZONE = 0x1000;

        /// <summary>
        ///  (String).
        /// </summary>
        public const int TAG_PENTAX_DAYLIGHT_SAVINGS = 0x1001;


		/// <summary>
		/// Constructor of the object.
		/// </summary>
        public PentaxDirectory()
            : base("PentaxMarkernote")
		{
			this.SetDescriptor(new PentaxDescriptor(this));
		}

	}
}
