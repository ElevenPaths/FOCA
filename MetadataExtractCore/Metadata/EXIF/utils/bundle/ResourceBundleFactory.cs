using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace com.utils.bundle
{
    /// <summary>
    /// This class is a bundle factory class.<br/>
    /// 
    /// You can switch the implementation or ResourceBundle using this class.<br/>
    /// </pre>
    /// </summary>

    public sealed class ResourceBundleFactory 
    {
        public static int USE_MANAGER = 0;
        public static int USE_TXTFILE = 1;

        /// <summary>
        /// Indicates the default instance you want to use for ALL your bundle.
        /// </summary>
        private static int DEFAULT_USE = USE_TXTFILE;

        /// <summary>
        /// All bundle stored in this dictionnary.
        /// </summary>
        private static IDictionary<string, IResourceBundle> BUNDLES = new Dictionary<string, IResourceBundle>();



        /// <summary>
        /// Constructor of the object. <br/>
        /// Does nothing, do not use.
        /// </summary>
        private ResourceBundleFactory()
        {
        }

        /// <summary>
        /// Gives an instance of resource bundle using default choice (see DEFAULT_USE).
        /// Caution <i>CultureInfo.CurrentCulture</i> will be used.
        /// </summary>
        /// <param name="aName">Name of the bundle you are looking for (ex: CanonMarkernote)</param>
        /// <returns></returns>
        public static IResourceBundle CreateDefaultBundle(string aName)
        {
            return ResourceBundleFactory.CreateBundle(aName, CultureInfo.CurrentCulture, ResourceBundleFactory.DEFAULT_USE);
        }

        /// <summary>
        /// Gives an instance of resource bundle using default choice (see DEFAULT_USE).
        /// </summary>
        /// <param name="aName">Name of the bundle you are looking for (ex: CanonMarkernote)</param>
        /// <param name="aCulturalInfo">a cultural info. Can be null</param>
        /// <returns></returns>
        public static IResourceBundle CreateDefaultBundle(string aName, CultureInfo aCulturalInfo)
        {
            return ResourceBundleFactory.CreateBundle(aName, aCulturalInfo, ResourceBundleFactory.DEFAULT_USE);
        }

        /// <summary>
        /// Gives an instance of resource bundle.
        /// </summary>
        /// <param name="aName">Name of the bundle you are looking for (ex: CanonMarkernote)</param>
        /// <param name="aCulturalInfo">a cultural info. Can be null</param>
        /// <param name="aType">a type of bundle (See USE_MANAGER or USE_TXTFILE)</param>
        /// <returns>the bundle found or loade.</returns>
        public static IResourceBundle CreateBundle(string aName, CultureInfo aCulturalInfo, int aType)
        {
            string key = aName;
            if (aCulturalInfo != null)
            {
                key += "_" + aCulturalInfo.ToString();
            }

            IResourceBundle resu = null;
            if (!ResourceBundleFactory.BUNDLES.ContainsKey(key))
            {
                try
                {
                    if (aType == ResourceBundleFactory.USE_MANAGER)
                    {
                        resu = new ResourceBundleWithManager(aName, aCulturalInfo);
                    }
                    else if (aType == ResourceBundleFactory.USE_TXTFILE)
                    {
                        resu = new ResourceBundle(aName, aCulturalInfo);
                    }
                }
                catch (Exception e)
                {
                    Trace.TraceError("Could not load bundle '" + aName + "' (" + e.Message + ")");
                }
                if (resu == null || resu["TEST"] == null)
                {
                    throw new Exception("Error while loading bundle '" + aName + "' for cultural '" + aCulturalInfo + "'");
                }
                ResourceBundleFactory.BUNDLES.Add(key, resu);
            }
            else
            {
                resu = ResourceBundleFactory.BUNDLES[key];
            }
            return resu;
        }
    }
}