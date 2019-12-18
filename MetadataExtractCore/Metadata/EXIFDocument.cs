using MetadataExtractCore.Diagrams;
using MetadataExtractCore.Utilities;
using MetadataExtractor.Formats.Jpeg;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace MetadataExtractCore.Extractors
{
    public class EXIFDocument : DocumentExtractor
    {
        private static readonly string[] IgnoredExifDirectories = new[] { "JPEG", "JFIF", "XMP", "File Type", "Interoperability", "ICC Profile", "IPTC", "PrintIM", "Huffman" };

        public EXIFDocument(Stream stm) : base(stm)
        {
        }

        public override FileMetadata AnalyzeFile()
        {
            try
            {
                this.foundMetadata = new FileMetadata();
                IReadOnlyList<MetadataExtractor.Directory> directories = MetadataExtractor.ImageMetadataReader.ReadMetadata(this.fileStream);

                foreach (MetadataExtractor.Directory currentDir in directories.Where(p => !IgnoredExifDirectories.Contains(p.Name)))
                {
                    if (currentDir is MetadataExtractor.Formats.Exif.ExifThumbnailDirectory)
                    {
                        try
                        {
                            uint offset = (uint)currentDir.GetObject(MetadataExtractor.Formats.Exif.ExifThumbnailDirectory.TagThumbnailOffset) + (uint)MetadataExtractor.Formats.Exif.ExifReader.JpegSegmentPreamble.Length;
                            uint length = (uint)currentDir.GetObject(MetadataExtractor.Formats.Exif.ExifThumbnailDirectory.TagThumbnailLength);

                            long currentPosition = this.fileStream.Position;
                            this.fileStream.Seek(0, SeekOrigin.Begin);
                            JpegSegment app1Segment = JpegSegmentReader.ReadSegments(new MetadataExtractor.IO.SequentialStreamReader(this.fileStream), new[] { JpegSegmentType.App1 }).FirstOrDefault();
                            if (app1Segment != null)
                            {
                                byte[] thumb = new byte[length];
                                Array.Copy(app1Segment.Bytes, offset, thumb, 0, length);
                                this.foundMetadata.Thumbnail = thumb;
                            }
                            this.fileStream.Seek(currentPosition, SeekOrigin.Begin);
                        }
                        catch (Exception)
                        {
                        }
                    }
                    else if (currentDir is MetadataExtractor.Formats.Exif.GpsDirectory gps)
                    {
                        if (this.foundMetadata.GPS == null)
                        {
                            MetadataExtractor.GeoLocation gpsLocation = gps.GetGeoLocation();
                            if (gpsLocation != null)
                            {
                                this.foundMetadata.GPS = new GeoLocation(gpsLocation.ToDmsString(), gpsLocation.Longitude, gpsLocation.Latitude);
                                if (gps.ContainsTag(MetadataExtractor.Formats.Exif.GpsDirectory.TagAltitude))
                                {
                                    this.foundMetadata.GPS.Altitude = $"{gps.GetDescription(MetadataExtractor.Formats.Exif.GpsDirectory.TagAltitude)} ({gps.GetDescription(MetadataExtractor.Formats.Exif.GpsDirectory.TagAltitudeRef)})"; ;
                                }
                            }
                        }
                    }
                    else
                    {
                        Dictionary<string, string> dicTags = new Dictionary<string, string>();
                        foreach (MetadataExtractor.Tag tag in currentDir.Tags)
                        {
                            string lcDescription = tag.Description?.Trim();
                            string lcName = tag.Name?.Trim();

                            if (!String.IsNullOrWhiteSpace(lcName) && !String.IsNullOrWhiteSpace(lcDescription) &&
                                !lcName.StartsWith("unknown", StringComparison.OrdinalIgnoreCase) && !lcDescription.ToLower().StartsWith("unknown", StringComparison.OrdinalIgnoreCase))
                            {
                                lcName = Functions.RemoveAccentsWithNormalization(lcName);
                                lcDescription = Functions.RemoveAccentsWithNormalization(lcDescription);

                                switch (tag.Type)
                                {
                                    case MetadataExtractor.Formats.Exif.ExifDirectoryBase.TagWinAuthor:
                                    case MetadataExtractor.Formats.Exif.ExifDirectoryBase.TagArtist:
                                    case MetadataExtractor.Formats.Exif.ExifDirectoryBase.TagCameraOwnerName:
                                        this.foundMetadata.Add(new User(lcDescription, false, "EXIF"));
                                        break;
                                    case MetadataExtractor.Formats.Exif.ExifDirectoryBase.TagSoftware:
                                        string strSoftware = Analysis.ApplicationAnalysis.GetApplicationsFromString(lcDescription);
                                        if (!String.IsNullOrEmpty(strSoftware))
                                        {
                                            this.foundMetadata.Add(new Application(strSoftware));
                                        }
                                        break;
                                    case MetadataExtractor.Formats.Exif.ExifDirectoryBase.TagDateTime:
                                    case MetadataExtractor.Formats.Exif.ExifDirectoryBase.TagDateTimeDigitized:
                                    case MetadataExtractor.Formats.Exif.ExifDirectoryBase.TagDateTimeOriginal:
                                        if (DateTime.TryParseExact(lcDescription, "yyyy:MM:dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date) &&
                                            date > DateTime.MinValue && (!foundMetadata.Dates.CreationDate.HasValue || this.foundMetadata.Dates.CreationDate > date))
                                        {
                                            this.foundMetadata.Dates.CreationDate = date;
                                        }
                                        break;
                                    case MetadataExtractor.Formats.Exif.ExifDirectoryBase.TagModel:
                                        this.foundMetadata.Model = lcDescription;
                                        break;
                                    case MetadataExtractor.Formats.Exif.ExifDirectoryBase.TagHostComputer:
                                        this.foundMetadata.OperatingSystem = lcDescription;
                                        break;
                                    default:
                                        break;
                                }

                                if (!dicTags.ContainsKey(lcName))
                                {
                                    dicTags.Add(lcName, lcDescription);
                                }
                            }
                        }

                        if (dicTags.Count > 0)
                        {
                            string makerKey = currentDir.Name;
                            int i = 1;
                            while (foundMetadata.Makernotes.ContainsKey(makerKey))
                            {
                                makerKey = $"{currentDir.Name} ({i})";
                                i++;
                            }

                            foundMetadata.Makernotes.Add(makerKey, dicTags);
                        }
                    }

                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Error analizing EXIF metadata ({e.ToString()})");
            }

            return this.foundMetadata;
        }
    }
}
