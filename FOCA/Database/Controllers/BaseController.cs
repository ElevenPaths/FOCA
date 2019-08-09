using System.Collections.Generic;
using System.Data.Entity.Migrations;

namespace FOCA.Database.Controllers
{
    public abstract class BaseController<T> where T : class
    {
        public virtual void Save(IList<T> items)
        {
            if (items.Count == 0)
                return;

            using (FocaContextDb context = new FocaContextDb())
            {
                foreach (var item in items)
                {
                    context.Set<T>().AddOrUpdate(item);
                }

                context.SaveChanges();
            }
        }
    }
}