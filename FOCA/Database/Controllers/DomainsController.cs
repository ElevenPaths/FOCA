using FOCA.Database.Entities;
using FOCA.ModifiedComponents;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace FOCA.Database.Controllers
{
    public class DomainsController : BaseController<DomainsItem>
    {

        public override void Save(IList<DomainsItem> items)
        {
            if (items.Count == 0)
                return;

            using (FocaContextDb context = new FocaContextDb())
            {
                HttpMapController httpMap = new HttpMapController();

                foreach (var domainsItem in items)
                {
                    context.Domains.AddOrUpdate(domainsItem);

                    context.SaveChanges();

                    httpMap.Save(domainsItem.map);
                }

                context.SaveChanges();
            }

        }

        /// <summary>
        /// Get Domains by Id.
        /// </summary>
        /// <param name="idProject"></param>
        /// <returns></returns>
        public ThreadSafeList<DomainsItem> GetDomainsById(int idProject)
        {
            using (FocaContextDb context = new FocaContextDb())
            {
                var result = context.Domains.Where(x => x.IdProject == idProject).Include("fingerPrinting").Include("map");
                LoadHttpMapValues(result);

                return new ThreadSafeList<DomainsItem>(result);
            }
        }

        /// <summary>
        /// Load value reference Httmap.
        /// </summary>
        /// <param name="result"></param>
        private void LoadHttpMapValues(IQueryable<DomainsItem> result)
        {
            var mapController = new HttpMapController();

            foreach (var item in result)
            {
                item.map.Documents = mapController.GetItemsByTypeById(2, item.map.Id);
                item.map.Files = mapController.GetItemsByTypeById(3, item.map.Id);
                item.map.Folders = mapController.GetItemsByTypeById(4, item.map.Id);
                item.map.Parametrized = mapController.GetItemsByTypeById(5, item.map.Id);
            }
        }
    }
}
