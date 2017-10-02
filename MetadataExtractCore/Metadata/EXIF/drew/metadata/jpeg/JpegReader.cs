using System.IO;
using com.drew.imaging.jpg;
using System.Diagnostics;

namespace com.drew.metadata.jpeg
{
	/// <summary>
	/// The JPEG reader class
	/// </summary>

    public class JpegReader : AbstractMetadataReader 
	{

		/// <summary>
		/// Creates a new JpegReader for the specified Jpeg jpegFile.
		/// </summary>
        /// <param name="aFile">where to read</param>
		public JpegReader(FileInfo aFile) : base(aFile, JpegSegmentReader.SEGMENT_SOF0)
		{
		}

        /// <summary>
        /// Constructor of the object
        /// </summary>
        /// <param name="data">the data to read</param>
        public JpegReader(byte[] aData)
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

			AbstractDirectory lcDirectory = aMetadata.GetDirectory("com.drew.metadata.jpeg.JpegDirectory");

			try 
			{
				// data precision
				int dataPrecision =
					base.Get16Bits(JpegDirectory.TAG_JPEG_DATA_PRECISION);
				lcDirectory.SetObject(
					JpegDirectory.TAG_JPEG_DATA_PRECISION,
					dataPrecision);

				// process height
				int height = base.Get32Bits(JpegDirectory.TAG_JPEG_IMAGE_HEIGHT);
				lcDirectory.SetObject(JpegDirectory.TAG_JPEG_IMAGE_HEIGHT, height);

				// process width
				int width = base.Get32Bits(JpegDirectory.TAG_JPEG_IMAGE_WIDTH);
				lcDirectory.SetObject(JpegDirectory.TAG_JPEG_IMAGE_WIDTH, width);

				// number of components
				int numberOfComponents =
					base.Get16Bits(JpegDirectory.TAG_JPEG_NUMBER_OF_COMPONENTS);
				lcDirectory.SetObject(
					JpegDirectory.TAG_JPEG_NUMBER_OF_COMPONENTS,
					numberOfComponents);

				// for each component, there are three bytes of data:
				// 1 - Component ID: 1 = Y, 2 = Cb, 3 = Cr, 4 = I, 5 = Q
				// 2 - Sampling factors: bit 0-3 vertical, 4-7 horizontal
				// 3 - Quantization table number
				int offset = 6;
				for (int i = 0; i < numberOfComponents; i++) 
				{
					int componentId = base.Get16Bits(offset++);
					int samplingFactorByte = base.Get16Bits(offset++);
					int quantizationTableNumber = base.Get16Bits(offset++);
					JpegComponent lcJpegComponent =
						new JpegComponent(
						componentId,
						samplingFactorByte,
						quantizationTableNumber);
					lcDirectory.SetObject(
						JpegDirectory.TAG_JPEG_COMPONENT_DATA_1 + i,
						lcJpegComponent);
				}

			} 
			catch (MetadataException me) 
			{
                lcDirectory.HasError = true;
				Trace.TraceError("MetadataException: " + me.Message);
			}

			return aMetadata;
		}

	}
}