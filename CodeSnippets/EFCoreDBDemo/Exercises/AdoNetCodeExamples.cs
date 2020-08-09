//ADO.NET namespaces: System.Data, System.Data.SqlClient
//using Microsoft.Data.SqlClient;
using System.Data.SqlClient;
using System.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

/*
You can use ADO.NET against Windows Azure’s SQL databases with essentially no difference in coding.
In fact, you are encouraged to make the earlier SqlDataAdapter or SqlDataReader tests work against 
a Windows Azure SQL database by modifying only the connection string and nothing else!
*/

namespace EFCoreDBDemo.Exercises
{
    public class AdoNetCodeExamples
    {
        public AdoNetCodeExamples()
        {

        }

        private string connectionString =
                "Server=(local);Database=AdventureWorksLT2017;Integrated Security=true;";

        public void Run()
        {
            //To take advantage of the benefits of ADO.NET, unnecessary connections to the database must be minimized.
            //Any command you use in ADO.NET outside of a DataAdapter requires you to specifically open your connection. 
            //You must take explicit measures to make sure that it is closed.This can be done via a try/catch/finally 
            //or try/finally structure, in which the call to close the connection is included in the finally statement.
            //You can also use the Using statement (which originally was available only in C#, but is now available in
            //VB.NET), which ensures that the Dispose method is called on IDisposable objects.
            //When querying data, there are two mechanisms you can use: a DataReader or a DataAdapter.
            //Underneath the abstractions, a DataAdapter uses a DataReader to populate the returned DataSet or DataTable.
            //Using a DataReader produces faster results than using a DataAdapter to return the same
            //data.Because the DataAdapter actually uses a DataReader to retrieve data, this should not surprise you. 

            //Example01();
            //Example02();

            /* Using a DataReader results in faster access times than a DataAdapter does.
            The DataAdapter approach takes approximately 3 milliseconds (ms) to run;
            the DataReader approach takes approximately 2 ms to run. The point here isn’t that the
            DataAdapter approach is 50 percent slower; it is approximately 1 ms slower. Any data access
            times measured in single-digit milliseconds is about as ideal as you can hope for in most
            circumstances. Something else you can do is use a profiling tool to monitor SQL Server (such
            as SQL Server Profiler) and you will notice that both approaches result in an identical query to
            the database.*/

            //Example03();
            //Example04();

            //Ado using async method to retrieve data 
            //Example05();
            //Example06();
        }

        //https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlcommand.beginexecutereader?view=dotnet-plat-ext-3.1 
        private SqlConnection _connection = null;
        public void Example06()
        {
            try
            {
                int customerId1 = 4;
                int customerId2 = 20;

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT CustomerID, FirstName, LastName");
                sql.Append(" FROM [SalesLT].[Customer] WHERE CustomerId between @CustomerId1 and @CustomerId2");

                _connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sql.ToString(), _connection);
                command.Parameters.AddWithValue("@CustomerId1", customerId1);
                command.Parameters.AddWithValue("@CustomerId2", customerId2);

                _connection.Open();
                // Although it is not required that you pass the
                // SqlCommand object as the second parameter in the
                // BeginExecuteReader call, doing so makes it easier
                // to call EndExecuteReader in the callback procedure.
                AsyncCallback callback = new AsyncCallback(HandleCallback);
                IAsyncResult result = command.BeginExecuteReader(callback, command);
                Console.WriteLine("Executing...");
                // Although it is not necessary, the following code
                // displays a counter in the console window, indicating that
                // the main thread is not blocked while awaiting the command
                // results.
                int count = 0;
                while (!result.IsCompleted)
                {
                    Console.WriteLine("Waiting ({0})", count++);
                    // Wait for 1/10 second, so the counter
                    // does not consume all available resources
                    // on the main thread.
                    System.Threading.Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception, Error: " + ex.Message);
            }
            finally
            {
                //Else unable to read data from connection in callback, because connection was closed
                //if (connection != null)
                //{
                //    connection.Close();
                //}
            }
        }

        private void HandleCallback(IAsyncResult result)
        {
            try
            {
                Console.WriteLine("Entering: HandleCallback(IAsyncResult result)");
                // Retrieve the original command object, passed
                // to this procedure in the AsyncState property
                // of the IAsyncResult parameter.
                SqlCommand command = (SqlCommand)result.AsyncState;
                using (SqlDataReader reader = command.EndExecuteReader(result))
                {
                    DisplayResults(reader);
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SqlException, Error ({0}): {1}", ex.Number, ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("InvalidOperationException, Error: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                // You might want to pass these errors
                // back out to the caller.
                Console.WriteLine("Exception, Error: {0}", ex.Message);
            }
            finally
            {
                if (_connection != null)
                {
                    _connection.Close();
                }
                //isExecuting = false;
            }
        }

        /*
        DataReaders provide multiple asynchronous methods that can be employed (BeginExecuteNonQuery,
        BeginExecuteReader, BeginExecuteXmlReader). DataAdapters on the
        other hand, essentially have only synchronous methods. With small-sized record sets,
        the differences in performance or advantages of using asynchronous methods are
        trivial. On large queries that take time, a DataReader, in conjunction with asynchronous
        methods, can greatly enhance the user experience.
        https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlcommand.beginexecutereader?view=dotnet-plat-ext-3.1
        */
        public void Example05()
        {
            int customerId1 = 4;
            int customerId2 = 20;

            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT CustomerID, FirstName, LastName");
            sql.Append(" FROM [SalesLT].[Customer] WHERE CustomerId between @CustomerId1 and @CustomerId2");

            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sql.ToString(), connection);
                command.Parameters.AddWithValue("@CustomerId1", customerId1);
                command.Parameters.AddWithValue("@CustomerId2", customerId2);

                connection.Open();
                IAsyncResult result = command.BeginExecuteReader(CommandBehavior.CloseConnection);

                // Although it is not necessary, the following code
                // displays a counter in the console window, indicating that
                // the main thread is not blocked while awaiting the command
                // results.
                int count = 0;
                while (!result.IsCompleted)
                {
                    Console.WriteLine("Waiting ({0})", count++);
                    // Wait for 1/10 second, so the counter
                    // does not consume all available resources
                    // on the main thread.
                    System.Threading.Thread.Sleep(100);
                }

                using (SqlDataReader reader = command.EndExecuteReader(result))
                {
                    DisplayResults(reader);
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SqlException, Error ({0}): {1}", ex.Number, ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("InvalidOperationException, Error: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                // You might want to pass these errors
                // back out to the caller.
                Console.WriteLine("Exception, Error: {0}", ex.Message);
            }
        }

        private static void DisplayResults(SqlDataReader reader)
        {
            // Display the data within the reader.
            while (reader.Read())
            {
                // Display all the columns.
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.Write("{0}\t", reader.GetValue(i));
                }
                Console.WriteLine();
            }
        }

        //Query of Customer Table using SqlDataReader
        //GetCustomersWithDataReader
        public void Example04()
        {
            int customerId1 = 4;
            int customerId2 = 20;
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT CustomerID, FirstName, LastName");
            sql.Append(" FROM [SalesLT].[Customer] WHERE CustomerId between @CustomerId1 and @CustomerId2");

            using (SqlConnection mainConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand customerQuery = new SqlCommand(sql.ToString(), mainConnection))
                {
                    customerQuery.Parameters.AddWithValue("@CustomerId1", customerId1);
                    customerQuery.Parameters.AddWithValue("@CustomerId2", customerId2);
                    mainConnection.Open();
                    //You can iterate through a DataReader only once and can iterate through it only in a
                    //forward - only fashion. a DataReader can indicate whether data was returned 
                    //(via the HasRows property), but the only way to know the exact record count 
                    //returned from a DataReader is to iterate through it and count it out specifically.
                    using (SqlDataReader reader = customerQuery.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        try
                        {
                            //DataReaders have a property called
                            //HasRows, which indicates whether data was returned from the query, but there is no way
                            //to know the exact number of rows without iterating through the DataReader and counting
                            //them.By contrast, the DataAdapter immediately makes the returned record count for each
                            //table available upon completion.
                            Console.WriteLine($"reader.HasRows: {reader.HasRows}");
                            int firstNameIndex = reader.GetOrdinal("FirstName");
                            int lastNameIndex = reader.GetOrdinal("LastName");
                            int customerIdIndex = reader.GetOrdinal("CustomerID");
                            while (reader.Read())
                            {
                                Console.WriteLine($"{(int)reader[customerIdIndex]} " +
                                    $"{(string)reader[firstNameIndex]} " +
                                    $"{(string)reader[lastNameIndex]}");
                            }
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine($"{ex.Message}");
                        }
                        finally
                        {
                            // This will soon be closed even if we encounter an exception
                            // but making it explicit in code.
                            if (mainConnection.State != ConnectionState.Closed)
                            {
                                mainConnection.Close();
                            }
                        }
                    }
                }
            }
        }

        /*
        The Fill method of DataAdapter objects enables you to populate only DataSets and
        DataTables. If you’re planning to use a custom business object, you have to first retrieve
        the DataSet or DataTables; then you need to write code to hydrate your business
        object collection. This can have an impact on application responsiveness as well as the
        memory your application uses.

        Although both types enable you to execute multiple queries and retrieve multiple return
        sets, only the DataSet lets you closely mimic the behavior of a relational database
        (for instance, add Relationships between tables using the Relations property or ensure
        that certain data integrity rules are adhered to via the EnforceConstraints property).

        The Fill method of the DataAdapter completes only when all the data has been retrieved
        and added to the DataSet or DataTable. This enables you to immediately determine
        the number of records in any given table. By contrast, a DataReader can indicate
        whether data was returned (via the HasRows property), but the only way to know the
        exact record count returned from a DataReader is to iterate through it and count it out
        specifically.

        You can iterate through a DataReader only once and can iterate through it only in a
        forward-only fashion. You can iterate through a DataTable any number of times in any
        manner you see fit.

        DataSets can be loaded directly from XML documents and can be persisted to XML
        natively. They are consequently inherently serializable, which affords many features
        not natively available to DataReaders (for instance, you can easily store a DataSet or
        a DataTable in Session or View State, but you can’t do the same with a DataReader).
        You can also easily pass a DataSet or DataTable in between tiers because it is already
        serializable, but you can’t do the same with a DataReader. However, a DataSet is also
        an expensive object with a large memory footprint. Despite the ease in doing so, it
        is generally ill-advised to store it in Session or Viewstate variables, or pass it across
        multiple application tiers because of the expensive nature of the object. If you serialize
        a DataSet, proceed with caution!

        After a DataSet or DataTable is populated and returned to the consuming code, no
        other interaction with the database is necessary unless or until you decide to send the
        localized changes back to the database. As previously mentioned, you can think of the
        dataset as an in-memory copy of the relevant portion of the database.
        */
        //GetCustomersWithDataAdapter
        public void Example03()
        {
            int customerId = 4;

            // ARRANGE
            DataSet customerData = new DataSet("CustomerData");
            DataTable customerTable = new DataTable("Customer");
            customerData.Tables.Add(customerTable);

            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT CustomerID, FirstName, LastName");
            sql.Append(" FROM [SalesLT].[Customer] WHERE CustomerId < @CustomerId ");

            using (SqlConnection mainConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand customerQuery = new SqlCommand(sql.ToString(), mainConnection))
                {
                    customerQuery.Parameters.AddWithValue("@CustomerId", customerId);
                    using (SqlDataAdapter customerAdapter = new SqlDataAdapter(customerQuery))
                    {
                        try
                        {
                            customerAdapter.Fill(customerData, "Customer");
                        }
                        finally
                        {
                            // This should already be closed even if we encounter an exception
                            // but making it explicit in code.
                            if (mainConnection.State != ConnectionState.Closed)
                            {
                                mainConnection.Close();
                            }
                        }
                    }
                }
            }

            Console.WriteLine($"Number of records returned: {customerData.Tables["Customer"].Rows.Count}");
            Console.WriteLine($"Number of records returned: {customerTable.Rows.Count}");

            int columnId = customerData.Tables["Customer"].Columns["CustomerID"].Ordinal;
            Console.WriteLine($"First row, id value: {customerData.Tables["Customer"].Rows[0].ItemArray[columnId]}");
            Console.WriteLine($"First row, id value: {customerTable.Rows[0].ItemArray[columnId]}");

            foreach (DataRow dr in customerData.Tables["Customer"].Rows)
            {
                var CustomerID = dr["CustomerID"];
                var FirstName = dr["FirstName"];
                var LastName = dr["LastName"];
                Console.WriteLine($"##{CustomerID} {FirstName} {LastName}");
            }
        }

        //https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/populating-a-dataset-from-a-dataadapter
        public void Example02()
        {
            decimal paramValue = 3000;
            string queryString =
                "SELECT ProductId, ListPrice, Name from SalesLT.Product "
                + "WHERE ListPrice > @listPrice "
                + "ORDER BY ListPrice DESC;";

            using (SqlConnection Conn = new SqlConnection(connectionString))
            {
                SqlCommand selectCMD = new SqlCommand(queryString, Conn);
                selectCMD.CommandTimeout = 30;
                selectCMD.Parameters.AddWithValue("@listPrice", paramValue);

                /*
                The code shown in this example does not explicitly open and close the Connection. 
                The Fill method implicitly opens the Connection that the DataAdapter is using if 
                it finds that the connection is not already open. If Fill opened the connection, 
                it also closes the connection when Fill is finished. This can simplify your code 
                when you deal with a single operation such as a Fill or an Update. However, if 
                you are performing multiple operations that require an open connection, you can 
                improve the performance of your application by explicitly calling the Open method 
                of the Connection, performing the operations against the data source, and then 
                calling the Close method of the Connection. You should try to keep connections 
                to the data source open as briefly as possible to free resources for use by other 
                client applications.
                */
                SqlDataAdapter productDA = new SqlDataAdapter();
                productDA.SelectCommand = selectCMD;
                //DataSet: In-memory copy of the RDBMS or portion of RDBMS relevant to the application. This
                //is a collection of DataTable objects, their relationships to one another, and other
                //metadata about the database and commands to interact with it.
                DataSet productDS = new DataSet();
                Conn.Open();
                productDA.Fill(productDS, "Products");
                Conn.Close();
                //DataTable: Corresponds to a specific view of data, from a SELECT query or generated
                //from.NET code. This is often analogous to a table in the RDBMS, although only partially
                //populated.It tracks the state of data stored in it so, when data is modified, you
                //can tell which records need to be saved back into the database.
                foreach (DataTable table in productDS.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        var ProductId = dr["ProductId"];
                        var ListPrice = dr["ListPrice"];
                        var Name = dr["Name"];
                        Console.WriteLine($"#{ProductId} {Name} {ListPrice}");
                    }
                }
            }
        }

        //https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/ado-net-code-examples
        public void Example01()
        {
            // Provide the query string with a parameter placeholder.
            string queryString =
                "SELECT ProductId, ListPrice, Name from SalesLT.Product "
                    + "WHERE ListPrice > @listPrice "
                    + "ORDER BY ListPrice DESC;";

            // Specify the parameter value.
            decimal paramValue = 3000;

            // Create and open the connection in a using block. This
            // ensures that all resources will be closed and disposed
            // when the code exits.
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                // Create the Command and Parameter objects.
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@listPrice", paramValue);

                // Open the connection in a try/catch block.
                // Create and execute the DataReader, writing the result
                // set to the console window.
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine("\t{0}\t{1}\t{2}",
                            reader[0], reader[1], reader[2]);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
