using System;
using System.Collections.Generic;
using System.IO;
using com.drew.imaging.jpg;
using com.drew.lang;
using com.utils;
using System.Diagnostics;

namespace com.drew.metadata.exif
{
    /// <summary>
    /// Extracts Exif data from a JPEG lcHeader segment, providing information about 
    /// the camera/scanner/capture device (if available).  
    /// Information is encapsulated in an Metadata object.
    /// </summary>

    public class ExifReader : AbstractMetadataReader
    {

        /// <summary>
        /// Represents the native byte ordering used in the JPEG segment.
        /// If true, then we're using Motorolla ordering (Big endian), else 
        /// we're using Intel ordering (Little endian).
        /// </summary>
        private bool isMotorollaByteOrder;

        /// <summary>
        /// Bean instance to store information about the image and camera/scanner/capture device.
        /// </summary>
        private Metadata metadata;
        private ExifDirectory _exifDirectory;
        private ExifDirectory ExifDirectory
        {
            get
            {
                if (this._exifDirectory == null)
                {
                    this._exifDirectory = (ExifDirectory)this.metadata.GetDirectory("com.drew.metadata.exif.ExifDirectory");

                }
                return this._exifDirectory;
            }
        }


        /// <summary>
        /// The number of bytes used per format descriptor.
        /// </summary>
        private static readonly int[] BYTES_PER_FORMAT = { 0, 1, 1, 2, 4, 8, 1, 1, 2, 4, 8, 4, 8 };

        /// <summary>
        /// The number of formats known.
        /// </summary>
        private static readonly int MAX_FORMAT_CODE = 12;

        // the format enumeration
        // TODO use the new DataFormat enumeration instead of these values
        private const int FMT_BYTE = 1;
        private const int FMT_STRING = 2;
        private const int FMT_USHORT = 3;
        private const int FMT_ULONG = 4;
        private const int FMT_URATIONAL = 5;
        private const int FMT_SBYTE = 6;
        private const int FMT_UNDEFINED = 7;
        private const int FMT_SSHORT = 8;
        private const int FMT_SLONG = 9;
        private const int FMT_SRATIONAL = 10;
        private const int FMT_SINGLE = 11;
        private const int FMT_DOUBLE = 12;

        public const int TAG_EXIF_OFFSET = 0x8769;
        public const int TAG_INTEROP_OFFSET = 0xA005;
        public const int TAG_GPS_INFO_OFFSET = 0x8825;
        public const int TAG_MAKER_NOTE = 0x927C;

        // NOT READONLY
        public static int TIFF_HEADER_START_OFFSET = 6;

        private const string MARK_AS_PROCESSED = "processed";


        /// <summary>
		/// Creates a new ExifReader for the specified Jpeg jpegFile.
		/// </summary>
        /// <param name="aFile">where to read</param>
        public ExifReader(FileInfo aFile)
            : base(aFile, JpegSegmentReader.SEGMENT_APP1)
		{
		}

        /// <summary>
        /// Constructor of the object
        /// </summary>
        /// <param name="data">the data to read</param>
        public ExifReader(byte[] aData)
            : base(aData)
        {
        }

        /// <summary>
        /// Extract tiff information (used by raw files)
        /// </summary>
        /// <param name="aMetadata">where to extract information</param>
        /// <returns>the information extracted</returns>
        public Metadata ExtractTiff(Metadata aMetadata)
        {
            return this.ExtractIFD(aMetadata, 0);
        }

        /// <summary>
        /// Reads metatdata from raw file.
        /// </summary>
        /// <param name="aMetadata">a meta data</param>
        /// <param name="aTiffHeaderOffset">an offset</param>
        /// <returns>the metadata found</returns>
        private Metadata ExtractIFD(Metadata aMetadata, int aTiffHeaderOffset)
        {
            this.metadata = aMetadata;
            if (base.data == null)
            {
                return this.metadata;
            }

            ExifDirectory directory = this.ExifDirectory;

            // this should be either "MM" or "II"
            string byteOrderIdentifier = Utils.Decode(base.data, aTiffHeaderOffset, 2, false);
            if (!this.SetByteOrder(byteOrderIdentifier))
            {
                directory.HasError = true;
                Trace.TraceError("Unclear distinction between Motorola/Intel byte ordering: "
                        + byteOrderIdentifier);
                return this.metadata;
            }

            // Check the next two values for correctness.
            if (this.Get16Bits(2 + aTiffHeaderOffset) != 0x2a)
            {
              //  directory.AddError("Invalid Exif start - should have 0x2A at offset 8 in Exif header");
              //  return this.metadata;
            }

            int firstDirectoryOffset = this.Get32Bits(4 + aTiffHeaderOffset) + aTiffHeaderOffset;

            // David Ekholm sent an digital camera image that has this problem
            if (firstDirectoryOffset >= base.data.Length - 1)
            {
                directory.HasError = true;
                Trace.TraceError("First exif directory offset is beyond end of Exif data segment");
                // First directory normally starts 14 bytes in -- try it here and catch another error in the worst case
                firstDirectoryOffset = 14;
            }

            IDictionary<int, string> processedDirectoryOffsets = new Dictionary<int, string>();

            // 0th IFD (we merge with Exif IFD)
            try
            {
                this.ProcessDirectory(directory, processedDirectoryOffsets,
                    firstDirectoryOffset, aTiffHeaderOffset);
            }
            catch (Exception e)
            {
                throw new MetadataException(e);
            }

            // after the extraction process, if we have the correct tags, we may be able to store thumbnail information
            this.StoreThumbnailBytes(directory, aTiffHeaderOffset);

            return this.metadata;
        }


        /// <summary>
        /// Performs the Exif data extraction, adding found values to the specified instance of Metadata.
        /// </summary>
        /// <param name="aMetadata">where to add meta data</param>
        /// <returns>the aMetadata</returns>
        public override Metadata Extract(Metadata metadata)
        {
            this.metadata = metadata;
            if (base.data == null)
            {
                return this.metadata;
            }

            // once we know there'str some data, create the directory and start working on it
            AbstractDirectory directory = this.metadata.GetDirectory("com.drew.metadata.exif.ExifDirectory");

            if (base.data.Length <= 14)
            {
                directory.HasError = true;
                Trace.TraceError("Exif data segment must contain at least 14 bytes");
                return this.metadata;
            }
            if (!"Exif\0\0".Equals(Utils.Decode(base.data, 0, 6, false)))
            {
                directory.HasError = true;
                Trace.TraceError("Exif data segment doesn't begin with 'Exif'");
                return this.metadata;
            }

            // this should be either "MM" or "II"
            string byteOrderIdentifier = Utils.Decode(base.data, 6, 2, false);
            if (!SetByteOrder(byteOrderIdentifier))
            {
                directory.HasError = true;
                Trace.TraceError("Unclear distinction between Motorola/Intel byte ordering");
                return this.metadata;
            }

            // Check the next two values for correctness.
            if (Get16Bits(8) != 0x2a)
            {
                directory.HasError = true;
                Trace.TraceError("Invalid Exif start - should have 0x2A at offSet 8 in Exif header");
                return this.metadata;
            }

            int firstDirectoryOffSet = Get32Bits(10) + TIFF_HEADER_START_OFFSET;

            // David Ekholm sent an digital camera image that has this problem
            if (firstDirectoryOffSet >= base.data.Length - 1)
            {
                directory.HasError = true;
                Trace.TraceError("First exif directory offSet is beyond end of Exif data segment");
                // First directory normally starts 14 bytes in -- try it here and catch another error in the worst case
                firstDirectoryOffSet = 14;
            }

            // 0th IFD (we merge with Exif IFD)
            //ProcessDirectory(directory, firstDirectoryOffSet);
            // after the extraction process, if we have the correct tags, we may be able to extract thumbnail information
            //ExtractThumbnail(directory);

            Dictionary<int, string> processedDirectoryOffsets = new Dictionary<int, string>();

            // 0th IFD (we merge with Exif IFD)
            ProcessDirectory(directory, processedDirectoryOffsets, firstDirectoryOffSet, TIFF_HEADER_START_OFFSET);

            // after the extraction process, if we have the correct tags, we may be able to store thumbnail information
            StoreThumbnailBytes(directory, TIFF_HEADER_START_OFFSET);


            return this.metadata;
        }

        /// <summary>
        /// Will stock the thumbnail into exif directory if available.
        /// </summary>
        /// <param name="exifDirectory">where to stock the thumbnail</param>
        /// <param name="tiffHeaderOffset">the tiff lcHeader lcOffset value</param>
        private void StoreThumbnailBytes(AbstractDirectory exifDirectory, int tiffHeaderOffset)
        {
            if (!exifDirectory.ContainsTag(ExifDirectory.TAG_COMPRESSION))
            {
                return;
            }

            if (!exifDirectory.ContainsTag(ExifDirectory.TAG_THUMBNAIL_LENGTH) ||
                !exifDirectory.ContainsTag(ExifDirectory.TAG_THUMBNAIL_OFFSET))
            {
                return;
            }
            try
            {
                int offset = exifDirectory.GetInt(ExifDirectory.TAG_THUMBNAIL_OFFSET);
                int length = exifDirectory.GetInt(ExifDirectory.TAG_THUMBNAIL_LENGTH);
                byte[] result = new byte[length];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = base.data[tiffHeaderOffset + offset + i];
                }
                exifDirectory.SetObject(ExifDirectory.TAG_THUMBNAIL_DATA, result);
            }
            catch (Exception e)
            {
                exifDirectory.HasError = true;
                Trace.TraceError("Unable to extract thumbnail: " + e.Message);
            }
        }


        /// <summary>
        /// Sets Motorolla byte order and idicates that it was found.
        /// </summary>
        /// <param name="byteOrderIdentifier">true if the Motorolla byte order is identified</param>
        /// <returns></returns>
        private bool SetByteOrder(string byteOrderIdentifier)
        {
            if ("MM".Equals(byteOrderIdentifier))
            {
                this.isMotorollaByteOrder = true;
                return true;
            }
            else if ("II".Equals(byteOrderIdentifier))
            {
                this.isMotorollaByteOrder = false;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Indicates if Directory Length is valid or not
        /// </summary>
        /// <param name="dirStartOffSet">where to start</param>
        /// <param name="tiffHeaderOffset">The tiff lcHeader lcOffset</param>
        /// <returns>true if Directory Length is valid</returns>
        private bool IsDirectoryLengthValid(int dirStartOffset, int tiffHeaderOffset)
        {
            int dirTagCount = Get16Bits(dirStartOffset);
            int dirLength = (2 + (12 * dirTagCount) + 4);
            // Note: Files that had thumbnails trimmed with jhead 1.3 or earlier might trigger this
            return !(dirLength + dirStartOffset + tiffHeaderOffset >= base.data.Length);
        }

        /// <summary>
        /// Determine the lcOffset at which a given InteropArray entry begins within the specified IFD. 
        /// </summary>
        /// <param name="dirStartOffset">the lcOffset at which the IFD starts</param>
        /// <param name="entryNumber">the zero-based entry number</param>
        /// <returns>the lcOffset at which a given InteropArray entry begins within the specified IFD</returns>
        private int CalculateTagOffset(int dirStartOffset, int entryNumber)
        {
            // add 2 bytes for the tag count
            // each entry is 12 bytes, so we skip 12 * the number seen so far
            return dirStartOffset + 2 + (12 * entryNumber);
        }

        /// <summary>
        /// Calculates tag value lcOffset
        /// </summary>
        /// <param name="byteCount">the byte count</param>
        /// <param name="dirEntryOffset">the dir entry lcOffset</param>
        /// <param name="tiffHeaderOffset">the tiff lcHeader ofset</param>
        /// <returns>-1 if error, or the valus lcOffset</returns>
        private int CalculateTagValueOffset(int byteCount, int dirEntryOffset, int tiffHeaderOffset)
        {
            if (byteCount > 4)
            {
                // If its bigger than 4 bytes, the dir entry contains an lcOffset.
                // dirEntryOffset must be passed, as some makernote implementations (e.g. FujiFilm) incorrectly use an
                // lcOffset relative to the start of the makernote itself, not the TIFF segment.
                int offsetVal = Get32Bits(dirEntryOffset + 8);
                if (offsetVal + byteCount > base.data.Length)
                {
                    // Bogus pointer lcOffset and / or bytecount value
                    return -1; // signal error
                }
                return tiffHeaderOffset + offsetVal;
            }
            // 4 bytes or less and value is in the dir entry itself
            return dirEntryOffset + 8;
        }


        /// <summary>
        /// Process one of the nested Tiff IFD directories.
        /// 2 bytes: number of tags for each tag
        ///		2 bytes: tag type	
        ///		2 bytes: format code	
        /// 	4 bytes: component count
        /// </summary>
        /// <param name="directory">the directory</param>
        /// <param name="dirStartOffSet">where to start</param>
        private void ProcessDirectory(AbstractDirectory directory, IDictionary<int, string> processedDirectoryOffsets, int dirStartOffset, int tiffHeaderOffset)
        {
            // check for directories we've already visited to avoid stack overflows when recursive/cyclic directory structures exist
            if (processedDirectoryOffsets.ContainsKey(dirStartOffset))
            {
                return;
            }
            // remember that we've visited this directory so that we don't visit it again later
            processedDirectoryOffsets.Add(dirStartOffset, MARK_AS_PROCESSED);

            if (dirStartOffset >= base.data.Length || dirStartOffset < 0)
            {
                directory.HasError = true;
                Trace.TraceError("Ignored directory marked to start outside data segement");
                return;
            }

            if (!IsDirectoryLengthValid(dirStartOffset, tiffHeaderOffset))
            {
                directory.HasError = true;
                Trace.TraceError("Illegally sized directory");
                return;
            }

            // First two bytes in the IFD are the tag count
            int dirTagCount = Get16Bits(dirStartOffset);

            // Handle each tag in this directory
            for (int tagNumber = 0; tagNumber < dirTagCount; tagNumber++)
            {
                int tagOffset = CalculateTagOffset(dirStartOffset, tagNumber);

                // 2 bytes for the tag type
                int tagType = Get16Bits(tagOffset);

                // 2 bytes for the format code
                int formatCode = Get16Bits(tagOffset + 2);
                if (formatCode < 1 || formatCode > MAX_FORMAT_CODE)
                {
                    directory.HasError = true;
                    Trace.TraceError("Invalid format code: " + formatCode);
                    continue;
                }

                // 4 bytes dictate the number of components in this tag'lcStr data
                int componentCount = Get32Bits(tagOffset + 4);
                if (componentCount < 0)
                {
                    directory.HasError = true;
                    Trace.TraceError("Negative component count in EXIF");
                    continue;
                }

                // each component may have more than one byte... calculate the total number of bytes
                int byteCount = componentCount * BYTES_PER_FORMAT[formatCode];
                int tagValueOffset = CalculateTagValueOffset(byteCount, tagOffset, tiffHeaderOffset);
                if (tagValueOffset < 0 || tagValueOffset > base.data.Length)
                {
                    directory.HasError = true;
                    Trace.TraceError("Illegal pointer offset value in EXIF");
                    continue;
                }


                // Check that this tag isn't going to allocate outside the bounds of the data array.
                // This addresses an uncommon OutOfMemoryError.
                if (byteCount < 0 || tagValueOffset + byteCount > base.data.Length)
                {
                    directory.HasError = true;
                    Trace.TraceError("Illegal number of bytes: " + byteCount);
                    continue;
                }

                // Calculate the value as an lcOffset for cases where the tag represents directory
                int subdirOffset = tiffHeaderOffset + Get32Bits(tagValueOffset);

                switch (tagType)
                {
                    case TAG_EXIF_OFFSET:
                        ProcessDirectory(this.metadata.GetDirectory("com.drew.metadata.exif.ExifDirectory"), processedDirectoryOffsets, subdirOffset, tiffHeaderOffset);
                        continue;
                    case TAG_INTEROP_OFFSET:
                        ProcessDirectory(this.metadata.GetDirectory("com.drew.metadata.exif.ExifInteropDirectory"), processedDirectoryOffsets, subdirOffset, tiffHeaderOffset);
                        continue;
                    case TAG_GPS_INFO_OFFSET:
                        ProcessDirectory(this.metadata.GetDirectory("com.drew.metadata.exif.GpsDirectory"), processedDirectoryOffsets, subdirOffset, tiffHeaderOffset);
                        continue;
                    case TAG_MAKER_NOTE:
                        ProcessMakerNote(tagValueOffset, processedDirectoryOffsets, tiffHeaderOffset);
                        continue;
                    default:
                        ProcessTag(directory, tagType, tagValueOffset, componentCount, formatCode);
                        break;
                }
            } // End of for
            // at the end of each IFD is an optional link to the next IFD
            int finalTagOffset = CalculateTagOffset(dirStartOffset, dirTagCount);
            int nextDirectoryOffset = Get32Bits(finalTagOffset);
            if (nextDirectoryOffset != 0)
            {
                nextDirectoryOffset += tiffHeaderOffset;
                if (nextDirectoryOffset >= base.data.Length)
                {
                    Trace.TraceWarning("Last 4 bytes of IFD reference another IFD with an address that is out of bounds\nNote this could have been caused by jhead 1.3 cropping too much");
                    return;
                }
                else if (nextDirectoryOffset < dirStartOffset)
                {
                    Trace.TraceWarning("Last 4 bytes of IFD reference another IFD with an address that is before the start of this directory");
                    return;
                }
                // the next directory is of same type as this one
                ProcessDirectory(directory, processedDirectoryOffsets, nextDirectoryOffset, tiffHeaderOffset);
            }
        }

        /// <summary>
        /// Determine the camera model and makernote format
        /// </summary>
        /// <param name="subdirOffset">the sub lcOffset dir</param>
        /// <param name="processedDirectoryOffsets">the processed directory offsets</param>
        /// <param name="tiffHeaderOffset">the tiff lcHeader lcOffset</param>
        private void ProcessMakerNote(int subdirOffset, IDictionary<int, string> processedDirectoryOffsets, int tiffHeaderOffset)
        {
            // Console.WriteLine("ProcessMakerNote value="+subdirOffSet);
            // Determine the camera model and makernote format
            AbstractDirectory exifDirectory = this.metadata.GetDirectory("com.drew.metadata.exif.ExifDirectory");
            if (exifDirectory == null)
            {
                return;
            }

            string cameraModel = exifDirectory.GetString(ExifDirectory.TAG_MAKE);
            string firstTwoChars = Utils.Decode(base.data, subdirOffset, 2, false);
            string firstThreeChars = Utils.Decode(base.data, subdirOffset, 3, false);
            string firstFourChars = Utils.Decode(base.data, subdirOffset, 4, false);
            string firstFiveChars = Utils.Decode(base.data, subdirOffset, 5, false);
            string firstSixChars = Utils.Decode(base.data, subdirOffset, 6, false);
            string firstSevenChars = Utils.Decode(base.data, subdirOffset, 7, false);
            string firstEightChars = Utils.Decode(base.data, subdirOffset, 8, false);

            if ("OLYMP".Equals(firstFiveChars) || "EPSON".Equals(firstFiveChars) || "AGFA".Equals(firstFourChars))
            {
                Trace.TraceInformation("Found an Olympus/Epson/Agfa directory.");
                // Olympus Makernote
                // Epson and Agfa use Olypus maker note standard, see:
                //     http://www.ozhiker.com/electronics/pjmt/jpeg_info/
                ProcessDirectory(
                    this.metadata.GetDirectory("com.drew.metadata.exif.OlympusDirectory"), processedDirectoryOffsets, subdirOffset + 8, tiffHeaderOffset);
            }
            else if (cameraModel != null && cameraModel.Trim().ToUpper().StartsWith("NIKON"))
            {
                if ("Nikon".Equals(Utils.Decode(base.data, subdirOffset, 5, false)))
                {
                    // There are two scenarios here:
                    // Type 1:
                    // :0000: 4E 69 6B 6F 6E 00 01 00-05 00 02 00 02 00 06 00 Nikon...........
                    // :0010: 00 00 EC 02 00 00 03 00-03 00 01 00 00 00 06 00 ................
                    // Type 3:
                    // :0000: 4E 69 6B 6F 6E 00 02 00-00 00 4D 4D 00 2A 00 00 Nikon....MM.*...
                    // :0010: 00 08 00 1E 00 01 00 07-00 00 00 04 30 32 30 30 ............0200
                    if (base.data[subdirOffset + 6] == 1)
                    {
                        Trace.TraceInformation("Found an Nykon Type 1 directory.");
                        ProcessDirectory(
                            this.metadata.GetDirectory("com.drew.metadata.exif.NikonType1Directory"), processedDirectoryOffsets, subdirOffset + 8, tiffHeaderOffset);
                    }
                    else if (base.data[subdirOffset + 6] == 2)
                    {
                        Trace.TraceInformation("Found an Nykon Type 2 directory.");
                        ProcessDirectory(
                            this.metadata.GetDirectory("com.drew.metadata.exif.NikonType2Directory"), processedDirectoryOffsets, subdirOffset + 18, subdirOffset + 10);
                    }
                    else
                    {
                        exifDirectory.HasError = true;
                        Trace.TraceError(
                            "Unsupported makernote for Nikon data ignored.");
                    }
                }
                else
                {
                    Trace.TraceInformation("Found an Nykon Type 2 directory.");
                    ProcessDirectory(
                        this.metadata.GetDirectory("com.drew.metadata.exif.NikonType2Directory"), processedDirectoryOffsets, subdirOffset, tiffHeaderOffset);
                }
            }
            else if ("SONY CAM".Equals(firstEightChars) || "SONY DSC".Equals(firstEightChars))
            {
                Trace.TraceInformation("Found a Sony directory.");
                ProcessDirectory(
                    this.metadata.GetDirectory("com.drew.metadata.exif.SonyDirectory"), processedDirectoryOffsets, subdirOffset + 12, tiffHeaderOffset);
            }
            else if ("KDK".Equals(firstThreeChars))
            {
                Trace.TraceInformation("Found a Kodak directory.");
                ProcessDirectory(
                    this.metadata.GetDirectory("com.drew.metadata.exif.KodakDirectory"), processedDirectoryOffsets, subdirOffset + 20, tiffHeaderOffset);
            }


            else if (cameraModel != null && "Canon".ToUpper().Equals(cameraModel.ToUpper()))
            {
                Trace.TraceInformation("Found a Canon directory.");
                ProcessDirectory(
                    this.metadata.GetDirectory("com.drew.metadata.exif.CanonDirectory"), processedDirectoryOffsets, subdirOffset, tiffHeaderOffset);
            }
            else if (cameraModel != null && cameraModel.ToUpper().StartsWith("CASIO"))
            {
                if ("QVC\u0000\u0000\u0000".Equals(firstSixChars))
                {
                    Trace.TraceInformation("Found a Casion Type 2 directory.");
                    ProcessDirectory(this.metadata.GetDirectory("com.drew.metadata.exif.CasioType2Directory"), processedDirectoryOffsets, subdirOffset + 6, tiffHeaderOffset);
                }
                else
                {
                    Trace.TraceInformation("Found a Casion Type 1 directory.");
                    ProcessDirectory(this.metadata.GetDirectory("com.drew.metadata.exif.CasioType1Directory"), processedDirectoryOffsets, subdirOffset, tiffHeaderOffset);
                }
            }
            else if ("FUJIFILM".Equals(firstEightChars) || "Fujifilm".ToUpper().Equals(cameraModel.ToUpper()))
            {
                Trace.TraceInformation("Found a Fujifilm directory.");
                // TODO make this field a passed parameter, to avoid threading issues
                bool byteOrderBefore = this.isMotorollaByteOrder;
                // bug in fujifilm makernote ifd means we temporarily use Intel byte ordering
                this.isMotorollaByteOrder = false;
                // the 4 bytes after "FUJIFILM" in the makernote point to the start of the makernote
                // IFD, though the lcOffset is relative to the start of the makernote, not the TIFF
                // lcHeader (like everywhere else)
                int ifdStart = subdirOffset + Get32Bits(subdirOffset + 8);
                ProcessDirectory(this.metadata.GetDirectory("com.drew.metadata.exif.FujifilmDirectory"), processedDirectoryOffsets, ifdStart, tiffHeaderOffset);
                this.isMotorollaByteOrder = byteOrderBefore;
            }
            else if (cameraModel != null && cameraModel.ToUpper().StartsWith("MINOLTA"))
            {
                Trace.TraceInformation("Found a Minolta directory, will use Olympus directory.");
                // Cases seen with the model starting with MINOLTA in capitals seem to have a valid Olympus makernote
                // area that commences immediately.
                ProcessDirectory(this.metadata.GetDirectory("com.drew.metadata.exif. OlympusDirectory"), processedDirectoryOffsets, subdirOffset, tiffHeaderOffset);
            }
            else if ("KC".Equals(firstTwoChars) || "MINOL".Equals(firstFiveChars) || "MLY".Equals(firstThreeChars) || "+M+M+M+M".Equals(firstEightChars))
            {
                // This Konica data is not understood.  Header identified in accordance with information at this site:
                // http://www.ozhiker.com/electronics/pjmt/jpeg_info/minolta_mn.html
                // TODO determine how to process the information described at the above website
                Trace.TraceError("Unsupported Konica/Minolta data ignored.");
            }
            else if ("KYOCERA".Equals(firstSevenChars))
            {
                Trace.TraceInformation("Found a Kyocera directory");
                // http://www.ozhiker.com/electronics/pjmt/jpeg_info/kyocera_mn.html
                ProcessDirectory(this.metadata.GetDirectory("com.drew.metadata.exif.KyoceraDirectory"), processedDirectoryOffsets, subdirOffset + 22, tiffHeaderOffset);
            }
            else if ("Panasonic\u0000\u0000\u0000".Equals(Utils.Decode(base.data, subdirOffset, 12, false)))
            {
                Trace.TraceInformation("Found a panasonic directory");
                // NON-Standard TIFF IFD Data using Panasonic Tags. There is no Next-IFD pointer after the IFD
                // Offsets are relative to the start of the TIFF lcHeader at the beginning of the EXIF segment
                // more information here: http://www.ozhiker.com/electronics/pjmt/jpeg_info/panasonic_mn.html
                ProcessDirectory(this.metadata.GetDirectory("com.drew.metadata.exif.PanasonicDirectory"), processedDirectoryOffsets, subdirOffset + 12, tiffHeaderOffset);
            }
            else if ("AOC\u0000".Equals(firstFourChars))
            {
                Trace.TraceInformation("Found a Casio type 2 directory");
                // NON-Standard TIFF IFD Data using Casio Type 2 Tags
                // IFD has no Next-IFD pointer at end of IFD, and
                // Offsets are relative to the start of the current IFD tag, not the TIFF lcHeader
                // Observed for:
                // - Pentax ist D
                ProcessDirectory(this.metadata.GetDirectory("com.drew.metadata.exif.CasioType2Directory"), processedDirectoryOffsets, subdirOffset + 6, subdirOffset);
            }
            else if (cameraModel != null && (cameraModel.ToUpper().StartsWith("PENTAX") || cameraModel.ToUpper().StartsWith("ASAHI")))
            {
                Trace.TraceInformation("Found a Pentax directory");
                // NON-Standard TIFF IFD Data using Pentax Tags
                // IFD has no Next-IFD pointer at end of IFD, and
                // Offsets are relative to the start of the current IFD tag, not the TIFF lcHeader
                // Observed for:
                // - PENTAX Optio 330
                // - PENTAX Optio 430
                ProcessDirectory(this.metadata.GetDirectory("com.drew.metadata.exif.PentaxDirectory"), processedDirectoryOffsets, subdirOffset, subdirOffset);
            }
            else
            {
                // TODO how to store makernote data when it'str not from a supported camera model?
                Trace.TraceError("Unsupported directory data ignored.");
            }
        }


        /// <summary>
        /// Processes tag
        /// </summary>
        /// <param name="directory">the directory</param>
        /// <param name="aTagType">the tag type</param>
        /// <param name="tagValueOffset">the lcOffset value</param>
        /// <param name="componentCount">the component count</param>
        /// <param name="formatCode">the format code</param>
        private void ProcessTag(
            AbstractDirectory directory,
            int tagType,
            int tagValueOffset,
            int componentCount,
            int formatCode)
        {
            // Directory simply stores raw values
            // The display side uses a Descriptor class per directory to turn the raw values into 'pretty' descriptions
            switch (formatCode)
            {
                case FMT_UNDEFINED:
                    Debug.Write("Found a tag made of bytes");
                    // this includes exif user comments
                    byte[] tagBytes = new byte[componentCount];
                    int byteCount = componentCount * BYTES_PER_FORMAT[formatCode];
                    for (int i = 0; i < byteCount; i++)
                    {
                        tagBytes[i] = base.data[tagValueOffset + i];
                    }
                    directory.SetObject(tagType, tagBytes);
                    break;
                case FMT_STRING:
                    Debug.Write("Found a tag made of string");
                    string lcStr = null;
                    if (tagType == ExifDirectory.TAG_USER_COMMENT)
                    {
                        lcStr =
                            ReadCommentString(
                            tagValueOffset,
                            componentCount,
                            formatCode);
                    }
                    else
                    {
                        lcStr = ReadString(tagValueOffset, componentCount);
                    }
                    directory.SetObject(tagType, lcStr);
                    break;
                case FMT_SRATIONAL: //goto case FMT_URATIONAL;
                case FMT_URATIONAL:
                    if (componentCount == 1)
                    {
                        Debug.Write("Found a tag made of rational");
                        Rational rational = new Rational(Get32Bits(tagValueOffset), Get32Bits(tagValueOffset + 4));
                        directory.SetObject(tagType, rational);

                    }
                    else
                    {
                        Debug.Write("Found a tag made of rationals");
                        Rational[] rationals = new Rational[componentCount];
                        for (int i = 0; i < componentCount; i++)
                        {
                            rationals[i] = new Rational(Get32Bits(tagValueOffset + (8 * i)), Get32Bits(tagValueOffset + 4 + (8 * i)));
                        }
                        directory.SetObject(tagType, rationals);
                        
                    }

                    break;
                case FMT_SBYTE: //goto case FMT_BYTE;
                case FMT_BYTE:
                    if (componentCount == 1)
                    {
                        Debug.Write("Found a tag made of byte");
                        // this may need to be a byte, but I think casting to int is fine
                        int b = base.data[tagValueOffset];
                        directory.SetObject(tagType, b);
                    }
                    else
                    {
                        Debug.Write("Found a tag made of bytes but will use ints");
                        int[] bytes = new int[componentCount];
                        for (int i = 0; i < componentCount; i++)
                        {
                            bytes[i] = base.data[tagValueOffset + i];
                        }
                        directory.SetIntArray(tagType, bytes);
                    }
                    break;
                case FMT_SINGLE: //goto case FMT_DOUBLE;
                case FMT_DOUBLE:
                    if (componentCount == 1)
                    {
                        Debug.Write("Found a tag made of double but will use int");
                        int i = base.data[tagValueOffset];
                        directory.SetObject(tagType, i);
                    }
                    else
                    {
                        Debug.Write("Found a tag made of doubles but will use ints");
                        int[] ints = new int[componentCount];
                        for (int i = 0; i < componentCount; i++)
                        {
                            ints[i] = base.data[tagValueOffset + i];
                        }
                        directory.SetIntArray(tagType, ints);
                    }
                    break;
                case FMT_USHORT: //goto case FMT_SSHORT;
                case FMT_SSHORT:
                    if (componentCount == 1)
                    {
                        Debug.Write("Found a tag made of short but will use int");
                        int i = Get16Bits(tagValueOffset);
                        directory.SetObject(tagType, i);
                    }
                    else
                    {
                        Debug.Write("Found a tag made of shorts but will use ints");
                        int[] ints = new int[componentCount];
                        for (int i = 0; i < componentCount; i++)
                        {
                            ints[i] = Get16Bits(tagValueOffset + (i * 2));
                        }
                        directory.SetIntArray(tagType, ints);
                    }
                    break;
                case FMT_SLONG: //goto case FMT_ULONG;
                case FMT_ULONG:
                    if (componentCount == 1)
                    {
                        Debug.Write("Found a tag made of long but will use int");
                        int i = Get32Bits(tagValueOffset);
                        directory.SetObject(tagType, i);
                    }
                    else
                    {
                        Debug.Write("Found a tag made of longs but will use ints");
                        int[] ints = new int[componentCount];
                        for (int i = 0; i < componentCount; i++)
                        {
                            ints[i] = Get32Bits(tagValueOffset + (i * 4));
                        }
                        directory.SetIntArray(tagType, ints);
                    }
                    break;
                default:
                    Trace.TraceWarning("Unknown format code " + formatCode + " for tag " + tagType);
                    break;
            }
        }

        /// <summary>
        /// Creates a string from the _data buffer starting at the specified offSet, 
        /// and ending where byte=='\0' or where Length==maxLength.
        /// </summary>
        /// <param name="offSet">the lcOffset</param>
        /// <param name="maxLength">the max length</param>
        /// <returns>a string representing what was read</returns>
        private string ReadString(int offSet, int maxLength)
        {
            int Length = 0;
            while ((offSet + Length) < base.data.Length
                && base.data[offSet + Length] != '\0'
                && Length < maxLength)
            {
                Length++;
            }
            return Utils.Decode(base.data, offSet, Length, false);
        }

        /// <summary>
        /// A special case of ReadString that handle Exif UserComment reading.
        /// This method is necessary as certain camere models prefix the comment string 
        /// with "ASCII\0", which is all that would be returned by ReadString(...).
        /// </summary>
        /// <param name="tagValueOffSet">the tag value lcOffset</param>
        /// <param name="componentCount">the component count</param>
        /// <param name="formatCode">the format code</param>
        /// <returns>a string</returns>
        private string ReadCommentString(
            int tagValueOffSet,
            int componentCount,
            int formatCode)
        {
            // Olympus has this padded with trailing spaces.  Remove these first.
            // ArrayIndexOutOfBoundsException bug fixed by Hendrik W�rdehoff - 20 Sep 2002
            int byteCount = componentCount * BYTES_PER_FORMAT[formatCode];
            for (int i = byteCount - 1; i >= 0; i--)
            {
                if (base.data[tagValueOffSet + i] == ' ')
                {
                    base.data[tagValueOffSet + i] = (byte)'\0';
                }
                else
                {
                    break;
                }
            }
            // Copy the comment
            if ("ASCII".Equals(Utils.Decode(base.data, tagValueOffSet, 5, false)))
            {
                for (int i = 5; i < 10; i++)
                {
                    byte b = base.data[tagValueOffSet + i];
                    if (b != '\0' && b != ' ')
                    {
                        return ReadString(tagValueOffSet + i, 1999);
                    }
                }
            }
            else if ("UNICODE".Equals(Utils.Decode(base.data, tagValueOffSet, 7, false)))
            {
                int start = tagValueOffSet + 7;
                for (int i = start; i < 10 + start; i++)
                {
                    byte b = base.data[i];
                    if (b == 0 || (char)b == ' ')
                    {
                        continue;
                    }
                    else
                    {
                        start = i;
                        break;
                    }

                }
                int end = base.data.Length;
                // TODO find a way to cut the string properly				
                return Utils.Decode(base.data, start, end - start, true);

            }

            // TODO implement support for UNICODE and JIS UserComment encodings..?
            return ReadString(tagValueOffSet, 1999);
        }

        /// <summary>
        /// Determine the offSet at which a given InteropArray entry begins within the specified IFD.
        /// </summary>
        /// <param name="ifdStartOffSet">the offSet at which the IFD starts</param>
        /// <param name="entryNumber">the zero-based entry number</param>
        /// <returns>the directory entry lcOffset</returns>
        private int CalculateDirectoryEntryOffSet(
            int ifdStartOffSet,
            int entryNumber)
        {
            return (ifdStartOffSet + 2 + (12 * entryNumber));
        }


        /// <summary>
        /// Gets a 16 bit aValue from aFile'str native byte order.  Between 0x0000 and 0xFFFF.
        /// </summary>
        /// <param name="offSet">the lcOffset</param>
        /// <returns>a 16 bit int</returns>
        protected override int Get16Bits(int offSet)
        {
            if (offSet < 0 || offSet >= base.data.Length)
            {
                throw new IndexOutOfRangeException(
                    "attempt to read data outside of exif segment (index "
                    + offSet
                    + " where max index is "
                    + (base.data.Length - 1)
                    + ")");
            }
            if (this.isMotorollaByteOrder)
            {
                // Motorola big first
                return (base.data[offSet] << 8 & 0xFF00) | (base.data[offSet + 1] & 0xFF);
            }
            else
            {
                // Intel ordering
                return (base.data[offSet + 1] << 8 & 0xFF00) | (base.data[offSet] & 0xFF);
            }
        }

        /// <summary>
        /// Gets a 32 bit aValue from aFile'str native byte order.
        /// </summary>
        /// <param name="offSet">the lcOffset</param>
        /// <returns>a 32b int</returns>
        protected override int Get32Bits(int offSet)
        {
            if (offSet < 0 || offSet >= base.data.Length)
            {
                throw new IndexOutOfRangeException(
                    "attempt to read data outside of exif segment (index "
                    + offSet
                    + " where max index is "
                    + (base.data.Length - 1)
                    + ")");
            }

            if (this.isMotorollaByteOrder)
            {
                // Motorola big first
                return (int)(((uint)(base.data[offSet] << 24 & 0xFF000000))
                    | ((uint)(base.data[offSet + 1] << 16 & 0xFF0000))
                    | ((uint)(base.data[offSet + 2] << 8 & 0xFF00))
                    | ((uint)(base.data[offSet + 3] & 0xFF)));
            }
            else
            {
                // Intel ordering
                return (int)(((uint)(base.data[offSet + 3] << 24 & 0xFF000000))
                    | ((uint)(base.data[offSet + 2] << 16 & 0xFF0000))
                    | ((uint)(base.data[offSet + 1] << 8 & 0xFF00))
                    | ((uint)(base.data[offSet] & 0xFF)));
            }
        }
    }
}
