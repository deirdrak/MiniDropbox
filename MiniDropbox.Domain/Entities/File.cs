using System;

namespace MiniDropbox.Domain
{
    public class File : IEntity
    {
        public virtual long Id { get; set; }
        public virtual bool IsArchived { get; set; }

        public virtual string Url { get; set; }

        public virtual DateTime CreatedDate { get; set; }
    }
}