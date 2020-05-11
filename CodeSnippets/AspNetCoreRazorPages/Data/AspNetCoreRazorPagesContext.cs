using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AspNetCoreRazorPages.Models;

namespace AspNetCoreRazorPages.Data
{
    public class AspNetCoreRazorPagesContext : DbContext
    {
        public AspNetCoreRazorPagesContext (DbContextOptions<AspNetCoreRazorPagesContext> options)
            : base(options)
        {
        }

        public DbSet<AspNetCoreRazorPages.Models.Movie> Movie { get; set; }
    }
}
