using System;
using System.IO;

namespace com.drew.metadata.exif
{
    /// <summary>
    /// The Exif Directory class
    /// </summary>

    public class ExifDirectory : AbstractDirectory
    {
        // TODO do these tags belong in the exif directory?
        public const int TAG_SUB_IFDS = 0x014A;
        public const int TAG_GPS_INFO = 0x8825;

        /// <summary>
        /// The actual aperture value of lens when the image was taken. Unit is APEX.
        /// To convert this value to ordinary F-number (F-stop), calculate this value'str power
        /// of root 2 (=1.4142). For example, if the ApertureValue is '5', F-number is 1.4142^5 = F5.6.
        /// </summary>
        public const int TAG_APERTURE = 0x9202;

        /// <summary>
        /// When image format is no compression, this value shows the number of bits
        /// per component for each pixel. Usually this value is '8,8,8'.
        /// </summary>
        public const int TAG_BITS_PER_SAMPLE = 0x0102;

        /// <summary>
        /// Shows compression method for Thumbnail.
        ///      1 = Uncompressed
        ///      2 = CCITT 1D
        ///      3 = T4/Group 3 Fax
        ///      4 = T6/Group 4 Fax
        ///      5 = LZW
        ///      6 = JPEG (old-style)
        ///      7 = JPEG
        ///      8 = Adobe Deflate
        ///      9 = JBIG B&W
        ///      10 = JBIG Color
        ///      32766 = Next
        ///      32771 = CCIRLEW
        ///      32773 = PackBits
        ///      32809 = Thunderscan
        ///      32895 = IT8CTPAD
        ///      32896 = IT8LW
        ///      32897 = IT8MP
        ///      32898 = IT8BL
        ///      32908 = PixarFilm
        ///      32909 = PixarLog
        ///      32946 = Deflate
        ///      32947 = DCS
        ///      34661 = JBIG
        ///      34676 = SGILog
        ///      34677 = SGILog24
        ///      34712 = JPEG 2000
        ///      34713 = Nikon NEF Compressed
        /// </summary>
        public const int TAG_COMPRESSION = 0x0103;
        public const int COMPRESSION_NONE = 1;
        public const int COMPRESSION_JPEG = 6;


        /// <summary>
        /// Shows the color space of the image data components.
        /// 0 = WhiteIsZero
        /// 1 = BlackIsZero
        /// 2 = RGB
        /// 3 = RGB Palette
        /// 4 = Transparency Mask
        /// 5 = CMYK
        /// 6 = YCbCr
        /// 8 = CIELab
        /// 9 = ICCLab
        /// 10 = ITULab
        /// 32803 = Color Filter Array
        /// 32844 = Pixar LogL
        /// 32845 = Pixar LogLuv
        /// 34892 = Linear Raw
        /// </summary>
        public const int TAG_PHOTOMETRIC_INTERPRETATION = 0x0106;

        /// <summary>
        /// 1 = No dithering or halftoning
        /// 2 = Ordered dither or halftone
        /// 3 = Randomized dither
        /// </summary>
        public const int TAG_THRESHOLDING = 0x0107;
        public const int PHOTOMETRIC_INTERPRETATION_MONOCHROME = 1;
        public const int PHOTOMETRIC_INTERPRETATION_RGB = 2;
        public const int PHOTOMETRIC_INTERPRETATION_YCBCR = 6;

        /// <summary>
        /// The position in the file of raster data.
        /// </summary>
        public const int TAG_STRIP_OFFSETS = 0x0111;
        /// <summary>
        /// Each pixel is composed of this many samples.
        /// </summary>
        public const int TAG_SAMPLES_PER_PIXEL = 0x0115;
        /// <summary>
        /// The raster is codified by a single block of data holding this many rows.
        /// </summary>
        public const int TAG_ROWS_PER_STRIP = 0x116;
        /// <summary>
        /// The size of the raster data in bytes.
        /// </summary>
        public const int TAG_STRIP_BYTE_COUNTS = 0x0117;
        public const int TAG_MIN_SAMPLE_VALUE = 0x0118;
        public const int TAG_MAX_SAMPLE_VALUE = 0x0119;


        /// <summary>
        /// When image format is no compression YCbCr, this value shows byte aligns of YCbCr data.
        /// If value is '1', Y/Cb/Cr value is chunky format, contiguous for each subsampling pixel.
        /// If value is '2', Y/Cb/Cr value is separated and stored to Y plane/Cb plane/Cr plane format.
        /// </summary>
        public const int TAG_PLANAR_CONFIGURATION = 0x011C;
        public const int TAG_YCBCR_SUBSAMPLING = 0x0212;
        public const int TAG_IMAGE_DESCRIPTION = 0x010E;
        public const int TAG_SOFTWARE = 0x0131;
        public const int TAG_DATETIME = 0x0132;
        public const int TAG_WHITE_POINT = 0x013E;
        public const int TAG_PRIMARY_CHROMATICITIES = 0x013F;
        public const int TAG_YCBCR_COEFFICIENTS = 0x0211;
        public const int TAG_REFERENCE_BLACK_WHITE = 0x0214;
        public const int TAG_COPYRIGHT = 0x8298;
        /// <summary>
        /// The new subfile type tag.
        /// 0 = Full-resolution Image
        /// 1 = Reduced-resolution image
        /// 2 = Single page of multi-page image
        /// 3 = Single page of multi-page reduced-resolution image
        /// 4 = Transparency mask
        /// 5 = Transparency mask of reduced-resolution image
        /// 6 = Transparency mask of multi-page image
        /// 7 = Transparency mask of reduced-resolution multi-page image
        /// </summary>
        public const int TAG_NEW_SUBFILE_TYPE = 0x00FE;
        /// <summary>
        /// The old subfile type tag.
        /// 1 = Full-resolution image (Main image)
        /// 2 = Reduced-resolution image (Thumbnail)
        /// 3 = Single page of multi-page image
        /// </summary>
        public const int TAG_SUBFILE_TYPE = 0x00FF;
        public const int TAG_TRANSFER_FUNCTION = 0x012D;
        public const int TAG_ARTIST = 0x013B;
        public const int TAG_PREDICTOR = 0x013D;
        public const int TAG_TILE_WIDTH = 0x0142;
        public const int TAG_TILE_LENGTH = 0x0143;
        public const int TAG_TILE_OFFSETS = 0x0144;
        public const int TAG_TILE_BYTE_COUNTS = 0x0145;
        public const int TAG_JPEG_TABLES = 0x015B;
        public const int TAG_CFA_REPEAT_PATTERN_DIM = 0x828D;

        /// <summary>
        /// There are two definitions for CFA pattern, I don't know the difference...
        /// </summary>
        public const int TAG_CFA_PATTERN_2 = 0x828E;
        public const int TAG_BATTERY_LEVEL = 0x828F;
        public const int TAG_IPTC_NAA = 0x83BB;
        public const int TAG_INTER_COLOR_PROFILE = 0x8773;
        public const int TAG_SPECTRAL_SENSITIVITY = 0x8824;
        public const int TAG_OECF = 0x8828;
        public const int TAG_INTERLACE = 0x8829;
        public const int TAG_TIME_ZONE_OFFSET = 0x882A;
        public const int TAG_SELF_TIMER_MODE = 0x882B;
        public const int TAG_FLASH_ENERGY = 0x920B;
        public const int TAG_SPATIAL_FREQ_RESPONSE = 0x920C;
        public const int TAG_NOISE = 0x920D;
        public const int TAG_IMAGE_NUMBER = 0x9211;
        public const int TAG_SECURITY_CLASSIFICATION = 0x9212;
        public const int TAG_IMAGE_HISTORY = 0x9213;
        public const int TAG_SUBJECT_LOCATION = 0x9214;

        /// <summary>
        /// There are two definitions for exposure index, I don't know the difference...
        /// </summary>
        public const int TAG_EXPOSURE_INDEX_2 = 0x9215;
        public const int TAG_TIFF_EP_STANDARD_ID = 0x9216;
        public const int TAG_FLASH_ENERGY_2 = 0xA20B;
        public const int TAG_SPATIAL_FREQ_RESPONSE_2 = 0xA20C;
        public const int TAG_SUBJECT_LOCATION_2 = 0xA214;
        public const int TAG_MAKE = 0x010F;
        public const int TAG_MODEL = 0x0110;
        public const int TAG_ORIENTATION = 0x0112;
        public const int TAG_X_RESOLUTION = 0x011A;
        public const int TAG_Y_RESOLUTION = 0x011B;
        public const int TAG_PAGE_NAME = 0x011D;
        public const int TAG_RESOLUTION_UNIT = 0x0128;
        public const int TAG_THUMBNAIL_OFFSET = 0x0201;
        public const int TAG_THUMBNAIL_LENGTH = 0x0202;
        public const int TAG_YCBCR_POSITIONING = 0x0213;

        /// <summary>
        /// Exposure time (reciprocal of shutter speed). Unit is second.
        /// </summary>
        public const int TAG_EXPOSURE_TIME = 0x829A;

        /// <summary>
        /// The actual F-number(F-stop) of lens when the image was taken.
        /// </summary>
        public const int TAG_FNUMBER = 0x829D;

        /// <summary>
        /// Exposure program that the camera used when image was taken.
        /// '1' means manual control, '2' program normal, '3' aperture priority, '4'
        /// shutter priority, '5' program creative (slow program),
        /// '6' program action (high-speed program), '7' portrait mode, '8' landscape mode.
        /// </summary>
        public const int TAG_EXPOSURE_PROGRAM = 0x8822;
        public const int TAG_ISO_EQUIVALENT = 0x8827;
        public const int TAG_EXIF_VERSION = 0x9000;
        public const int TAG_DATETIME_ORIGINAL = 0x9003;
        public const int TAG_DATETIME_DIGITIZED = 0x9004;
        public const int TAG_COMPONENTS_CONFIGURATION = 0x9101;

        /// <summary>
        /// Average (rough estimate) compression level in JPEG bits per pixel.
        /// </summary>
        public const int TAG_COMPRESSION_LEVEL = 0x9102;

        /// <summary>
        /// Shutter speed by APEX value. To convert this value to ordinary 'Shutter Speed';
        /// calculate this value'str power of 2, then reciprocal. For example, if the
        /// ShutterSpeedValue is '4', shutter speed is 1/(24)=1/16 second.
        /// </summary>
        public const int TAG_SHUTTER_SPEED = 0x9201;
        public const int TAG_BRIGHTNESS_VALUE = 0x9203;
        public const int TAG_EXPOSURE_BIAS = 0x9204;

        /// <summary>
        /// Maximum aperture value of lens. You can convert to F-number by calculating
        /// power of root 2 (same process of ApertureValue:0x9202).
        /// The actual aperture value of lens when the image was taken. To convert this
        /// value to ordinary f-number(f-stop), calculate the value'lcStr power of root 2
        /// (=1.4142). For example, if the ApertureValue is '5', f-number is 1.41425^5 = F5.6.
        /// </summary>
        public const int TAG_MAX_APERTURE = 0x9205;
        /// <summary>
        /// Indicates the distance the autofocus camera is focused to.  Tends to be less accurate as distance increases.
        /// </summary>
        public const int TAG_SUBJECT_DISTANCE = 0x9206;

        /// <summary>
        /// Exposure metering method. '0' means unknown, '1' average, '2' center
        /// weighted average, '3' spot, '4' multi-spot, '5' multi-segment, '6' partial, '255' other.
        /// </summary>
        public const int TAG_METERING_MODE = 0x9207;

        /// <summary>
        /// White balance (aka light source). '0' means unknown, '1' daylight,
        /// '2' fluorescent, '3' tungsten, '10' flash, '17' standard light A,
        /// '18' standard light B, '19' standard light C, '20' D55, '21' D65,
        /// '22' D75, '255' other.
        /// </summary>
        public const int TAG_LIGHT_SOURCE = 0x9208;

        /// <summary>
        /// This tag indicates the white balance mode set when the image was shot.
        /// Tag = 41987 (A403.H)
        /// Type = SHORT
        /// Count = 1
        /// Default = none
        ///  0 = Auto white balance
        ///  1 = Manual white balance
        ///  Other = reserved
        /// </summary>
        public const int TAG_WHITE_BALANCE_MODE = 0xA403;


        /// <summary>
        /// 0x0  = 0000000 = No Flash
        /// 0x1  = 0000001 = Fired
        /// 0x5  = 0000101 = Fired, Return not detected
        /// 0x7  = 0000111 = Fired, Return detected
        /// 0x9  = 0001001 = On
        ///  0xd  = 0001101 = On, Return not detected
        ///  0xf  = 0001111 = On, Return detected
        ///  0x10 = 0010000 = Off
        ///  0x18 = 0011000 = Auto, Did not fire
        ///  0x19 = 0011001 = Auto, Fired
        ///  0x1d = 0011101 = Auto, Fired, Return not detected
        ///  0x1f = 0011111 = Auto, Fired, Return detected
        ///  0x20 = 0100000 = No flash function
        ///  0x41 = 1000001 = Fired, Red-eye reduction
        ///  0x45 = 1000101 = Fired, Red-eye reduction, Return not detected
        ///  0x47 = 1000111 = Fired, Red-eye reduction, Return detected
        ///  0x49 = 1001001 = On, Red-eye reduction
        ///  0x4d = 1001101 = On, Red-eye reduction, Return not detected
        ///  0x4f = 1001111 = On, Red-eye reduction, Return detected
        ///  0x59 = 1011001 = Auto, Fired, Red-eye reduction
        ///  0x5d = 1011101 = Auto, Fired, Red-eye reduction, Return not detected
        ///  0x5f = 1011111 = Auto, Fired, Red-eye reduction, Return detected
        ///         6543210 (positions)
        ///
        ///  This is a bitmask.
        ///  0 = flash fired
        ///  1 = return detected
        /// 2 = return able to be detected
        /// 3 = unknown
        /// 4 = auto used
        /// 5 = unknown
        /// 6 = red eye reduction used
        /// </summary>
        public const int TAG_FLASH = 0x9209;

        /// <summary>
        /// Focal length of lens used to take image. Unit is millimeter.
        /// </summary>
        public const int TAG_FOCAL_LENGTH = 0x920A;
        public const int TAG_USER_COMMENT = 0x9286;
        public const int TAG_SUBSECOND_TIME = 0x9290;
        public const int TAG_SUBSECOND_TIME_ORIGINAL = 0x9291;
        public const int TAG_SUBSECOND_TIME_DIGITIZED = 0x9292;
        public const int TAG_FLASHPIX_VERSION = 0xA000;

        /// <summary>
        /// Defines Color Space. DCF image must use sRGB color space so value is always '1'.
        /// If the picture uses the other color space, value is '65535':Uncalibrated.
        /// </summary>
        public const int TAG_COLOR_SPACE = 0xA001;
        public const int TAG_EXIF_IMAGE_WIDTH = 0xA002;
        public const int TAG_EXIF_IMAGE_HEIGHT = 0xA003;
        public const int TAG_RELATED_SOUND_FILE = 0xA004;
        public const int TAG_FOCAL_PLANE_X_RES = 0xA20E;
        public const int TAG_FOCAL_PLANE_Y_RES = 0xA20F;

        /// <summary>
        /// Unit of FocalPlaneXResoluton/FocalPlaneYResolution.
        /// '1' means no-unit, '2' inch, '3' centimeter.
        ///
        /// Note: Some of Fujifilm'str digicam(e.g.FX2700,FX2900,Finepix4700Z/40i etc)
        /// uses value '3' so it must be 'centimeter', but it seems that they use a '8.3mm?'
        /// (1/3in.?) to their ResolutionUnit. Fuji'str BUG? Finepix4900Z has been changed to
        /// use value '2' but it doesn't match to actual value also.
        /// </summary>
        public const int TAG_FOCAL_PLANE_UNIT = 0xA210;
        public const int TAG_EXPOSURE_INDEX = 0xA215;
        public const int TAG_SENSING_METHOD = 0xA217;
        public const int TAG_FILE_SOURCE = 0xA300;
        public const int TAG_SCENE_TYPE = 0xA301;
        public const int TAG_CFA_PATTERN = 0xA302;

        public const int TAG_THUMBNAIL_IMAGE_WIDTH = 0x0100;
        public const int TAG_THUMBNAIL_IMAGE_HEIGHT = 0x0101;
        public const int TAG_THUMBNAIL_DATA = 0xF001;

        // these tags new with Exif 2.2 (?) [A401 - A4
        /// <summary>
        ///This tag indicates the use of special processing on image data, such as rendering
        ///geared to output. When special processing is performed, the reader is expected to
        ///disable or minimize any further processing.
        ///Tag = 41985 (A401.H)
        ///Type = SHORT
        ///Count = 1
        ///Default = 0
        /// 0 = Normal process
        /// 1 = Custom process
        /// Other = reserved
        /// </summary>
        public const int TAG_CUSTOM_RENDERED = 0xA401;

        /// <summary>
        /// This tag indicates the exposure mode set when the image was shot. In auto-bracketing
        /// mode, the camera shoots a series of frames of the same scene at different exposure settings.
        /// Tag = 41986 (A402.H)
        /// Type = SHORT
        /// Count = 1
        /// Default = none
        ///   0 = Auto exposure
        ///   1 = Manual exposure
        ///   2 = Auto bracket
        ///   Other = reserved
        /// </summary>
        public const int TAG_EXPOSURE_MODE = 0xA402;

        /// <summary>
        ///  This tag indicates the digital zoom ratio when the image was shot. If the
        ///  numerator of the recorded value is 0, this indicates that digital zoom was
        ///  not used.
        ///  Tag = 41988 (A404.H)
        ///  Type = RATIONAL
        ///  Count = 1
        ///  Default = none
        /// </summary>
        public const int TAG_DIGITAL_ZOOM_RATIO = 0xA404;

        /// <summary>
        /// This tag indicates the type of scene that was shot. It can also be used to
        /// record the mode in which the image was shot. Note that this differs from
        /// the scene type (SceneType) tag.
        /// Tag = 41990 (A406.H)
        /// Type = SHORT
        /// Count = 1
        /// Default = 0
        ///   0 = Standard
        ///   1 = Landscape
        ///   2 = Portrait
        ///   3 = Night scene
        ///   Other = reserved
        /// </summary>
        public const int TAG_SCENE_CAPTURE_TYPE = 0xA406;

        /// <summary>
        /// This tag indicates the degree of overall image gain adjustment.
        /// Tag = 41991 (A407.H)
        /// Type = SHORT
        /// Count = 1
        /// Default = none
        ///   0 = None
        ///   1 = Low gain up
        ///   2 = High gain up
        ///   3 = Low gain down
        ///   4 = High gain down
        ///   Other = reserved
        /// </summary>
        public const int TAG_GAIN_CONTROL = 0xA407;

        /// <summary>
        /// This tag indicates the direction of contrast processing applied by the camera
        /// when the image was shot.
        /// Tag = 41992 (A408.H)
        /// Type = SHORT
        /// Count = 1
        /// Default = 0
        ///   0 = Normal
        /// 1 = Soft
        ///   2 = Hard
        ///   Other = reserved
        /// </summary>
        public const int TAG_CONTRAST = 0xA408;

        /// <summary>
        /// This tag indicates the direction of saturation processing applied by the camera
        /// when the image was shot.
        /// Tag = 41993 (A409.H)
        /// Type = SHORT
        /// Count = 1
        /// Default = 0
        ///   0 = Normal
        /// 1 = Low saturation
        ///   2 = High saturation
        ///   Other = reserved
        /// </summary>
        public const int TAG_SATURATION = 0xA409;

        /// <summary>
        /// This tag indicates the direction of sharpness processing applied by the camera
        /// when the image was shot.
        /// Tag = 41994 (A40A.H)
        /// Type = SHORT
        /// Count = 1
        /// Default = 0
        ///   0 = Normal
        ///   1 = Soft
        ///   2 = Hard
        ///   Other = reserved
        /// </summary>
        public const int TAG_SHARPNESS = 0xA40A;

        // TODO support this tag (I haven't seen a camera'lcStr actual implementation of this yet)

        /// <summary>
        ///This tag indicates information on the picture-taking conditions of a particular
        /// camera model. The tag is used only to indicate the picture-taking conditions in
        /// the reader.
        /// Tag = 41995 (A40B.H)
        /// Type = UNDEFINED
        /// Count = Any
        /// Default = none
        ///
        /// The information is recorded in the format shown below. The data is recorded
        /// in Unicode using SHORT type for the number of display rows and columns and
        /// UNDEFINED type for the camera settings. The Unicode (UCS-2) string including
        /// Signature is NULL terminated. The specifics of the Unicode string are as given
        /// in ISO/IEC 10464-1.
        ///
        ///      Length  Type        Meaning
        ///      ------+-----------+------------------
        ///      2       SHORT       Display columns
        ///      2       SHORT       Display rows
        ///      Any     UNDEFINED   Camera setting-1
        ///      Any     UNDEFINED   Camera setting-2
        ///      :       :           :
        ///      Any     UNDEFINED   Camera setting-n
        /// </summary>
        public const int TAG_DEVICE_SETTING_DESCRIPTION = 0xA40B;

        /// <summary>
        /// This tag indicates the distance to the subject.
        ///Tag = 41996 (A40C.H)
        /// Type = SHORT
        /// Count = 1
        /// Default = none
        ///   0 = unknown
        ///   1 = Macro
        ///   2 = Close view
        ///   3 = Distant view
        ///   Other = reserved
        /// </summary>
        public const int TAG_SUBJECT_DISTANCE_RANGE = 0xA40C;

        // Windows Attributes added/found by Ryan Patridge
        public const int TAG_XP_TITLE = 0x9C9B;
        public const int TAG_XP_COMMENTS = 0x9C9C;
        public const int TAG_XP_AUTHOR = 0x9C9D;
        public const int TAG_XP_KEYWORDS = 0x9C9E;
        public const int TAG_XP_SUBJECT = 0x9C9F;


        /// <summary>
        /// This tag indicates the equivalent focal length assuming a 35mm film camera,
        /// in mm. A value of 0 means the focal length is unknown. Note that this tag
        /// differs from the FocalLength tag.
        /// Tag = 41989 (A405.H)
        /// Type = SHORT
        /// Count = 1
        /// Default = none
        /// </summary>
        public const int TAG_FOCAL_LENGTH_IN_35MM_FILM = 0xA405;
        /// <summary>
        /// This tag indicates an identifier assigned uniquely to each image. It is
        /// recorded as an ASCII string equivalent to hexadecimal notation and 128-bit
        /// fixed length.
        /// Tag = 42016 (A420.H)
        /// Type = ASCII
        /// Count = 33
        /// Default = none
        /// </summary>
        public const int TAG_IMAGE_UNIQUE_ID = 0xA420;


        // are these two exif values?
        public const int TAG_FILL_ORDER = 0x010A;
        public const int TAG_DOCUMENT_NAME = 0x010D;

        public const int TAG_RELATED_IMAGE_FILE_FORMAT = 0x1000;
        public const int TAG_RELATED_IMAGE_WIDTH = 0x1001;
        public const int TAG_RELATED_IMAGE_LENGTH = 0x1002;
        public const int TAG_TRANSFER_RANGE = 0x0156;
        public const int TAG_JPEG_PROC = 0x0200;
        public const int TAG_EXIF_OFFSET = 0x8769;
        public const int TAG_MARKER_NOTE = 0x927C;
        public const int TAG_INTEROPERABILITY_OFFSET = 0xA005;

        /// <summary>
        /// Constructor of the object.
        /// </summary>
        public ExifDirectory()
            : base("ExifMarkernote")
        {
            this.SetDescriptor(new ExifDescriptor(this));
        }

        /// <summary>
        /// Gets the thumbnail data.
        /// </summary>
        /// <returns>the thumbnail data or null if none</returns>
        public byte[] GetThumbnailData()
        {
            if (!ContainsThumbnail())
            {
                return null;
            }

            return this.GetByteArray(ExifDirectory.TAG_THUMBNAIL_DATA);
        }

        /// <summary>
        /// Writes the thumbnail in the given aFile
        /// </summary>
        /// <param name="filename">where to write the thumbnail</param>
        /// <exception cref="MetadataException">if there is not data in thumbnail</exception>
        public void WriteThumbnail(string filename)
        {
            byte[] data = GetThumbnailData();

            if (data == null)
            {
                throw new MetadataException("No thumbnail data exists.");
            }

            try
            {
                using (var stream = new FileStream(filename, FileMode.CreateNew))
                {
                    stream.Write(data, 0, data.Length);
                }
            } catch (Exception e)
            {
                throw new ExifProcessingException("Error writing thumbnail: " + e.Message);
            }
        }

        /// <summary>
        /// Indicates if there is thumbnail data or not
        /// </summary>
        /// <returns>true if there is thumbnail data, false if not</returns>
        public bool ContainsThumbnail()
        {
            return ContainsTag(ExifDirectory.TAG_THUMBNAIL_DATA);
        }
    }
}