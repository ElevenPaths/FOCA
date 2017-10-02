namespace com.drew.metadata.exif
{
  /// <summary>
  /// This class represents CANON marker note.
  /// </summary>

    public class CanonDirectory : AbstractDirectory
    {
        // CANON cameras have some funny bespoke fields that need further processing...
        public const int TAG_CANON_CAMERA_STATE_1 = 0x0001;
        public const int TAG_CANON_CAMERA_STATE_2 = 0x0004;

        public const int TAG_CANON_IMAGE_TYPE = 0x0006;
        public const int TAG_CANON_FIRMWARE_VERSION = 0x0007;
        public const int TAG_CANON_IMAGE_NUMBER = 0x0008;
        public const int TAG_CANON_OWNER_NAME = 0x0009;
        /// <summary>
        ///  To display serial number as on camera use: printf( "%04X%05d", highbyte, lowbyte )
        ///  TODO handle this in CanonMakernoteDescriptor
        /// </summary>
        public const int TAG_CANON_SERIAL_NUMBER = 0x000C;
        /// <summary>
        /// xb: 15.05.2008 -- http://owl.phy.queensu.ca/~phil/exiftool/TagNames/Canon.html#CameraSettings
        ///     Old State TAG_CANON_UNKNOWN_1
        /// </summary>
        public const int TAG_CANON_CanonCameraInfo = 0x000D;
        public const int TAG_CANON_CUSTOM_FUNCTIONS = 0x000F;

        // These 'sub'-tag values have been created for consistency -- they don't exist within the exif segment
        /// <summary>
        ///  1 = Macro
        ///  2 = Normal
        /// </summary>
        public const int TAG_CANON_STATE1_MACRO_MODE = 0xC101;
        public const int TAG_CANON_STATE1_SELF_TIMER_DELAY = 0xC102;
        /// <summary>
        ///  2 = Normal
        ///  3 = Fine
        ///  5 = Superfine
        /// </summary>
        public const int TAG_CANON_STATE1_QUALITY = 0xC103;
        /// <summary>
        ///  0 = Flash Not Fired
        ///  1 = Auto
        ///  2 = On
        ///  3 = Red Eye Reduction
        ///  4 = Slow Synchro
        ///  5 = Auto + Red Eye Reduction
        ///  6 = On + Red Eye Reduction
        ///  16 = External Flash
        /// </summary>
        public const int TAG_CANON_STATE1_FLASH_MODE = 0xC104;
        /// <summary>
        ///  0 = Single Frame or Timer Mode
        ///  1 = Continuous
        /// </summary>
        public const int TAG_CANON_STATE1_CONTINUOUS_DRIVE_MODE = 0xC105;
        public const int TAG_CANON_STATE1_UNKNOWN_2 = 0xC106;
        /// <summary>
        ///  0 = One-Shot
        ///  1 = AI Servo
        ///  2 = AI Focus
        ///  3 = Manual Focus
        ///  4 = Single
        ///  5 = Continuous
        ///  6 = Manual Focus
        /// </summary>
        public const int TAG_CANON_STATE1_FOCUS_MODE_1 = 0xC107;
        public const int TAG_CANON_STATE1_UNKNOWN_3 = 0xC108;
        
        /// <summary>
        /// xb: 15.05.2008 -- http://owl.phy.queensu.ca/~phil/exiftool/TagNames/Canon.html#CameraSettings
        ///     Old State TAG_CANON_STATE1_UNKNOWN_4
        /// 1 = JPEG 
        /// 2 = CRW+THM 
        /// 3 = AVI+THM 
        /// 4 = TIF 
        /// 5 = TIF+JPEG 
        /// 6 = CR2 
        /// 7 = CR2+JPEG
        /// </summary>
        public const int TAG_CANON_STATE1_RecordMode = 0xC109;
        
        /// <summary>
        ///  0 = Large
        ///  1 = Medium
        ///  2 = Small
        /// </summary>
        public const int TAG_CANON_STATE1_IMAGE_SIZE = 0xC10A;
        /// <summary>
        ///  0 = Full Auto
        ///  1 = Manual
        ///  2 = Landscape
        ///  3 = Fast Shutter
        ///  4 = Slow Shutter
        ///  5 = Night
        ///  6 = Black & White
        ///  7 = Sepia
        ///  8 = Portrait
        ///  9 = Sports
        ///  10 = Macro / Close-Up
        ///  11 = Pan Focus
        /// </summary>
        public const int TAG_CANON_STATE1_EASY_SHOOTING_MODE = 0xC10B;
        /// <summary>
        ///  0 = No Digital Zoom
        ///  1 = 2x
        ///  2 = 4x
        /// </summary>
        public const int TAG_CANON_STATE1_DIGITAL_ZOOM = 0xC10C;
        /// <summary>
        ///  0 = Normal
        ///  1 = High
        ///  65535 = Low
        /// </summary>
        public const int TAG_CANON_STATE1_CONTRAST = 0xC10D;
        /// <summary>
        ///  0 = Normal
        ///  1 = High
        ///  65535 = Low
        /// </summary>
        public const int TAG_CANON_STATE1_SATURATION = 0xC10E;
        /// <summary>
        ///  0 = Normal
        ///  1 = High
        ///  65535 = Low
        /// </summary>
        public const int TAG_CANON_STATE1_SHARPNESS = 0xC10F;
        /// <summary>
        ///  0 = Check ISOSpeedRatings EXIF tag for ISO Speed
        ///  15 = Auto ISO
        ///  16 = ISO 50
        ///  17 = ISO 100
        ///  18 = ISO 200
        ///  19 = ISO 400
        /// </summary>
        public const int TAG_CANON_STATE1_ISO = 0xC110;
        /// <summary>
        ///  3 = Evaluative
        ///  4 = Partial
        ///  5 = Center Weighted
        /// </summary>
        public const int TAG_CANON_STATE1_METERING_MODE = 0xC111;
        /// <summary>
        ///  0 = Manual
        ///  1 = Auto
        ///  3 = Close-up (Macro)
        ///  8 = Locked (Pan Mode)
        /// </summary>
        public const int TAG_CANON_STATE1_FOCUS_TYPE = 0xC112;
        /// <summary>
        ///  12288 = None (Manual Focus)
        ///  12289 = Auto Selected
        ///  12290 = Right
        ///  12291 = Center
        ///  12292 = Left
        /// </summary>
        public const int TAG_CANON_STATE1_AF_POINT_SELECTED = 0xC113;
        /// <summary>
        ///  0 = Easy Shooting (See Easy Shooting Mode)
        ///  1 = Program
        ///  2 = Tv-Priority
        ///  3 = Av-Priority
        ///  4 = Manual
        ///  5 = A-DEP
        /// </summary>
        public const int TAG_CANON_STATE1_EXPOSURE_MODE = 0xC114;
        public const int TAG_CANON_STATE1_UNKNOWN_7 = 0xC115;
        /// <summary>
        /// xb: 15.05.2008 -- http://owl.phy.queensu.ca/~phil/exiftool/TagNames/Canon.html#CameraSettings
        ///     Old State TAG_CANON_STATE1_UNKNOWN_8
        /// Canon LensType Values
        /// 1 = Canon EF 50mm f/1.8 
        /// 2 = Canon EF 28mm f/2.8 
        /// 4 = Canon EF 35-105mm f/3.5-4.5 or Sigma UC Zoom 35-135mm f/4-5.6 
        /// 6 = Tokina AF193-2 19-35mm f/3.5-4.5 or Sigma Lens 
        /// 7 = Canon EF 100-300mm f/5.6L 
        ///  ......
        /// </summary>
        public const int TAG_CANON_STATE1_LensType = 0xC116;
        public const int TAG_CANON_STATE1_LONG_FOCAL_LENGTH = 0xC117;
        public const int TAG_CANON_STATE1_SHORT_FOCAL_LENGTH = 0xC118;
        public const int TAG_CANON_STATE1_FOCAL_UNITS_PER_MM = 0xC119;
        /// <summary>
        /// xb: 15.05.2008 -- http://owl.phy.queensu.ca/~phil/exiftool/TagNames/Canon.html#CameraSettings
        ///     Old State TAG_CANON_STATE1_UNKNOWN_10
        /// </summary>
        public const int TAG_CANON_STATE1_MaxAperture = 0xC11A;
        /// <summary>
        /// xb: 15.05.2008 -- http://owl.phy.queensu.ca/~phil/exiftool/TagNames/Canon.html#CameraSettings
        ///     Old State TAG_CANON_STATE1_UNKNOWN_10
        /// </summary>
        public const int TAG_CANON_STATE1_MinAperture = 0xC11B;
        /// <summary>
        ///  0 = Flash Did Not Fire
        ///  1 = Flash Fired
        /// </summary>
        public const int TAG_CANON_STATE1_FLASH_ACTIVITY = 0xC11C;
        public const int TAG_CANON_STATE1_FLASH_DETAILS = 0xC11D;
        public const int TAG_CANON_STATE1_UNKNOWN_12 = 0xC11E;
        public const int TAG_CANON_STATE1_UNKNOWN_13 = 0xC11F;
        /// <summary>
        ///  0 = Focus Mode: Single
        ///  1 = Focus Mode: Continuous
        /// </summary>
        public const int TAG_CANON_STATE1_FOCUS_MODE_2 = 0xC120;

        /// <summary>
        /// xb: 15.05.2008 -- http://owl.phy.queensu.ca/~phil/exiftool/TagNames/Canon.html#CameraSettings
        ///     New constante
        /// </summary>
        public const int TAG_CANON_STATE1_AESetting = 0xC121;
        /// <summary>
        /// xb: 15.05.2008 -- http://owl.phy.queensu.ca/~phil/exiftool/TagNames/Canon.html#CameraSettings
        ///     New constante
        /// </summary>
        public const int TAG_CANON_STATE1_ImageStabilization = 0xC122;
        /// <summary>
        /// xb: 15.05.2008 -- http://owl.phy.queensu.ca/~phil/exiftool/TagNames/Canon.html#CameraSettings
        ///     New constante
        /// </summary>
        public const int TAG_CANON_STATE1_DisplayAperture = 0xC123;
        /// <summary>
        /// xb: 15.05.2008 -- http://owl.phy.queensu.ca/~phil/exiftool/TagNames/Canon.html#CameraSettings
        ///     New constante
        /// </summary>
        public const int TAG_CANON_STATE1_ZoomSourceWidth = 0xC124;
        /// <summary>
        /// xb: 15.05.2008 -- http://owl.phy.queensu.ca/~phil/exiftool/TagNames/Canon.html#CameraSettings
        ///     New constante
        /// </summary>
        public const int TAG_CANON_STATE1_ZoomTargetWidth = 0xC125;
  
        /// <summary>
        /// xb: 15.05.2008 -- http://owl.phy.queensu.ca/~phil/exiftool/TagNames/Canon.html#CameraSettings
        ///     New constante
        /// </summary>
        public const int TAG_CANON_STATE1_SpotMeteringMode = 0xC127;
        /// <summary>
        /// xb: 15.05.2008 -- http://owl.phy.queensu.ca/~phil/exiftool/TagNames/Canon.html#CameraSettings
        ///     New constante
        /// </summary>
        public const int TAG_CANON_STATE1_PhotoEffect = 0xC128;
        /// <summary>
        /// xb: 15.05.2008 -- http://owl.phy.queensu.ca/~phil/exiftool/TagNames/Canon.html#CameraSettings
        ///     New constante
        /// </summary>
        public const int TAG_CANON_STATE1_ManualFlashOutput = 0xC129;
        /// <summary>
        /// xb: 15.05.2008 -- http://owl.phy.queensu.ca/~phil/exiftool/TagNames/Canon.html#CameraSettings
        ///     New constante
        /// </summary>
        public const int TAG_CANON_STATE1_ColorTone = 0xC12A;

        /// <summary>
        /// xb: 15.05.2008 -- http://owl.phy.queensu.ca/~phil/exiftool/TagNames/Canon.html#CameraSettings
        ///     New Tags
        /// </summary>
        public const int TAG_CANON_FocalLength_FocalType = 0xC401;
        public const int TAG_CANON_FocalLength_FocalLength = 0xC402;
        public const int TAG_CANON_FocalLength_FocalPlaneXSize = 0xC403;
        public const int TAG_CANON_FocalLength_FocalPlaneYSize = 0xC404;
        /// =============================================================================================

        /// <summary>
        ///  0 = Auto
        ///  1 = Sunny
        ///  2 = Cloudy
        ///  3 = Tungsten
        ///  4 = Fluorescent
        ///  5 = Flash
        ///  6 = Custom
        /// </summary>
        public const int TAG_CANON_STATE2_WHITE_BALANCE = 0xC207;
        public const int TAG_CANON_STATE2_SEQUENCE_NUMBER = 0xC209;
        public const int TAG_CANON_STATE2_AF_POINT_USED = 0xC20E;
        /// <summary>
        ///  The value of this tag may be translated into a flash bias value, in EV.
        /// 
        ///  0xffc0 = -2 EV
        ///  0xffcc = -1.67 EV
        ///  0xffd0 = -1.5 EV
        ///  0xffd4 = -1.33 EV
        ///  0xffe0 = -1 EV
        ///  0xffec = -0.67 EV
        ///  0xfff0 = -0.5 EV
        ///  0xfff4 = -0.33 EV
        ///  0x0000 = 0 EV
        ///  0x000c = 0.33 EV
        ///  0x0010 = 0.5 EV
        ///  0x0014 = 0.67 EV
        ///  0x0020 = 1 EV
        ///  0x002c = 1.33 EV
        ///  0x0030 = 1.5 EV
        ///  0x0034 = 1.67 EV
        ///  0x0040 = 2 EV 
        /// </summary>
        public const int TAG_CANON_STATE2_FLASH_BIAS = 0xC20F;
        public const int TAG_CANON_STATE2_AUTO_EXPOSURE_BRACKETING = 0xC210;
        public const int TAG_CANON_STATE2_AEB_BRACKET_VALUE = 0xC211;
        public const int TAG_CANON_STATE2_SUBJECT_DISTANCE = 0xC213;

        /// <summary>
        ///  Long Exposure Noise Reduction
        ///  0 = Off
        ///  1 = On
        /// </summary>
        public const int TAG_CANON_CUSTOM_FUNCTION_LONG_EXPOSURE_NOISE_REDUCTION = 0xC301;

        /// <summary>
        ///  Shutter/Auto Exposure-lock buttons
        ///  0 = AF/AE lock
        ///  1 = AE lock/AF
        ///  2 = AF/AF lock
        ///  3 = AE+release/AE+AF
        /// </summary>
        public const int TAG_CANON_CUSTOM_FUNCTION_SHUTTER_AUTO_EXPOSURE_LOCK_BUTTONS = 0xC302;

        /// <summary>
        ///  Mirror lockup
        ///  0 = Disable
        ///  1 = Enable
        /// </summary>
        public const int TAG_CANON_CUSTOM_FUNCTION_MIRROR_LOCKUP = 0xC303;

        /// <summary>
        ///  Tv/Av and exposure level
        ///  0 = 1/2 stop
        ///  1 = 1/3 stop
        /// </summary>
        public const int TAG_CANON_CUSTOM_FUNCTION_TV_AV_AND_EXPOSURE_LEVEL = 0xC304;

        /// <summary>
        ///  AF-assist light
        ///  0 = On (Auto)
        ///  1 = Off
        /// </summary>
        public const int TAG_CANON_CUSTOM_FUNCTION_AF_ASSIST_LIGHT = 0xC305;

        /// <summary>
        ///  Shutter speed in Av mode
        ///  0 = Automatic
        ///  1 = 1/200 (fixed)
        /// </summary>
        public const int TAG_CANON_CUSTOM_FUNCTION_SHUTTER_SPEED_IN_AV_MODE = 0xC306;

        /// <summary>
        ///  Auto-Exposure Bracketting sequence/auto cancellation
        ///  0 = 0,-,+ / Enabled
        ///  1 = 0,-,+ / Disabled
        ///  2 = -,0,+ / Enabled
        ///  3 = -,0,+ / Disabled
        /// </summary>
        public const int TAG_CANON_CUSTOM_FUNCTION_BRACKETTING = 0xC307;

        /// <summary>
        ///  Shutter Curtain Sync
        ///  0 = 1st Curtain Sync
        ///  1 = 2nd Curtain Sync
        /// </summary>
        public const int TAG_CANON_CUSTOM_FUNCTION_SHUTTER_CURTAIN_SYNC = 0xC308;

        /// <summary>
        ///  Lens Auto-Focus stop button Function Switch
        ///  0 = AF stop
        ///  1 = Operate AF
        ///  2 = Lock AE and start timer
        /// </summary>
        public const int TAG_CANON_CUSTOM_FUNCTION_AF_STOP = 0xC309;

        /// <summary>
        ///  Auto reduction of fill flash
        ///  0 = Enable
        ///  1 = Disable
        /// </summary>
        public const int TAG_CANON_CUSTOM_FUNCTION_FILL_FLASH_REDUCTION = 0xC30A;

        /// <summary>
        ///  Menu button return position
        ///  0 = Top
        ///  1 = Previous (volatile)
        ///  2 = Previous
        /// </summary>
        public const int TAG_CANON_CUSTOM_FUNCTION_MENU_BUTTON_RETURN = 0xC30B;

        /// <summary>
        ///  SET button function when shooting
        ///  0 = Not Assigned
        ///  1 = Change Quality
        ///  2 = Change ISO Speed
        ///  3 = Select Parameters
        /// </summary>
        public const int TAG_CANON_CUSTOM_FUNCTION_SET_BUTTON_FUNCTION = 0xC30C;

        /// <summary>
        ///  Sensor cleaning
        ///  0 = Disable
        ///  1 = Enable
        /// </summary>
        public const int TAG_CANON_CUSTOM_FUNCTION_SENSOR_CLEANING = 0xC30D;

        /// <summary>
        /// Constructor of the object.
        /// </summary>
        public CanonDirectory()
            : base("CanonMarkernote")
        {
            base.SetDescriptor(new CanonDescriptor(this));
        }

        // ==============================================================================================
        // xb: 15.05.2008 -- http://owl.phy.queensu.ca/~phil/exiftool/TagNames/Canon.html#CameraSettings
        //     New Tags
        // ==============================================================================================
 
        /// <summary>
        /// --> Canon FocalLength Tags
        /// </summary>
        public const int TAG_CANON_FocalLength = 0x0002;
 
        /// <summary>
        /// --> Canon CanonModelID Values
        /// </summary>
        public const int TAG_CANON_CanonModelID = 0x0010;
        /// <summary>
        /// --> Canon AFInfo Tags
        /// </summary>
        public const int TAG_CANON_CanonAFInfo = 0x0012;    
        // public const int TAG_CANON_ThumbnailImageValidArea = 0x0013;  // int16u[4] (all zeros for full frame) 
        /// <summary>
        /// 0x90000000 = Format 1 
        /// 0xa0000000 = Format 2 
        /// </summary>
        public const int TAG_CANON_SerialNumberFormat = 0x0015;
        /// <summary>
        /// N 0 = Off 
        /// 1 = On (1) 
        /// 2 = On (2)
        /// </summary>
        public const int TAG_CANON_SuperMacro = 0x001a;
        /// <summary>
        /// (only used in postcard mode) 
        /// 0 = Off
        /// 1 = Date 
        /// 2 = Date & Time
        /// </summary>
        public const int TAG_CANON_DateStampMode = 0x001c;
        /// <summary>
        /// --> Canon MyColors Tags
        /// </summary>
        public const int TAG_CANON_MyColors = 0x001d;
        public const int TAG_CANON_FirmwareRevision = 0x001e;
        /// <summary>
        /// --> Canon FaceDetect1 Tags 
        /// </summary>
        public const int TAG_CANON_FaceDetect1 = 0x0024;
        /// <summary>
        /// --> Canon FaceDetect2 Tags
        /// </summary>
        public const int TAG_CANON_FaceDetect2 = 0x0025;
        /// <summary>
        /// --> Canon AFInfo2 Tags
        /// </summary>
        public const int TAG_CANON_CanonAFInfo2 = 0x0026;
        public const int TAG_CANON_RawDataOffset = 0x0081;
        public const int TAG_CANON_OriginalDecisionDataOffset = 0x0083;
        /// <summary>
        /// --> CanonCustom Functions1D Tags
        /// </summary>
        public const int TAG_CANON_CustomFunctions1D = 0x0090;
        /// <summary>
        /// --> CanonCustom PersonalFuncs Tags
        /// </summary>
        public const int TAG_CANON_PersonalFunctions = 0x0091;
        /// <summary>
        /// --> CanonCustom PersonalFuncValues Tags
        /// </summary>
        public const int TAG_CANON_PersonalFunctionValues = 0x0092;
        /// <summary>
        /// --> Canon FileInfo Tags
        /// </summary>
        public const int TAG_CANON_CanonFileInfo = 0x0093;
        /// <summary>
        /// (EOS 1D -- 5 rows: A1-7, B1-10, C1-11, D1-10, E1-7, center point is C6) 
        /// </summary>
        public const int TAG_CANON_AFPointsInFocus1D = 0x0094;
        public const int TAG_CANON_LensType = 0x0095;
        /// <summary>
        /// --> Canon SerialInfo Tags
        /// </summary>
        public const int TAG_CANON_InternalSerialNumber = 0x0096;
        public const int TAG_CANON_DustRemovalData = 0x0097;
        /// <summary>
        /// --> CanonCustom Functions2 Tags
        /// </summary>
        public const int TAG_CANON_CustomFunctions2 = 0x0099;
        /// <summary>
        /// --> Canon Processing Tags
        /// </summary>
        public const int TAG_CANON_ProcessingInfo = 0x00a0;
        public const int TAG_CANON_ToneCurveTable = 0x00a1;
        public const int TAG_CANON_SharpnessTable = 0x00a2;
        public const int TAG_CANON_SharpnessFreqTable = 0x00a3;
        public const int TAG_CANON_WhiteBalanceTable = 0x00a4;
        /// <summary>
        /// --> Canon ColorBalance Tags 
        /// </summary>
        public const int TAG_CANON_ColorBalance = 0x00a9;
        public const int TAG_CANON_ColorTemperature = 0x00ae;
        /// <summary>
        /// --> Canon Flags Tags
        /// </summary>
        public const int TAG_CANON_CanonFlags = 0x00b0;
        /// <summary>
        /// --> Canon ModifiedInfo Tags
        /// </summary>
        public const int TAG_CANON_ModifiedInfo = 0x00b1;
        public const int TAG_CANON_ToneCurveMatching = 0x00b2;
        public const int TAG_CANON_WhiteBalanceMatching = 0x00b3;
        /// <summary>
        /// 1 = sRGB 
        /// 2 = Adobe RGB 
        /// </summary>
        public const int TAG_CANON_ColorSpace = 0x00b4;
        /// <summary>
        /// --> Canon PreviewImageInfo Tags
        /// </summary>
        public const int TAG_CANON_PreviewImageInfo = 0x00b6;
        /// <summary>
        /// (offset of VRD "recipe data" if it exists)
        /// </summary>
        public const int TAG_CANON_VRDOffset = 0x00d0;
        /// <summary>
        /// --> Canon SensorInfo Tags 
        /// </summary>
        public const int TAG_CANON_SensorInfo = 0x00e0;
        /// <summary>
        /// --> Canon ColorBalance1 Tags
        /// --> Canon ColorBalance2 Tags 
        /// --> Canon ColorBalance3 Tags 
        /// --> Canon ColorBalance4 Tags 
        /// </summary>
        public const int TAG_CANON_ColorBalance1to4 = 0x4001;
        public const int TAG_CANON_UnknownBlock1 = 0x4002;
        /// <summary>
        /// --> Canon ColorInfo Tags 
        /// </summary>
        public const int TAG_CANON_ColorInfo = 0x4003;
        public const int TAG_CANON_UnknownBlock2 = 0x4005;
        public const int TAG_CANON_BlackLevel = 0x4008;
  
        public const int TAG_CANON_STATE2_AutoISO = 0xC201;
        public const int TAG_CANON_STATE2_BaseISO = 0xC202;
        public const int TAG_CANON_STATE2_MeasuredEV = 0xC203;
        public const int TAG_CANON_STATE2_TargetAperture = 0xC204;
        public const int TAG_CANON_STATE2_TargetExposureTime = 0xC205;
        public const int TAG_CANON_STATE2_ExposureCompensation = 0xC206;
        /// <summary>
        /// 0 = Off 
        /// 1 = Night Scene 
        /// 2 = On 
        /// 3 = None
        /// </summary>
        public const int TAG_CANON_STATE2_SlowShutter = 0xC208;
        public const int TAG_CANON_STATE2_OpticalZoomCode = 0xC20A;
        public const int TAG_CANON_STATE2_FlashGuideNumber = 0xC212;
        public const int TAG_CANON_STATE2_ControlMode = 0xC20D;
        public const int TAG_CANON_STATE2_FocusDistanceLower = 0xC214;
        public const int TAG_CANON_STATE2_FNumber = 0xC215;
        public const int TAG_CANON_STATE2_ExposureTime = 0xC216;
        public const int TAG_CANON_STATE2_BulbDuration = 0xC218;
        public const int TAG_CANON_STATE2_CameraType = 0xC21A;
        public const int TAG_CANON_STATE2_AutoRotate = 0xC21B;
        public const int TAG_CANON_STATE2_NDFilter = 0xC21C;
        public const int TAG_CANON_STATE2_SelfTimer2 = 0xC21D;
        public const int TAG_CANON_STATE2_FlashOutput = 0xC221;
 
        
        /// <summary>
        /// 0 = Standard 
        /// 1 = Manual 
        /// 2 = Custom 
        /// </summary>
        public const int TAG_CANON_ProcessingInfo_ToneCurve = 0xC501;
        /// <summary>
        /// (1D and 5D only)
        /// </summary>
        public const int TAG_CANON_ProcessingInfo_Sharpness = 0xC502;
        /// <summary>
        /// 0 = n/a 
        /// 1 = Lowest 
        /// 2 = Low 
        /// 3 = Standard 
        /// 4 = High 
        /// 5 = Highest 
        /// </summary>
        public const int TAG_CANON_ProcessingInfo_SharpnessFrequency = 0xC503;
        public const int TAG_CANON_ProcessingInfo_SensorRedLevel = 0xC504;
        public const int TAG_CANON_ProcessingInfo_SensorBlueLevel = 0xC505;
        public const int TAG_CANON_ProcessingInfo_WhiteBalanceRed = 0xC506;
        public const int TAG_CANON_ProcessingInfo_WhiteBalanceBlue = 0xC507;
        /// <summary>
        /// --> Canon WhiteBalance Values 
        /// </summary>
        public const int TAG_CANON_ProcessingInfo_WhiteBalance = 0xC508;
        public const int TAG_CANON_ProcessingInfo_ColorTemperature = 0xC509;
        /// <summary>
        /// --> Canon PictureStyle Values 
        /// </summary>
        public const int TAG_CANON_ProcessingInfo_PictureStyle = 0xC50a;
        public const int TAG_CANON_ProcessingInfo_DigitalGain = 0xC50b;
        /// <summary>
        /// (positive is a shift toward amber) 
        /// </summary>
        public const int TAG_CANON_ProcessingInfo_WBShiftAB = 0xC50c;
        /// <summary>
        /// (positive is a shift toward green) 
        /// </summary>
        public const int TAG_CANON_ProcessingInfo_WBShiftGM = 0xC50d;
 
        public const int TAG_CANON_SensorInfo_SensorWidth = 0xC601;
        public const int TAG_CANON_SensorInfo_SensorHeight = 0xC602;
        public const int TAG_CANON_SensorInfo_SensorLeftBorder = 0xC605;
        public const int TAG_CANON_SensorInfo_SensorTopBorder = 0xC606;
        public const int TAG_CANON_SensorInfo_SensorRightBorder = 0xC607;
        public const int TAG_CANON_SensorInfo_SensorBottomBorder = 0xC608;
        public const int TAG_CANON_SensorInfo_BlackMaskLeftBorder = 0xC609;
        public const int TAG_CANON_SensorInfo_BlackMaskTopBorder = 0xC60a;
        public const int TAG_CANON_SensorInfo_BlackMaskRightBorder = 0xC60b;
        public const int TAG_CANON_SensorInfo_BlackMaskBottomBorder = 0xC60c;
 
        // xb: 15.05.2008
        // =============================================================================================

        /// <summary>
        /// We need special handling for selected tags.
        /// </summary>
        /// <param name="aTagType">the tag type</param>
        /// <param name="someInts">what to set</param>
        public override void SetIntArray(int tagType, int[] ints)
        {
            if (tagType == TAG_CANON_CAMERA_STATE_1)
            {
                // this single tag has multiple values within
                int subTagTypeBase = 0xC100;
                // we intentionally skip the first array member
                for (int i = 1; i < ints.Length; i++)
                {
                    base.SetObject(subTagTypeBase + i, ints[i]);
                }
            }
            else if (tagType == TAG_CANON_CAMERA_STATE_2)
            {
                // this single tag has multiple values within
                int subTagTypeBase = 0xC200;
                // we intentionally skip the first array member
                for (int i = 1; i < ints.Length; i++)
                {
                    base.SetObject(subTagTypeBase + i, ints[i]);
                }
            }
            
            /// xb: 15.05.2008 -- http://owl.phy.queensu.ca/~phil/exiftool/TagNames/Canon.html#CameraSettings
            else if (tagType == TAG_CANON_FocalLength)
            {
              // this single tag has multiple values within
              int subTagTypeBase = 0xC400;
              // we intentionally skip the first array member
              for (int i = 0; i < ints.Length; i++)
              {
                base.SetObject(subTagTypeBase + i+1, ints[i]);
              }
            }
            else if (tagType == TAG_CANON_ProcessingInfo)
            {
              // this single tag has multiple values within
              int subTagTypeBase = 0xC500;
              // we intentionally skip the first array member
              for (int i = 1; i < ints.Length; i++)
              {
                base.SetObject(subTagTypeBase + i, ints[i]);
              }
            }
            else if (tagType == TAG_CANON_SensorInfo)
            {
              // this single tag has multiple values within
              int subTagTypeBase = 0xC600;
              // we intentionally skip the first array member
              for (int i = 1; i < ints.Length; i++)
              {
                base.SetObject(subTagTypeBase + i, ints[i]);
              }
            }      
            /// xb: 15.05.2008
                  
            if (tagType == TAG_CANON_CUSTOM_FUNCTIONS)
            {
                // this single tag has multiple values within
                int subTagTypeBase = 0xC300;
                // we intentionally skip the first array member
                for (int i = 1; i < ints.Length; i++)
                {
                    base.SetObject(subTagTypeBase + i + 1, ints[i] & 0x0F);
                }
            }
            else
            {
                // no special handling...
                base.SetIntArray(tagType, ints);
            }
        }
    }
}