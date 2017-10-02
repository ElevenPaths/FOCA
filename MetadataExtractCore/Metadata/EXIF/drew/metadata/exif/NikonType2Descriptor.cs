using System;
using com.drew.lang;

namespace com.drew.metadata.exif
{
    /// <summary>
    /// Tag descriptor for Nikon
    /// </summary>

    public class NikonType2Descriptor : AbstractTagDescriptor
    {
        /// <summary>
        /// Constructor of the object
        /// </summary>
        /// <param name="base.directory">a base.directory</param>
        public NikonType2Descriptor(AbstractDirectory aDirectory)
            : base(aDirectory)
        {
        }

        /// <summary>
        /// Returns a descriptive value of the the specified tag for this image. 
        /// Where possible, known values will be substituted here in place of the raw tokens actually 
        /// kept in the Exif segment. 
        /// If no substitution is available, the value provided by GetString(int) will be returned.
        /// This and GetString(int) are the only 'get' methods that won't throw an exception.
        /// </summary>
        /// <param name="aTagType">the tag to find a description for</param>
        /// <returns>a description of the image'str value for the specified tag, or null if the tag hasn't been defined.</returns>
        public override string GetDescription(int tagType)
        {
            switch (tagType)
            {
                case NikonType2Directory.TAG_NIKON_TYPE2_LENS:
                    return GetLensDescription();
                case NikonType2Directory.TAG_NIKON_TYPE2_CAMERA_HUE_ADJUSTMENT:
                    return GetHueAdjustmentDescription();
                case NikonType2Directory.TAG_NIKON_TYPE2_CAMERA_COLOR_MODE:
                    return GetColorModeDescription();
                case NikonType2Directory.TAG_NIKON_TYPE2_AUTO_FLASH_COMPENSATION:
                    return GetAutoFlashCompensationDescription();
                case NikonType2Directory.TAG_NIKON_TYPE2_ISO_1:
                    return GetIsoSettingDescription();
                case NikonType2Directory.TAG_NIKON_TYPE2_DIGITAL_ZOOM:
                    return GetDigitalZoomDescription();
                case NikonType2Directory.TAG_NIKON_TYPE2_AF_FOCUS_POSITION:
                    return GetAutoFocusPositionDescription();
                case NikonType2Directory.TAG_NIKON_TYPE2_FIRMWARE_VERSION:
                    return GetAutoFirmwareVersionDescription();
                default:
                    return base.directory.GetString(tagType);
            }
        }

        /// <summary>
        /// Returns auto focus position Description. 
        /// </summary>
        /// <returns>the auto focus position Description.</returns>
        private string GetAutoFocusPositionDescription()
        {
            if (!base.directory.ContainsTag(NikonType2Directory.TAG_NIKON_TYPE2_AF_FOCUS_POSITION))
            {
                return null;
            }
            int[] values = base.directory.GetIntArray(NikonType2Directory.TAG_NIKON_TYPE2_AF_FOCUS_POSITION);
            if (values.Length != 4 || values[0] != 0 || values[2] != 0 || values[3] != 0)
            {
                return BUNDLE["UNKNOWN", base.directory.GetString(NikonType2Directory.TAG_NIKON_TYPE2_AF_FOCUS_POSITION)];
            }
            switch (values[1])
            {
                case 0:
                    return BUNDLE["CENTER"];
                case 1:
                    return BUNDLE["TOP"];
                case 2:
                    return BUNDLE["BOTTOM"];
                case 3:
                    return BUNDLE["LEFT"];
                case 4:
                    return BUNDLE["RIGHT"];
                default:
                    return BUNDLE["UNKNOWN", values[1].ToString()];
            }
        }

        /// <summary>
        /// Returns digital zoom Description. 
        /// </summary>
        /// <returns>the digital zoom Description.</returns>
        private string GetDigitalZoomDescription()
        {
            if (!base.directory.ContainsTag(NikonType2Directory.TAG_NIKON_TYPE2_DIGITAL_ZOOM))
            {
                return null;
            }
            Rational rational = base.directory.GetRational(NikonType2Directory.TAG_NIKON_TYPE2_DIGITAL_ZOOM);
            if (rational.IntValue() == 1)
            {
                return BUNDLE["NO_DIGITAL_ZOOM"];
            }
            return BUNDLE["DIGITAL_ZOOM", rational.ToSimpleString(true)];
        }

        /// <summary>
        /// Returns iso setting Description. 
        /// </summary>
        /// <returns>the iso setting Description.</returns>
        private string GetIsoSettingDescription()
        {
            if (!base.directory.ContainsTag(NikonType2Directory.TAG_NIKON_TYPE2_ISO_1))
            {
                return null;
            }
            int[] values = base.directory.GetIntArray(NikonType2Directory.TAG_NIKON_TYPE2_ISO_1);
            if (values[0] != 0 || values[1] == 0)
            {
                return BUNDLE["UNKNOWN", base.directory.GetString(NikonType2Directory.TAG_NIKON_TYPE2_ISO_1)];
            }
            return BUNDLE["ISO", values[1].ToString()];
        }

        /// <summary>
        /// Returns auto flash compensation Description. 
        /// </summary>
        /// <returns>the auto flash compensation Description.</returns>
        private Rational GetAutoFlashCompensation()
        {
            if (!base.directory.ContainsTag(NikonType2Directory.TAG_NIKON_TYPE2_AUTO_FLASH_COMPENSATION))
            {
                return null;
            }
            byte[] bytes = base.directory.GetByteArray(NikonType2Directory.TAG_NIKON_TYPE2_AUTO_FLASH_COMPENSATION);

            if (bytes.Length == 3)
            {
                byte denominator = bytes[2];
                int numerator = (int)bytes[0] * bytes[1];
                return new Rational(numerator, denominator);
            }
            return null;
        }

        /// <summary>
        /// Returns auto falsh compensation Description. 
        /// </summary>
        /// <returns>the auto falsh compensation Description.</returns>
        private string GetAutoFlashCompensationDescription()
        {
            Rational ev = this.GetAutoFlashCompensation();

            if (ev == null)
            {
                return BUNDLE["UNKNOWN", "null"];
            }
            return BUNDLE["FLASH_SIMPLE", ev.FloatValue().ToString("0.##")];
        }

        /// <summary>
        /// Returns lens Description. 
        /// </summary>
        /// <returns>the lens Description.</returns>
        private string GetLensDescription()
        {
            if (!base.directory.ContainsTag(NikonType2Directory.TAG_NIKON_TYPE2_LENS))
            {
                return null;
            }

            Rational[] lensValues = base.directory.GetRationalArray(NikonType2Directory.TAG_NIKON_TYPE2_LENS);

            if (lensValues.Length != 4)
            {
                return base.directory.GetString(NikonType2Directory.TAG_NIKON_TYPE2_LENS);
            }
            string[] tab = new string[] { lensValues[0].IntValue().ToString(), lensValues[1].IntValue().ToString(), lensValues[2].IntValue().ToString(), lensValues[3].IntValue().ToString() };
            return BUNDLE["LENS", tab];
        }

        /// <summary>
        /// Returns hue adjustement Description. 
        /// </summary>
        /// <returns>the hue adjustement Description.</returns>
        private string GetHueAdjustmentDescription()
        {
            if (!base.directory.ContainsTag(NikonType2Directory.TAG_NIKON_TYPE2_CAMERA_HUE_ADJUSTMENT))
            {
                return null;
            }

            return BUNDLE["DEGREES", base.directory.GetString(NikonType2Directory.TAG_NIKON_TYPE2_CAMERA_HUE_ADJUSTMENT)];
        }

        /// <summary>
        /// Returns color mode Description. 
        /// </summary>
        /// <returns>the color mode Description.</returns>
        private string GetColorModeDescription()
        {
            if (!base.directory.ContainsTag(NikonType2Directory.TAG_NIKON_TYPE2_CAMERA_COLOR_MODE))
            {
                return null;
            }

            String raw = base.directory.GetString(NikonType2Directory.TAG_NIKON_TYPE2_CAMERA_COLOR_MODE);
            if (raw.StartsWith("MODE1"))
            {
                return BUNDLE["MODE_I_SRGB"];
            }

            return raw;
        }

        /// <summary>
        /// Returns auto firmware version Description. 
        /// </summary>
        /// <returns>the auto firmware version Description.</returns>
        private string GetAutoFirmwareVersionDescription()
        {
            if (!base.directory.ContainsTag(NikonType2Directory.TAG_NIKON_TYPE2_FIRMWARE_VERSION))
            {
                return null;
            }

            int[] ints = base.directory.GetIntArray(NikonType2Directory.TAG_NIKON_TYPE2_FIRMWARE_VERSION);
            return ExifDescriptor.ConvertBytesToVersionString(ints);
        }
    }
}
