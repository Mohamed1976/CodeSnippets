using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DataLibrary.CarDealerships.Data
{
    public class CarDealershipsContextFactory : IDesignTimeDbContextFactory<CarDealershipsContext>
    {
        public CarDealershipsContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CarDealershipsContext>();
            //var connectionString = @"server=.,5433;Database=AutoLotWorkShop;User Id=sa;Password=P@ssw0rd;";
            var connectionString = "Server=(local);Database=CarDealerships;Integrated Security=true;";
            optionsBuilder.UseSqlServer(connectionString);
            Console.WriteLine(connectionString);
            return new CarDealershipsContext(optionsBuilder.Options);
        }
    }
}
