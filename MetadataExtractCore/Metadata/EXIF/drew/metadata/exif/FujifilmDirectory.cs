namespace com.drew.metadata.exif
{
	/// <summary>
	/// The Fuji Film Makernote Directory
	/// </summary>

	public class FujifilmDirectory : AbstractDirectory 
	{
		public const int TAG_FUJIFILM_MAKERNOTE_VERSION = 0x0000;
		public const int TAG_FUJIFILM_QUALITY = 0x1000;
		public const int TAG_FUJIFILM_SHARPNESS = 0x1001;
		public const int TAG_FUJIFILM_WHITE_BALANCE = 0x1002;
		public const int TAG_FUJIFILM_COLOR = 0x1003;
		public const int TAG_FUJIFILM_TONE = 0x1004;
		public const int TAG_FUJIFILM_FLASH_MODE = 0x1010;
		public const int TAG_FUJIFILM_FLASH_STRENGTH = 0x1011;
		public const int TAG_FUJIFILM_MACRO = 0x1020;
		public const int TAG_FUJIFILM_FOCUS_MODE = 0x1021;
		public const int TAG_FUJIFILM_SLOW_SYNCHRO = 0x1030;
		public const int TAG_FUJIFILM_PICTURE_MODE = 0x1031;
		public const int TAG_FUJIFILM_UNKNOWN_1 = 0x1032;
		public const int TAG_FUJIFILM_CONTINUOUS_TAKING_OR_AUTO_BRACKETTING =	0x1100;
		public const int TAG_FUJIFILM_UNKNOWN_2 = 0x1200;
		public const int TAG_FUJIFILM_BLUR_WARNING = 0x1300;
		public const int TAG_FUJIFILM_FOCUS_WARNING = 0x1301;
		public const int TAG_FUJIFILM_AE_WARNING = 0x1302;

		/// <summary>
		/// Constructor of the object.
		/// </summary>
        public FujifilmDirectory()
            : base("FujiFilmMarkernote")
		{
			this.SetDescriptor(new FujifilmDescriptor(this));
		}

	}
}