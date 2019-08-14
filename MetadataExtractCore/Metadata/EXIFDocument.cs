using MetadataExtractCore.Diagrams;
using MetadataExtractCore.Utilities;
using System;
using System.Collections.Generic;
using System.IO;

namespace MetadataExtractCore.Extractors
{
    public class EXIFDocument : DocumentExtractor
    {
        private string strExt;

        public EXIFDocument(Stream stm, string strExt) : base(stm)
        {
            this.strExt = strExt;
        }

        public override FileMetadata AnalyzeFile()
        {
            com.drew.metadata.Metadata m;
            try
            {
                this.foundMetadata = new FileMetadata();
                if (strExt == ".raw" ||
                    strExt == ".cr2" ||
                    strExt == ".crw")
                {
                    m = com.drew.imaging.tiff.TiffMetadataReader.ReadMetadata(this.fileStream);
                }
                else
                {
                    m = com.drew.imaging.jpg.JpegMetadataReader.ReadMetadata(this.fileStream);
                }
                IEnumerator<com.drew.metadata.AbstractDirectory> lcDirectoryEnum = m.GetDirectoryIterator();
                while (lcDirectoryEnum.MoveNext())
                {
                    if (lcDirectoryEnum.Current.GetName() != "Jpeg Makernote")
                    {
                        Dictionary<string, string> dicTags = new Dictionary<string, string>();
                        com.drew.metadata.AbstractDirectory lcDirectory = lcDirectoryEnum.Current;
                        IEnumerator<com.drew.metadata.Tag> lcTagsEnum = lcDirectory.GetTagIterator();
                        while (lcTagsEnum.MoveNext())
                        {
                            com.drew.metadata.Tag tag = lcTagsEnum.Current;
                            if (tag.GetTagName() == "Thumbnail Data")
                            {
                                foundMetadata.Thumbnail = (byte[])tag.GetTagValue();
                            }
                            string lcDescription = "";
                            try
                            {
                                lcDescription = tag.GetDescription();
                            }
                            catch { };
                            string lcName = tag.GetTagName();
                            if (lcName.ToLower().StartsWith("unknown") || lcDescription.ToLower().StartsWith("unknown"))
                            {
                                continue;
                            }
                            lcName = Functions.RemoveAccentsWithNormalization(lcName);
                            lcDescription = Functions.RemoveAccentsWithNormalization(lcDescription);
                            if (lcName.ToLower() == "owner name" || lcName.ToLower() == "copyright")
                            {
                                if (!string.IsNullOrEmpty(lcDescription) && lcDescription.Trim() != string.Empty &&
                                    !lcDescription.ToLower().Contains("digital") && !lcDescription.ToLower().Contains("camera") && !lcDescription.ToLower().Contains("(c)") &&
                                    !lcDescription.ToLower().Contains("copyright"))
                                {
                                    this.foundMetadata.Add(new User(lcDescription, false, "Copyright/Owner name"));
                                }
                            }
                            if (lcName.ToLower() == "software")
                            {
                                string strSoftware = Analysis.ApplicationAnalysis.GetApplicationsFromString(lcDescription.Trim());
                                this.foundMetadata.Add(new Application(strSoftware));
                            }
                            if (lcName.ToLower() == "model")
                                this.foundMetadata.Model = lcDescription.Trim();
                            if (!dicTags.ContainsKey(lcName))
                            {
                                dicTags.Add(lcName, lcDescription);
                            }
                        }
                        foundMetadata.Makernotes.Add(lcDirectory.GetName(), dicTags);
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
