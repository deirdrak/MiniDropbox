using System.Collections.Generic;
using AutoMapper;
using MiniDropbox.Domain;
using MiniDropbox.Web.Models;
using Ninject.Modules;

namespace MiniDropbox.Web.Infrastructure
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.CreateMap<AccountSignUpModel, Account>();
            Mapper.CreateMap<Account, AccountSignUpModel>();
            
        }
    }
}