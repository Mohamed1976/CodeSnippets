using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using ASPNETCoreWebApp.Data;

//You can for example publish website to directory and start it using dotnet   
//C:\temp\TempWebsite>dotnet ASPNETCoreWebApp.dll

namespace ASPNETCoreWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            
            //Run seeders
            CreateDbIfNotExists(host);
            SeedDutchTraitDb(host);

            host.Run();
            //CreateHostBuilder(args).Build().Run();
        }

        //DutchSeeder contains Scoped dependencies
        //AddDbContext adds the context as scoped
        private static void SeedDutchTraitDb(IHost host)
        {
            var scopeFactory = host.Services.GetService<IServiceScopeFactory>();
            using(var scope = scopeFactory.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetService<DutchSeeder>();
                seeder.Seed();
            }
        }

        private static void CreateDbIfNotExists(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<StudentDataContext>();
                    //context.Database.EnsureCreated();
                    DbInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred creating the DB.");
                }
            }
        }

        //CreateDefaultBuilder is setting up a default configuration file
        //You can build your configuration manually
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(SetupConfiguration)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        //ASP NET legacy used webconfig xml based file
        //ASP NEt core is very flexable, JSON, XML, INI, flat files that need parsing, DB, Enviroment variables etc
        private static void SetupConfiguration(HostBuilderContext ctx, IConfigurationBuilder builder)
        {
            //Remove the default configuration options 
            builder.Sources.Clear();
            //Load all the configurations in one store, we can use to lookup values
            //If there is a conflict, the order below is treated as a hierarchie
            //Hierarchie allows you to set variables in development that can be overriden in Production  
            builder.AddJsonFile("appsettings.json", false, true)    //Least trusted in hierarchie
                .AddXmlFile("appsettings.xml", true)                //..
                .AddEnvironmentVariables();                         //Most trusted in hierarchie
        }

        //Original code
        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup>();
        //        });
    }
}
