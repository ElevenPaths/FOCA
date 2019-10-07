namespace com.drew.metadata.exif
{
	/// <summary>
	/// Tag descriptor for a Canon camera
	/// </summary>

	public class CanonDescriptor : AbstractTagDescriptor 
	{
		/// <summary>
		/// Constructor of the object
		/// </summary>
        /// <param name="aDirectory">a directory</param>
		public CanonDescriptor(AbstractDirectory aDirectory) : base(aDirectory)
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
                case CanonDirectory.TAG_CANON_STATE1_MACRO_MODE:
                    return this.GetMacroModeDescription();
                case CanonDirectory.TAG_CANON_STATE1_SELF_TIMER_DELAY:
                    return this.GetSelfTimerDelayDescription();
                case CanonDirectory.TAG_CANON_STATE1_FLASH_MODE:
                    return this.GetFlashModeDescription();
                case CanonDirectory.TAG_CANON_STATE1_CONTINUOUS_DRIVE_MODE:
                    return this.GetContinuousDriveModeDescription();
                case CanonDirectory.TAG_CANON_STATE1_FOCUS_MODE_1:
                    return this.GetFocusMode1Description();
                case CanonDirectory.TAG_CANON_STATE1_IMAGE_SIZE:
                    return this.GetImageSizeDescription();
                case CanonDirectory.TAG_CANON_STATE1_EASY_SHOOTING_MODE:
                    return this.GetEasyShootingModeDescription();
                case CanonDirectory.TAG_CANON_STATE1_CONTRAST:
                    return this.GetContrastDescription();
                case CanonDirectory.TAG_CANON_STATE1_SATURATION:
                    return this.GetSaturationDescription();
                case CanonDirectory.TAG_CANON_STATE1_SHARPNESS:
                    return this.GetSharpnessDescription();
                case CanonDirectory.TAG_CANON_STATE1_ISO:
                    return this.GetIsoDescription();
                case CanonDirectory.TAG_CANON_STATE1_METERING_MODE:
                    return this.GetMeteringModeDescription();
                case CanonDirectory.TAG_CANON_STATE1_AF_POINT_SELECTED:
                    return this.GetAfPointSelectedDescription();
                case CanonDirectory.TAG_CANON_STATE1_EXPOSURE_MODE:
                    return this.GetExposureModeDescription();
                case CanonDirectory.TAG_CANON_STATE1_LONG_FOCAL_LENGTH:
                    return this.GetLongFocalLengthDescription();
                case CanonDirectory.TAG_CANON_STATE1_SHORT_FOCAL_LENGTH:
                    return this.GetShortFocalLengthDescription();
                case CanonDirectory.TAG_CANON_STATE1_FOCAL_UNITS_PER_MM:
                    return this.GetFocalUnitsPerMillimetreDescription();
                case CanonDirectory.TAG_CANON_STATE1_FLASH_DETAILS:
                    return this.GetFlashDetailsDescription();
                case CanonDirectory.TAG_CANON_STATE1_FOCUS_MODE_2:
                    return this.GetFocusMode2Description();
                case CanonDirectory.TAG_CANON_STATE2_WHITE_BALANCE:
                    return this.GetWhiteBalanceDescription();
                case CanonDirectory.TAG_CANON_STATE2_AF_POINT_USED:
                    return this.GetAfPointUsedDescription();
                case CanonDirectory.TAG_CANON_STATE2_FLASH_BIAS:
                    return this.GetFlashBiasDescription();

                case CanonDirectory.TAG_CANON_STATE1_FLASH_ACTIVITY:
                    return this.GetFlashActivityDescription();
                case CanonDirectory.TAG_CANON_STATE1_FOCUS_TYPE:
                    return this.GetFocusTypeDescription();
                case CanonDirectory.TAG_CANON_STATE1_DIGITAL_ZOOM:
                    return this.GetDigitalZoomDescription();
                case CanonDirectory.TAG_CANON_STATE1_QUALITY:
                    return this.GetQualityDescription();
                case CanonDirectory.TAG_CANON_CUSTOM_FUNCTION_LONG_EXPOSURE_NOISE_REDUCTION:
                    return this.GetLongExposureNoiseReductionDescription();
                case CanonDirectory.TAG_CANON_CUSTOM_FUNCTION_SHUTTER_AUTO_EXPOSURE_LOCK_BUTTONS:
                    return this.GetShutterAutoExposureLockButtonDescription();
                case CanonDirectory.TAG_CANON_CUSTOM_FUNCTION_MIRROR_LOCKUP:
                    return this.GetMirrorLockupDescription();
                case CanonDirectory.TAG_CANON_CUSTOM_FUNCTION_TV_AV_AND_EXPOSURE_LEVEL:
                    return this.GetTvAndAvExposureLevelDescription();
                case CanonDirectory.TAG_CANON_CUSTOM_FUNCTION_AF_ASSIST_LIGHT:
                    return this.GetAutoFocusAssistLightDescription();
                case CanonDirectory.TAG_CANON_CUSTOM_FUNCTION_SHUTTER_SPEED_IN_AV_MODE:
                    return this.GetShutterSpeedInAvModeDescription();
                case CanonDirectory.TAG_CANON_CUSTOM_FUNCTION_BRACKETTING:
                    return this.GetAutoExposureBrackettingSequenceAndAutoCancellationDescription();
                case CanonDirectory.TAG_CANON_CUSTOM_FUNCTION_SHUTTER_CURTAIN_SYNC:
                    return this.GetShutterCurtainSyncDescription();
                case CanonDirectory.TAG_CANON_CUSTOM_FUNCTION_AF_STOP:
                    return this.GetLensAutoFocusStopButtonDescription();
                case CanonDirectory.TAG_CANON_CUSTOM_FUNCTION_FILL_FLASH_REDUCTION:
                    return this.GetFillFlashReductionDescription();
                case CanonDirectory.TAG_CANON_CUSTOM_FUNCTION_MENU_BUTTON_RETURN:
                    return this.GetMenuButtonReturnPositionDescription();
                case CanonDirectory.TAG_CANON_CUSTOM_FUNCTION_SET_BUTTON_FUNCTION:
                    return this.GetSetButtonFunctionWhenShootingDescription();
                case CanonDirectory.TAG_CANON_CUSTOM_FUNCTION_SENSOR_CLEANING:
                    return this.GetSensorCleaningDescription();
                default: 
					return base.directory.GetString(aTagType);
			}
		}

        /// <summary>
        /// Returns the menu button return position Description. 
        /// </summary>
        /// <returns>the menu button return position Description.</returns>
        private string GetMenuButtonReturnPositionDescription()
        {
            if (!base.directory.ContainsTag(CanonDirectory.TAG_CANON_CUSTOM_FUNCTION_MENU_BUTTON_RETURN))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(CanonDirectory.TAG_CANON_CUSTOM_FUNCTION_MENU_BUTTON_RETURN);
            switch (lcVal)
            {
                case 0: return BUNDLE["TOP"];
                case 1: return BUNDLE["PREVIOUS_VOLATILE"];
                case 2: return BUNDLE["PREVIOUS"];
                default: return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }

        /// <summary>
        /// Returns the set button function when shooting Description. 
        /// </summary>
        /// <returns>the set button function when shooting Description.</returns>
        private string GetSetButtonFunctionWhenShootingDescription()
        {
            if (!base.directory.ContainsTag(CanonDirectory.TAG_CANON_CUSTOM_FUNCTION_SET_BUTTON_FUNCTION))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(CanonDirectory.TAG_CANON_CUSTOM_FUNCTION_SET_BUTTON_FUNCTION);
            switch (lcVal)
            {
                case 0: return BUNDLE["NOT_ASSIGNED"];
                case 1: return BUNDLE["CHANGE_QUALITY"];
                case 2: return BUNDLE["CHANGE_ISO_SPEED"];
                case 3: return BUNDLE["SELECT_PARAMETERS"];
                default: return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }

        /// <summary>
        /// Returns the sensor cleaning Description. 
        /// </summary>
        /// <returns>the sensor cleaning Description.</returns>
        private string GetSensorCleaningDescription()
        {
            if (!base.directory.ContainsTag(CanonDirectory.TAG_CANON_CUSTOM_FUNCTION_SENSOR_CLEANING))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(CanonDirectory.TAG_CANON_CUSTOM_FUNCTION_SENSOR_CLEANING);
            switch (lcVal)
            {
                case 0: return BUNDLE["DISABLED"];
                case 1: return BUNDLE["ENABLED"];
                default: return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }

        /// <summary>
        /// Returns the fill flash reduction Description. 
        /// </summary>
        /// <returns>the fill flash reduction Description.</returns>
        private string GetFillFlashReductionDescription()
        {
            if (!base.directory.ContainsTag(CanonDirectory.TAG_CANON_CUSTOM_FUNCTION_FILL_FLASH_REDUCTION))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(CanonDirectory.TAG_CANON_CUSTOM_FUNCTION_FILL_FLASH_REDUCTION);
            switch (lcVal)
            {
                case 0: return BUNDLE["ENABLED"];
                case 1: return BUNDLE["DISABLED"];
                default: return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }

        /// <summary>
        /// Returns the lens auto focus stop Description. 
        /// </summary>
        /// <returns>the lens auto focus stop Description.</returns>
        private string GetLensAutoFocusStopButtonDescription()
        {
            if (!base.directory.ContainsTag(CanonDirectory.TAG_CANON_CUSTOM_FUNCTION_AF_STOP))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(CanonDirectory.TAG_CANON_CUSTOM_FUNCTION_AF_STOP);
            switch (lcVal)
            {
                case 0: return BUNDLE["AF_STOP"];
                case 1: return BUNDLE["OPERATE_AF"];
                case 2: return BUNDLE["LOCK_AE_AND_START_TIMER"];
                default: return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }
        
        /// <summary>
        /// Returns the shutter curtain sync Description. 
        /// </summary>
        /// <returns>the shutter curtain sync Description.</returns>
        private string GetShutterCurtainSyncDescription()
        {
            if (!base.directory.ContainsTag(CanonDirectory.TAG_CANON_CUSTOM_FUNCTION_SHUTTER_CURTAIN_SYNC))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(CanonDirectory.TAG_CANON_CUSTOM_FUNCTION_SHUTTER_CURTAIN_SYNC);
            switch (lcVal)
            {
                case 0: return BUNDLE["1_CURTAIN_SYNC"];
                case 1: return BUNDLE["2_CURTAIN_SYNC"];
                default: return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }

        /// <summary>
        /// Returns the auto exposure bracketting sequence and auto cancellation Description. 
        /// </summary>
        /// <returns>the auto exposure bracketting sequence and auto cancellation Description.</returns>
        private string GetAutoExposureBrackettingSequenceAndAutoCancellationDescription()
        {
            if (!base.directory.ContainsTag(CanonDirectory.TAG_CANON_CUSTOM_FUNCTION_BRACKETTING))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(CanonDirectory.TAG_CANON_CUSTOM_FUNCTION_BRACKETTING);
            switch (lcVal)
            {
                case 0: return BUNDLE["0_M_P_ENABLED"];
                case 1: return BUNDLE["0_M_P_DISABLED"];
                case 2: return BUNDLE["M_0_P_ENABLED"];
                case 3: return BUNDLE["M_0_P_DISABLED"];
                default: return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }

        /// <summary>
        /// Returns the shutter speed in Av mode Description. 
        /// </summary>
        /// <returns>the shutter speed in Av mode Description.</returns>
        private string GetShutterSpeedInAvModeDescription()
        {
            if (!base.directory.ContainsTag(CanonDirectory.TAG_CANON_CUSTOM_FUNCTION_SHUTTER_SPEED_IN_AV_MODE))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(CanonDirectory.TAG_CANON_CUSTOM_FUNCTION_SHUTTER_SPEED_IN_AV_MODE);
            switch (lcVal)
            {
                case 0: return BUNDLE["AUTOMATIC"];
                case 1: return BUNDLE["1_200_FIXED"];
                default: return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }

        /// <summary>
        /// Returns the auto focus assist light Description. 
        /// </summary>
        /// <returns>the auto focus assist light Description.</returns>
        private string GetAutoFocusAssistLightDescription()
        {
            if (!base.directory.ContainsTag(CanonDirectory.TAG_CANON_CUSTOM_FUNCTION_AF_ASSIST_LIGHT))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(CanonDirectory.TAG_CANON_CUSTOM_FUNCTION_AF_ASSIST_LIGHT);
            switch (lcVal)
            {
                case 0: return BUNDLE["ON_AUTO"];
                case 1: return BUNDLE["OFF"];
                default: return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }

        /// <summary>
        /// Returns the Tv and Av exposure level Description. 
        /// </summary>
        /// <returns>the Tv and Av exposure level Description.</returns>
        private string GetTvAndAvExposureLevelDescription()
        {
            if (!base.directory.ContainsTag(CanonDirectory.TAG_CANON_CUSTOM_FUNCTION_TV_AV_AND_EXPOSURE_LEVEL))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(CanonDirectory.TAG_CANON_CUSTOM_FUNCTION_TV_AV_AND_EXPOSURE_LEVEL);
            switch (lcVal)
            {
                case 0:
                case 1: return BUNDLE["1_2_STOP"];
                default: return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }

        /// <summary>
        /// Returns the mirror lock up Description. 
        /// </summary>
        /// <returns>the mirror lock up Description.</returns>
        private string GetMirrorLockupDescription()
        {
            if (!base.directory.ContainsTag(CanonDirectory.TAG_CANON_CUSTOM_FUNCTION_MIRROR_LOCKUP))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(CanonDirectory.TAG_CANON_CUSTOM_FUNCTION_MIRROR_LOCKUP);
            switch (lcVal)
            {
                case 0: return BUNDLE["DISABLED"];
                case 1: return BUNDLE["ENABLED"];
                default: return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }

        /// <summary>
        /// Returns the shutter auto exposure lock button Description. 
        /// </summary>
        /// <returns>the shutter auto exposure lock button Description.</returns>
        private string GetShutterAutoExposureLockButtonDescription()
        {
            if (!base.directory.ContainsTag(CanonDirectory.TAG_CANON_CUSTOM_FUNCTION_SHUTTER_AUTO_EXPOSURE_LOCK_BUTTONS))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(CanonDirectory.TAG_CANON_CUSTOM_FUNCTION_SHUTTER_AUTO_EXPOSURE_LOCK_BUTTONS);
            switch (lcVal)
            {
                case 0: return BUNDLE["AF_AE_LOCK"];
                case 1: return BUNDLE["AE_LOCK_AF"];
                case 2: return BUNDLE["AE_AF_LOCK"];
                case 3: return BUNDLE["AE_RELEASE_AE_AF"];
                default: return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }

        /// <summary>
        /// Returns the long exposure noise reduction Description. 
        /// </summary>
        /// <returns>the long exposure noise reduction Description.</returns>
        private string GetLongExposureNoiseReductionDescription()
        {
            if (!base.directory.ContainsTag(CanonDirectory.TAG_CANON_CUSTOM_FUNCTION_LONG_EXPOSURE_NOISE_REDUCTION))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(CanonDirectory.TAG_CANON_CUSTOM_FUNCTION_LONG_EXPOSURE_NOISE_REDUCTION);
            switch (lcVal)
            {
                case 0: return BUNDLE["OFF"];
                case 1: return BUNDLE["ON"];
                default: return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }

        /// <summary>
        /// Returns the quality Description. 
        /// </summary>
        /// <returns>the quality Description.</returns>
        private string GetQualityDescription()
        {
            if (!base.directory.ContainsTag(CanonDirectory.TAG_CANON_STATE1_QUALITY))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(CanonDirectory.TAG_CANON_STATE1_QUALITY);
            switch (lcVal)
            {
                case 2:
                    return BUNDLE["NORMAL"];
                case 3:
                    return BUNDLE["FINE"];
                case 5:
                    return BUNDLE["SUPERFINE"];
                default:
                    return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }

        /// <summary>
        /// Returns the digital zoom Description. 
        /// </summary>
        /// <returns>the digital zoom Description.</returns>
        private string GetDigitalZoomDescription()
        {
            if (!base.directory.ContainsTag(CanonDirectory.TAG_CANON_STATE1_DIGITAL_ZOOM))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(CanonDirectory.TAG_CANON_STATE1_DIGITAL_ZOOM);
            switch (lcVal)
            {
                case 0:
                    return BUNDLE["NO_DIGITAL_ZOOM"];
                case 1:
                    return BUNDLE["DIGITAL_ZOOM", "2"];
                case 2:
                    return BUNDLE["DIGITAL_ZOOM", "4"];
                default:
                    return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }

        /// <summary>
        /// Returns the focus type Description. 
        /// </summary>
        /// <returns>the focus type Description.</returns>
        private string GetFocusTypeDescription()
        {
            if (!base.directory.ContainsTag(CanonDirectory.TAG_CANON_STATE1_FOCUS_TYPE))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(CanonDirectory.TAG_CANON_STATE1_FOCUS_TYPE);
            switch (lcVal)
            {
                case 0:
                    return BUNDLE["MANUAL"];
                case 1:
                case 2:
                    return BUNDLE["AUTO"];
                case 3:
                    return BUNDLE["CLOSE_UP_MACRO"];
                case 8:
                    return BUNDLE["LOCKED_PAN_MODE"];
                default:
                    return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }

        /// <summary>
        /// Returns the Flash actvity  Description. 
        /// </summary>
        /// <returns>the Flash activity Description.</returns>
        private string GetFlashActivityDescription()
        {
            if (!base.directory.ContainsTag(CanonDirectory.TAG_CANON_STATE1_FLASH_ACTIVITY))
            {
                return null;
            }
            int lcVal = base.directory.GetInt(CanonDirectory.TAG_CANON_STATE1_FLASH_ACTIVITY);
            switch (lcVal)
            {
                case 0:
                    return BUNDLE["FLASH_DID_NOT_FIRE"];
                case 1:
                    return BUNDLE["FLASH_FIRED"];
                default:
                    return BUNDLE["UNKNOWN", lcVal.ToString()];
            }
        }


		/// <summary>
		/// Returns the Flash Bias Description. 
		/// </summary>
		/// <returns>the Flash Bias Description.</returns>
		private string GetFlashBiasDescription() 
		{
            if (!base.directory
                .ContainsTag(CanonDirectory.TAG_CANON_STATE2_FLASH_BIAS))
            {
                return null;
            }
			int lcVal =
				base.directory.GetInt(
				CanonDirectory.TAG_CANON_STATE2_FLASH_BIAS);
            bool isNegative = false;
            if (lcVal > 0xF000)
            {
                isNegative = true;
                lcVal = 0xFFFF - lcVal;
                lcVal++;
            }

            // this tag is interesting in that the values returned are:
            //  0, 0.375, 0.5, 0.626, 1
            // not
            //  0, 0.33,  0.5, 0.66,  1
            return BUNDLE["FLASH_BIAS_NEW", ((isNegative) ? "-" : ""), (lcVal / 32.0).ToString()];
		}

		/// <summary>
		/// Returns Af Point Used Description. 
		/// </summary>
		/// <returns>the Af Point Used Description.</returns>
		private string GetAfPointUsedDescription()
		{
            if (!base.directory
                .ContainsTag(
                CanonDirectory.TAG_CANON_STATE2_AF_POINT_USED))
            {
                return null;
            }
			int lcVal =
				base.directory.GetInt(
				CanonDirectory.TAG_CANON_STATE2_AF_POINT_USED);
			if ((lcVal & 0x7) == 0) 
			{
				return BUNDLE["RIGHT"];
			} 
			else if ((lcVal & 0x7) == 1) 
			{
                return BUNDLE["CENTER"];
			} 
			else if ((lcVal & 0x7) == 2) 
			{
				return BUNDLE["LEFT"];
			} 
			else 
			{
				return BUNDLE["UNKNOWN", lcVal.ToString()];
			}
		}

		/// <summary>
		/// Returns White Balance Description. 
		/// </summary>
		/// <returns>the White Balance Description.</returns>
		private string GetWhiteBalanceDescription() 
		{
			if (!base.directory
				.ContainsTag(
				CanonDirectory.TAG_CANON_STATE2_WHITE_BALANCE))
				return null;
			int lcVal =
				base.directory.GetInt(
				CanonDirectory.TAG_CANON_STATE2_WHITE_BALANCE);
			switch (lcVal) 
			{
				case 0 :
					return BUNDLE["AUTO"];
				case 1 :
					return BUNDLE["SUNNY"];
				case 2 :
					return BUNDLE["CLOUDY"];
				case 3 :
					return BUNDLE["TUNGSTEN"];
				case 4 :
					return BUNDLE["FLUORESCENT"];
				case 5 :
					return BUNDLE["FLASH"];
				case 6 :
					return BUNDLE["CUSTOM"];
				default :
					return BUNDLE["UNKNOWN", lcVal.ToString()];
			}
		}

		/// <summary>
		/// Returns Focus Mode 2 description. 
		/// </summary>
		/// <returns>the Focus Mode 2 description</returns>
		private string GetFocusMode2Description() 
		{
            if (!base.directory
                .ContainsTag(CanonDirectory.TAG_CANON_STATE1_FOCUS_MODE_2))
            {
                return null;
            }
			int lcVal =
				base.directory.GetInt(
				CanonDirectory.TAG_CANON_STATE1_FOCUS_MODE_2);
			switch (lcVal) 
			{
				case 0 :
					return BUNDLE["SINGLE"];
				case 1 :
					return BUNDLE["CONTINUOUS"];
				default :
					return BUNDLE["UNKNOWN", lcVal.ToString()];
			}
		}

		/// <summary>
		/// Returns Flash Details description. 
		/// </summary>
		/// <returns>the Flash Details description</returns>
		private string GetFlashDetailsDescription()  
		{
            if (!base.directory
                .ContainsTag(
                CanonDirectory.TAG_CANON_STATE1_FLASH_DETAILS))
            {
                return null;
            }
			int lcVal =
				base.directory.GetInt(
				CanonDirectory.TAG_CANON_STATE1_FLASH_DETAILS);
			if (((lcVal << 14) & 1) > 0) 
			{
				return BUNDLE["EXTERNAL_E_TTL"];
			}
			if (((lcVal << 13) & 1) > 0) 
			{
				return BUNDLE["INTERNAL_FLASH"];
			}
			if (((lcVal << 11) & 1) > 0) 
			{
				return BUNDLE["FP_SYNC_USED"];
			}
			if (((lcVal << 4) & 1) > 0) 
			{
				return BUNDLE["FP_SYNC_ENABLED"];
			}
			return BUNDLE["UNKNOWN", lcVal.ToString()];
		}

		/// <summary>
		/// Returns Focal Units Per Millimetre description. 
		/// </summary>
		/// <returns>the Focal Units Per Millimetre description</returns>
		private string GetFocalUnitsPerMillimetreDescription() 
		{
            if (!base.directory
                .ContainsTag(
                CanonDirectory.TAG_CANON_STATE1_FOCAL_UNITS_PER_MM))
            {
                return "";
            }
			int lcVal =
				base.directory.GetInt(
				CanonDirectory.TAG_CANON_STATE1_FOCAL_UNITS_PER_MM);
			if (lcVal != 0) 
			{
				return lcVal.ToString();
			} 
			return "";
		}

		/// <summary>
		/// Returns Short Focal Length description. 
		/// </summary>
		/// <returns>the Short Focal Length description</returns>
		private string GetShortFocalLengthDescription()  
		{
            if (!base.directory
                .ContainsTag(
                CanonDirectory.TAG_CANON_STATE1_SHORT_FOCAL_LENGTH))
            {
                return null;
            }
			int lcVal =
				base.directory.GetInt(
				CanonDirectory.TAG_CANON_STATE1_SHORT_FOCAL_LENGTH);
			string units = GetFocalUnitsPerMillimetreDescription();
			return BUNDLE["FOCAL_LENGTH", lcVal.ToString(), units];
		}

		/// <summary>
		/// Returns Long Focal Length description. 
		/// </summary>
		/// <returns>the Long Focal Length description</returns>
		private string GetLongFocalLengthDescription() 
		{
            if (!base.directory
                .ContainsTag(
                CanonDirectory.TAG_CANON_STATE1_LONG_FOCAL_LENGTH))
            {
                return null;
            }
			int lcVal =
				base.directory.GetInt(
				CanonDirectory.TAG_CANON_STATE1_LONG_FOCAL_LENGTH);
			string units = GetFocalUnitsPerMillimetreDescription();
			return BUNDLE["FOCAL_LENGTH", lcVal.ToString(), units];
		}

		/// <summary>
		/// Returns Exposure Mode description. 
		/// </summary>
		/// <returns>the Exposure Mode description</returns>
		private string GetExposureModeDescription() 
		{
            if (!base.directory
                .ContainsTag(
                CanonDirectory.TAG_CANON_STATE1_EXPOSURE_MODE))
            {
                return null;
            }
			int lcVal =
				base.directory.GetInt(
				CanonDirectory.TAG_CANON_STATE1_EXPOSURE_MODE);
			switch (lcVal) 
			{
				case 0 :
					return BUNDLE["EASY_SHOOTING"];
				case 1 :
					return BUNDLE["PROGRAM"];
				case 2 :
					return BUNDLE["TV_PRIORITY"];
				case 3 :
					return BUNDLE["AV_PRIORITY"];
				case 4 :
					return BUNDLE["MANUAL"];
				case 5 :
					return BUNDLE["A_DEP"];
				default :
					return BUNDLE["UNKNOWN", lcVal.ToString()];
			}
		}

		/// <summary>
		/// Returns Af Point Selected description. 
		/// </summary>
		/// <returns>the Af Point Selected description</returns>
		private string GetAfPointSelectedDescription() 
		{
            if (!base.directory
                .ContainsTag(
                CanonDirectory.TAG_CANON_STATE1_AF_POINT_SELECTED))
            {
                return null;
            }
			int lcVal =
				base.directory.GetInt(
				CanonDirectory.TAG_CANON_STATE1_AF_POINT_SELECTED);
			switch (lcVal) 
			{
				case 0x3000 :
					return BUNDLE["NONE_MF"];
				case 0x3001 :
					return BUNDLE["AUTO_SELECTED"];
				case 0x3002 :
					return BUNDLE["RIGHT"];
				case 0x3003 :
					return BUNDLE["CENTER"];
				case 0x3004 :
					return BUNDLE["LEFT"];
				default :
					return BUNDLE["UNKNOWN", lcVal.ToString()];
			}
		}

		/// <summary>
		/// Returns Metering Mode description. 
		/// </summary>
		/// <returns>the Metering Mode description</returns>
		private string GetMeteringModeDescription() 
		{
            if (!base.directory
                .ContainsTag(
                CanonDirectory.TAG_CANON_STATE1_METERING_MODE))
            {
                return null;
            }
			int lcVal =
				base.directory.GetInt(
				CanonDirectory.TAG_CANON_STATE1_METERING_MODE);
			switch (lcVal) 
			{
				case 3 :
					return BUNDLE["EVALUATIVE"];
				case 4 :
					return BUNDLE["PARTIAL"];
				case 5 :
					return BUNDLE["CENTER_WEIGHTED"];
				default :
					return BUNDLE["UNKNOWN", lcVal.ToString()];
			}
		}

		/// <summary>
		/// Returns ISO description. 
		/// </summary>
		/// <returns>the ISO description</returns>
		private string GetIsoDescription() 
		{
            if (!base.directory
                .ContainsTag(CanonDirectory.TAG_CANON_STATE1_ISO))
            {
                return null;
            }
			int lcVal =
				base.directory.GetInt(CanonDirectory.TAG_CANON_STATE1_ISO);
			switch (lcVal) 
			{
				case 0 :
					return BUNDLE["ISO_NOT_SPECIFIED"];
				case 15 :
					return BUNDLE["AUTO"];
				case 16 :
					return BUNDLE["ISO", "50"];
				case 17 :
					return BUNDLE["ISO", "100"];
				case 18 :
					return BUNDLE["ISO", "200"];
				case 19 :
					return BUNDLE["ISO", "400"];
				default :
					return BUNDLE["UNKNOWN", lcVal.ToString()];
			}
		}

		/// <summary>
		/// Returns Sharpness description. 
		/// </summary>
		/// <returns>the Sharpness description</returns>
		private string GetSharpnessDescription() 
		{
            if (!base.directory
                .ContainsTag(CanonDirectory.TAG_CANON_STATE1_SHARPNESS))
            {
                return null;
            }
			int lcVal =
				base.directory.GetInt(
				CanonDirectory.TAG_CANON_STATE1_SHARPNESS);
			switch (lcVal) 
			{
				case 0xFFFF :
					return BUNDLE["LOW"];
				case 0x000 :
					return BUNDLE["NORMAL"];
				case 0x001 :
					return BUNDLE["HIGH"];
				default :
					return BUNDLE["UNKNOWN", lcVal.ToString()];
			}
		}

		/// <summary>
		/// Returns Saturation description. 
		/// </summary>
		/// <returns>the Saturation description</returns>
		private string GetSaturationDescription() 
		{
            if (!base.directory
                .ContainsTag(CanonDirectory.TAG_CANON_STATE1_SATURATION))
            {
                return null;
            }
			int lcVal =
				base.directory.GetInt(
				CanonDirectory.TAG_CANON_STATE1_SATURATION);
			switch (lcVal) 
			{
				case 0xFFFF :
					return BUNDLE["LOW"];
				case 0x000 :
					return BUNDLE["NORMAL"];
				case 0x001 :
					return BUNDLE["HIGH"];
				default :
					return BUNDLE["UNKNOWN", lcVal.ToString()];
			}
		}

		/// <summary>
		/// Returns Contrast description. 
		/// </summary>
		/// <returns>the Contrast description</returns>
		private string GetContrastDescription() 
		{
            if (!base.directory
                .ContainsTag(CanonDirectory.TAG_CANON_STATE1_CONTRAST))
            {
                return null;
            }
			int lcVal =
				base.directory.GetInt(
				CanonDirectory.TAG_CANON_STATE1_CONTRAST);
			switch (lcVal) 
			{
				case 0xFFFF :
					return BUNDLE["LOW"];
				case 0x000 :
					return BUNDLE["NORMAL"];
				case 0x001 :
					return BUNDLE["HIGH"];
				default :
					return BUNDLE["UNKNOWN", lcVal.ToString()];
			}
		}

		/// <summary>
		/// Returns Easy Shooting Mode description. 
		/// </summary>
		/// <returns>the Easy Shooting Mode description</returns>
		private string GetEasyShootingModeDescription() 
		{
            if (!base.directory
                .ContainsTag(
                CanonDirectory.TAG_CANON_STATE1_EASY_SHOOTING_MODE))
            {
                return null;
            }
			int lcVal =
				base.directory.GetInt(
				CanonDirectory.TAG_CANON_STATE1_EASY_SHOOTING_MODE);
			switch (lcVal) 
			{
				case 0 :
					return BUNDLE["FULL_AUTO"];
				case 1 :
					return BUNDLE["MANUAL"];
				case 2 :
					return BUNDLE["LANDSCAPE"];
				case 3 :
					return BUNDLE["FAST_SHUTTER"];
				case 4 :
					return BUNDLE["SLOW_SHUTTER"];
				case 5 :
					return BUNDLE["NIGHT"];
				case 6 :
					return BUNDLE["BLACK_AND_WHITE"];
				case 7 :
					return BUNDLE["SEPIA"];
				case 8 :
					return BUNDLE["PORTRAIT"];
				case 9 :
					return BUNDLE["SPORTS"];
				case 10 :
					return BUNDLE["MACRO_CLOSEUP"];
				case 11 :
					return BUNDLE["PAN_FOCUS"];
				default :
					return BUNDLE["UNKNOWN", lcVal.ToString()];
			}
		}

		/// <summary>
		/// Returns Image Size description. 
		/// </summary>
		/// <returns>the Image Size description</returns>
		private string GetImageSizeDescription() 
		{
            if (!base.directory
                .ContainsTag(CanonDirectory.TAG_CANON_STATE1_IMAGE_SIZE))
            {
                return null;
            }
			int lcVal =
				base.directory.GetInt(
				CanonDirectory.TAG_CANON_STATE1_IMAGE_SIZE);
			switch (lcVal) 
			{
				case 0 :
					return BUNDLE["LARGE"];
				case 1 :
					return BUNDLE["MEDIUM"];
				case 2 :
					return BUNDLE["SMALL"];
				default :
					return BUNDLE["UNKNOWN", lcVal.ToString()];
			}
		}

		/// <summary>
		/// Returns Focus Mode 1 description. 
		/// </summary>
		/// <returns>the Focus Mode 1 description</returns>
		private string GetFocusMode1Description() 
		{
            if (!base.directory
                .ContainsTag(CanonDirectory.TAG_CANON_STATE1_FOCUS_MODE_1))
            {
                return null;
            }
			int lcVal =
				base.directory.GetInt(
				CanonDirectory.TAG_CANON_STATE1_FOCUS_MODE_1);
			switch (lcVal) 
			{
				case 0 :
					return BUNDLE["ONE_SHOT"];
				case 1 :
					return BUNDLE["AI_SERVO"];
				case 2 :
					return BUNDLE["AI_FOCUS"];
				case 3 :
					return BUNDLE["MF"];
				case 4 :
					// TODO should check field 32 here (FOCUS_MODE_2)
					return BUNDLE["SINGLE"];
				case 5 :
					return BUNDLE["CONTINUOUS"];
				case 6 :
					return BUNDLE["MF"];
				default :
					return BUNDLE["UNKNOWN", lcVal.ToString()];
			}
		}

		/// <summary>
		/// Returns Continuous Drive Mode description. 
		/// </summary>
		/// <returns>the Continuous Drive Mode description</returns>
		private string GetContinuousDriveModeDescription()
		{
            if (!base.directory
                .ContainsTag(
                CanonDirectory
                .TAG_CANON_STATE1_CONTINUOUS_DRIVE_MODE))
            {
                return null;
            }
			int lcVal =
				base.directory.GetInt(
				CanonDirectory.TAG_CANON_STATE1_CONTINUOUS_DRIVE_MODE);
			switch (lcVal) 
			{
				case 0 :
					if (base.directory
						.GetInt(
						CanonDirectory
						.TAG_CANON_STATE1_SELF_TIMER_DELAY)
						== 0) 
					{
						return BUNDLE["SINGLE_SHOT"];
					} 
					else 
					{
						return BUNDLE["SINGLE_SHOT_WITH_SELF_TIMER"];
					}
				case 1 :
					return BUNDLE["CONTINUOUS"];
				default :
					return BUNDLE["UNKNOWN", lcVal.ToString()];
			}
		}

		/// <summary>
		/// Returns Flash Mode description. 
		/// </summary>
		/// <returns>the Flash Mode description</returns>
		private string GetFlashModeDescription() 
		{
            if (!base.directory
                .ContainsTag(CanonDirectory.TAG_CANON_STATE1_FLASH_MODE))
            {
                return null;
            }
			int lcVal =
				base.directory.GetInt(
				CanonDirectory.TAG_CANON_STATE1_FLASH_MODE);
			switch (lcVal) 
			{
				case 0 :
					return BUNDLE["NO_FLASH_FIRED"];
				case 1 :
					return BUNDLE["AUTO"];
				case 2 :
					return BUNDLE["ON"];
				case 3 :
					return BUNDLE["RED_EYE_REDUCTION"];
				case 4 :
					return BUNDLE["SLOW_SYNCHRO"];
				case 5 :
					return BUNDLE["AUTO_AND_RED_EYE_REDUCTION"];
				case 6 :
					return BUNDLE["ON_AND_RED_EYE_REDUCTION"];
				case 16 :
					// note: this lcVal not set on Canon D30
					return BUNDLE["EXTERNAL_FLASH"];
				default :
					return BUNDLE["UNKNOWN", lcVal.ToString()];
			}
		}

		/// <summary>
		/// Returns Self Timer Delay description. 
		/// </summary>
		/// <returns>the Self Timer Delay description</returns>
		private string GetSelfTimerDelayDescription() 
		{
            if (!base.directory
                .ContainsTag(
                CanonDirectory.TAG_CANON_STATE1_SELF_TIMER_DELAY))
            {
                return null;
            }
			int lcVal =
				base.directory.GetInt(
				CanonDirectory.TAG_CANON_STATE1_SELF_TIMER_DELAY);
			if (lcVal == 0) 
			{
				return BUNDLE["SELF_TIMER_DELAY_NOT_USED"];
			} 
			// TODO find an image that tests this calculation
			return BUNDLE["SELF_TIMER_DELAY", ((double) lcVal * 0.1d).ToString()];
		}

		/// <summary>
		/// Returns Macro Mode description. 
		/// </summary>
		/// <returns>the Macro Mode description</returns>
		private string GetMacroModeDescription() 
		{
            if (!base.directory
                .ContainsTag(CanonDirectory.TAG_CANON_STATE1_MACRO_MODE))
            {
                return null;
            }
			int lcVal =
				base.directory.GetInt(
				CanonDirectory.TAG_CANON_STATE1_MACRO_MODE);
			switch (lcVal) 
			{
                case 0:
                    return BUNDLE["OFF"];
				case 1 :
					return BUNDLE["MACRO"];
				case 2 :
					return BUNDLE["NORMAL"];
				default :
					return BUNDLE["UNKNOWN", lcVal.ToString()];
			}
		}
	}
}