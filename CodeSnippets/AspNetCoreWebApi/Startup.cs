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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            //Retrieve ConnectionString using secret manager tool
            //https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-2.2&tabs=windows
            //string ConnectionStringCmdDB = Configuration["CommanderConnection:ConnectionString"];
            services.AddDbContext<CommanderContext>(options =>
                options.UseSqlServer(
                    Configuration["CommanderConnection:ConnectionString"]));

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
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

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
    }
}
