using FOCA.Database.Entities;
using FOCA.ModifiedComponents;
using System.Linq;

namespace FOCA.Database.Controllers
{
    public class LimitsController : BaseController<Limits>
    {
        public ThreadSafeList<Limits> GetLimitsByIdProject(int idProject)
        {
            using (FocaContextDb context = new FocaContextDb())
            {
                var result = context.Limits.Where(x => x.IdProject == idProject);

                return new ThreadSafeList<Limits>(result);
            }
        }
    }
}
