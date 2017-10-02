using com.drew.lang;

namespace com.drew.metadata.exif
{
	/// <summary>
	/// Tag descriptor for GPS
	/// </summary>

	public class GpsDescriptor : AbstractTagDescriptor 
	{
		/// <summary>
		/// Constructor of the object
		/// </summary>
		/// <param name="aDirectory">a base.directory</param>
		public GpsDescriptor(AbstractDirectory aDirectory) : base(aDirectory)
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
				case GpsDirectory.TAG_GPS_ALTITUDE :
					return GetGpsAltitudeDescription();
				case GpsDirectory.TAG_GPS_ALTITUDE_REF :
					return GetGpsAltitudeRefDescription();
				case GpsDirectory.TAG_GPS_STATUS :
					return GetGpsStatusDescription();
				case GpsDirectory.TAG_GPS_MEASURE_MODE :
					return GetGpsMeasureModeDescription();
				case GpsDirectory.TAG_GPS_SPEED_REF :
					return GetGpsSpeedRefDescription();
				case GpsDirectory.TAG_GPS_TRACK_REF :
				case GpsDirectory.TAG_GPS_IMG_DIRECTION_REF :					
				case GpsDirectory.TAG_GPS_DEST_BEARING_REF :
					return GetGpsDirectionReferenceDescription(tagType);
				case GpsDirectory.TAG_GPS_TRACK :
				case GpsDirectory.TAG_GPS_IMG_DIRECTION :
				case GpsDirectory.TAG_GPS_DEST_BEARING :
					return GetGpsDirectionDescription(tagType);
				case GpsDirectory.TAG_GPS_DEST_DISTANCE_REF :
					return GetGpsDestinationReferenceDescription();
				case GpsDirectory.TAG_GPS_TIME_STAMP :
					return GetGpsTimeStampDescription();
					// three rational numbers -- displayed in HH"MM"SS.ss
				case GpsDirectory.TAG_GPS_LONGITUDE :
					return GetGpsLongitudeDescription();
				case GpsDirectory.TAG_GPS_LATITUDE :
					return GetGpsLatitudeDescription();
				default :
					return base.directory.GetString(tagType);
			}
		}

		/// <summary>
		/// Returns the Gps Latitude Description. 
		/// </summary>
		/// <returns>the Gps Latitude Description.</returns>
		private string GetGpsLatitudeDescription()  
		{
            if (!base.directory.ContainsTag(GpsDirectory.TAG_GPS_LATITUDE))
            {
                return null;
            }
			return GetHoursMinutesSecondsDescription(GpsDirectory.TAG_GPS_LATITUDE);
		}

		/// <summary>
		/// Returns the Gps Longitude Description. 
		/// </summary>
		/// <returns>the Gps Longitude Description.</returns>
		private string GetGpsLongitudeDescription()  
		{
            if (!base.directory.ContainsTag(GpsDirectory.TAG_GPS_LONGITUDE))
            {
                return null;
            }
			return GetHoursMinutesSecondsDescription(
				GpsDirectory.TAG_GPS_LONGITUDE);
		}

		/// <summary>
		/// Returns the Hours Minutes Seconds Description. 
		/// </summary>
        /// <param name="aTagType">the tag type</param>
		/// <returns>the Hours Minutes Seconds Description.</returns>
		private string GetHoursMinutesSecondsDescription(int tagType)
		{
			Rational[] components = base.directory.GetRationalArray(tagType);
			// TODO create an HoursMinutesSecods class ??
			int deg = components[0].IntValue();
			float min = components[1].FloatValue();
			float sec = components[2].FloatValue();
			// carry fractions of minutes into seconds -- thanks Colin Briton
			sec += (min % 1) * 60;
			string[] tab = new string[] {deg.ToString(), ((int) min).ToString(), sec.ToString()};
			return BUNDLE["HOURS_MINUTES_SECONDS", tab];
		}

		/// <summary>
		/// Returns the Gps Time Stamp Description. 
		/// </summary>
		/// <returns>the Gps Time Stamp Description.</returns>
		private string GetGpsTimeStampDescription()  
		{
			// time in hour, min, sec
            if (!base.directory.ContainsTag(GpsDirectory.TAG_GPS_TIME_STAMP))
            {
                return null;
            }
			int[] timeComponents =
				base.directory.GetIntArray(GpsDirectory.TAG_GPS_TIME_STAMP);
			string[] tab = new string[] {timeComponents[0].ToString(), timeComponents[1].ToString(), timeComponents[2].ToString()};
			return BUNDLE["GPS_TIME_STAMP", tab];
		}

		/// <summary>
		/// Returns the Gps Destination Reference Description. 
		/// </summary>
		/// <returns>the Gps Destination Reference Description.</returns>
		private string GetGpsDestinationReferenceDescription() 
		{
            if (!base.directory.ContainsTag(GpsDirectory.TAG_GPS_DEST_DISTANCE_REF))
            {
                return null;
            }
			string destRef =
				base.directory.GetString(GpsDirectory.TAG_GPS_DEST_DISTANCE_REF).Trim().ToUpper();
            switch (destRef)
            {
                case "K": return BUNDLE["KILOMETERS"];
                case "M": return BUNDLE["MILES"];
                case "N": return BUNDLE["KNOTS"];
                default: return BUNDLE["UNKNOWN", destRef];
            }
		}

		/// <summary>
		/// Returns the Gps Direction Description. 
		/// </summary>
		/// <returns>the Gps Direction Description.</returns>
		private string GetGpsDirectionDescription(int tagType) 
		{
            if (!base.directory.ContainsTag(tagType))
            {
                return null;
            }
			string gpsDirection = base.directory.GetString(tagType).Trim();
			return BUNDLE["DEGREES", gpsDirection];
		}

		/// <summary>
		/// Returns the Gps Direction Reference Description. 
		/// </summary>
		/// <returns>the Gps Direction Reference Description.</returns>
		private string GetGpsDirectionReferenceDescription(int tagType) 
		{
            if (!base.directory.ContainsTag(tagType))
            {
                return null;
            }
			string gpsDistRef = base.directory.GetString(tagType).Trim().ToUpper();
            switch (gpsDistRef)
            {
                case "T": return BUNDLE["TRUE_DIRECTION"];
                case "M": return BUNDLE["MAGNETIC_DIRECTION"];
                default: return BUNDLE["UNKNOWN", gpsDistRef];
            }
		}

		/// <summary>
		/// Returns the Gps Speed Ref Description. 
		/// </summary>
		/// <returns>the Gps Speed Ref Description.</returns>
		private string GetGpsSpeedRefDescription() 
		{
            if (!base.directory.ContainsTag(GpsDirectory.TAG_GPS_SPEED_REF))
            {
                return null;
            }
			string gpsSpeedRef =
				base.directory.GetString(GpsDirectory.TAG_GPS_SPEED_REF).Trim().ToUpper();
            switch (gpsSpeedRef)
            {
                case "K": return BUNDLE["KPH"];
                case "M": return BUNDLE["MPH"];
                case "N": return BUNDLE["KNOTS"];
                default: return BUNDLE["UNKNOWN", gpsSpeedRef];
            }
		}

		/// <summary>
		/// Returns the Gps Measure Mode Description. 
		/// </summary>
		/// <returns>the Gps Measure Mode Description.</returns>
        private string GetGpsMeasureModeDescription()
        {
            if (!base.directory.ContainsTag(GpsDirectory.TAG_GPS_MEASURE_MODE))
            {
                return null;
            }
            string gpsSpeedMeasureMode =
                base.directory.GetString(GpsDirectory.TAG_GPS_MEASURE_MODE).Trim();

            switch (gpsSpeedMeasureMode)
            {
                case "2":
                case "3": return BUNDLE["DIMENSIONAL_MEASUREMENT", gpsSpeedMeasureMode];
                default: return BUNDLE["UNKNOWN", gpsSpeedMeasureMode];
            }
        }

		/// <summary>
		/// Returns the Gps Status Description. 
		/// </summary>
		/// <returns>the Gps Status Description.</returns>
		private string GetGpsStatusDescription() 
		{
            if (!base.directory.ContainsTag(GpsDirectory.TAG_GPS_STATUS))
            {
                return null;
            }
			string gpsStatus =
				base.directory.GetString(GpsDirectory.TAG_GPS_STATUS).Trim().ToUpper();
            switch (gpsStatus)
            {
                case "A": return BUNDLE["MEASUREMENT_IN_PROGESS"];
                case "V": return BUNDLE["MEASUREMENT_INTEROPERABILITY"];
                default: return BUNDLE["UNKNOWN", gpsStatus];
            }
		}

		/// <summary>
		/// Returns the Gps Altitude Ref Description. 
		/// </summary>
		/// <returns>the Gps Altitude Ref Description.</returns>
		private string GetGpsAltitudeRefDescription()  
		{
            if (!base.directory.ContainsTag(GpsDirectory.TAG_GPS_ALTITUDE_REF))
            {
                return null;
            }
			int alititudeRef = base.directory.GetInt(GpsDirectory.TAG_GPS_ALTITUDE_REF);
			if (alititudeRef == 0) 
			{
				return BUNDLE["SEA_LEVEL"];
			} 
			return BUNDLE["UNKNOWN", alititudeRef.ToString()];
		}

		/// <summary>
		/// Returns the Gps Altitude Description. 
		/// </summary>
		/// <returns>the Gps Altitude Description.</returns>
		private string GetGpsAltitudeDescription()  
		{
            if (!base.directory.ContainsTag(GpsDirectory.TAG_GPS_ALTITUDE))
            {
                return null;
            }
			string alititude =
				base.directory.GetRational(
				GpsDirectory.TAG_GPS_ALTITUDE).ToSimpleString(
				true);
			return BUNDLE["METRES", alititude];
		}
	}
}
