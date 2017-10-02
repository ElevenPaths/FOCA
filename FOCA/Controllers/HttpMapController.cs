using System.Linq;
using FOCA.Analysis.HttpMap;
using FOCA.ModifiedComponents;

namespace FOCA.Controllers
{
    public class HttpMapController : BaseController
    {
        //Save
        //AddNew
        //Update
        public void Save(HttpMap item)
        {
            var listBackups =
                item.Backups.Select(x => new HttpMapTypesFiles() {IdHttMap = item.Id, IdType = 1, Value = x.Url}).ToList();

            var listDocuments = item.Documents.Select(x => new HttpMapTypesFiles() {IdHttMap = item.Id ,IdType = 2, Value = x.ToString()}).ToList();
            var listFiles = item.Files.Select(x => new HttpMapTypesFiles() { IdHttMap = item.Id, IdType = 3, Value = x.ToString() }).ToList();
            var listFolders = item.Folders.Select(x => new HttpMapTypesFiles() { IdHttMap = item.Id, IdType = 4, Value = x.ToString() }).ToList();
            var listParametrized = item.Parametrized.Select(x => new HttpMapTypesFiles() { IdHttMap = item.Id, IdType = 5, Value = x.ToString() }).ToList();
            
            CurrentContextDb.HttpMapTypesFiles.AddRange(listBackups);
            CurrentContextDb.HttpMapTypesFiles.AddRange(listDocuments);
            CurrentContextDb.HttpMapTypesFiles.AddRange(listFiles);
            CurrentContextDb.HttpMapTypesFiles.AddRange(listFolders);
            CurrentContextDb.HttpMapTypesFiles.AddRange(listParametrized);

            CurrentContextDb.SaveChanges();
        }

        public ThreadSafeList<string> GetItemsByTypeById(int i, int id)
        {
            var result = CurrentContextDb.HttpMapTypesFiles.Where(x => x.IdHttMap == id && x.IdType == i).ToList();

            var items = new ThreadSafeList<string>(result.Select(x=>x.Value));

            return items;
        }
    }
}
