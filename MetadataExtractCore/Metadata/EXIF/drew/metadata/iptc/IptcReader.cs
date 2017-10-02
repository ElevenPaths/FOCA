using System;
using System.IO;
using com.drew.imaging.jpg;
using com.utils;
using System.Diagnostics;

namespace com.drew.metadata.iptc
{
    /// <summary>
    /// The Iptc reader class
    /// </summary>

    public class IptcReader : AbstractMetadataReader
    {

        /// <summary>
		/// Creates a new IptcReader for the specified Jpeg jpegFile.
		/// </summary>
        /// <param name="aFile">where to read</param>
		public IptcReader(FileInfo aFile) : base(aFile, JpegSegmentReader.SEGMENT_APPD)
		{
		}

        /// <summary>
        /// Constructor of the object
        /// </summary>
        /// <param name="data">the data to read</param>
        public IptcReader(byte[] aData)
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

            AbstractDirectory lcDirectory = aMetadata.GetDirectory("com.drew.metadata.iptc.IptcDirectory");

            // find start of data
            int offset = 0;
            try
            {
                while (offset < base.data.Length - 1 && Get32Bits(offset) != 0x1c02)
                {
                    offset++;
                }
            }
            catch (MetadataException e)
            {
                lcDirectory.HasError = true;
                Trace.TraceError(
                    "Couldn't find start of Iptc data (invalid segment) ("+e.Message+")");
                return aMetadata;
            }

            // for each tag
            while (offset < base.data.Length)
            {
                // identifies start of a tag
                if (base.data[offset] != 0x1c)
                {
                    break;
                }
                // we need at least five bytes left to read a tag
                if ((offset + 5) >= base.data.Length)
                {
                    break;
                }

                offset++;

                int directoryType;
                int tagType;
                int tagByteCount;
                try
                {
                    directoryType = base.data[offset++];
                    tagType = base.data[offset++];
                    tagByteCount = Get32Bits(offset);
                }
                catch (MetadataException e)
                {
                    lcDirectory.HasError = true;
                    Trace.TraceError(
                        "Iptc data segment ended mid-way through tag descriptor ("+e.Message+")");
                    return aMetadata;
                }
                offset += 2;
                if ((offset + tagByteCount) > base.data.Length)
                {
                    lcDirectory.HasError = true;
                    Trace.TraceError(
                        "Data for tag extends beyond end of IPTC segment");
                    break;
                }

                ProcessTag(lcDirectory, directoryType, tagType, offset, tagByteCount);
                offset += tagByteCount;
            }

            return aMetadata;
        }


        /// <summary>
        /// This method serves as marsheller of objects for dataset. 
        /// It converts from IPTC octets to relevant java object.
        /// </summary>
        /// <param name="aDirectory">the directory</param>
        /// <param name="aDirectoryType">the directory type</param>
        /// <param name="aTagType">the tag type</param>
        /// <param name="anOffset">the lcOffset</param>
        /// <param name="aTagByteCount">the tag byte count</param>
        private void ProcessTag(
            AbstractDirectory aDirectory,
            int aDirectoryType,
            int aTagType,
            int anOffset,
            int aTagByteCount)
        {
            int tagIdentifier = aTagType | (aDirectoryType << 8);

            switch (tagIdentifier)
            {
                case IptcDirectory.TAG_RECORD_VERSION:
                    // short
                    short shortValue = (short)((base.data[anOffset] << 8) | base.data[anOffset + 1]);
                    aDirectory.SetObject(tagIdentifier, shortValue);
                    return;
                case IptcDirectory.TAG_URGENCY:
                    // byte
                    aDirectory.SetObject(tagIdentifier, base.data[anOffset]);
                    return;
                case IptcDirectory.TAG_RELEASE_DATE:
                case IptcDirectory.TAG_DATE_CREATED:
                    // Date object
                    if (aTagByteCount >= 8)
                    {
                        string dateStr = Utils.Decode(base.data, anOffset, aTagByteCount, false);
                        try
                        {
                            int year = Convert.ToInt32(dateStr.Remove(4));
                            int month = Convert.ToInt32(dateStr.Substring(4, 2)); //No -1 here;
                            int day = Convert.ToInt32(dateStr.Substring(6, 2));
                            DateTime date = new DateTime(year, month, day);
                            aDirectory.SetObject(tagIdentifier, date);
                            return;
                        }
                        catch (FormatException)
                        {
                            // fall through and we'll store whatever was there as a String
                        }
                    }
                    break; // Added for .Net compiler
                //case IptcDirectory.TAG_RELEASE_TIME:
                //case IptcDirectory.TAG_TIME_CREATED: 
            }
            // If no special handling by now, treat it as a string
            string str = null;
            if (aTagByteCount < 1)
            {
                str = "";
            }
            else
            {
                str = Utils.Decode(base.data, anOffset, aTagByteCount, false);
            }
            if (aDirectory.ContainsTag(tagIdentifier))
            {
                string[] oldStrings;
                string[] newStrings;
                try
                {
                    oldStrings = aDirectory.GetStringArray(tagIdentifier);
                }
                catch (MetadataException)
                {
                    oldStrings = null;
                }
                if (oldStrings == null)
                {
                    newStrings = new String[1];
                }
                else
                {
                    newStrings = new string[oldStrings.Length + 1];
                    for (int i = 0; i < oldStrings.Length; i++)
                    {
                        newStrings[i] = oldStrings[i];
                    }
                }
                newStrings[newStrings.Length - 1] = str;
                aDirectory.SetObject(tagIdentifier, newStrings);
            }
            else
            {
                aDirectory.SetObject(tagIdentifier, str);
            }
        }
    }
}