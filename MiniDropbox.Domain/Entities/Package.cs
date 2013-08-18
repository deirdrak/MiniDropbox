using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniDropbox.Domain
{
    public class Package:IEntity
    {
            public virtual long Id { get; set; }
            public virtual string Name { get; set; }
            public virtual string Description { get; set; }
            public virtual double Price { get; set; }
            public virtual int SpaceLimit { get; set; }
            public virtual int DaysAvailable { get; set; }
            public virtual DateTime CreationDate { get; set; }
            public virtual bool IsArchived { get; set; }
    }
}
