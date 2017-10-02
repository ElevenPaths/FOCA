using System;
using System.Collections.Generic;
using System.Linq;
using com.drew.metadata;
using com.drew.imaging.tiff;
using com.drew.imaging.jpg;
using System.IO;
using MetadataExtractCore.Utilities;
using MetadataExtractCore.Diagrams;

namespace MetadataExtractCore.Metadata
{
    [Serializable]
    public class EXIFDocument : MetaExtractor
    {
        private string strExt;

        public SerializableDictionary<string, SerializableDictionary<string, string>> dicAnotherMetadata { get; set;}

        public byte[] Thumbnail {get; set;}

        public EXIFDocument() { }

        public EXIFDocument(Stream stm, string strExt)
        {
            this.stm = new MemoryStream();
            Functions.CopyStream(stm, this.stm);
            this.strExt = strExt;
            dicAnotherMetadata = new SerializableDictionary<string, SerializableDictionary<string, string>>();
        }

        public override void analyzeFile()
        {
            com.drew.metadata.Metadata m;
            try
            {
                if (strExt.ToLower() == ".raw" ||
                    strExt.ToLower() == ".cr2" ||
                    strExt.ToLower() == ".crw")
                {
                    m = TiffMetadataReader.ReadMetadata(stm);
                }
                else
                {
                    m = JpegMetadataReader.ReadMetadata(stm);
                }
                IEnumerator<AbstractDirectory> lcDirectoryEnum = m.GetDirectoryIterator();
                while (lcDirectoryEnum.MoveNext())
                {
                    if (lcDirectoryEnum.Current.GetName() != "Jpeg Makernote")
                    {
                        SerializableDictionary<string, string> dicTags = new SerializableDictionary<string, string>();
                        AbstractDirectory lcDirectory = lcDirectoryEnum.Current;
                        IEnumerator<Tag> lcTagsEnum = lcDirectory.GetTagIterator();
                        while (lcTagsEnum.MoveNext())
                        {
                            Tag tag = lcTagsEnum.Current;
                            if (tag.GetTagName() == "Thumbnail Data")
                            {
                                Thumbnail = (byte[])tag.GetTagValue();
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
                                    FoundUsers.AddUniqueItem(lcDescription, false, "Copyright/Owner name");
                                }
                            }
                            if (lcName.ToLower() == "software")
                            {
                                string strSoftware = Analysis.ApplicationAnalysis.GetApplicationsFromString(lcDescription.Trim());
                                if (!FoundMetaData.Applications.Items.Any(A => A.Name == strSoftware))
                                    FoundMetaData.Applications.Items.Add(new ApplicationsItem(strSoftware));
                            }
                            if (lcName.ToLower() == "model")
                                FoundMetaData.Model = lcDescription.Trim();
                            dicTags.Add(lcName, lcDescription);
                        }
                        dicAnotherMetadata.Add(lcDirectory.GetName(), dicTags);
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Error analizing EXIF metadata ({0})", e.ToString());
            }
            finally
            {
                this.stm.Close();
            }
        }
    }
}
