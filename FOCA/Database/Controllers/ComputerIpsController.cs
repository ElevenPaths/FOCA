using FOCA.Database.Entities;
using FOCA.ModifiedComponents;
using System.Data.Entity;
using System.Linq;

namespace FOCA.Database.Controllers
{
    public class ComputerIpsController : BaseController<ComputerIPsItem>
    {
        public ThreadSafeList<ComputerIPsItem> GetComputerIpsByIdProject(int idProject)
        {
            using (FocaContextDb context = new FocaContextDb())
            {
                var result = context.ComputerIps.Where(x => x.IdProject == idProject).Include("Computer").Include("Ip");
                return new ThreadSafeList<ComputerIPsItem>(result);
            }
        }
    }
}
