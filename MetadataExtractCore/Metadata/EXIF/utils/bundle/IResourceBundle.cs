using System.Collections.Generic;

namespace com.utils.bundle
{
    /// <summary>
    /// This interface represent a bundle class.<br/>
    /// 
    /// Used for internationalisation (multi-language).<br/>
    /// 
    /// Allow the use of messages with holes.<br/>
    /// </pre>
    /// </summary>

    public interface IResourceBundle
    {
        /// <summary>
        /// Gets/sets the name of this bundle (ex: CanonMarkernote).
        /// </summary>
        string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets/sets the full name of this bundle (ex: /resources/en/CanonMarkernote.txt)
        /// </summary>
        string Fullname
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the dictionnaries entry for this bundle.
        /// </summary>
        IDictionary<string, string> Entries
        {
            get;            
        }



        /// <summary>
        /// Indexator on a simple aMessage.
        /// </summary>
        /// <param name="aKey">the referenced key</param>
        /// <returns>the aMessage attached to this key, or launch a MissingResourceException if none found</returns>
        string this[string aKey]
        {
            get;
        }

        /// <summary>
        /// Indexator on a aMessage with one hole {0} in it.
        /// </summary>
        /// <param name="aKey">the referenced key</param>
        /// <param name="fillGapWith">what to put in hole {0}</param>
        /// <returns>the aMessage attached to this key, or launch a MissingResourceException if none found</returns>
        string this[string aKey, string fillGapWith]
        {
            get;
        }

        /// <summary>
        /// Indexator on a aMessage with two holes {0} and {1} in it.
        /// </summary>
        /// <param name="aKey">the referenced key</param>
        /// <param name="fillGap0">what to put in hole {0}</param>
        /// <param name="fillGap1">what to put in hole {1}</param>
        /// <returns>the aMessage attached to this key, or launch a MissingResourceException if none found</returns>
        string this[string aKey, string fillGap0, string fillGap1]
        {
            get;
        }

        /// <summary>
        /// Indexator on a aMessage with many holes {0}, {1}, {2] ... in it.
        /// </summary>
        /// <param name="aKey">the referenced key</param>
        /// <param name="fillGapWith">what to put in holes. fillGapWith[0] used for {0}, fillGapWith[1] used for {1} ...</param>
        /// <returns>the aMessage attached to this key, or launch a MissingResourceException if none found</returns>
        string this[string aKey, string[] fillGapWith]
        {
            get;
        }
    }
}