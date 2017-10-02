using System;
using System.Diagnostics;
using System.IO;
using com.codec.jpeg;
using com.drew.metadata;
using com.drew.metadata.jpeg;
using com.drew.metadata.iptc;
using com.drew.metadata.exif;

namespace com.drew.imaging.jpg
{
	/// <summary>
	/// This class will extract MetaData from a picture.
	/// </summary>
	public class JpegMetadataReader
	{
        /// <summary>
        /// Constructor of the object
        /// </summary>
        /// <exception cref="Exception">Allways</exception>
        private JpegMetadataReader() : base()
        {
            throw new Exception("Do not use");
        }

		/// <summary>
		/// Reads MetaData from a aFile
		/// </summary>
		/// <param name="aFile">where to read information</param>
		/// <returns>the aMetadata object</returns>
		public static Metadata ReadMetadata(FileInfo aFile) 
		{
            JpegSegmentReader lcSegmentReader = null;
            Metadata lcMetadata = null;
            try
            {
                lcSegmentReader = new JpegSegmentReader(aFile);
                lcMetadata = JpegMetadataReader.ExtractJpegSegmentReaderMetadata(lcSegmentReader);

            }
            finally
            {
                if (lcSegmentReader != null)
                {
                    //Dispose will call close for this class
                    lcSegmentReader.Dispose();
                }
            } 
            return lcMetadata;
		}

        public static Metadata ReadMetadata(Stream aStream)
        {
            JpegSegmentReader lcSegmentReader = null;
            Metadata lcMetadata = null;
            try
            {
                lcSegmentReader = new JpegSegmentReader(aStream);
                lcMetadata = JpegMetadataReader.ExtractJpegSegmentReaderMetadata(lcSegmentReader);

            }
            finally
            {
                if (lcSegmentReader != null)
                {
                    // Dispose will call close for this class
                    lcSegmentReader.Dispose();
                }
            }
            return lcMetadata;
        }

		/// <summary>
		/// Extracts aMetadata from a SegmentReader
		/// </summary>
		/// <param name="aSegmentReader">where to extract aMetadata</param>
		/// <returns>the aMetadata found</returns>
		private static Metadata ExtractJpegSegmentReaderMetadata(JpegSegmentReader aSegmentReader) 
		{
			Metadata lcMetadata = new Metadata();
			try 
			{
				byte[] lcExifSegment =
					aSegmentReader.ReadSegment(JpegSegmentReader.SEGMENT_APP1);
				new ExifReader(lcExifSegment).Extract(lcMetadata);
			}
            catch (Exception e) 
			{
                Trace.TraceWarning("Error in reading Exif segment ("+e.Message+")");
				// in the interests of catching as much data as possible, continue
			}

			try 
			{
				byte[] lcIptcSegment =
					aSegmentReader.ReadSegment(JpegSegmentReader.SEGMENT_APPD);
				new IptcReader(lcIptcSegment).Extract(lcMetadata);
			} 
			catch (Exception e) 
			{
                Trace.TraceWarning("Error in reading Iptc segment (" + e.Message + ")");
			}

			try 
			{
				byte[] lcJpegSegment =
					aSegmentReader.ReadSegment(JpegSegmentReader.SEGMENT_SOF0);
				new JpegReader(lcJpegSegment).Extract(lcMetadata);
			}
            catch (Exception e) 
			{
                Trace.TraceWarning("Error in reading Jpeg segment (" + e.Message + ")");
			}

			try 
			{
				byte[] lcJpegCommentSegment =
					aSegmentReader.ReadSegment(JpegSegmentReader.SEGMENT_COM);
				new JpegCommentReader(lcJpegCommentSegment).Extract(lcMetadata);
			}
            catch (Exception e) 
			{
                Trace.TraceWarning("Error in reading Jpeg Comment segment (" + e.Message + ")");
			}

			return lcMetadata;
		}

		/// <summary>
		/// Reads aMetadata from a JPEGDecodeParam object
		/// </summary>
		/// <param name="aDecodeParam">where to find aMetadata</param>
		/// <returns>the aMetadata found</returns>
		public static Metadata ReadMetadata(JPEGDecodeParam aDecodeParam) 
		{
			Metadata lcMetadata = new Metadata();

			// We should only really be seeing Exif in _data[0]... the 2D array exists
			// because markers can theoretically appear multiple times in the aFile.			
			byte[][] lcExifSegment =
				aDecodeParam.GetMarkerData(JPEGDecodeParam.APP1_MARKER);
			if (lcExifSegment != null && lcExifSegment[0].Length > 0) 
			{
				new ExifReader(lcExifSegment[0]).Extract(lcMetadata);
			}

			// similarly, use only the first IPTC segment
			byte[][] lcIptcSegment =
				aDecodeParam.GetMarkerData(JPEGDecodeParam.APPD_MARKER);
			if (lcIptcSegment != null && lcIptcSegment[0].Length > 0) 
			{
				new IptcReader(lcIptcSegment[0]).Extract(lcMetadata);
			}

			// NOTE: Unable to utilise JpegReader for the SOF0 frame here, as the aDecodeParam doesn't contain the byte[]

			// similarly, use only the first Jpeg Comment segment
			byte[][] lcJpegCommentSegment =
				aDecodeParam.GetMarkerData(JPEGDecodeParam.COMMENT_MARKER);
			if (lcJpegCommentSegment != null && lcJpegCommentSegment[0].Length > 0) 
			{
				new JpegCommentReader(lcJpegCommentSegment[0]).Extract(lcMetadata);
			}

			return lcMetadata;
		}
	}
}
