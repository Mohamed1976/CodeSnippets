using EFCoreDBDemo.Exercises;
using EFCoreDBDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EFCoreDBDemo
{
    /// <summary>
    /// https://vmsdurano.com/getting-started-with-entity-framework-core-database-first-development/
    /// https://www.learnentityframeworkcore.com/walkthroughs/existing-database
    /// We need to install the following packages:
    /// Microsoft.EntityFrameworkCore.SqlServer
    /// Microsoft.EntityFrameworkCore.Tools
    /// Microsoft.EntityFrameworkCore.SqlServer.Design
    /// 
    /// Go to Tools –> NuGet Package Manager –> Package Manager Console
    /// And then run the following command below to create a model from the existing database:
    /// Scaffold-DbContext "Server=(local);Database=AdventureWorksLT2017;Integrated Security=true;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models 
    /// 
    /// PM> Scaffold-DbContext "Server=(local);Database=AdventureWorksLT2017;Integrated Security=true;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models
    /// Build started...
    /// Build succeeded.
    /// The column 'SalesLT.SalesOrderHeader.OnlineOrderFlag' would normally be mapped to a non-nullable 
    /// bool property, but it has a default constraint.Such a column is mapped to a nullable bool property 
    /// to allow a difference between setting the property to false and invoking the default constraint.
    /// See https://go.microsoft.com/fwlink/?linkid=851278 for details.
    /// 
    /// The DbContext class will take the name of the database plus "Context", You can override this using the -c or --context option e.g.
    /// -c "AdventureContext"
    /// 
    /// </summary>

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                WCFExamples wCFExamples = new WCFExamples();
                wCFExamples.Run().Wait();

                EFExamples eFExamples = new EFExamples();
                eFExamples.Run().Wait();

                LinqToXmlExamples linqToXmlExamples = new LinqToXmlExamples();
                linqToXmlExamples.Run().Wait();

                DBPerformance dBPerformance = new DBPerformance();
                dBPerformance.Run().Wait();

                LinqExamples linqExamples = new LinqExamples();
                linqExamples.Run();

                AdoNetCodeExamples adoNetCodeExamples = new AdoNetCodeExamples();
                adoNetCodeExamples.Run();

                CacheExamples cacheExamples = new CacheExamples();
                cacheExamples.Run();

                TransactionExamples transactionExamples = new TransactionExamples();
                transactionExamples.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                //throw;
            }

            Console.ReadLine();
        }
    }
}
