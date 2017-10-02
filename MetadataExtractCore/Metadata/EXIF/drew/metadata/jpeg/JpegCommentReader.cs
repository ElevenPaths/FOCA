using System.IO;
using com.drew.imaging.jpg;
using com.utils;

namespace com.drew.metadata.jpeg
{
	/// <summary>
	/// The Jpeg reader class
	/// </summary>

    public class JpegCommentReader : AbstractMetadataReader 
	{

        /// <summary>
		/// Creates a new JpegCommentReader for the specified Jpeg jpegFile.
		/// </summary>
        /// <param name="aFile">where to read</param>
		public JpegCommentReader(FileInfo aFile) : base(aFile, JpegSegmentReader.SEGMENT_COM)
		{
		}

        /// <summary>
        /// Constructor of the object
        /// </summary>
        /// <param name="data">the data to read</param>
        public JpegCommentReader(byte[] aData)
            : base(aData)
        {
        }

		/// <summary>
		/// Extracts aMetadata
		/// </summary>
		/// <param name="aMetadata">where to add aMetadata</param>
		/// <returns>the aMetadata found</returns>
		public override Metadata Extract(Metadata aMetadata) 
		{
			if (base.data == null) 
			{
				return aMetadata;
			}

			AbstractDirectory lcDirectory = aMetadata.GetDirectory("com.drew.metadata.jpeg.JpegCommentDirectory");
            string comment = Utils.Decode(base.data, true);
			lcDirectory.SetObject(JpegCommentDirectory.TAG_JPEG_COMMENT,comment);
			return aMetadata;
		}
	}
}