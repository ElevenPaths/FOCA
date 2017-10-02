namespace com.drew.metadata.exif
{
	/// <summary>
	/// The GPS Directory class
	/// </summary>

	public class GpsDirectory : AbstractDirectory 
	{
		/// <summary>
		/// GPS tag version GPSVersionID 0 0 BYTE 4
		/// </summary>
		public const int TAG_GPS_VERSION_ID = 0x0000;
		/// <summary>
		/// North or South Latitude GPSLatitudeRef 1 1 ASCII 2
		/// </summary>
		public const int TAG_GPS_LATITUDE_REF = 0x0001;
		/// <summary>
		/// Latitude GPSLatitude 2 2 RATIONAL 3
		/// </summary>
		public const int TAG_GPS_LATITUDE = 0x0002;
		/// <summary>
		/// East or West Longitude GPSLongitudeRef 3 3 ASCII 2
		/// </summary>
		public const int TAG_GPS_LONGITUDE_REF = 0x0003;
		/// <summary>
		/// Longitude GPSLongitude 4 4 RATIONAL 3
		/// </summary>
		public const int TAG_GPS_LONGITUDE = 0x0004;
		/// <summary>
		/// Altitude reference GPSAltitudeRef 5 5 BYTE 1
		/// </summary>
		public const int TAG_GPS_ALTITUDE_REF = 0x0005;
		/// <summary>
		/// Altitude GPSAltitude 6 6 RATIONAL 1
		/// </summary>
		public const int TAG_GPS_ALTITUDE = 0x0006;
		/// <summary>
		/// GPS time (atomic clock) GPSTimeStamp 7 7 RATIONAL 3
		/// </summary>
		public const int TAG_GPS_TIME_STAMP = 0x0007;
		/// <summary>
		/// GPS satellites used for measurement GPSSatellites 8 8 ASCII Any
		/// </summary>
		public const int TAG_GPS_SATELLITES = 0x0008;
		/// <summary>
		/// GPS receiver status GPSStatus 9 9 ASCII 2
		/// </summary>
		public const int TAG_GPS_STATUS = 0x0009;
		/// <summary>
		/// GPS measurement mode GPSMeasureMode 10 A ASCII 2
		/// </summary>
		public const int TAG_GPS_MEASURE_MODE = 0x000A;
		/// <summary>
		/// Measurement precision GPSDOP 11 B RATIONAL 1
		/// </summary>
		public const int TAG_GPS_DOP = 0x000B;
		/// <summary>
		/// Speed unit GPSSpeedRef 12 C ASCII 2
		/// </summary>
		public const int TAG_GPS_SPEED_REF = 0x000C;
		/// <summary>
		/// Speed of GPS receiver GPSSpeed 13 D RATIONAL 1
		/// </summary>
		public const int TAG_GPS_SPEED = 0x000D;
		/// <summary>
		/// Reference for direction of movement GPSTrackRef 14 E ASCII 2
		/// </summary>
		public const int TAG_GPS_TRACK_REF = 0x000E;
		/// <summary>
		/// Direction of movement GPSTrack 15 F RATIONAL 1
		/// </summary>
		public const int TAG_GPS_TRACK = 0x000F;
		/// <summary>
		/// Reference for direction of image GPSImgDirectionRef 16 10 ASCII 2
		/// </summary>
		public const int TAG_GPS_IMG_DIRECTION_REF = 0x0010;
		/// <summary>
		/// Direction of image GPSImgDirection 17 11 RATIONAL 1
		/// </summary>
		public const int TAG_GPS_IMG_DIRECTION = 0x0011;
		/// <summary>
		/// Geodetic survey data used GPSMapDatum 18 12 ASCII Any
		/// </summary>
		public const int TAG_GPS_MAP_DATUM = 0x0012;
		/// <summary>
		/// Reference for latitude of destination GPSDestLatitudeRef 19 13 ASCII 2
		/// </summary>
		public const int TAG_GPS_DEST_LATITUDE_REF = 0x0013;
		/// <summary>
		/// Latitude of destination GPSDestLatitude 20 14 RATIONAL 3
		/// </summary>
		public const int TAG_GPS_DEST_LATITUDE = 0x0014;
		/// <summary>
		/// Reference for longitude of destination GPSDestLongitudeRef 21 15 ASCII 2
		/// </summary>
		public const int TAG_GPS_DEST_LONGITUDE_REF = 0x0015;
		/// <summary>
		/// Longitude of destination GPSDestLongitude 22 16 RATIONAL 3
		/// </summary>
		public const int TAG_GPS_DEST_LONGITUDE = 0x0016;
		/// <summary>
		/// Reference for bearing of destination GPSDestBearingRef 23 17 ASCII 2
		/// </summary>
		public const int TAG_GPS_DEST_BEARING_REF = 0x0017;
		/// <summary>
		/// Bearing of destination GPSDestBearing 24 18 RATIONAL 1
		/// </summary>
		public const int TAG_GPS_DEST_BEARING = 0x0018;
		/// <summary>
		/// Reference for distance to destination GPSDestDistanceRef 25 19 ASCII 2
		/// </summary>
		public const int TAG_GPS_DEST_DISTANCE_REF = 0x0019;
		/// <summary>
		/// Distance to destination GPSDestDistance 26 1A RATIONAL 1
		/// </summary>
		public const int TAG_GPS_DEST_DISTANCE = 0x001A;

		/// <summary>
		/// Constructor of the object.
		/// </summary>
        public GpsDirectory()
            : base("GpsMarkernote")
		{
			this.SetDescriptor(new GpsDescriptor(this));
		}
	} 
}
