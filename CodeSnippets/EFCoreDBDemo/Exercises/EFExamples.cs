using DataLibrary.Data;
using EFCoreDBDemo.Models;
using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.Extensions.Configuration;

namespace EFCoreDBDemo.Exercises
{
    //The essence of deferred execution. It is only at the point at which you request the data 
    //by trying to reference it that you actually initiate a data retrieval.
    //Data is retrieved when it is required, not beforehand.

    //To understand EF(and what’s needed for the exam), you need to know that there are three
    //parts to the EF modeling.Your.NET code works with the conceptual model.You also need
    //to have some notion of the underlying storage mechanism(which, by the way, can change
    //without necessarily affecting the conceptual model). Finally, you should understand how EF
    //handles the mapping between the two.

    //EF performs a SELECT TOP (2) to enforce the constraint from the use of the Linq SingleOrDefault command.    
    //When using LINQ, if you have a collection and you want to return a single item from it,
    //you have two obvious options if you ignore nulls(four if you want to handle nulls). The
    //First function effectively says, “Give me the first one of the collection.” The Single function,
    //however, says, “Give me the single item that is in the collection and throw an exception if
    //there are more or fewer than one item.” In both cases, the xxxOrDefault handles the case
    //when the collection is empty by returning a null value.A bad programming habit that
    //many developers have is to overuse the First function when the Single function is the appropriate
    //choice.In the previous test, Single is the appropriate function to use because you
    //don’t want to allow for the possibility of more than one Customer with the same ID; if that
    //happens, something bad is going on!

    //ICollection and Hash- Set types are simple collection types that are unrelated to EF, databases, or anything
    //else. ICollection<T> is a simple interface that represents a collection; nothing special here.A
    //HashSet<T> is similar to a generic List<T>, but is optimized for fast lookups (via hashtables, as
    //the name implies) at the cost of losing order.

    public class EFExamples
    {
        private string connectionString =
            "Server=(local);Database=AdventureWorksLT2017;Integrated Security=true;MultipleActiveResultSets=true;";

        private readonly ILoggerFactory _loggerFactory =
            LoggerFactory.Create(builder =>
            {
                builder.AddConsole()
                .AddFilter(level => level == LogLevel.Information);
            });

        public async Task Run()
        {
            Action myAction = () => { Console.WriteLine("Action called. "); };
            Action exception = () => { throw new NotImplementedException(); };

            //Lazyloading();
            //LazyloadingCustomers();
            //CustomerAddress is not loaded when LazyLoading is false  
            //LazyloadingCustomers(false);
            //EagerloadingCustomers();
            //ExplicitLoading();
            //await AddRecords(myAction);
            //SimpleRead();
            //await AddRecords(exception);
            //SimpleRead();

            //await AddRecordsTS(myAction);
            //SimpleRead();
            //await AddRecordsTS(myAction); // exception);
            //SimpleRead();

            //await UpdateRecords(" the fox is always at work");
            //SimpleRead();

            //SimpleRead();
            //Console.WriteLine("\n\n");
            //await attachAndUpdateRecords();
            //SimpleRead();
            //ReadData();
            //SimpleRead();
            //SimpleReadV2();
            //SimpleReadV3();
        }

        /*Using SecretManager Tool in Console Application
         * https://cmatskas.com/configure-and-use-user-secrets-in-net-core-2-0-console-apps-in-development/
         * https://stackoverflow.com/questions/42268265/how-to-get-manage-user-secrets-in-a-net-core-console-application
         * https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-2.2&tabs=windows
        C:\repos\CodeSnippets\CodeSnippets\EFCoreDBDemo>dotnet user-secrets --help
        User Secrets Manager 3.1.5-servicing.20271.5

        Usage: dotnet user-secrets [options] [command]

        Options:
          -?|-h|--help                        Show help information
          --version                           Show version information
          -v|--verbose                        Show verbose output
          -p|--project <PROJECT>              Path to project. Defaults to searching the current directory.
          -c|--configuration <CONFIGURATION>  The project configuration to use. Defaults to 'Debug'.
          --id                                The user secret ID to use.

        Commands:
          clear   Deletes all the application secrets
          init    Set a user secrets ID to enable secret storage
          list    Lists all the application secrets
          remove  Removes the specified user secret
          set     Sets the user secret to the specified value

        Use "dotnet user-secrets [command] --help" for more information about a command.

        C:\Users\moham\source\repos\CodeSnippets\CodeSnippets\EFCoreDBDemo>dotnet user-secrets set "World:ConnectionString" "server=localhost;database=world;user=MyUser;pwd=MyPassword"
        Successfully saved World:ConnectionString = server=localhost;database=world;user=MyUser;pwd=MyPassword" to the secret store.

        */
        //Using MySQL DB
        private void SimpleReadV3()
        {
            //Retrieve ConnectionString from SecretManager 
            var configBuilder = new ConfigurationBuilder();

            //string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            //if (env == "Development")
            //{
            //    builder.AddUserSecrets<Program>();
            //}

            configBuilder.AddUserSecrets<EFExamples>();
            IConfigurationRoot Configuration = configBuilder.Build();
            string ConnectionString = Configuration["World:ConnectionString"];
            string ServerVersion = Configuration["World:ServerVersion"];
        
            var builder = new DbContextOptionsBuilder<worldContext>();
            builder.UseMySql(ConnectionString, options =>
            {
                options.ServerVersion(ServerVersion);
            });
            builder.UseLoggerFactory(_loggerFactory);

            using (worldContext dbContext = new worldContext(builder.Options))
            {
                var continents = dbContext.Continent.ToList();
                foreach (var c in continents)
                {
                    System.Console.WriteLine($"ID:{c.Id} Name:{c.Name}");
                }

                var countries = from c in dbContext.Country
                                where c.Population > 100000000
                                select c;


                Console.WriteLine($"Countries with a population more than 100 million");
                foreach (var c in countries)
                {
                    Console.WriteLine($"{c.Name} => {c.Population}");
                }
            }
        }

        //GetContext using factory
        private void SimpleReadV2()
        {
            using (AdventureWorksLT2017Context adventureWorksContext =
                new AWDbContextFactory().CreateDbContext(null))
            {
                IQueryable<ProductCategory> categories = adventureWorksContext.ProductCategory
                    .Where(p => p.Name.Contains("Alfa") || p.Name.Contains("Beta"));

                //Deferred execution
                foreach (ProductCategory p in categories)
                {
                    Console.WriteLine($"{p.ProductCategoryId} {p.Name}");
                }
            }
        }

        private static readonly Func<AdventureWorksLT2017Context, string, IEnumerable<ProductCategory>> compiledQuery =
            EF.CompileQuery((AdventureWorksLT2017Context ctx, string grpName) =>
                ctx.ProductCategory.Where(p => p.Name.Contains(grpName)));

        /* https://cmatskas.com/improve-ef-core-performance-with-compiled-queries/
         * https://gunnarpeipman.com/ef-core-compiled-queries/
         * https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/ef/language-reference/compiled-queries-linq-to-entit
         * To speed up the processing of your queries, Entity Framework supports compiled queries.
            A compiled query is used to cache the resulting SQL and only the parameters are changed
            when you run the query. Starting with .NET 4.5, queries are cached automatically. 
            This is done by generating a hash of the query and comparing that hash against the 
            in-memory cache of queries that have run previously.
            If you need even more performance, you can start compiling the queries manually.
            The compiled query needs to be static, so you avoid doing the compiling each time you run the query.
            What’s important to understand about compiled queries is that if you make a change to
            the query, EF needs to recompile it. So a generic query that you append with a Count or a
            ToList changes the semantics of the query and requires a recompilation. This means that your
            performance will improve when you have specific queries for specific actions.
            When the EF caches queries by default, the need to cache them manually is not that
            important anymore. But knowing how it works can help you if you need to apply some final
            optimizations.
        */
        private void ReadData()
        {
            try
            {
                using (AdventureWorksLT2017Context adventureWorksContext =
                    new AdventureWorksLT2017Context())
                {
                    IEnumerable<ProductCategory> categories = compiledQuery(adventureWorksContext, "Alfa");
                    foreach (ProductCategory p in categories)
                    {
                        Console.WriteLine($"{p.ProductCategoryId} {p.Name}");
                    }

                    categories = compiledQuery(adventureWorksContext, "Beta");
                    foreach (ProductCategory p in categories)
                    {
                        Console.WriteLine($"{p.ProductCategoryId} {p.Name}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                //throw;
            }        
        }

        /* https://entityframework.net/knowledge-base/41025338/why-use-attach-for-update-entity-framework-6-
         * https://stackoverflow.com/questions/30987806/dbset-attachentity-vs-dbcontext-entryentity-state-entitystate-modified
         * https://www.mssqltips.com/sqlservertip/6203/entity-framework-core-disconnected-entities-with-trackgraph/
         * https://docs.microsoft.com/en-us/ef/ef6/saving/change-tracking/entity-state
         * If you have an entity that you know already exists in the database but which is not currently 
         * being tracked by the context - which is true in your case - then you can tell the context to 
         * track the entity using the Attach method on DbSet. So in summary what Attach method does is 
         * track the entity in the context and change its state to Unchanged. When you modify a property 
         * after that, the tracking changes will change its state to Modified for you. In the case you expose 
         * above you are telling explicitly that state is Modified but also to attach the entity to your context.
         * 
         * Attach, includes them in the context with an EntityState value of Unchanged. As such, 
         * it matters greatly when you decide to call Attach. If you call it after changes are made 
         * to the item that would differentiate it from those in the underlying store, nothing would 
         * happen. If you call Attach earlier in the process, before any of the attributes are changed, 
         * a call to SaveChanges would end up executing an update operation.
         * 
        */
        private async Task attachAndUpdateRecords()
        {
            var builder = new DbContextOptionsBuilder<AdventureWorksLT2017Context>();
            builder.UseSqlServer(connectionString);
            builder.UseLoggerFactory(_loggerFactory);

            try
            {
                using (AdventureWorksLT2017Context adventureWorksContext =
                    new AdventureWorksLT2017Context(builder.Options))
                {
                    //9060 Test Beta 7/29/2020 8:26:11 PM
                    //ProductCategory category = new ProductCategory() { ProductCategoryId = 9060 };
                    //adventureWorksContext.ProductCategory.Attach(category);
                    //category.Name = $"Test Alfa {DateTime.Now.ToString()}";
                    //int rows = await adventureWorksContext.SaveChangesAsync();
                    //Console.WriteLine($"Rows affected: {rows}");

                    //The following code snippet illustrates how the EntityState property can be set to Modified.
                    //Note that this approach will enable tracking for only the ProductCategory entity in this example. 
                    //The related entities of the ProductCategory entity will not be tracked.
                    ProductCategory category = new ProductCategory() 
                    { 
                        ProductCategoryId = 9060,                                           
                        Name = $"Test Alfa {DateTime.Now.ToString()}"
                    };

                    adventureWorksContext.ProductCategory.Attach(category);
                    adventureWorksContext.Entry(category).Property("Name").IsModified = true;
                    int rows = await adventureWorksContext.SaveChangesAsync();
                    Console.WriteLine($"Rows affected: {rows}");

                    //Suppose that you create an ProductCategory instance that you didn’t get from the database using a
                    //variable named _category.You can use the following to attach it to the context, which
                    //causes it to be persisted to the database when you attempt to call SaveChanges:
                    ProductCategory _category = new ProductCategory()
                    {
                        Name = $"Test Alfa_ {DateTime.Now.ToString()}"
                    };

                    adventureWorksContext.ProductCategory.Attach(_category);
                    rows = await adventureWorksContext.SaveChangesAsync();
                    Console.WriteLine($"Rows affected: {rows}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                Console.WriteLine($"InnerException: {ex.InnerException}");
                //throw;
            }
        }

        //Because of Tracking we can retrieve records and update it by calling savechanges 
        /*
        After an entity is returned from a query, you can interact with it as you can with any other
        .NET class. You can make whatever changes you like to it; when you’re done, call the
        SaveChanges() method of the context that will then persist all the object changes back to the
        database. When SaveChanges is called, it returns an integer value reflecting the total number
        of modifications that were made to the underlying database. 
        You can similarly update an item that you created (provided that it is a valid entity type
        defined in the context) by using the Add or Attach methods and then calling SaveChanges.
         */
        private async Task UpdateRecords(string msg)
        {
            var builder = new DbContextOptionsBuilder<AdventureWorksLT2017Context>();
            builder.UseSqlServer(connectionString);
            builder.UseLoggerFactory(_loggerFactory);

            try
            {
                using (AdventureWorksLT2017Context adventureWorksContext =
                   new AdventureWorksLT2017Context(builder.Options))
                {
                    ProductCategory category = await adventureWorksContext.ProductCategory
                        .Where(p => p.Name.Contains("Alfa"))
                        .OrderBy(c => c.ProductCategoryId)
                        .FirstOrDefaultAsync();

                    Console.WriteLine($"{category.ProductCategoryId} {category.Name}");
                    category.Name = $"Test Alfa {DateTime.Now.ToString()}" ;

                    int rows = await adventureWorksContext.SaveChangesAsync();
                    Console.WriteLine($"Rows affected: {rows}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                Console.WriteLine($"InnerException: {ex.InnerException}");
                //throw;
            }
        }

        /* TransactionScope
        The beauty of the TransactionScope is that you just need to call the Complete
        method after SaveChanges or whenever you’re comfortable ending the transaction. If it fails,
        you don’t need to do anything special; just handle processing as you normally would in case
        of a failure. For the Transaction, you need to concern yourself only with if/when it successfully
        completes. The benefit of TransactionScope is that it handles everything from the simple to the
        complex in basically one method: Complete.         
         */
        private async Task AddRecordsTS(Action action)
        {
            var builder = new DbContextOptionsBuilder<AdventureWorksLT2017Context>();
            builder.UseSqlServer(connectionString);
            builder.UseLoggerFactory(_loggerFactory);

            using (AdventureWorksLT2017Context adventureWorksContext =
                new AdventureWorksLT2017Context(builder.Options))
            {
                try
                {   //https://particular.net/blog/transactionscope-and-async-await-be-one-with-the-flow
                    using (TransactionScope CurrentScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        await adventureWorksContext.ProductCategory.AddAsync(
                            new ProductCategory() { Name = "Test Alfa " + DateTime.Now.ToString() });
                        await adventureWorksContext.SaveChangesAsync();
                        await adventureWorksContext.ProductCategory.AddAsync(
                            new ProductCategory() { Name = "Test Beta " + DateTime.Now.ToString() });
                        await adventureWorksContext.SaveChangesAsync();
                        action();
                        CurrentScope.Complete();
                    }
                }
                //Handle the exception as you normally would
                //It won't be committed so transaction wise you're done
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception ex: {ex.Message}");
                }
            }
        }

        private async Task AddRecords(Action action)
        {
            var builder = new DbContextOptionsBuilder<AdventureWorksLT2017Context>();
            builder.UseSqlServer(connectionString);
            builder.UseLoggerFactory(_loggerFactory);

            using (AdventureWorksLT2017Context adventureWorksContext =
                new AdventureWorksLT2017Context(builder.Options))
            {
                try
                {
                    await adventureWorksContext.BeginTransactionAsync();
                    await adventureWorksContext.ProductCategory.AddAsync(
                        new ProductCategory() { Name = "Test Alfa " + DateTime.Now.ToString() });
                    await adventureWorksContext.SaveChangesAsync();
                    await adventureWorksContext.ProductCategory.AddAsync(
                        new ProductCategory() { Name = "Test Beta " + DateTime.Now.ToString() });
                    await adventureWorksContext.SaveChangesAsync();
                    action();
                    await adventureWorksContext.CommitTransactionAsync();
                }
                //You don’t need to necessarily call the Rollback method in an Exception handler, but it is
                //typically where you would do it. (Why roll back if things processed successfully?)
                //catch(EntityCommandExecutionException ex)
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception ex: {ex.Message}");
                    adventureWorksContext.RollbackTransaction();
                    //throw;
                }
            }
        }

        private void SimpleRead()
        {
            using (AdventureWorksLT2017Context adventureWorksContext =
                new AdventureWorksLT2017Context())
            {
                //String Output = (from p in adventureWorksContext.ProductCategory
                //                where p.Name.Contains("Alfa") || p.Name.Contains("Beta")
                //                select p).ToString();
                
                //Console.WriteLine($"Query: {Output}");

                IQueryable < ProductCategory > categories = adventureWorksContext.ProductCategory
                    .Where(p => p.Name.Contains("Alfa") || p.Name.Contains("Beta"));

                //Deferred execution
                foreach (ProductCategory p in categories)
                {
                    Console.WriteLine($"{p.ProductCategoryId} {p.Name}");
                }
            }
        }

        /* https://www.tektutorialshub.com/entity-framework-core/explicit-loading-in-entity-framework-core/
         * https://entityframeworkcore.com/querying-data-loading-eager-lazy
         * https://docs.microsoft.com/en-us/ef/core/querying/related-data
         * Explicit loading
         * You can explicitly load a navigation property via the DbContext.Entry(...) API.
         * You can also explicitly load a navigation property by executing a separate query that 
         * returns the related entities. If change tracking is enabled, then when loading an entity, 
         * EF Core will automatically set the navigation properties of the newly-loaded entitiy to 
         * refer to any entities already loaded, and set the navigation properties of the already-loaded 
         * entities to refer to the newly-loaded entity. */
        private void ExplicitLoading()
        {
            var builder = new DbContextOptionsBuilder<AdventureWorksLT2017Context>();
            builder.UseSqlServer(connectionString);
            builder.UseLoggerFactory(_loggerFactory);

            var ids = new List<int> { 29485, 29486, 29489, 29490 };
            using (AdventureWorksLT2017Context adventureWorksContext =
                new AdventureWorksLT2017Context(builder.Options))
            {
                IQueryable<Customer> customers = adventureWorksContext.Customer
                    .Where(c => ids.Contains(c.CustomerId)); //.Take(3);

                foreach (Customer customer in customers)
                {
                    Console.WriteLine($"{customer.FirstName} {customer.LastName}");

                    adventureWorksContext.Entry(customer)
                        .Collection(c => c.CustomerAddress)
                        .Load();

                    foreach (CustomerAddress address in customer.CustomerAddress)
                    {
                        adventureWorksContext.Entry(address).Reference(c => c.Address).Load();

                        Console.WriteLine($"{address.Address.AddressLine1} {address.Address.AddressLine2}");
                    }
                }
            }
        }

        /* Eager loading means that the related data is loaded from the database as part of the initial query.
         * You can use the Include method to specify related data to be included in query results.

        * Entity Framework Core will automatically fix-up navigation properties to any other entities that 
        * were previously loaded into the context instance. So even if you don't explicitly include the data 
        * for a navigation property, the property may still be populated if some or all of the related entities 
        * were previously loaded.
        * 
        * You can include related data from multiple relationships in a single query.
        */
        private void EagerloadingCustomers()
        {
            var builder = new DbContextOptionsBuilder<AdventureWorksLT2017Context>();
            builder.UseSqlServer(connectionString);
            builder.UseLoggerFactory(_loggerFactory);

            var ids = new List<int> { 29485, 29486, 29489, 29490 };
            using (AdventureWorksLT2017Context adventureWorksContext =
                new AdventureWorksLT2017Context(builder.Options))
            {
                IQueryable<Customer> customers = adventureWorksContext.Customer
                    .Include(c => c.CustomerAddress)
                        //You can drill down through relationships to include multiple levels of related 
                        //data using the ThenInclude method. The following example loads all CustomerAddress, 
                        //their related Address. You can chain multiple calls to ThenInclude to continue 
                        //including further levels of related data. You can combine all of this to include 
                        //related data from multiple levels and multiple roots in the same query.
                        .ThenInclude(c => c.Address)
                    .Where(c => ids.Contains(c.CustomerId));

                foreach (Customer customer in customers)
                {
                    Console.WriteLine($"{customer.FirstName} {customer.LastName}");
                    foreach (CustomerAddress address in customer.CustomerAddress)
                    {
                        Console.WriteLine($"\t{address.Address.AddressLine1} {address.Address.AddressLine2}");
                    }
                }
            }
        }

        private void LazyloadingCustomers(bool bLazyloading = true)
        {
            var builder = new DbContextOptionsBuilder<AdventureWorksLT2017Context>();
            builder.UseSqlServer(connectionString);
            builder.UseLoggerFactory(_loggerFactory);
            builder.UseLazyLoadingProxies(useLazyLoadingProxies: bLazyloading);

            var ids = new List<int> { 29485, 29486, 29489, 29490 };
            using (AdventureWorksLT2017Context adventureWorksContext =
                new AdventureWorksLT2017Context(builder.Options))
            {
                IQueryable<Customer> customers = adventureWorksContext.Customer
                    .Where(c => ids.Contains(c.CustomerId)); //.Take(3);

                foreach (Customer customer in customers)
                {
                    Console.WriteLine($"{customer.FirstName} {customer.LastName}");
                    foreach(CustomerAddress address in customer.CustomerAddress)
                    {
                        Console.WriteLine($"{address.Address.AddressLine1} {address.Address.AddressLine2}");                        
                    }
                }
            }
        }

        /* https://docs.microsoft.com/en-us/ef/core/querying/related-data
         * Lazy loading means that the related data is transparently loaded from the database when the navigation property is accessed.
         * The simplest way to use lazy-loading is by installing the Microsoft.EntityFrameworkCore.Proxies 
         * package and enabling it with a call to UseLazyLoadingProxies. For 
         * Install-Package Microsoft.EntityFrameworkCore.Proxies
         * 
         * EF Core will then enable lazy loading for any navigation property that can be overridden--that is, it
         * must be virtual and on a class that can be inherited from. For example, in the following entities,
         * the Customer.SalesOrderHeader and SalesOrderHeader.SalesOrderDetail navigation properties will be lazy-loaded.
         * Note you need to set in connectionstring: MultipleActiveResultSets=true;
         */
        private void Lazyloading(bool bLazyloading = true)
        {
            var builder = new DbContextOptionsBuilder<AdventureWorksLT2017Context>();
            builder.UseSqlServer(connectionString);
            builder.UseLoggerFactory(_loggerFactory);
            builder.UseLazyLoadingProxies(useLazyLoadingProxies: bLazyloading);

            using (AdventureWorksLT2017Context adventureWorksContext =
                new AdventureWorksLT2017Context(builder.Options))
            {
                IQueryable<Customer> customers = adventureWorksContext.Customer.Take(3);

                foreach (Customer customer in customers)
                {
                    Console.WriteLine($"{customer.FirstName} {customer.LastName}");
                    Console.WriteLine($"\tOrders");
                    foreach (SalesOrderHeader salesOrderHeader in customer.SalesOrderHeader)
                    {
                        Console.WriteLine($"\t\t{salesOrderHeader.OrderDate} " +
                            $"${salesOrderHeader.SubTotal} ${salesOrderHeader.TaxAmt}");
                    }
                }
            }
        }
    }
}
