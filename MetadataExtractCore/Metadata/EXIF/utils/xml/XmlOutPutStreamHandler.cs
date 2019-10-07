using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using com.drew.metadata;

namespace com.utils.xml
{
    /// <summary>
    /// This class will handle XML for a Directory class.
    ///
    /// For default XML stream you can have a look to MetadataExtractor.dtd file and sampleFile.xml.
    /// </summary>

    public class XmlOutPutStreamHandler : IOutPutTextStreamHandler
    {
        private static Dictionary<string, string> FORBIDEN_CHAR = XmlOutPutStreamHandler.BuildForbidenChar();

        /// <summary>
        /// The cached DTD.
        /// </summary>
        private static string LOADED_DTD = null;

        /// <summary>
        /// The DTD path and file name.
        /// </summary>
        private string dtdFileName;
        public string DtdFileName
        {
            get
            {
                return this.dtdFileName;
            }
            set
            {
                this.dtdFileName = value;
                LoadDtd();
            }
        }


        private Metadata metadata;
        public Metadata Metadata
        {
            get
            {
                return this.metadata;
            }
            set
            {
                this.metadata = value;
            }
        }

        /// <summary>
        /// Get/set the unknown option
        /// </summary>
        private bool doUnknown;
        public bool DoUnknown
        {
            get
            {
                return this.doUnknown;
            }
            set
            {
                this.doUnknown = value;
            }
        }


        /// <summary>
        /// Constructor of the object.
        /// </summary>
        public XmlOutPutStreamHandler()
            : this(null)
        {
        }

        /// <summary>
        /// Constructor of the object.
        /// </summary>
        /// <param name="aMetadata">the metadata that shoud be transformed into XML</param>
        public XmlOutPutStreamHandler(Metadata aMetadata)
            : base()
        {
            this.Metadata = aMetadata;
            this.DtdFileName = "MetadataExtractor.dtd";
        }

        /// <summary>
        /// Gives all forbiden letter in XML standard and their correspondance.
        /// </summary>
        /// <returns>All forbiden chars and their XML correspondance</returns>
        private void LoadDtd()
        {
            try
            {
                StringBuilder lcBuff = new StringBuilder();
                using (var lcStreamReader = new StreamReader(File.Open(this.DtdFileName, FileMode.Open, FileAccess.Read)))
                {
                    while (!lcStreamReader.EndOfStream)
                    {
                        lcBuff.Append(lcStreamReader.ReadLine());
                        lcBuff.AppendLine();
                    }
                }
                LOADED_DTD = lcBuff.ToString();
            }
            catch (Exception)
            {
                // Oups Dtd not found
                LOADED_DTD = null;
            }
        }


        /// <summary>
        /// Gives all forbiden letter in XML standard and their correspondance.
        /// </summary>
        /// <returns>All forbiden chars and their XML correspondance</returns>
        protected static Dictionary<string, string> BuildForbidenChar()
        {
            Dictionary<string, string> lcResu = new Dictionary<string, string>(5);
            lcResu.Add("<", "&lt;");
            lcResu.Add(">", "&gt;");
            lcResu.Add("&", "&amp;");
            lcResu.Add("\'", "&apos;");
            lcResu.Add("\"", "&quot;");
            return lcResu;
        }

        /// <summary>
        /// Start out put stream
        /// </summary>
        /// <param name="aBuff">where to put informations</param>
        /// <param name="someParam">Specify encoding here in 0, in 1 you can add a XSLT ref, in 2 the number of files, in 3 the dtd path</param>
        public void StartTextStream(StringBuilder aBuff, string[] someParam)
        {
            aBuff.Append("<?xml version=\"1.0\" encoding=\"").Append(someParam[0]).Append("\" ?>").AppendLine();
            if (someParam.Length > 3)
            {
                // We've got a DTD
                this.DtdFileName = someParam[3];
            }

            // If we have a DTD
            if (LOADED_DTD != null)
            {
                aBuff.Append("<!DOCTYPE metadataExtractor [");
                aBuff.AppendLine();
                aBuff.Append(LOADED_DTD);
                aBuff.Append("]>");
                aBuff.AppendLine();
            }

            if (someParam.Length > 1 && someParam[1] != null)
            {
                aBuff.Append("<?xml-stylesheet type=\"text/xsl\" href=\"").Append(someParam[1]).Append("\"?>");
            }
            if (someParam.Length > 2)
            {
                int lcNbFile = 0;
                try
                {
                    lcNbFile = Convert.ToInt16(someParam[2]);
                    // Finally will open tag
                }
                catch (FormatException e)
                {
                    // An error occured
                    aBuff.Append("<!-- ").Append(e.Message).Append(" -->");
                    lcNbFile = -1;
                }
                finally
                {
                    // Then we deal with more than one file
                    Open(aBuff, "metadataExtractor", "nbFile", lcNbFile.ToString(), true);
                }
            }
        }

        /// <summary>
        /// Finish out put stream
        /// </summary>
        /// <param name="aBuff">where to put informations</param>
        /// <param name="someParam">Should contain nb file in [2]</param>
        public void EndTextStream(StringBuilder aBuff, string[] someParam)
        {
            if (someParam.Length >= 2)
            {
                int lcNbFile = 0;
                try
                {
                    lcNbFile = Convert.ToInt16(someParam[2]);
                    // Finally will close files tag
                }
                catch (FormatException e)
                {
                    // An error occured
                    aBuff.Append("<!-- ").Append(e.Message).Append(" -->");
                }
                finally
                {
                    Close(aBuff, "metadataExtractor", true);
                }
            }
        }


        /// <summary>
        /// Normalize a value into XML
        /// </summary>
        /// <param name="aBuff">where to put new XML value</param>
        /// <param name="aValue">the value to normalize</param>
        /// <param name="useCdata">if true will use CDATA, if false will replace FORBIDEN chars by their normal value</param>
        public virtual void Normalize(StringBuilder aBuff, string aValue, bool useCdata)
        {
            if (aValue != null)
            {
                aValue = aValue.Trim();
                if (useCdata)
                {
                    aBuff.Append("<![CDATA[").Append(aValue).Append("]]>");
                }
                else
                {
                    // check if value contains strange char and replace them if needed
                    IEnumerator lcEnumChar = FORBIDEN_CHAR.GetEnumerator();
                    while (lcEnumChar.MoveNext())
                    {
                        KeyValuePair<string, string> lcPair = (KeyValuePair<string, string>)lcEnumChar.Current;
                        aValue = aValue.Replace(lcPair.Key, lcPair.Value);
                    }
                    aBuff.Append(aValue);
                }
            }
        }

        /// <summary>
        /// Opens an XML tag.
        /// </summary>
        /// <param name="aBuff">where to open tag</param>
        /// <param name="aTag">what to put inside the tag</param>
        /// <param name="isNewLine">if true will go to new line after open</param>
        private void Close(StringBuilder aBuff, string aTag, bool isNewLine)
        {
            aBuff.Append("</").Append(aTag).Append('>');
            if (isNewLine)
            {
                aBuff.AppendLine();
            }
        }

        /// <summary>
        /// Closes an XML tag.
        /// </summary>
        /// <param name="aBuff">where to close tag</param>
        /// <param name="aTag">what to put inside the tag</param>
        /// <param name="isNewLine">if true will go to new line after close</param>
        private void Open(StringBuilder aBuff, string aTag, bool isNewLine)
        {
            aBuff.Append('<').Append(aTag).Append('>');
            if (isNewLine)
            {
                aBuff.AppendLine();
            }
        }

        /// <summary>
        /// Opens an XML tag.
        /// </summary>
        /// <param name="aBuff">where to open tag</param>
        /// <param name="aTagName">what to put inside the tag</param>
        /// <param name="attName1">name of an attribute for this tag (can be null)</param>
        /// <param name="attValue1">value of the first attribute for this tag</param>
        /// <param name="isNewLine">if true will go to new line after open</param>
        private void Open(StringBuilder aBuff, string aTagName, string attName1, object attValue1, bool isNewLine)
        {
            aBuff.Append('<').Append(aTagName);
            if (attName1 != null)
            {
                aBuff.Append(' ').Append(attName1).Append("=\"");
                aBuff.Append(attValue1).Append('\"');
            }
            aBuff.Append('>');
            if (isNewLine)
            {
                aBuff.AppendLine();
            }
        }

        /// <summary>
        /// Creates an XML tag using the Tag object info.
        /// Examples :
        /// <pre>
        /// &lt;tag type="0x0044"&gt;
        ///   &lt;tagLabel&gt;White Balance&lt;/tagLabel&gt;
        ///   &lt;tagDescription>&lt;![CDATA[Very bright]]&gt;&lt;/tagDescription&gt;
        /// &lt;/tag&gt;
        /// <br/>
        /// &lt;tag type="0x0044"&gt;
        ///   &lt;tagLabel&gt;White Balance&lt;/tagLabel&gt;
        ///   &lt;tagDescription/&gt;
        /// &lt;/tag&gt;
        /// <br/>
        /// &lt;tag type="0x0044"&gt;
        ///   &lt;tagLabel&gt;White Balance&lt;/tagLabel&gt;
        ///   &lt;tagDescription/&gt;
        ///   &lt;tagError&gt;&lt;![CDATA[Oups something is wrong]]&gt;&lt;/tagError&gt;
        /// &lt;/tag&gt;
        /// </pre>
        /// </summary>
        /// <param name="aBuff">where to put tag</param>
        /// <param name="aTag">the tag</param>
        protected virtual void CreateTag(StringBuilder aBuff, Tag aTag)
        {
            if (aTag != null)
            {
                string lcDescription = null;
                string lcError = null;
                try
                {
                    lcDescription = aTag.GetDescription();
                }
                catch (MetadataException e)
                {
                    lcError = e.Message;
                }
                string lcName = aTag.GetTagName();
                string lcHexName = aTag.GetTagTypeHex();

                if (!this.DoUnknown && lcDescription != null && (lcName.ToLower().StartsWith("unknown") || lcDescription.ToLower().StartsWith("unknown")))
                {
                    // No unKnown and is unKnown so do nothing
                    return;
                }
                Open(aBuff, "tag", "type", lcHexName, true);

                Open(aBuff, "tagLabel", false);
                Normalize(aBuff, lcName, false);
                Close(aBuff, "tagLabel", false);

                if (!string.IsNullOrEmpty(lcDescription))
                {
                    Open(aBuff, "tagDescription", false);
                    Normalize(aBuff, lcDescription, false);
                    Close(aBuff, "tagDescription", false);
                }
                else
                {
                    aBuff.Append("<tagDescription/>").AppendLine();
                }

                if (lcError != null)
                {
                    Open(aBuff, "tagError", false);
                    Normalize(aBuff, lcError, false);
                    Close(aBuff, "tagError", false);
                }
                else
                {
                    // Does nothing if no error, this will limit the size of the XML
                    // stream since 99% of tag will be fine
                    // aBuff.Append("<tagError/>");
                }
                Close(aBuff, "tag", true);
            }
        }

        /// <summary>
        /// Creates a directory tag.
        ///
        /// Examples :
        /// <pre>
        /// &lt;directory name="Exif"&gt;
        ///   &lt;tag&gt;
        ///     ...
        ///   &lt;/tag&gt;
        ///   &lt;tag&gt;
        ///     ...
        ///   &lt;/tag&gt;
        /// &lt;/directory&gt;
        /// </pre>
        /// </summary>
        /// <param name="aBuff">where to put info</param>
        /// <param name="aDirectory">the information to add</param>
        protected virtual void CreateDirectory(StringBuilder aBuff, AbstractDirectory aDirectory)
        {
            if (aDirectory != null)
            {
                Open(aBuff, "directory name=\"" + aDirectory.GetName() + "\"", true);
                IEnumerator<Tag> lcTagsEnum = aDirectory.GetTagIterator();
                while (lcTagsEnum.MoveNext())
                {
                    Tag lcTag = lcTagsEnum.Current;
                    CreateTag(aBuff, lcTag);
                    lcTag = null;
                }
                Close(aBuff, "directory", true);
            }
        }


        /// <summary>
        /// Transform the metatdat object into an XML stream.
        /// </summary>
        /// <returns>The metadata object as XML stream</returns>
        public virtual string AsText()
        {
            StringBuilder lcBuff = new StringBuilder();
            IEnumerator<AbstractDirectory> lcDirectoryEnum = this.Metadata.GetDirectoryIterator();
            while (lcDirectoryEnum.MoveNext())
            {
                AbstractDirectory lcDirectory = lcDirectoryEnum.Current;
                CreateDirectory(lcBuff, lcDirectory);
                lcDirectory = null;
            }
            return lcBuff.ToString();
        }
    }
}
