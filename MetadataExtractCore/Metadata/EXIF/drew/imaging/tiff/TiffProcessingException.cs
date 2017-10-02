using System;
using com.drew.lang;

namespace com.drew.imaging.tiff
{
    /// <summary>
    /// Represents a TiffProcessing exception
    /// </summary>
    [Serializable]
    public class TiffProcessingException : CompoundException
    {
        /// <summary>
        /// Constructor of the object
        /// </summary>
        /// <param name="aMessage">The error aMessage</param>
        public TiffProcessingException(string aMessage)
            : base(aMessage)
        {
        }

        /// <summary>
        /// Constructor of the object
        /// </summary>
        /// <param name="aMessage">The error aMessage</param>
        /// <param name="aCause">The aCause of the exception</param>
        public TiffProcessingException(string aMessage, Exception aCause)
            : base(aMessage, aCause)
        {
        }

        /// <summary>
        /// Constructor of the object
        /// </summary>
        /// <param name="aCause">The aCause of the exception</param>
        public TiffProcessingException(Exception aCause)
            : base(aCause)
        {
        }
    }
}
