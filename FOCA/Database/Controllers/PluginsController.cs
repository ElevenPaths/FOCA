using FOCA.ModifiedComponents;
using FOCA.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FOCA.Database.Controllers
{
    public class PluginsController : BaseController<Plugin>
    {
        public override void Save(IList<Plugin> items)
        {
            try
            {
                if (items.Count == 0)
                    return;

                using (FocaContextDb context = new FocaContextDb())
                {
                    var allPlugins = context.Plugins.ToList();
                    context.Plugins.RemoveRange(allPlugins);
                    context.Plugins.AddRange(items);

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ThreadSafeList<Plugin> GetAllPlugins()
        {
            using (FocaContextDb context = new FocaContextDb())
            {
                var result = context.Plugins.ToList();
                return new ThreadSafeList<Plugin>(result); ;
            }
        }
    }
}

