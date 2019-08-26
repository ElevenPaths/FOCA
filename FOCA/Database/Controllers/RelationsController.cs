using FOCA.Database.Entities;
using FOCA.ModifiedComponents;
using System.Data.Entity;
using System.Linq;

namespace FOCA.Database.Controllers
{
    public class RelationsController : BaseController<RelationsItem>
    {
        public ThreadSafeList<RelationsItem> GetRelationsByIdProject(int idProject)
        {
            using (FocaContextDb context = new FocaContextDb())
            {
                var result = context.Relations.Where(x => x.IdProject == idProject).Include("Domain").Include("Ip");
                return new ThreadSafeList<RelationsItem>(result);
            }
        }
    }
}
