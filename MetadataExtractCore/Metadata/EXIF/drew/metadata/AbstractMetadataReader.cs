using System;
using System.Collections;
using System.IO;
using com.drew.imaging.jpg;

namespace com.drew.metadata
{
    /// <summary>
    /// An abstract reader class
    /// </summary>

    public abstract class AbstractMetadataReader : IMetadataReader
    {
        /// <summary>
        /// The data segment
        /// </summary>
        protected readonly byte[] data;

        /// <summary>
        /// Creates a new Reader for the specified file.
        /// </summary>
        /// <param name="aFile">where to read</param>
        protected AbstractMetadataReader(FileInfo aFile, byte aSegment)
            : this(
            new JpegSegmentReader(aFile).ReadSegment(
            aSegment))
        {
        }

        /// <summary>
        /// Constructor of the object
        /// </summary>
        /// <param name="data">the data to read</param>
        protected AbstractMetadataReader(byte[] aData)
        {
            this.data = aData;
        }

        /// <summary>
        /// Performs the data extraction, returning a new instance of Metadata. 
        /// </summary>
        /// <returns>a new instance of Metadata</returns>
        public Metadata Extract()
        {
            return Extract(new Metadata());
        }

        /// <summary>
        /// Extracts aMetadata
        /// </summary>
        /// <param name="aMetadata">where to add aMetadata</param>
        /// <returns>the aMetadata found</returns>
        public abstract Metadata Extract(Metadata metadata);

        /// <summary>
        /// Returns an int calculated from two bytes of data at the specified lcOffset (MSB, LSB).
        /// </summary>
        /// <param name="anOffset">position within the data buffer to read first byte</param>
        /// <returns>the 32 bit int value, between 0x0000 and 0xFFFF</returns>
        protected virtual int Get32Bits(int anOffset)
        {
            if (anOffset >= this.data.Length)
            {
                throw new MetadataException("Attempt to read bytes from outside Iptc data buffer");
            }
            return ((this.data[anOffset] & 255) << 8) | (this.data[anOffset + 1] & 255);
        }

        /// <summary>
        /// Returns an int calculated from one byte of data at the specified lcOffset.
        /// </summary>
        /// <param name="anOffset">position within the data buffer to read byte</param>
        /// <returns>the 16 bit int value, between 0x00 and 0xFF</returns>
        protected virtual int Get16Bits(int anOffset)
        {
            if (anOffset >= this.data.Length)
            {
                throw new MetadataException("Attempt to read bytes from outside Jpeg segment data buffer");
            }

            return (this.data[anOffset] & 255);
        }
   }
}