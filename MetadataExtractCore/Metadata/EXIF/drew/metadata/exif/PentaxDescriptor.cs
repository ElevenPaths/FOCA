namespace com.drew.metadata.exif
{
    /// <summary>
    /// Tag descriptor for pentax
    /// </summary>

    public class PentaxDescriptor : AbstractTagDescriptor
    {
        /// <summary>
        /// Constructor of the object
        /// </summary>
        /// <param name="base.directory">a base.directory</param>
        public PentaxDescriptor(AbstractDirectory aDirectory)
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
                case PentaxDirectory.TAG_PENTAX_CAPTURE_MODE:
                    return GetCaptureModeDescription();
                case PentaxDirectory.TAG_PENTAX_QUALITY_LEVEL:
                    return GetQualityLevelDescription();
                case PentaxDirectory.TAG_PENTAX_FOCUS_MODE:
                    return GetFocusModeDescription();
                case PentaxDirectory.TAG_PENTAX_FLASH_MODE:
                    return GetFlashModeDescription();
                case PentaxDirectory.TAG_PENTAX_WHITE_BALANCE:
                    return GetWhiteBalanceDescription();
                case PentaxDirectory.TAG_PENTAX_DIGITAL_ZOOM:
                    return GetDigitalZoomDescription();
                case PentaxDirectory.TAG_PENTAX_SHARPNESS:
                    return GetSharpnessDescription();
                case PentaxDirectory.TAG_PENTAX_CONTRAST:
                    return GetContrastDescription();
                case PentaxDirectory.TAG_PENTAX_SATURATION:
                    return GetSaturationDescription();
                case PentaxDirectory.TAG_PENTAX_ISO_SPEED:
                    return GetIsoSpeedDescription();
                case PentaxDirectory.TAG_PENTAX_COLOR:
                    return GetColorDescription();
                case PentaxDirectory.TAG_PENTAX_PRINT_IMAGE_MATCHING_INFO:
                    return GetPrintImageMatchingInfoDescription();
                default:
                    return base.directory.GetString(tagType);
            }
        }

        /// <summary>
        /// Returns the color Description. 
        /// </summary>
        /// <returns>the color Description.</returns>
        private string GetColorDescription()
        {
            if (!base.directory.ContainsTag(PentaxDirectory.TAG_PENTAX_COLOR))
            {
                return null;
            }
            int aValue = base.directory.GetInt(PentaxDirectory.TAG_PENTAX_COLOR);
            switch (aValue)
            {
                case 1: return BUNDLE["NORMAL"];
                case 2: return BUNDLE["BLACK_AND_WHITE"];
                case 3: return BUNDLE["SEPIA"];
                default: return BUNDLE["UNKNOWN", aValue.ToString()];
            }
        }

        /// <summary>
        /// Returns the iso speed Description. 
        /// </summary>
        /// <returns>the iso speed Description.</returns>
        private string GetIsoSpeedDescription()
        {
            if (!base.directory.ContainsTag(PentaxDirectory.TAG_PENTAX_ISO_SPEED))
            {
                return null;
            }
            int aValue = base.directory.GetInt(PentaxDirectory.TAG_PENTAX_ISO_SPEED);
            switch (aValue)
            {
                case 100:
                case 10: return BUNDLE["ISO", "100"];
                case 16:
                case 200: return BUNDLE["ISO", "200"];
                default: return BUNDLE["UNKNOWN", aValue.ToString()];
            }
        }

        /// <summary>
        /// Returns the saturation Description. 
        /// </summary>
        /// <returns>the saturation Description.</returns>
        private string GetSaturationDescription()
        {
            if (!base.directory.ContainsTag(PentaxDirectory.TAG_PENTAX_SATURATION))
            {
                return null;
            }
            int aValue = base.directory.GetInt(PentaxDirectory.TAG_PENTAX_SATURATION);
            switch (aValue)
            {
                case 0: return BUNDLE["NORMAL"];
                case 1: return BUNDLE["LOW"];
                case 2: return BUNDLE["HIGH"];
                default: return BUNDLE["UNKNOWN", aValue.ToString()];
            }
        }

        /// <summary>
        /// Returns the contrast Description. 
        /// </summary>
        /// <returns>the contrast Description.</returns>
        private string GetContrastDescription()
        {
            if (!base.directory.ContainsTag(PentaxDirectory.TAG_PENTAX_CONTRAST))
            {
                return null;
            }
            int aValue = base.directory.GetInt(PentaxDirectory.TAG_PENTAX_CONTRAST);
            switch (aValue)
            {
                case 0: return BUNDLE["NORMAL"];
                case 1: return BUNDLE["LOW"];
                case 2: return BUNDLE["HIGH"];
                default: return BUNDLE["UNKNOWN", aValue.ToString()];
            }
        }

        /// <summary>
        /// Returns the sharpness Description. 
        /// </summary>
        /// <returns>the sharpness Description.</returns>
        private string GetSharpnessDescription()
        {
            if (!base.directory.ContainsTag(PentaxDirectory.TAG_PENTAX_SHARPNESS))
            {
                return null;
            }
            int aValue = base.directory.GetInt(PentaxDirectory.TAG_PENTAX_SHARPNESS);
            switch (aValue)
            {
                case 0: return BUNDLE["NORMAL"];
                case 1: return BUNDLE["SOFT"];
                case 2: return BUNDLE["HARD"];
                default: return BUNDLE["UNKNOWN", aValue.ToString()];
            }
        }

        /// <summary>
        /// Returns the digial zoom Description. 
        /// </summary>
        /// <returns>the digital zoom Description.</returns>
        private string GetDigitalZoomDescription()
        {
            if (!base.directory.ContainsTag(PentaxDirectory.TAG_PENTAX_DIGITAL_ZOOM))
            {
                return null;
            }
            float aValue = base.directory.GetFloat(PentaxDirectory.TAG_PENTAX_DIGITAL_ZOOM);
            if (aValue == 0)
            {
                return BUNDLE["OFF"];
            }
            return aValue.ToString();
        }

        /// <summary>
        /// Returns the white balance Description. 
        /// </summary>
        /// <returns>the white balance Description.</returns>
        private string GetWhiteBalanceDescription()
        {
            if (!base.directory.ContainsTag(PentaxDirectory.TAG_PENTAX_WHITE_BALANCE))
            {
                return null;
            }
            int aValue = base.directory.GetInt(PentaxDirectory.TAG_PENTAX_WHITE_BALANCE);
            switch (aValue)
            {
                case 0: return BUNDLE["AUTO"];
                case 1: return BUNDLE["DAYLIGHT"];
                case 2: return BUNDLE["SHADE"];
                case 3: return BUNDLE["TUNGSTEN"];
                case 4: return BUNDLE["FLUORESCENT"];
                case 5: return BUNDLE["MANUAL"];
                default: return BUNDLE["UNKNOWN", aValue.ToString()];
            }
        }

        /// <summary>
        /// Returns the flash mode Description. 
        /// </summary>
        /// <returns>the dlash mode Description.</returns>
        private string GetFlashModeDescription()
        {
            if (!base.directory.ContainsTag(PentaxDirectory.TAG_PENTAX_FLASH_MODE))
            {
                return null;
            }
            int aValue = base.directory.GetInt(PentaxDirectory.TAG_PENTAX_FLASH_MODE);
            switch (aValue)
            {
                case 1: return BUNDLE["AUTO"];
                case 2: return BUNDLE["FLASH_ON"];
                case 4: return BUNDLE["FLASH_OFF"];
                case 6: return BUNDLE["RED_EYE_REDUCTION"];
                default: return BUNDLE["UNKNOWN", aValue.ToString()];
            }
        }

        /// <summary>
        /// Returns the focus mode Description. 
        /// </summary>
        /// <returns>the focus mode Description.</returns>
        private string GetFocusModeDescription()
        {
            if (!base.directory.ContainsTag(PentaxDirectory.TAG_PENTAX_FOCUS_MODE))
            {
                return null;
            }
            int aValue = base.directory.GetInt(PentaxDirectory.TAG_PENTAX_FOCUS_MODE);
            switch (aValue)
            {
                case 2: return BUNDLE["CUSTOM"];
                case 3: return BUNDLE["AUTO"];
                default: return BUNDLE["UNKNOWN", aValue.ToString()];
            }
        }

        /// <summary>
        /// Returns the quality level Description. 
        /// </summary>
        /// <returns>the quality level Description.</returns>
        private string GetQualityLevelDescription()
        {
            if (!base.directory.ContainsTag(PentaxDirectory.TAG_PENTAX_QUALITY_LEVEL))
            {
                return null;
            }
            int aValue = base.directory.GetInt(PentaxDirectory.TAG_PENTAX_QUALITY_LEVEL);
            switch (aValue)
            {
                case 0: return BUNDLE["GOOD"];
                case 1: return BUNDLE["BETTER"];
                case 2: return BUNDLE["BEST"];
                default: return BUNDLE["UNKNOWN", aValue.ToString()];
            }
        }

        /// <summary>
        /// Returns the capture mode Description. 
        /// </summary>
        /// <returns>the capture mode Description.</returns>
        private string GetCaptureModeDescription()
        {
            if (!base.directory.ContainsTag(PentaxDirectory.TAG_PENTAX_CAPTURE_MODE))
            {
                return null;
            }
            int aValue = base.directory.GetInt(PentaxDirectory.TAG_PENTAX_CAPTURE_MODE);
            switch (aValue)
            {
                case 1: return BUNDLE["AUTO"];
                case 2: return BUNDLE["NIGHT_SCENE"];
                case 3: return BUNDLE["MANUAL"];
                case 4: return BUNDLE["MULTIPLE"];
                default: return BUNDLE["UNKNOWN", aValue.ToString()];
            }
        }

        /// <summary>
        /// Returns the print image matching info Description. 
        /// </summary>
        /// <returns>the print image matching info Description.</returns>
        private string GetPrintImageMatchingInfoDescription()
        {
            if (!base.directory.ContainsTag(PentaxDirectory.TAG_PENTAX_PRINT_IMAGE_MATCHING_INFO))
            {
                return null;
            }
            byte[] bytes = base.directory.GetByteArray(PentaxDirectory.TAG_PENTAX_PRINT_IMAGE_MATCHING_INFO);
            return BUNDLE["BYTES", bytes.Length.ToString()];
        }

    }
}