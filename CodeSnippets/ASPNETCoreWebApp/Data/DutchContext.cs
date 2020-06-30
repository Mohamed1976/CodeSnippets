using ASPNETCoreWebApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCoreWebApp.Data
{
    public class DutchContext : DbContext
    {
        public DutchContext(DbContextOptions<DutchContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        //OrderItems are not included because we dont want to query
        //OrderItems directly, OrderItems are part of orders
        //In our demo we only display orderitems in context with order
    }
}

/*
 * Example how to use DB migrations and DB Updates
 * https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/migrations?view=aspnetcore-3.1
 * https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli
Microsoft Windows [Version 10.0.18362.836]
(c) 2019 Microsoft Corporation. All rights reserved.

C:\Users\moham>cd C:\CodeSnippets\ASPNETCoreWebApp
The system cannot find the path specified.

C:\Users\moham>cd C:\Users\moham\source\repos\CodeSnippets\CodeSnippets\ASPNETCoreWebApp

C:\Users\moham\source\repos\CodeSnippets\CodeSnippets\ASPNETCoreWebApp>dotnet-ef migrations add CurrencyConstraintDB --context DutchContext
Build started...
Build succeeded.
warn: Microsoft.EntityFrameworkCore.Model.Validation[30000]
      No type was specified for the decimal column 'UnitPrice' on entity type 'OrderItem'. This will cause values to be silently truncated if they do not fit in the default precision and scale. Explicitly specify the SQL server column type that can accommodate all the values using 'HasColumnType()'.
warn: Microsoft.EntityFrameworkCore.Model.Validation[30000]
      No type was specified for the decimal column 'Price' on entity type 'Product'. This will cause values to be silently truncated if they do not fit in the default precision and scale. Explicitly specify the SQL server column type that can accommodate all the values using 'HasColumnType()'.
Done. To undo this action, use 'ef migrations remove'

C:\Users\moham\source\repos\CodeSnippets\CodeSnippets\ASPNETCoreWebApp>dotnet-ef migrations add CurrencyConstraintDB --context DutchContext
Build started...
Build succeeded.
warn: Microsoft.EntityFrameworkCore.Model.Validation[30000]
      No type was specified for the decimal column 'UnitPrice' on entity type 'OrderItem'. This will cause values to be silently truncated if they do not fit in the default precision and scale. Explicitly specify the SQL server column type that can accommodate all the values using 'HasColumnType()'.
warn: Microsoft.EntityFrameworkCore.Model.Validation[30000]
      No type was specified for the decimal column 'Price' on entity type 'Product'. This will cause values to be silently truncated if they do not fit in the default precision and scale. Explicitly specify the SQL server column type that can accommodate all the values using 'HasColumnType()'.
The name 'CurrencyConstraintDB' is used by an existing migration.

C:\Users\moham\source\repos\CodeSnippets\CodeSnippets\ASPNETCoreWebApp>dotnet ef database drop --context DutchContext
Build started...
Build succeeded.
warn: Microsoft.EntityFrameworkCore.Model.Validation[30000]
      No type was specified for the decimal column 'UnitPrice' on entity type 'OrderItem'. This will cause values to be silently truncated if they do not fit in the default precision and scale. Explicitly specify the SQL server column type that can accommodate all the values using 'HasColumnType()'.
warn: Microsoft.EntityFrameworkCore.Model.Validation[30000]
      No type was specified for the decimal column 'Price' on entity type 'Product'. This will cause values to be silently truncated if they do not fit in the default precision and scale. Explicitly specify the SQL server column type that can accommodate all the values using 'HasColumnType()'.
Are you sure you want to drop the database 'DutchTreatDb' on server '(localdb)\mssqllocaldb'? (y/N)
y
Dropping database 'DutchTreatDb'.
Successfully dropped database 'DutchTreatDb'.

C:\Users\moham\source\repos\CodeSnippets\CodeSnippets\ASPNETCoreWebApp>dotnet-ef migrations add CurrencyConstraintDB --context DutchContext

C:\Users\moham\source\repos\CodeSnippets\CodeSnippets\ASPNETCoreWebApp>dotnet-ef migrations add InitialCreate --context DutchContext
Build started...
Build succeeded.
warn: Microsoft.EntityFrameworkCore.Model.Validation[30000]
      No type was specified for the decimal column 'UnitPrice' on entity type 'OrderItem'. This will cause values to be silently truncated if they do not fit in the default precision and scale. Explicitly specify the SQL server column type that can accommodate all the values using 'HasColumnType()'.
warn: Microsoft.EntityFrameworkCore.Model.Validation[30000]
      No type was specified for the decimal column 'Price' on entity type 'Product'. This will cause values to be silently truncated if they do not fit in the default precision and scale. Explicitly specify the SQL server column type that can accommodate all the values using 'HasColumnType()'.
Done. To undo this action, use 'ef migrations remove'

C:\Users\moham\source\repos\CodeSnippets\CodeSnippets\ASPNETCoreWebApp>dotnet-ef migrations add InitialCreate --context DutchContext

C:\Users\moham\source\repos\CodeSnippets\CodeSnippets\ASPNETCoreWebApp>dotnet-ef database update --context DutchContext
Build started...
Build succeeded.
warn: Microsoft.EntityFrameworkCore.Model.Validation[30000]
      No type was specified for the decimal column 'UnitPrice' on entity type 'OrderItem'. This will cause values to be silently truncated if they do not fit in the default precision and scale. Explicitly specify the SQL server column type that can accommodate all the values using 'HasColumnType()'.
warn: Microsoft.EntityFrameworkCore.Model.Validation[30000]
      No type was specified for the decimal column 'Price' on entity type 'Product'. This will cause values to be silently truncated if they do not fit in the default precision and scale. Explicitly specify the SQL server column type that can accommodate all the values using 'HasColumnType()'.
Done.

C:\Users\moham\source\repos\CodeSnippets\CodeSnippets\ASPNETCoreWebApp>dotnet-ef migrations add CurrencyConstraintDB2 --context DutchContext
Build started...
Build succeeded.
warn: Microsoft.EntityFrameworkCore.Model.Validation[30000]
      No type was specified for the decimal column 'UnitPrice' on entity type 'OrderItem'. This will cause values to be silently truncated if they do not fit in the default precision and scale. Explicitly specify the SQL server column type that can accommodate all the values using 'HasColumnType()'.
An operation was scaffolded that may result in the loss of data. Please review the migration for accuracy.
Done. To undo this action, use 'ef migrations remove'

C:\Users\moham\source\repos\CodeSnippets\CodeSnippets\ASPNETCoreWebApp>dotnet-ef database update --context DutchContext
Build started...
Build succeeded.
Done.

C:\Users\moham\source\repos\CodeSnippets\CodeSnippets\ASPNETCoreWebApp>dotnet-ef migrations add CurrencyConstraintDB3 --context DutchContext
Build started...
Build succeeded.
An operation was scaffolded that may result in the loss of data. Please review the migration for accuracy.
Done. To undo this action, use 'ef migrations remove'

C:\Users\moham\source\repos\CodeSnippets\CodeSnippets\ASPNETCoreWebApp>dotnet-ef database update --context DutchContext
Build started...
Build succeeded.
Done.

C:\Users\moham\source\repos\CodeSnippets\CodeSnippets\ASPNETCoreWebApp>

//Installed Nugets to use EntityFrameworkCore(DBContext)
//Microsoft.EntityFrameworkCore.SqlServer
//Microsoft.EntityFrameworkCore.Design

//To create database you need to install ef tool
//using cmd type: dotnet tool install dotnet-ef -g
//-g specifies it is installed globally
//You can invoke the tool using the following command in cmd: dotnet-ef
//Tool 'dotnet-ef' (version '3.1.3') was successfully installed.
//dotnet-ef --h
//Commands:
//database Commands to manage the database.
//dbcontext   Commands to manage DbContext types.
//migrations Commands to manage migrations.

//Create database with command in cmd: dotnet-ef database update --context DutchContext

//C:\CodeSnippets\ASPNETCoreWebApp>dotnet-ef database update --context DutchContext
//Build started...
//Build succeeded.
//warn: Microsoft.EntityFrameworkCore.Model.Validation[30000]
//      No type was specified for the decimal column 'UnitPrice' on entity type 'OrderItem'. This will cause values to be silently truncated if they do not fit in the default precision and scale.Explicitly specify the SQL server column type that can accommodate all the values using 'HasColumnType()'.
//warn: Microsoft.EntityFrameworkCore.Model.Validation[30000]
//      No type was specified for the decimal column 'Price' on entity type 'Product'. This will cause values to be silently truncated if they do not fit in the default precision and scale.Explicitly specify the SQL server column type that can accommodate all the values using 'HasColumnType()'.
//Done.

//Creates the database that can store our information
//C:\CodeSnippets\ASPNETCoreWebApp>dotnet-ef migrations add InitialDB --context DutchContext
//Build started...
//Build succeeded.
//warn: Microsoft.EntityFrameworkCore.Model.Validation[30000]
//      No type was specified for the decimal column 'UnitPrice' on entity type 'OrderItem'. This will cause values to be silently truncated if they do not fit in the default precision and scale.Explicitly specify the SQL server column type that can accommodate all the values using 'HasColumnType()'.
//warn: Microsoft.EntityFrameworkCore.Model.Validation[30000]
//      No type was specified for the decimal column 'Price' on entity type 'Product'. This will cause values to be silently truncated if they do not fit in the default precision and scale.Explicitly specify the SQL server column type that can accommodate all the values using 'HasColumnType()'.
//Done.To undo this action, use 'ef migrations remove'

//Migrations folder is created in root is moved to subdir Data

//Finally run update to update database, create tables
//C:\CodeSnippets\ASPNETCoreWebApp>dotnet-ef database update --context DutchContext
//Build started...
//Build succeeded.
//warn: Microsoft.EntityFrameworkCore.Model.Validation[30000]
//      No type was specified for the decimal column 'UnitPrice' on entity type 'OrderItem'. This will cause values to be silently truncated if they do not fit in the default precision and scale.Explicitly specify the SQL server column type that can accommodate all the values using 'HasColumnType()'.
//warn: Microsoft.EntityFrameworkCore.Model.Validation[30000]
//      No type was specified for the decimal column 'Price' on entity type 'Product'. This will cause values to be silently truncated if they do not fit in the default precision and scale.Explicitly specify the SQL server column type that can accommodate all the values using 'HasColumnType()'.
//Done.

//Defined [DataType(DataType.Currency)] [Range(1, 100)] on OrderItem and Product
//Created migration
//C:\Users\moham\source\repos\CodeSnippets\CodeSnippets\ASPNETCoreWebApp>dotnet-ef migrations add CurrencyConstraintDB --context DutchContext
//Build started...
//Build succeeded.
//warn: Microsoft.EntityFrameworkCore.Model.Validation[30000]
//      No type was specified for the decimal column 'UnitPrice' on entity type 'OrderItem'. This will cause values to be silently truncated if they do not fit in the default precision and scale.Explicitly specify the SQL server column type that can accommodate all the values using 'HasColumnType()'.
//warn: Microsoft.EntityFrameworkCore.Model.Validation[30000]
//      No type was specified for the decimal column 'Price' on entity type 'Product'. This will cause values to be silently truncated if they do not fit in the default precision and scale.Explicitly specify the SQL server column type that can accommodate all the values using 'HasColumnType()'.
//Done.To undo this action, use 'ef migrations remove'
 
*/
