using System.Collections.Generic;
using System.Text;
using com.drew.metadata;
using System.Globalization;

namespace com.utils.xml
{
    /// <summary>
    /// This class will handle text for a metatdata class
    /// </summary>

    public class TxtOutPutStreamHandler : IOutPutTextStreamHandler
    {
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
        public TxtOutPutStreamHandler()
            : this(null)
        {
        }

        /// <summary>
        /// Constructor of the object.
        /// </summary>
        /// <param name="aMetadata">the metadata that shoud be transformed into txt</param>
        public TxtOutPutStreamHandler(Metadata aMetadata)
            : base()
        {
            this.Metadata = aMetadata;
        }

        /// <summary>
        /// Gives all forbiden letter in txt standard and their correspondance.
        /// </summary>
        /// <returns>All forbiden chars and their txt correspondance</returns>
        public string RemoveAccentsWithNormalization(string inputString)
        {
            string normalizedString = inputString.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < normalizedString.Length; i++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(normalizedString[i]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(normalizedString[i]);
                }
            }
            return (sb.ToString().Normalize(NormalizationForm.FormC));
        }

        /// <summary>
        /// Creates an TXT tag using the Tag object info.
        /// </summary>
        /// <param name="aBuff">where to put tag</param>
        /// <param name="aTag">the tag</param>
        protected virtual void CreateTag(StringBuilder aBuff, Tag aTag)
        {
            if (aTag != null)
            {
                string lcDescription = null;
                try
                {
                    lcDescription = aTag.GetDescription();
                }
                catch (MetadataException)
                {
                    // Does not care here
                }
                string lcName = aTag.GetTagName();
                if (!this.DoUnknown && (lcName.ToLower().StartsWith("unknown") || lcDescription.ToLower().StartsWith("unknown")))
                {
                    // No unKnown and is unKnown so do nothing
                    return;
                }
                aBuff.Append(RemoveAccentsWithNormalization(lcName)); 
                aBuff.Append('=');
                aBuff.Append(RemoveAccentsWithNormalization(lcDescription));
                aBuff.AppendLine();
            }
        }

        /// <summary>
        /// Creates a directory tag.
        /// </summary>
        /// <param name="aBuff">where to put info</param>
        /// <param name="aDirectory">the information to add</param>
        protected virtual void CreateDirectory(StringBuilder aBuff, AbstractDirectory aDirectory)
        {
            if (aDirectory != null)
            {
                aBuff.Append("--| ").Append(aDirectory.GetName()).Append(" |--");
                aBuff.AppendLine();
                IEnumerator<Tag> lcTagsEnum = aDirectory.GetTagIterator();
                while (lcTagsEnum.MoveNext())
                {
                    Tag lcTag = lcTagsEnum.Current;
                    CreateTag(aBuff, lcTag);
                    lcTag = null;
                }
            }
        }

        /// <summary>
        /// Transform the metatdat object into an TXT stream.
        /// </summary>
        /// <returns>The metadata object as TXT stream</returns>
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
