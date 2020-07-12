using AspNetCoreWebApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreWebApi.Data
{
    //dotnet-ef migrations add initialCmdDB --context CommanderContext
    //dotnet-ef database update --context CommanderContext

    //The EF Core tools version '3.1.3' is older than that of the runtime '3.1.5'. Update the tools for the latest features and bug fixes.
    //dotnet tool update --global dotnet-ef
    //Tool 'dotnet-ef' was successfully updated from version '3.1.3' to version '3.1.5'.

    public class CommanderContext : DbContext
    {
        public CommanderContext(DbContextOptions<CommanderContext> opt) : base(opt)
        {

        }

        public DbSet<Command> Commands { get; set; }

    }
}
