namespace com.drew.metadata.exif
{
    /// <summary>
    /// Tag descriptor for a casio camera type 2
    /// </summary>

    public class CasioType2Descriptor : AbstractTagDescriptor
    {
        /// <summary>
        /// Constructor of the object
        /// </summary>
        /// <param name="aDirectory">a base.directory</param>
        public CasioType2Descriptor(AbstractDirectory aDirectory)
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
        public override string GetDescription(int aTagType)
        {
            switch (aTagType)
            {
                case CasioType2Directory.TAG_CASIO_TYPE2_THUMBNAIL_DIMENSIONS:
                    return this.GetThumbnailDimensionsDescription();
                case CasioType2Directory.TAG_CASIO_TYPE2_THUMBNAIL_SIZE:
                    return this.GetThumbnailSizeDescription();
                case CasioType2Directory.TAG_CASIO_TYPE2_THUMBNAIL_OFFSET:
                    return this.GetThumbnailOffsetDescription();
                case CasioType2Directory.TAG_CASIO_TYPE2_QUALITY_MODE:
                    return this.GetQualityModeDescription();
                case CasioType2Directory.TAG_CASIO_TYPE2_IMAGE_SIZE:
                    return this.GetImageSizeDescription();
                case CasioType2Directory.TAG_CASIO_TYPE2_FOCUS_MODE_1:
                    return this.GetFocusMode1Description();
                case CasioType2Directory.TAG_CASIO_TYPE2_ISO_SENSITIVITY:
                    return this.GetIsoSensitivityDescription();
                case CasioType2Directory.TAG_CASIO_TYPE2_WHITE_BALANCE_1:
                    return this.GetWhiteBalance1Description();
                case CasioType2Directory.TAG_CASIO_TYPE2_FOCAL_LENGTH:
                    return this.GetFocalLengthDescription();
                case CasioType2Directory.TAG_CASIO_TYPE2_SATURATION:
                    return this.GetSaturationDescription();
                case CasioType2Directory.TAG_CASIO_TYPE2_CONTRAST:
                    return this.GetContrastDescription();
                case CasioType2Directory.TAG_CASIO_TYPE2_SHARPNESS:
                    return this.GetSharpnessDescription();
                case CasioType2Directory.TAG_CASIO_TYPE2_PRINT_IMAGE_MATCHING_INFO:
                    return this.GetPrintImageMatchingInfoDescription();
                case CasioType2Directory.TAG_CASIO_TYPE2_CASIO_PREVIEW_THUMBNAIL:
                    return this.GetCasioPreviewThumbnailDescription();
                case CasioType2Directory.TAG_CASIO_TYPE2_WHITE_BALANCE_BIAS:
                    return this.GetWhiteBalanceBiasDescription();
                case CasioType2Directory.TAG_CASIO_TYPE2_WHITE_BALANCE_2:
                    return this.GetWhiteBalance2Description();
                case CasioType2Directory.TAG_CASIO_TYPE2_OBJECT_DISTANCE:
                    return this.GetObjectDistanceDescription();
                case CasioType2Directory.TAG_CASIO_TYPE2_FLASH_DISTANCE:
                    return this.GetFlashDistanceDescription();
                case CasioType2Directory.TAG_CASIO_TYPE2_RECORD_MODE:
                    return this.GetRecordModeDescription();
                case CasioType2Directory.TAG_CASIO_TYPE2_SELF_TIMER:
                    return this.GetSelfTimerDescription();
                case CasioType2Directory.TAG_CASIO_TYPE2_QUALITY:
                    return this.GetQualityDescription();
                case CasioType2Directory.TAG_CASIO_TYPE2_FOCUS_MODE_2:
                    return this.GetFocusMode2Description();
                case CasioType2Directory.TAG_CASIO_TYPE2_TIME_ZONE:
                    return this.GetTimeZoneDescription();
                case CasioType2Directory.TAG_CASIO_TYPE2_BESTSHOT_MODE:
                    return this.GetBestShotModeDescription();
                case CasioType2Directory.TAG_CASIO_TYPE2_CCD_ISO_SENSITIVITY:
                    return this.GetCcdIsoSensitivityDescription();
                case CasioType2Directory.TAG_CASIO_TYPE2_COLOR_MODE:
                    return this.GetColorModeDescription();
                case CasioType2Directory.TAG_CASIO_TYPE2_ENHANCEMENT:
                    return this.GetEnhancementDescription();
                case CasioType2Directory.TAG_CASIO_TYPE2_FILTER:
                    return this.GetFilterDescription();
                default:
                    return base.directory.GetString(aTagType);
            }
        }

        /// <summary>
        /// Returns filter Description. 
        /// </summary>
        /// <returns>the filter Description.</returns>
        private string GetFilterDescription()
        {
            if (!base.directory.ContainsTag(CasioType2Directory.TAG_CASIO_TYPE2_FILTER))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(CasioType2Directory.TAG_CASIO_TYPE2_FILTER);
            switch (lcVal)
            {
                case 0:
                    return BUNDLE["OFF"];
                default:
                    return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }

        /// <summary>
        /// Returns enhancement Description. 
        /// </summary>
        /// <returns>the enhancement Description.</returns>
        private string GetEnhancementDescription()
        {
            if (!base.directory.ContainsTag(CasioType2Directory.TAG_CASIO_TYPE2_ENHANCEMENT))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(CasioType2Directory.TAG_CASIO_TYPE2_ENHANCEMENT);
            switch (lcVal)
            {
                case 0:
                    return BUNDLE["OFF"];
                default:
                    return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }

        /// <summary>
        /// Returns color mode Description. 
        /// </summary>
        /// <returns>the color mode Description.</returns>
        private string GetColorModeDescription()
        {
            if (!base.directory.ContainsTag(CasioType2Directory.TAG_CASIO_TYPE2_COLOR_MODE))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(CasioType2Directory.TAG_CASIO_TYPE2_COLOR_MODE);
            switch (lcVal)
            {
                case 0:
                    return BUNDLE["OFF"];
                default:
                    return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }

        /// <summary>
        /// Returns CCD ISO sensitivity Description. 
        /// </summary>
        /// <returns>the CCD ISO sensitivity Description.</returns>
        private string GetCcdIsoSensitivityDescription()
        {
            if (!base.directory.ContainsTag(CasioType2Directory.TAG_CASIO_TYPE2_CCD_ISO_SENSITIVITY))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(CasioType2Directory.TAG_CASIO_TYPE2_CCD_ISO_SENSITIVITY);
            switch (lcVal)
            {
                case 0:
                    return BUNDLE["OFF"];
                case 1:
                    return BUNDLE["ON"];
                default:
                    return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }

        /// <summary>
        /// Returns best shot mode Description. 
        /// </summary>
        /// <returns>the best shot mode Description.</returns>
        private string GetBestShotModeDescription()
        {
            if (!base.directory.ContainsTag(CasioType2Directory.TAG_CASIO_TYPE2_BESTSHOT_MODE))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(CasioType2Directory.TAG_CASIO_TYPE2_BESTSHOT_MODE);
            switch (lcVal)
            {
                default:
                    return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }

        /// <summary>
        /// Returns time zone Description. 
        /// </summary>
        /// <returns>the time zone Description.</returns>
        private string GetTimeZoneDescription()
        {
            if (!base.directory.ContainsTag(CasioType2Directory.TAG_CASIO_TYPE2_TIME_ZONE))
            {
                return null;
            }
            return base.directory.GetString(CasioType2Directory.TAG_CASIO_TYPE2_TIME_ZONE);
        }

        /// <summary>
        /// Returns focus mode 2 Description. 
        /// </summary>
        /// <returns>the focus mode 2 Description.</returns>
        private string GetFocusMode2Description()
        {
            if (!base.directory.ContainsTag(CasioType2Directory.TAG_CASIO_TYPE2_FOCUS_MODE_2))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(CasioType2Directory.TAG_CASIO_TYPE2_FOCUS_MODE_2);
            switch (lcVal)
            {
                case 1:
                    return BUNDLE["FIXATION"];
                case 6:
                    return BUNDLE["MULTI_AREA_FOCUS"];
                default:
                    return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }

        /// <summary>
        /// Returns quality description Description. 
        /// </summary>
        /// <returns>the quality description Description.</returns>
        private string GetQualityDescription()
        {
            if (!base.directory.ContainsTag(CasioType2Directory.TAG_CASIO_TYPE2_QUALITY))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(CasioType2Directory.TAG_CASIO_TYPE2_QUALITY);
            switch (lcVal)
            {
                case 3:
                    return BUNDLE["FINE"];
                default:
                    return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }

        /// <summary>
        /// Returns self timer Description. 
        /// </summary>
        /// <returns>the self timer Description.</returns>
        private string GetSelfTimerDescription()
        {
            if (!base.directory.ContainsTag(CasioType2Directory.TAG_CASIO_TYPE2_SELF_TIMER))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(CasioType2Directory.TAG_CASIO_TYPE2_SELF_TIMER);
            switch (lcVal)
            {
                case 1:
                    return BUNDLE["OFF"];
                default:
                    return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }

        /// <summary>
        /// Returns record mode Description. 
        /// </summary>
        /// <returns>the record mode Description.</returns>
        private string GetRecordModeDescription()
        {
            if (!base.directory.ContainsTag(CasioType2Directory.TAG_CASIO_TYPE2_RECORD_MODE))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(CasioType2Directory.TAG_CASIO_TYPE2_RECORD_MODE);
            switch (lcVal)
            {
                case 2:
                    return BUNDLE["NORMAL"];
                default:
                    return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }

        /// <summary>
        /// Returns flash distance Description. 
        /// </summary>
        /// <returns>the flash distance Description.</returns>
        private string GetFlashDistanceDescription()
        {
            if (!base.directory.ContainsTag(CasioType2Directory.TAG_CASIO_TYPE2_FLASH_DISTANCE))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(CasioType2Directory.TAG_CASIO_TYPE2_FLASH_DISTANCE);
            switch (lcVal)
            {
                case 0:
                    return BUNDLE["OFF"];
                default:
                    return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }

        /// <summary>
        /// Returns object distance Description. 
        /// </summary>
        /// <returns>the object distance Description.</returns>
        private string GetObjectDistanceDescription()
        {
            if (!base.directory.ContainsTag(CasioType2Directory.TAG_CASIO_TYPE2_OBJECT_DISTANCE))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(CasioType2Directory.TAG_CASIO_TYPE2_OBJECT_DISTANCE);
            return BUNDLE["DISTANCE_MM", lcVal.ToString()];
        }

        /// <summary>
        /// Returns white balance 2 Description. 
        /// </summary>
        /// <returns>the white balance 2 Description.</returns>
        private string GetWhiteBalance2Description()
        {
            if (!base.directory.ContainsTag(CasioType2Directory.TAG_CASIO_TYPE2_WHITE_BALANCE_2))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(CasioType2Directory.TAG_CASIO_TYPE2_WHITE_BALANCE_2);
            switch (lcVal)
            {
                case 0:
                    return BUNDLE["MANUAL"];
                case 1:
                    return BUNDLE["AUTO"]; // unsure about this
                case 4:
                    return BUNDLE["FLASH"]; // unsure about this
                case 12:
                    return BUNDLE["FLASH"];
                default:
                    return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }

        /// <summary>
        /// Returns white balance bias Description. 
        /// </summary>
        /// <returns>the white balance bias Description.</returns>
        private string GetWhiteBalanceBiasDescription()
        {
            if (!base.directory.ContainsTag(CasioType2Directory.TAG_CASIO_TYPE2_WHITE_BALANCE_BIAS))
            {
                return null;
            }
            return base.directory.GetString(CasioType2Directory.TAG_CASIO_TYPE2_WHITE_BALANCE_BIAS);
        }

        /// <summary>
        /// Returns casio preview thumbnail Description. 
        /// </summary>
        /// <returns>the casio preview thumbnail Description.</returns>
        private string GetCasioPreviewThumbnailDescription()
        {
            if (!base.directory.ContainsTag(CasioType2Directory.TAG_CASIO_TYPE2_CASIO_PREVIEW_THUMBNAIL))
            {
                return null;
            }
            byte[] lcBytes = base.directory.GetByteArray(CasioType2Directory.TAG_CASIO_TYPE2_CASIO_PREVIEW_THUMBNAIL);
            return BUNDLE["BYTES_OF_IMAGE_DATA", lcBytes.Length.ToString()];
        }

        /// <summary>
        /// Returns Print Image Matching Info Description. 
        /// </summary>
        /// <returns>the Print Image Matching Info Description.</returns>
        private string GetPrintImageMatchingInfoDescription()
        {
            // TODO research PIM specification http://www.ozhiker.com/electronics/pjmt/jpeg_info/pim.html
            if (!base.directory.ContainsTag(CasioType2Directory.TAG_CASIO_TYPE2_PRINT_IMAGE_MATCHING_INFO))
            {
                return null;
            }
            return base.directory.GetString(CasioType2Directory.TAG_CASIO_TYPE2_PRINT_IMAGE_MATCHING_INFO);
        }

        /// <summary>
        /// Returns sharpness description Description. 
        /// </summary>
        /// <returns>the sharpness description Description.</returns>
        private string GetSharpnessDescription()
        {
            if (!base.directory.ContainsTag(CasioType2Directory.TAG_CASIO_TYPE2_SHARPNESS))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(CasioType2Directory.TAG_CASIO_TYPE2_SHARPNESS);
            switch (lcVal)
            {
                case 0:
                    return "-1";
                case 1:
                    return BUNDLE["NORMAL"];
                case 2:
                    return "+1";
                default:
                    return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }

        /// <summary>
        /// Returns contrast Description. 
        /// </summary>
        /// <returns>the contrast Description.</returns>
        private string GetContrastDescription()
        {
            if (!base.directory.ContainsTag(CasioType2Directory.TAG_CASIO_TYPE2_CONTRAST))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(CasioType2Directory.TAG_CASIO_TYPE2_CONTRAST);
            switch (lcVal)
            {
                case 0:
                    return "-1";
                case 1:
                    return BUNDLE["NORMAL"];
                case 2:
                    return "+1";
                default:
                    return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }

        /// <summary>
        /// Returns saturation Description. 
        /// </summary>
        /// <returns>the saturation Description.</returns>
        private string GetSaturationDescription()
        {
            if (!base.directory.ContainsTag(CasioType2Directory.TAG_CASIO_TYPE2_SATURATION))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(CasioType2Directory.TAG_CASIO_TYPE2_SATURATION);
            switch (lcVal)
            {
                case 0:
                    return "-1";
                case 1:
                    return BUNDLE["NORMAL"];
                case 2:
                    return "+1";
                default:
                    return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }

        /// <summary>
        /// Returns focal length Description. 
        /// </summary>
        /// <returns>the focal length Description.</returns>
        private string GetFocalLengthDescription()
        {
            if (!base.directory.ContainsTag(CasioType2Directory.TAG_CASIO_TYPE2_FOCAL_LENGTH)) return null;
            double lcVal = base.directory.GetDouble(CasioType2Directory.TAG_CASIO_TYPE2_FOCAL_LENGTH);
            return BUNDLE["DISTANCE_MM", (lcVal / 10.0).ToString()];
        }

        /// <summary>
        /// Returns white balance 1 Description. 
        /// </summary>
        /// <returns>the white balance 1 Description.</returns>
        private string GetWhiteBalance1Description()
        {
            if (!base.directory.ContainsTag(CasioType2Directory.TAG_CASIO_TYPE2_WHITE_BALANCE_1))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(CasioType2Directory.TAG_CASIO_TYPE2_WHITE_BALANCE_1);
            switch (lcVal)
            {
                case 0:
                    return BUNDLE["AUTO"];
                case 1:
                    return BUNDLE["DAYLIGHT"];
                case 2:
                    return BUNDLE["SHADE"];
                case 3:
                    return BUNDLE["TUNGSTEN"];
                case 4:
                    return BUNDLE["FLUORESCENT"];
                case 5:
                    return BUNDLE["MANUAL"];
                default:
                    return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }

        /// <summary>
        /// Returns ISO sensitivity Description. 
        /// </summary>
        /// <returns>the ISO sensitivity Description.</returns>
        private string GetIsoSensitivityDescription()
        {
            if (!base.directory.ContainsTag(CasioType2Directory.TAG_CASIO_TYPE2_ISO_SENSITIVITY))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(CasioType2Directory.TAG_CASIO_TYPE2_ISO_SENSITIVITY);
            switch (lcVal)
            {
                case 3:
                    return BUNDLE["ISO", "50"];
                case 4:
                    return BUNDLE["ISO","64"];
                case 6:
                    return BUNDLE["ISO","100"];
                case 9:
                    return BUNDLE["ISO","200"];
                default:
                    return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }

        /// <summary>
        /// Returns focus mode 1 Description. 
        /// </summary>
        /// <returns>the focus mode 1 Description.</returns>
        private string GetFocusMode1Description()
        {
            if (!base.directory.ContainsTag(CasioType2Directory.TAG_CASIO_TYPE2_FOCUS_MODE_1))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(CasioType2Directory.TAG_CASIO_TYPE2_FOCUS_MODE_1);
            switch (lcVal)
            {
                case 0:
                    return BUNDLE["NORMAL"];
                case 1:
                    return BUNDLE["MACRO"];
                default:
                    return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }

        /// <summary>
        /// Returns image size Description. 
        /// </summary>
        /// <returns>the image size Description.</returns>
        private string GetImageSizeDescription()
        {
            if (!base.directory.ContainsTag(CasioType2Directory.TAG_CASIO_TYPE2_IMAGE_SIZE))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(CasioType2Directory.TAG_CASIO_TYPE2_IMAGE_SIZE);
            switch (lcVal)
            {
                case 0: return BUNDLE["PIXELS", "640 x 480"];
                case 4: return BUNDLE["PIXELS", "1600 x 1200"];
                case 5: return BUNDLE["PIXELS", "2048 x 1536"];
                case 20: return BUNDLE["PIXELS", "2288 x 1712"];
                case 21: return BUNDLE["PIXELS", "2592 x 1944"];
                case 22: return BUNDLE["PIXELS", "2304 x 1728"];
                case 36: return BUNDLE["PIXELS", "3008 x 2008"];
                default: return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }

        /// <summary>
        /// Returns quality mode Description. 
        /// </summary>
        /// <returns>the quality mode Description.</returns>
        private string GetQualityModeDescription()
        {
            if (!base.directory.ContainsTag(CasioType2Directory.TAG_CASIO_TYPE2_QUALITY_MODE))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(CasioType2Directory.TAG_CASIO_TYPE2_QUALITY_MODE);
            switch (lcVal)
            {
                case 1:
                    return BUNDLE["FINE"];
                case 2:
                    return BUNDLE["SUPERFINE"];
                default:
                    return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }

        /// <summary>
        /// Returns thumbnail lcOffset Description. 
        /// </summary>
        /// <returns>the thumbnail lcOffset Description.</returns>
        private string GetThumbnailOffsetDescription()
        {
            if (!base.directory.ContainsTag(CasioType2Directory.TAG_CASIO_TYPE2_THUMBNAIL_OFFSET))
            {
                return null;
            }
            return base.directory.GetString(CasioType2Directory.TAG_CASIO_TYPE2_THUMBNAIL_OFFSET);
        }

        /// <summary>
        /// Returns thumbnail size Description. 
        /// </summary>
        /// <returns>the thumbnail size Description.</returns>
        private string GetThumbnailSizeDescription()
        {
            if (!base.directory.ContainsTag(CasioType2Directory.TAG_CASIO_TYPE2_THUMBNAIL_SIZE))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(CasioType2Directory.TAG_CASIO_TYPE2_THUMBNAIL_SIZE);
            return BUNDLE["BYTES", lcVal.ToString()];
        }

        /// <summary>
        /// Returns thumbnail dimension Description. 
        /// </summary>
        /// <returns>the thumbnail dimension Description.</returns>
        private string GetThumbnailDimensionsDescription()
        {
            if (!base.directory.ContainsTag(CasioType2Directory.TAG_CASIO_TYPE2_THUMBNAIL_DIMENSIONS))
            {
                return null;
            }
            int[] lcDimensions = base.directory.GetIntArray(CasioType2Directory.TAG_CASIO_TYPE2_THUMBNAIL_DIMENSIONS);
            if (lcDimensions.Length != 2)
            {
                return base.directory.GetString(CasioType2Directory.TAG_CASIO_TYPE2_THUMBNAIL_DIMENSIONS);
            }
            return BUNDLE["PIXELS_BI", lcDimensions[0].ToString(), lcDimensions[1].ToString()];
        }
    }
}