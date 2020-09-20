using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DataLibrary.BankDemo.Data
{
    public class BankContextFactory : IDesignTimeDbContextFactory<BankContext>
    {
        public BankContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BankContext>();
            //var connectionString = @"server=.,5433;Database=AutoLotWorkShop;User Id=sa;Password=P@ssw0rd;";
            var connectionString = "Server=(local);Database=BankDemo;Integrated Security=true;";
            optionsBuilder.UseSqlServer(connectionString);
            Console.WriteLine(connectionString);
            return new BankContext(optionsBuilder.Options);
        }
    }
}
