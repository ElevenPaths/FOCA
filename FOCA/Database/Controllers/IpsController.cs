using FOCA.Database.Entities;
using FOCA.ModifiedComponents;
using System.Data.Entity.Validation;
using System.Linq;

namespace FOCA.Database.Controllers
{
    public class IpsController : BaseController
    {
        public void Save(ThreadSafeList<IPsItem> items)
        {
            if (items.Count == 0)
                return;

            foreach (var ipItem in items)
            {
                if (ipItem.Id == 0)
                    AddNew(ipItem);
                else
                    Update(ipItem);
            }

            CurrentContextDb.SaveChanges();
        }

        private static void Update(IPsItem item)
        {
            var ipItem = CurrentContextDb.Ips.FirstOrDefault(x => x.Id == item.Id);

            if (ipItem != null)
            {
                ipItem.Information = item.Information;
                ipItem.Ip = item.Ip;
                ipItem.Source = item.Source;
                ipItem.ZoneTransfer = item.ZoneTransfer;
                ipItem.activeDNS = item.activeDNS;
            }
        }

        private static void AddNew(IPsItem item)
        {
            try
            {
                item.IdProject = Program.data.Project.Id;
                CurrentContextDb.Ips.Add(item);
            }
            catch (DbEntityValidationException ex)
            {
                throw new DbEntityValidationException(ex.Message);
            }
        }

        public ThreadSafeList<IPsItem> GetIpsByIdProject(int idProject)
        {
            var result = CurrentContextDb.Ips.Where(x => x.IdProject == idProject);

            var items = new ThreadSafeList<IPsItem>(result);

            return items;
        }
    }
}
