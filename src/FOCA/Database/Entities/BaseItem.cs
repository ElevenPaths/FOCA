namespace FOCA.Database.Entities
{
    public abstract class BaseItem
    {
        public virtual int IdProject { get; set; }

        public BaseItem()
        {
            IdProject = Program.data.Project.Id;
        }
    }
}
