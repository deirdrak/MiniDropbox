using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiniDropbox.Web.Models
{
    public class RegisteredUsersListModel
        {
            public RegisteredUsersListModel()
            {
                
            }

            public RegisteredUsersListModel(long id, bool isArchived, string name, string lastName, string eMail, bool blocked, int spaceLimit)
            {
                SpaceLimit = spaceLimit;
                Blocked = blocked;
                EMail = eMail;
                LastName = lastName;
                Name = name;
                Archived = isArchived;
                Id = id;
            }
            
            public long Id { get; private set; }
            public string Name { get; private set; }
            public string LastName { get; private set; }
            public string EMail { get; private set; }
            public int SpaceLimit { get; private set; }
            public bool Archived { get; private set; }
            public bool Blocked { get; private set; }
        }
}