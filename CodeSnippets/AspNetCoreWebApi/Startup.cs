using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using AspNetCoreWebApi.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using Newtonsoft.Json.Serialization;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using DataLibrary.BankDemo.Data;
using DataLibrary.Repository.Interfaces;
using DataLibrary.Repository;
using DataLibrary.MusicStore.Data;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.OData.Edm;
using Microsoft.AspNet.OData.Builder;
using DataLibrary.MusicStore.Models;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.Net.Http.Headers;

namespace AspNetCoreWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //OutputCache for ASP Net Core
            //WebEssentials.AspNetCore.OutputCaching
            //services.AddOutputCaching();

            //Used for OData, see details in AlbumsController 
            //services.AddMvc(option => option.EnableEndpointRouting = false);
            //services.AddMvcCore(op =>
            //services.AddMvc(op =>
            //{
            //    op.EnableEndpointRouting = false;

            //    foreach (var formatter in op.OutputFormatters
            //        .OfType<ODataOutputFormatter>()
            //        .Where(it => !it.SupportedMediaTypes.Any()))
            //    {
            //        formatter.SupportedMediaTypes.Add(
            //            new MediaTypeHeaderValue("application/prs.mock-odata"));
            //    }
            //    foreach (var formatter in op.InputFormatters
            //        .OfType<ODataInputFormatter>()
            //        .Where(it => !it.SupportedMediaTypes.Any()))
            //    {
            //        formatter.SupportedMediaTypes.Add(
            //            new MediaTypeHeaderValue("application/prs.mock-odata"));
            //    }

            //    //foreach (var outputFormatter in options.OutputFormatters.OfType<ODataOutputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
            //    //{
            //    //    outputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
            //    //}
            //    //foreach (var inputFormatter in options.InputFormatters.OfType<ODataInputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
            //    //{
            //    //    inputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
            //    //}
            //});

            /* Add the BankContext */
            //services.AddDbContext<BankContext>(options =>
            //    options.UseSqlServer(
            //        Configuration.GetConnectionString("BankDBConnection")));

            string connectionString = Configuration.GetConnectionString("BankDBConnection");

            //We can use dbcontext-pooling, has some advantages as can be read here 
            //https://docs.microsoft.com/en-us/ef/core/what-is-new/ef-core-2.0#dbcontext-pooling
            //https://stackoverflow.com/questions/48443567/adddbcontext-or-adddbcontextpool
            services.AddDbContextPool<BankContext>(
                options => options.UseSqlServer(connectionString, so => so.EnableRetryOnFailure()));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            //Created repository will be injected with BankContext, 
            //BankContext is registered as a dependency in the service container. 
            services.AddScoped<ICustomerRepo, CustomerRepo>();

            //Retrieve ConnectionString using secret manager tool
            //https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-2.2&tabs=windows
            //string ConnectionStringCmdDB = Configuration["CommanderConnection:ConnectionString"];
            services.AddDbContext<CommanderContext>(options =>
                options.UseSqlServer(
                    Configuration["CommanderConnection:ConnectionString"]));

            //For a quick run, I used in-memory database provided by EF Core.
            //dotnet add package Microsoft.EntityFrameworkCore.InMemory
            //Register context to DI interface.
            services.AddDbContext<MusicContext>(opts => opts.UseInMemoryDatabase("AlbumsDB"));

            //Used for OData, see details in AlbumsController 
            //Register OData to DI interface.
            //services.AddOData();

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            //https://dotnetcoretutorials.com/2017/11/29/json-patch-asp-net-core/
            //services.AddControllersWithViews();
            services.AddControllersWithViews().AddNewtonsoftJson(s => {
                s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            services.AddRazorPages();

            //http://www.binaryintellect.net/articles/a7d9edfd-1f86-45f8-a668-64cc86d8e248.aspx
            //In-memory caching needs to enabled in the Startup class
            services.AddMemoryCache();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSingleton<ICommanderRepo, MockCommanderRepo>();

            //Add Swagger to display all endpoints in Web API
            //We need to add the following packages, info can be found here:
            //https://www.youtube.com/watch?v=9QU_y7-VsC8&t
            //Swashbuckle.AspNetCore
            //Swashbuckle.Core
            //Swashbuckle.AspNetCore.Swagger
            //Swashbuckle.AspNetCore.SwaggerGen
            //Swashbuckle.AspNetCore.SwaggerUI
            //Swashbuckle.AspNetCore.Annotations
            services.AddSwaggerGen(setup =>
            {
                setup.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Title = "Web API tutorial",
                        Version= "v1"
                    });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //OutputCache for ASP Net Core
            //WebEssentials.AspNetCore.OutputCaching
            //app.UseOutputCaching();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //Used for OData, see details in AlbumsController
            //Because of adding (app.UseMvc(routeBuilder), I needed to add:
            //services.AddMvc(option => option.EnableEndpointRouting = false);
            //https://docs.microsoft.com/en-us/aspnet/core/migration/22-to-30?view=aspnetcore-3.1&tabs=visual-studio#use-mvc-without-endpoint-routing
            //https://stackoverflow.com/questions/57684093/using-usemvc-to-configure-mvc-is-not-supported-while-using-endpoint-routing
            //app.UseMvc(routeBuilder =>
            //{
            //    routeBuilder.Select().Expand().Count().Filter().OrderBy().MaxTop(100).SkipToken().Build();
            //    routeBuilder.MapODataServiceRoute("odata", "odata", GetEdmModel());
            //});

            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "Web API tutorial");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }

        //Used for OData, see details in AlbumsController
        private static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Album>("Albums");
            builder.EntitySet<Song>("Songs");
            return builder.GetEdmModel();
        }
    }
}
