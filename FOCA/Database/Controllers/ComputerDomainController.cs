using FOCA.Database.Entities;
using FOCA.ModifiedComponents;
using System.Data.Entity;
using System.Linq;

namespace FOCA.Database.Controllers
{
    public class ComputerDomainController : BaseController
    {
        public void Save(ThreadSafeList<ComputerDomainsItem> items)
        {
            if (items.Count == 0)
                return;

            foreach (var comDomainItem in items)
            {
                if (comDomainItem.Id == 0)
                    AddNew(comDomainItem);
                else
                    Update(comDomainItem);
            }

            CurrentContextDb.SaveChanges();
        }

        private static void Update(ComputerDomainsItem item)
        {

            var comDomain = CurrentContextDb.ComputerDomain.FirstOrDefault(x => x.Id == item.Id);

            if (comDomain != null)
            {
                comDomain.IdProject = Program.data.Project.Id;
                comDomain.Source = item.Source;
                comDomain.Computer = item.Computer;
                comDomain.Domain = item.Domain;
            }
        }

        private static void AddNew(ComputerDomainsItem item)
        {
            item.IdProject = Program.data.Project.Id;
            CurrentContextDb.ComputerDomain.Add(item);
        }

        public ThreadSafeList<ComputerDomainsItem> GetComputerDomainByIdProject(int idProject)
        {
            var result = CurrentContextDb.ComputerDomain.Where(x => x.IdProject == idProject).Include("Computer").Include("Domain");

            var items = new ThreadSafeList<ComputerDomainsItem>(result);

            return items;
        }
    }
}
