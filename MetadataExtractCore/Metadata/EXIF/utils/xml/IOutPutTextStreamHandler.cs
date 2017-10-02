using com.drew.metadata;

namespace com.utils.xml
{
    /// <summary>
    /// This class handles output text format for metadata.
    /// </summary>

    public interface IOutPutTextStreamHandler
    {
        /// <summary>
        /// Get/set the unknown option
        /// </summary>
        bool DoUnknown
        {
            get;
            set;
        }

        /// <summary>
        /// Get/set the metdata attribute
        /// </summary>
        Metadata Metadata
        {
            get;
            set;
        }

        /// <summary>
        /// Transform the Metadata object into a text stream.
        /// </summary>
        /// <returns>The Metadata object as a text stream</returns>
        string AsText();
    }
}
