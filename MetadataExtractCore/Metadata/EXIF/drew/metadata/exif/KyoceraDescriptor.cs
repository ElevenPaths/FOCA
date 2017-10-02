namespace com.drew.metadata.exif
{
    /// <summary>
    /// Tag descriptor for Kyocera
    /// </summary>

    public class KyoceraDescriptor : AbstractTagDescriptor
    {
        /// <summary>
        /// Constructor of the object
        /// </summary>
        /// <param name="aDirectory">a base.directory</param>
        public KyoceraDescriptor(AbstractDirectory aDirectory)
            : base(aDirectory)
        {
        }

        /// <summary>
        /// Returns a descriptive value of the the specified tag for this image. 
        /// Where possible, known values will be substituted here in place of the raw tokens actually 
        /// kept in the Exif segment. 
        /// If no substitution is available, the value provided by GetString(int) will be returned.
        /// This and GetString(int) are the only 'get' methods that won't throw an exception.
        /// </summary>
        /// <param name="aTagType">the tag to find a description for</param>
        /// <returns>a description of the image'str value for the specified tag, or null if the tag hasn't been defined.</returns>
        public override string GetDescription(int aTagType)
        {
            switch (aTagType)
            {
                case KyoceraDirectory.TAG_KYOCERA_PRINT_IMAGE_MATCHING_INFO:
                    return GetPrintImageMatchingInfoDescription();
                case KyoceraDirectory.TAG_KYOCERA_PROPRIETARY_THUMBNAIL:
                    return GetProprietaryThumbnailDataDescription();
                default:
                    return base.directory.GetString(aTagType);
            }
        }

        /// <summary>
        /// Returns Print Image Matching (PIM) Info Description. 
        /// </summary>
        /// <returns>the Print Image Matching (PIM) Info Description.</returns>
        private string GetPrintImageMatchingInfoDescription()
        {
            if (!base.directory.ContainsTag(KyoceraDirectory.TAG_KYOCERA_PRINT_IMAGE_MATCHING_INFO))
            {
                return null;
            }
            byte[] bytes = base.directory.GetByteArray(KyoceraDirectory.TAG_KYOCERA_PRINT_IMAGE_MATCHING_INFO);
            return BUNDLE["BYTES",bytes.Length.ToString()];
        }

        /// <summary>
        /// Returns Proprietary Thumbnail Format Data Description. 
        /// </summary>
        /// <returns>the Proprietary Thumbnail Format Data Description.</returns>
        private string GetProprietaryThumbnailDataDescription()
        {
            if (!base.directory.ContainsTag(KyoceraDirectory.TAG_KYOCERA_PROPRIETARY_THUMBNAIL))
            {
                return null;
            }
            byte[] bytes = base.directory.GetByteArray(KyoceraDirectory.TAG_KYOCERA_PROPRIETARY_THUMBNAIL);
            return BUNDLE["BYTES", bytes.Length.ToString()];
        }
    }
}
