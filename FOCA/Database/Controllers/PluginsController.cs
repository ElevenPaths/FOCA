using System;
using System.Linq;
using FOCA.ModifiedComponents;
using FOCA.Plugins;

namespace FOCA.Database.Controllers
{
    public class PluginsController : BaseController
    {
        public void Save(ThreadSafeList<Plugin> items)
        {
            try
            {
                if (items.Count == 0)
                    return;

                var allPlugins = CurrentContextDb.Plugins.ToList();
                CurrentContextDb.Plugins.RemoveRange(allPlugins);
                CurrentContextDb.Plugins.AddRange(items);

                CurrentContextDb.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ThreadSafeList<Plugin> GetAllPlugins()
        {
            var result = CurrentContextDb.Plugins.ToList();
   
            var items = new ThreadSafeList<Plugin>(result);

            return items;
        }

    }
}

