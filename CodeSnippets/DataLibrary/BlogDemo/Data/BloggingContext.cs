using DataLibrary.BlogDemo.Helpers;
using DataLibrary.BlogDemo.Interceptors;
using DataLibrary.BlogDemo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;

namespace DataLibrary.BlogDemo.Data
{
    //https://docs.microsoft.com/en-us/ef/core/modeling/backing-field
    public class BloggingContext : DbContext
    {
        public BloggingContext()
        {
            ChangeTracker.StateChanged += ChangeTracker_StateChanged;
            ChangeTracker.Tracked += ChangeTracker_Tracked;
        }

        public BloggingContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Blog> Blogs { get; set; }

        /*Interceptors are great for edge cases, that need custom behavior, that is not supported by 
         * the EF Core provider. For example, you might need to call a stored procedure, that returns 
         * a result set and an out parameter, but the database provider needs the DbCommand to specify 
         * CommandType.StoredProcedure instead of CommandType.Text to support that. EF Core does not 
         * provide a direct way to set the CommandType of a DbCommand, so you can use a command 
         * interceptor in this case to do so yourself. Another good reasons is, to ensure that specific 
         * preconditions are setup for a command. For example, you might need to ensure, that a certain 
         * time zone (or other database option) is always set just before command execution. You might 
         * also need to alter the generated SQL statements for some other reasons, before they hit the 
         * database. For example, a provider might not support something, that you need to use in your 
         * SQL query, or might contain a bug, that results in a mistranslation which you are able to 
         * fix this way. There are also scenarios for test suites, where interceptors can become handy. */
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = @"Server=(local);Database=Demo.Blog;Integrated Security=true;";

            optionsBuilder.UseSqlServer(connectionString);

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connectionString)
                    .AddInterceptors(new List<IInterceptor>
                    {
                        new CommandInterceptor(),
                        new ConnectionInterceptor(),
                        new TransactionInterceptor()
                    });
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>()
                .Property(b => b.Url)
                .HasField("_url");
        }

        //Not often used, only in some edge cases
        //ChangeTracker.Tracked event is fired when object goes from being tracked to not being 
        //tracked and vice versa. When object is added to the ChangeTracker 
        private void ChangeTracker_Tracked(object sender, 
            Microsoft.EntityFrameworkCore.ChangeTracking.EntityTrackedEventArgs e)
        {
            var source = (e.FromQuery) ? "Database" : "Code";
            if (e.Entry.Entity is Blog b)
            {
                Console.WriteLine($"Blog entry {b.Name} was added from {source}");
            }
        }

        private void ChangeTracker_StateChanged(object sender, 
            Microsoft.EntityFrameworkCore.ChangeTracking.EntityStateChangedEventArgs e)
        {
            if (e.Entry.Entity is Blog b)
            {
                var action = string.Empty;
                Console.WriteLine($"Blog {b.Name} was {e.OldState} before the state changed to {e.NewState}");
                //Note you can use the value to update an audit table.  
                Console.WriteLine("{0}", b);
                switch (e.NewState)
                {
                    case EntityState.Added:
                    case EntityState.Deleted:
                    case EntityState.Modified:
                        break;
                    case EntityState.Unchanged: //It means it has been persisted
                        switch (e.OldState)     //You are going to log data after it has been modified
                        {
                            case EntityState.Added:
                                action = "Added";
                                break;
                            case EntityState.Deleted:
                                action = "Deleted";
                                break;
                            case EntityState.Modified:
                                action = "Edited";
                                break;
                        }
                        break;
                }
            }
        }
    }
}
