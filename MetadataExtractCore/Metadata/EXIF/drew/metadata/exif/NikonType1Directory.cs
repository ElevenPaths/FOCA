namespace com.drew.metadata.exif
{

	public class NikonType1Directory : AbstractNikonTypeDirectory 
	{
		// TYPE1 is for E-Series cameras prior to (not including) E990
		public const int TAG_NIKON_TYPE1_UNKNOWN_1 = 0x0002;
		public const int TAG_NIKON_TYPE1_QUALITY = 0x0003;
		public const int TAG_NIKON_TYPE1_COLOR_MODE = 0x0004;
		public const int TAG_NIKON_TYPE1_IMAGE_ADJUSTMENT = 0x0005;
		public const int TAG_NIKON_TYPE1_CCD_SENSITIVITY = 0x0006;
		public const int TAG_NIKON_TYPE1_WHITE_BALANCE = 0x0007;
		public const int TAG_NIKON_TYPE1_FOCUS = 0x0008;
		public const int TAG_NIKON_TYPE1_UNKNOWN_2 = 0x0009;
		public const int TAG_NIKON_TYPE1_DIGITAL_ZOOM = 0x000A;
		public const int TAG_NIKON_TYPE1_CONVERTER = 0x000B;
		public const int TAG_NIKON_TYPE1_UNKNOWN_3 = 0x0F00;

		/// <summary>
		/// Constructor of the object.
		/// </summary>
        public NikonType1Directory()
            : base("NikonTypeMarkernote")
		{
			this.SetDescriptor(new NikonType1Descriptor(this));
		}
	}
}