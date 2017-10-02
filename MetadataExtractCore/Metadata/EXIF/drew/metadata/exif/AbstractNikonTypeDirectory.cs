namespace com.drew.metadata.exif
{
    /// <summary>
    /// Class for all Nikon directory.
    /// </summary>

	public abstract class AbstractNikonTypeDirectory : AbstractDirectory 
	{
        /// <summary>
        /// Creates a new Directory. 
        /// </summary>
        /// <param name="aBundleName">bundle name for this directory</param>
        protected AbstractNikonTypeDirectory(string aBundleName)
            : base(aBundleName)
        {
        }
	}
}