using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using MiniDropbox.Domain;

namespace MiniDropbox.Data.AutoMappingOverride
{
    internal class AccountOverride : IAutoMappingOverride<Account>
    {
        public void Override(AutoMapping<Account> mapping)
        {
            /* mapping.HasMany(x => x.Referrals)
                 .Inverse()
                 .Access.CamelCaseField(Prefix.Underscore);*/
        }
    }
}
