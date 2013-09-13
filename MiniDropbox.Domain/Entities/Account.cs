using System.Collections.Generic;

namespace MiniDropbox.Domain
{
    public class Account : IEntity
    {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string LastName { get; set; }
        public virtual string EMail { get; set; }
        public virtual string Password { get; set; }
        public virtual int SpaceLimit { get; set; }
        public virtual int UsedSpace { get; set; }
        public virtual bool IsArchived { get; set; }
        public virtual bool IsBlocked { get; set; }
        public virtual bool IsAdmin { get; set; }
        public virtual string BucketName { get; set; }

        public virtual IList<File> Files { get; set; }
        public virtual IList<Account> Referrals { get; set; }
      
    }
}