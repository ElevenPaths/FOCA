using System;
using System.IO;

namespace com.drew.imaging.jpg
{
    /// <summary>
    /// Will analyze a stream form an image
    /// </summary>
    public class JpegSegmentReader
    {
        private FileInfo file;

        private byte[] data;

        private Stream stream;

        private JpegSegmentData segmentDataMap;

        /// <summary>
        /// Private, because this segment crashes my algorithm, and searching for it doesn't work (yet).
        /// <summary>
        private const byte SEGMENT_SOS = (byte)0xDA;

        /// <summary>
        /// Private, because one wouldn't search for it.
        /// </summary>
        private const byte MARKER_EOI = (byte)0xD9;

        /// <summary>
        /// APP0 Jpeg segment identifier -- Jfif data.
        /// </summary>
        public const byte SEGMENT_APP0 = (byte)0xE0;
        /// <summary>
        /// APP1 Jpeg segment identifier -- where Exif data is kept.
        /// </summary>
        public const byte SEGMENT_APP1 = (byte)0xE1;
        /// <summary>
        /// APP2 Jpeg segment identifier.
        /// </summary>
        public const byte SEGMENT_APP2 = (byte)0xE2;
        /// <summary>
        /// APP3 Jpeg segment identifier.
        /// </summary>
        public const byte SEGMENT_APP3 = (byte)0xE3;
        /// <summary>
        /// APP4 Jpeg segment identifier.
        /// </summary>
        public const byte SEGMENT_APP4 = (byte)0xE4;
        /// <summary>
        /// APP5 Jpeg segment identifier.
        /// </summary>
        public const byte SEGMENT_APP5 = (byte)0xE5;
        /// <summary>
        /// APP6 Jpeg segment identifier.
        /// </summary>
        public const byte SEGMENT_APP6 = (byte)0xE6;
        /// <summary>
        /// APP7 Jpeg segment identifier.
        /// </summary>
        public const byte SEGMENT_APP7 = (byte)0xE7;
        /// <summary>
        /// APP8 Jpeg segment identifier.
        /// </summary>
        public const byte SEGMENT_APP8 = (byte)0xE8;
        /// <summary>
        /// APP9 Jpeg segment identifier.
        /// </summary>
        public const byte SEGMENT_APP9 = (byte)0xE9;
        /// <summary>
        /// APPA Jpeg segment identifier -- can hold Unicode comments.
        /// </summary>
        public const byte SEGMENT_APPA = (byte)0xEA;
        /// <summary>
        /// APPB Jpeg segment identifier.
        /// </summary>
        public const byte SEGMENT_APPB = (byte)0xEB;
        /// <summary>
        /// APPC Jpeg segment identifier.
        /// </summary>
        public const byte SEGMENT_APPC = (byte)0xEC;
        /// <summary>
        /// APPD Jpeg segment identifier -- IPTC data in here.
        /// </summary>
        public const byte SEGMENT_APPD = (byte)0xED;
        /// <summary>
        /// APPE Jpeg segment identifier.
        /// </summary>
        public const byte SEGMENT_APPE = (byte)0xEE;
        /// <summary>
        /// APPF Jpeg segment identifier.
        /// </summary>
        public const byte SEGMENT_APPF = (byte)0xEF;
        /// <summary>
        /// Start Of Image segment identifier.
        /// </summary>
        public const byte SEGMENT_SOI = (byte)0xD8;
        /// <summary>
        /// Define Quantization Table segment identifier.
        /// </summary>
        public const byte SEGMENT_DQT = (byte)0xDB;
        /// <summary>
        /// Define Huffman Table segment identifier.
        /// </summary>
        public const byte SEGMENT_DHT = (byte)0xC4;
        /// <summary>
        /// Start-of-Frame Zero segment identifier.
        /// </summary>
        public const byte SEGMENT_SOF0 = (byte)0xC0;
        /// <summary>
        /// Jpeg comment segment identifier.
        /// </summary>
        public const byte SEGMENT_COM = (byte)0xFE;


        /// <summary>
        /// Constructor of the object
        /// </summary>
        /// <param name="aFile">where to read</param>
        public JpegSegmentReader(FileInfo aFile)
            : base()
        {
            this.file = aFile;
            this.data = null;
            this.stream = null;
            this.ReadSegments();
        }

        /// <summary>
        /// Constructor of the object
        /// </summary>
        /// <param name="aFileContents">where to read</param>
        public JpegSegmentReader(byte[] aFileContents)
        {
            this.file = null;
            this.stream = null;
            this.data = aFileContents;
            this.ReadSegments();
        }


        /// <summary>
        /// Constructor of the object
        /// </summary>
        /// <param name="aStream">where to read.</param>
        public JpegSegmentReader(Stream aStream)
        {
            this.stream = aStream;
            this.file = null;
            this.data = null;
            this.ReadSegments();
        }

        /// <summary>
        /// Reads the first instance of a given Jpeg segment, returning the contents as a byte array.
        /// </summary>
        /// <param name="aSegmentMarker">the byte identifier for the desired segment</param>
        /// <returns>the byte array if found, else null</returns>
        /// <exception cref="JpegProcessingException">for any problems processing the Jpeg data</exception>
        public byte[] ReadSegment(byte aSegmentMarker)
        {
            return this.ReadSegment(aSegmentMarker, 0);
        }

        /// <summary>
        /// Reads the first instance of a given Jpeg segment, returning the contents as a byte array.
        /// </summary>
        /// <param name="aSegmentMarker">the byte identifier for the desired segment</param>
        /// <param name="anOccurrence">the anOccurrence of the specified segment within the jpeg aFile</param>
        /// <returns>the byte array if found, else null</returns>
        /// <exception cref="JpegProcessingException">for any problems processing the Jpeg data</exception>
        public byte[] ReadSegment(byte aSegmentMarker, int anOccurrence)
        {
            return this.segmentDataMap.GetSegment(aSegmentMarker);
        }

        /// <summary>
        /// Gets the number of segment
        /// </summary>
        /// <param name="aSegmentMarker">the byte identifier for the desired segment</param>
        /// <returns>the number of segment or zero if segment does not exist</returns>
        public int GetSegmentCount(byte aSegmentMarker)
        {
            return this.segmentDataMap.GetSegmentCount(aSegmentMarker);
        }

        /// <summary>
        /// Reads segments
        /// </summary>
        /// <exception cref="JpegProcessingException">for any problems processing the Jpeg data</exception>
        private void ReadSegments()
        {
            this.segmentDataMap = new JpegSegmentData();
            using (var lcInStream = this.GetJpegInputStream())
            {
                try
                {
                    int lcOffset = 0;
                    // first two bytes should be jpeg magic number
                    if (!this.IsValidJpegHeaderBytes(lcInStream))
                    {
                        throw new JpegProcessingException("not a jpeg file");
                    }
                    lcOffset += 2;
                    do
                    {
                        // next byte is 0xFF
                        byte lcSegmentIdentifier = (byte)(lcInStream.ReadByte() & 0xFF);
                        if ((lcSegmentIdentifier & 0xFF) != 0xFF)
                        {
                            throw new JpegProcessingException(
                                "expected jpeg segment start identifier 0xFF at offset "
                                + lcOffset
                                + ", not 0x"
                                + (lcSegmentIdentifier & 0xFF).ToString("X"));
                        }
                        lcOffset++;
                        // next byte is <segment-marker>
                        byte lcSegmentMarker = (byte)(lcInStream.ReadByte() & 0xFF);
                        lcOffset++;
                        // next 2-bytes are <segment-size>: [high-byte] [low-byte]
                        byte[] lcSegmentLengthBytes = new byte[2];
                        lcInStream.Read(lcSegmentLengthBytes, 0, 2);
                        lcOffset += 2;
                        int lcSegmentLength =
                            ((lcSegmentLengthBytes[0] << 8) & 0xFF00)
                            | (lcSegmentLengthBytes[1] & 0xFF);
                        // segment length includes size bytes, so subtract two
                        lcSegmentLength -= 2;
                        if (lcSegmentLength > (lcInStream.Length - lcInStream.Position))
                        {
                            throw new JpegProcessingException("segment size would extend beyond file stream length");
                        }
                        else if (lcSegmentLength < 0)
                        {
                            throw new JpegProcessingException("segment size would be less than zero");
                        }
                        byte[] lcSegmentBytes = new byte[lcSegmentLength];
                        lcInStream.Read(lcSegmentBytes, 0, lcSegmentLength);
                        lcOffset += lcSegmentLength;
                        if ((lcSegmentMarker & 0xFF) == (SEGMENT_SOS & 0xFF))
                        {
                            // The 'Start-Of-Scan' segment'str length doesn't include the image data, instead would
                            // have to search for the two bytes: 0xFF 0xD9 (EOI).
                            // It comes last so simply return at this point
                            return;
                        }
                        else if ((lcSegmentMarker & 0xFF) == (MARKER_EOI & 0xFF))
                        {
                            // the 'End-Of-Image' segment -- this should never be found in this fashion
                            return;
                        }
                        else
                        {
                            this.segmentDataMap.AddSegment(lcSegmentMarker, lcSegmentBytes);
                        }
                        // didn't find the one we're looking for, loop through to the next segment
                    } while (true);
                }
                catch (Exception ioe)
                {
                    throw new JpegProcessingException(
                        "IOException processing Jpeg file: " + ioe.Message,
                        ioe);
                }
            }
        }

        /// <summary>
        /// Private helper method to create a BufferedInputStream of Jpeg data
        /// from whichever data source was specified upon construction of this instance.
        /// </summary>
        /// <returns>a BufferedStream of Jpeg data</returns>
        /// <exception cref="JpegProcessingException">for any problems processing the Jpeg data</exception>
        private BufferedStream GetJpegInputStream()
        {
            if (this.stream != null)
            {
                if (this.stream is BufferedStream)
                {
                    return (BufferedStream)this.stream;
                }
                else
                {
                    return new BufferedStream(this.stream);
                }
            }
            Stream lcInputStream = null;
            if (this.data == null)
            {
                try
                {
                    // Added read only access for ASPX use, thanks for Ryan Patridge
                    lcInputStream = this.file.Open(FileMode.Open, FileAccess.Read);
                }
                catch (FileNotFoundException e)
                {
                    throw new JpegProcessingException(
                        "Jpeg file \"" + file.FullName + "\" does not exist",
                        e);
                }
            }
            else
            {
                lcInputStream = new MemoryStream(this.data);
            }
            return new BufferedStream(lcInputStream);
        }

        /// <summary>
        /// Helper method that validates the Jpeg aFile'str magic number.
        /// </summary>
        /// <param name="aFileStream">the InputStream to read bytes from, which must be positioned at its start (i.e. no bytes read yet)</param>
        /// <returns>true if the magic number is Jpeg (0xFFD8)</returns>
        /// <exception cref="JpegProcessingException">for any problems processing the Jpeg data</exception>
        private bool IsValidJpegHeaderBytes(BufferedStream aFileStream)
        {
            byte[] lcHeader = new byte[2];
            aFileStream.Read(lcHeader, 0, 2);
            return ((lcHeader[0] & 0xFF) == 0xFF && (lcHeader[1] & 0xFF) == 0xD8);
        }

        /// <summary>
        /// Close the stream.
        /// </summary>
        public void Close()
        {
            if (this.stream != null)
            {
                this.stream.Close();
            }
        }

        /// <summary>
        /// Dispose the stream and all object linked with it
        /// </summary>
        public void Dispose()
        {
            if (this.stream != null)
            {
                this.stream.Close();
                this.stream.Dispose();
            }
            if (this.file != null)
            {
                this.file = null;
            }
        }
    }
}