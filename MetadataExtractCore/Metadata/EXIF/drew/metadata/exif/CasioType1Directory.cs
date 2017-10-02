namespace com.drew.metadata.exif
{
	/// <summary>
	/// This class represents CASIO marker note.
	/// </summary>

	public class CasioType1Directory : AbstractCasioTypeDirectory 
	{
		public const int TAG_CASIO_RECORDING_MODE = 0x0001;
		public const int TAG_CASIO_QUALITY = 0x0002;
		public const int TAG_CASIO_FOCUSING_MODE = 0x0003;
		public const int TAG_CASIO_FLASH_MODE = 0x0004;
		public const int TAG_CASIO_FLASH_INTENSITY = 0x0005;
		public const int TAG_CASIO_OBJECT_DISTANCE = 0x0006;
		public const int TAG_CASIO_WHITE_BALANCE = 0x0007;
		public const int TAG_CASIO_UNKNOWN_1 = 0x0008;
		public const int TAG_CASIO_UNKNOWN_2 = 0x0009;
		public const int TAG_CASIO_DIGITAL_ZOOM = 0x000A;
		public const int TAG_CASIO_SHARPNESS = 0x000B;
		public const int TAG_CASIO_CONTRAST = 0x000C;
		public const int TAG_CASIO_SATURATION = 0x000D;
		public const int TAG_CASIO_UNKNOWN_3 = 0x000E;
		public const int TAG_CASIO_UNKNOWN_4 = 0x000F;
		public const int TAG_CASIO_UNKNOWN_5 = 0x0010;
		public const int TAG_CASIO_UNKNOWN_6 = 0x0011;
		public const int TAG_CASIO_UNKNOWN_7 = 0x0012;
		public const int TAG_CASIO_UNKNOWN_8 = 0x0013;
		public const int TAG_CASIO_CCD_SENSITIVITY = 0x0014;

		/// <summary>
		/// Constructor of the object.
		/// </summary>
        public CasioType1Directory()
            : base("CasioMarkernote")
		{
			base.SetDescriptor(new CasioType1Descriptor(this));
		}

	}
}
