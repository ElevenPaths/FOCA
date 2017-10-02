using System.Data.Entity;
using System.Linq;
using FOCA.ModifiedComponents;

namespace FOCA.Controllers
{
    public class DomainsController : BaseController
    {
        /// <summary>
        /// Save items.
        /// </summary>
        /// <param name="items"></param>
        public void Save(ThreadSafeList<DomainsItem> items)
        {
            if (items.Count == 0)
                return;

            foreach (var domainItem in items)
            {
                if (domainItem.Id == 0)
                    AddNew(domainItem);
                else
                    Update(domainItem);
            }

            CurrentContextDb.SaveChanges();
        }

        /// <summary>
        /// Update Item.
        /// </summary>
        /// <param name="item"></param>
        private static void Update(DomainsItem item)
        {
           var domain = CurrentContextDb.Domains.FirstOrDefault(x => x.Id == item.Id);

            if (domain != null)
            {
                domain.IdProject = Program.data.Project.Id;
                domain.Domain = item.Domain;
                domain.fingerPrinting = item.fingerPrinting;
                domain.informationOptions = item.informationOptions;
                domain.jspFingerprintingAnalyzed = item.jspFingerprintingAnalyzed;
                domain.map = item.map;
                domain.multiplesChoisesAnalyzed = item.multiplesChoisesAnalyzed;
                domain.os = item.os;
                domain.robotsAnalyzed = item.robotsAnalyzed;
                domain.Source = item.Source;
                domain.techAnalysis = item.techAnalysis;
            }
        }

        /// <summary>
        /// Add new.
        /// </summary>
        /// <param name="item"></param>
        private static void AddNew(DomainsItem item)
        {
            item.IdProject = Program.data.Project.Id;
            CurrentContextDb.Domains.Add(item);

            CurrentContextDb.SaveChanges();

            new HttpMapController().Save(item.map);
        }

        /// <summary>
        /// Get Domains by Id.
        /// </summary>
        /// <param name="idProject"></param>
        /// <returns></returns>
        public ThreadSafeList<DomainsItem> GetDomainsById(int idProject)
        {
            var result = CurrentContextDb.Domains.Where(x => x.IdProject == idProject).Include("fingerPrinting").Include("map");
            LoadHttpMapValues(result);
            var items = new ThreadSafeList<DomainsItem>(result);

            return items;
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
                item.map.Files     = mapController.GetItemsByTypeById(3, item.map.Id);
                item.map.Folders   = mapController.GetItemsByTypeById(4, item.map.Id);
                item.map.Parametrized = mapController.GetItemsByTypeById(5, item.map.Id);
            }
        }
    }
}
