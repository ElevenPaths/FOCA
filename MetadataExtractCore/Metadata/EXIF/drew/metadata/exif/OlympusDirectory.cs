namespace com.drew.metadata.exif
{

	public class OlympusDirectory : AbstractDirectory 
	{
        /// <summary>
        ///  Used by Konica / Minolta cameras.
        /// </summary>
        public const int TAG_OLYMPUS_MAKERNOTE_VERSION = 0x0000;

        /// <summary>
        ///  Used by Konica / Minolta cameras.
        /// </summary>
        public const int TAG_OLYMPUS_CAMERA_SETTINGS_1 = 0x0001;

        /// <summary>
        ///  Alternate Camera Settings Tag. Used by Konica / Minolta cameras.
        /// </summary>
        public const int TAG_OLYMPUS_CAMERA_SETTINGS_2 = 0x0003;

        /// <summary>
        ///  Used by Konica / Minolta cameras.
        /// </summary>
        public const int TAG_OLYMPUS_COMPRESSED_IMAGE_SIZE = 0x0040;

        /// <summary>
        ///  Used by Konica / Minolta cameras.
        /// </summary>
        public const int TAG_OLYMPUS_MINOLTA_THUMBNAIL_OFFSET_1 = 0x0081;

        /// <summary>
        ///  Alternate Thumbnail Offset. Used by Konica / Minolta cameras.
        /// </summary>
        public const int TAG_OLYMPUS_MINOLTA_THUMBNAIL_OFFSET_2 = 0x0088;

        /// <summary>
        ///  Length of thumbnail in bytes. Used by Konica / Minolta cameras.
        /// </summary>
        public const int TAG_OLYMPUS_MINOLTA_THUMBNAIL_LENGTH = 0x0089;

        /// <summary>
        ///  Used by Konica / Minolta cameras
        ///  0 = Natural Color
        ///  1 = Black & White
        ///  2 = Vivid color
        ///  3 = Solarization
        ///  4 = AdobeRGB
        /// </summary>
        public const int TAG_OLYMPUS_COLOR_MODE = 0x0101;

        /// <summary>
        ///  Used by Konica / Minolta cameras.
        ///  0 = Raw
        ///  1 = Super Fine
        ///  2 = Fine
        ///  3 = Standard
        ///  4 = Extra Fine
        /// </summary>
        public const int TAG_OLYMPUS_IMAGE_QUALITY_1 = 0x0102;

        /// <summary>
        ///  Not 100% sure about this tag.
        /// 
        ///  Used by Konica / Minolta cameras.
        ///  0 = Raw
        ///  1 = Super Fine
        ///  2 = Fine
        ///  3 = Standard
        ///  4 = Extra Fine
        /// </summary>
        public const int TAG_OLYMPUS_IMAGE_QUALITY_2 = 0x0103;


        /// <summary>
        ///  Three values:
        ///  Value 1: 0=Normal, 2=Fast, 3=Panorama
        ///  Value 2: Sequence Number Value 3:
        ///  1 = Panorama Direction: Left to Right
        ///  2 = Panorama Direction: Right to Left
        ///  3 = Panorama Direction: Bottom to Top
        ///  4 = Panorama Direction: Top to Bottom
        /// </summary>
        public const int TAG_OLYMPUS_SPECIAL_MODE = 0x0200;

        /// <summary>
        ///  1 = Standard Quality
        ///  2 = High Quality
        ///  3 = Super High Quality
        /// </summary>
        public const int TAG_OLYMPUS_JPEG_QUALITY = 0x0201;

        /// <summary>
        ///  0 = Normal (Not Macro)
        ///  1 = Macro
        /// </summary>
        public const int TAG_OLYMPUS_MACRO_MODE = 0x0202;


        public const int TAG_OLYMPUS_UNKNOWN_1 = 0x0203;

        /// <summary>
        ///  Zoom Factor (0 or 1 = normal)
        /// </summary>
        public const int TAG_OLYMPUS_DIGI_ZOOM_RATIO = 0x0204;


        public const int TAG_OLYMPUS_UNKNOWN_2 = 0x0205;
        public const int TAG_OLYMPUS_UNKNOWN_3 = 0x0206;
        public const int TAG_OLYMPUS_FIRMWARE_VERSION = 0x0207;
        public const int TAG_OLYMPUS_PICT_INFO = 0x0208;
        public const int TAG_OLYMPUS_CAMERA_ID = 0x0209;

        /// <summary>
        ///  Used by Epson cameras
        ///  Units = pixels
        /// </summary>
        public const int TAG_OLYMPUS_IMAGE_WIDTH = 0x020B;

        /// <summary>
        ///  Used by Epson cameras
        ///  Units = pixels
        /// </summary>
        public const int TAG_OLYMPUS_IMAGE_HEIGHT = 0x020C;

        /// <summary>
        ///  A string. Used by Epson cameras.
        /// </summary>
        public const int TAG_OLYMPUS_ORIGINAL_MANUFACTURER_MODEL = 0x020D;

        /// <summary>
        ///  See the PIM specification here:
        ///  http://www.ozhiker.com/electronics/pjmt/jpeg_info/pim.html
        /// </summary>
        public const int TAG_OLYMPUS_PRINT_IMAGE_MATCHING_INFO = 0x0E00;


        public const int TAG_OLYMPUS_DATA_DUMP = 0x0F00;
        public const int TAG_OLYMPUS_FLASH_MODE = 0x1004;
        public const int TAG_OLYMPUS_BRACKET = 0x1006;
        public const int TAG_OLYMPUS_FOCUS_MODE = 0x100B;
        public const int TAG_OLYMPUS_FOCUS_DISTANCE = 0x100C;
        public const int TAG_OLYMPUS_ZOOM = 0x100D;
        public const int TAG_OLYMPUS_MACRO_FOCUS = 0x100E;
        public const int TAG_OLYMPUS_SHARPNESS = 0x100F;
        public const int TAG_OLYMPUS_COLOR_MATRIX = 0x1011;
        public const int TAG_OLYMPUS_BLACK_LEVEL = 0x1012;
        public const int TAG_OLYMPUS_WHITE_BALANCE = 0x1015;
        public const int TAG_OLYMPUS_RED_BIAS = 0x1017;
        public const int TAG_OLYMPUS_BLUE_BIAS = 0x1018;
        public const int TAG_OLYMPUS_SERIAL_NUMBER = 0x101A;
        public const int TAG_OLYMPUS_FLASH_BIAS = 0x1023;
        public const int TAG_OLYMPUS_CONTRAST = 0x1029;
        public const int TAG_OLYMPUS_SHARPNESS_FACTOR = 0x102A;
        public const int TAG_OLYMPUS_COLOR_CONTROL = 0x102B;
        public const int TAG_OLYMPUS_VALID_BITS = 0x102C;
        public const int TAG_OLYMPUS_CORING_FILTER = 0x102D;
        public const int TAG_OLYMPUS_FINAL_WIDTH = 0x102E;
        public const int TAG_OLYMPUS_FINAL_HEIGHT = 0x102F;
        public const int TAG_OLYMPUS_COMPRESSION_RATIO = 0x1034;

		/// <summary>
		/// Constructor of the object.
		/// </summary>
        public OlympusDirectory()
            : base("OlympusMarkernote")
		{
			this.SetDescriptor(new OlympusDescriptor(this));
		}
	}
}
