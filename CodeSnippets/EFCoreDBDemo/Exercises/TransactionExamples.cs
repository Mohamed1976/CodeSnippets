using EFCoreDBDemo.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Transactions;

namespace EFCoreDBDemo.Exercises
{
    public class TransactionExamples
    {

        public void Run()
        {
            //TransactionScopeExample();
            //SqlTransactionExample();
            //EFTransactionExample();
        }

        private string connectionString = 
            "Server=(local);Database=AdventureWorksLT2017;Integrated Security=true;";

        //https://entityframework.net/how-ef-transaction-works
        public void EFTransactionExample()
        {
            try
            {
                using (AdventureWorksLT2017Context context = new AdventureWorksLT2017Context())
                {
                    List<ProductCategory> productCategories = new List<ProductCategory>()
                {
                    new ProductCategory() { Name =$"Test {Guid.NewGuid()}"},
                    new ProductCategory() { Name =$"Test {Guid.NewGuid()}"},
                    new ProductCategory() { Name =$"Test {Guid.NewGuid()}"}
                };

                    context.ProductCategory.AddRange(productCategories);
                    int entries = context.SaveChanges();
                    Console.WriteLine($"Entries written to DB: {entries}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                //throw;
            }
        }

        /* The following example creates a SqlConnection and a SqlTransaction. 
         * It also demonstrates how to use the BeginTransaction, Commit, and Rollback methods. 
         * The transaction is rolled back on any error, or if it is disposed without first being 
         * committed. Try/Catch error handling is used to handle any errors when attempting to 
         * commit or roll back the transaction.
         * https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqltransaction?view=dotnet-plat-ext-3.1
         * https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlconnection.begintransaction?view=dotnet-plat-ext-3.1
         * https://docs.microsoft.com/en-us/dotnet/api/microsoft.data.sqlclient.sqltransaction?view=sqlclient-dotnet-core-2.0
        */
        public void SqlTransactionExample()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;

                // Start a local transaction.
                transaction = connection.BeginTransaction("SampleTransaction");

                // Must assign both transaction object and connection
                // to Command object for a pending local transaction
                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    string sql = $"Insert into [SalesLT].[ProductCategory] (Name) values(@Name)";
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("@Name", $"Test {Guid.NewGuid()}");
                    int retVal = command.ExecuteNonQuery();
                    Console.WriteLine("Rows to be affected by customerQuery: {0}", retVal);
                    command.CommandText = sql;
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@Name", $"Test {Guid.NewGuid()}");
                    retVal = command.ExecuteNonQuery();
                    Console.WriteLine("Rows to be affected by customerQuery: {0}", retVal);

                    // Attempt to commit the transaction.
                    transaction.Commit();
                    Console.WriteLine("Both records are written to database.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                    Console.WriteLine("  Message: {0}", ex.Message);
                    //throw;
                    // Attempt to roll back the transaction.
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        // This catch block will handle any errors that may have occurred
                        // on the server that would cause the rollback to fail, such as
                        // a closed connection.
                        Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                        Console.WriteLine("  Message: {0}", ex2.Message);
                    }
                }
            }
        }

        /* The TransactionScope class was introduced in version 2.0 of the .NET Framework. 
         * It’s easy to use and powerful. Other than the declaration and instantiation of it, 
         * the only thing you need to know about it is that it has a method named Complete() 
         * that you should call if you are satisfied it completed successfully. This is a key point. 
         * Calling Complete() tells the transaction manager that everything should be committed. 
         * If it isn’t called, the transaction is automatically rolled back. Also, when called 
         * correctly in a using block, if an Exception is thrown during execution inside the 
         * TransactionScope, the transaction will be rolled back as well. 
         * Besides its simplicity, it also has the benefit of being able to handle both simple 
         * and distributed connections and promote simple transactions to distributed ones automatically. 
         * In the previous example, a new TransactionScope was declared and instantiated. Then two 
         * SqlConnections were created inside of it and two corresponding SqlCommands. There are 
         * no exception handlers, so any exceptions result in the transaction being rolled back. 
         * There are two important takeaways here. First, when the call to Open() is made on 
         * FirstConnection, it’s created inside a simple transaction. When Open is called on 
         * SecondConnection, the transaction is escalated to a full distributed transaction. 
         * This happens automatically with no intervention on the developer’s part. The second 
         * takeaway is that, in order for everything to happen correctly, the last statement, 
         * the call to Complete(), must happen before the transaction commits.
         * https://docs.microsoft.com/en-us/dotnet/api/system.transactions.transactionscope.complete?view=netcore-3.1
         * https://docs.microsoft.com/en-us/dotnet/api/system.transactions.transactionscope?view=netcore-3.1
         * https://docs.microsoft.com/en-us/dotnet/framework/data/transactions/implementing-an-implicit-transaction-using-transaction-scope         
         */
        public void TransactionScopeExample()
        {
            try
            {
                // Create the TransactionScope to execute the commands, guaranteeing
                // that both commands can commit or roll back as a single unit of work.
                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection mainConnection = new SqlConnection(connectionString))
                    {
                        // Opening the connection automatically enlists it in the 
                        // TransactionScope as a lightweight transaction.
                        mainConnection.Open();
                        string sql = $"Insert into [SalesLT].[ProductCategory] (Name) values(@Name)";
                        using (SqlCommand customerQuery = new SqlCommand(sql, mainConnection))
                        {
                            customerQuery.Parameters.AddWithValue("@Name", $"Test {Guid.NewGuid()}");
                            int retVal = customerQuery.ExecuteNonQuery();
                            Console.WriteLine("Rows to be affected by customerQuery: {0}", retVal);
                        }

                        // If you get here, this means that command1 succeeded. By nesting
                        // the using block for connection2 inside that of connection1, you
                        // conserve server and network resources as connection2 is opened
                        // only when there is a chance that the transaction can commit.
                        using (SqlConnection connection2 = new SqlConnection(connectionString))
                        {
                            // The transaction is escalated to a full distributed
                            // transaction when connection2 is opened.
                            connection2.Open();
                            
                            string _sql = $"Insert into [SalesLT].[ProductCategory] (Name) values(@Name)";
                            using (SqlCommand customerQuery = new SqlCommand(_sql, connection2))
                            {
                                customerQuery.Parameters.AddWithValue("@Name", $"Test {Guid.NewGuid()}");
                                int retVal = customerQuery.ExecuteNonQuery();
                                Console.WriteLine("Rows to be affected by customerQuery: {0}", retVal);
                            }
                        }
                    }

                    // The Complete method commits the transaction. If an exception has been thrown,
                    // Complete is not  called and the transaction is rolled back.
                    scope.Complete();
                    Console.WriteLine("scope.Complete()");
                }
            }            
            catch (TransactionAbortedException ex)
            {
                Console.WriteLine("TransactionAbortedException Message: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                //Exception Message: This platform does not support distributed transactions.
                Console.WriteLine("Exception Message: {0}", ex.Message);
            }
            finally
            {
            }
        }
    }
}
