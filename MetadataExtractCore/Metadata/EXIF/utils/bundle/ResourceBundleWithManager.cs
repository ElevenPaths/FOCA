using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using System.Globalization;

namespace com.utils.bundle
{
    /// <summary>
    /// This class is a bundle class that <i>try</i> to use the xxx.resources file.<br/>
    /// 
    /// For some misterious reason I could not make it work so it is not use.<br/>
    /// I keep it in cas I would need it some day. <br/>
    /// </summary>

    sealed class ResourceBundleWithManager : AbstractResourceBundle
    {
        private CultureInfo culturalInfo;
        private ResourceManager resourceManager;

        private IDictionary<string, string> resourceManagerAsDic;
        public override IDictionary<string, string> Entries
        {
            get { return this.resourceManagerAsDic; }
        }


        /// <summary>
        /// Indexator on a aMessage with many holes {0}, {1}, {2] ... in it.
        /// </summary>
        /// <param name="aKey">the referenced key</param>
        /// <param name="fillGapWith">what to put in holes. fillGapWith[0] used for {0}, fillGapWith[1] used for {1} ...</param>
        /// <returns>the aMessage attached to this key, or launch a MissingResourceException if none found</returns>
        public override string this[string aKey, string[] fillGapWith]
        {
            get
            {
                string resu = this.resourceManager.GetString(aKey, this.culturalInfo);
                if (resu == null)
                {
                    throw new MissingResourceException("\"" + aKey + "\" Not found");
                }
                return replace(resu, fillGapWith);
            }
        }

        /// <summary>
        /// Constructor of the object.
        /// 
        /// Keep private, use the other one.
        /// </summary>
        private ResourceBundleWithManager()
            : base()
        {
        }

        /// <summary>
        /// Constructor of the object.
        /// </summary>
        /// <param name="aPropertyFileName">The resource file where to find keys. Do not add the extension and do not forget to add your resource file into the assembly.</param>
        public ResourceBundleWithManager(string aPropertyFileName)
            : this(aPropertyFileName, null)
        {
        }

        /// <summary>
        /// Constructor of the object.
        /// </summary>
        /// <param name="aPropertyFileName">The resource file where to find keys. Do not add the extension and do not forget to add your resource file into the assembly.</param>
        /// <param name="aCultureInfo">The culture info. Can be null</param>
        public ResourceBundleWithManager(string aPropertyFileName, CultureInfo aCulturalInfo)
            : base()
        {
            this.resourceManager = new ResourceManager(aPropertyFileName, Assembly.GetExecutingAssembly());
            this.resourceManagerAsDic = new Dictionary<string, string>();
            if (aCulturalInfo != null)
            {
                ResourceSet rs = this.resourceManager.GetResourceSet(aCulturalInfo, false, false);
                IDictionaryEnumerator idicnum = rs.GetEnumerator();
                while (idicnum.MoveNext())
                {
                    this.resourceManagerAsDic.Add((string)idicnum.Key, (string)idicnum.Value);
                }
            }
            this.Name = aPropertyFileName;
            this.Fullname = this.resourceManager.BaseName;

            this.culturalInfo = aCulturalInfo;
        }

        /// <summary>
        /// Clean the object.
        /// </summary>				
        public void Dispose()
        {
            if (this.resourceManager != null)
            {
                this.resourceManager.ReleaseAllResources();
                this.resourceManager = null;
            }
        }
    }
}