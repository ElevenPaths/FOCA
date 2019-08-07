using FOCA.Database.Entities;
using FOCA.ModifiedComponents;
using System;
using System.Data.Entity;
using System.Linq;

namespace FOCA.Database.Controllers
{
    public class ComputerIpsController : BaseController
    {
        public void Save(ThreadSafeList<ComputerIPsItem> items)
        {
            if (items.Count == 0)
                return;

            foreach (var computerIpItem in items)
            {
                if (computerIpItem.Id == 0)
                    AddNew(computerIpItem);
                else
                    Update(computerIpItem);
            }


            CurrentContextDb.SaveChanges();
        }

        private static void Update(ComputerIPsItem item)
        {
            try
            {
                var computerIp = CurrentContextDb.ComputerIps.FirstOrDefault(x => x.Id == item.Id);

                if (computerIp != null)
                {
                    computerIp.Computer = item.Computer;
                    computerIp.Ip = item.Ip;
                    computerIp.Source = item.Source;
                }
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        private static void AddNew(ComputerIPsItem item)
        {
            item.IdProject = Program.data.Project.Id;
            CurrentContextDb.ComputerIps.Add(item);
        }

        public ThreadSafeList<ComputerIPsItem> GetComputerIpsByIdProject(int idProject)
        {
            var result =
                CurrentContextDb.ComputerIps.Where(x => x.IdProject == idProject).Include("Computer").Include("Ip");

            var items = new ThreadSafeList<ComputerIPsItem>(result);

            return items;
        }
    }
}
