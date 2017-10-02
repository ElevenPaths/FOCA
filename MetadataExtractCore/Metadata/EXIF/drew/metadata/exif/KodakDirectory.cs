namespace com.drew.metadata.exif
{
	/// <summary>
	/// The GPS Directory class
	/// </summary>

	public class KodakDirectory : AbstractDirectory 
	{
        // No Tag for now

        /// <summary>
		/// Constructor of the object.
		/// </summary>
        public KodakDirectory()
            : base("KodakMarkernote")
		{
			this.SetDescriptor(new KodakDescriptor(this));
		}
	} 
}
