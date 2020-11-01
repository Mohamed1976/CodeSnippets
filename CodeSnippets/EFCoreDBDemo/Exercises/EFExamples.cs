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
using DataLibrary.ContosoUniversity.Data;
using DataLibrary.ContosoUniversity.Models;
using DataLibrary.ContosoUniversity.Models.ViewModels;
using DataLibrary.BankDemo.Data;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using DataLibrary.Repository;
using DataLibrary.MusicStore.Data;
using DataLibrary.MusicStore.Models;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Internal;
using DataLibrary.RepositoryV2;
//using DataLibrary.CarDealerships.Models;
//using DataLibrary.BankDemo.Models;

/* References
Entity Framework Core MS Toolbox series
https://github.com/skimedic/presentations/tree/master/DOTNETCORE/Channel9_EFCoreShows/EntityFrameworkCoreExamples
https://www.youtube.com/watch?v=Y__n6OOt9IQ
https://www.youtube.com/watch?v=aJZyfio-kz4
https://www.youtube.com/watch?v=Gij0WYUuxJc
https://www.youtube.com/watch?v=IjSLmsu4MeA
https://www.youtube.com/watch?v=dsyijlyz85s
https://www.youtube.com/watch?v=7DnVcBzpEiQ
https://www.youtube.com/watch?v=-HGG6BAwd_A
https://www.youtube.com/watch?v=m5BhHHn3_uw
https://www.youtube.com/watch?v=mFX9mRx-HJk


EF Intro series
https://github.com/skimedic/presentations/tree/master/DOTNETCORE/Channel9
https://www.youtube.com/watch?v=xx5_pVsLP44
https://www.youtube.com/watch?v=ta46892FM6g
https://www.youtube.com/watch?v=KtnkkEPlm44
https://www.youtube.com/watch?v=aX9EBlRM9U8
https://www.youtube.com/watch?v=iFHsQuBB6ZU

*/
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

            //PopulateSchoolContext();
            //DisplaySchoolContext();
            //GlobalQueryFilters();
            //GlobalQueryFiltersV2();

            //https://github.com/brendan-ssw/Northwind-EFCore3/blob/master/Northwind.EF.WebApi/Controllers/CustomerController.cs
            //https://github.com/kgrzybek/sample-dotnet-core-cqrs-api/blob/master/src/SampleProject.API/Customers/CustomersController.cs
            //https://github.com/dotnet/AspNetCore.Docs/blob/master/aspnetcore/data/ef-mvc/intro/samples/cu-final/Controllers/StudentsController.cs
            //https://github.com/jbogard/ContosoUniversityDotNetCore-Pages/blob/master/ContosoUniversity/Models/Instructor.cs
            //DisplayStudentsAndInstructor();
            //DisplayStudents();
            //OverviewStudents();
            //DisplayInstructors();
            //DisplayDepartments();
            //DisplayCourses();
            //AddOfficeAssignment();
            //DisplayInstructor();
            //OverviewStudentsV2();
            //BankDemo();

            //TODO
            //In order to add new migrations, you need to use DataBase.Migration() instead of EnsuredCreated 
            //Note when adding a new Migration i got the next error
            //C:\Users\moham\source\repos\CodeSnippets\CodeSnippets>dotnet ef migrations add 
            //FormatDecimalColumn -o BankDemo\Data\Migrations -c DataLibrary.BankDemo.Data.BankContext 
            //--project DataLibrary --startup-project            
            //System.IndexOutOfRangeException: Index was outside the bounds of the array.
            //Index was outside the bounds of the array.
            //SeedBankDB();

            //SeedCarDealershipsDB();
            //DisplayCarDealershipsDB();
            //ConcurrencyExample();

            //Using UDF User Defined Functions
            //SetupShopDatabase();
            //DisplayOrderAndDetails();
            //Console.WriteLine("---------------------------------");
            //Console.WriteLine("Updating the records");
            //UpdateRecords();
            //DisplayOrderAndDetails();
            //Console.WriteLine("---------------------------------");
            //Console.WriteLine("Get Records Over 50");
            //GetRecordsOver(50);

            //By defining private properties as in the DataLibrary.BlogDemo.Models.Blog
            //EF Core uses the private properties to populate the Blog object instead
            //of using the corresponding setters, hence not triggering propertychanged events
            //SetupDatabase();
            //UpdateBlogDB();

            //Using ChangeTracker.StateChanged in DBContext, we get notified
            //When objects change, we can use this info to update audit table  
            //For example every time a Blog is changed we can write to the 
            //BlogAudit table, so we keep track of changes made to the table.
            //This can be used as an alternative to Database triggers
            //UpdateBlogDB(url:"www.nu.nl", name:"Dutch World News");
            //SimulateTransientErrorHandling();

            //Run testcases in Transaction
            //DataLibrary.BlogDemo.Data.BloggingContext context = new DataLibrary.BlogDemo.Data.BloggingContext();
            //ExecuteInATransaction(context, ExecuteTestcases);

            ////New context
            //using var _context = new DataLibrary.BlogDemo.Data.BloggingContext();
            //foreach(DataLibrary.BlogDemo.Models.Blog b in _context.Blogs.OrderBy(b => b.Id))
            //{
            //    Console.WriteLine($"{b.Id} {b.Name} ({b.Url}) IsDirty = {b.IsDirty}");
            //}

            //DisplayCustomers("itz");
            //SimpleQueries();
            //ChangeTracking();
            //RepositoryExample();
            //GetPersonAndRelatedData();
            //ExplicitlyLoadRelatedData();
            //CreateProjections();
            //PersistingData();
            //DataFiltering();
            //DisplaySongs();
            //ShowBankCustomers();
            //SimpleSchoolQuery();
            //QueryFiltering();
            //QueryFilteringV2();
            //AdventureWorksQueryWithLazyLoading();
            //RepositoryExampleV1();
            RepositoryExample();
        
            
        }

        private void RepositoryExample()
        {
            var builder = new DbContextOptionsBuilder<BankContext>();
            builder.UseSqlServer("Server=(local);Database=BankDemo;Integrated Security=true;");
            //builder.UseLoggerFactory(_loggerFactory);
            BankContext bankContext = new BankContext(builder.Options);
            CustomerRepo customerRepo = new CustomerRepo(bankContext);

            var customers = customerRepo.GetAll();
            Console.WriteLine($"\n-------------------------------CUSTOMERS-------------------------------");
            foreach (var c in customers)
            {
                Console.WriteLine($"{c.Id}, {c.FullName}, from object: {c.FirstName} {c.LastName}");
            }

            Console.WriteLine($"\n-------------------------------EMAILS-------------------------------");
            EmailRepo emailRepo = new EmailRepo(bankContext);
            var emails = emailRepo.GetAll();
            foreach (var e in emails)
            {
                Console.WriteLine($"{e.Id} {e.EmailAddress} {e.CreationDate.ToString()}");
            }
        }

        //In order to use DbContextOptionsBuilder we need the following namespace:
        //Microsoft.EntityFrameworkCore
        //Assembly: Microsoft.EntityFrameworkCore.dll
        //https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.dbcontextoptionsbuilder?view=efcore-3.1
        private void RepositoryExampleV1()
        {
            const string ConnectionString 
                = "Server=(local);Database=ContosoUniversity;Integrated Security=true;";

            DbContextOptionsBuilder<DataLibrary.ContosoUniversity.Data.SchoolContext> builder = 
                new DbContextOptionsBuilder<DataLibrary.ContosoUniversity.Data.SchoolContext>();

            builder.UseSqlServer(ConnectionString, sqloptions =>
            {
                sqloptions.EnableRetryOnFailure(
                    maxRetryCount: 6,
                    maxRetryDelay: TimeSpan.FromSeconds(60),
                    errorNumbersToAdd: new List<int>() { });
                sqloptions.CommandTimeout(60);
            });

            builder.UseLoggerFactory(_loggerFactory);
            builder.EnableSensitiveDataLogging();

            using (IUnitOfWork unitOfWork = new UnitOfWork(new SchoolContext(builder.Options)))
            {
                //Because of change tracking of reference types we can update Student object
                var studentFound = unitOfWork.Students
                    .SingleOrDefault(s => s.PersonalInformation.LastName.Contains("Gov"));

                if (studentFound != null)
                    studentFound.PersonalInformation.LastName = "Cuomo";

                unitOfWork.Complete();

                IEnumerable<DataLibrary.ContosoUniversity.Models.Student> students =
                    unitOfWork.Students.GetAll();

                foreach(var student in students)
                {
                    Console.WriteLine($"{student.FullName}");
                }

                return;
                var stud = new DataLibrary.ContosoUniversity.Models.Student()
                {                     
                    EnrollmentDate = DateTime.Now,
                    PersonalInformation = new Person()
                    {
                        FirstMidName = "Gret",
                        LastName = "Gov",
                        EmailAddress = "Gret.Gov@hotmail.com",
                        DateOfBirth = DateTime.Now
                    }
                };

                unitOfWork.Students.Add(stud);
                unitOfWork.Complete();

                students = unitOfWork.Students.GetAll();

                foreach (var student in students)
                {
                    Console.WriteLine($"{student.FullName}");
                }
                
                IEnumerable<DataLibrary.ContosoUniversity.Models.Instructor> instructors = 
                    unitOfWork.Instructors.GetInstructors(1);

                foreach(var instructor in instructors)
                {
                    Console.WriteLine($"{instructor.PersonalInformation.FullName}");
                }

                instructors = unitOfWork.Instructors.GetInstructorsWithCourse(1);

                foreach (var instructor in instructors)
                {
                    Console.WriteLine($"#{instructor.PersonalInformation.FullName}");
                    foreach(var courseAssignment in instructor.CourseAssignments)
                    {
                        Console.WriteLine($"\t{courseAssignment.Course.Title}");
                    }
                }
                
                IEnumerable<DataLibrary.ContosoUniversity.Models.Course> courses  
                    = unitOfWork.Courses.GetAll();

                foreach(var course in courses)
                {
                    Console.WriteLine(course);
                }

                courses = unitOfWork.Courses.Find(c => c.Title.Contains("Ma"));

                foreach (var course in courses)
                {
                    Console.WriteLine("#" + course);
                }

                courses = unitOfWork.Courses.GetTopEnrollmentCourses(1);
                foreach (var course in courses)
                {
                    Console.WriteLine($"## {course} {course.Enrollments.Count}");
                }

                courses = unitOfWork.Courses.GetTopEnrollmentCourses(2);
                foreach (var course in courses)
                {
                    Console.WriteLine($"### {course} {course.Enrollments.Count}");
                }

                courses = unitOfWork.Courses.GetTopEnrollmentCourses(10);
                foreach (var course in courses)
                {
                    Console.WriteLine($"#### {course} {course.Enrollments.Count}");
                }
            }
        }

        private void AdventureWorksQueryWithLazyLoading()
        {
            const string ConnectionString = "Server=(local);Database=Adventureworks2016;Integrated Security=true;";

            try
            {
                var builder = new DbContextOptionsBuilder<DataLibrary.Adventureworks.Data.AwDbContext>();
                //builder.UseSqlServer("Server=(local);Database=Adventureworks2016;Integrated Security=true;");
                //The EnableRetryOnFailure configuration
                //https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.infrastructure.sqlserverdbcontextoptionsbuilder?view=efcore-3.1
                builder.UseSqlServer(ConnectionString, sqloptions => 
                {
                    sqloptions.EnableRetryOnFailure(
                        maxRetryCount: 6,
                        maxRetryDelay: TimeSpan.FromSeconds(60),
                        errorNumbersToAdd: new List<int>() { });

                    sqloptions.CommandTimeout(60); 
                });

                builder.UseLoggerFactory(_loggerFactory);
                builder.EnableSensitiveDataLogging();
                builder.UseLazyLoadingProxies();

                using DataLibrary.Adventureworks.Data.AwDbContext context = 
                    new DataLibrary.Adventureworks.Data.AwDbContext(builder.Options);

                //Query is executed, retrieves all customers due to ToList<Customer>()
                /*SELECT TOP(@__p_0) [c].[CustomerID], [c].[AccountNumber], [c].[ModifiedDate], [c].[PersonID], [c].[rowguid], [c].[StoreID], [c].[TerritoryID]
                FROM[Sales].[Customer] AS[c]
                LEFT JOIN[Person].[Person] AS[p] ON[c].[PersonID] = [p].[BusinessEntityID]
                WHERE[p].[BusinessEntityID] IS NOT NULL*/
                IList<DataLibrary.Adventureworks.Models.Customer> _customers = context.Customer
                    .Where(c => c.Person != null && c.Person.EmailAddress != null)
                    .Take(100) //Top 100
                    .ToList<DataLibrary.Adventureworks.Models.Customer>();

                DataLibrary.Adventureworks.Models.Customer _customer =
                    _customers.Where(c => c.CustomerId == 11000).FirstOrDefault();

                /*SELECT[p].[BusinessEntityID], [p].[AdditionalContactInfo], [p].[Demographics], [p].[EmailPromotion], [p].[FirstName], [p].[LastName], [p].[MiddleName], [p].[ModifiedDate], [p].[NameStyle], [p].[PersonType], [p].[rowguid], [p].[Suffix], [p].[Title]
                FROM[Person].[Person] AS[p]
                WHERE[p].[BusinessEntityID] = @__p_0*/
                Console.WriteLine("Person object loaded.");
                context.Entry(_customer).Reference(c => c.Person).Load();

                /*SELECT[e].[BusinessEntityID], [e].[EmailAddressID], [e].[EmailAddress], [e].[ModifiedDate], [e].[rowguid]
                FROM[Person].[EmailAddress] AS[e]
                WHERE[e].[BusinessEntityID] = @__p_0*/
                Console.WriteLine("EmailAddress collection loaded.");
                context.Entry(_customer.Person).Collection(p => p.EmailAddress).Load();

                Console.WriteLine($"CustomerId: {_customer.CustomerId} {_customer.Person.FirstName}");
                foreach (var email in _customer.Person.EmailAddress)
                {
                    Console.WriteLine($"\t\t{email.EmailAddress1}");
                }

                return;
                //Using eager loading
                //Query is executed, retrieves all customers due to ToList<Customer>(),
                //The Where clausule makes selection, and only 1000 records 
                /*SELECT[t].[CustomerID], [t].[AccountNumber], [t].[ModifiedDate], [t].[PersonID], [t].[rowguid], [t].[StoreID], [t].[TerritoryID], [p0].[BusinessEntityID], [p0].[AdditionalContactInfo], [p0].[Demographics], [p0].[EmailPromotion], [p0].[FirstName], [p0].[LastName], [p0].[MiddleName], [p0].[ModifiedDate], [p0].[NameStyle], [p0].[PersonType], [p0].[rowguid], [p0].[Suffix], [p0].[Title], [e].[BusinessEntityID], [e].[EmailAddressID], [e].[EmailAddress], [e].[ModifiedDate], [e].[rowguid]
                FROM(
                    SELECT TOP(@__p_0)[c].[CustomerID], [c].[AccountNumber], [c].[ModifiedDate], [c].[PersonID], [c].[rowguid], [c].[StoreID], [c].[TerritoryID]
                    FROM[Sales].[Customer] AS[c]
                    LEFT JOIN[Person].[Person] AS[p] ON[c].[PersonID] = [p].[BusinessEntityID]
                    WHERE[p].[BusinessEntityID] IS NOT NULL
                ) AS[t]
                LEFT JOIN[Person].[Person] AS[p0] ON[t].[PersonID] = [p0].[BusinessEntityID]
                LEFT JOIN[Person].[EmailAddress] AS[e] ON[p0].[BusinessEntityID] = [e].[BusinessEntityID]
                ORDER BY[t].[CustomerID], [e].[BusinessEntityID], [e].[EmailAddressID]*/
                IList<DataLibrary.Adventureworks.Models.Customer> customers = context.Customer
                    .Include(c => c.Person) //Eager loading 
                        .ThenInclude(p => p.EmailAddress)  //Eager loading
                    .Where(c => c.Person != null && c.Person.EmailAddress != null)
                    .Take(100) //Top 100
                    .ToList<DataLibrary.Adventureworks.Models.Customer>();

                //CustomerId: 11000 Jon
                //jon24 @adventure-works.com
                //Filtering is done client side
                DataLibrary.Adventureworks.Models.Customer customer =
                    customers.Where(c => c.CustomerId == 11000).FirstOrDefault();
                
                Console.WriteLine($"CustomerId: {customer.CustomerId} {customer.Person.FirstName}");
                foreach (var email in customer.Person.EmailAddress)
                {
                    Console.WriteLine($"\t\t{email.EmailAddress1}");
                }

                //foreach(var customer in customers)
                //{
                //    Console.WriteLine($"CustomerId: {customer.CustomerId}");
                //    Console.WriteLine($"\tFirstName: {customer?.Person?.FirstName}");
                //    if(customer.Person != null && 
                //        customer.Person.EmailAddress != null && 
                //        customer.Person.EmailAddress.Count > 0)
                //    {
                //        foreach (var email in customer.Person.EmailAddress)
                //        {
                //            Console.WriteLine($"\t\t{email.EmailAddress1}");
                //        }
                //    }

                //if(customer.Person != null)
                //{ 
                //    Console.WriteLine($"\tFirstName: {customer.Person.FirstName}");

                //    if(customer.Person.EmailAddress != null && customer.Person.EmailAddress.Count > 0)
                //    { 
                //        //Show emailaddresses
                //        foreach (var email in customer.Person.EmailAddress)
                //        {
                //            Console.WriteLine($"\t\t{email.EmailAddress1}");
                //        }
                //    }
                //}
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}, InnerException: {ex?.InnerException?.Message}");
            }
        }

        /* Id: 1, Title: Literature
        Id: 2, Title: Composition
        Id: 3, Title: Trigonometry
        Id: 4, Title: Calculus
        Id: 5, Title: Macroeconomics
        Id: 6, Title: Microeconomics
        Id: 7, Title: Chemistry
        Id: 15, Title: Physics
        Id: 16, Title: Physics
        Id: 17, Title: Physics */
        private void QueryFilteringV2()
        {
            try
            {
                List<History> history = new List<History>()
                {
                    new History() { Id = 1, IsSuspended = false },
                    new History() { Id = 2, IsSuspended = false },
                    new History() { Id = 3, IsSuspended = false },
                    new History() { Id = 4, IsSuspended = true },
                    new History() { Id = 5, IsSuspended = false },
                    new History() { Id = 6, IsSuspended = false },
                    new History() { Id = 7, IsSuspended = false },
                    new History() { Id = 15, IsSuspended = true },
                    new History() { Id = 16, IsSuspended = true },
                    new History() { Id = 17, IsSuspended = true },
                };

                var builder = new DbContextOptionsBuilder<SchoolContext>();
                builder.UseSqlServer("Server=(local);Database=ContosoUniversity;Integrated Security=true;MultipleActiveResultSets=True;Connection Timeout=120;");
                builder.UseLoggerFactory(_loggerFactory);
                builder.EnableSensitiveDataLogging();

                using SchoolContext _context = new SchoolContext(builder.Options);
                //Note you need to filter the Courses client side.
                //.AsQueryable() results in an error because you can not do the filtering server side.
                //
                //https://www.codeproject.com/articles/732425/ienumerable-vs-iqueryable
                //IEnumerable explained
                //While querying data from database, IEnumerable executes select query on server side, 
                //load data in-memory on client side and then filter data. Hence does more work and 
                //becomes slow.
                //
                //IQueryable explained
                //While querying data from database, IQueryable executes select query on server 
                //side with all filters. Hence does less work and becomes fast.
                //Join between local data and server data   
                var query = _context.Courses.AsEnumerable()//.AsQueryable()
                    .Join(history, x => x.Id, y => y.Id, (x, y) => new { Current = x, Historical = y })
                    .Where(x => !x.Historical.IsSuspended)
                    .Select(x => x.Current);

                Console.WriteLine("Execute deferred query.");
                foreach (Course c in query)
                {
                    Console.WriteLine($"Id: {c.Id}, Title: {c.Title}");
                }

                Func<int,bool> checkFunc = (id) =>
                {
                    return id > 7;
                };

                Console.WriteLine("#Execute deferred query.\n");
                //Similar query filtering client side
                var _query = _context.Courses.AsEnumerable()
                    .Where(x => checkFunc(x.Id));

                foreach (Course c in _query)
                {
                    Console.WriteLine($"Id: {c.Id}, Title: {c.Title}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}, InnerException: {ex?.InnerException?.Message}");
            }
        }

        public class History
        {
            public bool IsSuspended { get; set; }
            public int Id { get; set; }
        }

        //Displays students and their associated enrollments.  
        //Enrollments are filtered based on the Department of the course in each enrollment.  
        //You need to return students with their filtered list of enrollments included 
        //in a single round trip to the database.
        private void QueryFiltering()
        {
            try
            {
                var builder = new DbContextOptionsBuilder<SchoolContext>();
                builder.UseSqlServer("Server=(local);Database=ContosoUniversity;Integrated Security=true;MultipleActiveResultSets=True;Connection Timeout=120;");
                builder.UseLoggerFactory(_loggerFactory);
                builder.EnableSensitiveDataLogging();

                using SchoolContext _context = new SchoolContext(builder.Options);

                //Selects Enrollments of students that have courses in the english department  
                //Selects student and enrolement
                /*SELECT [t0].[Id], [t0].[EnrollmentDate], [t0].[IsDeleted], [t0].[TimeStamp], [t1].[Id], [t1].[DateOfBirth], [t1].[EmailAddress], [t1].[FirstName], [t1].[LastName], [t].[Id], [t].[CourseID], [t].[Grade], [t].[IsDeleted], [t].[StudentID], [t].[TimeStamp]
                  FROM [Student] AS [s]
                  INNER JOIN (
                      SELECT [e].[Id], [e].[CourseID], [e].[Grade], [e].[IsDeleted], [e].[StudentID], [e].[TimeStamp], [c].[Id] AS [Id0], [d].[Id] AS [Id1]
                      FROM [Enrollment] AS [e]
                      INNER JOIN [Dbo].[Course] AS [c] ON [e].[CourseID] = [c].[Id]
                      INNER JOIN [Department] AS [d] ON [c].[DepartmentID] = [d].[Id]
                      WHERE [d].[Name] = N'English'
                  ) AS [t] ON [s].[Id] = [t].[StudentID]
                  INNER JOIN (
                      SELECT [s0].[Id], [s0].[EnrollmentDate], [s0].[IsDeleted], [s0].[TimeStamp]
                      FROM [Student] AS [s0]
                      WHERE [s0].[IsDeleted] <> CAST(1 AS bit)
                  ) AS [t0] ON [t].[StudentID] = [t0].[Id]
                  LEFT JOIN (
                      SELECT [s1].[Id], [s1].[DateOfBirth], [s1].[EmailAddress], [s1].[FirstName], [s1].[LastName]
                      FROM [Student] AS [s1]
                      WHERE [s1].[LastName] IS NOT NULL AND ([s1].[FirstName] IS NOT NULL AND [s1].[DateOfBirth] IS NOT NULL)
                  ) AS [t1] ON [t0].[Id] = [t1].[Id]
                  WHERE [s].[IsDeleted] <> CAST(1 AS bit)
                6 FullName: Justice, Peggy, Enrollment Date: 9/1/2011, EmailAddress:Peggy.Justice@gmail.com, Date Of Birth: 8/15/1991, Age: 29, CourseID: 1
                2 FullName: Alonso, Meredith, Enrollment Date: 9/1/2012, EmailAddress:Meredith.Alonso@gmail.com, Date Of Birth: 8/15/1999, Age: 21, CourseID: 2
                5 FullName: Li, Yan, Enrollment Date: 9/1/2012, EmailAddress:Yan.Li@gmail.com, Date Of Birth: 8/15/1993, Age: 27, CourseID: 2 */
                IEnumerable<Student> students = _context.Students
                    .SelectMany(s => s.Enrollments
                        .Where(e => e.Course.DepartmentNavigation.Name == "English"))
                    .Select(e => new { student = e.Student, enrollment = e }) //Result in anonymous type c#
                    //Note if you remove .ToList(), query will be optimised on the final select,
                    //hence no enrollments will be retrieved. 
                    .ToList() //Executes query on the database
                    .Select(a => a.student); //Convert the anonymous type to IEnumerable<Student>   

                foreach (Student student in students)
                {
                    Console.WriteLine($"{student.Id} {student}");
                    foreach(Enrollment e in student.Enrollments)
                    {
                        Console.WriteLine($"\tCourseID: { e.CourseID}");
                    }                    
                }

                //foreach (var student in students)
                //{
                //    Console.WriteLine($"{student.student.Id} {student.student}, CourseID: {student.enrollment.CourseID}");
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}, InnerException: {ex?.InnerException?.Message}");
            }
        }

        private void SimpleSchoolQuery()
        {
            try
            {
                var builder = new DbContextOptionsBuilder<SchoolContext>();
                builder.UseSqlServer("Server=(local);Database=ContosoUniversity;Integrated Security=true;MultipleActiveResultSets=True;Connection Timeout=120;");
                builder.UseLoggerFactory(_loggerFactory);
                builder.EnableSensitiveDataLogging();

                using SchoolContext _context = new SchoolContext(builder.Options);
                                
                IEnumerable<Course> courses3 = _context.Courses.Include("DepartmentNavigation");
                Console.WriteLine($"IEnumerable<Course> courses3, not executed because of deferred execution.");
                courses3 = courses3.Where(c => c.Id > 4);
                //Query is executed and filtering is done client side
                //SELECT[c].[Id], [c].[Credits], [c].[DepartmentID], [c].[IsDeleted], [c].[TimeStamp], [c].[Title], [d].[Id], [d].[Budget], [d].[InstructorID], [d].[IsDeleted], [d].[Name], [d].[StartDate], [d].[TimeStamp]
                //FROM[Dbo].[Course] AS[c]
                //INNER JOIN[Department] AS[d] ON[c].[DepartmentID] = [d].[Id]
                List<Course> _courses3 = courses3.ToList();
                foreach (Course c in _courses3)
                {
                    Console.WriteLine($"{c.Id} {c.Title} {c.DepartmentNavigation.Name}");
                }

                return;
                IQueryable<Course> courses2 = _context.Courses;
                courses2 = courses2.Include("DepartmentNavigation");
                //Deferred execution, query is now executed on the database.
                Console.WriteLine($"Press key to execute query.");
                Console.ReadKey();
                //SELECT[c].[Id], [c].[Credits], [c].[DepartmentID], [c].[IsDeleted], [c].[TimeStamp], [c].[Title], [d].[Id], [d].[Budget], [d].[InstructorID], [d].[IsDeleted], [d].[Name], [d].[StartDate], [d].[TimeStamp]
                //FROM[Dbo].[Course] AS[c]
                //INNER JOIN[Department] AS[d] ON[c].[DepartmentID] = [d].[Id]
                List<Course> _courses2 = courses2.ToList();
                foreach (Course c in _courses2)
                {
                    Console.WriteLine($"{c.Title} {c.DepartmentNavigation.Name}");
                }

                return;
                using (SchoolContext context = new SchoolContext())
                {
                    IQueryable<Course> courses = context.Courses;
                    courses = courses.Include("DepartmentNavigation");
                    //Deferred execution, query is now executed on the database.
                    List<Course> _courses = courses.ToList();

                    foreach(Course c in _courses)
                    {
                        Console.WriteLine($"{c.Title} {c.DepartmentNavigation.Name}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}, InnerException: {ex?.InnerException?.Message}");
            }
        }

        private void ShowBankCustomers()
        {
            IEnumerable<DataLibrary.BankDemo.Models.Customer> customers =
                GetCustomers(c => c.Id > 50 && c.Id < 60);

            foreach(var c in customers)
            {
                Console.WriteLine($"{c.Id} {c.FullName}");
            }
        }

        private IEnumerable<DataLibrary.BankDemo.Models.Customer> 
            GetCustomers(Expression<Func<DataLibrary.BankDemo.Models.Customer, bool>> predicate) 
        {
            var builder = new DbContextOptionsBuilder<BankContext>();
            builder.UseSqlServer("Server=(local);Database=BankDemo;Integrated Security=true;MultipleActiveResultSets=True;Connection Timeout=120;");
            builder.UseLoggerFactory(_loggerFactory);
            builder.EnableSensitiveDataLogging();

            using BankContext context = new BankContext(builder.Options);

            IEnumerable<DataLibrary.BankDemo.Models.Customer> customers = 
                context.Customers.Where(predicate).ToList();

            return customers;
        }

        //For a quick run, I used in-memory database provided by EF Core.
        //dotnet add package Microsoft.EntityFrameworkCore.InMemory
        private void DisplaySongs()
        {
            var builder = new DbContextOptionsBuilder<MusicContext>();
            builder.UseInMemoryDatabase("MusicDB");
            builder.UseLoggerFactory(_loggerFactory);
            builder.EnableSensitiveDataLogging();
            using MusicContext musicContext = new MusicContext(builder.Options);

            //Seed the database
            if (!musicContext.Albums.Any())
            {
                musicContext.Database.EnsureCreated();
            }

            foreach (Album album in musicContext.Albums.Include("Songs"))
            {
                Console.WriteLine($"{album}, count{album.Songs.Count}");
                foreach(Song song in album.Songs)
                {
                    Console.WriteLine($"\t{song}");
                }
            }
        }

        private void DataFiltering()
        {
            var builder = new DbContextOptionsBuilder<BankContext>();
            builder.UseSqlServer("Server=(local);Database=BankDemo;Integrated Security=true;MultipleActiveResultSets=True;Connection Timeout=120;");
            builder.UseLoggerFactory(_loggerFactory);
            builder.EnableSensitiveDataLogging();

            using (BankContext context = new BankContext(builder.Options))
            {
                //Both linq statements below result in the same SQL query 
                //Filtering is done client side
                IEnumerable<DataLibrary.BankDemo.Models.Customer> customers2 = 
                    context.Customers.ToList().AsQueryable();
                customers2 = customers2.Where(c => c.Id < 50);

                //Resulting query
                //SELECT[c].[Id], [c].[FirstName], [c].[IsDeleted], [c].[LastName], [c].[TimeStamp]
                //FROM[Customers] AS[c]
                //WHERE[c].[IsDeleted] <> CAST(1 AS bit)
                foreach (var c in customers2)
                {
                    Console.WriteLine($"#{c.Id} {c.FullName}");
                }

                //Filtering is done client side
                IEnumerable<DataLibrary.BankDemo.Models.Customer> customers = context.Customers;
                customers = customers.Where(c => c.Id < 50);
                //The resulting query.
                //SELECT[c].[Id], [c].[FirstName], [c].[IsDeleted], [c].[LastName], [c].[TimeStamp]
                //FROM[Customers] AS[c]
                //WHERE[c].[IsDeleted] <> CAST(1 AS bit)
                foreach (var c in customers)
                {
                    Console.WriteLine($"{c.Id} {c.FullName}");
                }
            }

            using (BankContext context = new BankContext(builder.Options))
            {
                //Filtering is done server side.
                IQueryable<DataLibrary.BankDemo.Models.Customer> customers = context.Customers;
                customers = customers.Where(c => c.Id < 50);
                //Resulting query
                //SELECT[c].[Id], [c].[FirstName], [c].[IsDeleted], [c].[LastName], [c].[TimeStamp]
                //FROM[Customers] AS[c]
                //WHERE([c].[IsDeleted] <> CAST(1 AS bit)) AND([c].[Id] < 50)
                foreach (var c in customers)
                {
                    Console.WriteLine($"{c.Id} {c.FullName}");
                }
            }

            using (BankContext context = new BankContext(builder.Options))
            {
                //Is executed on the server because of ToList(), retrieves complete list of customers
                IQueryable<DataLibrary.BankDemo.Models.Customer> customers = 
                    context.Customers.ToList().AsQueryable();
                //The resulting query retrieves all customers, note where clause is caused by queryfilter defined in DBContext
                //SELECT[c].[Id], [c].[FirstName], [c].[IsDeleted], [c].[LastName], [c].[TimeStamp]
                //FROM[Customers] AS[c]
                //WHERE[c].[IsDeleted] <> CAST(1 AS bit)

                //Filtering is done client side
                customers = customers.Where(c => c.Id < 50);
                foreach(var c in customers)
                {
                    Console.WriteLine($"{c.Id} {c.FullName}");
                }
            }
        }

        //https://github.com/skimedic/presentations/blob/master/DOTNETCORE/Channel9/EfCore/D_PersistingData.cs
        private void PersistingData()
        {
            var builder = new DbContextOptionsBuilder<BankContext>();
            //server=(localdb)\mssqllocaldb;Database=Adventureworks2016;Trusted_Connection=True;
            builder.UseSqlServer("Server=(local);Database=BankDemo;Integrated Security=true;MultipleActiveResultSets=True;Connection Timeout=120;");
            builder.UseLoggerFactory(_loggerFactory);
            builder.EnableSensitiveDataLogging();

            using BankContext context = new BankContext(builder.Options);

            try
            {
                ShouldExecuteInATransaction(AddNewCustomer, context);
                ShouldExecuteInATransaction(AddAnObjectGraph, context);
                ShouldExecuteInATransaction(AddNewCustomers, context);

                //using (var transaction = context.Database.BeginTransaction())
                //{
                //    var person2 = new DataLibrary.BankDemo.Models.Customer
                //    {
                //        FirstName = "John",
                //        LastName = "Deer",
                //    };

                //    context.Customers.Add(person2);
                //    context.SaveChanges();
                //    Console.WriteLine($"{person2.Id} {person2.FullName}");
                //    transaction.Rollback();
                //}

                //return;
                //var person = new DataLibrary.BankDemo.Models.Customer
                //{
                //    FirstName = "Barney",
                //    LastName = "Rubble",
                //};

                //context.Customers.Add(person);
                //context.SaveChanges();
                //Console.WriteLine($"{person.Id} {person.FullName}");

            }
            catch(Exception ex)
            {
                Console.WriteLine($"{ex.Message} {ex?.InnerException?.Message}");
            }
            finally
            {

            }
        }

        private void AddNewCustomers(BankContext context)
        {
            List<DataLibrary.BankDemo.Models.Customer> customers = new
                List<DataLibrary.BankDemo.Models.Customer>()
            {
                new DataLibrary.BankDemo.Models.Customer
                {
                    FirstName = "Michael",
                    LastName = "Shikorsky",
                },
                new DataLibrary.BankDemo.Models.Customer
                {
                    FirstName = "Bill",
                    LastName = "MicrosoftGates",
                }
            };

            context.Customers.AddRange(customers);
            context.SaveChanges();

            var customersSelected = context.Customers.Where(c => c.LastName == "Shikorsky" || c.LastName == "MicrosoftGates");

            foreach(var c in customersSelected)
            {
                Console.WriteLine($"{c.Id} {c.FullName}");
            }

            Console.WriteLine($"From list: {customers[0].Id} {customers[0].FullName}");
            Console.WriteLine($"From list: {customers[1].Id} {customers[1].FullName}");
        }

        private void AddNewCustomer(BankContext context)
        {
            var customer = new DataLibrary.BankDemo.Models.Customer
            {
                FirstName = "John",
                LastName = "Deer",
            };

            context.Customers.Add(customer);
            context.SaveChanges();
            Console.WriteLine($"{customer.Id} {customer.FullName}");
        }

        private void AddAnObjectGraph(BankContext context)
        {
            var customer = new DataLibrary.BankDemo.Models.Customer
            {
                FirstName = "Barney",
                LastName = "Littlerock",
            };

            customer.EmailAddresses.Add(
                new DataLibrary.BankDemo.Models.Email()
                {
                    EmailAddress = "Barney.Littlerock@gmail.com"
                }); ;

            context.Customers.Add(customer);
            context.SaveChanges();
            Console.WriteLine($"{customer.Id} {customer.FullName} {customer.EmailAddresses.First().EmailAddress}");
        }

        //The database action is rolledback to keep db clean.
        public void ShouldExecuteInATransaction(Action<BankContext> testMethod, BankContext context)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                testMethod(context);
                transaction.Rollback();
            }
        }

        private void CreateProjections()
        {
            var builder = new DbContextOptionsBuilder<DataLibrary.Adventureworks.Data.AwDbContext>();
            //server=(localdb)\mssqllocaldb;Database=Adventureworks2016;Trusted_Connection=True;
            builder.UseSqlServer("Server=(local);Database=Adventureworks2016;Integrated Security=true;");
            builder.UseLoggerFactory(_loggerFactory);
            builder.EnableSensitiveDataLogging();

            using DataLibrary.Adventureworks.Data.AwDbContext context =
                new DataLibrary.Adventureworks.Data.AwDbContext(builder.Options);

            //Create list of anonymous objects
            var anonymousList = context.Person
                .OrderBy(p => p.LastName)
                .ThenBy(p => p.FirstName)
                .Take(20)
                .Select(x => new
                {
                    x.FirstName,
                    x.MiddleName,
                    x.LastName,
                    x.EmailAddress
                })
                .ToList();

            foreach(var o in anonymousList)
            {
                Console.WriteLine($"{o.FirstName} {o.LastName} {o.EmailAddress.Count}");
            }

            //IQueryable<ICollection<DataLibrary.Adventureworks.Models.EmailAddress>> 
            //    emailCollection = context.Person.Select(x => x.EmailAddress);

            List<ICollection<DataLibrary.Adventureworks.Models.EmailAddress>> emailCollection 
                = context.Person.Select(x => x.EmailAddress).ToList();


            int index = 0;
            foreach(ICollection<DataLibrary.Adventureworks.Models.EmailAddress> emailAddresses in emailCollection)
            {
                index++;
                foreach (DataLibrary.Adventureworks.Models.EmailAddress emailAddress in emailAddresses)
                {
                    Console.WriteLine($"{index} {emailAddress.EmailAddress1}");
                }
            }

            ////Select Many flattens the list
            IQueryable<DataLibrary.Adventureworks.Models.EmailAddress> emailAddresses2 = 
                context.Person.SelectMany(x => x.EmailAddress);

            ////Project to a ViewModel
            List<PersonViewModel> newVMList = context.Person
                .Select(x => new PersonViewModel
                {
                    FirstName = x.FirstName,
                    MiddleName = x.MiddleName,
                    LastName = x.LastName
                })
                .ToList();
        }

        //A nested class
        public class PersonViewModel
        {
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }
        }

        //ExplicitlyLoading
        //LazyLoading
        //EagerLoading
        private void ExplicitlyLoadRelatedData()
        {
            var builder = new DbContextOptionsBuilder<DataLibrary.Adventureworks.Data.AwDbContext>();
            //server=(localdb)\mssqllocaldb;Database=Adventureworks2016;Trusted_Connection=True;
            builder.UseSqlServer("Server=(local);Database=Adventureworks2016;Integrated Security=true;");
            builder.UseLoggerFactory(_loggerFactory);
            builder.EnableSensitiveDataLogging();

            using DataLibrary.Adventureworks.Data.AwDbContext context =
                new DataLibrary.Adventureworks.Data.AwDbContext(builder.Options);

            var p = context.Person.FirstOrDefault(x => x.BusinessEntityId == 1);
            context.Entry(p).Reference(p => p.Employee).Load();
            context.Entry(p).Collection(p => p.EmailAddress).Load();

            Console.WriteLine($"{p.FirstName} {p.LastName} " +
                $"{p?.Employee?.HireDate.ToShortDateString()} " +
                $"{p?.EmailAddress.Count} " +
                $"{p?.EmailAddress.FirstOrDefault().EmailAddress1}");
        }

        private void GetPersonAndRelatedData()
        {
            var builder = new DbContextOptionsBuilder<DataLibrary.Adventureworks.Data.AwDbContext>();
            //server=(localdb)\mssqllocaldb;Database=Adventureworks2016;Trusted_Connection=True;
            builder.UseSqlServer("Server=(local);Database=Adventureworks2016;Integrated Security=true;");
            builder.UseLoggerFactory(_loggerFactory);
            builder.EnableSensitiveDataLogging();

            try
            {
                using DataLibrary.Adventureworks.Data.AwDbContext context =
                    new DataLibrary.Adventureworks.Data.AwDbContext(builder.Options);

                //Get collections (many of many to one)
                var persons = context.Person.Include(x => x.EmailAddress);

                //Get Parent (one of one to one)
                var persons2 = context.Person.Include(x => x.BusinessEntity);

                //Get Chain of related
                IQueryable<DataLibrary.Adventureworks.Models.Person> persons3 = context.Person
                    .Include(x => x.Employee)
                        .ThenInclude(x => x.SalesPerson)
                    .Where(p => p.Employee.SalesPerson != null); //If you want to include records with SalesPerson 
                    //.Take(10);
                //persons3.ToList(); //Execute query, deferred execution
                foreach (var p in persons3)
                {
                    Console.WriteLine($"{p.FirstName} {p.LastName} " +
                        $"{p?.Employee?.HireDate.ToShortDateString()} " +
                        $"{p?.Employee?.SalesPerson?.Bonus}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message} {ex?.InnerException?.Message}");
                //throw;
            }
        }

        //https://github.com/skimedic/presentations/blob/master/DOTNETCORE/Channel9/EfCore/A_ChangeTracking.cs
        private void ChangeTracking()
        {
            var builder = new DbContextOptionsBuilder<DataLibrary.Adventureworks.Data.AwDbContext>();
            //server=(localdb)\mssqllocaldb;Database=Adventureworks2016;Trusted_Connection=True;
            builder.UseSqlServer("Server=(local);Database=Adventureworks2016;Integrated Security=true;");
            builder.UseLoggerFactory(_loggerFactory);
            builder.EnableSensitiveDataLogging();
            int index = 0;

            try
            {
                using DataLibrary.Adventureworks.Data.AwDbContext context =
                    new DataLibrary.Adventureworks.Data.AwDbContext(builder.Options);
                
                Console.WriteLine("*** Create new Customer ***");
                DataLibrary.Adventureworks.Models.Person person =
                    new DataLibrary.Adventureworks.Models.Person()
                    {
                        AdditionalContactInfo = "Home",
                        FirstName = "Barney",
                        LastName = "Rubble",
                        Title = "Neighbor"
                    };

                EntityEntry<DataLibrary.Adventureworks.Models.Person> newEntityEntry 
                    = context.Entry(person);

                DisplayEntityStatus(newEntityEntry); //Entity State => Detached

                //GetEntity()
                Console.WriteLine("*** Get Entity *** ");
                var person2 = context.Person.Find(1);
                var person3 = context.Person.Where(x => x.BusinessEntityId == 5).AsNoTracking();

                index = 0;
                foreach (EntityEntry e in context.ChangeTracker.Entries())
                {
                    DisplayEntityStatus(e);
                }

                Console.WriteLine("*** Add Entity *** ");
                DataLibrary.Adventureworks.Models.Person person4 =
                    new DataLibrary.Adventureworks.Models.Person()
                    {
                        AdditionalContactInfo = "Office",
                        FirstName = "Donald",
                        LastName = "Duck",
                        Title = "Meester"
                    };

                context.Person.Add(person4);

                //0 Entity State => Added (person4) 
                //1 Entity State => Unchanged (person2, since it is not in memory it is queried )

                index = 0;
                foreach (EntityEntry e in context.ChangeTracker.Entries())
                {
                    DisplayEntityStatus(e);
                }

                //EditEntity()
                Console.WriteLine("*** Edit Entity *** ");
                var person5 = context.Person.Find(2);
                Console.WriteLine($"person5 {person5.FirstName} {person5.LastName}");
                person5.LastName = "Flinstone";
                context.Person.Update(person);
                //_context.SaveChanges();                
                //return _context.ChangeTracker.Entries().First();
                index = 0;
                foreach (EntityEntry e in context.ChangeTracker.Entries())
                {
                    DisplayEntityStatus(e);
                }

                //DeleteEntity()
                Console.WriteLine("*** Delete Entity *** ");
                var person6 = context.Person.Find(6);
                //This isn't in memory -> retrieved from database
                context.Entry(person6).State = EntityState.Deleted; //EntityState.Modified;
                //This must be in memory -> retrieved from database
                //_context.Person.Remove(person);
                //_context.SaveChanges();
                //return _context.ChangeTracker.Entries().First();
                index = 0;
                foreach (EntityEntry e in context.ChangeTracker.Entries())
                {
                    DisplayEntityStatus(e);
                    DisplayModifiedPropertyStatus(e);
                }

                Console.WriteLine("*** Modify Entity *** ");
                var person7 = context.Person.Find(7);
                person7.FirstName = "Alfa";
                person7.LastName = "Beta";
                index = 0;
                foreach (EntityEntry e in context.ChangeTracker.Entries())
                {
                    DisplayEntityStatus(e);
                    DisplayModifiedPropertyStatus(e);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}, InnerException: {ex?.InnerException?.Message}");
            }

            //Function within function, local
            void DisplayEntityStatus(EntityEntry entry)
            {                
                Console.WriteLine($"{index++} Entity State => {entry.State}");
            }

            void DisplayModifiedPropertyStatus(EntityEntry entry)
            {
                Console.WriteLine("*** Changed Properties");
                foreach (var prop in entry.Properties
                    .Where(x => !x.IsTemporary && x.IsModified))
                {
                    Console.WriteLine(
                        $"\tProperty: {prop.Metadata.Name}\r\n\t Orig Value: {prop.OriginalValue}\r\n\t Curr Value: {prop.CurrentValue}");
                }
            }
        }

        private void SimpleQueries()
        {
            var builder = new DbContextOptionsBuilder<DataLibrary.Adventureworks.Data.AwDbContext>();
            //server=(localdb)\mssqllocaldb;Database=Adventureworks2016;Trusted_Connection=True;
            builder.UseSqlServer("Server=(local);Database=Adventureworks2016;Integrated Security=true;");
            //builder.UseLoggerFactory(_loggerFactory);
            try
            {
                using DataLibrary.Adventureworks.Data.AwDbContext context = 
                    new DataLibrary.Adventureworks.Data.AwDbContext(builder.Options);

                //Nothing Happens because of deffered execution
                IQueryable<DataLibrary.Adventureworks.Models.Person> query 
                    = context.Person.AsQueryable();

                //This can't be translated into SQL by the SQL Server tranlsation engine
                //Not all Linq commands are supported by SQL database provider 
                //Throws InvalidOperationException ex
                //_ = context.Person.LastOrDefault(x => x.PersonType == "em");

                /* This will throw an error InvalidOperationException ex
                _ = _context.Person.SingleOrDefault(x => x.BusinessEntityId <= 2);

                Executed Query: 
                SELECT TOP(2) * -- actual fields listed in real query
                FROM [Person].[Person] AS [p]
                WHERE [p].[BusinessEntityID] <= 2 */

                //All in one statement
                var query4 = context.Person
                    .Where(x => x.PersonType == "em" && x.EmailPromotion == 1);
                
                //Chained statements
                var query5 = context.Person
                    .Where(x => x.PersonType == "em")
                    .Where(x => x.EmailPromotion == 1);

                //Chaining the were clausule
                IQueryable<DataLibrary.Adventureworks.Models.Person> query6 = 
                    context.Person.AsQueryable();                
                
                //You can have multiple filters, simple as shown below 
                if(true) //if (!String.IsNullOrEmpty(searchString))
                {
                    query6 = query6.Where(x => x.PersonType == "em");
                }

                if(true)
                {
                    query6 = query6.Where(x => x.EmailPromotion == 1);
                }

                //Or's can't be chained
                IQueryable<DataLibrary.Adventureworks.Models.Person> query7 = context.Person
                    .Where(x => x.PersonType == "em" || x.EmailPromotion == 1)
                    .AsQueryable();

                List<int> searchList = new List<int> { 1, 3, 5 };
                var query3 = context.Person.Where(x => searchList.Contains(x.BusinessEntityId));
                _ = context.Person.Where(x => x.LastName.Contains("UF"));
                _ = context.Person.Where(x => EF.Functions.Like(x.LastName, "%UF%"));

                //IsDate translates to the TSQL IsDate function 
                _ = context.Person.Where(x => EF.Functions.IsDate(x.ModifiedDate.ToString()));
                decimal sum = context.Product.Sum(x => x.ListPrice);
                int count = context.Product.Count(x => x.ListPrice != 0);
                decimal avg = context.Product.Average(x => (decimal?)x.ListPrice) ?? 0;
                decimal max = context.Product.Max(x => (decimal?)x.ListPrice) ?? 0;
                decimal min = context.Product.Min(x => (decimal?)x.ListPrice) ?? 0;
                bool any = context.Product.Any(x => x.ListPrice != 0);
                bool all = context.Product.All(x => x.ListPrice != 0);

                //Complex PK with immediate execution
                var productVendor = context.ProductVendor.Find(2, 1688);
                
                Console.WriteLine($"productVendor: {productVendor.BusinessEntityId} " +
                    $"{productVendor.ProductId} {productVendor.LastReceiptCost}");

                //All immediate execution
                //NOTE: should use an order by with these
                _ = context.Person.Where(x => x.BusinessEntityId == 1).FirstOrDefault();
                _ = context.Person.FirstOrDefault(x => x.BusinessEntityId == 1);
                //Using Single - Exception if more than one is found
                _ = context.Person.SingleOrDefault(x => x.BusinessEntityId == 1);

                //Using paging
                var prodList = context.Person
                    .Where(x => x.PersonType == "em")
                    .OrderBy(x => x.EmailPromotion)
                    .Skip(25).Take(50);

                IOrderedQueryable<DataLibrary.Adventureworks.Models.Person> query1 =
                    context.Person.OrderBy(x => x.PersonType).ThenBy(x => x.EmailPromotion);
                IOrderedQueryable<DataLibrary.Adventureworks.Models.Person> query2 =
                    context.Person.OrderByDescending(x => x.PersonType).ThenBy(x => x.EmailPromotion);

                var person1 = query.OrderBy(c => c.BusinessEntity).FirstOrDefault();
                var person2 = query.SingleOrDefault(x => x.BusinessEntityId == 1);
                var person3 = context.Person.Find(1);

                Console.WriteLine($"person1: {person1.FirstName} {person1.LastName}");
                Console.WriteLine($"person2: {person2.FirstName} {person2.LastName}");
                Console.WriteLine($"person3: {person3.FirstName} {person3.LastName}");

                //Now query executes
                List<DataLibrary.Adventureworks.Models.Person> list = query.ToList();

                foreach (var p in list)
                {
                    Console.WriteLine($"{p.BusinessEntity} {p.FirstName} {p.LastName}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}, InnerException: {ex?.InnerException?.Message}");
            }
        }


        private void DisplayCustomers(string searchString = default)
        {
            var builder = new DbContextOptionsBuilder<BankContext>();
            builder.UseSqlServer("Server=(local);Database=BankDemo;Integrated Security=true;");
            builder.UseLoggerFactory(_loggerFactory);

            try
            {
                using (BankContext context = new BankContext(builder.Options))
                {
                    IQueryable<DataLibrary.BankDemo.Models.Customer> customers = context.Customers;

                    if (!String.IsNullOrEmpty(searchString))
                    {
                        //Method is translated to database functions, so selection is done on the SQL server
                        //An implementation of the SQL LIKE operation. On relational databases this is usually directly translated to SQL.
                        //https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.dbfunctionsextensions.like?view=efcore-3.1
                        customers = customers.Where(c => EF.Functions.Like(c.FirstName, $"%{searchString}%") || 
                        EF.Functions.Like(c.LastName, $"%{searchString}%"));
                    }

                    customers = customers.OrderBy(c => c.FirstName)
                        .ThenBy(c => c.LastName);

                    foreach(var c in customers)
                    {
                        Console.WriteLine($"{c.Id} {c.FirstName} {c.LastName}");
                    }

                    Console.WriteLine($"\n\nClient side filtering.");
                    //In contrast to SQL server filetering we can also use c# function filtering
                    //Client Filtering, all records are retrieved from the database  
                    //each record is then selected using a predicate
                    SearchString = searchString;
                    //Exception: The LINQ expression 'DbSet<Customer>
                    //.Where(c => !(c.IsDeleted))
                    //.Where(c => EFExamples.CustomerFilter(c))' could not be translated. 
                    //Either rewrite the query in a form that can be translated, or switch 
                    //to client evaluation explicitly by inserting a call to either AsEnumerable(), 
                    //AsAsyncEnumerable(), ToList(), or ToListAsync(). 
                    //See https://go.microsoft.com/fwlink/?linkid=2101038 for more information., InnerException:
                    var _customers = context.Customers
                        .AsEnumerable()//Retrieves all the customers from DB, client side filtering 
                        .Where(c => CustomerFilter(c))
                        .OrderBy(c => c.FirstName)
                        .ThenBy(c => c.LastName);

                    foreach (var c in _customers)
                    {
                        Console.WriteLine($"{c.Id} {c.FirstName} {c.LastName}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}, InnerException: {ex?.InnerException?.Message}");
            }
        }

        public string SearchString { get; private set; }

        private bool CustomerFilter(DataLibrary.BankDemo.Models.Customer customer)
        {
            return customer.FirstName.Contains(SearchString) || 
                customer.LastName.Contains(SearchString);
        }

        private void ExecuteTestcases(DataLibrary.BlogDemo.Data.BloggingContext context)
        {
            Console.WriteLine($"Blogs.Count: {context.Blogs.Count()}");

            var blog = new DataLibrary.BlogDemo.Models.Blog
            {
                Name = "@@@@Aljazeera arabic new",
                Url = "@@@@http://aljazeera.com"
            };

            context.Blogs.Add(blog);
            context.SaveChanges();

            blog = new DataLibrary.BlogDemo.Models.Blog
            {
                Name = "@@@@BBC world",
                Url = "@@@@http://bbc.com"
            };

            context.Blogs.Add(blog);
            context.SaveChanges();

            Console.WriteLine($"Blogs.Count: {context.Blogs.Count()}");

            var blogs = context.Blogs.OrderBy(b => b.Url).ToList();
            foreach (var b in blogs)
            {
                Console.WriteLine($"{b.Name} ({b.Url}) IsDirty = {b.IsDirty}");
            }

            //throw new Exception("Simulated exception to rollback transaction");
        }

        //Transient Database errors
        //You should turn on EnableRetryOnFailure() on each database context
        //Connection resiliency, is the ability to retry connections when certain transient errors occure
        //For example no database connection available from connection pool, not a problem if you try later
        //Retry on failure is default turned off, you need to specify it explicit in the DBContext
        //Note to simulate Transient Error Handling, comment out the OnConfiguring method in BloggingContext   
        private void SimulateTransientErrorHandling()
        {
            //SetupDatabase()
            DataLibrary.BlogDemo.Data.BloggingContext db = SetUpContext();

            using (db)
            {
                var blog = new DataLibrary.BlogDemo.Models.Blog 
                { 
                    Name = "World news leader", 
                    Url = "http://cnn.com" 
                };
                
                db.Add(blog);

                try
                {
                    db.SaveChanges();
                    //If you use retry strategy, you need to use the following Transaction strategy.  
                    //ExecuteInATransaction(db);
                }
                //Retry limit exceeded! Maximum number of retries (5) exceeded while executing database 
                //operations with 'CustomExecutionStrategy'. See inner exception for the most recent failure.
                catch (RetryLimitExceededException ex)
                {
                    //A retry limit error occurred
                    //Should handle intelligently
                    Console.WriteLine($"Retry limit exceeded! {ex.Message}");
                }
                catch (Exception ex)
                {
                    //Should handle intelligently
                    Console.WriteLine(ex);
                    throw;
                }
            }
        }

        //https://github.com/skimedic/presentations/blob/master/DOTNETCORE/Channel9_EFCoreShows/EntityFrameworkCoreExamples/ConnectionResiliency/Program.cs
        //https://www.youtube.com/watch?v=-HGG6BAwd_A
        private static void ExecuteInATransaction(DataLibrary.BlogDemo.Data.BloggingContext context,
            Action<DataLibrary.BlogDemo.Data.BloggingContext> MethodToExecute = null)
        {
            IExecutionStrategy strategy = context.Database.CreateExecutionStrategy();
            strategy.Execute(() =>
            {
                using var transaction = context.Database.BeginTransaction();
                try
                {
                    MethodToExecute?.Invoke(context);
                    //Do Work
                    transaction.Commit();
                    Console.WriteLine("transaction.Commit()");
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    Console.WriteLine("transaction.Rollback()");
                }
            });
        }

        private static DataLibrary.BlogDemo.Data.BloggingContext SetUpContext()
        {
            var contextOptionsBuilder = new DbContextOptionsBuilder<DataLibrary.BlogDemo.Data.BloggingContext>();
            var connectionString =
                @"Server=(local);Database=Demo.Blog;User Id=myUsername;Password=myPassword;MultipleActiveResultSets=true;";
            //@"Server =.\dev2019;Database=SpyStore;user id=foo;password=bar;MultipleActiveResultSets=true;";
            //contextOptionsBuilder.UseSqlServer(connectionString,
            //    o => o.EnableRetryOnFailure());
            contextOptionsBuilder.UseSqlServer(connectionString,
                o => o.ExecutionStrategy(c => 
                new DataLibrary.BlogDemo.Helpers.CustomExecutionStrategy(c, 5, new TimeSpan(0, 0, 0, 0, 30))));
            return new DataLibrary.BlogDemo.Data.BloggingContext(contextOptionsBuilder.Options);
        }

        private void UpdateBlogDB(string name = ".NET Musings", string url = "http://www.skimedic.com")
        {
            using (var db = new DataLibrary.BlogDemo.Data.BloggingContext())
            {
                Console.WriteLine("Setting properties through C#");
                var blog = new DataLibrary.BlogDemo.Models.Blog { Name = name };
                blog.Url = url;

                db.Blogs.Add(blog);
                db.SaveChanges();
            }

            using (var db = new DataLibrary.BlogDemo.Data.BloggingContext())
            {
                Console.WriteLine("Setting properties through EF materialization");
                var blogs = db.Blogs.OrderBy(b => b.Url).ToList();
                foreach (var b in blogs)
                {
                    Console.WriteLine($"{b.Name} ({b.Url}) IsDirty = {b.IsDirty}");
                }
            }
        }

        private static void SetupDatabase()
        {
            using (var db = new DataLibrary.BlogDemo.Data.BloggingContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }
        }

        //To use UDF User Defined Functions 
        //1) Create Tables
        //2) Create UDFs 
        //3) Add Computed column using the defined UDF in step 2         
        //You can reference SQL Server functions by "mocking" the function in c# (in the DBContext class)      
        private static void UpdateRecords()
        {
            using var db = new DataLibrary.ShopDemo.Data.ShopContext();
            var orderDetails = db.OrderDetails.ToList();
            orderDetails.ForEach(x => x.Quantity++);
            db.SaveChanges();
        }
        private static void GetRecordsOver(decimal amount)
        {
            using var db = new DataLibrary.ShopDemo.Data.ShopContext();
            foreach (var order in db.Orders
                .Where(x => DataLibrary.ShopDemo.Data.ShopContext.GetOrderTotal(x.Id) > amount))
            {
                Console.WriteLine($"Qty: {order.CustomerId}, Total: {order.OrderTotal}");
            }
        }
        private static void DisplayOrderAndDetails()
        {
            using var db = new DataLibrary.ShopDemo.Data.ShopContext();
            var order = db.Orders.FirstOrDefault();
            Console.WriteLine($"Order Total:{order.OrderTotal}");
            db.Entry(order).Collection(o => o.OrderDetails).Load();
            Console.WriteLine($"Qty: {order.CustomerId}, Total: {order.OrderTotal}");
            order.OrderDetails.ForEach(x =>
            {
                Console.WriteLine($"Detail: Qty: {x.Quantity}, Cost: {x.UnitCost} = Total: {x.LineItemTotal}");
            });
        }

        private static void SetupShopDatabase()
        {
            using (var db = new DataLibrary.ShopDemo.Data.ShopContext())
            {
                db.Database.EnsureDeleted();
                db.Database.Migrate();
                var order = new DataLibrary.ShopDemo.Models.Order
                {
                    CustomerId = 1,
                    OrderDate = DateTime.Now.Subtract(new TimeSpan(20, 0, 0, 0)),
                    ShipDate = DateTime.Now.Subtract(new TimeSpan(5, 0, 0, 0)),
                };
                var details = new List<DataLibrary.ShopDemo.Models.OrderDetail>
                {
                    new DataLibrary.ShopDemo.Models.OrderDetail() {Order = order, Quantity = 3, UnitCost = 12.99M},
                    new DataLibrary.ShopDemo.Models.OrderDetail() {Order = order, Quantity = 2, UnitCost = 39.99M},
                };
                order.OrderDetails = details;
                db.Orders.Add(order);
                db.SaveChanges();
            }
        }

        //By using a Timestamp we can avoid lost updates.
        //The user can be informed in case of concurrency problems such as updating the
        //same row or field.
        private void ConcurrencyExample()
        {
            var builder = new DbContextOptionsBuilder<BankContext>();
            builder.UseSqlServer("Server=(local);Database=BankDemo;Integrated Security=true;");
            builder.UseLoggerFactory(_loggerFactory);

            using BankContext context = new BankContext(builder.Options);
            try
            {
                //using (BankContext context = new BankContext(builder.Options))
                //{
                var customer1 = context.Customers
                    .OrderBy(c => c.LastName)
                    .Take(1)
                    .FirstOrDefault();

                var customer2 = context.Customers
                    .OrderBy(c => c.LastName)
                    .Skip(1)
                    .Take(1)
                    .FirstOrDefault();

                //Percy Bahringer, id: 59, 00 00 00 00 00 00 08 19
                //Jan Bailey, id: 13, 00 00 00 00 00 00 07 EA
                string timeStamp = BitConverter.ToString(customer1.TimeStamp).Replace("-", " ");
                //string timeStamp = Encoding.UTF8.GetString(customer1.TimeStamp, 0, 
                //    customer1.TimeStamp.Length);
                Console.WriteLine($"{customer1.FullName}, id: {customer1.Id}, " +
                    $"{timeStamp}");

                timeStamp = BitConverter.ToString(customer2.TimeStamp);//.Replace("-", " ");
                //timeStamp = Encoding.UTF8.GetString(customer2.TimeStamp, 0,
                //customer2.TimeStamp.Length);
                Console.WriteLine($"{customer2.FullName}, id: {customer2.Id}, " +
                    $"{timeStamp}");
                //}

                //change values outside of current context, you could do the same by going to DB and change value
                //and then continue with the code by using for example Console.ReadKey();
                context.Database
                    .ExecuteSqlInterpolated($"Update [dbo].[Customers] set LastName = 'Bahringer##' where Id = {customer1.Id};");

                //Causes concurrency Exception because previous call changed the Row Timestamp   
                customer1.LastName = "Bahringer@@";
                //Record is changed without any problems
                customer2.LastName = "Bailey@@";
                context.SaveChanges();
            }
            catch(DbUpdateConcurrencyException ex)
            {
                //Database operation expected to affect 1 row(s) but actually affected 0 row(s). 
                //Data may have been modified or deleted since entities were loaded. 
                //See http://go.microsoft.com/fwlink/?LinkId=527962 for information on 
                //understanding and handling optimistic concurrency exceptions.
                Console.WriteLine(ex.Message);
                EntityEntry entryEntity = ex.Entries[0];
                //Kept in DbChangeTracker
                PropertyValues originalValues = entryEntity.OriginalValues;
                PropertyValues currentValues = entryEntity.CurrentValues;
                IEnumerable<PropertyEntry> modifiedEntries =
                    entryEntity.Properties.Where(e => e.IsModified);
                foreach (var itm in modifiedEntries)
                {
                    //Console.WriteLine($"{itm.Metadata.Name},");
                }
                //Needs to call to database to get values
                PropertyValues databaseValues = entryEntity.GetDatabaseValues();
                //Discards local changes, gets database values, resets change tracker
                entryEntity.Reload();
                //logging stuff here
                //throw new AppDbUpdateException(ex,IList<Customer>)
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}, InnerException: {ex?.InnerException.Message}");
            }
        }

        private void DisplayCarDealershipsDB()
        {
            try
            {
                using var context = new DataLibrary.CarDealerships.Data
                    .CarDealershipsContextFactory().CreateDbContext(new string[0]);

                var customerOrders = context.CustomerOrderViewModels;
                foreach(DataLibrary.CarDealerships.Models.ViewModels.CustomerOrderViewModel 
                    customerOrder in customerOrders)
                {
                    Console.WriteLine(customerOrder);
                }

                var orders = context.Orders
                    .Include(o => o.CustomerNavigation)
                        .ThenInclude(c => c.CreditRisks)
                    .Include(o => o.CarNavigation)
                        .ThenInclude(c => c.MakeNavigation);
                //.ToList();

                //https://docs.microsoft.com/en-us/ef/core/what-is-new/ef-core-5.0/whatsnew
                //https://gunnarpeipman.com/ef-core-toquerystring/
                //https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.extensions.querybuilder.toquerystring?view=aspnetcore-3.1
                //https://www.nuget.org/packages/Microsoft.AspNetCore.Http.Extensions/
                //var sql = orders.ToQueryString();

                foreach (DataLibrary.CarDealerships.Models.Order o in orders)
                {
                    Console.WriteLine($"#Order id: {o.Id}, Customer: {o.CustomerNavigation.FullName}, " +
                        $"Brand: {o.CarNavigation?.MakeNavigation?.Name}, Color: {o.CarNavigation.Color}");
                    //Show CreditRisks
                    foreach(DataLibrary.CarDealerships.Models.CreditRisk c in o.CustomerNavigation.CreditRisks)
                    {
                        Console.WriteLine($"\tCreditRisk id: {c.Id}");
                    }
                }

                Console.WriteLine($"\n\n");

                var customers = context.Customers
                    .Include(c => c.CreditRisks)
                    .ToList();

                foreach (DataLibrary.CarDealerships.Models.Customer c in customers)
                {
                    Console.WriteLine($"Customer id: {c.Id}, FullName: {c.FullName}");
                    //Show CreditRisks
                    foreach (DataLibrary.CarDealerships.Models.CreditRisk cr in c.CreditRisks)
                    {
                        Console.WriteLine($"\tCreditRisk id: {cr.Id}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}, InnerException: {ex?.InnerException.Message}");
                //throw;
            }
        }

        private void SeedCarDealershipsDB()
        {
            using var context = new DataLibrary.CarDealerships.Data
                .CarDealershipsContextFactory().CreateDbContext(new string[0]);

            DataLibrary.CarDealerships.Data.DbInitializer.Initialize(context);
        }

        private void SeedBankDB()
        {
            var builder = new DbContextOptionsBuilder<BankContext>();
            builder.UseSqlServer("Server=(local);Database=BankDemo;Integrated Security=true;");
            //builder.UseLoggerFactory(_loggerFactory);

            try
            {
                var customers = new BankRepository().GetCustomers();
                //DisplayCustomers(customers);
                //using (TransactionScope scope = new TransactionScope())
                //{
                Console.WriteLine("Seeding Bank DataBase...");
                using (BankContext context = new BankContext(builder.Options))
                {
                    Console.WriteLine("Deleting Bank DataBase database...");
                    context.Database.EnsureDeleted();
                    Console.WriteLine("Applying migrations to Bank DataBase database...");
                    context.Database.Migrate();
                    //Migrate() is mutual exclusive with EnsureCreated()  
                    //context.Database.EnsureCreated();

                    context.Customers.AddRange(customers);
                    context.SaveChanges();
                }
                //scope.Complete();
                Console.WriteLine("Finished seeding Bank DataBase...");
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}, InnerException: {ex?.InnerException.Message}");
                //throw;
            }        
        }

        private void BankDemo()
        {
            var builder = new DbContextOptionsBuilder<BankContext>();
            builder.UseSqlServer("Server=(local);Database=BankDemo;Integrated Security=true;");
            //builder.UseLoggerFactory(_loggerFactory);

            try
            {
                using (BankContext context = new BankContext(builder.Options))
                {
                    //Seed the BankDemo DataBase 
                    //DataLibrary.BankDemo.Data.DbInitializer.Initialize(context);

                    //If record does not exists add it
                    if(!context.Customers
                        .Where(c => c.FirstName == "Donald" && c.LastName == "Duck")
                        .Any())
                    {                    
                        //Add customer record
                        DataLibrary.BankDemo.Models.Customer customer =
                            new DataLibrary.BankDemo.Models.Customer()
                            {
                                FirstName = "Donald",
                                LastName = "Duck"
                            };

                        context.Customers.Add(customer);
                        context.SaveChanges();
                    }

                    //We expect just one record, hence Single() 
                    DataLibrary.BankDemo.Models.Customer donald = context.Customers
                        .Where(c => c.FirstName == "Donald" && c.LastName == "Duck")
                        .Include(c => c.Addresses)
                        //Same as above only written different 
                        .Include(nameof(DataLibrary.BankDemo.Models.Customer.Accounts))
                        .Single();

                    //If no Addresses specified we add some
                    if (donald.Addresses.Count == 0 )
                    {
                        Console.WriteLine("Adding address of Donald.");
                        donald.Addresses = new List<DataLibrary.BankDemo.Models.Address>
                        {      
                            new DataLibrary.BankDemo.Models.Address()
                            {
                                Country="United States",
                                State="Washington",
                                City="Washington",
                                StreetAddress="2 15th St NW",
                                ZipCode="DC 20024",
                                CreationDate = DateTime.Now,
                            }                                
                        };
                        //Because of change tracking we can just save the changes
                        context.SaveChanges();
                    }

                    //If no Accounts specified we add some
                    if(donald.Accounts.Count == 0)
                    {
                        Console.WriteLine("Adding Account of Donald.");
                        donald.Accounts = new List<DataLibrary.BankDemo.Models.Account>
                        {
                            new DataLibrary.BankDemo.Models.Account("NLABNAGHIJ0123456789", 670000)
                            {
                                Type = DataLibrary.BankDemo.Models.AccountType.Savings,
                                //Note in order to add Transactions and store them in the 
                                //database we dont have to use the include(Transactions) in the context above    
                                Transactions = new List<DataLibrary.BankDemo.Models.Transaction>
                                {
                                    new DataLibrary.BankDemo.Models.Transaction(
                                        DataLibrary.BankDemo.Models.TransactionType.Deposit,500,
                                        DateTime.Now,"Added money to Savings account"),
                                    new DataLibrary.BankDemo.Models.Transaction(
                                        DataLibrary.BankDemo.Models.TransactionType.Withdrawal,900,
                                        DateTime.Now,"Hush money"),
                                    new DataLibrary.BankDemo.Models.Transaction(
                                        DataLibrary.BankDemo.Models.TransactionType.Deposit,800,
                                        DateTime.Now,"Added lottery money won."),
                                }
                            }
                        };
                        //Because of change tracking we can just save the changes
                        context.SaveChanges();
                    }

                    //Using eager loading to load all related data  
                    var customers = context.Customers
                        .Include(c => c.Addresses)
                        .Include(c => c.EmailAddresses)
                        .Include(c => c.Accounts)
                            .ThenInclude(a => a.Transactions)
                        .AsEnumerable();

                    DisplayCustomers(customers);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"InitBankDemo, Exception: {ex.Message}");
            }
        }

        private void DisplayCustomers(IEnumerable<DataLibrary.BankDemo.Models.Customer> customers)
        {
            foreach (DataLibrary.BankDemo.Models.Customer c in customers)
            {
                Console.WriteLine($"Customer, FirstName: {c.FirstName}, LastName: {c.LastName}");

                foreach (DataLibrary.BankDemo.Models.Email e in c.EmailAddresses)
                {
                    Console.WriteLine($"\tEmail, EmailAddress: {e.EmailAddress}, " +
                        $"CreationDate: {e.CreationDate}");
                }

                foreach (DataLibrary.BankDemo.Models.Address a in c.Addresses)
                {
                    Console.WriteLine($"\tAddress, StreetAddress: {a.StreetAddress}");
                }

                foreach (DataLibrary.BankDemo.Models.Account account in c.Accounts)
                {
                    Console.WriteLine($"\tAccount, Number: {account.Number}, " +
                        $"Type: {account.Type}, Balance: {account.Balance}");
                    foreach (DataLibrary.BankDemo.Models.Transaction t in account.Transactions)
                    {
                        Console.WriteLine($"\t\tTransaction, Description: {t.Description} Type: " +
                            $"{t.Type} Amount: {t.Amount}");
                    }
                }
            }
        }

        private void NewOfficeAssignment()
        {
            try
            {
                using (SchoolContext context = new SchoolContext())
                {
                    Instructor instructor = context.Instructors
                        .Where(i => i.Id == 1).Single();

                    instructor.OfficeAssignment =
                        new OfficeAssignment()
                        {
                            Location = "Wallstreet 2048"
                        };

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                Console.WriteLine($"Exception: {ex.InnerException.Message}");
                //throw;
            }
        }

        public void DisplayInstructor()
        {
            using (SchoolContext context = new SchoolContext())
            {
                List<Instructor> instructors = context.Instructors
                    .Include(i => i.OfficeAssignment)
                    .ToList();

                foreach (Instructor i in instructors)
                {
                    Console.WriteLine($"Instructor: {i.FullName}, " +
                        $"OfficeAssignment: {i?.OfficeAssignment.Location}");
                }
            }
        }

        public void AddOfficeAssignment()
        {
            using (SchoolContext context = new SchoolContext())
            {
                //var instructors = context.Instructors
                //    .Where(i => i.OfficeAssignment == null);

                //foreach(Instructor i in instructors)
                //{
                //    //1 Zheng, Roger
                //    //5 Abercrombie, Kim
                //    Console.WriteLine($"{i.Id} {i.FullName}");
                //}

                Instructor instructor = context.Instructors
                    .Where(i => i.Id == 1).Single();

                instructor.OfficeAssignment =
                    new OfficeAssignment()
                    {
                        Location = "Mainstreet 1024"
                    };

                Instructor instructor2 = context.Instructors
                    .Where(i => i.Id == 5).Single();

                instructor2.OfficeAssignment =
                    new OfficeAssignment()
                    {
                        Location = "Greenwhich avenue 255"
                    };

                context.SaveChanges();
            }
        }

        public void DisplayStudentsAndInstructor()
        {
            using (SchoolContext context = new SchoolContext())
            {
                foreach(Student s in context.Students)
                {
                    Console.WriteLine($"Student: {s.FullName}");
                }

                foreach (Instructor i in context.Instructors)
                {
                    Console.WriteLine($"Instructor: {i.FullName}");
                }
            }
        }

        private void DisplayCourses()
        {
            using (SchoolContext context = new SchoolContext())
            {
                var courses = context.Courses
                    .Include(c => c.DepartmentNavigation)
                    .Include(c => c.Enrollments)
                        .ThenInclude(e => e.Student)
                    .Include(c => c.CourseAssignments)
                        .ThenInclude(c => c.Instructor)
                    .OrderBy(c => c.Title);

                foreach(Course c in courses)
                {
                    Console.WriteLine($"Course: {c.Title}");

                    Console.WriteLine($"\tInstructors:");                    
                    foreach(CourseAssignment ca in c.CourseAssignments)
                    {
                        Console.WriteLine($"\t\tInstructor: {ca.Instructor.PersonalInformation.FullName}");                        
                    }

                    Console.WriteLine($"\tEnrollments:");                    
                    foreach(Enrollment e in c.Enrollments)
                    {
                        Console.WriteLine($"\t\tStudent: {e.Student.PersonalInformation.FullName}, " +
                            $"Grade: {e?.Grade}");
                    }
                }
            }
        }

        private void DisplayDepartments()
        {
            using (SchoolContext context = new SchoolContext())
            {
                var departments = context.Departments
                    .Include(nameof(Department.Administrator)) //Instead of lambda we can use nameof 
                    .Include(nameof(Department.Courses));
            
                foreach(Department d in departments)
                {
                    Console.WriteLine($"Department name: {d.Name}, Administrator: {d.Administrator.PersonalInformation.FullName}");
                    foreach(Course c in d.Courses)
                    {
                        Console.WriteLine($"\tCourse: {c.Title}");
                    }
                }            
            }
        }

        private void DisplayInstructors()
        {
            try
            {
                using (SchoolContext context = new SchoolContext())
                {
                    var instructors = context.Instructors
                        .Include(i => i.OfficeAssignment)
                        .Include(i => i.CourseAssignments)
                            .ThenInclude(c => c.Course)
                                .ThenInclude(c => c.DepartmentNavigation)
                        .OrderBy(i => i.PersonalInformation.LastName)
                        .ToList();

                    foreach (Instructor i in instructors)
                    {
                        Console.WriteLine($"{i.PersonalInformation.FullName}, " +
                            $"Office: {i.OfficeAssignment?.Location}");
                        foreach (CourseAssignment c in i.CourseAssignments)
                        {
                            Console.WriteLine($"\tCourse: {c.Course.Title}, " +
                                $"Department: {c.Course.DepartmentNavigation.Name}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception {ex.Message}");
                if(ex.InnerException != null)
                    Console.WriteLine($"Exception {ex.InnerException.Message}");
                //throw;
            }
        }

        private void OverviewStudentsV2()
        {
            string query = "SELECT EnrollmentDate, COUNT(*) AS StudentCount "
                + "FROM Student "
                + "GROUP BY EnrollmentDate";

            using (SchoolContext context = new SchoolContext())
            {
                List<EnrollmentViewModel> enrollments = context.EnrollmentViewModels
                    .FromSqlRaw(query)
                    .AsNoTracking()
                    .ToList();

                foreach(EnrollmentViewModel enrollment in enrollments)
                {
                    Console.WriteLine($"{enrollment.EnrollmentDate?.ToShortDateString()} " +
                        $"{enrollment.StudentCount}");
                }
            }
        }

        private void OverviewStudents()
        {
            using (SchoolContext context = new SchoolContext())
            {
                var students = from student in context.Students
                               group student by student.EnrollmentDate into EnrollmentDateGroup
                               orderby EnrollmentDateGroup.Key descending
                               select new { EnrollmentDate = EnrollmentDateGroup.Key, 
                                   Count = EnrollmentDateGroup.Count() }; 
                    
                foreach(var s in students)
                {
                    Console.WriteLine($"{s.EnrollmentDate.ToShortDateString()} {s.Count}");
                }
            }
        }

        private void DisplayStudents()
        {
            using (SchoolContext context = new SchoolContext())
            {
                foreach(Student s in context.Students.AsNoTracking())
                {
                    Console.WriteLine(s);
                }

                //Display Student and his Enrollments
                var students = context.Students
                    .Include(s => s.Enrollments) //Eager loading
                        .ThenInclude(e => e.Course).AsNoTracking(); //Eager loading

                Console.WriteLine("\n\n");
                foreach (Student s in students)
                {
                    Console.WriteLine(s.PersonalInformation.FullName);
                    foreach(Enrollment e in s.Enrollments)
                    {
                        Console.WriteLine($"\tCourse: {e.Course.Title}, Grade: {e.Grade}");
                    }
                }
            }
        }

        private void GlobalQueryFiltersV2()
        {
            using (SchoolContext context = new SchoolContext())
            {
                //QueryFilters are also applied on raw queries 
                List<Student> students =  context.Students
                    .FromSqlRaw("select * from Student")
                    .ToList();

                foreach (DataLibrary.ContosoUniversity.Models.Student student in students)
                {
                    Console.WriteLine($"{student.PersonalInformation.FullName} " +
                        $"{student.PersonalInformation.DateOfBirth.ToShortDateString()} {student.PersonalInformation.Age} " +
                        $"{student.EnrollmentDate.ToShortDateString()}");
                }

                //We can ignore QueryFilters using IgnoreQueryFilters() 
                List<Student> _students = context.Students
                    .FromSqlRaw("select * from Student")
                    .IgnoreQueryFilters()
                    .ToList();

                Console.WriteLine("\n\n");
                foreach (DataLibrary.ContosoUniversity.Models.Student student in _students)
                {
                    Console.WriteLine($"{student.PersonalInformation.FullName} " +
                        $"{student.PersonalInformation.DateOfBirth.ToShortDateString()} {student.PersonalInformation.Age} " +
                        $"{student.EnrollmentDate.ToShortDateString()}");
                }
            }
        }

        private void GlobalQueryFilters()
        {
            using (SchoolContext context = new SchoolContext())
            {

                foreach (DataLibrary.ContosoUniversity.Models.Student student in context.Students)
                {
                    Console.WriteLine($"{student.PersonalInformation.FullName} " +
                        $"{student.PersonalInformation.DateOfBirth.ToShortDateString()} {student.PersonalInformation.Age} " +
                        $"{student.EnrollmentDate.ToShortDateString()}");
                }

                Student _student = context.Students
                    .Where(s => s.PersonalInformation.FirstMidName == "Nino")
                    .FirstOrDefault();

                //If student found set to deleted. 
                if(_student != default(Student))
                {
                    _student.IsDeleted = true;
                    context.SaveChanges();
                }

                Console.WriteLine("\n\n");
                foreach (DataLibrary.ContosoUniversity.Models.Student student in context.Students)
                {
                    Console.WriteLine($"{student.PersonalInformation.FullName} " +
                        $"{student.PersonalInformation.DateOfBirth.ToShortDateString()} {student.PersonalInformation.Age} " +
                        $"{student.EnrollmentDate.ToShortDateString()}");
                }
            }
        }

        private void DisplaySchoolContext()
        {
            try
            {
                using (SchoolContext context = new SchoolContext())
                {
                    foreach(DataLibrary.ContosoUniversity.Models.Student student in  context.Students)
                    {
                        Console.WriteLine($"{student.PersonalInformation.FullName} " +
                            $"{student.PersonalInformation.DateOfBirth.ToShortDateString()} {student.PersonalInformation.Age} " +
                            $"{student.EnrollmentDate.ToShortDateString()}"); 
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                //throw;
            }
        }

        private void PopulateSchoolContext()
        {
            try
            {
                using (SchoolContext context = new SchoolContext())
                {
                    Console.WriteLine("Start: PopulateSchoolContext");
                    DataLibrary.ContosoUniversity.Data.DbInitializer.Initialize(context);
                    Console.WriteLine("Finished: PopulateSchoolContext");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                //throw;
            }
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
