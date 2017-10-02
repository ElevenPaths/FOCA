namespace com.drew.metadata.exif
{
	/// <summary>
	/// This class represents EXIF INTEROP marker note.
	/// </summary>

	public class ExifInteropDirectory : AbstractDirectory 
	{
		public const int TAG_INTEROP_INDEX = 0x0001;
		public const int TAG_INTEROP_VERSION = 0x0002;
		public const int TAG_RELATED_IMAGE_FILE_FORMAT = 0x1000;
		public const int TAG_RELATED_IMAGE_WIDTH = 0x1001;
		public const int TAG_RELATED_IMAGE_LENGTH = 0x1002;

		/// <summary>
		/// Constructor of the object.
		/// </summary>
        public ExifInteropDirectory()
            : base("ExifInteropMarkernote")
		{
			this.SetDescriptor(new ExifInteropDescriptor(this));
		}

	}
}