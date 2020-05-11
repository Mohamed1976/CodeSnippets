using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using AspNetCoreRazorPages.Data;

namespace AspNetCoreRazorPages
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        //Strategies to modify Model and DataBase
        
        //1) Have the Entity Framework automatically drop and re-create the database using the new 
        //model class schema. This approach is convenient early in the development cycle; it 
        //allows you to quickly evolve the model and database schema together.The downside is 
        //that you lose existing data in the database. Don't use this approach on a production 
        //database! Dropping the DB on schema changes and using an initializer to automatically 
        //seed the database with test data is often a productive way to develop an app.

        //2) Explicitly modify the schema of the existing database so that it matches the model 
        //classes.The advantage of this approach is that you keep your data.You can make this 
        //change either manually or by creating a database change script.

        //3) Use Code First Migrations to update the database schema.

        //For the creation of the movie example I used the tutorial below
        //https://docs.microsoft.com/en-us/aspnet/core/tutorials/razor-pages/razor-pages-start?view=aspnetcore-3.1&tabs=visual-studio
        // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            services.AddDbContext<AspNetCoreRazorPagesContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("AspNetCoreRazorPagesContext")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
