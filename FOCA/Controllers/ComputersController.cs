using System.Data.Entity;
using System.Linq;
using FOCA.ModifiedComponents;

namespace FOCA.Controllers
{
    public class ComputersController : BaseController
    {
        //Save
        //AddNew
        //Update
        public void Save(ThreadSafeList<ComputersItem> items)
        {
            if (items.Count == 0)
                return;

            foreach (var computerItem in items)
            {
                if (computerItem.Id == 0)
                    AddNew(computerItem);
                else
                    Update(computerItem);
            }

            CurrentContextDb.SaveChanges();
        }

        private static void Update(ComputersItem item)
        {
            var computer = CurrentContextDb.Computers.FirstOrDefault(x => x.Id == item.Id);

            if (computer != null)
            {
                computer.Description = item.Description;
                computer.NotOS = item.NotOS;
                computer.localName = item.localName;
                computer.name = item.name;
            }
        }

        private static void AddNew(ComputersItem item)
        {
            item.IdProject = Program.data.Project.Id;
            CurrentContextDb.Computers.Add(item);
        }

        public ThreadSafeList<ComputersItem> GetComputersByIdProject(int idProject)
        {
            var result = CurrentContextDb.Computers.Where(x => x.IdProject == idProject).Include("Printers").
                Include("RemoteFolders").
                Include("RemotePasswords").
                Include("RemotePrinters").
                Include("RemoteUsers").
                Include("Software").
                Include("Users");

            var items = new ThreadSafeList<ComputersItem>(result);

            return items;
        }
    }
}
