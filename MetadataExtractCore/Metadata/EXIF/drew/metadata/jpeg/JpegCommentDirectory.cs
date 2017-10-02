namespace com.drew.metadata.jpeg
{
	/// <summary>
	/// The JpegComment Directory class
	/// </summary>

	public class JpegCommentDirectory : AbstractDirectory 
	{
		/// <summary>
		/// This is in bits/sample, usually 8 (12 and 16 not supported by most software).
		/// </summary>
		public static int TAG_JPEG_COMMENT = 0;

		/// <summary>
		/// Constructor of the object.
		/// </summary>
        public JpegCommentDirectory()
            : base("JpegMarkernote") 
		{
			this.SetDescriptor(new JpegCommentDescriptor(this));
		}
	}
}