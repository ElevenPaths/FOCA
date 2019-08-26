using FOCA.Database.Entities;
using FOCA.ModifiedComponents;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace FOCA.Database.Controllers
{
    public class FilesController : BaseController<FilesItem>
    {
        public override void Save(IList<FilesItem> items)
        {
            try
            {
                if (items.Count == 0)
                    return;

                using (FocaContextDb context = new FocaContextDb())
                {
                    foreach (var fileItem in items)
                    {
                        fileItem.Date = DateTime.Now;
                        fileItem.ModifiedDate = DateTime.Now;
                        context.Files.AddOrUpdate(fileItem);
                    }

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ThreadSafeList<FilesItem> GetFilesByIdProject(int idProject)
        {
            using (FocaContextDb context = new FocaContextDb())
            {
                var result = context.Files.Where(x => x.IdProject == idProject)
                .Include("Metadata")
                .Include("Metadata.FoundUsers")
                .Include("Metadata.FoundUsers.Items")
                .Include("Metadata.FoundEmails")
                .Include("Metadata.FoundEmails.Items")
                .Include("Metadata.FoundDates")
                .Include("Metadata.FoundPrinters")
                .Include("Metadata.FoundPrinters.Items")
                .Include("Metadata.FoundPaths")
                .Include("Metadata.FoundPaths.Items")
                .Include("Metadata.FoundOldVersions")
                .Include("Metadata.FoundOldVersions.Items")
                .Include("Metadata.FoundHistory")
                .Include("Metadata.FoundHistory.Items")
                .Include("Metadata.FoundMetaData")
                .Include("Metadata.FoundMetaData.Applications")
                .Include("Metadata.FoundMetaData.Applications.Items")
                .Include("Metadata.FoundServers")
                .Include("Metadata.FoundServers.Items")
                .Include("Metadata.FoundPasswords")
                .Include("Metadata.FoundPasswords.Items");

                return new ThreadSafeList<FilesItem>(result);
            }
        }
    }
}
