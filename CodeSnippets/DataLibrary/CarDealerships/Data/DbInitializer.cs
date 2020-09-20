using DataLibrary.CarDealerships.Models.Base;
using DataLibrary.CarDealerships.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq;

namespace DataLibrary.CarDealerships.Data
{
    public static class DbInitializer
    {
        public static void Initialize(CarDealershipsContext context)
        {
            try
            {
                Console.WriteLine("Started seeding the CarDealerships database...");
                ProcessInsert(context, context.Customers, Customers);
                ProcessInsert(context, context.Makes, Makes);
                ProcessInsert(context, context.Cars, Inventory);
                ProcessInsert(context, context.Orders, Orders);
                ProcessInsert(context, context.CreditRisks, CreditRisks);
                Console.WriteLine("Finished seeding the CarDealerships database...");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DBInitialize, Exception: {ex.Message} " +
                    $"InnerException: {ex?.InnerException.Message}");
                throw;
            }
        }

        private static void ProcessInsert<TEntity>(CarDealershipsContext context, 
            DbSet<TEntity> table, List<TEntity> records) where TEntity : BaseEntity
        {
            if (!table.Any())
            {
                Console.WriteLine($"Started seeding table {typeof(TEntity).FullName}...");
                IExecutionStrategy strategy = context.Database.CreateExecutionStrategy();
                strategy.Execute(() =>
                {
                    using (var transaction = context.Database.BeginTransaction())
                    { 
                        try
                        {
                            var metaData = context.Model.FindEntityType(typeof(TEntity).FullName);
                            context.Database.ExecuteSqlRaw(
                                $"SET IDENTITY_INSERT {metaData.GetSchema()}.{metaData.GetTableName()} ON");
                            table.AddRange(records);
                            context.SaveChanges();
                            context.Database.ExecuteSqlRaw(
                                $"SET IDENTITY_INSERT {metaData.GetSchema()}.{metaData.GetTableName()} OFF");
                            transaction.Commit();
                            Console.WriteLine($"Finished seeding table {typeof(TEntity).FullName}...");
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                        }
                    }
                });
            }
            else
            {
                Console.WriteLine($"Skipping seeding table {typeof(TEntity).FullName}...");
            }
        }

        //If you want to set Identity column in your objects you need to set IDENTITY_INSERT is set to On 
        //DBInitialize, Exception: An error occurred while updating the entries. See the inner exception 
        //for details. InnerException: Cannot insert explicit value for identity column in table 'Customers' 
        //when IDENTITY_INSERT is set to OFF.
        //https://stackoverflow.com/questions/1334012/cannot-insert-explicit-value-for-identity-column-in-table-table-when-identity
        public static List<Customer> Customers => new List<Customer>
        {
            new Customer {Id = 1, PersonalInformation = new Person { FirstName = "Dave", LastName = "Brenner"} },
            new Customer {Id = 2, PersonalInformation = new Person { FirstName = "Matt", LastName = "Walton" } },
            new Customer {Id = 3, PersonalInformation = new Person { FirstName = "Steve", LastName = "Hagen" } },
            new Customer {Id = 4, PersonalInformation = new Person { FirstName = "Pat", LastName = "Walton" } },
            new Customer {Id = 5, PersonalInformation = new Person { FirstName = "Bad", LastName = "Customer" } },
        };

        public static List<Make> Makes => new List<Make>
        {
            new Make {Id = 1, Name = "VW"},
            new Make {Id = 2, Name = "Ford"},
            new Make {Id = 3, Name = "Saab"},
            new Make {Id = 4, Name = "Yugo"},
            new Make {Id = 5, Name = "BMW"},
            new Make {Id = 6, Name = "Pinto"},
        };

        public static List<Car> Inventory => new List<Car>
        {
            new Car {Id = 1, MakeId = 1, Color = "Black", PetName = "Zippy"},
            new Car {Id = 2, MakeId = 2, Color = "Rust", PetName = "Rusty"},
            new Car {Id = 3, MakeId = 3, Color = "Black", PetName = "Mel"},
            new Car {Id = 4, MakeId = 4, Color = "Yellow", PetName = "Clunker"},
            new Car {Id = 5, MakeId = 5, Color = "Black", PetName = "Bimmer"},
            new Car {Id = 6, MakeId = 5, Color = "Green", PetName = "Hank"},
            new Car {Id = 7, MakeId = 5, Color = "Pink", PetName = "Pinky"},
            new Car {Id = 8, MakeId = 6, Color = "Black", PetName = "Pete"},
            new Car {Id = 9, MakeId = 4, Color = "Brown", PetName = "Brownie"},
        };

        public static List<Order> Orders => new List<Order>
        {
            new Order {Id = 1, CustomerId = 1, CarId = 5},
            new Order {Id = 2, CustomerId = 2, CarId = 1},
            new Order {Id = 3, CustomerId = 3, CarId = 4},
            new Order {Id = 4, CustomerId = 4, CarId = 7},
        };

        public static List<CreditRisk> CreditRisks => new List<CreditRisk>
        {
            new CreditRisk
            {
                Id = 1,
                CustomerId = Customers[4].Id,
                PersonalInformation = new Person
                {
                    FirstName = Customers[4].PersonalInformation.FirstName,
                    LastName = Customers[4].PersonalInformation.LastName
                }
            }
        };
    }
}

/*
https://github.com/skimedic/presentations/blob/master/DOTNETCORE/Channel9_EFCoreShows/EntityFrameworkCoreExamples/Migrations/Initialization/SampleDa
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Migrations.EFStructures;
using Migrations.Models.Base;

namespace Migrations.Initialization
{
    public static class SampleDataInitializer
    {
        public static void DropAndCreateDatabase(ApplicationDbContext context)
        {
            context.Database.EnsureDeleted();
            //This doesn't run the migrations, so SQL objects will be missing
            //DON'T USE THIS => context.Database.EnsureCreated();
            context.Database.Migrate();
        }

        public static void InitializeData(ApplicationDbContext context)
        {
            DropAndCreateDatabase(context);
            SeedData(context);
        }

        public static void ClearDatabase(ApplicationDbContext context)
        {
            context.Database.Migrate();
            ClearData(context);
            SeedData(context);
        }

        internal static void ClearData(ApplicationDbContext context)
        {
            var entities = new[] {"Order", "Customer", "Make", "Car", "CreditRisk"};
            foreach (var entityName in entities)
            {
                var entity = context.Model.FindEntityType($"AutoLotDal.Models.Entities.{entityName}");
                var tableName = entity.GetTableName();
                var schemaName = entity.GetSchema();
                context.Database.ExecuteSqlRaw($"DELETE FROM {schemaName}.{tableName}");
            }
        }

        internal static void ResetIdentity(ApplicationDbContext context)
        {
            foreach (var entity in context.Model.GetEntityTypes())
            {
                var tableName = entity.GetTableName();
                var schemaName = entity.GetSchema();
                context.Database.ExecuteSqlRaw($"DBCC CHECKIDENT (\"{schemaName}.{tableName}\", RESEED, 1);");
                //context.Database.ExecuteSqlRaw($"DBCC CHECKIDENT (\"{schemaName}.{tableName}\", RESEED);");
            }
        }

        internal static void ResetIdentityWorker(ApplicationDbContext context, IEnumerable<IEntityType> entities)
        {
            foreach (var entity in entities)
            {
                var tableName = context.Model.FindEntityType(entity.GetTableName());
                var schemaName = context.Model.FindEntityType(entity.GetSchema());
                context.Database.ExecuteSqlRaw($"DBCC CHECKIDENT (\"{schemaName}.{tableName}\", RESEED, 1);");
                //context.Database.ExecuteSqlRaw($"DBCC CHECKIDENT (\"{schemaName}.{tableName}\", RESEED);");
            }
        }

        internal static void SeedData(ApplicationDbContext context)
        {
            try
            {
                ProcessInsert(context, context.Customers, SampleData.Customers);
                ProcessInsert(context, context.Makes, SampleData.Makes);
                ProcessInsert(context, context.Cars, SampleData.Inventory);
                ProcessInsert(context, context.Orders, SampleData.Orders);
                ProcessInsert(context, context.CreditRisks, SampleData.CreditRisks);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        private static void ProcessInsert<TEntity>(
            ApplicationDbContext context, DbSet<TEntity> table, List<TEntity> records) where TEntity : BaseEntity
        {
            if (!table.Any())
            {
                IExecutionStrategy strategy = context.Database.CreateExecutionStrategy();
                strategy.Execute(() =>
                {
                    using var transaction = context.Database.BeginTransaction();
                    try
                    {
                        var metaData = context.Model.FindEntityType(typeof(TEntity).FullName);
                        context.Database.ExecuteSqlRaw(
                            $"SET IDENTITY_INSERT {metaData.GetSchema()}.{metaData.GetTableName()} ON");
                        table.AddRange(records);
                        context.SaveChanges();
                        context.Database.ExecuteSqlRaw(
                            $"SET IDENTITY_INSERT {metaData.GetSchema()}.{metaData.GetTableName()} OFF");
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                });
            }
        }
    }
}
*/