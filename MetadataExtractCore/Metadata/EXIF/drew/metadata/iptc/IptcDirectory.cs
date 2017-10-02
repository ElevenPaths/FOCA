namespace com.drew.metadata.iptc
{
	/// <summary>
	/// The Iptc Directory class
	/// </summary>

	public class IptcDirectory : AbstractDirectory 
	{
		public const int TAG_RECORD_VERSION = 0x0200;
		public const int TAG_CAPTION = 0x0278;
		public const int TAG_WRITER = 0x027a;
		public const int TAG_HEADLINE = 0x0269;
		public const int TAG_SPECIAL_INSTRUCTIONS = 0x0228;
		public const int TAG_BY_LINE = 0x0250;
		public const int TAG_BY_LINE_TITLE = 0x0255;
		public const int TAG_CREDIT = 0x026e;
		public const int TAG_SOURCE = 0x0273;
		public const int TAG_OBJECT_NAME = 0x0205;
		public const int TAG_DATE_CREATED = 0x0237;
		public const int TAG_CITY = 0x025a;
		public const int TAG_PROVINCE_OR_STATE = 0x025f;
		public const int TAG_COUNTRY_OR_PRIMARY_LOCATION = 0x0265;
		public const int TAG_ORIGINAL_TRANSMISSION_REFERENCE = 0x0267;
		public const int TAG_CATEGORY = 0x020f;
		public const int TAG_SUPPLEMENTAL_CATEGORIES = 0x0214;
		public const int TAG_URGENCY = 0x0200 | 10;
		public const int TAG_KEYWORDS = 0x0200 | 25;
		public const int TAG_COPYRIGHT_NOTICE = 0x0274;
		public const int TAG_RELEASE_DATE = 0x0200 | 30;
		public const int TAG_RELEASE_TIME = 0x0200 | 35;
		public const int TAG_TIME_CREATED = 0x0200 | 60;
		public const int TAG_ORIGINATING_PROGRAM = 0x0200 | 65;

    	/// <summary>
		/// Constructor of the object.
		/// </summary>
        public IptcDirectory()
            : base("IptcMarkernote")
		{
			this.SetDescriptor(new IptcDescriptor(this));
		}

	}
}