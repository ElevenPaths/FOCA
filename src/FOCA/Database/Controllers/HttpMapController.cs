using FOCA.Analysis.HttpMap;
using FOCA.Database.Entities;
using FOCA.ModifiedComponents;
using System.Linq;

namespace FOCA.Database.Controllers
{
    public class HttpMapController : BaseController<HttpMap>
    {
        public void Save(HttpMap item)
        {
            using (FocaContextDb context = new FocaContextDb())
            {
                var listDocuments = item.Documents.Select(x => new HttpMapTypesFiles() { IdHttMap = item.Id, IdType = 2, Value = x.ToString() }).ToList();
                var listFiles = item.Files.Select(x => new HttpMapTypesFiles() { IdHttMap = item.Id, IdType = 3, Value = x.ToString() }).ToList();
                var listFolders = item.Folders.Select(x => new HttpMapTypesFiles() { IdHttMap = item.Id, IdType = 4, Value = x.ToString() }).ToList();
                var listParametrized = item.Parametrized.Select(x => new HttpMapTypesFiles() { IdHttMap = item.Id, IdType = 5, Value = x.ToString() }).ToList();

                context.HttpMapTypesFiles.AddRange(listDocuments);
                context.HttpMapTypesFiles.AddRange(listFiles);
                context.HttpMapTypesFiles.AddRange(listFolders);
                context.HttpMapTypesFiles.AddRange(listParametrized);

                context.SaveChanges();
            }
        }

        public ThreadSafeList<string> GetItemsByTypeById(int i, int id)
        {
            using (FocaContextDb context = new FocaContextDb())
            {
                var result = context.HttpMapTypesFiles.Where(x => x.IdHttMap == id && x.IdType == i).ToList();

                return new ThreadSafeList<string>(result.Select(x => x.Value)); ;
            }
        }
    }
}
