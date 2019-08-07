namespace FOCA.Database.Controllers
{
    public abstract class BaseController
    {
        internal static FocaContextDb FocaContextDbValue;

        public static FocaContextDb CurrentContextDb
        {
            get { return FocaContextDbValue ?? (FocaContextDbValue = new FocaContextDb()); }
        }

        public static void DisposeContext()
        {
            FocaContextDbValue = new FocaContextDb();
        }
    }
}