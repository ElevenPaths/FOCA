using System.Collections;
using System.Collections.Generic;
using System.Resources;

namespace com.utils.bundle
{

    public sealed class SpecialResourceWriter
    {
        public SpecialResourceWriter()
        {
            // Load all bunlde
            IList<IResourceBundle> allBundle = new List<IResourceBundle>(20);
            allBundle.Add(ResourceBundleFactory.CreateBundle("CanonMarkernote", null, ResourceBundleFactory.USE_TXTFILE));
            allBundle.Add(ResourceBundleFactory.CreateBundle("CasioMarkernote", null, ResourceBundleFactory.USE_TXTFILE));
            allBundle.Add(ResourceBundleFactory.CreateBundle("Commons", null, ResourceBundleFactory.USE_TXTFILE));
            allBundle.Add(ResourceBundleFactory.CreateBundle("ExifInteropMarkernote", null, ResourceBundleFactory.USE_TXTFILE));
            allBundle.Add(ResourceBundleFactory.CreateBundle("ExifMarkernote", null, ResourceBundleFactory.USE_TXTFILE));
            allBundle.Add(ResourceBundleFactory.CreateBundle("FujiFilmMarkernote", null, ResourceBundleFactory.USE_TXTFILE));
            allBundle.Add(ResourceBundleFactory.CreateBundle("GpsMarkernote", null, ResourceBundleFactory.USE_TXTFILE));
            allBundle.Add(ResourceBundleFactory.CreateBundle("IptcMarkernote", null, ResourceBundleFactory.USE_TXTFILE));
            allBundle.Add(ResourceBundleFactory.CreateBundle("JpegMarkernote", null, ResourceBundleFactory.USE_TXTFILE));
            allBundle.Add(ResourceBundleFactory.CreateBundle("KodakMarkernote", null, ResourceBundleFactory.USE_TXTFILE));
            allBundle.Add(ResourceBundleFactory.CreateBundle("KyoceraMarkernote", null, ResourceBundleFactory.USE_TXTFILE));
            allBundle.Add(ResourceBundleFactory.CreateBundle("NikonTypeMarkernote", null, ResourceBundleFactory.USE_TXTFILE));
            allBundle.Add(ResourceBundleFactory.CreateBundle("OlympusMarkernote", null, ResourceBundleFactory.USE_TXTFILE));
            allBundle.Add(ResourceBundleFactory.CreateBundle("PanasonicMarkernote", null, ResourceBundleFactory.USE_TXTFILE));
            allBundle.Add(ResourceBundleFactory.CreateBundle("PentaxMarkernote", null, ResourceBundleFactory.USE_TXTFILE));
            allBundle.Add(ResourceBundleFactory.CreateBundle("SonyMarkernote", null, ResourceBundleFactory.USE_TXTFILE));

            IEnumerator<IResourceBundle> enumRb = allBundle.GetEnumerator();
            while (enumRb.MoveNext())
            {
                IResourceBundle bdl = enumRb.Current;
                IDictionary<string,string> idic = bdl.Entries;
                IDictionaryEnumerator enumDic =  (IDictionaryEnumerator)idic.GetEnumerator();
                using (var rw = new ResourceWriter(bdl.Fullname + ".resources"))
                {
                    while (enumDic.MoveNext())
                    {
                        rw.AddResource((string)enumDic.Key, (string)enumDic.Value);
                    }
                }
            }
        }
    }

}