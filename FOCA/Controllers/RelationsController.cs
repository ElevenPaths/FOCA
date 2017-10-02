using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using FOCA.ModifiedComponents;

namespace FOCA.Controllers
{
    public class RelationsController : BaseController
    {
        public void Save(ThreadSafeList<RelationsItem> items)
        {
            if (items.Count == 0)
                return;

            foreach (var relationItem in items)
            {
                if (relationItem.Id == 0)
                    AddNew(relationItem);
                else
                    Update(relationItem);
            }

            CurrentContextDb.SaveChanges();
        }

        private static void Update(RelationsItem item)
        {
            var relation = CurrentContextDb.Relations.FirstOrDefault(x => x.Id == item.Id);

            if (relation != null)
            {
                relation.Domain = item.Domain;
                relation.Ip = item.Ip;
                relation.Source = item.Source;
            }
        }

        private static void AddNew(RelationsItem item)
        {
            try
            {
                item.IdProject = Program.data.Project.Id;
                item.Ip.IdProject = Program.data.Project.Id;
                CurrentContextDb.Relations.Add(item);
            }
            catch (DbEntityValidationException ex)
            {
                throw new DbEntityValidationException(ex.Message);
            }
        }

        public ThreadSafeList<RelationsItem> GetReltationsByIdProject(int idProject)
        {
            var result = CurrentContextDb.Relations.Where(x => x.IdProject == idProject).Include("Domain").Include("Ip");

            var items = new ThreadSafeList<RelationsItem>(result);

            return items;
        }
    }
}
