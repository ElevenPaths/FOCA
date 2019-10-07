using System;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;

using com.drew.lang;
using com.utils.bundle;

namespace com.drew.metadata
{
    /// <summary>
    /// Base class for all Metadata directory types with supporting
    /// methods for setting and getting tag values.
    /// </summary>
    [Serializable]

    public abstract class AbstractDirectory
    {
        /// <summary>
        /// List of date format that will be used if <i>standard</i> format does not work.
        /// </summary>
        private readonly static string[] DATE_FORMATS = new string[] { "dd/MM/yyyy HH:mm:ss", "yyyy:MM:dd HH:mm:ss", "yyyy-MM-dd_HH-mm-ss", "yyyy/MM/dd HH:mm:ss", "dd/MM/yyyy", "yyyy/MM/dd", "yyyy-MM-dd" };

        /// <summary>
        /// Map of values hashed by type identifiers.
        /// </summary>
        private IDictionary<int, object> tagMap;

        /// <summary>
        /// The descriptor used to interpret tag values.
        /// </summary>
        private AbstractTagDescriptor descriptor;

        /// <summary>
        /// A convenient list holding tag values in the order in which they were stored. <br/>
        /// This is used for creation of an iterator, and for counting the number of defined tags.
        /// </summary>
        private IList<Tag> definedTagList;

        /// <summary>
        /// The bundle name used by this directory.
        /// </summary>
        private string bundleName;
        protected string BundleName
        {
            get
            {
                return this.bundleName;
            }
            set
            {
                this.bundleName = value;
            }
        }

        /// <summary>
        /// Indicates if there is error in this directory
        /// </summary>
        private bool hasError;
        public bool HasError
        {
            get
            {
                return this.hasError;
            }
            set
            {
                this.hasError = value;
            }
        }

        /// <summary>
        /// Provides the map of tag names, hashed by tag type identifier. <br/>
        /// Will contain all tag value and tag name for all descriptor.
        /// </summary>
        private static IDictionary<Type, IDictionary<int, string>> tagNameMap;



        /// <summary>
        /// Creates a new Directory.
        /// </summary>
        private AbstractDirectory() : base()
        {
            this.tagMap = new Dictionary<int, object>();
            this.definedTagList = new List<Tag>();
            this.HasError = false;
            if (AbstractDirectory.tagNameMap == null)
            {
                AbstractDirectory.tagNameMap = new Dictionary<Type, IDictionary<int, string>>(25);
            }
        }

        /// <summary>
        /// Creates a new Directory.
        /// </summary>
        /// <param name="aBundleName">bundle name for this directory</param>
        protected AbstractDirectory(string aBundleName)
            : this()
        {
            this.BundleName = aBundleName;
            // Load the bundle
            IResourceBundle bundle = ResourceBundleFactory.CreateDefaultBundle(aBundleName);
            AbstractDirectory.tagNameMap[this.GetType()] = AbstractDirectory.FillTagMap(this.GetType(), bundle);
        }

        /// <summary>
        /// Indicates whether the specified tag type has been set.
        /// </summary>
        /// <param name="aTagType">the tag type to check for</param>
        /// <returns>true if a value exists for the specified tag type, false if not</returns>
        public bool ContainsTag(int aTagType)
        {
            return this.tagMap.ContainsKey(aTagType);
        }

        /// <summary>
        /// Returns an Iterator of Tag instances that have been set in this Directory.
        /// </summary>
        /// <returns>an Iterator of Tag instances</returns>
        public IEnumerator<Tag> GetTagIterator()
        {
            return this.definedTagList.GetEnumerator();
        }

        /// <summary>
        /// Returns the number of tags set in this Directory.
        /// </summary>
        /// <returns>the number of tags set in this Directory</returns>
        public int GetTagCount()
        {
            return this.definedTagList.Count;
        }

        /// <summary>
        /// Sets the descriptor used to interperet tag values.
        /// </summary>
        /// <param name="aDescriptor">the descriptor used to interperet tag values</param>
        /// <exception cref="NullReferenceException">if aDescriptor is null</exception>
        public void SetDescriptor(AbstractTagDescriptor aDescriptor)
        {
            if (aDescriptor == null)
            {
                throw new NullReferenceException("Cannot set a null descriptor");
            }
            this.descriptor = aDescriptor;
        }

        /// <summary>
        /// Sets an int array for the specified tag.
        /// </summary>
        /// <param name="aTagType">the tag identifier</param>
        /// <param name="someInts">the int array to store</param>
        public virtual void SetIntArray(int aTagType, int[] someInts)
        {
            this.SetObject(aTagType, someInts);
        }

        /// <summary>
        /// Helper method, containing common functionality for all 'add' methods.
        /// </summary>
        /// <param name="aTagType">the tag value as an int</param>
        /// <param name="aValue">the value for the specified tag</param>
        /// <exception cref="NullReferenceException">if aValue is null</exception>
        public void SetObject(int aTagType, object aValue)
        {
            if (aValue == null)
            {
                throw new NullReferenceException("Cannot set a null object");
            }

            if (!this.tagMap.ContainsKey(aTagType))
            {
                this.tagMap.Add(aTagType, aValue);
                this.definedTagList.Add(new Tag(aTagType, this));
            }
            else
            {
                // We remove it and re-add it with the new value
                this.tagMap.Remove(aTagType);
                this.tagMap.Add(aTagType, aValue);
            }
        }

        /// <summary>
        /// Returns the specified tag value as an int, if possible.
        /// </summary>
        /// <param name="aTagType">the specified tag type</param>
        /// <returns>the specified tag value as an int, if possible.</returns>
        /// <exception cref="MetadataException">if tag not found</exception>
        public int GetInt(int aTagType)
        {
            object lcObj = this.GetObject(aTagType);
            if (lcObj == null)
            {
                throw new MetadataException(
                    "Tag "
                    + GetTagName(aTagType)
                    + " has not been set -- check using containsTag() first");
            }
            else if (lcObj is string str)
            {
                try
                {
                    return Convert.ToInt32(str);
                }
                catch (FormatException)
                {
                    string lcStr = str;
                    int lcVal = 0;
                    for (int i = lcStr.Length - 1; i >= 0; i--)
                    {
                        lcVal += lcStr[i] << (i * 8);
                    }
                    return lcVal;
                }
            }
            else if (lcObj is Rational rational)
            {
                return (rational).IntValue();
            }
            else if (lcObj is byte[] lcTab)
            {
                if (lcTab.Length > 0)
                {
                    return (int)lcTab[0];
                }
            }
            else if (lcObj is int || lcObj is byte || lcObj is long || lcObj is float || lcObj is double)
            {
                try
                {
                    return Convert.ToInt32(lcObj);
                }
                catch (FormatException e)
                {
                    throw new MetadataException("Unable to parse as int object of type:'" + lcObj.GetType() + "' that look like:'" + lcObj.ToString() + "'", e);
                }
            }
            throw new MetadataException("Obj is :" + lcObj.GetType() + " and look like:" + lcObj.ToString());
        }

        /// <summary>
        /// Gets the specified tag value as a string array, if possible.  Only supported where the tag is set as string[], string, int[], byte[] or Rational[].
        /// </summary>
        /// <param name="aTagType">the tag identifier</param>
        /// <returns>the tag value as an array of Strings</returns>
        /// <exception cref="MetadataException">if tag not found or if it cannot be represented as a string[]</exception>
        public string[] GetStringArray(int aTagType)
        {
            object lcObj = GetObject(aTagType);
            if (lcObj == null)
            {
                throw new MetadataException(
                    "Tag "
                    + GetTagName(aTagType)
                    + " has not been set -- check using containsTag() first");
            }
            else if (lcObj is string[] strArray)
            {
                return strArray;
            }
            else if (lcObj is string str)
            {
                return new string[] { str };
            }
            else if (lcObj is int[] lcInts)
            {
                string[] lcStrings = new string[lcInts.Length];
                for (int i = 0; i < lcStrings.Length; i++)
                {
                    lcStrings[i] = lcInts[i].ToString();
                }
                return lcStrings;
            }
            else if (lcObj is byte[] lcBytes)
            {
                string[] lcStrings = new string[lcBytes.Length];
                for (int i = 0; i < lcStrings.Length; i++)
                {
                    lcStrings[i] = lcBytes[i].ToString();
                }
                return lcStrings;
            }
            else if (lcObj is Rational[] lcRationals)
            {
                string[] lcStrings = new string[lcRationals.Length];
                for (int i = 0; i < lcStrings.Length; i++)
                {
                    lcStrings[i] = lcRationals[i].ToSimpleString(false);
                }
                return lcStrings;
            }
            throw new MetadataException("Obj is :" + lcObj.GetType() + " and look like:" + lcObj.ToString());
        }

        /// <summary>
        /// Gets the specified tag value as an int array, if possible.  Only supported where the tag is set as string, int[], byte[] or Rational[].
        /// </summary>
        /// <param name="aTagType">the tag identifier</param>
        /// <returns>the tag value as an int array</returns>
        /// <exception cref="MetadataException">if tag not found or if it cannot be represented as a int[]</exception>
        public int[] GetIntArray(int aTagType)
        {
            object lcObj = GetObject(aTagType);
            if (lcObj == null)
            {
                throw new MetadataException(
                    "Tag "
                    + GetTagName(aTagType)
                    + " has not been set -- check using containsTag() first");
            }
            else if (lcObj is Rational[] lcRationals)
            {
                int[] lcInts = new int[lcRationals.Length];
                for (int i = 0; i < lcInts.Length; i++)
                {
                    lcInts[i] = lcRationals[i].IntValue();
                }
                return lcInts;
            }
            else if (lcObj is int[] intArray)
            {
                return intArray;
            }
            else if (lcObj is byte[] lcBytes)
            {
                int[] lcInts = new int[lcBytes.Length];
                for (int i = 0; i < lcBytes.Length; i++)
                {
                    lcInts[i] = lcBytes[i];
                }
                return lcInts;
            }
            else if (lcObj is string lcStr)
            {
                int[] lcInts = new int[lcStr.Length];
                for (int i = 0; i < lcStr.Length; i++)
                {
                    lcInts[i] = lcStr[i];
                }
                return lcInts;
            }
            throw new MetadataException("Obj is :" + lcObj.GetType() + " and look like:" + lcObj.ToString());
        }

        /// <summary>
        /// Gets the specified tag value as an byte array, if possible.  Only supported where the tag is set as string, int[], byte[] or Rational[].
        /// </summary>
        /// <param name="aTagType">the tag identifier</param>
        /// <returns>the tag value as a byte array</returns>
        /// <exception cref="MetadataException">if tag not found or if it cannot be represented as a byte[]</exception>
        public byte[] GetByteArray(int aTagType)
        {
            object lcObj = GetObject(aTagType);
            if (lcObj == null)
            {
                throw new MetadataException(
                    "Tag "
                    + GetTagName(aTagType)
                    + " has not been set -- check using containsTag() first");
            }
            else if (lcObj is Rational[] lcRationals)
            {
                byte[] lcBytes = new byte[lcRationals.Length];
                for (int i = 0; i < lcBytes.Length; i++)
                {
                    lcBytes[i] = lcRationals[i].ByteValue();
                }
                return lcBytes;
            }
            else if (lcObj is byte[] byteArray)
            {
                return byteArray;
            }
            else if (lcObj is int[] lcInts)
            {
                byte[] lcBytes = new byte[lcInts.Length];
                for (int i = 0; i < lcInts.Length; i++)
                {
                    lcBytes[i] = (byte)lcInts[i];
                }
                return lcBytes;
            }
            else if (lcObj is string lcStr)
            {
                byte[] lcBytes = new byte[lcStr.Length];
                for (int i = 0; i < lcStr.Length; i++)
                {
                    lcBytes[i] = (byte)lcStr[i];
                }
                return lcBytes;
            }
            throw new MetadataException("Obj is :" + lcObj.GetType() + " and look like:" + lcObj.ToString());
        }

        /// <summary>
        /// Returns the specified tag value as a double, if possible.
        /// </summary>
        /// <param name="aTagType">the specified tag type</param>
        /// <returns>the specified tag value as a double, if possible.</returns>
        public double GetDouble(int aTagType)
        {
            object lcObj = GetObject(aTagType);
            if (lcObj == null)
            {
                throw new MetadataException(
                    "Tag "
                    + GetTagName(aTagType)
                    + " has not been set -- check using containsTag() first");
            }
            else if (lcObj is Rational rational)
            {
                return rational.DoubleValue();
            }
            else if (lcObj is double || lcObj is string || lcObj is int || lcObj is byte || lcObj is long || lcObj is float)
            {
                try
                {
                    return Convert.ToDouble(lcObj);
                }
                catch (FormatException e)
                {
                    throw new MetadataException("Unable to parse as double object of type:'" + lcObj.GetType() + "' that look like:'" + lcObj.ToString() + "'", e);
                }
            }
            throw new MetadataException("Obj is :" + lcObj.GetType() + " and look like:" + lcObj.ToString());
        }

        /// <summary>
        /// Returns the specified tag value as a float, if possible.
        /// </summary>
        /// <param name="aTagType">the specified tag type</param>
        /// <returns>the specified tag value as a float, if possible.</returns>
        public float GetFloat(int aTagType)
        {
            object lcObj = GetObject(aTagType);
            if (lcObj == null)
            {
                throw new MetadataException(
                    "Tag "
                    + GetTagName(aTagType)
                    + " has not been set -- check using containsTag() first");
            }
            else if (lcObj is Rational rational)
            {
                return rational.FloatValue();
            }
            else if (lcObj is float || lcObj is string || lcObj is int || lcObj is byte || lcObj is long || lcObj is double)
            {
                try
                {
                    return (float)Convert.ToDouble(lcObj);
                }
                catch (FormatException e)
                {
                    throw new MetadataException("Unable to parse as float object of type:'" + lcObj.GetType() + "' that look like:'" + lcObj.ToString() + "'", e);
                }
            }
            throw new MetadataException("Obj is :" + lcObj.GetType() + " and look like:" + lcObj.ToString());
        }

        /// <summary>
        /// Returns the specified tag value as a long, if possible.
        /// </summary>
        /// <param name="aTagType">the specified tag type</param>
        /// <returns>the specified tag value as a long, if possible.</returns>
        public long GetLong(int aTagType)
        {
            object lcObj = GetObject(aTagType);
            if (lcObj == null)
            {
                throw new MetadataException(
                    "Tag "
                    + GetTagName(aTagType)
                    + " has not been set -- check using containsTag() first");
            }
            else if (lcObj is Rational rational)
            {
                return (rational).LongValue();
            }
            else if (lcObj is long || lcObj is string || lcObj is int || lcObj is byte || lcObj is float || lcObj is double)
            {
                try
                {
                    return Convert.ToInt64(lcObj);
                }
                catch (FormatException e)
                {
                    throw new MetadataException("Unable to parse as long object of type:'" + lcObj.GetType() + "' that look like:'" + lcObj.ToString() + "'", e);
                }
            }
            throw new MetadataException("Obj is :" + lcObj.GetType() + " and look like:" + lcObj.ToString());
        }

        /// <summary>
        /// Returns the specified tag value as a boolean, if possible.
        /// </summary>
        /// <param name="aTagType">the specified tag type</param>
        /// <returns>the specified tag value as a boolean, if possible.</returns>
        public bool GetBoolean(int aTagType)
        {
            object lcObj = GetObject(aTagType);
            if (lcObj == null)
            {
                throw new MetadataException(
                    "Tag "
                    + GetTagName(aTagType)
                    + " has not been set -- check using containsTag() first");
            }
            else if (lcObj is Boolean boolean)
            {
                return boolean;
            }
            else if (lcObj is string str)
            {
                try
                {
                    return Convert.ToBoolean(str);
                }
                catch (FormatException e)
                {
                    throw new MetadataException("Unable to parse as boolean object of type:'" + lcObj.GetType() + "' that look like:'" + lcObj.ToString() + "'", e);
                }
            }
            throw new MetadataException("Obj is :" + lcObj.GetType() + " and look like:" + lcObj.ToString());
        }

        /// <summary>
        /// Returns the specified tag value as a date, if possible.
        /// </summary>
        /// <param name="aTagType">the specified tag type</param>
        /// <returns>the specified tag value as a date, if possible.</returns>
        public DateTime GetDate(int aTagType)
        {
            object lcObj = GetObject(aTagType);
            if (lcObj == null)
            {
                throw new MetadataException(
                    "Tag "
                    + GetTagName(aTagType)
                    + " has not been set -- check using containsTag() first");
            }
            else if (lcObj is DateTime dateTime)
            {
                return dateTime;
            }
            else if (lcObj is string lcDateString)
            {
                try
                {
                    return DateTime.Parse(lcDateString);
                }
                catch (FormatException)
                {
                    // Was not able to parse date using standard format
                    // We try the following format
                    DateTime resu = AbstractDirectory.ParseDate(lcDateString);
                    if (resu == DateTime.Today)
                    {
                        Trace.TraceWarning("Was not able to parse date '" + lcDateString + "'");
                    }
                    return resu;
                }
            }
            throw new MetadataException("Obj is :" + lcObj.GetType() + " and look like:" + lcObj.ToString());
        }

        /// <summary>
        /// Will try to transform the string in date. <br/>
        /// Will use all date format found in DATE_FORMATS
        /// </summary>
        /// <param name="aDate">the date to parse</param>
        /// <returns>the date found or today if none found (today because null is not a date in C#)</returns>
        private static DateTime ParseDate(string aDate)
        {
            if (aDate == null || aDate.Trim().Length == 0)
            {
                return DateTime.Today;
            }
            for (int i = 0; i < DATE_FORMATS.Length; i++)
            {
                try
                {
                    return DateTime.ParseExact(aDate, DATE_FORMATS[i], null);
                }
                catch (FormatException)
                {
                    Debug.Write("Date '" + aDate + "' does not match patern '" + DATE_FORMATS[i] + "', will try an other one");
                }
            }
            // If we get here it means that no format worked.
            return DateTime.Today;
        }


        /// <summary>
        /// Returns the specified tag value as a rational, if possible.
        /// </summary>
        /// <param name="aTagType">the specified tag type</param>
        /// <returns>the specified tag value as a rational, if possible.</returns>
        public Rational GetRational(int aTagType)
        {
            object lcObj = GetObject(aTagType);
            if (lcObj == null)
            {
                throw new MetadataException(
                    "Tag "
                    + GetTagName(aTagType)
                    + " has not been set -- check using containsTag() first");
            }
            else if (lcObj is Rational rational)
            {
                return rational;
            }
            throw new MetadataException("Obj is :" + lcObj.GetType().Name + " and look like:" + lcObj.ToString());
        }

        /// <summary>
        /// Gets the specified tag value as a rational array, if possible.  Only supported where the tag is set as Rational[].
        /// </summary>
        /// <param name="aTagType">the tag identifier</param>
        /// <returns>the tag value as a rational array</returns>
        /// <exception cref="MetadataException">if tag not found or if it cannot be represented as a rational[]</exception>
        public Rational[] GetRationalArray(int aTagType)
        {
            object lcObj = GetObject(aTagType);
            if (lcObj == null)
            {
                throw new MetadataException(
                    "Tag "
                    + GetTagName(aTagType)
                    + " has not been set -- check using containsTag() first");
            }
            else if (lcObj is Rational[] rationals)
            {
                return rationals;
            }
            throw new MetadataException("Obj is :" + lcObj.GetType().Name + " and look like:" + lcObj.ToString());
        }

        /// <summary>
        /// Returns the specified tag value as a string.
        /// This value is the 'raw' value.
        /// A more presentable decoding of this value may be obtained from the corresponding Descriptor.
        /// </summary>
        /// <param name="aTagType">the specified tag type</param>
        /// <returns>the string reprensentation of the tag value, or null if the tag hasn't been defined.</returns>
        public string GetString(int aTagType)
        {

            object lcObj = GetObject(aTagType);
            if (lcObj == null)
            {
                return null;
            }
            else if (lcObj is Rational rational)
            {
                return (rational).ToSimpleString(true);
            }
            else if (lcObj.GetType().IsArray)
            {
                string lcStr = lcObj.GetType().ToString();

                int lcArrayLength = 0;

                if (lcStr.IndexOf("Int") != -1)
                {
                    // handle arrays of objects and primitives
                    lcArrayLength = ((int[])lcObj).Length;
                }
                else if (lcStr.IndexOf("Rational") != -1)
                {
                    lcArrayLength = ((Rational[])lcObj).Length;
                }
                else if (lcStr.IndexOf("string") != -1 || lcStr.IndexOf("String") != -1)
                {
                    lcArrayLength = ((string[])lcObj).Length;
                }

                StringBuilder lcBuff = new StringBuilder();
                for (int i = 0; i < lcArrayLength; i++)
                {
                    if (i != 0)
                    {
                        lcBuff.Append(' ');
                    }
                    if (lcStr.IndexOf("Int") != -1)
                    {
                        lcBuff.Append(((int[])lcObj)[i].ToString());
                    }
                    else if (lcStr.IndexOf("Rational") != -1)
                    {
                        lcBuff.Append(((Rational[])lcObj)[i].ToString());
                    }
                    else if (lcStr.IndexOf("string") != -1 || lcStr.IndexOf("String") != -1)
                    {
                        lcBuff.Append(((string[])lcObj)[i].ToString());
                    }
                }
                return lcBuff.ToString();
            }
            return lcObj.ToString();
        }

        /// <summary>
        /// Returns the object hashed for the particular tag type specified, if available.
        /// </summary>
        /// <param name="aTagType">the tag type identifier</param>
        /// <returns>the tag value as an object if available, else null</returns>
        public object GetObject(int aTagType)
        {
            if (this.tagMap.ContainsKey(aTagType))
            {
                return this.tagMap[aTagType];
            }
            return null;
        }

        /// <summary>
        /// Returns the name of a specified tag as a string.
        /// </summary>
        /// <param name="aTagType">the tag type identifier</param>
        /// <returns>the tag name as a string</returns>
        public string GetTagName(int aTagType)
        {
            if (!AbstractDirectory.tagNameMap[this.GetType()].ContainsKey(aTagType))
            {
                StringBuilder buff = new StringBuilder(32);
                buff.Append("Unknown tag (0x");
                string lcHex = aTagType.ToString("X");
                for (int i = 0; i < 4 - lcHex.Length; i++)
                {
                    buff.Append('0');
                }
                return buff.Append(lcHex).Append(')').ToString();
            }
            return AbstractDirectory.tagNameMap[this.GetType()][aTagType];
        }

        /// <summary>
        /// Provides a description of a tag value using the descriptor set by setDescriptor(Descriptor).
        /// </summary>
        /// <param name="aTagType">the tag type identifier</param>
        /// <returns>the tag value'str description as a string</returns>
        /// <exception cref="MetadataException">if a descriptor hasn't been set, or if an error occurs during calculation of the description within the Descriptor</exception>
        public string GetDescription(int aTagType)
        {
            if (this.descriptor == null)
            {
                throw new MetadataException("A descriptor must be set using setDescriptor(...) before descriptions can be provided");
            }

            return this.descriptor.GetDescription(aTagType);
        }

        /// <summary>
        /// Provides the name of the directory, for display purposes.  E.g. Exif
        /// </summary>
        /// <returns>the name of the directory</returns>
        public string GetName()
        {
            return ResourceBundleFactory.CreateDefaultBundle(this.BundleName)["MARKER_NOTE_NAME"];
        }

        /// <summary>
        /// Fill the map with all (TAG_xxx value, BUNDLE[TAG_xxx name]).
        /// </summary>
        /// <param name="aType">where to look for fields like TAG_xxx</param>
        /// <param name="aTagMap">where to put tag found</param>
        protected static IDictionary<int, string> FillTagMap(Type aType, IResourceBundle aBundle)
        {
            FieldInfo[] lcAllContTag = aType.GetFields();
            IDictionary<int, string> lcResu = new Dictionary<int, string>(lcAllContTag.Length);
            for (int i = 0; i < lcAllContTag.Length; i++)
            {
                string lcMemberName = lcAllContTag[i].Name;
                if (lcAllContTag[i].IsPublic && lcMemberName.StartsWith("TAG_"))
                {
                    int lcMemberValue = (int)lcAllContTag[i].GetValue(null);
                    try
                    {
                        lcResu.Add(lcMemberValue, aBundle[lcMemberName]);
                    }
                    catch (MissingResourceException mre)
                    {
                        Trace.TraceError("Could not find the key '" + aType + "' for type '" + lcMemberName + "' (" + mre.Message + ")");
                    }
                }
            }
            return lcResu;
        }
    }
}
