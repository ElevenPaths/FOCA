using com.drew.lang;namespace com.drew.metadata.exif
{
	/// <summary>
	/// Fujifilm'str digicam added the MakerNote tag from the Year2000'str model 
	/// (e.g.Finepix1400, Finepix4700). It uses IFD format and start from ASCII character 
	/// 'FUJIFILM', and next 4 bytes(aValue 0x000c) points the offSet to first IFD entry. 
	/// Example of actual data structure is shown below.
	/// :0000: 46 55 4A 49 46 49 4C 4D-0C 00 00 00 0F 00 00 00 :0000: FUJIFILM........
	/// :0010: 07 00 04 00 00 00 30 31-33 30 00 10 02 00 08 00 :0010: ......0130......
	/// There are two big differences to the other manufacturers.
	/// - Fujifilm'str Exif data uses Motorola align, but MakerNote ignores it and uses Intel align.
	/// - The other manufacturer'str MakerNote counts the "offSet to data" from the first byte of 
	///   TIFF lcHeader (same as the other IFD), but Fujifilm counts it from the first byte of MakerNote itself.
	/// </summary>

	public class FujifilmDescriptor : AbstractTagDescriptor 
	{
		/// <summary>
		/// Constructor of the object
		/// </summary>
		/// <param name="aDirectory">a base.directory</param>
		public FujifilmDescriptor(AbstractDirectory aDirectory) : base(aDirectory)
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
			switch(tagType) 
			{
				case FujifilmDirectory.TAG_FUJIFILM_SHARPNESS :
					return GetSharpnessDescription();
				case FujifilmDirectory.TAG_FUJIFILM_WHITE_BALANCE :
					return GetWhiteBalanceDescription();
				case FujifilmDirectory.TAG_FUJIFILM_COLOR :
					return GetColorDescription();
				case FujifilmDirectory.TAG_FUJIFILM_TONE :
					return GetToneDescription();
				case FujifilmDirectory.TAG_FUJIFILM_FLASH_MODE :
					return GetFlashModeDescription();
				case FujifilmDirectory.TAG_FUJIFILM_FLASH_STRENGTH :
					return GetFlashStrengthDescription();
				case FujifilmDirectory.TAG_FUJIFILM_MACRO :
					return GetMacroDescription();
				case FujifilmDirectory.TAG_FUJIFILM_FOCUS_MODE :
					return GetFocusModeDescription();
				case FujifilmDirectory.TAG_FUJIFILM_SLOW_SYNCHRO :
					return GetSlowSyncDescription();
				case FujifilmDirectory.TAG_FUJIFILM_PICTURE_MODE :
					return GetPictureModeDescription();
				case FujifilmDirectory.TAG_FUJIFILM_CONTINUOUS_TAKING_OR_AUTO_BRACKETTING :
					return GetContinuousTakingOrAutoBrackettingDescription();
				case FujifilmDirectory.TAG_FUJIFILM_BLUR_WARNING :
					return GetBlurWarningDescription();
				case FujifilmDirectory.TAG_FUJIFILM_FOCUS_WARNING :
					return GetFocusWarningDescription();
				case FujifilmDirectory.TAG_FUJIFILM_AE_WARNING :
					return GetAutoExposureWarningDescription();
				default :
					return base.directory.GetString(tagType);
			}
		}

		/// <summary>
		/// Returns the Auto Exposure Description. 
		/// </summary>
		/// <returns>the Auto Exposure Description.</returns>
		private string GetAutoExposureWarningDescription()
		{
			if (!base.directory
				.ContainsTag(FujifilmDirectory.TAG_FUJIFILM_AE_WARNING))
				return null;
			int aValue =
				base.directory.GetInt(
				FujifilmDirectory.TAG_FUJIFILM_AE_WARNING);
			switch (aValue) 
			{
				case 0 :
					return BUNDLE["AE_GOOD"];
				case 1 :
					return BUNDLE["OVER_EXPOSED"];
				default :
					return BUNDLE["UNKNOWN", aValue.ToString()];
			}
		}

		/// <summary>
		/// Returns the Focus Warning Description. 
		/// </summary>
		/// <returns>the Focus Warning Description.</returns>
		private string GetFocusWarningDescription()  
		{
			if (!base.directory
				.ContainsTag(FujifilmDirectory.TAG_FUJIFILM_FOCUS_WARNING))
				return null;
			int aValue =
				base.directory.GetInt(
				FujifilmDirectory.TAG_FUJIFILM_FOCUS_WARNING);
			switch (aValue) 
			{
				case 0 :
					return BUNDLE["AUTO_FOCUS_GOOD"];
				case 1 :
					return BUNDLE["OUT_OF_FOCUS"];
				default :
					return BUNDLE["UNKNOWN", aValue.ToString()];
			}
		}

		/// <summary>
		/// Returns the Blur Warning Description. 
		/// </summary>
		/// <returns>the Blur Warning Description.</returns>
		private string GetBlurWarningDescription()  
		{
			if (!base.directory
				.ContainsTag(FujifilmDirectory.TAG_FUJIFILM_BLUR_WARNING))
				return null;
			int aValue =
				base.directory.GetInt(
				FujifilmDirectory.TAG_FUJIFILM_BLUR_WARNING);
			switch (aValue) 
			{
				case 0 :
					return BUNDLE["NO_BLUR_WARNING"];
				case 1 :
					return BUNDLE["BLUR_WARNING"];
				default :
					return BUNDLE["UNKNOWN", aValue.ToString()];
			}
		}

		/// <summary>
		/// Returns the Continuous Taking Or AutoBracketting Description. 
		/// </summary>
		/// <returns>the Continuous Taking Or AutoBracketting Description.</returns>
		private string GetContinuousTakingOrAutoBrackettingDescription()
		{
			if (!base.directory
				.ContainsTag(
				FujifilmDirectory
				.TAG_FUJIFILM_CONTINUOUS_TAKING_OR_AUTO_BRACKETTING))
				return null;
			int aValue =
				base.directory.GetInt(
				FujifilmDirectory
				.TAG_FUJIFILM_CONTINUOUS_TAKING_OR_AUTO_BRACKETTING);
			switch (aValue) 
			{
				case 0 :
					return BUNDLE["OFF"];
				case 1 :
					return BUNDLE["ON"];
				default :
					return BUNDLE["UNKNOWN", aValue.ToString()];
			}
		}

		/// <summary>
		/// Returns the Picture Mode Description. 
		/// </summary>
		/// <returns>the Picture Mode Description.</returns>
		private string GetPictureModeDescription()  
		{
			if (!base.directory
				.ContainsTag(FujifilmDirectory.TAG_FUJIFILM_PICTURE_MODE))
				return null;
			int aValue =
				base.directory.GetInt(
				FujifilmDirectory.TAG_FUJIFILM_PICTURE_MODE);
			switch (aValue) 
			{
				case 0 :
					return BUNDLE["AUTO"];
				case 1 :
					return BUNDLE["PORTRAIT_SCENE"];
				case 2 :
					return BUNDLE["LANDSCAPE_SCENE"];
				case 4 :
					return BUNDLE["SPORTS_SCENE"];
				case 5 :
					return BUNDLE["NIGHT_SCENE"];
				case 6 :
					return BUNDLE["PROGRAM_AE"];
				case 256 :
					return BUNDLE["APERTURE_PRIORITY_AE"];
				case 512 :
					return BUNDLE["SHUTTER_PRIORITY_AE"];
				case 768 :
					return BUNDLE["MANUAL_EXPOSURE"];
				default :
					return BUNDLE["UNKNOWN", aValue.ToString()];
			}
		}

		/// <summary>
		/// Returns the Slow Sync Description. 
		/// </summary>
		/// <returns>the Slow Sync Description.</returns>
		private string GetSlowSyncDescription()  
		{
			if (!base.directory
				.ContainsTag(FujifilmDirectory.TAG_FUJIFILM_SLOW_SYNCHRO))
				return null;
			int aValue =
				base.directory.GetInt(
				FujifilmDirectory.TAG_FUJIFILM_SLOW_SYNCHRO);
			switch (aValue) 
			{
				case 0 :
					return BUNDLE["OFF"];
				case 1 :
					return BUNDLE["ON"];
				default :
					return BUNDLE["UNKNOWN", aValue.ToString()];
			}
		}

		/// <summary>
		/// Returns the Focus Mode Description. 
		/// </summary>
		/// <returns>the Focus Mode Description.</returns>
		private string GetFocusModeDescription()  
		{
			if (!base.directory
				.ContainsTag(FujifilmDirectory.TAG_FUJIFILM_FOCUS_MODE))
				return null;
			int aValue =
				base.directory.GetInt(
				FujifilmDirectory.TAG_FUJIFILM_FOCUS_MODE);
			switch (aValue) 
			{
				case 0 :
					return BUNDLE["AUTO_FOCUS"];
				case 1 :
					return BUNDLE["MANUAL_FOCUS"];
				default :
					return BUNDLE["UNKNOWN", aValue.ToString()];
			}
		}

		/// <summary>
		/// Returns the Macro Description. 
		/// </summary>
		/// <returns>the Macro Description.</returns>
		private string GetMacroDescription()  
		{
			if (!base.directory
				.ContainsTag(FujifilmDirectory.TAG_FUJIFILM_MACRO))
				return null;
			int aValue =
				base.directory.GetInt(FujifilmDirectory.TAG_FUJIFILM_MACRO);
			switch (aValue) 
			{
				case 0 :
					return BUNDLE["OFF"];
				case 1 :
					return BUNDLE["ON"];
				default :
					return BUNDLE["UNKNOWN", aValue.ToString()];
			}
		}

		/// <summary>
		/// Returns the Flash Strength Description. 
		/// </summary>
		/// <returns>the Flash Strength Description.</returns>
		private string GetFlashStrengthDescription()  
		{
			if (!base.directory
				.ContainsTag(
				FujifilmDirectory.TAG_FUJIFILM_FLASH_STRENGTH))
				return null;
			Rational aValue =
				base.directory.GetRational(
				FujifilmDirectory.TAG_FUJIFILM_FLASH_STRENGTH);
			return BUNDLE["FLASH_STRENGTH", aValue.ToSimpleString(false)];
		}

		/// <summary>
		/// Returns the Flash Mode Description. 
		/// </summary>
		/// <returns>the Flash Mode Description.</returns>
		private string GetFlashModeDescription()  
		{
			if (!base.directory
				.ContainsTag(FujifilmDirectory.TAG_FUJIFILM_FLASH_MODE))
				return null;
			int aValue =
				base.directory.GetInt(
				FujifilmDirectory.TAG_FUJIFILM_FLASH_MODE);
			switch (aValue) 
			{
				case 0 :
					return BUNDLE["AUTO"];
				case 1 :
					return BUNDLE["ON"];
				case 2 :
					return BUNDLE["OFF"];
				case 3 :
					return BUNDLE["RED_EYE_REDUCTION"];
				default :
					return BUNDLE["UNKNOWN", aValue.ToString()];
			}
		}

		/// <summary>
		/// Returns the Tone Description. 
		/// </summary>
		/// <returns>the Tone Description.</returns>
		private string GetToneDescription()  
		{
			if (!base.directory
				.ContainsTag(FujifilmDirectory.TAG_FUJIFILM_TONE))
				return null;
			int aValue =
				base.directory.GetInt(FujifilmDirectory.TAG_FUJIFILM_TONE);
			switch (aValue) 
			{
				case 0 :
					return BUNDLE["NORMAL_STD"];
				case 256 :
					return BUNDLE["HIGH_HARD"];
				case 512 :
					return BUNDLE["LOW_ORG"];
				default :
					return BUNDLE["UNKNOWN", aValue.ToString()];
			}
		}

		/// <summary>
		/// Returns the Color Description. 
		/// </summary>
		/// <returns>the Color Description.</returns>
		private string GetColorDescription()  
		{
			if (!base.directory
				.ContainsTag(FujifilmDirectory.TAG_FUJIFILM_COLOR))
				return null;
			int aValue =
				base.directory.GetInt(FujifilmDirectory.TAG_FUJIFILM_COLOR);
			switch (aValue) 
			{
				case 0 :
					return BUNDLE["NORMAL_STD"];
				case 256 :
					return BUNDLE["HIGH"];
				case 512 :
					return BUNDLE["LOW_ORG"];
				default :
					return BUNDLE["UNKNOWN", aValue.ToString()];
			}
		}

		/// <summary>
		/// Returns the White Balance Description. 
		/// </summary>
		/// <returns>the White Balance Description.</returns>
		private string GetWhiteBalanceDescription()  
		{
			if (!base.directory
				.ContainsTag(FujifilmDirectory.TAG_FUJIFILM_WHITE_BALANCE))
				return null;
			int aValue =
				base.directory.GetInt(
				FujifilmDirectory.TAG_FUJIFILM_WHITE_BALANCE);
			switch (aValue) 
			{
				case 0 :
					return BUNDLE["AUTO"];
				case 256 :
					return BUNDLE["DAYLIGHT"];
				case 512 :
					return BUNDLE["CLOUDY"];
				case 768 :
					return BUNDLE["DAYLIGHTCOLOR_FLUORESCENCE"];
				case 769 :
					return BUNDLE["DAYWHITECOLOR_FLUORESCENCE"];
				case 770 :
					return BUNDLE["WHITE_FLUORESCENCE"];
				case 1024 :
					return BUNDLE["INCANDENSCENSE"];
				case 3840 :
					return BUNDLE["CUSTOM_WHITE_BALANCE"];
				default :
					return BUNDLE["UNKNOWN", aValue.ToString()];
			}
		}

		/// <summary>
		/// Returns the Sharpness Description. 
		/// </summary>
		/// <returns>the Sharpness Description.</returns>
		private string GetSharpnessDescription()  
		{
			if (!base.directory
				.ContainsTag(FujifilmDirectory.TAG_FUJIFILM_SHARPNESS))
				return null;
			int aValue =
				base.directory.GetInt(
				FujifilmDirectory.TAG_FUJIFILM_SHARPNESS);
			switch (aValue) 
			{
				case 1 :
				case 2 :
					return BUNDLE["SOFT"];
				case 3 :
					return BUNDLE["NORMAL"];
				case 4 :
				case 5 :
					return BUNDLE["HARD"];
				default :
					return BUNDLE["UNKNOWN", aValue.ToString()];
			}
		}
	}
}