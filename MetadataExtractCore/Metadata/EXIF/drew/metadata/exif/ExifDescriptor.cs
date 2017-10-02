using System;
using System.Text;
using com.drew.lang;
using com.utils;

namespace com.drew.metadata.exif
{
	/// <summary>
	/// Tag descriptor for almost every images
	/// </summary>

	public class ExifDescriptor : AbstractTagDescriptor 
	{
		/// <summary>
		/// Dictates whether rational values will be represented in decimal format in instances 
		/// where decimal notation is elegant (such as 1/2 -> 0.5, but not 1/3).
		/// </summary>
		private readonly bool allowDecimalRepresentationOfRationals = true;

		/// <summary>
		/// Constructor of the object
		/// </summary>
		/// <param name="directory">a directory</param>
		public ExifDescriptor(AbstractDirectory directory) : base(directory)
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
			switch(aTagType) 
			{
				case ExifDirectory.TAG_ORIENTATION:
					return GetOrientationDescription();
				case ExifDirectory.TAG_RESOLUTION_UNIT:
					return GetResolutionDescription();
				case ExifDirectory.TAG_YCBCR_POSITIONING:
					return GetYCbCrPositioningDescription();
				case ExifDirectory.TAG_EXPOSURE_TIME:
					return GetExposureTimeDescription();
				case ExifDirectory.TAG_SHUTTER_SPEED:
					return GetShutterSpeedDescription();
				case ExifDirectory.TAG_FNUMBER:
					return GetFNumberDescription();
				case ExifDirectory.TAG_X_RESOLUTION:
					return GetXResolutionDescription();
				case ExifDirectory.TAG_Y_RESOLUTION:
					return GetYResolutionDescription();
				case ExifDirectory.TAG_THUMBNAIL_OFFSET:
					return GetThumbnailOffSetDescription();
				case ExifDirectory.TAG_THUMBNAIL_LENGTH:
					return GetThumbnailLengthDescription();
				case ExifDirectory.TAG_COMPRESSION_LEVEL:
					return GetCompressionLevelDescription();
				case ExifDirectory.TAG_SUBJECT_DISTANCE:
					return GetSubjectDistanceDescription();
				case ExifDirectory.TAG_METERING_MODE:
					return GetMeteringModeDescription();
				case ExifDirectory.TAG_FLASH:
					return GetFlashDescription();
				case ExifDirectory.TAG_FOCAL_LENGTH:
					return GetFocalLengthDescription();
				case ExifDirectory.TAG_COLOR_SPACE:
					return GetColorSpaceDescription();
				case ExifDirectory.TAG_EXIF_IMAGE_WIDTH:
					return GetExifImageWidthDescription();
				case ExifDirectory.TAG_EXIF_IMAGE_HEIGHT:
					return GetExifImageHeightDescription();
				case ExifDirectory.TAG_FOCAL_PLANE_UNIT:
					return GetFocalPlaneResolutionUnitDescription();
				case ExifDirectory.TAG_FOCAL_PLANE_X_RES:
					return GetFocalPlaneXResolutionDescription();
				case ExifDirectory.TAG_FOCAL_PLANE_Y_RES:
					return GetFocalPlaneYResolutionDescription();
				case ExifDirectory.TAG_THUMBNAIL_IMAGE_WIDTH:
					return GetThumbnailImageWidthDescription();
				case ExifDirectory.TAG_THUMBNAIL_IMAGE_HEIGHT:
					return GetThumbnailImageHeightDescription();
				case ExifDirectory.TAG_BITS_PER_SAMPLE:
					return GetBitsPerSampleDescription();
				case ExifDirectory.TAG_COMPRESSION:
					return GetCompressionDescription();
				case ExifDirectory.TAG_PHOTOMETRIC_INTERPRETATION:
					return GetPhotometricInterpretationDescription();
				case ExifDirectory.TAG_ROWS_PER_STRIP:
					return GetRowsPerStripDescription();
				case ExifDirectory.TAG_STRIP_BYTE_COUNTS:
					return GetStripByteCountsDescription();
				case ExifDirectory.TAG_SAMPLES_PER_PIXEL:
					return GetSamplesPerPixelDescription();
				case ExifDirectory.TAG_PLANAR_CONFIGURATION:
					return GetPlanarConfigurationDescription();
				case ExifDirectory.TAG_YCBCR_SUBSAMPLING:
					return GetYCbCrSubsamplingDescription();
				case ExifDirectory.TAG_EXPOSURE_PROGRAM:
					return GetExposureProgramDescription();
				case ExifDirectory.TAG_APERTURE:
					return GetApertureValueDescription();
				case ExifDirectory.TAG_MAX_APERTURE:
					return GetMaxApertureValueDescription();
				case ExifDirectory.TAG_SENSING_METHOD:
					return GetSensingMethodDescription();
				case ExifDirectory.TAG_EXPOSURE_BIAS:
					return GetExposureBiasDescription();
				case ExifDirectory.TAG_FILE_SOURCE:
					return GetFileSourceDescription();
				case ExifDirectory.TAG_SCENE_TYPE:
					return GetSceneTypeDescription();
				case ExifDirectory.TAG_COMPONENTS_CONFIGURATION:
					return GetComponentConfigurationDescription();
				case ExifDirectory.TAG_EXIF_VERSION:
					return GetExifVersionDescription();
				case ExifDirectory.TAG_FLASHPIX_VERSION:
					return GetFlashPixVersionDescription();
				case ExifDirectory.TAG_REFERENCE_BLACK_WHITE:
					return GetReferenceBlackWhiteDescription();
				case ExifDirectory.TAG_ISO_EQUIVALENT:
					return GetIsoEquivalentDescription();
				case ExifDirectory.TAG_THUMBNAIL_DATA:
					return GetThumbnailDescription();
				case ExifDirectory.TAG_XP_AUTHOR:
					return GetXPAuthorDescription();
				case ExifDirectory.TAG_XP_COMMENTS:
					return GetXPCommentsDescription();
				case ExifDirectory.TAG_XP_KEYWORDS:
					return GetXPKeywordsDescription();
				case ExifDirectory.TAG_XP_SUBJECT:
					return GetXPSubjectDescription();
				case ExifDirectory.TAG_XP_TITLE:
					return GetXPTitleDescription();
                case ExifDirectory.TAG_SUBFILE_TYPE:
                    return GetNewSubfileTypeDescription();
                case ExifDirectory.TAG_NEW_SUBFILE_TYPE :
                    return GetNewSubfileTypeDescription();
                case ExifDirectory.TAG_THRESHOLDING :
                    return GetThresholdingDescription();
                case ExifDirectory.TAG_FILL_ORDER :
                    return GetFillOrderDescription();
                case ExifDirectory.TAG_SUBJECT_DISTANCE_RANGE :
                    return GetSubjectDistanceRangeDescription();
                case ExifDirectory.TAG_SHARPNESS :
                    return GetSharpnessDescription();
                case ExifDirectory.TAG_SATURATION :
                    return GetSaturationDescription();
                case ExifDirectory.TAG_CONTRAST:
                    return GetContrastDescription();
                case ExifDirectory.TAG_GAIN_CONTROL:
                    return GetGainControlDescription();
                case ExifDirectory.TAG_SCENE_CAPTURE_TYPE:
                    return GetSceneCaptureTypeDescription();
                case ExifDirectory.TAG_FOCAL_LENGTH_IN_35MM_FILM:
                    return Get35mmFilmEquivFocalLengthDescription();
                case ExifDirectory.TAG_DIGITAL_ZOOM_RATIO:
                    return GetDigitalZoomRatioDescription();
                case ExifDirectory.TAG_WHITE_BALANCE_MODE :
                    return GetWhiteBalanceModeDescription();
                case ExifDirectory.TAG_EXPOSURE_MODE:
                    return GetExposureModeDescription();

				default :
					return base.directory.GetString(aTagType);
			}
		}

        /// <summary>
        /// Gets the custom rendered description.
        /// </summary>
        /// <returns>The custom rendered description</returns>
        private string GetCustomRenderedDescription()
        {
            if (!base.directory.ContainsTag(ExifDirectory.TAG_CUSTOM_RENDERED))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(ExifDirectory.TAG_CUSTOM_RENDERED);
            switch (lcVal)
            {
                case 0:
                    return BUNDLE["NORMAL_PROCESS"];
                case 1:
                    return BUNDLE["CUSTOM_PROCESS"];
                default:
                    return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }

        /// <summary>
        /// Gets the exposure mode description.
        /// </summary>
        /// <returns>The exposure mode description</returns>
        private string GetExposureModeDescription()
        {
            if (!base.directory.ContainsTag(ExifDirectory.TAG_EXPOSURE_MODE))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(ExifDirectory.TAG_EXPOSURE_MODE);
            switch (lcVal)
            {
                case 0:
                    return BUNDLE["AUTO_EXPOSURE"];
                case 1:
                    return BUNDLE["MANUAL_EXPOSURE"];
                case 2:
                    return BUNDLE["AUTO_BRACKET"];
                default:
                    return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }


        /// <summary>
        /// Gets the white balance mode description.
        /// </summary>
        /// <returns>The white balance mode description</returns>
        private string GetWhiteBalanceModeDescription()
        {
            if (!base.directory.ContainsTag(ExifDirectory.TAG_WHITE_BALANCE_MODE))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(ExifDirectory.TAG_WHITE_BALANCE_MODE);
            switch (lcVal)
            {
                case 0:
                    return BUNDLE["AUTO_WHITE_BALANCE"];
                case 1:
                    return BUNDLE["MANUAL_WHITE_BALANCE"];
                default:
                    return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }


        /// <summary>
        /// Gets the digital zoom ratio description.
        /// </summary>
        /// <returns>The digital zoom ratio description</returns>
        private string GetDigitalZoomRatioDescription()
        {
            if (!base.directory.ContainsTag(ExifDirectory.TAG_DIGITAL_ZOOM_RATIO))
            {
                return null;
            }
            Rational lcRational = base.directory.GetRational(ExifDirectory.TAG_DIGITAL_ZOOM_RATIO);
            if (lcRational.GetNumerator() == 0)
            {
                return BUNDLE["DIGITAL_ZOOM_NOT_USED"];
            }

            return (lcRational.DoubleValue()).ToString();
        }

        /// <summary>
        /// Gets the 35mm film equivalent focal length description.
        /// </summary>
        /// <returns>The 35mm film equivalent focal length description</returns>
        private string Get35mmFilmEquivFocalLengthDescription()
        {
            if (!base.directory.ContainsTag(ExifDirectory.TAG_FOCAL_LENGTH_IN_35MM_FILM))
            {
                return null;
            }
            int lcEquivalentFocalLength = base.directory.GetInt(ExifDirectory.TAG_FOCAL_LENGTH_IN_35MM_FILM);

            if (lcEquivalentFocalLength == 0)
            {
                return BUNDLE["UNKNOWN", lcEquivalentFocalLength.ToString()];
            }
            return BUNDLE["DISTANCE_MM", lcEquivalentFocalLength.ToString()];
        }

        /// <summary>
        /// Gets the scene capture type description.
        /// </summary>
        /// <returns>The scene capture type description</returns>
        private string GetSceneCaptureTypeDescription()
        {
            if (!base.directory.ContainsTag(ExifDirectory.TAG_SCENE_CAPTURE_TYPE))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(ExifDirectory.TAG_SCENE_CAPTURE_TYPE);
            switch (lcVal)
            {
                case 0:
                    return BUNDLE["STANDARD"];
                case 1:
                    return BUNDLE["LANDSCAPE"];
                case 2:
                    return BUNDLE["PORTRAIT"];
                case 3:
                    return BUNDLE["NIGHT_SCENE"];
                default:
                    return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }

        /// <summary>
        /// Gets the gain control description.
        /// </summary>
        /// <returns>The gain control description</returns>
        private string GetGainControlDescription()
        {
            if (!base.directory.ContainsTag(ExifDirectory.TAG_GAIN_CONTROL))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(ExifDirectory.TAG_GAIN_CONTROL);
            switch (lcVal)
            {
                case 0:
                    return BUNDLE["NONE"];
                case 1:
                    return BUNDLE["LOW_GAIN_UP"];
                case 2:
                    return BUNDLE["LOW_GAIN_DOWN"];
                case 3:
                    return BUNDLE["HIGH_GAIN_UP"];
                case 4:
                    return BUNDLE["HIGH_GAIN_DOWN"];
                default:
                    return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }


        /// <summary>
        /// Gets the contrast description.
        /// </summary>
        /// <returns>The constrast description</returns>
        private string GetContrastDescription()
        {
            if (!base.directory.ContainsTag(ExifDirectory.TAG_CONTRAST))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(ExifDirectory.TAG_CONTRAST);
            switch (lcVal)
            {
                case 0:
                    return BUNDLE["NONE"];
                case 1:
                    return BUNDLE["SOFT"];
                case 2:
                    return BUNDLE["HARD"];
                default:
                    return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }

        /// <summary>
        /// Gets the subfile type description.
        /// </summary>
        /// <returns>The subfile type description</returns>
        private string getSubfileTypeDescription()
        {
            if (!base.directory.ContainsTag(ExifDirectory.TAG_SUBFILE_TYPE))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(ExifDirectory.TAG_SUBFILE_TYPE);
            switch (lcVal)
            {
                case 1: return BUNDLE["FULL_RESOLUTION_IMAGE"];
                case 2: return BUNDLE["REDUCED_RESOLUTION_IMAGE"];
                case 3: return BUNDLE["SINGLE_PAGE_OF_MULTI_PAGE_IMAGE"];
                default:
                    return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }

        /// <summary>
        /// Gets the new subfile type description.
        /// </summary>
        /// <returns>The new subfile type description</returns>
        private string GetNewSubfileTypeDescription()
        {
            if (!base.directory.ContainsTag(ExifDirectory.TAG_NEW_SUBFILE_TYPE))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(ExifDirectory.TAG_NEW_SUBFILE_TYPE);
            switch (lcVal)
            {
                case 1: return BUNDLE["FULL_RESOLUTION_IMAGE"];
                case 2: return BUNDLE["REDUCED_RESOLUTION_IMAGE"];
                case 3: return BUNDLE["SINGLE_PAGE_OF_MULTI_PAGE_REDUCED_RESOLUTION_IMAGE"];
                case 4: return BUNDLE["TRANSPARENCY_MASK"];
                case 5: return BUNDLE["TRANSPARENCY_MASK_OF_REDUCED_RESOLUTION_IMAGE"];
                case 6: return BUNDLE["TRANSPARENCY_MASK_OF_MULTI_PAGE_IMAGE"];
                case 7: return BUNDLE["TRANSPARENCY_MASK_OF_REDUCED_RESOLUTION_MULTI_PAGE_IMAGE"];
                default:
                    return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }

        /// <summary>
        /// Gets the new thresholding description.
        /// </summary>
        /// <returns>The thresholding description</returns>
        private string GetThresholdingDescription()
        {
            if (!base.directory.ContainsTag(ExifDirectory.TAG_THRESHOLDING))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(ExifDirectory.TAG_THRESHOLDING);
            switch (lcVal)
            {
                case 1: return BUNDLE["NO_DITHERING_OR_HALFTONING"];
                case 2: return BUNDLE["ORDERED_DITHER_OR_HALFTONE"];
                case 3: return BUNDLE["RANDOMIZED_DITHER"];
                default:
                    return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }

        /// <summary>
        /// Gets the fill order description.
        /// </summary>
        /// <returns>The fill order description</returns>
        private string GetFillOrderDescription()
        {
            if (!base.directory.ContainsTag(ExifDirectory.TAG_FILL_ORDER))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(ExifDirectory.TAG_FILL_ORDER);
            switch (lcVal)
            {
                case 1: return BUNDLE["NORMAL"];
                case 2: return BUNDLE["REVERSED"];
                default:
                    return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }

        /// <summary>
        /// Gets the subject distance range description.
        /// </summary>
        /// <returns>The subject distance range description</returns>
        private string GetSubjectDistanceRangeDescription()
        {
            if (!base.directory.ContainsTag(ExifDirectory.TAG_SUBJECT_DISTANCE_RANGE))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(ExifDirectory.TAG_SUBJECT_DISTANCE_RANGE);
            switch (lcVal)
            {
                case 1:
                    return BUNDLE["MACRO"];
                case 2:
                    return BUNDLE["CLOSE_VIEW"];
                case 3:
                    return BUNDLE["DISTANT_VIEW"];
                default:
                    return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }

        /// <summary>
        /// Gets the sharpness description.
        /// </summary>
        /// <returns>The sharpness description</returns>
        private string GetSharpnessDescription()
        {
            if (!base.directory.ContainsTag(ExifDirectory.TAG_SHARPNESS))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(ExifDirectory.TAG_SHARPNESS);
            switch (lcVal)
            {
                case 0:
                    return BUNDLE["NONE"];
                case 1:
                    return BUNDLE["LOW"];
                case 2:
                    return BUNDLE["HARD"];
                default:
                    return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }

        /// <summary>
        /// Gets the saturation description.
        /// </summary>
        /// <returns>The saturation description</returns>
        private string GetSaturationDescription()
        {
            if (!base.directory.ContainsTag(ExifDirectory.TAG_SATURATION))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(ExifDirectory.TAG_SATURATION);
            switch (lcVal)
            {
                case 0:
                    return BUNDLE["NONE"];
                case 1:
                    return BUNDLE["LOW_SATURATION"];
                case 2:
                    return BUNDLE["HIGH_SATURATION"];
                default:
                    return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }



		/// <summary>
		/// Returns the Thumbnail Description. 
		/// </summary>
		/// <returns>the Thumbnail Description.</returns>
		private string GetThumbnailDescription() 
		{
            if (!base.directory.ContainsTag(ExifDirectory.TAG_THUMBNAIL_DATA))
            {
                return null;
            }
			int[] lcThumbnailBytes =
				base.directory.GetIntArray(ExifDirectory.TAG_THUMBNAIL_DATA);
			return BUNDLE["THUMBNAIL_BYTES", lcThumbnailBytes.Length.ToString()];
		}

		/// <summary>
		/// Returns the Iso Equivalent Description. 
		/// </summary>
		/// <returns>the Iso Equivalent Description.</returns>
		private string GetIsoEquivalentDescription() 
		{
            if (!base.directory.ContainsTag(ExifDirectory.TAG_ISO_EQUIVALENT))
            {
                return null;
            }
			int lcIsoEquiv = base.directory.GetInt(ExifDirectory.TAG_ISO_EQUIVALENT);
			if (lcIsoEquiv < 50) 
			{
				lcIsoEquiv *= 200;
			}
			return lcIsoEquiv.ToString();
		}

		/// <summary>
		/// Returns the Reference Black White Description. 
		/// </summary>
		/// <returns>the Reference Black White Description.</returns>
		private string GetReferenceBlackWhiteDescription()
		{
            if (!base.directory.ContainsTag(ExifDirectory.TAG_REFERENCE_BLACK_WHITE))
            {
                return null;
            }
			int[] lcInts =
				base.directory.GetIntArray(ExifDirectory.TAG_REFERENCE_BLACK_WHITE);

			string[] lcSPos = new string[] {lcInts[0].ToString(), lcInts[1].ToString(),lcInts[2].ToString(),lcInts[3].ToString(),lcInts[4].ToString(),lcInts[5].ToString()};
			return BUNDLE["POS",lcSPos];
		}

		/// <summary>
		/// Returns the Exif Version Description. 
		/// </summary>
		/// <returns>the Exif Version Description.</returns>
		private string GetExifVersionDescription() 
		{
            if (!base.directory.ContainsTag(ExifDirectory.TAG_EXIF_VERSION))
            {
                return null;
            }
			int[] lcInts = base.directory.GetIntArray(ExifDirectory.TAG_EXIF_VERSION);
			return ExifDescriptor.ConvertBytesToVersionString(lcInts);
		}

		/// <summary>
		/// Returns the Flash Pix Version Description. 
		/// </summary>
		/// <returns>the Flash Pix Version Description.</returns>
		private string GetFlashPixVersionDescription() 
		{
            if (!base.directory.ContainsTag(ExifDirectory.TAG_FLASHPIX_VERSION))
            {
                return null;
            }
			int[] lcInts = base.directory.GetIntArray(ExifDirectory.TAG_FLASHPIX_VERSION);
			return ExifDescriptor.ConvertBytesToVersionString(lcInts);
		}

		/// <summary>
		/// Returns the Scene Type Description. 
		/// </summary>
		/// <returns>the Scene Type Description.</returns>
		private string GetSceneTypeDescription() 
		{
            if (!base.directory.ContainsTag(ExifDirectory.TAG_SCENE_TYPE))
            {
                return null;
            }
			int lcSceneType = base.directory.GetInt(ExifDirectory.TAG_SCENE_TYPE);
			if (lcSceneType == 1) 
			{
				return BUNDLE["DIRECTLY_PHOTOGRAPHED_IMAGE"];
			} 
			return BUNDLE["UNKNOWN", lcSceneType.ToString()];
		}

		/// <summary>
		/// Returns the File Source Description. 
		/// </summary>
		/// <returns>the File Source Description.</returns>
		private string GetFileSourceDescription() 
		{
            if (!base.directory.ContainsTag(ExifDirectory.TAG_FILE_SOURCE))
            {
                return null;
            }
			int lcFileSource = base.directory.GetInt(ExifDirectory.TAG_FILE_SOURCE);
			if (lcFileSource == 3) 
			{
				return BUNDLE["DIGITAL_STILL_CAMERA"];
			} 
			return BUNDLE["UNKNOWN", lcFileSource.ToString()];
		}

		/// <summary>
		/// Returns the Exposure Bias Description. 
		/// </summary>
		/// <returns>the Exposure Bias Description.</returns>
		private string GetExposureBiasDescription() 
		{
            if (!base.directory.ContainsTag(ExifDirectory.TAG_EXPOSURE_BIAS))
            {
                return null;
            }
			Rational lcExposureBias =
				base.directory.GetRational(ExifDirectory.TAG_EXPOSURE_BIAS);
			return lcExposureBias.ToSimpleString(true);
		}

		/// <summary>
		/// Returns the Max Aperture Value Description. 
		/// </summary>
		/// <returns>the Max Aperture Value Description.</returns>
		private string GetMaxApertureValueDescription() 
		{
            if (!base.directory.ContainsTag(ExifDirectory.TAG_MAX_APERTURE))
            {
                return null;
            }
			double lcApertureApex =
				base.directory.GetDouble(ExifDirectory.TAG_MAX_APERTURE);
			double lcRootTwo = Math.Sqrt(2);
			double lcFStop = Math.Pow(lcRootTwo, lcApertureApex);
			return BUNDLE["APERTURE", lcFStop.ToString("0.#")];
		}

		/// <summary>
		/// Returns the Aperture Value Description. 
		/// </summary>
		/// <returns>the Aperture Value Description.</returns>
		private string GetApertureValueDescription() 
		{
            if (!base.directory.ContainsTag(ExifDirectory.TAG_APERTURE))
            {
                return null;
            }
			double lcApertureApex = base.directory.GetDouble(ExifDirectory.TAG_APERTURE);
			double lcRootTwo = Math.Sqrt(2);
			double lcFStop = Math.Pow(lcRootTwo, lcApertureApex);
			return BUNDLE["APERTURE", lcFStop.ToString("0.#")];
		}

		/// <summary>
		/// Returns the Exposure Program Description. 
		/// </summary>
		/// <returns>the Exposure Program Description.</returns>
		private string GetExposureProgramDescription() 
		{
            if (!base.directory.ContainsTag(ExifDirectory.TAG_EXPOSURE_PROGRAM))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(ExifDirectory.TAG_EXPOSURE_PROGRAM);
			switch (lcVal) 
			{
				case 1 :
					return BUNDLE["MANUAL_CONTROL"];
				case 2 :
					return BUNDLE["PROGRAM_NORMAL"];
				case 3 :
					return BUNDLE["APERTURE_PRIORITY"];
				case 4 :
					return BUNDLE["SHUTTER_PRIORITY"];
				case 5 :
					return BUNDLE["PROGRAM_CREATIVE"];
				case 6 :
					return BUNDLE["PROGRAM_ACTION"];
				case 7 :
					return BUNDLE["PORTRAIT_MODE"];
				case 8 :
					return BUNDLE["LANDSCAPE_MODE"];
				default :
					return BUNDLE["UNKNOWN_PROGRAM", lcVal.ToString()];
			}
		}

		/// <summary>
		/// Returns the YCbCr Subsampling Description. 
		/// </summary>
		/// <returns>the YCbCr Subsampling Description.</returns>
		private string GetYCbCrSubsamplingDescription() 
		{
            if (!base.directory.ContainsTag(ExifDirectory.TAG_YCBCR_SUBSAMPLING))
            {
                return null;
            }
			int[] lcPositions =
				base.directory.GetIntArray(ExifDirectory.TAG_YCBCR_SUBSAMPLING);
			if (lcPositions[0] == 2 && lcPositions[1] == 1) 
			{
				return BUNDLE["YCBCR_422"];
			} 
			else if (lcPositions[0] == 2 && lcPositions[1] == 2) 
			{
				return BUNDLE["YCBCR_420"];
			} 
    		return BUNDLE["UNKNOWN"];
		}

		/// <summary>
		/// Returns the Planar Configuration Description. 
		/// </summary>
		/// <returns>the Planar Configuration Description.</returns>
		private string GetPlanarConfigurationDescription()
		{
            if (!base.directory.ContainsTag(ExifDirectory.TAG_PLANAR_CONFIGURATION))
            {
                return null;
            }
			// When image format is no compression YCbCr, this aValue shows byte aligns of YCbCr
			// data. If aValue is '1', Y/Cb/Cr aValue is chunky format, contiguous for each subsampling
			// pixel. If aValue is '2', Y/Cb/Cr aValue is separated and stored to Y plane/Cb plane/Cr
			// plane format.

			switch (base.directory.GetInt(ExifDirectory.TAG_PLANAR_CONFIGURATION)) 
			{
				case 1 :
					return BUNDLE["CHUNKY"];
				case 2 :
					return BUNDLE["SEPARATE"];
				default :
					return BUNDLE["UNKNOWN_CONFIGURATION"];
			}
		}

		/// <summary>
		/// Returns the Samples Per Pixel Description. 
		/// </summary>
		/// <returns>the Samples Per Pixel Description.</returns>
		private string GetSamplesPerPixelDescription() 
		{
            if (!base.directory.ContainsTag(ExifDirectory.TAG_SAMPLES_PER_PIXEL))
            {
                return null;
            }
			return BUNDLE["SAMPLES_PIXEL", base.directory.GetString(ExifDirectory.TAG_SAMPLES_PER_PIXEL)];
		}

		/// <summary>
		/// Returns the Rows Per Strip Description. 
		/// </summary>
		/// <returns>the Rows Per Strip Description.</returns>
		private string GetRowsPerStripDescription() 
		{
            if (!base.directory.ContainsTag(ExifDirectory.TAG_ROWS_PER_STRIP))
            {
                return null;
            }
			return BUNDLE["ROWS_STRIP", base.directory.GetString(ExifDirectory.TAG_ROWS_PER_STRIP)];
		}

		/// <summary>
		/// Returns the Strip Byte Counts Description. 
		/// </summary>
		/// <returns>the Strip Byte Counts Description.</returns>
		private string GetStripByteCountsDescription() 
		{
            if (!base.directory.ContainsTag(ExifDirectory.TAG_STRIP_BYTE_COUNTS))
            {
                return null;
            }
			return BUNDLE["BYTES", base.directory.GetString(ExifDirectory.TAG_STRIP_BYTE_COUNTS)];
		}

		/// <summary>
		/// Returns the Photometric Interpretation Description. 
		/// </summary>
		/// <returns>the Photometric Interpretation Description.</returns>
		private string GetPhotometricInterpretationDescription()
		{
            if (!base.directory
                .ContainsTag(ExifDirectory.TAG_PHOTOMETRIC_INTERPRETATION))
            {
                return null;
            }
			// Shows the color space of the image data components. '1' means monochrome,
			// '2' means RGB, '6' means YCbCr.
            switch (base.directory
                .GetInt(ExifDirectory.TAG_PHOTOMETRIC_INTERPRETATION))
            {
                case 0: return BUNDLE["WHITE_IS_ZERO"];
                case 1: return BUNDLE["BLACK_IS_ZERO"];
                case 2: return BUNDLE["RGB"];
                case 3: return BUNDLE["RGB_PALETTE"];
                case 4: return BUNDLE["TRANSPARENCY_MASK"];
                case 5: return BUNDLE["CMYK"];
                case 6: return BUNDLE["YCBCR"];
                case 8: return BUNDLE["CIELAB"];
                case 9: return BUNDLE["ICCLAB"];
                case 10: return BUNDLE["ITULAB"];
                case 32803: return BUNDLE["COLOR_FILTER_ARRAY"];
                case 32844: return BUNDLE["PIXAR_LOGL"];
                case 32845: return BUNDLE["PIXAR_LOGLUV"];
                case 32892: return BUNDLE["LINEAR_RAW"];
                default: return BUNDLE["UNKNOWN_COLOR_SPACE"];
            }
		}

		/// <summary>
		/// Returns the Compression Description. 
		/// </summary>
		/// <returns>the Compression Description.</returns>
		private string GetCompressionDescription() 
		{
            if (!base.directory.ContainsTag(ExifDirectory.TAG_COMPRESSION))
            {
                return null;
            }
			// '1' means no compression, '6' means JPEG compression.
            switch (base.directory.GetInt(ExifDirectory.TAG_COMPRESSION))
            {
                case 1: return BUNDLE["UNCOMPRESSED"];
                case 2: return BUNDLE["CCITT_1D"];
                case 3: return BUNDLE["T4_GROUP_3_FAC"];
                case 4: return BUNDLE["T6_GROUP_4_FAC"];
                case 5: return BUNDLE["LZW"];
                case 6: return BUNDLE["JPEG_OLD_STYLE"];
                case 7: return BUNDLE["JPEG"];
                case 8: return BUNDLE["ADOBE_DEFLATE"];
                case 9: return BUNDLE["JBIG_B_W"];
                case 10: return BUNDLE["JBIG_COLOR"];
                case 32766: return BUNDLE["NEXT"];
                case 32771: return BUNDLE["CCIRLEW"];
                case 32773: return BUNDLE["PACKBITS"];
                case 32809: return BUNDLE["THUNDERSCA"];
                case 32895: return BUNDLE["IT8CTPAD"];
                case 32896: return BUNDLE["IT8LW"];
                case 32897: return BUNDLE["IT8MP"];
                case 32898: return BUNDLE["IT8BL"];
                case 32908: return BUNDLE["PIXARFILM"];
                case 32909: return BUNDLE["PIXARLOG"];
                case 32946: return BUNDLE["DEFLATE"];
                case 32947: return BUNDLE["DCS"];
                case 32661: return BUNDLE["JBIG"];
                case 32676: return BUNDLE["SGILOG"];
                case 32677: return BUNDLE["SGILOG24"];
                case 32712: return BUNDLE["JPEG_2000"];
                case 32713: return BUNDLE["NIKON_NEF_COMPRESSED"];
                default: return BUNDLE["UNKNOWN_COMPRESSION"];
            }
		}

		/// <summary>
		/// Returns the Bits Per Sample Description. 
		/// </summary>
		/// <returns>the Bits Per Sample Description.</returns>
		private string GetBitsPerSampleDescription() 
		{
            if (!base.directory.ContainsTag(ExifDirectory.TAG_BITS_PER_SAMPLE))
            {
                return null;
            }
			return BUNDLE["BITS_COMPONENT_PIXEL",base.directory.GetString(ExifDirectory.TAG_BITS_PER_SAMPLE)];
		}

		/// <summary>
		/// Returns the Thumbnail Image Width Description. 
		/// </summary>
		/// <returns>the Thumbnail Image Width Description.</returns>
		private string GetThumbnailImageWidthDescription() 
		{
            if (!base.directory.ContainsTag(ExifDirectory.TAG_THUMBNAIL_IMAGE_WIDTH))
            {
                return null;
            }
			return BUNDLE["PIXELS", base.directory.GetString(ExifDirectory.TAG_THUMBNAIL_IMAGE_WIDTH)];
		}

		/// <summary>
		/// Returns the Thumbnail Image Height Description. 
		/// </summary>
		/// <returns>the Thumbnail Image Height Description.</returns>
		private string GetThumbnailImageHeightDescription() 
		{
            if (!base.directory.ContainsTag(ExifDirectory.TAG_THUMBNAIL_IMAGE_HEIGHT))
            {
                return null;
            }
			return BUNDLE["PIXELS", base.directory.GetString(ExifDirectory.TAG_THUMBNAIL_IMAGE_HEIGHT)];
		}

		/// <summary>
		/// Returns the Focal Plane X Resolution Description. 
		/// </summary>
		/// <returns>the Focal Plane X Resolution Description.</returns>
		private string GetFocalPlaneXResolutionDescription()
		{
            if (!base.directory.ContainsTag(ExifDirectory.TAG_FOCAL_PLANE_X_RES))
            {
                return null;
            }
			Rational lcRational =
				base.directory.GetRational(ExifDirectory.TAG_FOCAL_PLANE_X_RES);
			return BUNDLE["FOCAL_PLANE", lcRational.GetReciprocal().ToSimpleString(allowDecimalRepresentationOfRationals),
			GetFocalPlaneResolutionUnitDescription().ToLower()];
		}

		/// <summary>
		/// Returns the Focal Plane Y Resolution Description. 
		/// </summary>
		/// <returns>the Focal Plane Y Resolution Description.</returns>
		private string GetFocalPlaneYResolutionDescription()
		{
            if (!base.directory.ContainsTag(ExifDirectory.TAG_FOCAL_PLANE_Y_RES))
            {
                return null;
            }
			Rational lcRational =
				base.directory.GetRational(ExifDirectory.TAG_FOCAL_PLANE_Y_RES);
			return BUNDLE["FOCAL_PLANE", lcRational.GetReciprocal().ToSimpleString(allowDecimalRepresentationOfRationals),
				GetFocalPlaneResolutionUnitDescription().ToLower()];
		}

		/// <summary>
		/// Returns the Focal Plane Resolution Unit Description. 
		/// </summary>
		/// <returns>the Focal Plane Resolution Unit Description.</returns>
		private string GetFocalPlaneResolutionUnitDescription()
		{
            if (!base.directory.ContainsTag(ExifDirectory.TAG_FOCAL_PLANE_UNIT))
            {
                return null;
            }
			// Unit of FocalPlaneXResoluton/FocalPlaneYResolution. '1' means no-unit,
			// '2' inch, '3' centimeter.
			switch (base.directory.GetInt(ExifDirectory.TAG_FOCAL_PLANE_UNIT)) 
			{
				case 1 :
					return BUNDLE["NO_UNIT"];
				case 2 :
					return BUNDLE["INCHES"];
				case 3 :
					return BUNDLE["CM"];
				default :
					return "";
			}
		}

		/// <summary>
		/// Returns the Exif Image Width Description. 
		/// </summary>
		/// <returns>the Exif Image Width Description.</returns>
		private string GetExifImageWidthDescription() 
		{
            if (!base.directory.ContainsTag(ExifDirectory.TAG_EXIF_IMAGE_WIDTH))
            {
                return null;
            }
			return BUNDLE["PIXELS", base.directory.GetInt(ExifDirectory.TAG_EXIF_IMAGE_WIDTH).ToString()];
		}

		/// <summary>
		/// Returns the Exif Image Height Description. 
		/// </summary>
		/// <returns>the Exif Image Height Description.</returns>
		private string GetExifImageHeightDescription() 
		{
            if (!base.directory.ContainsTag(ExifDirectory.TAG_EXIF_IMAGE_HEIGHT))
            {
                return null;
            }
			return BUNDLE["PIXELS", base.directory.GetInt(ExifDirectory.TAG_EXIF_IMAGE_HEIGHT).ToString()];
		}

		/// <summary>
		/// Returns the Color Space Description. 
		/// </summary>
		/// <returns>the Color Space Description.</returns>
		private string GetColorSpaceDescription() 
		{
            if (!base.directory.ContainsTag(ExifDirectory.TAG_COLOR_SPACE))
            {
                return null;
            }
			int lcColorSpace = base.directory.GetInt(ExifDirectory.TAG_COLOR_SPACE);
            switch (lcColorSpace)
            {
                case 1: return BUNDLE["SRGB"];
                case 65535: return BUNDLE["UNDEFINED"];
                default: return BUNDLE["UNKNOWN"];
            }
		}

		/// <summary>
		/// Returns the Focal Length Description. 
		/// </summary>
		/// <returns>the Focal Length Description.</returns>
		private string GetFocalLengthDescription() 
		{
            if (!base.directory.ContainsTag(ExifDirectory.TAG_FOCAL_LENGTH))
            {
                return null;
            }
			Rational lcFocalLength =
				base.directory.GetRational(ExifDirectory.TAG_FOCAL_LENGTH);
			return BUNDLE["DISTANCE_MM", (lcFocalLength.DoubleValue()).ToString("0.0##")];
		}

		/// <summary>
		/// Returns the Flash Description. 
		/// </summary>
		/// <returns>the Flash Description.</returns>
		private string GetFlashDescription() 
		{
            if (!base.directory.ContainsTag(ExifDirectory.TAG_FLASH))
            {
                return null;
            }          
            int lcVal = base.directory.GetInt(ExifDirectory.TAG_FLASH);
            StringBuilder sb = new StringBuilder();

            if ((lcVal & 0x1) != 0)
            {
                sb.Append(BUNDLE["FLASH_FIRED"]);
            }
            else
            {
                sb.Append(BUNDLE["FLASH_DID_NOT_FIRE"]);
            }

            // check if we're able to detect a return, before we mention it
            if ((lcVal & 0x4) != 0)
            {
                sb.Append(", ");
                if ((lcVal & 0x2) != 0)
                {
                    sb.Append(BUNDLE["RETURN_DETECTED"]);
                }
                else
                {
                    sb.Append(BUNDLE["RETURN_NOT_DETECTED"]);
                }
            }

            if ((lcVal & 0x10) != 0)
            {
                sb.Append(", ").Append(BUNDLE["AUTO"]);
            }

            if ((lcVal & 0x40) != 0)
            {
                sb.Append(", ").Append(BUNDLE["RED_EYE_REDUCTION"]);
            }

            return sb.ToString();
		}

		/// <summary>
		/// Returns the light source Description. 
		/// </summary>
        /// <returns>the light source Description.</returns>
		private string GetLightSourceDescription() 
		{
            if (!base.directory.ContainsTag(ExifDirectory.TAG_LIGHT_SOURCE))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(ExifDirectory.TAG_LIGHT_SOURCE);
			switch (lcVal) 
			{
				case 0 :
					return BUNDLE["UNKNOWN"];
				case 1 :
					return BUNDLE["DAYLIGHT"];
				case 2 :
					return BUNDLE["FLUORESCENT"];
				case 3 :
					return BUNDLE["TUNGSTEN"];
				case 10 :
					return BUNDLE["FLASH"];
				case 17 :
					return BUNDLE["STANDARD_LIGHT"];
				case 18 :
					return BUNDLE["STANDARD_LIGHT_B"];
				case 19 :
					return BUNDLE["STANDARD_LIGHT_C"];
				case 20 :
					return BUNDLE["D55"];
				case 21 :
					return BUNDLE["D65"];
				case 22 :
					return BUNDLE["D75"];
				case 255 :
					return BUNDLE["OTHER"];
				default :
					return BUNDLE["UNKNOWN", lcVal.ToString()];
			}
		}

		/// <summary>
		/// Returns the Metering Mode Description. 
		/// </summary>
		/// <returns>the Metering Mode Description.</returns>
		private string GetMeteringModeDescription() 
		{
            if (!base.directory.ContainsTag(ExifDirectory.TAG_METERING_MODE))
            {
                return null;
            }
			// '0' means unknown, '1' average, '2' center weighted average, '3' spot
			// '4' multi-spot, '5' multi-segment, '6' partial, '255' other
			int lcMeteringMode = base.directory.GetInt(ExifDirectory.TAG_METERING_MODE);
			switch (lcMeteringMode) 
			{
				case 0 :
					return BUNDLE["UNKNOWN"];
				case 1 :
					return BUNDLE["AVERAGE"];
				case 2 :
					return BUNDLE["CENTER_WEIGHTED_AVERAGE"];
				case 3 :
					return BUNDLE["SPOT"];
				case 4 :
					return BUNDLE["MULTI_SPOT"];
				case 5 :
					return BUNDLE["MULTI_SEGMENT"];
				case 6 :
					return BUNDLE["PARTIAL"];
				case 255 :
					return BUNDLE["OTHER"];
				default :
					return "";
			}
		}

		/// <summary>
		/// Returns the Subject Distance Description. 
		/// </summary>
		/// <returns>the Subject Distance Description.</returns>
		private string GetSubjectDistanceDescription() 
		{
            if (!base.directory.ContainsTag(ExifDirectory.TAG_SUBJECT_DISTANCE))
            {
                return null;
            }
			Rational lcDistance =
				base.directory.GetRational(ExifDirectory.TAG_SUBJECT_DISTANCE);
			return BUNDLE["METRES", (lcDistance.DoubleValue()).ToString("0.0##")];
		}

		/// <summary>
		/// Returns the Compression Level Description. 
		/// </summary>
		/// <returns>the Compression Level Description.</returns>
		private string GetCompressionLevelDescription() 
		{
            if (!base.directory.ContainsTag(ExifDirectory.TAG_COMPRESSION_LEVEL))
            {
                return null;
            }
			Rational lcCompressionRatio =
				base.directory.GetRational(ExifDirectory.TAG_COMPRESSION_LEVEL);
			string lcRatio =
				lcCompressionRatio.ToSimpleString(
				allowDecimalRepresentationOfRationals);
			if (lcCompressionRatio.IsInteger() && lcCompressionRatio.IntValue() == 1) 
			{
				return BUNDLE["BIT_PIXEL", lcRatio];
			} 
    		return BUNDLE["BITS_PIXEL", lcRatio];
		}

		/// <summary>
		/// Returns the Thumbnail Length Description. 
		/// </summary>
		/// <returns>the Thumbnail Length Description.</returns>
		private string GetThumbnailLengthDescription() 
		{
            if (!base.directory.ContainsTag(ExifDirectory.TAG_THUMBNAIL_LENGTH))
            {
                return null;
            }
			return BUNDLE["BYTES", base.directory.GetString(ExifDirectory.TAG_THUMBNAIL_LENGTH)];
		}

		/// <summary>
		/// Returns the Thumbnail OffSet Description. 
		/// </summary>
		/// <returns>the Thumbnail OffSet Description.</returns>
		private string GetThumbnailOffSetDescription() 
		{
            if (!base.directory.ContainsTag(ExifDirectory.TAG_THUMBNAIL_OFFSET))
            {
                return null;
            }
			return BUNDLE["BYTES", base.directory.GetString(ExifDirectory.TAG_THUMBNAIL_OFFSET)];
		}

		/// <summary>
		/// Returns the Y Resolution Description. 
		/// </summary>
		/// <returns>the Y Resolution Description.</returns>
		private string GetYResolutionDescription() 
		{
            if (!base.directory.ContainsTag(ExifDirectory.TAG_Y_RESOLUTION))
            {
                return null;
            }
			Rational lcResolution =
				base.directory.GetRational(ExifDirectory.TAG_Y_RESOLUTION);
			return BUNDLE["DOTS_PER", lcResolution.ToSimpleString(allowDecimalRepresentationOfRationals),GetResolutionDescription().ToLower()];
		}

		/// <summary>
		/// Returns the X Resolution Description. 
		/// </summary>
		/// <returns>the X Resolution Description.</returns>
		private string GetXResolutionDescription() 
		{
            if (!base.directory.ContainsTag(ExifDirectory.TAG_X_RESOLUTION))
            {
                return null;
            }
			Rational lcResolution =
				base.directory.GetRational(ExifDirectory.TAG_X_RESOLUTION);
			return BUNDLE["DOTS_PER", lcResolution.ToSimpleString(allowDecimalRepresentationOfRationals),GetResolutionDescription().ToLower()];
		}

		/// <summary>
		/// Returns the Exposure Time Description. 
		/// </summary>
		/// <returns>the Exposure Time Description.</returns>
		private string GetExposureTimeDescription() 
		{
            if (!base.directory.ContainsTag(ExifDirectory.TAG_EXPOSURE_TIME))
            {
                return null;
            }
			return BUNDLE["SEC", base.directory.GetString(ExifDirectory.TAG_EXPOSURE_TIME)];
		}

		/// <summary>
		/// Returns the Shutter Speed Description. 
		/// </summary>
		/// <returns>the Shutter Speed Description.</returns>
		private string GetShutterSpeedDescription() 
		{
            // I believe this method to now be stable, but am leaving some 
            // alternative snippets of code in here, to assist anyone who'lcStr 
            // looking into this (given that I don't have a public CVS).
            if (!base.directory.ContainsTag(ExifDirectory.TAG_SHUTTER_SPEED))
            {
                return null;
            }
            // Thanks to Mark Edwards for spotting and patching a bug in the calculation of this
            // description (spotted bug using a Canon EOS 300D)
            // thanks also to Gli Blr for spotting this bug
            float lcApexValue = base.directory.GetFloat(ExifDirectory.TAG_SHUTTER_SPEED);
            if (lcApexValue <= 1)
            {
                float lcApexPower = (float)(1 / (Math.Exp(lcApexValue * Math.Log(2))));
                long lcApexPower10 = (long)Math.Round((double)lcApexPower * 10.0);
                float lcFApexPower = (float)lcApexPower10 / 10.0f;
                return BUNDLE["SHUTTER_SPEED_SEC", lcFApexPower.ToString()];
            }
            else
            {
                int apexPower = (int)((Math.Exp(lcApexValue * Math.Log(2))));
                return BUNDLE["SHUTTER_SPEED", apexPower.ToString()];
            }

            // This alternative implementation offered by Bill Richards
            // TODO determine which is the correct / more-correct implementation
            // double apexValue = base.directory.GetDouble(ExifDirectory.TAG_SHUTTER_SPEED);
            // double apexPower = Math.Pow(2.0, apexValue);

            // StringBuilder sb = new StringBuilder();
            // if (apexPower > 1) {
            // apexPower = Math.Floor(apexPower);
            // }
            // if (apexPower < 1) {
            // sb.Append((int)Math.Round(1/apexPower));
            // } else {
            // sb.Append("1/");
            // sb.Append((int)apexPower);
            // }
            // sb.Append(" sec");
            // return sb.ToString();
		}

		/// <summary>
		/// Returns the F Number Description. 
		/// </summary>
		/// <returns>the F Number Description.</returns>
		private string GetFNumberDescription() 
		{
            if (!base.directory.ContainsTag(ExifDirectory.TAG_FNUMBER))
            {
                return null;
            }
			Rational lcFNumber = base.directory.GetRational(ExifDirectory.TAG_FNUMBER);
			return BUNDLE["APERTURE", lcFNumber.DoubleValue().ToString("0.#")];
		}

		/// <summary>
		/// Returns the YCbCr Positioning Description. 
		/// </summary>
		/// <returns>the YCbCr Positioning Description.</returns>
		private string GetYCbCrPositioningDescription() 
		{
            if (!base.directory.ContainsTag(ExifDirectory.TAG_YCBCR_POSITIONING))
            {
                return null;
            }
			int lcYCbCrPosition =
				base.directory.GetInt(ExifDirectory.TAG_YCBCR_POSITIONING);
			switch (lcYCbCrPosition) 
			{
				case 1 :
					return BUNDLE["CENTER_OF_PIXEL_ARRAY"];
				case 2 :
					return BUNDLE["DATUM_POINT"];
				default :
					return lcYCbCrPosition.ToString();
			}
		}

		/// <summary>
		/// Returns the Orientation Description. 
		/// </summary>
		/// <returns>the Orientation Description.</returns>
		private string GetOrientationDescription() 
		{
            if (!base.directory.ContainsTag(ExifDirectory.TAG_ORIENTATION))
            {
                return null;
            }
			int lcOrientation = base.directory.GetInt(ExifDirectory.TAG_ORIENTATION);
			switch (lcOrientation) 
			{
				case 1 :
					return BUNDLE["TOP_LEFT_SIDE"];
				case 2 :
					return BUNDLE["TOP_RIGHT_SIDE"];
				case 3 :
					return BUNDLE["BOTTOM_RIGHT_SIDE"];
				case 4 :
					return BUNDLE["BOTTOM_LEFT_SIDE"];
				case 5 :
					return BUNDLE["LEFT_SIDE_TOP"];
				case 6 :
					return BUNDLE["RIGHT_SIDE_TOP"];
				case 7 :
					return BUNDLE["RIGHT_SIDE_BOTTOM"];
				case 8 :
					return BUNDLE["LEFT_SIDE_BOTTOM"];
				default :
					return lcOrientation.ToString();
			}
		}

		/// <summary>
		/// Returns the Resolution Description. 
		/// </summary>
		/// <returns>the Resolution Description.</returns>
		private string GetResolutionDescription() 
		{
            if (!base.directory.ContainsTag(ExifDirectory.TAG_RESOLUTION_UNIT))
            {
                return "";
            }
			// '1' means no-unit, '2' means inch, '3' means centimeter. Default aValue is '2'(inch)
			int lcResolutionUnit = base.directory.GetInt(ExifDirectory.TAG_RESOLUTION_UNIT);
			switch (lcResolutionUnit) 
			{
				case 1 :
					return BUNDLE["NO_UNIT"];
				case 2 :
					return BUNDLE["INCHES"];
				case 3 :
					return BUNDLE["CM"];
				default :
					return "";
			}
		}

		/// <summary>
		/// Returns the Sensing Method Description. 
		/// </summary>
		/// <returns>the Sensing Method Description.</returns>
		private string GetSensingMethodDescription() 
		{
            if (!base.directory.ContainsTag(ExifDirectory.TAG_SENSING_METHOD))
            {
                return null;
            }
			// '1' Not defined, '2' One-chip color area sensor, '3' Two-chip color area sensor
			// '4' Three-chip color area sensor, '5' Color sequential area sensor
			// '7' Trilinear sensor '8' Color sequential linear sensor,  'Other' reserved
			int lcSensingMethod = base.directory.GetInt(ExifDirectory.TAG_SENSING_METHOD);
			switch (lcSensingMethod) 
			{
				case 1 :
					return BUNDLE["NOT_DEFINED"];
				case 2 :
					return BUNDLE["ONE_CHIP_COLOR"];
				case 3 :
					return BUNDLE["TWO_CHIP_COLOR"];
				case 4 :
					return BUNDLE["THREE_CHIP_COLOR"];
				case 5 :
					return BUNDLE["COLOR_SEQUENTIAL"];
				case 7 :
					return BUNDLE["TRILINEAR_SENSOR"];
				case 8 :
					return BUNDLE["COLOR_SEQUENTIAL_LINEAR"];
				default :
					return "";
			}
		}

		/// <summary>
		/// Returns the XP author description. 
		/// </summary>
		/// <returns>the XP author description.</returns>
		private string GetXPAuthorDescription() 
		{
            if (!base.directory.ContainsTag(ExifDirectory.TAG_XP_AUTHOR))
            {
                return null;
            }
			return Utils.Decode(base.directory.GetByteArray(ExifDirectory.TAG_XP_AUTHOR), true);
		}

		/// <summary>
		/// Returns the XP comments description. 
		/// </summary>
		/// <returns>the XP comments description.</returns>
		private string GetXPCommentsDescription()
		{
            if (!base.directory.ContainsTag(ExifDirectory.TAG_XP_COMMENTS))
            {
                return null;
            }
			return Utils.Decode(base.directory.GetByteArray(ExifDirectory.TAG_XP_COMMENTS), true);
        } 

		/// <summary>
		/// Returns the XP keywords description. 
		/// </summary>
		/// <returns>the XP keywords description.</returns>
		private string  GetXPKeywordsDescription()
		{
            if (!base.directory.ContainsTag(ExifDirectory.TAG_XP_KEYWORDS))
            {
                return null;
            }
			return Utils.Decode(base.directory.GetByteArray(ExifDirectory.TAG_XP_KEYWORDS), true);
		} 

		/// <summary>
		/// Returns the XP subject description. 
		/// </summary>
		/// <returns>the XP subject description.</returns>
		private string  GetXPSubjectDescription()
		{
            if (!base.directory.ContainsTag(ExifDirectory.TAG_XP_SUBJECT))
            {
                return null;
            }
			return Utils.Decode(base.directory.GetByteArray(ExifDirectory.TAG_XP_SUBJECT), true);
		} 

		/// <summary>
		/// Returns the XP title description. 
		/// </summary>
		/// <returns>the XP title description.</returns>
		private string  GetXPTitleDescription() 
		{
            if (!base.directory.ContainsTag(ExifDirectory.TAG_XP_TITLE))
            {
                return null;
            }
			return Utils.Decode(base.directory.GetByteArray(ExifDirectory.TAG_XP_TITLE), true);
		}


		/// <summary>
		/// Returns the Component Configuration Description. 
		/// </summary>
		/// <returns>the Component Configuration Description.</returns>
		private string GetComponentConfigurationDescription()
		{
            if (!base.directory.ContainsTag(ExifDirectory.TAG_COMPONENTS_CONFIGURATION))
            {
                return null;
            }
			int[] lcComponents =
				base.directory.GetIntArray(ExifDirectory.TAG_COMPONENTS_CONFIGURATION);
			string[] lcComponentStrings = { "", "Y", "Cb", "Cr", "R", "G", "B" };
			StringBuilder lcComponentConfig = new StringBuilder();
			for (int i = 0; i < Math.Min(4, lcComponents.Length); i++) 
			{
				int lcId = lcComponents[i];
				if (lcId > 0 && lcId < lcComponentStrings.Length) 
				{
					lcComponentConfig.Append(lcComponentStrings[lcId]);
				}
			}
			return lcComponentConfig.ToString();
		}

		/// <summary>
		/// Takes a series of 4 bytes from the specified offSet, and converts these to a 
		/// well-known version number, where possible.  For example, (hex) 30 32 31 30 == 2.10).
		/// </summary>
        /// <param name="someComponents">the four version values</param>
		/// <returns>the version as a string of form 2.10</returns>
		public static string ConvertBytesToVersionString(int[] someComponents) 
		{
			StringBuilder lcVersion = new StringBuilder();
			for (int i = 0; i < 4 && i < someComponents.Length; i++) 
			{
                // In order to avoid strange characters in some version (like Nikon)
                if (someComponents[i] > 31)
                {
                    if (i == 2)
                    {
                        lcVersion.Append('.');
                    }
                    string digit = ((char)someComponents[i]).ToString();
                    if (i == 0 && "0".Equals(digit))
                    {
                        continue;
                    }
                    lcVersion.Append(digit);
                }
			}
			return lcVersion.ToString();
		}
	}
}