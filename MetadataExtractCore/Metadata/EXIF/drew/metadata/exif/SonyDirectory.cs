namespace com.drew.metadata.exif
{
    /// <summary>
    /// The sony directory class.
    /// </summary>

	public class SonyDirectory : AbstractDirectory 
	{
        // No tag for now

		/// <summary>
		/// Constructor of the object.
		/// </summary>
        public SonyDirectory()
            : base("SonyMarkernote")
		{
			this.SetDescriptor(new SonyDescriptor(this));
		}
	}
}
