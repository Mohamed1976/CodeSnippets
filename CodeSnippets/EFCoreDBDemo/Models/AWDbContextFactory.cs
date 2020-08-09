using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

//https://github.com/skimedic/presentations/blob/ecf3d0ecd76fac1d9b837367332fdf1a45db8b28/DOTNETCORE/Channel9_EFCoreShows/EntityFrameworkCoreExamples/PerformanceEfCore/EfStructures/Aw2016DbContextFactory.cs
//https://medium.com/@speedforcerun/implementing-idesigntimedbcontextfactory-in-asp-net-core-2-0-2-1-3718bba6db84
//https://entityframeworkcore.com/knowledge-base/50502354/add-an-implementation-of--idesigntimedbcontextfactory-applicationdbcontext---to-the-project
//https://snede.net/you-dont-need-a-idesigntimedbcontextfactory/
namespace EFCoreDBDemo.Models
{
    /* https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dbcontext-creation
    https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.design.idesigntimedbcontextfactory-1?view=efcore-3.1
    Design-time DbContext Creation
    Some of the EF Core Tools commands (for example, the Migrations commands) require a derived DbContext 
    instance to be created at design time in order to gather details about the application's entity types 
    and how they map to a database schema. In most cases, it is desirable that the DbContext thereby created 
    is configured in a similar way to how it would be configured at run time.

    From a design-time factory
    You can also tell the tools how to create your DbContext by implementing the IDesignTimeDbContextFactory<TContext> 
    interface: If a class implementing this interface is found in either the same project as the derived DbContext or 
    in the application's startup project, the tools bypass the other ways of creating the DbContext and use the 
    design-time factory instead. */
    public class AWDbContextFactory : IDesignTimeDbContextFactory<AdventureWorksLT2017Context>
    {

        private string connectionString =
            "Server=(local);Database=AdventureWorksLT2017;Integrated Security=true;MultipleActiveResultSets=true;";

        public AdventureWorksLT2017Context CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AdventureWorksLT2017Context>();
            //optionsBuilder.UseSqlServer(connectionString);
            optionsBuilder.UseSqlServer(connectionString, options =>
            {
                options.EnableRetryOnFailure().CommandTimeout(100);
            })
            .EnableSensitiveDataLogging()
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            Console.WriteLine(connectionString);
            return new AdventureWorksLT2017Context(optionsBuilder.Options);
        }

        private string GetConnectionString(string dbName = default)
        {

            //var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            //IConfigurationRoot configuration = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json")
            //    .AddJsonFile($"appsettings.{environmentName}.json", true)
            //    .AddEnvironmentVariables();
            //    .Build();

            //var connectionString = configuration.GetConnectionString("YourDbContext");

            return connectionString;
        }

        //The appsettings.json file would look something like
        //{
        //    "ConnectionStrings": {
        //        "YourDbContext": "ConnectionStringGoesHere"
        //    }
        //}
    }
}
