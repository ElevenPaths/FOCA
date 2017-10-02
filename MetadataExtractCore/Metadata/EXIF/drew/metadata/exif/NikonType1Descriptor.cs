using com.drew.lang;

namespace com.drew.metadata.exif
{
	/// <summary>
	/// There are 3 formats of Nikon'str MakerNote. MakerNote of E700/E800/E900/E900S/E910/E950 starts 
	/// from ASCII string "Nikon". Data format is the same as IFD, but it starts from offSet 0x08. 
	/// This is the same as Olympus except start string. Example of actual data structure is shown below.
	/// :0000: 4E 69 6B 6F 6E 00 01 00-05 00 02 00 02 00 06 00 Nikon...........
	/// :0010: 00 00 EC 02 00 00 03 00-03 00 01 00 00 00 06 00 ................
	/// </summary>

	public class NikonType1Descriptor : AbstractTagDescriptor 
	{

		/// <summary>
		/// Constructor of the object
		/// </summary>
		/// <param name="aDirectory">a base.directory</param>
		public NikonType1Descriptor(AbstractDirectory aDirectory) : base(aDirectory)
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
				case NikonType1Directory.TAG_NIKON_TYPE1_QUALITY :
					return GetQualityDescription();
				case NikonType1Directory.TAG_NIKON_TYPE1_COLOR_MODE :
					return GetColorModeDescription();
				case NikonType1Directory.TAG_NIKON_TYPE1_IMAGE_ADJUSTMENT :
					return GetImageAdjustmentDescription();
				case NikonType1Directory.TAG_NIKON_TYPE1_CCD_SENSITIVITY :
					return GetCcdSensitivityDescription();
				case NikonType1Directory.TAG_NIKON_TYPE1_WHITE_BALANCE :
					return GetWhiteBalanceDescription();
				case NikonType1Directory.TAG_NIKON_TYPE1_FOCUS :
					return GetFocusDescription();
				case NikonType1Directory.TAG_NIKON_TYPE1_DIGITAL_ZOOM :
					return GetDigitalZoomDescription();
				case NikonType1Directory.TAG_NIKON_TYPE1_CONVERTER :
					return GetConverterDescription();
				default :
					return base.directory.GetString(tagType);
			}
		}

		/// <summary>
		/// Returns the Converter Description. 
		/// </summary>
		/// <returns>the Converter Description.</returns>
		private string GetConverterDescription()  
		{
            if (!base.directory
                .ContainsTag(
                NikonType1Directory.TAG_NIKON_TYPE1_CONVERTER))
            {
                return null;
            }
			int aValue =
				base.directory.GetInt(
				NikonType1Directory.TAG_NIKON_TYPE1_CONVERTER);
			switch (aValue) 
			{
				case 0 :
					return BUNDLE["NONE"];
				case 1 :
					return BUNDLE["FISHEYE_CONVERTER"];
				default :
					return BUNDLE["UNKNOWN", aValue.ToString()];
			}
		}

		/// <summary>
		/// Returns the Digital Zoom Description. 
		/// </summary>
		/// <returns>the Digital Zoom Description.</returns>
		private string GetDigitalZoomDescription()  
		{
            if (!base.directory
                .ContainsTag(
                NikonType1Directory.TAG_NIKON_TYPE1_DIGITAL_ZOOM))
            {
                return null;
            }
			Rational aValue =
				base.directory.GetRational(
				NikonType1Directory.TAG_NIKON_TYPE1_DIGITAL_ZOOM);
			if (aValue.GetNumerator() == 0) 
			{
				return BUNDLE["NO_DIGITAL_ZOOM"];
			}
			return BUNDLE["DIGITAL_ZOOM", aValue.ToSimpleString(true)];
		}

		/// <summary>
		/// Returns the Focus Description. 
		/// </summary>
		/// <returns>the Focus Description.</returns>
		private string GetFocusDescription()  
		{
            if (!base.directory
                .ContainsTag(NikonType1Directory.TAG_NIKON_TYPE1_FOCUS))
            {
                return null;
            }
			Rational aValue =
				base.directory.GetRational(
				NikonType1Directory.TAG_NIKON_TYPE1_FOCUS);
			if (aValue.GetNumerator() == 1 && aValue.GetDenominator() == 0) 
			{
				return BUNDLE["INFINITE"];
			}
			return aValue.ToSimpleString(true);
		}

		/// <summary>
		/// Returns the White Balance Description. 
		/// </summary>
		/// <returns>the White Balance Description.</returns>
		private string GetWhiteBalanceDescription()  
		{
            if (!base.directory
                .ContainsTag(
                NikonType1Directory.TAG_NIKON_TYPE1_WHITE_BALANCE))
            {

                return null;
            }
			int aValue =
				base.directory.GetInt(
				NikonType1Directory.TAG_NIKON_TYPE1_WHITE_BALANCE);
			switch (aValue) 
			{
				case 0 :
					return BUNDLE["AUTO"];
				case 1 :
					return BUNDLE["PRESET"];
				case 2 :
					return BUNDLE["DAYLIGHT"];
				case 3 :
					return BUNDLE["INCANDESCENSE"];
				case 4 :
					return BUNDLE["FLUORESCENT"];
				case 5 :
					return BUNDLE["CLOUDY"];
				case 6 :
					return BUNDLE["SPEEDLIGHT"];
				default :
					return BUNDLE["UNKNOWN", aValue.ToString()];
			}
		}

		/// <summary>
		/// Returns the Ccd Sensitivity Description. 
		/// </summary>
		/// <returns>the Ccd Sensitivity Description.</returns>
		private string GetCcdSensitivityDescription()  
		{
            if (!base.directory
                .ContainsTag(
                NikonType1Directory.TAG_NIKON_TYPE1_CCD_SENSITIVITY))
            {
                return null;
            }
			int aValue =
				base.directory.GetInt(
				NikonType1Directory.TAG_NIKON_TYPE1_CCD_SENSITIVITY);
			switch (aValue) 
			{
				case 0 :
					return BUNDLE["ISO","80"];
				case 2 :
					return BUNDLE["ISO","160"];
				case 4 :
					return BUNDLE["ISO","320"];
				case 5 :
					return BUNDLE["ISO","100"];
				default :
					return BUNDLE["UNKNOWN", aValue.ToString()];
			}
		}

		/// <summary>
		/// Returns the Image Adjustment Description. 
		/// </summary>
		/// <returns>the Image Adjustment Description.</returns>
		private string GetImageAdjustmentDescription()  
		{
            if (!base.directory
                .ContainsTag(
                NikonType1Directory.TAG_NIKON_TYPE1_IMAGE_ADJUSTMENT))
            {
                return null;
            }
			int aValue =
				base.directory.GetInt(
				NikonType1Directory.TAG_NIKON_TYPE1_IMAGE_ADJUSTMENT);
			switch (aValue) 
			{
				case 0 :
					return BUNDLE["NORMAL"];
				case 1 :
					return BUNDLE["BRIGHT_P"];
				case 2 :
					return BUNDLE["BRIGHT_M"];
				case 3 :
					return BUNDLE["CONTRAST_P"];
				case 4 :
					return BUNDLE["CONTRAST_M"];
				default :
					return BUNDLE["UNKNOWN", aValue.ToString()];
			}
		}

		/// <summary>
		/// Returns the Color Mode Description. 
		/// </summary>
		/// <returns>the Color Mode Description.</returns>
		private string GetColorModeDescription()  
		{
            if (!base.directory
                .ContainsTag(
                NikonType1Directory.TAG_NIKON_TYPE1_COLOR_MODE))
            {
                return null;
            }
			int aValue =
				base.directory.GetInt(
				NikonType1Directory.TAG_NIKON_TYPE1_COLOR_MODE);
			switch (aValue) 
			{
				case 1 :
					return BUNDLE["COLOR"];
				case 2 :
					return BUNDLE["MONOCHROME"];
				default :
					return BUNDLE["UNKNOWN", aValue.ToString()];
			}
		}

		/// <summary>
		/// Returns the Quality Description. 
		/// </summary>
		/// <returns>the Quality Description.</returns>
		private string GetQualityDescription()  
		{
            if (!base.directory
                .ContainsTag(NikonType1Directory.TAG_NIKON_TYPE1_QUALITY))
            {
                return null;
            }
			int aValue =
				base.directory.GetInt(
				NikonType1Directory.TAG_NIKON_TYPE1_QUALITY);
			switch (aValue) 
			{
				case 1 :
					return BUNDLE["VGA_BASIC"];
				case 2 :
					return BUNDLE["VGA_NORMAL"];
				case 3 :
					return BUNDLE["VGA_FINE"];
				case 4 :
					return BUNDLE["SXGA_BASIC"];
				case 5 :
					return BUNDLE["SXGA_NORMAL"];
				case 6 :
					return BUNDLE["SXGA_FINE"];
				default :
					return BUNDLE["UNKNOWN", aValue.ToString()];
			}
		}
	}
}