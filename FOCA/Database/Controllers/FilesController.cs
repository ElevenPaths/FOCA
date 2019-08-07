using FOCA.Database.Entities;
using FOCA.ModifiedComponents;
using System;
using System.Data.Entity;
using System.Linq;

namespace FOCA.Database.Controllers
{
    public class FilesController : BaseController
    {
        public void Save(ThreadSafeList<FilesItem> items)
        {
            try
            {
                if (items.Count == 0)
                    return;

                foreach (var fileItem in items)
                {
                    if (fileItem.Id == 0)
                        AddNew(fileItem);
                    else
                        Update(fileItem);
                }

                CurrentContextDb.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void Update(FilesItem item)
        {
            var file = CurrentContextDb.Files.FirstOrDefault(x => x.Id == item.Id);

            if (file != null)
            {
                file.Date = item.Date;
                file.Downloaded = item.Downloaded;
                file.Ext = item.Ext;
                file.Metadata = item.Metadata;
                file.ModifiedDate = item.ModifiedDate;
                file.Path = item.Path;
                file.Processed = item.Processed;
                file.Size = item.Size;
                file.URL = item.URL;
            }
        }

        private static void AddNew(FilesItem item)
        {
            item.Date = DateTime.Now;
            item.ModifiedDate = DateTime.Now;
            item.IdProject = Program.data.Project.Id;
            CurrentContextDb.Files.Add(item);
        }

        public ThreadSafeList<FilesItem> GetFilesByIdProject(int idProject)
        {
            var result = CurrentContextDb.Files.Where(x => x.IdProject == idProject)
                .Include("Metadata")
                .Include("Metadata.FoundPaths")
                .Include("Metadata.FoundUsers")
                .Include("Metadata.FoundEmails")
                .Include("Metadata.FoundDates")
                .Include("Metadata.FoundPrinters")
                .Include("Metadata.FoundPaths")
                .Include("Metadata.FoundOldVersions")
                .Include("Metadata.FoundHistory")
                .Include("Metadata.FoundMetaData")
                .Include("Metadata.FoundUsers")
                .Include("Metadata.FoundServers")
                .Include("Metadata.FoundPasswords");

            var items = new ThreadSafeList<FilesItem>(result);

            return items;
        }
    }
}
