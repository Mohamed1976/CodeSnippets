using ASPNETCoreWebApp.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/*
You define the configuration using profiles. And then you let AutoMapper know in what 
assemblies are those profiles defined by calling the IServiceCollection extension method 
AddAutoMapper at startup:

services.AddAutoMapper(Assembly.GetExecutingAssembly());

https://docs.automapper.org/en/stable/Dependency-injection.html
*/
namespace ASPNETCoreWebApp.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterViewModel, ApplicationUser>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));
        }
    }
}
