using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainDrivenDatabaseDeployer;
using FizzWare.NBuilder;
using MiniDropbox.Domain;
using NHibernate;

namespace MiniDropbox.DatabaseDeployer
{
    public class RoleSeeder : IDataSeeder
    {
        private readonly ISession _session;

        public RoleSeeder(ISession session)
        {
            _session = session;
        }

        public void Seed()
        {
            IList<Role> roleList = Builder<Role>.CreateListOfSize(2).Build();
           
            roleList.Add(new Role{Name = "Admin"});
            roleList.Add(new Role{Name = "User"});

        }
    }
}
