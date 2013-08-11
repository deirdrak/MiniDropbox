using System;
using AcklenAvenue.Data;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Conventions.Helpers;
using MiniDropbox.Domain;

namespace MiniDropbox.Data
{
    public class MappingScheme : IDatabaseMappingScheme<MappingConfiguration>
    {
        public Action<MappingConfiguration> Mappings
        {
            get
            {
                AutoPersistenceModel autoPersistenceModel = AutoMap.Assemblies(typeof (IEntity).Assembly)
                                                                   .Where(t => typeof (IEntity).IsAssignableFrom(t))
                    // .UseOverridesFromAssemblyOf<AccountOverride>()
                                                                   .Conventions.Add(DefaultCascade.All());

                return x => x.AutoMappings.Add(autoPersistenceModel);
            }
        }
    }
}