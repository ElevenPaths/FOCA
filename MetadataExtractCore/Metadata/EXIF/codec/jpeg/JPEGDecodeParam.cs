using System;

namespace com.codec.jpeg
{

	public abstract class JPEGDecodeParam
	{
		/// <summary>
		/// Unknown or Undefined Color ID
		/// </summary>
		public readonly static int COLOR_ID_UNKNOWN = 0;

		/// <summary>
		/// Monochrome
		/// </summary>
		public readonly static int COLOR_ID_GRAY = 1;

		/// <summary>
		/// Red, Green, and Blue
		/// </summary>
		public readonly static int COLOR_ID_RGB = 2;

		/// <summary>
		/// YCbCr
		/// </summary>
		public readonly static int COLOR_ID_YCbCr = 3;

		/// <summary>
		/// CMYK
		/// </summary>
		public readonly static int COLOR_ID_CMYK = 4;

		/// <summary>
		/// PhotoYCC
		/// </summary>
		public readonly static int COLOR_ID_PYCC = 5;
 
		/// <summary>
		/// RGB-Alpha
		/// </summary>
		public readonly static int COLOR_ID_RGBA = 6;
 
		/// <summary>
		/// YCbCr-Alpha
		/// </summary>
		public readonly static int COLOR_ID_YCbCrA = 7;

		/// <summary>
		/// RGB-Alpha with R, G, and B inverted.
		/// </summary>
		public readonly static int COLOR_ID_RGBA_INVERTED = 8;
 
		/// <summary>
		/// YCbCr-Alpha with Y, Cb, and Cr inverted.
		/// </summary>
		public readonly static int COLOR_ID_YCbCrA_INVERTED = 9;
	
		/// <summary>
		/// PhotoYCC-Alpha
		/// </summary>
		public readonly static int COLOR_ID_PYCCA = 10;

		/// <summary>
		/// YCbCrK
		/// </summary>
		public readonly static int COLOR_ID_YCCK = 11;	    

		/// <summary>
		/// Number of color ids defined.
		/// </summary>
		public readonly static int NUM_COLOR_ID = 12;

		/// <summary>
		/// Number of allowed Huffman and Quantization Tables
		/// </summary>
		public readonly static int  NUM_TABLES = 4;  
	
		/// <summary>
		/// The X and Y units simply indicate the aspect ratio of the pixels.
		/// </summary>
		public  readonly static int DENSITY_UNIT_ASPECT_RATIO = 0;
		
		/// <summary>
		/// Pixel density is in pixels per inch.
		/// </summary>
		public  readonly static int DENSITY_UNIT_DOTS_INCH    = 1;
		
		/// <summary>
		/// Pixel density is in pixels per centemeter.
		/// </summary>
		public readonly static int DENSITY_UNIT_DOTS_CM      = 2;
		
		/// <summary>
		/// The max known value for DENSITY_UNIT
		/// </summary>
		public readonly static int NUM_DENSITY_UNIT = 3;

		/// <summary>
		/// APP0 marker - JFIF info
		/// </summary>
		public readonly static int APP0_MARKER  = 0xE0;
		
		/// <summary>
		/// APP1 marker
		/// </summary>
		public readonly static int APP1_MARKER  = 0xE1;
		
		/// <summary>
		/// APP2 marker
		/// </summary>
		public readonly static int APP2_MARKER  = 0xE2;
		
		/// <summary>
		/// APP3 marker
		/// </summary>
		public readonly static int APP3_MARKER  = 0xE3;
		
		/// <summary>
		/// APP4 marker
		/// </summary>
		public readonly static int APP4_MARKER  = 0xE4;
		
		/// <summary>
		/// APP5 marker
		/// </summary>
		public readonly static int APP5_MARKER  = 0xE5;
		
		/// <summary>
		/// APP6 marker
		/// </summary>
		public readonly static int APP6_MARKER  = 0xE6;
		
		/// <summary>
		/// APP7 marker
		/// </summary>
		public readonly static int APP7_MARKER  = 0xE7;
		
		/// <summary>
		/// APP8 marker
		/// </summary>
		public readonly static int APP8_MARKER  = 0xE8;
		
		/// <summary>
		/// APP9 marker
		/// </summary>
		public readonly static int APP9_MARKER  = 0xE9;
		
		/// <summary>
		/// APPA marker
		/// </summary>
		public readonly static int APPA_MARKER  = 0xEA;
		
		/// <summary>
		/// APPB marker
		/// </summary>
		public readonly static int APPB_MARKER  = 0xEB;
		
		/// <summary>
		/// APPC marker
		/// </summary>
		public readonly static int APPC_MARKER  = 0xEC;
		
		/// <summary>
		/// APPD marker
		/// </summary>
		public readonly static int APPD_MARKER  = 0xED;
		
		/// <summary>
		/// APPE marker - Adobe info
		/// </summary>
		public readonly static int APPE_MARKER  = 0xEE;
		
		/// <summary>
		/// APPF marker
		/// </summary>
		public readonly static int APPF_MARKER  = 0xEF;

		/// <summary>
		/// Adobe marker indicates presence/need for Adobe marker.
		/// </summary>
		public readonly static int COMMENT_MARKER = 0XFE;
	
		/// <summary>
		/// Get the image width 
		/// </summary>
		/// <returns>the width of the image data in pixels.</returns>
		public abstract int  GetWidth();

		/// <summary>
		/// Get the image height
		/// </summary>
		/// <returns>The height of the image data in pixels.</returns>
		public abstract int  GetHeight();

		/// <summary>
		/// Return the Horizontal subsampling lcFactor for requested
		/// Component. The Subsample lcFactor is the number of input pixels
		/// that contribute to each output pixel.  This is distinct from
		/// the way the JPEG to each output pixel.  This is distinct from
		/// the way the JPEG standard defines this quantity, because 
		/// fractional subsampling factors are not allowed, and it was felt
		/// </summary>
		/// <param name="component">The component of the encoded image to return the subsampling lcFactor for.</param>
		/// <returns>The subsample lcFactor.</returns>
		public abstract int GetHorizontalSubsampling(int component);

		/// <summary>
		/// Return the Vertical subsampling lcFactor for requested Component. 
		/// The Subsample lcFactor is the number of input pixels that contribute to each output pixel. 
		/// This is distinct from the way the JPEG to each output pixel. 
		/// This is distinct from the way the JPEG standard defines this quantity, because 
		/// fractional subsampling factors are not allowed, and it was felt 
		/// </summary>
		/// <param name="component">The component of the encoded image to return the subsampling lcFactor for.</param>
		/// <returns>The subsample lcFactor.</returns>
		public abstract int GetVerticalSubsampling(int component);

		/// <summary>
		/// Returns the coefficient quantization tables or NULL if not defined. 
		/// tableNum must range in value from 0 - 3. 
		/// </summary>
		/// <param name="tableNum">the index of the table to be returned.</param>
		/// <returns>Quantization table stored at index tableNum.</returns>
		public abstract JPEGQTable GetQTable(int tableNum );

		/// <summary>
		/// Returns the Quantization table for the requested component. 
		/// </summary>
		/// <param name="component">the image component of interest.</param>
		/// <returns>Quantization table associated with component</returns>
		public abstract JPEGQTable GetQTableForComponent(int component);

		/// <summary>
		/// Returns the DC Huffman coding table requested or null if not defined 
		/// </summary>
		/// <param name="tableNum">the index of the table to be returned.</param>
		/// <returns>Huffman table stored at index tableNum.  </returns>
		public abstract JPEGHuffmanTable GetDCHuffmanTable( int tableNum );

		/// <summary>
		/// Returns the DC Huffman coding table for the requested component. 
		/// </summary>
		/// <param name="component">the image component of interest.</param>
		/// <returns>Huffman table associated with component</returns>
		public abstract JPEGHuffmanTable GetDCHuffmanTableForComponent(int component);

		/// <summary>
		/// Returns the AC Huffman coding table requested or null if not defined 
		/// </summary>
		/// <param name="tableNum">the index of the table to be returned.</param>
		/// <returns>Huffman table stored at index tableNum.</returns>
		public abstract JPEGHuffmanTable GetACHuffmanTable( int tableNum );

		/// <summary>
		/// Returns the AC Huffman coding table for the requested component. 
		/// </summary>
		/// <param name="component">the image component of interest.</param>
		/// <returns>Huffman table associated with component</returns>
		public abstract JPEGHuffmanTable GetACHuffmanTableForComponent(int component);

		/// <summary>
		/// Get the number of the DC Huffman table that will be used for a particular component. 
		/// </summary>
		/// <param name="component">The Component of interest.</param>
		/// <returns>The table number of the DC Huffman table for component.</returns>
		public abstract int GetDCHuffmanComponentMapping(int component);

		/// <summary>
		/// Get the number of the AC Huffman table that will be used for a particular component. 
		/// </summary>
		/// <param name="component">The Component of interest.</param>
		/// <returns>The table number of the AC Huffman table for component.</returns>
		public abstract int GetACHuffmanComponentMapping(int component);

		/// <summary>
		/// Get the number of the quantization table that will be used for a particular component. 
		/// </summary>
		/// <param name="component">The Component of interest.</param>
		/// <returns>The table number of the Quantization table for component.</returns>
		public abstract int GetQTableComponentMapping(int component);

		/// <summary>
		/// Returns true if the image information in the ParamBlock is currently valid. 
		/// This indicates if image data was read from the stream for decoding and weather 
		/// image data should be written when encoding. 
		/// </summary>
		/// <returns>true if the image information in the ParamBlock is currently valid. </returns>
		public abstract bool IsImageInfoValid();

		/// <summary>
		/// Returns true if the tables in the ParamBlock are currently valid.
		/// This indicates that tables were read from the stream for decoding. 
		/// When encoding this indicates wether tables should be written to the stream.
		/// </summary>
		/// <returns>true if the tables in the ParamBlock are currently valid.</returns>
		public abstract bool IsTableInfoValid();

		/// <summary>
		/// Returns true if at least one instance of the marker is present in the Parameter object.
		/// For encoding returns true if there is at least one instance of the marker to be written.
		/// </summary>
		/// <param name="marker"></param>
		/// <returns>The marker of interest.</returns>
		public abstract bool GetMarker(int marker);

		/// <summary>
		/// Returns a 'byte[][]' associated with the requested marker in the parameter object.
		/// Each entry in the 'byte[][]' is the data associated with one instance of 
		/// the marker (each marker can theoretically appear any number of times in a stream).
		/// </summary>
		/// <param name="marker">The marker of interest.</param>
		/// <returns>The 'byte[][]' for this marker or null if none available.</returns>
		public abstract byte[][] GetMarkerData(int marker);

		/// <summary>
		/// Returns the JPEG Encoded color id. This is generally speaking only used 
		/// if you are decoding into Rasters.  Note that when decoding into a Raster no 
		/// color conversion is performed. 
		/// </summary>
		/// <returns>The value of the JPEG encoded data'str color id.</returns>
		public abstract int GetEncodedColorID();

		/// <summary>
		/// Returns the number of components for the current encoding COLOR_ID. 
		/// </summary>
		/// <returns>the number of Components</returns>
		public abstract int GetNumComponents();

		/// <summary>
		/// Get the MCUs per restart marker. 
		/// </summary>
		/// <returns>The number of MCUs between restart markers.</returns>
		public abstract int GetRestartInterval();

		/// <summary>
		/// Get the code for pixel size units This value is copied from the APP0 marker.
		/// It isn't used by the JPEG codec.  If the APP0 marker wasn't present then you 
		/// can not rely on this value. 
		/// </summary>
		/// <returns>Value indicating the density unit one of the DENSITY_UNIT_* constants.</returns>
		public abstract int GetDensityUnit();

		/// <summary>
		/// Get the horizontal pixel density This value is copied from the APP0 marker.
		/// It isn't used by the JPEG code.  If the APP0 marker wasn't present then 
		/// you can not rely on this value. 
		/// </summary>
		/// <returns>The horizontal pixel density, in units described by</returns>
		public abstract int GetXDensity();

		/// <summary>
		/// Get the vertical pixel density This value is copied into the APP0 marker.
		/// It isn't used by the JPEG code. If the APP0 marker wasn't present then 
		/// you can not rely on this value. 
		/// </summary>
		/// <returns>The verticle pixel density, in units described by</returns>
		public abstract int getYDensity();	
	}
}