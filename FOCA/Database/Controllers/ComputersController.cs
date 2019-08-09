using FOCA.Database.Entities;
using FOCA.ModifiedComponents;
using System.Data.Entity;
using System.Linq;

namespace FOCA.Database.Controllers
{
    public class ComputersController : BaseController<ComputersItem>
    {
        public ThreadSafeList<ComputersItem> GetComputersByIdProject(int idProject)
        {
            using (FocaContextDb context = new FocaContextDb())
            {
                var result = context.Computers.Where(x => x.IdProject == idProject).
                Include("Description").
                Include("Description.Items").
                Include("Printers").
                Include("Printers.Items").
                Include("Printers.Items.RemoteUsers").
                Include("Printers.Items.RemoteUsers.Items").
                Include("RemoteFolders").
                Include("RemoteFolders.Items").
                Include("RemoteFolders.Items.RemoteUsers").
                Include("RemoteFolders.Items.RemoteUsers.Items").
                Include("RemotePasswords").
                Include("RemotePasswords.Items").
                Include("RemotePrinters").
                Include("RemotePrinters.Items").
                Include("RemotePrinters.Items.RemoteUsers").
                Include("RemotePrinters.Items.RemoteUsers.Items").
                Include("RemoteUsers").
                Include("RemoteUsers.Items").
                Include("Software").
                Include("Software.Items").
                Include("Users").
                Include("Users.Items");

                return new ThreadSafeList<ComputersItem>(result);
            }
        }
    }
}
