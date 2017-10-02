using System;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace com.utils
{
	/// <summary>
	/// Class that try to recreate some Java functionnalities.
	/// </summary>

	public sealed class Utils
	{
		/// <summary>
		/// Constructor of the object
		/// </summary>
		/// <exception cref="UnauthorizedAccessException">always</exception>
        private Utils()
            : base()
        {
            throw new UnauthorizedAccessException("Do not use");
        }

		/// <summary>
		/// Builds a string from a byte array
		/// </summary>
		/// <param name="anArray">the array of byte</param>
		/// <param name="lcOffset">where to start</param>
		/// <param name="length">the length to transform in string</param>
		/// <param name="removeSpace">if true, spaces will be avoid</param>
		/// <returns>a string representing the array of byte</returns>
		public static string Decode(byte[] anArray, int offset, int length, bool removeSpace) 
		{
			StringBuilder sb = new StringBuilder(length);
			for(int i=offset; i<length+offset; i++) 
			{
				char aChar = (char)anArray[i];
				if (removeSpace && (anArray[i] == 0)) 
				{
					continue;
				}
                // Do not take '\r' char
                if (anArray[i] == (int)'\r')
                {
                    // Put a space instead
                    sb.Append(' ');
                    continue;
                }

				sb.Append(aChar);
			}
			return sb.ToString();
		}

		/// <summary>
		/// Builds a string from a byte array
		/// </summary>
		/// <param name="anArray">the array of byte</param>
		/// <param name="removeSpace">if true, spaces will be avoid</param>
		/// <returns>a string representing the array of byte</returns>
		public static string Decode(byte[] anArray, bool removeSpace) 
		{
			return Decode(anArray, 0, anArray.Length, removeSpace);
		}

		/// <summary>
		/// Search for files in the given directory.
		/// </summary>
        /// <param name="aRootDirectory">Where to start the search</param>
        /// <param name="doRecurse">if true will do sub directories as well</param>
        /// <param name="aSearchPattern">if not null will take only file with the given axtension (ex "*.jpg")</param>
		/// <returns>a list of file name</returns>
        public static List<string> SearchAllFileIn(String aRootDirectory, bool doRecurse, string aSearchPattern)
        {
            List<string> lcResult = new List<string>();
            if (Directory.Exists(aRootDirectory))
            {
                string[] lc2List = Directory.GetFiles(aRootDirectory, aSearchPattern, (doRecurse) ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                for (int i = 0; i < lc2List.Length; i++)
                {
                    lcResult.Add(lc2List[i]);
                }
            }
            return lcResult;
        }
	}
}
