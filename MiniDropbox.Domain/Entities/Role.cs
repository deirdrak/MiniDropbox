namespace MiniDropbox.Domain
{
    public class Role : IEntity
    {
        public virtual long Id { get; set; }
        public virtual bool IsArchived { get; set; }

        public virtual string Name { get; set; }
    }
}
