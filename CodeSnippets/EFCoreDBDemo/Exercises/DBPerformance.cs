using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DataLibraryDapper;
using EFCoreDBDemo.Models;
using DataLibrary;
using System.Diagnostics;
using System.Linq;
using DataLibrary.Data;
using Microsoft.EntityFrameworkCore;
using EFCoreDBDemo.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Data.SqlClient;

//https://www.youtube.com/watch?v=Y__n6OOt9IQ
//If you use NoTracking, you can have a performance increase in EF
//Context connection pool makes already created connections to database available 
//Query type does not use entity Tracking (NoTracking)
//Concept of Batching inceases speed in EF Core (Uses math to calculate 
//for example that 1000 inserts can be done in 50 calls to the database (batches of 40 inserts) ) 
//Batches can also reduce the number of calls to SQL Azure (Saves money if you pay per call)  
//Batch size can be adjusted on a query basis or for the complete DBContext  

/* Our architect, in my our company, does not want us to use stored procedures going forward. 
 * He wants us to linq queries. I am concerned about that. I have tried to write best linq queries, 
 * the sql it generates is not the best. But feel, I can write better sql queries than linq with 
 * stored procedures. And I prefer to use linq for simple queries but for complex queries I like 
 * to use stored procedures. Is that a good practice?  The determination should be made based upon 
 * desired and realized performance, amongst other considerations.  The determination should be made 
 * based upon desired and realized performance, amongst other considerations.
 * 
 * Should I create new DBContext for every request or use single instance through out the application?
 * Your DBContext instance should be dependency injected into what you have handling each post get or put.
 * Use this -> services.AddTransient<yourDBContext>();   trust me, that is the way to go, I tried with 
 * AddSingleton and AddScoped, but Transient is safest. Add transient is a bad option, scoped is the way to go. 
 */
namespace EFCoreDBDemo.Exercises
{
    public class DBPerformance
    {
        private string connectionString =
            "Server=(local);Database=AdventureWorksLT2017;Integrated Security=true;";

        public async Task Run()
        {
            //Example01();
            //await Example02();
            //await Example03();

            //ResetAndWarmUpEFCore();
            //ResetAndWarmUpEFDapper()

            //RunTest(GetAllCustomersDapper, "Get All Customers Dapper");
            //RunTest(GetAllCustomersEFCore, "Get All Customers EF Core");
            //RunTest(AddRecordsAndSaveEFCore, "Add Records And Save EF Core", maxIterations:3);
            //RunTest(AddRecordsAndSaveDapper, "Add Records And Save Dapper", maxIterations:3);
            //RunTest(AddRecordsAndSaveNoBatching, "Add Records And Save EF Core No Batching", maxIterations: 3);

            //RunTest(RunComplexQueryEFCore, "Run Complex Query EF Core");
            //RunTest(GetAllCustomersEFCoreAsNoTracking, "Get All Customers EF Core As No Tracking", 20);

            //GetProductsFromSql();
            //GetProductsFromSproc();
            //AddRecordsAndSaveNoBatching();

            //List<ProductViewModel> products = await InterpolatedQueryAsync();
            //int count = 0;
            //foreach (ProductViewModel p in products)
            //{
            //    Console.WriteLine($"{++count} {p.ProductName} {p.ProductNumber} {p.GroupName}");
            //}

            //List<ProductViewModel> products = await InterpolatedQuerySPAsync();
            //int count = 0;
            //foreach (ProductViewModel p in products)
            //{
            //    Console.WriteLine($"{++count} {p.ProductName} {p.ProductNumber} {p.GroupName}");
            //}

            //GetProductsFromSprocWithPrm();
        }

        /*NOTE you need to use  Microsoft.Data.SqlClient instead of System.Data.SqlClient
         * else you would get the following exception:
         * “SqlParameterCollection only accepts non-null SqlParameter type objects, not String objects”

        SQL syntax to create Stored Procedure 
        CREATE OR ALTER   PROCEDURE [dbo].[uspGetTopCustomersAndCount]
        (
	        @Top int,
            @OverallCount INT OUTPUT
        )
        AS
        BEGIN
            SET @OverallCount = (SELECT COUNT(*) FROM [AdventureWorksLT2017].[SalesLT].[Customer])
            SELECT TOP (@Top) * FROM [AdventureWorksLT2017].[SalesLT].[Customer] order by CustomerID
	        --SELECT TOP (@Top) CustomerID, FirstName, LastName FROM [AdventureWorksLT2017].[SalesLT].[Customer] order by CustomerID 
        END
        GO

        First, declare the @count variable to hold the the value of the output parameter of the stored procedure:
                DECLARE @count INT;

        Then, execute the [dbo].[uspGetTopCustomersAndCount] stored procedure and passing the parameters:
        EXEC[dbo].[uspGetTopCustomersAndCount]
                @Top = 10, 
                @OverallCount = @count OUTPUT;

        SELECT @count AS 'Number of Customers found';

        References:
        https://erikej.github.io/efcore/2020/08/03/ef-core-call-stored-procedures-out-parameters.html
        https://www.sqlservertutorial.net/sql-server-stored-procedures/stored-procedure-output-parameters/
        https://docs.microsoft.com/en-us/ef/core/querying/raw-sql

        */
        private void GetProductsFromSprocWithPrm()
        {
            try
            {
                var builder = new DbContextOptionsBuilder<AdventureWorksLT2017Context>();
                builder.UseSqlServer(connectionString);
                builder.UseLoggerFactory(_loggerFactory);

                using (AdventureWorksLT2017Context adventureWorksContext = new AdventureWorksLT2017Context(builder.Options))
                {
                    var parameterTop = new SqlParameter
                    {
                        ParameterName = "Top",
                        SqlDbType = System.Data.SqlDbType.Int,
                        Value = 10,
                    };

                    var parameterOverallCount = new SqlParameter
                    {
                        ParameterName = "OverallCount",
                        SqlDbType = System.Data.SqlDbType.Int,
                        Direction = System.Data.ParameterDirection.Output,
                    };

                    var customers = adventureWorksContext.Set<SimpleCustomer>()
                        .FromSqlRaw("EXEC [dbo].[uspGetTopCustomersAndCount]  @Top, @OverallCount OUTPUT ",
                        parameterTop, parameterOverallCount)
                        .ToList();

                    Console.WriteLine($"parameterOverallCount: {(int)parameterOverallCount.Value}");

                    parameterTop.Value = 20;
                    List<Customer> customers_ = adventureWorksContext.Customer
                        .FromSqlRaw("EXEC [dbo].[uspGetTopCustomersAndCount]  @Top, @OverallCount OUTPUT ",
                        parameterTop, parameterOverallCount)
                        .ToList();

                    Console.WriteLine($"parameterOverallCount: {(int)parameterOverallCount.Value}");

                    int count = 0;
                    foreach (var c in customers)
                    {
                        Console.WriteLine($"{++count} {c.CustomerId} {c.FirstName} {c.LastName}");
                    }

                    Console.WriteLine("\n\n");

                    count = 0;
                    foreach (Customer c in customers_)
                    {
                        Console.WriteLine($"#{++count} {c.CustomerId} {c.FirstName} {c.LastName}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                //throw;
            }
        }

        //You can create ViewModels to store data from different tables  
        //public virtual DbSet<ProductViewModel> ProductViewModels { get; set; }
        //modelBuilder.Entity<ProductViewModel>().HasNoKey(); In early versions this is the same as DbQuery<ProductViewModel> instead of DbSet<ProductViewModel>  
        //https://docs.microsoft.com/en-us/sql/relational-databases/stored-procedures/create-a-stored-procedure?view=sql-server-ver15
        private async Task<List<ProductViewModel>> InterpolatedQuerySPAsync()
        {
            using (AdventureWorksLT2017Context adventureWorksContext =
                new AdventureWorksLT2017Context())
            {
                const string selectedGroupName = @"Helm%";

                List<ProductViewModel> products = await adventureWorksContext.ProductViewModels
                    //@FirstName = { emp.FirstName } , @LastName = { emp.LastName },@Address = {emp.Address}, @ZipCode = { emp.ZipCode} ,@State = {  emp.State },  @Country = {emp.Country} ,@EmployeeID ={emp.EmployeeId}
                    .FromSqlInterpolated($@"dbo.uspGetProductsByName @GroupName = { selectedGroupName }")
                    //.FromSqlInterpolated($@"dbo.uspGetProductsByName {selectedGroupName}")
                    //.FromSqlInterpolated($@"EXEC dbo.uspGetProductsByName {selectedGroupName}")
                    .AsNoTracking()
                    .ToListAsync();

                return products;
            }
        }

        //https://www.tektutorialshub.com/entity-framework-core/logging-in-ef-core/
        private readonly ILoggerFactory _loggerFactory = 
            LoggerFactory.Create(builder =>
            {
                builder.AddConsole()
                .AddFilter(level => level == LogLevel.Information);
            });

        //Query type dont use tracking (NoTracking)
        //You need to call ToList() in order to execute query, deferred execution  
        //We can use linq operators on Query type 
        private async Task<List<ProductViewModel>> InterpolatedQueryAsync()
        {
            var builder = new DbContextOptionsBuilder<AdventureWorksLT2017Context>();
            builder.UseSqlServer(connectionString);
            builder.UseLoggerFactory(_loggerFactory);

            using (AdventureWorksLT2017Context adventureWorksContext = 
                new AdventureWorksLT2017Context(builder.Options))
            {
                const string selectedGroupName = @"Road%";

                FormattableString sql = $@"SELECT p.Name as ProductName, p.ProductNumber, c.Name as GroupName 
                                        from SalesLT.Product p 
                                        inner join SalesLT.ProductCategory c 
                                        on p.ProductCategoryID = c.ProductCategoryID 
                                        where c.Name like ({selectedGroupName})";

                string sql2 = $@"SELECT p.Name as ProductName, p.ProductNumber, c.Name as GroupName 
                                from SalesLT.Product p 
                                inner join SalesLT.ProductCategory c 
                                on p.ProductCategoryID = c.ProductCategoryID";

                //Note if you want to combine FromSqlInterpolated sql query with linq operators
                //you should not end your sql query (sql, sql2) with ;
                List<ProductViewModel> products = await adventureWorksContext.ProductViewModels
                    .FromSqlInterpolated(sql)
                    .OrderBy(p => p.ProductName)
                    .ToListAsync();

                //If we want to execute queries without c# interpolation we can use FromSqlRaw  
                //List<ProductViewModel> products = await adventureWorksContext.ProductViewModels
                //    .FromSqlRaw(sql2)
                //    .OrderBy(p => p.ProductName)
                //    .ToListAsync();

                return products;

                //int count = 0;
                //foreach (ProductViewModel p in products)
                //{
                //    Console.WriteLine($"{++count} {p.ProductName} {p.ProductNumber} {p.GroupName}");
                //}
            }
        }

        private void GetProductsFromSproc()
        {
            using (AdventureWorksLT2017Context adventureWorksContext = new AdventureWorksLT2017Context())
            {
                List<ProductViewModel> products = adventureWorksContext.ProductViewModels
                    .FromSqlRaw("dbo.uspGetProducts")                    
                    .ToList();

                int count = 0; 
                foreach (ProductViewModel p in products)
                {
                    Console.WriteLine($"{++count} {p.ProductName} {p.ProductNumber} {p.GroupName}");
                }
            }
        }

        private void GetProductsFromSql()
        {
            using (AdventureWorksLT2017Context adventureWorksContext = new AdventureWorksLT2017Context())
            {
                List<ProductViewModel> products = adventureWorksContext.ProductViewModels
                    .FromSqlInterpolated(@$"SELECT p.Name as ProductName, p.ProductNumber, c.Name as GroupName from SalesLT.Product p inner join SalesLT.ProductCategory c on p.ProductCategoryID = c.ProductCategoryID")
                    .OrderBy(p => p.ProductName)
                    .ToList();
            
                foreach(ProductViewModel p in products)
                {
                    Console.WriteLine($"{p.ProductName} {p.ProductNumber} {p.GroupName}");
                }
            }
        }

        private void RunTest(Action testMethod, string Label, int maxIterations = 10)
        {
            Console.WriteLine($"\nRunTest: {Label}");
            List<long> duration = new List<long>();
            var stopwatch = new Stopwatch();
            for (int iteration = 0; iteration < maxIterations; iteration++)
            {
                stopwatch.Restart();
                testMethod();
                stopwatch.Stop();
                var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                duration.Add(elapsedMilliseconds);
                Console.WriteLine($"{iteration}:      {elapsedMilliseconds.ToString().PadLeft(4)}ms");
            }
            Console.WriteLine();
            Console.WriteLine($"Max duration:      {duration.Max().ToString().PadLeft(4)}ms");
            Console.WriteLine($"Min duration:      {duration.Min().ToString().PadLeft(4)}ms");
            Console.WriteLine($"Avg duration:      {duration.Average().ToString().PadLeft(4)}ms");
        }

        public void ResetAndWarmUpEFCore()
        {
            Trace.WriteLine("Entering ResetAndWarmUpEFCore()");
            using (AdventureWorksContext adventureWorksContext = new AdventureWorksContext())
            {
                int rowsAffected = adventureWorksContext.Database.ExecuteSqlRaw(
                    @"DELETE FROM [SalesLT].[ProductCategory] WHERE Name LIKE 'Test %'");
                Trace.WriteLine($"rowsAffected: {rowsAffected}");
                DataLibrary.Models.Customer customer = adventureWorksContext.Customer.FirstOrDefault();
                Trace.Assert(customer != default(DataLibrary.Models.Customer));
            }
            Trace.WriteLine("Exiting ResetAndWarmUpEFCore()");
        }

        public void GetAllCustomersEFCoreAsNoTracking()
        {
            using (AdventureWorksContext adventureWorksContext = new AdventureWorksContext())
            {
                List<DataLibrary.Models.Customer> customers = 
                    adventureWorksContext.Customer.AsNoTracking().ToList();
                Trace.Assert(customers.Count > 0);
            }
        }

        public void GetAllCustomersEFCore()
        {
            using (AdventureWorksContext adventureWorksContext = new AdventureWorksContext())
            {
                List<DataLibrary.Models.Customer> customers = adventureWorksContext.Customer.ToList();
                Trace.Assert(customers.Count > 0);
            }
        }

        public void AddRecordsAndSaveNoBatching()
        {
            var builder = new DbContextOptionsBuilder<AdventureWorksContext>();
            builder.UseSqlServer(connectionString, options => options.MaxBatchSize(1));

            using (AdventureWorksContext adventureWorksContext = new AdventureWorksContext(builder.Options))
            {
                for (int i = 0; i < 1000; i++)
                {
                    adventureWorksContext.ProductCategory.Add(
                        new DataLibrary.Models.ProductCategory
                        {
                            Name = $"Test {Guid.NewGuid()}"
                        });
                }
                adventureWorksContext.SaveChanges();
            }
        }

        public void AddRecordsAndSaveEFCore()
        {
            using (AdventureWorksContext adventureWorksContext = new AdventureWorksContext())
            {
                for (int i = 0; i < 1000; i++)
                {
                    adventureWorksContext.ProductCategory.Add(
                        new DataLibrary.Models.ProductCategory 
                        { 
                            Name = $"Test {Guid.NewGuid()}" 
                        });
                }
                adventureWorksContext.SaveChanges();
            }
        }

        public void RunComplexQueryEFCore()
        {
            using (AdventureWorksContext adventureWorksContext = new AdventureWorksContext())
            {
                var customers = adventureWorksContext.Customer
                    .Include(nameof(DataLibrary.Models.Customer.CustomerAddress))
                    //.Include(nameof(DataLibrary.Models.CustomerAddress.Address))
                    .Include(nameof(DataLibrary.Models.Customer.SalesOrderHeader))
                    //.Include(nameof(DataLibrary.Models.SalesOrderHeader.SalesOrderDetail))
                    //.Include(nameof(DataLibrary.Models.SalesOrderHeader.BillToAddress))
                    //.Include(nameof(DataLibrary.Models.SalesOrderHeader.ShipToAddress))
                    .Select(c =>
                    new 
                    {
                        c.FirstName,
                        c.LastName,
                        OrderCount = c.SalesOrderHeader.Count,
                        AddressCount = c.CustomerAddress.Count,
                        //How to avoid processing Default value
                        //c.CustomerAddress.OrderByDescending(a => a.AddressId).FirstOrDefault().Address.City
                    }).Take(100).ToList();

                //foreach(var c in customers)
                //{
                //    Console.WriteLine($"{c.FirstName} {c.LastName} {c.OrderCount} {c.AddressCount}");
                //        //$"{c.AddressCount} {c.City}");
                //}
            }
        }

        public void ResetAndWarmUpDapper()
        {
            using (IDataAccess dataAccess = new DataAccess(connectionString))
            {
                Trace.WriteLine("Entering ResetAndWarmUpDapper().");
                string deleteSql = @"DELETE FROM [SalesLT].[ProductCategory] WHERE Name LIKE 'Test %'";
                Trace.WriteLine(deleteSql);
                int rowsAffected = dataAccess.SaveData<dynamic>(deleteSql, new { });
                Trace.WriteLine($"rowsAffected: {rowsAffected}");

                string selectSql = @"SELECT TOP(1) * from [SalesLT].[Customer]";
                IEnumerable<Customer> customers = dataAccess.LoadData<dynamic, Customer>(selectSql, new { });
                Trace.WriteLine($"customers.Count(): {customers.Count()}");
                foreach (Customer c in customers)
                {
                    Trace.WriteLine($"{c.CustomerId} {c.FirstName} {c.LastName}");
                }
                Trace.WriteLine("Exiting ResetAndWarmUpDapper().");
            }
        }

        public void GetAllCustomersDapper()
        {
            Trace.WriteLine("Entering GetAllCustomersDapper().");
            using (IDataAccess dataAccess = new DataAccess(connectionString))
            {
                string sql = "select * from [SalesLT].[Customer]";
                IEnumerable<Customer> customers =
                    dataAccess.LoadData<dynamic, Customer>(sql, new { });
                Trace.WriteLine($"customers.Count: {customers.Count()}");
                Trace.Assert(customers.Count() > 0);
            }
            Trace.WriteLine("Exiting GetAllCustomersDapper().");
        }

        public void AddRecordsAndSaveDapper()
        {
            Trace.WriteLine("Entering AddRecordsAndSaveDapper().");
            string insertSql = $"Insert into [SalesLT].[ProductCategory] (Name) values(@Name)";

            using (IDataAccess dataAccess = new DataAccess(connectionString))
            {
                for (int i = 0; i < 1000; i++)
                {
                    int rowsAffected = dataAccess.SaveData<dynamic>(insertSql, 
                        new { Name = $"Test {Guid.NewGuid()}" });
                    Trace.WriteLine($"Insert i == {i}, rowsAffected: {rowsAffected}");
                }
            }

            Trace.WriteLine("Exiting AddRecordsAndSaveDapper().");
        }

        public async Task Example03()
        {
            try
            {
                IDataAccess dataAccess = new DataAccess(connectionString);

                //Insert statement
                string insertSql = $"Insert into [SalesLT].[ProductCategory] (Name, " +
                    $"rowguid, ModifiedDate) values(@Name, @rowguid, @ModifiedDate)";
                dynamic parameters = new
                {
                    Name = "Cars",
                    rowguid = Guid.NewGuid(),
                    ModifiedDate = DateTime.Now
                };

                //Console.WriteLine($"{insertSql}");
                int rowsAffected = await dataAccess.SaveDataAsync<dynamic>(insertSql, parameters);
                Console.WriteLine($"rowsAffected: {rowsAffected}");

                string sql = "select * from [SalesLT].[ProductCategory]";
                IEnumerable<ProductCategory> categories = await
                    dataAccess.LoadDataAsync<dynamic, ProductCategory>(sql, new { });

                foreach (ProductCategory p in categories)
                {
                    Console.WriteLine($"{p.Name} {p.ModifiedDate}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception {ex.Message}");
                //throw;
            }
        }

        public async Task Example02()
        {
            try
            {
                IDataAccess dataAccess = new DataAccess(connectionString);
                string sql = "select * from [SalesLT].[Customer]";
                IEnumerable<Customer> customers = await 
                    dataAccess.LoadDataAsync<dynamic, Customer>(sql, new { });

                foreach (Customer c in customers)
                {
                    Console.WriteLine($"{c.CustomerId} {c.FirstName} {c.LastName}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception {ex.Message}");
                //throw;
            }
        }

        public void Example01()
        {
            IDataAccess dataAccess = new DataAccess(connectionString);
            string sql = "select * from [SalesLT].[Customer]";
            IEnumerable<Customer> customers = dataAccess.LoadData<dynamic, Customer>(sql, new { });
            foreach (Customer c in customers)
            {
                Console.WriteLine($"{c.CustomerId} {c.FirstName} {c.LastName}");
            }

            //Insert statement
            string insertSql = $"Insert into[SalesLT].[ProductCategory] (Name, " +
                $"rowguid, ModifiedDate) values(@Name, @rowguid, @ModifiedDate)";
            dynamic parameters = new
            {
                Name = "Watch",
                rowguid = Guid.NewGuid(),
                ModifiedDate = DateTime.Now
            };

            Console.WriteLine($"{insertSql}");
            int rowsAffected = dataAccess.SaveData<dynamic>(insertSql, parameters);
            Console.WriteLine($"rowsAffected: {rowsAffected}");
        }
    }
}
