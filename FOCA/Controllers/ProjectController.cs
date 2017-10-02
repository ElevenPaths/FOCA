using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using MetadataExtractCore.Diagrams;

namespace FOCA.Controllers
{
    public class ProjectController : BaseController
    {
        public void Save(Project item)
        {
            if (item.Id != 0)
                Update(item);
            else
                AddNew(item);

            CurrentContextDb.SaveChanges();
        }

        private static void Update(Project item)
        {
            var project = CurrentContextDb.Projects.FirstOrDefault(x => x.Id == item.Id);

            if (project != null)
            {
                project.AlternativeDomains = item.AlternativeDomains;
                project.Domain = item.Domain;
                project.FolderToDownload = item.FolderToDownload;
                project.ProjectDate = item.ProjectDate;
                project.ProjectNotes = item.ProjectNotes;
                project.ProjectName = item.ProjectName;
                project.ProjectState = item.ProjectState;
                project.ProjectSaveFile = item.ProjectSaveFile;
            }
        }

        private static void AddNew(Project item)
        {
            try
            {
                CurrentContextDb.Projects.Add(item);
            }
            catch (DbEntityValidationException ex)
            {
                throw new DbEntityValidationException(ex.Message);
            }
        }

        public Project GetProjectById(int idProject)
        {
            try
            {
                var result = CurrentContextDb.Projects.FirstOrDefault(x => x.Id == idProject);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Project> GetAllProjects()
        {
            var result = CurrentContextDb.Projects.ToList();

            return result;
        }
    }
}
