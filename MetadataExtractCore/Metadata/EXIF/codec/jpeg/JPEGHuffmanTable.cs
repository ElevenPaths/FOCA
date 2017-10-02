using System;

namespace com.codec.jpeg
{
	/// <summary>
	/// A class to encapsulate a JPEG Huffman table.
	/// </summary>
	public sealed class JPEGHuffmanTable
	{
		/// <summary>
		/// The maximum number of symbol lengths (max symbol length in bits = 16)
		/// </summary>
		private static readonly int HUFF_MAX_LEN=17;

		/// <summary>
		/// the maximum number of symbols
		/// </summary>
		private static readonly int HUFF_MAX_SYM=256;

		/// <summary>
		/// bits[k] = number of symbols with length k bits
		/// </summary>
		private short[] lengths;

		/// <summary>
		/// Symbols in order of increasing length
		/// </summary>
		private short[] symbols;

		/// <summary>
		/// Standard Huffman table ( JPEG standard section K.3 )
		/// </summary>
		public static readonly JPEGHuffmanTable StdDCLuminance = JPEGHuffmanTable.InitStdDCLuminance();

		/// <summary>
		/// Initialize Standard Huffman table.
		/// </summary>
		/// <returns>Standard Huffman table ( JPEG standard section K.3 )</returns>
		private static JPEGHuffmanTable InitStdDCLuminance() 
		{
			short[] lengths = { 
								  0, 0, 1, 5, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 };
			short[] symbols = {
								  0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
			JPEGHuffmanTable resu = new JPEGHuffmanTable();
			resu.lengths = lengths;
			resu.symbols = symbols;
			resu.checkTable();
			return resu;
		}
	
	

		/// <summary>
		/// Standard Huffman table Chrominance ( JPEG standard section K.3 )
		/// </summary>
		public static readonly JPEGHuffmanTable StdDCChrominance = JPEGHuffmanTable.InitStdDCChrominance();

		/// <summary>
		/// Initialize Standard Huffman table Chrominance. 
		/// </summary>
		/// <returns>Standard Huffman table Chrominance ( JPEG standard section K.3 )</returns>
		private static JPEGHuffmanTable InitStdDCChrominance() 
		{
			short[] lengths = { // 0-base
								  0, 0, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 };
			short[] symbols = {
								  0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
			JPEGHuffmanTable resu = new JPEGHuffmanTable();
			resu.lengths = lengths;
			resu.symbols = symbols;
			resu.checkTable();
			return resu;
		}
	
		/// <summary>
		/// Standard Huffman table Luminance. 
		/// </summary>
		public static readonly JPEGHuffmanTable StdACLuminance = JPEGHuffmanTable.InitStdACLuminance();

		/// <summary>
		/// Initialize Standard Huffman table Luminance. 
		/// </summary>
		/// <returns>Standard Huffman table Luminance ( JPEG standard section K.3 )</returns>
		private static JPEGHuffmanTable InitStdACLuminance() 
		{
			short[] lengths = { // 0-base
								  0, 0, 2, 1, 3, 3, 2, 4, 3, 5, 5, 4, 4, 0, 0, 1, 0x7d };
			short[] symbols = { 
								  0x01, 0x02, 0x03, 0x00, 0x04, 0x11, 0x05, 0x12,
								  0x21, 0x31, 0x41, 0x06, 0x13, 0x51, 0x61, 0x07,
								  0x22, 0x71, 0x14, 0x32, 0x81, 0x91, 0xa1, 0x08,
								  0x23, 0x42, 0xb1, 0xc1, 0x15, 0x52, 0xd1, 0xf0,
								  0x24, 0x33, 0x62, 0x72, 0x82, 0x09, 0x0a, 0x16,
								  0x17, 0x18, 0x19, 0x1a, 0x25, 0x26, 0x27, 0x28,
								  0x29, 0x2a, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39,
								  0x3a, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49,
								  0x4a, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59,
								  0x5a, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69,
								  0x6a, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79,
								  0x7a, 0x83, 0x84, 0x85, 0x86, 0x87, 0x88, 0x89,
								  0x8a, 0x92, 0x93, 0x94, 0x95, 0x96, 0x97, 0x98,
								  0x99, 0x9a, 0xa2, 0xa3, 0xa4, 0xa5, 0xa6, 0xa7,
								  0xa8, 0xa9, 0xaa, 0xb2, 0xb3, 0xb4, 0xb5, 0xb6,
								  0xb7, 0xb8, 0xb9, 0xba, 0xc2, 0xc3, 0xc4, 0xc5,
								  0xc6, 0xc7, 0xc8, 0xc9, 0xca, 0xd2, 0xd3, 0xd4,
								  0xd5, 0xd6, 0xd7, 0xd8, 0xd9, 0xda, 0xe1, 0xe2,
								  0xe3, 0xe4, 0xe5, 0xe6, 0xe7, 0xe8, 0xe9, 0xea,
								  0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8,
								  0xf9, 0xfa };
			JPEGHuffmanTable resu = new JPEGHuffmanTable();
			resu.lengths = lengths;
			resu.symbols = symbols;
			resu.checkTable();
			return resu;
		}
	
		/// <summary>
		/// Standard Huffman table ACChrominance ( JPEG standard section K.3 )
		/// </summary>
		public static readonly JPEGHuffmanTable StdACChrominance = JPEGHuffmanTable.InitStdACChrominance();

		/// <summary>
		/// Initialize Standard Huffman table ACChrominance. 
		/// </summary>
		/// <returns>Standard Huffman table ACChrominance ( JPEG standard section K.3 )</returns>
		private static JPEGHuffmanTable InitStdACChrominance() 
		{
			short[] lengths =  { // 0-base
								   0, 0, 2, 1, 2, 4, 4, 3, 4, 7, 5, 4, 4, 0, 1, 2, 0x77 };
			short[] symbols = { 
								  0x00, 0x01, 0x02, 0x03, 0x11, 0x04, 0x05, 0x21,
								  0x31, 0x06, 0x12, 0x41, 0x51, 0x07, 0x61, 0x71,
								  0x13, 0x22, 0x32, 0x81, 0x08, 0x14, 0x42, 0x91,
								  0xa1, 0xb1, 0xc1, 0x09, 0x23, 0x33, 0x52, 0xf0,
								  0x15, 0x62, 0x72, 0xd1, 0x0a, 0x16, 0x24, 0x34,
								  0xe1, 0x25, 0xf1, 0x17, 0x18, 0x19, 0x1a, 0x26,
								  0x27, 0x28, 0x29, 0x2a, 0x35, 0x36, 0x37, 0x38,
								  0x39, 0x3a, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48,
								  0x49, 0x4a, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58,
								  0x59, 0x5a, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68,
								  0x69, 0x6a, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78,
								  0x79, 0x7a, 0x82, 0x83, 0x84, 0x85, 0x86, 0x87,
								  0x88, 0x89, 0x8a, 0x92, 0x93, 0x94, 0x95, 0x96,
								  0x97, 0x98, 0x99, 0x9a, 0xa2, 0xa3, 0xa4, 0xa5,
								  0xa6, 0xa7, 0xa8, 0xa9, 0xaa, 0xb2, 0xb3, 0xb4,
								  0xb5, 0xb6, 0xb7, 0xb8, 0xb9, 0xba, 0xc2, 0xc3,
								  0xc4, 0xc5, 0xc6, 0xc7, 0xc8, 0xc9, 0xca, 0xd2,
								  0xd3, 0xd4, 0xd5, 0xd6, 0xd7, 0xd8, 0xd9, 0xda,
								  0xe2, 0xe3, 0xe4, 0xe5, 0xe6, 0xe7, 0xe8, 0xe9,
								  0xea, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8,
								  0xf9, 0xfa };
			JPEGHuffmanTable resu = new JPEGHuffmanTable();
			resu.lengths = lengths;
			resu.symbols = symbols;
			resu.checkTable();
			return resu;
		}

		/// <summary>
		/// Private constructor used to construct the Standard Huffman tables 
		/// </summary>
		private JPEGHuffmanTable() : base()
		{
			lengths = null;
			symbols = null;
		}

		/// <summary>
		/// Creates a Huffman Table and initializes it. 
		/// </summary>
		/// <param name="lengths">lengths[k] = # of symbols with codes of length k bits; lengths[0] is ignored</param>
		/// <param name="symbols">symbols in order of increasing code length</param>
		/// <exception cref="ArgumentException">if the length of lengths is greater than 17 or if the length of symbols is greater than 256 or if any of the values in lengths or symbols is less than zero</exception>
		public JPEGHuffmanTable( short[] lengths, short[] symbols ) 
		{
			if ( lengths.Length > HUFF_MAX_LEN )
				throw new ArgumentException( "lengths array is too long" );
			for (int i=1; i < lengths.Length; i++)
				if (lengths[i] < 0)
					throw new ArgumentException
						( "Values in lengths array must be non-negative." );

		
			if ( symbols.Length > HUFF_MAX_SYM )
				throw new ArgumentException( "symbols array is too long" );
			for (int i=0; i < symbols.Length; i++)
				if (symbols[i] < 0)
					throw new ArgumentException
						( "Values in symbols array must be non-negative." );
		
			this.lengths = new short[lengths.Length];
			this.symbols = new short[symbols.Length];
		
			Array.Copy( lengths, 0, this.lengths, 0, lengths.Length );
			Array.Copy( symbols, 0, this.symbols, 0, symbols.Length );

			checkTable();
		}

		/// <summary>
		/// This checks that the table they gave us isn't 'illegal' It checks that the symbol 
		/// length counts are possible, and that they gave us at least enough symbols for 
		/// the symbol length counts. Eventually this might check that there aren't duplicate 
		/// symbols. 
		/// </summary>
		private void checkTable() 
		{
			int numVals=2;
			int sum=0;
			for (int i=1; i<lengths.Length; i++) 
			{
				sum     += lengths[i];
				numVals -= lengths[i];
				numVals *= 2;
			}

			if (numVals < 0) 
				throw new ArgumentException
					("Invalid Huffman Table provided, lengths are incorrect.");

			// I'll let them go if they gave us 'extra' symbols...
			if (sum > symbols.Length)
				throw new ArgumentException
					("Invalid Huffman Table provided, not enough symbols.");
		}

		/// <summary>
		/// Return a copy of the array containing the number of symbols for each length in the Huffman table. 
		/// </summary>
		/// <returns>A short array where array[k] = # of symbols in the table of length k. array[0] is unused</returns>
		public short[] GetLengths() 
		{
			short[] ret = new short[ lengths.Length];
			Array.Copy( lengths, 0, ret, 0, lengths.Length);
			return ret;
		}

		/// <summary>
		/// Return an array containing the Huffman symbols arranged by increasing length. 
		/// To make use of this array you must refer the the lengths array.
		/// </summary>
		/// <returns>A short array of Huffman symbols</returns>
		public short[] GetSymbols() 
		{
			short[] ret = new short[symbols.Length];
			Array.Copy( symbols, 0, ret, 0, symbols.Length);
			return ret;
		}
	}
}