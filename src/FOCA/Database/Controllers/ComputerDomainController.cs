using FOCA.Database.Entities;
using FOCA.ModifiedComponents;
using System.Data.Entity;
using System.Linq;

namespace FOCA.Database.Controllers
{
    public class ComputerDomainController : BaseController<ComputerDomainsItem>
    {
        public ThreadSafeList<ComputerDomainsItem> GetComputerDomainByIdProject(int idProject)
        {
            using (FocaContextDb context = new FocaContextDb())
            {
                var result = context.ComputerDomain.Where(x => x.IdProject == idProject).Include("Computer").Include("Domain");

                return new ThreadSafeList<ComputerDomainsItem>(result);
            }
        }
    }
}
