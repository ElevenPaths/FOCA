using FOCA.Database.Entities;
using FOCA.ModifiedComponents;
using System.Linq;

namespace FOCA.Database.Controllers
{
    public class IpsController : BaseController<IPsItem>
    {
        public ThreadSafeList<IPsItem> GetIpsByIdProject(int idProject)
        {
            using (FocaContextDb context = new FocaContextDb())
            {
                var result = context.Ips.Where(x => x.IdProject == idProject);

                return new ThreadSafeList<IPsItem>(result);
            }
        }
    }
}
