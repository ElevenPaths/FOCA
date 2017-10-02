namespace com.drew.metadata.exif
{
	/// <summary>
	/// Tag descriptor for a casio camera
	/// </summary>

	public class CasioType1Descriptor : AbstractTagDescriptor 					
	{
		/// <summary>
		/// Constructor of the object
		/// </summary>
		/// <param name="aDirectory">a base.directory</param>
		public CasioType1Descriptor(AbstractDirectory aDirectory) : base(aDirectory) 
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
				case CasioType1Directory.TAG_CASIO_RECORDING_MODE:
					return this.GetRecordingModeDescription();
				case CasioType1Directory.TAG_CASIO_QUALITY:
					return this.GetQualityDescription();
				case CasioType1Directory.TAG_CASIO_FOCUSING_MODE:
					return this.GetFocusingModeDescription();
				case CasioType1Directory.TAG_CASIO_FLASH_MODE:
					return this.GetFlashModeDescription();
				case CasioType1Directory.TAG_CASIO_FLASH_INTENSITY:
					return this.GetFlashIntensityDescription();
				case CasioType1Directory.TAG_CASIO_OBJECT_DISTANCE:
					return this.GetObjectDistanceDescription();
				case CasioType1Directory.TAG_CASIO_WHITE_BALANCE:
					return this.GetWhiteBalanceDescription();
				case CasioType1Directory.TAG_CASIO_DIGITAL_ZOOM:
					return this.GetDigitalZoomDescription();
				case CasioType1Directory.TAG_CASIO_SHARPNESS:
					return this.GetSharpnessDescription();
				case CasioType1Directory.TAG_CASIO_CONTRAST:
					return this.GetContrastDescription();
				case CasioType1Directory.TAG_CASIO_SATURATION:
					return this.GetSaturationDescription();
				case CasioType1Directory.TAG_CASIO_CCD_SENSITIVITY:
					return GetCcdSensitivityDescription();
				default :
					return base.directory.GetString(aTagType);
			}
		}

		/// <summary>
		/// Returns the Ccd Sensitivity Description. 
		/// </summary>
		/// <returns>the Ccd Sensitivity Description.</returns>
		private string GetCcdSensitivityDescription() 
		{
            if (!base.directory
                .ContainsTag(CasioType1Directory.TAG_CASIO_CCD_SENSITIVITY))
            {
                return null;
            }
			int lcVal =
				base.directory.GetInt(
				CasioType1Directory.TAG_CASIO_CCD_SENSITIVITY);
			switch (lcVal) 
			{
					// these four for QV3000
				case 64 :
					return BUNDLE["NORMAL"];
				case 125 :
					return BUNDLE["CCD_P_1"];
				case 250 :
					return BUNDLE["CCD_P_2"];
				case 244 :
					return BUNDLE["CCD_P_3"];
					// these two for QV8000/2000
				case 80 :
					return BUNDLE["NORMAL"];
				case 100 :
					return BUNDLE["HIGH"];
				default :
					return BUNDLE["UNKNOWN", lcVal.ToString()];
			}
		}

		/// <summary>
		/// Returns the saturation Description. 
		/// </summary>
		/// <returns>the saturation Description.</returns>
		private string GetSaturationDescription() 
		{
            if (!base.directory
                .ContainsTag(CasioType1Directory.TAG_CASIO_SATURATION))
            {
                return null;
            }
			int lcVal =
				base.directory.GetInt(CasioType1Directory.TAG_CASIO_SATURATION);
			switch (lcVal) 
			{
				case 0 :
					return BUNDLE["NORMAL"];
				case 1 :
					return BUNDLE["LOW"];
				case 2 :
					return BUNDLE["HIGH"];
				default :
					return BUNDLE["UNKNOWN", lcVal.ToString()];
			}
		}

		/// <summary>
		/// Returns the contrast Description. 
		/// </summary>
		/// <returns>the contrast Description.</returns>
		private string GetContrastDescription() 
		{
            if (!base.directory
                .ContainsTag(CasioType1Directory.TAG_CASIO_CONTRAST))
            {
                return null;
            }
			int lcVal =
				base.directory.GetInt(CasioType1Directory.TAG_CASIO_CONTRAST);
			switch (lcVal) 
			{
				case 0 :
					return BUNDLE["NORMAL"];
				case 1 :
					return BUNDLE["LOW"];
				case 2 :
					return BUNDLE["HIGH"];
				default :
					return BUNDLE["UNKNOWN", lcVal.ToString()];
			}
		}

		/// <summary>
		/// Returns the sharpness Description. 
		/// </summary>
		/// <returns>the sharpness Description.</returns>
		private string GetSharpnessDescription() 
		{
            if (!base.directory
                .ContainsTag(CasioType1Directory.TAG_CASIO_SHARPNESS))
            {
                return null;
            }
			int lcVal =
				base.directory.GetInt(CasioType1Directory.TAG_CASIO_SHARPNESS);
			switch (lcVal) 
			{
				case 0 :
					return BUNDLE["NORMAL"];
				case 1 :
					return BUNDLE["SOFT"];;
				case 2 :
					return BUNDLE["HARD"];;
				default :
					return BUNDLE["UNKNOWN", lcVal.ToString()];
			}
		}

		/// <summary>
		/// Returns the Digital Zoom Description. 
		/// </summary>
		/// <returns>the Digital Zoom Description.</returns>
		private string GetDigitalZoomDescription() 
		{
            if (!base.directory
                .ContainsTag(CasioType1Directory.TAG_CASIO_DIGITAL_ZOOM))
            {
                return null;
            }
			int lcVal =
				base.directory.GetInt(CasioType1Directory.TAG_CASIO_DIGITAL_ZOOM);
			switch (lcVal) 
			{
                case 0x10000:
                    return BUNDLE["NO_DIGITAL_ZOOM"];
                case 0x10001:
                case 0x20000:
                    return BUNDLE["DIGITAL_ZOOM", "2"];
                case 0x40000:
                    return BUNDLE["DIGITAL_ZOOM", "4"];
                default:
					return BUNDLE["UNKNOWN", lcVal.ToString()];
			}
		}

		/// <summary>
		/// Returns the White Balance Description. 
		/// </summary>
		/// <returns>the White Balance Description.</returns>
		private string GetWhiteBalanceDescription() 
		{
            if (!base.directory
                .ContainsTag(CasioType1Directory.TAG_CASIO_WHITE_BALANCE))
            {
                return null;
            }
			int lcVal =
				base.directory.GetInt(CasioType1Directory.TAG_CASIO_WHITE_BALANCE);
			switch (lcVal) 
			{
				case 1 :
					return BUNDLE["AUTO"];
				case 2 :
					return BUNDLE["TUNGSTEN"];
				case 3 :
					return BUNDLE["DAYLIGHT"];
				case 4 :
					return BUNDLE["FLUORESCENT"];
				case 5 :
					return BUNDLE["SHADE"];
				case 129 :
					return BUNDLE["MANUAL"];
				default :
					return BUNDLE["UNKNOWN", lcVal.ToString()];
			}
		}

		/// <summary>
		/// Returns the Object Distance Description. 
		/// </summary>
		/// <returns>the Object Distance Description.</returns>
		private string GetObjectDistanceDescription() 
		{
            if (!base.directory
                .ContainsTag(CasioType1Directory.TAG_CASIO_OBJECT_DISTANCE))
            {
                return null;
            }
			int lcVal =
				base.directory.GetInt(
				CasioType1Directory.TAG_CASIO_OBJECT_DISTANCE);
			return BUNDLE["DISTANCE_MM", lcVal.ToString()];
		}

		/// <summary>
		/// Returns the Flash Intensity Description. 
		/// </summary>
		/// <returns>the Flash Intensity Description.</returns>
		private string GetFlashIntensityDescription() 
		{
            if (!base.directory
                .ContainsTag(CasioType1Directory.TAG_CASIO_FLASH_INTENSITY))
            {
                return null;
            }
			int lcVal =
				base.directory.GetInt(
				CasioType1Directory.TAG_CASIO_FLASH_INTENSITY);
			switch (lcVal) 
			{
				case 11 :
					return BUNDLE["WEAK"];
				case 13 :
					return BUNDLE["NORMAL"];
				case 15 :
					return BUNDLE["STRONG"];
				default :
					return BUNDLE["UNKNOWN", lcVal.ToString()];
			}
		}

		/// <summary>
		/// Returns the Flash Mode Description. 
		/// </summary>
		/// <returns>the Flash Mode Description.</returns>
		private string GetFlashModeDescription() 
		{
            if (!base.directory
                .ContainsTag(CasioType1Directory.TAG_CASIO_FLASH_MODE))
            {
                return null;
            }
			int lcVal =
				base.directory.GetInt(CasioType1Directory.TAG_CASIO_FLASH_MODE);
			switch (lcVal) 
			{
				case 1 :
					return BUNDLE["AUTO"];
				case 2 :
					return BUNDLE["ON"];
				case 3 :
					return BUNDLE["OFF"];
				case 4 :
                    // this documented as additional value for off here:
                    // http://www.ozhiker.com/electronics/pjmt/jpeg_info/casio_mn.html
					return BUNDLE["RED_EYE_REDUCTION"];
				default :
					return BUNDLE["UNKNOWN", lcVal.ToString()];
			}
		}

		/// <summary>
		/// Returns the Focusing Mode Description. 
		/// </summary>
		/// <returns>the Focusing Mode Description.</returns>
		private string GetFocusingModeDescription() 
		{
            if (!base.directory
                .ContainsTag(CasioType1Directory.TAG_CASIO_FOCUSING_MODE))
            {
                return null;
            }
			int lcVal =
				base.directory.GetInt(CasioType1Directory.TAG_CASIO_FOCUSING_MODE);
			switch (lcVal) 
			{
				case 2 :
					return BUNDLE["MACRO"];
				case 3 :
					return BUNDLE["AUTO_FOCUS"];
				case 4 :
					return BUNDLE["MANUAL_FOCUS"];
				case 5 :
					return BUNDLE["INFINITY"];
				default :
					return BUNDLE["UNKNOWN", lcVal.ToString()];
			}
		}

		/// <summary>
		/// Returns the quality Description. 
		/// </summary>
		/// <returns>the quality Description.</returns>
		private string GetQualityDescription() 
		{
            if (!base.directory.ContainsTag(CasioType1Directory.TAG_CASIO_QUALITY))
            {
                return null;
            }
			int lcVal =
				base.directory.GetInt(CasioType1Directory.TAG_CASIO_QUALITY);
			switch (lcVal) 
			{
				case 1 :
					return BUNDLE["ECONOMY"];
				case 2 :
					return BUNDLE["NORMAL"];
				case 3 :
					return BUNDLE["FINE"];
				default :
					return BUNDLE["UNKNOWN", lcVal.ToString()];
			}
		}

		/// <summary>
		/// Returns the Focussing Mode Description. 
		/// </summary>
		/// <returns>the Focussing Mode Description.</returns>
		private string GetFocussingModeDescription() 
		{
            if (!base.directory
                .ContainsTag(CasioType1Directory.TAG_CASIO_FOCUSING_MODE))
            {
                return null;
            }
			int lcVal =
				base.directory.GetInt(CasioType1Directory.TAG_CASIO_FOCUSING_MODE);
			switch (lcVal) 
			{
				case 2 :
					return BUNDLE["MACRO"];
				case 3 :
					return BUNDLE["AUTO_FOCUS"];
				case 4 :
					return BUNDLE["MANUAL_FOCUS"];
				case 5 :
					return BUNDLE["INFINITY"];
				default :
					return BUNDLE["UNKNOWN", lcVal.ToString()];
			}
		}

		/// <summary>
		/// Returns the Recording Mode Description. 
		/// </summary>
		/// <returns>the Recording Mode Description.</returns>
		private string GetRecordingModeDescription() 
		{
            if (!base.directory
                .ContainsTag(CasioType1Directory.TAG_CASIO_RECORDING_MODE))
            {
                return null;
            }
			int lcVal =
				base.directory.GetInt(CasioType1Directory.TAG_CASIO_RECORDING_MODE);
			switch (lcVal) 
			{
				case 1 :
					return BUNDLE["SINGLE_SHUTTER"];
				case 2 :
					return BUNDLE["PANORAMA"];
				case 3 :
					return BUNDLE["NIGHT_SCENE"];
				case 4 :
					return BUNDLE["PORTRAIT"];
				case 5 :
					return BUNDLE["LANDSCAPE"];
				default :
					return BUNDLE["UNKNOWN", lcVal.ToString()];
			}
		}
	}
}