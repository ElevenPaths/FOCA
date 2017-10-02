namespace com.drew.metadata.exif
{
    /// <summary>
    /// This class represents CASIO marker note type 2.
    /// </summary>

    public class CasioType2Directory : AbstractCasioTypeDirectory
    {
        /// <summary>
        /// 2 values - x,y dimensions in pixels.
        /// </summary>
        public const int TAG_CASIO_TYPE2_THUMBNAIL_DIMENSIONS = 0x0002;

        /// <summary>
        /// Size in bytes
        /// </summary>
        public const int TAG_CASIO_TYPE2_THUMBNAIL_SIZE = 0x0003;

        /// <summary>
        /// Offset of Preview Thumbnail
        /// </summary>
        public const int TAG_CASIO_TYPE2_THUMBNAIL_OFFSET = 0x0004;

        /// <summary>
        /// 1 = Fine
        /// 2 = Super Fine
        /// </summary>
        public const int TAG_CASIO_TYPE2_QUALITY_MODE = 0x0008;

        /// <summary>
        /// 0 = 640 x 480 pixels
        /// 4 = 1600 x 1200 pixels
        /// 5 = 2048 x 1536 pixels
        /// 20 = 2288 x 1712 pixels
        /// 21 = 2592 x 1944 pixels
        /// 22 = 2304 x 1728 pixels
        /// 36 = 3008 x 2008 pixels
        /// </summary>
        public const int TAG_CASIO_TYPE2_IMAGE_SIZE = 0x0009;

        /// <summary>
        /// 0 = Normal
        /// 1 = Macro
        /// </summary>
        public const int TAG_CASIO_TYPE2_FOCUS_MODE_1 = 0x000D;
        
        /// <summary>
        /// 3 = 50
        /// 4 = 64
        /// 6 = 100
        /// 9 = 200
        /// </summary>
        public const int TAG_CASIO_TYPE2_ISO_SENSITIVITY = 0x0014;

        /// <summary>
        /// 0 = Auto
        /// 1 = Daylight
        /// 2 = Shade
        /// 3 = Tungsten
        /// 4 = Fluorescent
        /// 5 = Manual
        /// </summary>
        public const int TAG_CASIO_TYPE2_WHITE_BALANCE_1 = 0x0019;

        /// <summary>
        /// Units are tenths of a millimetre
        /// </summary>
        public const int TAG_CASIO_TYPE2_FOCAL_LENGTH = 0x001D;
        
        /// <summary>
        /// 0 = -1
        /// 1 = Normal
        /// 2 = +1
        /// </summary>
        public const int TAG_CASIO_TYPE2_SATURATION = 0x001F;

        /// <summary>
        /// 0 = -1 
        /// 1 = Normal 
        /// 2 = +1
        /// </summary>
        public const int TAG_CASIO_TYPE2_CONTRAST = 0x0020;

        /// <summary>
        /// 0 = -1
        /// 1 = Normal
        /// 2 = +1
        /// </summary>
        public const int TAG_CASIO_TYPE2_SHARPNESS = 0x0021;

        /// <summary>
        /// See PIM specification here: http://www.ozhiker.com/electronics/pjmt/jpeg_info/pim.html
        /// </summary>
        public const int TAG_CASIO_TYPE2_PRINT_IMAGE_MATCHING_INFO = 0x0E00;

        /// <summary>
        /// Alternate thumbnail lcOffset
        /// </summary>
        public const int TAG_CASIO_TYPE2_CASIO_PREVIEW_THUMBNAIL = 0x2000;

        public const int TAG_CASIO_TYPE2_WHITE_BALANCE_BIAS = 0x2011;
                
        /// <summary>
        /// 12 = Flash
        /// 0 = Manual
        /// 1 = Auto?
        /// 4 = Flash?
        /// </summary>
        public const int TAG_CASIO_TYPE2_WHITE_BALANCE_2 = 0x2012;

        /// <summary>
        /// Units are millimetres
        /// </summary>
        public const int TAG_CASIO_TYPE2_OBJECT_DISTANCE = 0x2022;

        /// <summary>
        /// 0 = Off
        /// </summary>
        public const int TAG_CASIO_TYPE2_FLASH_DISTANCE = 0x2034;

        /// <summary>
        /// 2 = Normal Mode
        /// </summary>
        public const int TAG_CASIO_TYPE2_RECORD_MODE = 0x3000;

        /// <summary>
        /// 1 = Off?
        /// </summary>
        public const int TAG_CASIO_TYPE2_SELF_TIMER = 0x3001;

        /// <summary>
        /// 3 = Fine
        /// </summary>
        public const int TAG_CASIO_TYPE2_QUALITY = 0x3002;

        /// <summary>
        /// 1 = Fixation
        /// 6 = Multi-Area Auto Focus
        /// </summary>
        public const int TAG_CASIO_TYPE2_FOCUS_MODE_2 = 0x3003;

        public const int TAG_CASIO_TYPE2_TIME_ZONE = 0x3006;
        public const int TAG_CASIO_TYPE2_BESTSHOT_MODE = 0x3007;
        
        /// <summary>
        /// 0 = Off
        /// 1 = On?
        /// </summary>
        public const int TAG_CASIO_TYPE2_CCD_ISO_SENSITIVITY = 0x3014;
        
        /// <summary>
        /// 0 = Off
        /// </summary>
        public const int TAG_CASIO_TYPE2_COLOR_MODE = 0x3015;

        /// <summary>
        /// 0 = Off
        /// </summary>
        public const int TAG_CASIO_TYPE2_ENHANCEMENT = 0x3016;

        /// <summary>
        /// 0 = Off
        /// </summary>
        public const int TAG_CASIO_TYPE2_FILTER = 0x3017;

        /// <summary>
        /// Constructor of the object.
        /// </summary>
        public CasioType2Directory()
            : base("CasioMarkernote")
        {
            base.SetDescriptor(new CasioType2Descriptor(this));
        }
    }
}
