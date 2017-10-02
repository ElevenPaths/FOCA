using System.Data.Entity.Validation;
using System.Linq;
using FOCA.ModifiedComponents;

namespace FOCA.Controllers
{
    public class LimitsController : BaseController
    {
        public void Save(ThreadSafeList<Limits> items)
        {
            if (items.Count == 0)
                return;

            foreach (var limitItem in items)
            {
                if (limitItem.Id == 0)
                    AddNew(limitItem);
                else
                    Update(limitItem);
            }

            CurrentContextDb.SaveChanges();
        }

        private static void Update(Limits item)
        {
            var limit = CurrentContextDb.Limits.FirstOrDefault(x => x.Id == item.Id);

            if (limit != null)
            {
                limit.Higher = item.Higher;
                limit.Lower = item.Lower;
                limit.Range = item.Range;
            }
        }

        private static void AddNew(Limits item)
        {
            try
            {
                CurrentContextDb.Limits.Add(item);
            }
            catch (DbEntityValidationException ex)
            {
                throw new DbEntityValidationException(ex.Message);
            }
        }

        public ThreadSafeList<Limits> GetLimitsByIdProject(int idProject)
        {
            var result = CurrentContextDb.Limits.Where(x => x.IdProject == idProject);

            var items = new ThreadSafeList<Limits>(result);

            return items;
        }
    }
}
