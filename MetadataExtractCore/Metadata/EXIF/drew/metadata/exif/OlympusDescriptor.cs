using System.Text;

namespace com.drew.metadata.exif
{
	/// <summary>
	/// Tag descriptor for Olympus
	/// </summary>

	public class OlympusDescriptor : AbstractTagDescriptor 
	{
		/// <summary>
		/// Constructor of the object
		/// </summary>
		/// <param name="aDirectory">a base.directory</param>
		public OlympusDescriptor(AbstractDirectory aDirectory) : base(aDirectory)
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
				case OlympusDirectory.TAG_OLYMPUS_SPECIAL_MODE :
					return GetSpecialModeDescription();
				case OlympusDirectory.TAG_OLYMPUS_JPEG_QUALITY :
					return GetJpegQualityDescription();
				case OlympusDirectory.TAG_OLYMPUS_MACRO_MODE :
					return GetMacroModeDescription();
				case OlympusDirectory.TAG_OLYMPUS_DIGI_ZOOM_RATIO :
					return GetDigiZoomRatioDescription();
				default:
					return base.directory.GetString(tagType);
			}
		}

		/// <summary>
		/// Returns the Digi Zoom Ratio Description. 
		/// </summary>
		/// <returns>the Digi Zoom Ratio Description.</returns>
		private string GetDigiZoomRatioDescription()  
		{
            if (!directory
                .ContainsTag(OlympusDirectory.TAG_OLYMPUS_DIGI_ZOOM_RATIO))
            {
                return null;
            }
			int aValue =
				directory.GetInt(
				OlympusDirectory.TAG_OLYMPUS_DIGI_ZOOM_RATIO);
			switch (aValue) 
			{
				case 0 :
					return BUNDLE["NORMAL"];
                case 1:
                    return BUNDLE["DIGITAL_ZOOM", "1"];
				case 2 :
                    return BUNDLE["DIGITAL_ZOOM", "2"];
				default :
					return BUNDLE["UNKNOWN", aValue.ToString()];
			}
		}

		/// <summary>
		/// Returns the Macro Mode Description. 
		/// </summary>
		/// <returns>the Macro Mode Description.</returns>
		private string GetMacroModeDescription()  
		{
            if (!directory
                .ContainsTag(OlympusDirectory.TAG_OLYMPUS_MACRO_MODE))
            {
                return null;
            }
			int aValue =
				directory.GetInt(OlympusDirectory.TAG_OLYMPUS_MACRO_MODE);
			switch (aValue) 
			{
				case 0 :
					return BUNDLE["NORMAL_NO_MACRO"];
				case 1 :
					return BUNDLE["MACRO"];
				default :
					return BUNDLE["UNKNOWN", aValue.ToString() ];
			}
		}

		/// <summary>
		/// Returns the Jpeg Quality Description. 
		/// </summary>
		/// <returns>the Jpeg Quality Description.</returns>
		private string GetJpegQualityDescription()  
		{
            if (!base.directory
                .ContainsTag(OlympusDirectory.TAG_OLYMPUS_JPEG_QUALITY))
            {
                return null;
            }
			int aValue =
				base.directory.GetInt(
				OlympusDirectory.TAG_OLYMPUS_JPEG_QUALITY);
			switch (aValue) 
			{
				case 1 :
					return BUNDLE["SQ"];
				case 2 :
					return BUNDLE["HQ"];
				case 3 :
					return BUNDLE["SHQ"];
				default :
					return BUNDLE["UNKNOWN", aValue.ToString() ];
			}
		}

		/// <summary>
		/// Returns the Special Mode Description. 
		/// </summary>
		/// <returns>the Special Mode Description.</returns>
		private string GetSpecialModeDescription()  
		{
            if (!directory
                .ContainsTag(OlympusDirectory.TAG_OLYMPUS_SPECIAL_MODE))
            {
                return null;
            }
			var values =
				directory.GetIntArray(
				OlympusDirectory.TAG_OLYMPUS_SPECIAL_MODE);
			var desc = new StringBuilder();
			switch (values[0]) 
			{
				case 0 :
					desc.Append(BUNDLE["NORMAL_PICTURE_TAKING_MODE"]);
					break;
				case 1 :
					desc.Append(BUNDLE["UNKNOWN_PICTURE_TAKING_MODE"]);
					break;
				case 2 :
					desc.Append(BUNDLE["FAST_PICTURE_TAKING_MODE"]);
					break;
				case 3 :
					desc.Append(BUNDLE["PANORAMA_PICTURE_TAKING_MODE"]);
					break;
				default :
					desc.Append(BUNDLE["UNKNOWN_PICTURE_TAKING_MODE"]);
					break;
			}
			desc.Append(" - ");
			switch (values[1]) 
			{
				case 0 :
					desc.Append(BUNDLE["UNKNOWN_SEQUENCE_NUMBER"]);
					break;
				default :
					desc.Append(BUNDLE["X_RD_IN_A_SEQUENCE", values[1].ToString()]);
					break;
			}
			switch (values[2]) 
			{
				case 1 :
					desc.Append(BUNDLE["LEFT_TO_RIGHT_PAN_DIR"]);
					break;
				case 2 :
					desc.Append(BUNDLE["RIGHT_TO_LEFT_PAN_DIR"]);
					break;
				case 3 :
					desc.Append(BUNDLE["BOTTOM_TO_TOP_PAN_DIR"]);
					break;
				case 4 :
					desc.Append(BUNDLE["TOP_TO_BOTTOM_PAN_DIR"]);
					break;
			}
			return desc.ToString();
		}
	}
}