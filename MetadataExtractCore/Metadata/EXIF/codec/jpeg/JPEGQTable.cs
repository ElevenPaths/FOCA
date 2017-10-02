using System;

namespace com.codec.jpeg
{
	/// <summary>
	/// Class to encapsulate the JPEG quantization tables.
	/// </summary>
	public sealed class JPEGQTable
	{
		/// <summary>
		/// Quantization step for each coefficient in zig-zag order
		/// </summary>
		private int[] quantval;

		/// <summary>
		/// The number of coefficients in a DCT block
		/// </summary>
		private static readonly byte QTABLESIZE = 64;
	

		/// <summary>
		/// This is the sample luminance quantization table given in the JPEG spec 
		/// section K.1, expressed in zigzag order. The spec says that the values 
		/// given produce "good" quality, and when divided by 2, "very good" quality. 
		/// </summary>
		public static readonly JPEGQTable StdLuminance = JPEGQTable.InitStdLuminance();

		/// <summary>
		/// Initialize the StdLuminance table.
		/// </summary>
		/// <returns>the StdLuminance table.</returns>
		private static JPEGQTable InitStdLuminance() 
		{
			int[] lumVals = {
								16,   11,  12,  14,  12,  10,  16,  14,
								13,   14,  18,  17,  16,  19,  24,  40,
								26,   24,  22,  22,  24,  49,  35,  37,
								29,   40,  58,  51,  61,  60,  57,  51,
								56,   55,  64,  72,  92,  78,  64,  68,
								87,   69,  55,  56,  80, 109,  81,  87,
								95,   98, 103, 104, 103,  62,  77, 113,
								121, 112, 100, 120,  92, 101, 103,  99
							};
			JPEGQTable resu = new JPEGQTable();
			resu.quantval = lumVals;
			return resu;
		}

		/// <summary>
		/// This is the sample luminance quantization table given in the JPEG spec 
		/// section K.1, expressed in zigzag order. The spec says that the values 
		/// given produce "good" quality, and when divided by 2, "very good" quality. 
		/// </summary>
		public static readonly JPEGQTable StdChrominance = JPEGQTable.InitStdChrominance();

		/// <summary>
		/// Initialize the StdChrominance table.
		/// </summary>
		/// <returns>the StdChrominance table.</returns>
		private static JPEGQTable InitStdChrominance() 
		{
			int [] chromVals = {
								   17,  18,  18,  24,  21,  24,  47,  26,
								   26,  47,  99,  66,  56,  66,  99,  99,
								   99,  99,  99,  99,  99,  99,  99,  99,
								   99,  99,  99,  99,  99,  99,  99,  99,
								   99,  99,  99,  99,  99,  99,  99,  99,
								   99,  99,  99,  99,  99,  99,  99,  99,
								   99,  99,  99,  99,  99,  99,  99,  99,
								   99,  99,  99,  99,  99,  99,  99,  99
							   };
			JPEGQTable resu = new JPEGQTable();
			resu.quantval = chromVals;
			return resu;
		}
		
		/// <summary>
		/// Constructs an empty quantization table. This is used to create the Std Q-Tables. 
		/// </summary>
		private JPEGQTable() : base()
		{
			quantval = new int[QTABLESIZE];
		}

		/// <summary>
		/// Constructs an quantization table from the array that was passed.
		/// The coefficents must be in zig-zag order.
		/// The array must be of length 64.
		/// </summary>
		/// <param name="table">the quantization table (this is copied).</param>
		/// <exception cref="ArgumentException">if table has not a length of 64</exception>
		public JPEGQTable(int[] table ) 
		{
			if ( table.Length != QTABLESIZE ) 
			{
				throw new ArgumentException("Quantization table is the wrong size.");
			} 
			else 
			{
				quantval = new int[QTABLESIZE];
				Array.Copy(table, 0, quantval, 0, QTABLESIZE );
			}
		}
		
		/// <summary>
		/// Returns the current quantization table as an array of someInts in zig zag order. 
		/// </summary>
		/// <returns>A copy of the contained quantization table.</returns>
		public int[] GetTable() 
		{ 
			int[] table = new int[QTABLESIZE];
			Array.Copy(quantval, 0, table, 0, QTABLESIZE );
			return table;
		}

		/// <summary>
		/// Returns a new Quantization table where the values are multiplied by 
		/// scaleFactor and then clamped to the range 1..32767 (or to 1..255 if 
		/// forceBaseline is 'true'). 
		/// 
		/// Values less than one tend to improve the quality level of the table, 
		/// and values greater than one degrade the quality level of the table.
		/// </summary>
		/// <param name="scaleFactor">the multiplication lcFactor for the table</param>
		/// <param name="forceBaseline">if true the values will be clamped to the range  [1 .. 255]</param>
		/// <returns>A new Q-Table that is a linear multiple of this Q-Table</returns>
		public JPEGQTable GetScaledInstance(float scaleFactor, 
			bool forceBaseline ) 
		{
			long  max    = (forceBaseline)?255L:32767L;
			int []ret    = new int[QTABLESIZE];

			for (int i=0; i<QTABLESIZE; i++ ) 
			{
				long holder = (long)((quantval[i] * scaleFactor) + 0.5);

				// limit to valid range
				if (holder <= 0L) holder = 1L;

				// Max quantizer for 12 bits
				if (holder > max ) holder = max; 
			
				ret[i] = (int)holder;
			}
			return new JPEGQTable(ret);
		}
	}
}
