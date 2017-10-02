using System;
using com.drew.metadata;
using com.drew.lang;
using com.utils.bundle;


namespace com.drew.metadata.exif
{
    /// <summary>
    /// Mother class for all CasioMarkerNote directory.
    /// </summary>

	public abstract class AbstractCasioTypeDirectory : AbstractDirectory 
	{
        /// <summary>
        /// Creates a new Directory. 
        /// </summary>
        /// <param name="aBundleName">bundle name for this directory</param>
        protected AbstractCasioTypeDirectory(string aBundleName)
            : base(aBundleName)
        {
        }
	}
}