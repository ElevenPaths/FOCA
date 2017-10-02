namespace com.drew.metadata.exif
{

	public class NikonType2Directory : AbstractNikonTypeDirectory 
	{
        /// <summary>
        /// Values observed
        /// - 0200 (D70)
        /// - 0200 (D1X)
        /// </summary>
        public const int TAG_NIKON_TYPE2_FIRMWARE_VERSION = 0x0001;

        /// <summary>
        /// Values observed
        /// - 0 250
        /// - 0 400
        /// </summary>
        public const int TAG_NIKON_TYPE2_ISO_1 = 0x0002;

        /// <summary>
        /// Values observed
        /// - COLOR (seen in the D1X)
        /// </summary>
        public const int TAG_NIKON_TYPE2_COLOR_MODE = 0x0003;

        /// <summary>
        /// Values observed
        /// - FILE
        /// - RAW
        /// - NORMAL
        /// - FINE
        /// </summary>
        public const int TAG_NIKON_TYPE2_QUALITY_AND_FILE_FORMAT = 0x0004;

        /// <summary>
        /// The white balance as set in the camera.
        ///
        /// Values observed
        /// - AUTO
        /// - SUNNY (D70)
        /// - FLASH (D1X)
        /// (presumably also SHADOW / INCANDESCENT / FLUORESCENT / CLOUDY)
        /// </summary>
        public const int TAG_NIKON_TYPE2_CAMERA_WHITE_BALANCE = 0x0005;

        /// <summary>
        /// The sharpening as set in the camera.
        ///
        /// Values observed
        /// - AUTO
        /// - NORMAL (D70)
        /// - NONE (D1X)
        /// </summary>
        public const int TAG_NIKON_TYPE2_CAMERA_SHARPENING = 0x0006;

        /// <summary>
        /// The auto-focus type used by the camera.
        ///
        /// Values observed
        /// - AF-S
        /// - AF-C
        /// - MANUAL
        /// </summary>
        public const int TAG_NIKON_TYPE2_AF_TYPE = 0x0007;

        /// <summary>
        /// Values observed
        /// - NORMAL
        /// - RED-EYE
        ///
        /// Note: when TAG_NIKON_TYPE2_AUTO_FLASH_MODE is blank, Nikon Browser displays "Flash Sync Mode: Not Attached"
        /// </summary>
        public const int TAG_NIKON_TYPE2_FLASH_SYNC_MODE = 0x0008;

        /// <summary>
        /// Values observed
        /// - Built-in,TTL
        /// - Optional,TTL (with speedlight SB800, flash sync mode as NORMAL.  NikonBrowser reports Auto Flash Comp: 0 EV -- which tag is that?) (D70)
        /// - NEW_TTL (Nikon Browser interprets as "D-TTL")
        /// - (blank -- accompanied FlashSyncMode of NORMAL) (D70)
        /// </summary>
        public const int TAG_NIKON_TYPE2_AUTO_FLASH_MODE = 0x0009;

        /// <summary>
        /// Added during merge of Type2 & Type3.  May apply to earlier models, such as E990 and D1.
        /// </summary>
        public const int TAG_NIKON_TYPE2_UNKNOWN_34 = 0x000A;

        /// <summary>
        /// Values observed
        /// - 0
        /// </summary>
        public const int TAG_NIKON_TYPE2_CAMERA_WHITE_BALANCE_FINE = 0x000B;

        /// <summary>
        /// The first two numbers are coefficients to multiply red and blue channels according to white balance as set in the
        /// camera. The meaning of the third and the fourth numbers is unknown.
        ///
        /// Values observed
        /// - 2.25882352 1.76078431 0.0 0.0
        /// - 10242/1 34305/1 0/1 0/1
        /// - 234765625/100000000 1140625/1000000 1/1 1/1
        /// </summary>
        public const int TAG_NIKON_TYPE2_CAMERA_WHITE_BALANCE_RB_COEFF = 0x000C;

        /// <summary>
        /// Values observed
        /// - 0,1,6,0 (hex)
        /// </summary>
        public const int TAG_NIKON_TYPE2_UNKNOWN_1 = 0x000D;

        /// <summary>
        /// Values observed
        /// - î
        /// - 0,1,c,0 (hex)
        /// </summary>
        public const int TAG_NIKON_TYPE2_UNKNOWN_2 = 0x000E;

        /// <summary>
        /// Added during merge of Type2 & Type3.  May apply to earlier models, such as E990 and D1.
        /// </summary>
        public const int TAG_NIKON_TYPE2_ISO_SELECTION = 0x000F;

        /// <summary>
        /// Added during merge of Type2 & Type3.  May apply to earlier models, such as E990 and D1.
        /// </summary>
        public const int TAG_NIKON_TYPE2_DATA_DUMP = 0x0010;

        /// <summary>
        /// Values observed
        /// - 914
        /// - 1379 (D70)
        /// - 2781 (D1X)
        /// - 6942 (D100)
        /// </summary>
        public const int TAG_NIKON_TYPE2_UNKNOWN_3 = 0x0011;

        /// <summary>
        /// Values observed
        /// - (no value -- blank)
        /// </summary>
        public const int TAG_NIKON_TYPE2_AUTO_FLASH_COMPENSATION = 0x0012;

        /// <summary>
        /// Values observed
        /// - 0 250
        /// - 0 400
        /// </summary>
        public const int TAG_NIKON_TYPE2_ISO_2 = 0x0013;

        /// <summary>
        /// Values observed
        /// - 0 0 49163 53255
        /// - 0 0 3008 2000 (the image dimensions were 3008x2000) (D70)
        /// </summary>
        public const int TAG_NIKON_TYPE2_UNKNOWN_21 = 0x0016;

        /// <summary>
        /// Values observed
        /// - (blank)
        /// </summary>
        public const int TAG_NIKON_TYPE2_UNKNOWN_22 = 0x0017;

        /// <summary>
        /// Values observed
        /// - (blank)
        /// </summary>
        public const int TAG_NIKON_TYPE2_UNKNOWN_23 = 0x0018;

        /// <summary>
        /// Values observed
        /// - 0
        /// </summary>
        public const int TAG_NIKON_TYPE2_UNKNOWN_24 = 0x0019;

        /// <summary>
        /// Added during merge of Type2 & Type3.  May apply to earlier models, such as E990 and D1.
        /// </summary>
        public const int TAG_NIKON_TYPE2_IMAGE_ADJUSTMENT = 0x0080;

        /// <summary>
        /// The tone compensation as set in the camera.
        ///
        /// Values observed
        /// - AUTO
        /// - NORMAL (D1X, D100)
        /// </summary>
        public const int TAG_NIKON_TYPE2_CAMERA_TONE_COMPENSATION = 0x0081;

        /// <summary>
        /// Added during merge of Type2 & Type3.  May apply to earlier models, such as E990 and D1.
        /// </summary>
        public const int TAG_NIKON_TYPE2_ADAPTER = 0x0082;

        /// <summary>
        /// Values observed
        /// - 6
        /// - 6 (D70)
        /// - 2 (D1X)
        /// </summary>
        public const int TAG_NIKON_TYPE2_UNKNOWN_4 = 0x0083;

        /// <summary>
        /// A pair of focal/max-fstop values that describe the lens used.
        ///
        /// Values observed
        /// - 180.0,180.0,2.8,2.8 (D100)
        /// - 240/10 850/10 35/10 45/10
        /// - 18-70mm f/3.5-4.5 (D70)
        /// - 17-35mm f/2.8-2.8 (D1X)
        /// - 70-200mm f/2.8-2.8 (D70)
        ///
        /// Nikon Browser identifies the lens as "18-70mm F/3.5-4.5 G" which
        /// is identical to lcMetadata extractor, except for the "G".  This must
        /// be coming from another tag...
        /// </summary>
        public const int TAG_NIKON_TYPE2_LENS = 0x0084;

        /// <summary>
        /// Added during merge of Type2 & Type3.  May apply to earlier models, such as E990 and D1.
        /// </summary>
        public const int TAG_NIKON_TYPE2_MANUAL_FOCUS_DISTANCE = 0x0085;

        /// <summary>
        /// Added during merge of Type2 & Type3.  May apply to earlier models, such as E990 and D1.
        /// </summary>
        public const int TAG_NIKON_TYPE2_DIGITAL_ZOOM = 0x0086;

        /// <summary>
        /// Values observed
        /// - 0
        /// - 9
        /// - 3 (D1X)
        /// </summary>
        public const int TAG_NIKON_TYPE2_UNKNOWN_5 = 0x0087;

        /// <summary>
        /// Values observed
        /// -
        /// </summary>
        public const int TAG_NIKON_TYPE2_AF_FOCUS_POSITION = 0x0088;

        /// <summary>
        /// Values observed
        /// - 0
        /// - 1
        /// </summary>
        public const int TAG_NIKON_TYPE2_UNKNOWN_7 = 0x0089;

        /// <summary>
        /// Values observed
        /// - 0
        /// - 0
        /// </summary>
        public const int TAG_NIKON_TYPE2_UNKNOWN_20 = 0x008A;

        /// <summary>
        /// Values observed
        /// - 48,1,c,0 (hex) (D100)
        /// - @
        /// </summary>
        public const int TAG_NIKON_TYPE2_UNKNOWN_8 = 0x008B;

        /// <summary>
        /// Unknown.  Fabrizio believes this may be a lookup table for the user-defined curve.
        ///
        /// Values observed
        /// - (blank) (D1X)
        /// </summary>
        public const int TAG_NIKON_TYPE2_UNKNOWN_9 = 0x008C;

        /// <summary>
        /// The color space as set in the camera.
        ///
        /// Values observed
        /// - MODE1
        /// - Mode I (sRGB) (D70)
        /// - MODE2 (D1X, D100)
        /// </summary>
        public const int TAG_NIKON_TYPE2_CAMERA_COLOR_MODE = 0x008D;

        /// <summary>
        /// Values observed
        /// - NATURAL
        /// - SPEEDLIGHT (D70, D1X)
        /// </summary>
        public const int TAG_NIKON_TYPE2_LIGHT_SOURCE = 0x0090;

        /// <summary>
        /// Values observed
        /// - 0100)
        /// - 0103 (D70)
        /// - 0100 (D1X)
        /// </summary>
        public const int TAG_NIKON_TYPE2_UNKNOWN_11 = 0x0091;

        /// <summary>
        /// The hue adjustment as set in the camera.
        ///
        /// Values observed
        /// - 0
        /// </summary>
        public const int TAG_NIKON_TYPE2_CAMERA_HUE_ADJUSTMENT = 0x0092;

        /// <summary>
        /// Values observed
        /// - OFF
        /// </summary>
        public const int TAG_NIKON_TYPE2_NOISE_REDUCTION = 0x0095;

        /// <summary>
        /// Values observed
        /// - 0100'~e3
        /// - 0103
        /// </summary>
        public const int TAG_NIKON_TYPE2_UNKNOWN_12 = 0x0097;

        /// <summary>
        /// Values observed
        /// - 0100fht@7b,4x,D"Y
        /// - 01015
        /// - 0100w\cH+D$$h$î5Q (D1X)
        /// - 30,31,30,30,0,0,b,48,7c,7c,24,24,5,15,24,0,0,0,0,0 (hex) (D100)
        /// </summary>
        public const int TAG_NIKON_TYPE2_UNKNOWN_13 = 0x0098;

        /// <summary>
        /// Values observed
        /// - 2014 662 (D1X)
        /// - 1517,1012 (D100)
        /// </summary>
        public const int TAG_NIKON_TYPE2_UNKNOWN_14 = 0x0099;

        /// <summary>
        /// Values observed
        /// - 78/10 78/10
        /// - 78/10 78/10 (D70)
        /// - 59/10 59/5 (D1X)
        /// - 7.8,7.8 (D100)
        /// </summary>
        public const int TAG_NIKON_TYPE2_UNKNOWN_15 = 0x009A;

        /// <summary>
        /// Values observed
        /// - NO= 00002539
        /// </summary>
        public const int TAG_NIKON_TYPE2_UNKNOWN_25 = 0x00A0;

        /// <summary>
        /// Values observed
        /// - 1564851
        /// </summary>
        public const int TAG_NIKON_TYPE2_UNKNOWN_26 = 0x00A2;

        /// <summary>
        /// Values observed
        /// - 0
        /// </summary>
        public const int TAG_NIKON_TYPE2_UNKNOWN_27 = 0x00A3;

        /// <summary>
        /// This appears to be a sequence number to indentify the exposure.  This value seems to increment
        /// for constecutive exposures (observed on D70).
        ///
        /// Values observed
        /// - 5062
        /// </summary>
        public const int TAG_NIKON_TYPE2_EXPOSURE_SEQUENCE_NUMBER = 0x00A7;

        /// <summary>
        /// Values observed
        /// - 0100 (D70)
        /// </summary>
        public const int TAG_NIKON_TYPE2_UNKNOWN_32 = 0x00A8;

        /// <summary>
        /// Values observed
        /// - NORMAL (D70)
        /// </summary>
        public const int TAG_NIKON_TYPE2_UNKNOWN_33 = 0x00A9;

        /// <summary>
        /// Nikon Browser suggests this value represents Saturation...
        /// Values observed
        /// - NORMAL (D70)
        /// </summary>
        public const int TAG_NIKON_TYPE2_UNKNOWN_29 = 0x00AA;

        /// <summary>
        /// Values observed
        /// - AUTO (D70)
        /// - (blank) (D70)
        /// </summary>
        public const int TAG_NIKON_TYPE2_UNKNOWN_30 = 0x00AB;

        /// <summary>
        /// Data about changes set by Nikon Capture Editor.
        ///
        /// Values observed
        /// </summary>
        public const int TAG_NIKON_TYPE2_CAPTURE_EDITOR_DATA = 0x0E01;

        /// <summary>
        /// Values observed
        /// - 1473
        /// - 7036 (D100)
        /// </summary>
        public const int TAG_NIKON_TYPE2_UNKNOWN_16 = 0x0E10;

		/// <summary>
		/// Constructor of the object.
		/// </summary>
        public NikonType2Directory()
            : base("NikonTypeMarkernote")
		{
			this.SetDescriptor(new NikonType2Descriptor(this));
		}
	}
}
