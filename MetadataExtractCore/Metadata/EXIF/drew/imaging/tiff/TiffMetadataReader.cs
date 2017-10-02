using System;
using System.IO;
using com.codec.jpeg;
using com.drew.metadata;
using com.drew.metadata.exif;

namespace com.drew.imaging.tiff
{
	/// <summary>
	/// This class will extract MetaData from a picture.
	/// </summary>
    public class TiffMetadataReader
    {
        /// <summary>
        /// Constructor of the object.
        /// </summary>
        private TiffMetadataReader()
            : base()
        {
            throw new Exception("Do not use");
        }

        /// <summary>
        /// Constructor of the object.
        /// </summary>
        /// <param name="aFile">Where to read metadata from</param>
        /// <returns>a meta data</returns>
        public static Metadata ReadMetadata(FileInfo aFile)
        {
            Metadata lcMetadata = null;
            try
            {
                using (var lcStream = aFile.OpenRead()) {
                    lcMetadata = ReadMetadata(lcStream);
                }

            }
            catch (Exception e)
            {
                throw new MetadataException("Error reading metadata from tiff file: " + e.Message,
                    e);
            }
            return lcMetadata;
        }

        /// <summary>
        /// Constructor of the object.
        /// </summary>
        /// <param name="aStream">Where to read information from. Caution, you are responsible for closing this stream.</param>
        /// <returns>a meta data object</returns>
        public static Metadata ReadMetadata(Stream aStream)
        {
            Metadata metadata = new Metadata();
            try
            {
                byte[] buffer = new byte[(int)aStream.Length];
                aStream.Read(buffer, 0, buffer.Length);

                new ExifReader(buffer).ExtractTiff(metadata);
            }
            catch (MetadataException e)
            {
                throw new TiffProcessingException(e);
            }
            return metadata;
        }
    }
}
