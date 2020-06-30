using ASPNETCoreWebApp.Configuration;
using ASPNETCoreWebApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/*
Install NuGet packages: 
Microsoft.AspNetCore.Identity
Microsoft.AspNetCore.Identity.EntityFrameworkCore;

Use Package Manager Console to add migration and update database 
Tools->Nuget Package Manager->Package Manager Console

PM> add-migration CreateIdentityMigration -context AppIdentityDbContext
Build started...
Build succeeded.
To undo this action, use Remove-Migration.

PM> Update-Database -Migration CreateIdentityMigration -context AppIdentityDbContext
Build started...
Build succeeded.
Done.

https://www.entityframeworktutorial.net/efcore/pmc-commands-for-ef-core-migration.aspx#update-database
https://www.entityframeworktutorial.net/efcore/entity-framework-core-migration.aspx
*/

/*
After adding roles to Identity

PM> add-migration InsertedRoles -context AppIdentityDbContext
Build started...
Build succeeded.
To undo this action, use Remove-Migration.
  
PM> Update-Database InsertedRoles -context AppIdentityDbContext
Build started...
Build succeeded.
Done.  
*/
namespace ASPNETCoreWebApp.Data
{
    public class AppIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new RoleConfiguration());
        }
    }
}
