using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ASPNETCoreWebApp.Models;

namespace ASPNETCoreWebApp.Data
{
    public class StudentDataContext : DbContext
    {
        public StudentDataContext (DbContextOptions<StudentDataContext> options)
            : base(options)
        {
        }

        public DbSet<ASPNETCoreWebApp.Models.Student> Students { get; set; }
    }
}
