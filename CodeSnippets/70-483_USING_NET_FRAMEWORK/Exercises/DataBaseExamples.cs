using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml;

namespace _70_483_USING_NET_FRAMEWORK.Exercises
{
    public class DataBaseExamples
    {
        public void Run()
        {
            //DataReaderExampleV3().Wait();
            //XmlReaderExample();
            //DataReaderExampleV2();
            //TransientErrors();
            ///DistributedTransaction();
            //CourseInsert();
            //DataReaderExample();
            //RetrievingLargeBinaryData();
            //RetrieveImage();
            //DependentInserts();
            return;
            //Demonstrate the ReadCommitted transaction:
            //Rows effected: 1
            //30120 Mohamed Kalmoua 245 - 555 - 0173
            //-----------------------------------------------
            Console.WriteLine("Demonstrate the ReadCommitted transaction: ");
            Task[] tasks2 =
            {
                Task.Factory.StartNew(()=>DirtyRead(System.Data.IsolationLevel.ReadCommitted)),
                Task.Factory.StartNew(()=>UpdatePhone())
            };

            Task.WaitAll(tasks2);
            Console.WriteLine("-----------------------------------------------");

            //Demonstrate the ReadUncommitted transaction:
            //30120 Mohamed Kalmoua 777 - 555 - 1024
            //Rows effected: 1
            //------------------------------------------------
            Console.WriteLine("Demonstrate the ReadUncommitted transaction: ");
            Task[] tasks =
            {
                Task.Factory.StartNew(()=>DirtyRead(System.Data.IsolationLevel.ReadUncommitted)),
                Task.Factory.StartNew(()=>UpdatePhone())
            };

            Task.WaitAll(tasks);
            Console.WriteLine("-----------------------------------------------");
        }

        private async Task DataReaderExampleV3()
        {
            Console.WriteLine($"Entering DataReaderExampleV3()");
            const string _connectionString
                = "Server=(local);Database=ContosoUniversity;Integrated Security=true;MultipleActiveResultSets=true;Connection Timeout=60;";

            const string sql = "SELECT* FROM [ContosoUniversity].[dbo].[Course];" +
                "SELECT* FROM [ContosoUniversity].[dbo].[Department];";

            try
            {
                SqlConnection connection = new SqlConnection(_connectionString);
                SqlCommand cmd = new SqlCommand(sql, connection);
                await connection.OpenAsync();
                SqlDataReader reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleResult);
                while (reader.Read())
                {
                    Console.WriteLine($"{reader[0].ToString()} {reader[3].ToString()} {reader[4].ToString()}");
                }
                //Because we used CommandBehavior.SingleResult, second result set is not returned 
                bool more = reader.NextResult();
                Console.WriteLine($"RetVal = reader.NextResult(): {more}");

                reader.Close();
                cmd.Dispose();
                connection.Close();
                connection.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex.Message);
            }

            Console.WriteLine($"Exiting DataReaderExampleV3()");
        }

        //https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader.create?view=netframework-4.8
        //https://stackoverflow.com/questions/13743250/meaning-of-xml-version-1-0-encoding-utf-8
        //https://stackoverflow.com/questions/4518544/xmlreader-from-a-string-content
        private void XmlReaderExample()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                "<!-- This is a sample XML -->" +
                "<Items>" +
                "<Item>First item</Item>" +
                "<Item>Second item</Item>" +
                "</Items>";

            try
            {
                byte[] data = Encoding.UTF8.GetBytes(xml);
                using (MemoryStream memStream = new MemoryStream(data))
                {
                    using (XmlReader reader = XmlReader.Create(memStream))
                    {
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Text)
                                Console.WriteLine(reader.Value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"XmlReaderExample: {ex.Message}");
            }
        }

        /*
        SingleResult: The SQL statement returns a single result set. Data.IDataReader.NextResult will return false.

        */
        private void DataReaderExampleV2()
        {
            Console.WriteLine($"Entering DataReaderExample()");
            const string _connectionString
                = "Server=(local);Database=ContosoUniversity;Integrated Security=true;MultipleActiveResultSets=true;Connection Timeout=60;";

            const string sql = "SELECT* FROM [ContosoUniversity].[dbo].[Course];" +
                "SELECT* FROM [ContosoUniversity].[dbo].[Department];";

            try
            {
                SqlConnection connection = new SqlConnection(_connectionString);
                SqlCommand cmd = new SqlCommand(sql, connection);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SingleResult);
                while (reader.Read())
                {
                    Console.WriteLine($"{reader[0].ToString()} {reader[3].ToString()} {reader[4].ToString()}");
                }
                //Because we used CommandBehavior.SingleResult, second result set is not returned 
                bool more = reader.NextResult();
                Console.WriteLine($"RetVal = reader.NextResult(): {more}");

                reader.Close();
                cmd.Dispose();
                connection.Close();
                connection.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}",ex.Message);
            }
        }

        private void TransientErrors()
        {
            Console.WriteLine("Entering TransientErrors()");

            for (int i = 0; i < 5; i ++)
            {
                try
                {
                    Console.WriteLine($"Loop: {i}");
                    throw new Exception("Transient error");
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                    if(true)//if(IsTransient(ex.Number)) SqlException 
                    {
                        continue;
                    }
                    else
                    {
                        break;
                    }                    
                }
            }

            Console.WriteLine("Exiting TransientErrors()");
        }

        /* Example of Distributed Transaction 
           https://docs.microsoft.com/en-us/dotnet/framework/data/transactions/implementing-an-implicit-transaction-using-transaction-scope 
            https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/system-transactions-integration-with-sql-server
         */
        private void DistributedTransaction()
        {
            Console.WriteLine($"Entering DistributedTransaction()");
            const string _connectionString
                = "Server=(local);Database=ContosoUniversity;Integrated Security=true;MultipleActiveResultSets=true;Connection Timeout=60;";

            const string sql = "INSERT INTO [ContosoUniversity].[dbo].[Course] (Title,Credits," +
                "DepartmentID,IsDeleted) VALUES(@Title,@Credits,@DepartmentID,@IsDeleted);";

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand(sql, connection))
                        {
                            SqlParameter prm = new SqlParameter("@Title", SqlDbType.VarChar, 50);
                            prm.Value = "Physics";
                            cmd.Parameters.Add(prm);
                            cmd.Parameters.AddWithValue("@Credits", 4);
                            cmd.Parameters.AddWithValue("@DepartmentID", 1);
                            cmd.Parameters.AddWithValue("@IsDeleted", 0);
                            connection.Open();
                            int rows = cmd.ExecuteNonQuery();
                            Console.WriteLine($"rows: {rows}");
                        }
                    }

                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand(sql, connection))
                        {
                            SqlParameter prm = new SqlParameter("@Title", SqlDbType.VarChar, 50);
                            prm.Value = "Physics";
                            cmd.Parameters.Add(prm);
                            cmd.Parameters.AddWithValue("@Credits", 4);
                            cmd.Parameters.AddWithValue("@DepartmentID", 1);
                            cmd.Parameters.AddWithValue("@IsDeleted", 0);
                            connection.Open();
                            int rows = cmd.ExecuteNonQuery();
                            Console.WriteLine($"rows: {rows}");
                        }
                    }

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }

        private void CourseInsert()
        {
            Console.WriteLine($"Entering CourseInsert()");
            const string _connectionString
                = "Server=(local);Database=ContosoUniversity;Integrated Security=true;MultipleActiveResultSets=true;Connection Timeout=60;";

            const string sql = "INSERT INTO [ContosoUniversity].[dbo].[Course] (Title,Credits," +
                "DepartmentID,IsDeleted) VALUES(@Title,@Credits,@DepartmentID,@IsDeleted);";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        SqlParameter prm = new SqlParameter("@Title", SqlDbType.VarChar, 50);
                        prm.Value = "Physics";
                        cmd.Parameters.Add(prm);
                        cmd.Parameters.AddWithValue("@Credits", 4);
                        cmd.Parameters.AddWithValue("@DepartmentID", 1);
                        cmd.Parameters.AddWithValue("@IsDeleted", 0);
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        Console.WriteLine($"rows: {rows}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
            Console.WriteLine($"Exiting CourseInsert");
        }

        /*
        The SqlDataReader.Read Method advances the SqlDataReader to the next record. 
        The default position of the SqlDataReader is before the first record. Therefore, 
        you must call Read to begin accessing any data. The SqlDataReader.NextResult 
        method advances the data reader to the next result, when reading the results of 
        batch Transact-SQL statements. Used to process multiple results, which can be 
        generated by executing batch Transact-SQL statements. By default, the data reader 
        is positioned on the first result.
        */
        private void DataReaderExample()
        {
            Console.WriteLine($"Entering DataReaderExample()");
            const string _connectionString
                = "Server=(local);Database=ContosoUniversity;Integrated Security=true;MultipleActiveResultSets=true;Connection Timeout=60;";

            const string sql = "SELECT* FROM [ContosoUniversity].[dbo].[Course];" +
                "SELECT* FROM [ContosoUniversity].[dbo].[Department];";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            Console.WriteLine($"{reader[0].ToString()} {reader[3].ToString()} {reader[4].ToString()}");
                        }

                        //https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqldatareader.nextresult?view=dotnet-plat-ext-3.1
                        bool more = reader.NextResult();
                        Console.WriteLine();

                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader[0].ToString()} {reader[3].ToString()} {reader[4].ToString()}");
                        }
                    }
                }
            }
        }

        /*
        SequentialAccess provides a way for the DataReader to handle rows that contain columns 
        with large binary values. Rather than loading the entire row, SequentialAccess enables 
        the DataReader to load data as a stream. You can then use the GetBytes or GetChars method 
        to specify a byte location to start the read operation, and a limited buffer size for the 
        data being returned. Use SequentialAccess to retrieve large values and binary data. 
        Otherwise, an OutOfMemoryException might occur and the connection will be closed.         
        https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/retrieving-binary-data
        https://docs.microsoft.com/en-us/dotnet/api/system.data.commandbehavior?view=netcore-3.1
        https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlcommand.executereader?view=netframework-4.8
        Provides a way for the DataReader to handle rows that contain columns with large binary values. 
        Rather than loading the entire row, SequentialAccess enables the DataReader to load data as a stream. 
        You can then use the GetBytes or GetChars method to specify a byte location to start the read operation, 
        and a limited buffer size for the data being returned.
        */
        private void RetrievingLargeBinaryData()
        {
            Console.WriteLine($"Entering RetrievingLargeBinaryData()");
            const string _connectionString
                = "Server=(local);Database=AdventureWorksLT2017;Integrated Security=true;MultipleActiveResultSets=true;Connection Timeout=60;";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "SELECT ThumbnailPhotoFileName, ThumbNailPhoto FROM [AdventureWorksLT2017].[SalesLT].[Product];";
                SqlCommand cmd = new SqlCommand(sql, connection);

                // Writes the BLOB to a file (*.gif).  
                FileStream stream;
                // Streams the BLOB to the FileStream object.  
                BinaryWriter writer;

                // Size of the BLOB buffer.  
                int bufferSize = 100;
                // The BLOB byte[] buffer to be filled by GetBytes.  
                byte[] outByte = new byte[bufferSize];
                // The bytes returned from GetBytes.  
                long retval;
                // The starting position in the BLOB output.  
                long startIndex = 0;
                int index = 1;

                // The publisher id to use in the file name.  
                string pubID = "";

                // Open the connection and read data into the DataReader.  
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);

                while (reader.Read())
                {
                    // Get the publisher id, which must occur before getting the logo.  
                    pubID = reader.GetString(0);

                    //Custom numeric format strings
                    //https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-numeric-format-strings
                    // Create a file to hold the output.  
                    stream = new FileStream(
                        (++index).ToString("000") + "_" + pubID , FileMode.OpenOrCreate, FileAccess.Write);
                    writer = new BinaryWriter(stream);

                    // Reset the starting byte for the new BLOB.  
                    startIndex = 0;

                    // Read bytes into outByte[] and retain the number of bytes returned.  
                    retval = reader.GetBytes(1, startIndex, outByte, 0, bufferSize);

                    // Continue while there are bytes beyond the size of the buffer.  
                    while (retval == bufferSize)
                    {
                        writer.Write(outByte);
                        writer.Flush();

                        // Reposition start index to end of last buffer and fill buffer.  
                        startIndex += bufferSize;
                        retval = reader.GetBytes(1, startIndex, outByte, 0, bufferSize);
                    }

                    // Write the remaining buffer.  
                    writer.Write(outByte, 0, (int)retval);
                    writer.Flush();

                    // Close the output file.  
                    writer.Close();
                    stream.Close();
                }

                // Close the reader and the connection.  
                reader.Close();
            }

            Console.WriteLine($"Exiting RetrievingLargeBinaryData()");
        }

        /*
        You are developing an ASP.NET MVC application. The application has a page that searches for and displays an image stored in a database. Members of the
        EntityClient namespace are used to access an ADO.NET Entity Framework data model Images and associated metadata are stored in a database table.
        You need to run a query that returns only the image while minimizing the amount of data that is transmitted.
        Which method of the EntityCommand type should you use?
        ExecuteScalar has limitation of 2033 characters so for big blobs it may be not sufficient. 
        ExecuteReader would solve that limitation.
        The SqlCommand.ExecuteScalar method executes the query, and returns the first column of the first 
        row in the result set returned by the query. Additional columns or rows are ignored.        
        https://msdn.microsoft.com/en-us/library/system.data.entityclient.entitycommand(v=vs.110).aspx
        https://www.codeproject.com/Articles/10861/Storing-and-Retrieving-Images-from-SQL-Server-usin
        */
        private void RetrieveImage()
        {
            Console.WriteLine($"Entering RetrieveImage()");
            const string _connectionString
                = "Server=(local);Database=AdventureWorksLT2017;Integrated Security=true;MultipleActiveResultSets=true;Connection Timeout=60;";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "SELECT ThumbNailPhoto FROM [AdventureWorksLT2017].[SalesLT].[Product] WHERE ProductID=@id;";
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@id", 713);
                connection.Open();

                byte[] barrImg = (byte[])cmd.ExecuteScalar();                
                string strfn = Convert.ToString(DateTime.Now.ToFileTime())+".gif";
                Console.WriteLine(strfn);
                FileStream fs = new FileStream(strfn, FileMode.CreateNew, FileAccess.Write);
                fs.Write(barrImg, 0, barrImg.Length);
                fs.Flush();
                fs.Close();
            }
        }

        private void DependentInserts()
        {
            Console.WriteLine($"Entering DependentInserts");
            const string _connectionString 
                = "Server=(local);Database=ContosoUniversity;Integrated Security=true;MultipleActiveResultSets=true;Connection Timeout=60;";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.Connection = connection;
                SqlTransaction transaction = connection.BeginTransaction("Initial");
                command.Transaction = transaction;

                try
                {
                    command.CommandText = "INSERT INTO [ContosoUniversity].[dbo].[Course] (Title,Credits,DepartmentID,IsDeleted) " +
                        "VALUES(@Title,@Credits,@DepartmentID,@IsDeleted);";

                    command.Parameters.AddWithValue("@Title", "Physics");
                    command.Parameters.AddWithValue("@Credits", 4);
                    command.Parameters.AddWithValue("@DepartmentID", 1);
                    command.Parameters.AddWithValue("@IsDeleted", 0);

                    int rows = command.ExecuteNonQuery();
                    Console.WriteLine($"Rows effected: {rows}");
                    transaction.Save("CourseInsert");

                    try
                    {
                        command.CommandText = "INSERT INTO [ContosoUniversity].[dbo].[Department] (Name,Budget,StartDate,InstructorID,IsDeleted) VALUES('Physics', 100000, '2020-09-01 00:00:00.0000000',2,0);";
                        rows = command.ExecuteNonQuery();
                        //throw new Exception("A testcase exception.");
                    }
                    //If an error occurs, rollback to Save point "CourseInsert" 
                    //The course will be inserted
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception: {ex.Message}");
                        transaction.Rollback("CourseInsert");
                        //transaction.Rollback("Initial");
                    }

                    Console.WriteLine($"Executing transaction.Commit()");
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                    transaction.Rollback("Initial");
                }
            }
            Console.WriteLine($"Exiting DependentInserts");
        }

        //https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlconnection.connectiontimeout?view=dotnet-plat-ext-3.1
        private const string connectionString = "Server=(local);Database=AdventureWorksLT2017;Integrated Security=true;MultipleActiveResultSets=true;Connection Timeout=60;";

        public void DirtyRead(System.Data.IsolationLevel isolationLevel = System.Data.IsolationLevel.ReadUncommitted)
        {
            //30120 Mohamed Kalmoua 245-555-0173
            const string sql = @"WaitFor Delay '00:00:05';
                SELECT CustomerID, FirstName, LastName, Phone
                FROM[AdventureWorksLT2017].[SalesLT].[Customer]
                WHERE CustomerID=@ID;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction transaction = 
                    conn.BeginTransaction(isolationLevel, "DirtyRead"))
                {                     
                    using (SqlCommand command = new SqlCommand(sql, conn, transaction))
                    {
                        command.Parameters.AddWithValue("@ID", 30120);
                        //https://docs.microsoft.com/en-us/dotnet/api/microsoft.data.sqlclient.sqlcommand.connection?view=sqlclient-dotnet-core-2.0
                        command.CommandTimeout = 60;
                        using (SqlDataReader sqlDataReader = command.ExecuteReader())
                        {
                            while (sqlDataReader.Read())
                            {
                                Console.WriteLine($"{(int)sqlDataReader["CustomerID"]} " +
                                    $"{(string)sqlDataReader["FirstName"]} " +
                                    $"{(string)sqlDataReader["LastName"]} " +
                                    $"{(string)sqlDataReader["Phone"]}");
                            }
                        }
                    }
                    transaction.Commit();
                }
            }
        }

        public void UpdatePhone(System.Data.IsolationLevel isolationLevel = System.Data.IsolationLevel.ReadUncommitted)
        {
            //30120 Mohamed Kalmoua 245-555-0173
            const string sql = @"Update [AdventureWorksLT2017].[SalesLT].[Customer]
                set Phone='777-555-1024' 
                WHERE CustomerID=@ID;
                WaitFor Delay '00:00:30';";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction transaction =
                    conn.BeginTransaction(isolationLevel, "DirtyWrite"))
                {
                    using (SqlCommand command = new SqlCommand(sql, conn, transaction))
                    {
                        command.Parameters.AddWithValue("@ID", 30120);
                        command.CommandTimeout = 60;
                        int rows = command.ExecuteNonQuery();
                        Console.WriteLine($"Rows effected: {rows}");
                    }
                    transaction.Rollback();
                }
            }
        }
    }
}

/*
Specifying a transaction isolation level
The IsolationLevel enum is used to manage how multiple transactions interact with one another. 
Another way to describe it is that IsolationLevels control the locking behavior employed for the 
execution of a command. However, there’s a problem that many developers stumble over, so let’s first 
get this out of the way. There are actually two Isolation enums: one in System.Data.IsolationLevel and 
a second in System.Transaction.IsolationLevel. Just as there were two CacheItemPriority enums that generally 
served the same purpose, these two IsolationLevel enums generally serve the same purpose, but for two different 
sets of classes. Fortunately, both have the same values, so there isn’t much to remember between the two other 
than the fact that the two exist; sometimes you need one, and other times you need the other.

IsolationLevel Enum
https://docs.microsoft.com/en-us/dotnet/api/system.data.isolationlevel?redirectedfrom=MSDN&view=netcore-3.1


--Create Database
--https://docs.microsoft.com/en-us/sql/t-sql/statements/create-database-transact-sql?view=sql-server-ver15
USE master;
GO
IF DB_ID (N'mytest') IS NOT NULL
DROP DATABASE mytest;
GO
CREATE DATABASE mytest;
GO
waitfor delay '00:00:15'
-- Verify the database files and sizes
SELECT name, size, size*1.0/128 AS [Size in MBs]
FROM sys.master_files
WHERE name = N'mytest';
GO

--Create Employee table 
use [mytest]
IF OBJECT_ID('Employee') is not null
begin 
DROP TABLE Employee
end

create table Employee(ID int,FirstName Varchar(50), LastName Varchar(50), Salary Int)

insert into Employee(ID,FirstName,LastName,Salary)
values( 1,'David','Mclane',1000)

insert into Employee(ID,FirstName,LastName,Salary)
values( 2,'Steve','Clock',2000)

insert into Employee(ID,FirstName,LastName,Salary)
values( 3,'Chris','Gates',3000)   

--Check content Employee table 
use [mytest]
select *from Employee


------------------------------------------------------------------------------------
ReadCommitted


Read Committed
In select query it will take only commited values of table. If any transaction is opened and 
incompleted on table in others sessions then select query will wait till no transactions are pending on same table.
Session 1

use [mytest]
begin tran
update Employee set Salary=999 where ID=1
waitfor delay '00:00:30'
commit


Run both sessions side by side.
Output: 999
In second session, it returns the result only after execution of complete transaction in 
first session because of the lock on Emp table. We have used wait command to delay 15 
seconds after updating the Emp table in transaction.
Because of lock by transaction in session 1, the query returns after 30 second (Transaction in session is then committed) 

Session 2

use [mytest]
set transaction isolation level read committed
select Salary from Employee where ID=1

------------------------------------------------------------------------------------

Reference for examples below: http://www.besttechtools.com/articles/article/sql-server-isolation-levels-by-example
------------------------------------------------------------------------------------
ReadCommitted


ReadCommitted

Session 1


use [mytest]
begin transaction
select * from Employee
waitfor delay '00:00:30'
commit


ReadCommitted
Run both sessions side by side.
Output: 1000

In session2, there won't be any delay in execution because in session1 Emp table is used 
under transaction but it is not used update or delete command hence Emp table is not locked.

Session 2


use [mytest]
set transaction isolation level read committed
select * from Employee
------------------------------------------------------------------------------------

------------------------------------------------------------------------------------
ReadCommitted


ReadCommitted

Session 1


use [mytest]
begin tran
select * from Employee            
waitfor delay '00:00:30'
update Employee set Salary=999 where ID=1
commit


ReadCommitted
Run both sessions side by side.

Output: 1000
In session2, there won't be any delay in execution because when session2 is executed Emp 
table in session1 is not locked(used only select command, locking on Emp table occurs after wait delay command).

Session 2


use [mytest]
set transaction isolation level read committed
select Salary from Employee where ID=1
------------------------------------------------------------------------------------

------------------------------------------------------------------------------------
ReadUncommitted


Read Uncommitted

If any table is updated(insert or update or delete) 
under a transaction and same transaction is not completed 
that is not committed or roll backed then uncommitted values 
will displaly(Dirty Read) in select query of "Read Uncommitted" isolation 
transaction sessions. There won't be any delay in select query execution because 
this transaction level does not wait for committed values on table.

Session 1


use [mytest]
begin transaction
update Employee set Salary=2020 where ID=1
waitfor delay '00:00:30'
rollback


Read uncommitted

Run both sessions at a time one by one.

Output: 2020

Select query in Session2 executes after update Emp table in transaction and before transaction rolled back. 
Hence 2020 is returned instead of 1000.

Session 2


use [mytest]
set transaction isolation level read uncommitted
select Salary from Employee where ID=1


If you want to maintain Isolation level "Read Committed" but you want dirty read values for specific 
tables then use with(nolock) in select query for same tables as shown below.


use [mytest]
set transaction isolation level read committed
select * from Employee with(nolock)
------------------------------------------------------------------------------------

------------------------------------------------------------------------------------
Repeatable Read


Repeatable Read
select query data of table that is used under transaction of isolation level 
"Repeatable Read" can not be modified from any other sessions till transaction is completed.

Session 1


use [mytest]
set transaction isolation level repeatable read
begin transaction
select * from Employee where ID in(1,2)
waitfor delay '00:00:30'
select * from Employee where ID in(1,2)
rollback


Repeatable Read

Run both sessions side by side.

Output: 2030

Update command in session 2 will wait till session 1 transaction is completed because emp table row with ID=1 
has locked in session1 transaction.

Session 2


use [mytest]
update Employee set Salary=2030 where ID=1;
select *from Employee;


Note if you update a record that is not selected in session 1, the record is not locked
hence the update query below will execute directly, because session 1 selects ID in(1,2). 
Session 2 will execute without any delay because row with ID=3 is not locked, 
that is only 2 records whose IDs are 1,2 are locked in Session 1.
Session 2


use [mytest]
update Employee set Salary=5050 where ID=3;
select *from Employee;
------------------------------------------------------------------------------------

------------------------------------------------------------------------------------
Repeatable Read


Repeatable Read

Session 1


use [mytest]
set transaction isolation level repeatable read
begin tran
select * from Employee
waitfor delay '00:00:30'
select * from Employee
rollback


Repeatable Read

Session 2 will execute without any delay because it has insert query for new entry. 
This isolation level allows to insert new data but does not allow to modify data that is used in select query executed in transaction.
You can notice two results displayed in Session 1 have different number of row count(1 row extra in sectond result set).

Session 2


use [mytest]
insert into Employee(ID,FirstName,LastName,Salary) values( 11,'Stewart','Gold',11000)
------------------------------------------------------------------------------------

------------------------------------------------------------------------------------
Serializable


Serializable

Serializable Isolation is similar to Repeatable Read Isolation but the difference is it 
prevents Phantom Read. This works based on range lock. If table has index then it locks 
records based on index range used in WHERE clause(like where ID between 1 and 3). 
If table doesn't have index then it locks complete table.
Assume table does not have index column.

Session 1


use [mytest]
set transaction isolation level serializable
begin tran
select * from Employee
waitfor delay '00:00:30'
select * from Employee
rollback


Serializable

Complete Emp table will be locked during the transaction in Session 1. 
Unlike "Repeatable Read", insert query in Session 2 will wait till session 1 execution is completed. 
Hence Phantom read is prevented and both queries in session 1 will display same number of rows.

Session 2


use [mytest]
insert into Employee(ID,FirstName,LastName,Salary) values( 12,'April','Goose',11000)
------------------------------------------------------------------------------------

------------------------------------------------------------------------------------
Serializable

Assume table has primary key on column "ID". In our example script, primary key is not added. 
Add primary key on column Emp.ID before executing below examples.
https://docs.microsoft.com/en-us/sql/relational-databases/tables/create-primary-keys?view=sql-server-ver15
ALTER TABLE [dbo].[Employee]
ALTER COLUMN ID int NOT NULL;

ALTER TABLE [dbo].[Employee]
ADD CONSTRAINT PK_EmployeeID PRIMARY KEY CLUSTERED (ID);

set transaction isolation level serializable
begin tran
select * from Employee where ID between 1 and 3
waitfor delay '00:00:15'
select * from Employee where ID between 1 and 3
rollback

insert into Employee(ID,FirstName,LastName,Salary) values( 12,'April','Goose',11000)

Since Session 1 is filtering IDs between 1 and 3, only those records whose IDs range between 
1 and 3 will be locked and these records can not be modified and no new records with ID range 
between 1 to 3 will be inserted. In this example, new record with ID=11 will be inserted in Session 2 without any delay.
------------------------------------------------------------------------------------

------------------------------------------------------------------------------------
Serializable


Serializable

Session 1


use [mytest]
set transaction isolation level serializable
begin tran
select * from Employee where ID between 1 and 3
waitfor delay '00:00:30'
select * from Employee where ID = 21
rollback


Serializable

Update query will execute because session 1 will lock row ID = 21 after delay.   

Session 2


use [mytest]    
Update Employee set Salary = 75000 where ID = 21;
select *from Employee;
------------------------------------------------------------------------------------

------------------------------------------------------------------------------------
Snapshot


Snapshot

Snapshot isolation is similar to Serializable isolation. The difference is Snapshot does not hold lock on 
table during the transaction so table can be modified in other sessions. Snapshot isolation maintains versioning 
in Tempdb for old data in case of any data modification occurs in other sessions then existing transaction displays 
the old data from Tempdb.

https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql/snapshot-isolation-in-sql-server
Enable SNAPSHOT ISOLATION 
ALTER DATABASE mytest  
SET ALLOW_SNAPSHOT_ISOLATION ON 

Session 1


use [mytest]
set transaction isolation level snapshot
begin transaction
select * from Employee
waitfor delay '00:00:30'
select * from Employee
rollback


Snapshot

Session 2


use [mytest]    
insert into Employee(ID,FirstName,LastName,Salary) values( 17,'Donald','Duck',11000)
select * from Employee
------------------------------------------------------------------------------------

------------------------------------------------------------------------------------
Dirty Read

A dirty read (aka uncommitted dependency) occurs when a transaction is allowed to read data from a row that 
has been modified by another running transaction and not yet committed. Dirty reads work similarly to 
non-repeatable reads; however, the second transaction would not need to be committed for the first query 
to return a different result. The only thing that may be prevented in the READ UNCOMMITTED isolation level 
is updates appearing out of order in the results; that is, earlier updates will always appear in a result 
set before later updates. In our example, Transaction 2 changes a row, but does not commit the changes. 
Transaction 1 then reads the uncommitted data. Now if Transaction 2 rolls back its changes (already read by Transaction 1) 
or updates different changes to the database, then the view of the data may be wrong in the records of Transaction 1.

Session 1

use [mytest]
begin transaction
UPDATE Employee SET Salary = 250000 WHERE ID = 1
waitfor delay '00:00:15'
rollback

Dirty read

Reads two different values because transaction was rolledback in session 1,
meaning a temporary transaction value is read.  

Session 2

use [mytest]
set transaction isolation level read uncommitted
select *from Employee WHERE ID = 1
waitfor delay '00:00:30'
select *from Employee WHERE ID = 1


Note by changing to read committed, we can avoid reading dirty values.   

use [mytest]
set transaction isolation level read committed
select *from Employee WHERE ID = 1
waitfor delay '00:00:30'
select *from Employee WHERE ID = 1
------------------------------------------------------------------------------------

------------------------------------------------------------------------------------ 
NON REPEATABLE READ

A non-repeatable read occurs when, during the course of a transaction, a row is retrieved twice and the values within the row differ between reads.
Non-repeatable reads phenomenon may occur in a lock-based concurrency control method when read locks are not acquired when performing a SELECT, 
or when the acquired locks on affected rows are released as soon as the SELECT operation is performed. Under the multiversion concurrency control 
method, non-repeatable reads may occur when the requirement that a transaction affected by a commit conflict must roll back is relaxed.

In this example, Transaction 2 commits successfully, which means that its changes to the row with id 1 should become visible. 
However, Transaction 1 has already seen a different value for age in that row. At the SERIALIZABLE and REPEATABLE READ isolation levels, 
the DBMS must return the old value for the second SELECT. At READ COMMITTED and READ UNCOMMITTED, the DBMS may return the updated value; this is a non-repeatable read.

There are two basic strategies used to prevent non-repeatable reads. The first is to delay the execution of Transaction 2 until Transaction 1 has committed or rolled back. 
This method is used when locking is used, and produces the serial schedule T1, T2. A serial schedule exhibits repeatable reads behaviour.

Session 1

use [mytest]
begin transaction
waitfor delay '00:00:15'
UPDATE Employee SET Salary = 990000 WHERE ID = 1
commit

NON REPEATABLE READ

Session 2

use [mytest]
set transaction isolation level read committed
begin transaction
select *from Employee WHERE ID = 1;
waitfor delay '00:00:30'
select *from Employee WHERE ID = 1;
commit

Note by changing to repeatable read we can lock rows used in transaction and avoid
non-repeatable reads. The two select queries below results in the same output.   

use [mytest]
set transaction isolation level repeatable read
begin transaction
select *from Employee WHERE ID = 1;
waitfor delay '00:00:30'
select *from Employee WHERE ID = 1;
commit
------------------------------------------------------------------------------------

------------------------------------------------------------------------------------
Phantom reads

A phantom read occurs when, in the course of a transaction, new rows are added or removed by another transaction to the records being read.
This can occur when range locks are not acquired on performing a SELECT ... WHERE operation. The phantom reads anomaly is a special case of 
Non-repeatable reads when Transaction 1 repeats a ranged SELECT ... WHERE query and, between both operations, Transaction 2 creates (i.e. INSERT) 
new rows (in the target table) which fulfil that WHERE clause.
Note that Transaction 1 executed the same query twice. If the highest level of isolation were maintained, the same set of rows should be returned both times, 
and indeed that is what is mandated to occur in a database operating at the SQL SERIALIZABLE isolation level. However, at the lesser isolation levels, 
a different set of rows may be returned the second time. In the SERIALIZABLE isolation mode, Query 1 would result in all records with age in the range 10 to 30 being locked, 
thus Query 2 would block until the first transaction was committed. In REPEATABLE READ mode, the range would not be locked, allowing the record to be inserted and the second 
execution of Query 1 to include the new row in its results.

Session 1

use [mytest]
begin transaction
waitfor delay '00:00:15'
insert into Employee(ID,FirstName,LastName,Salary) values( 007,'James','Bond',700000)
commit

Phantom reads

Session 2

use [mytest]
set transaction isolation level repeatable read
begin transaction
select *from Employee WHERE ID Between 1 and 100;
waitfor delay '00:00:30'
select *from Employee WHERE ID Between 1 and 100;;
commit
------------------------------------------------------------------------------------

References:
View sessions, exec sp_who exec sp_who2 
https://dataedo.com/kb/query/sql-server/list-database-sessions

Isolation (database systems)
https://en.wikipedia.org/wiki/Isolation_(database_systems)

IsolationLevel Enum
https://docs.microsoft.com/en-us/dotnet/api/system.data.isolationlevel?view=netcore-3.1

Transaction types illustrated with SQL
http://www.besttechtools.com/articles/article/sql-server-isolation-levels-by-example

SET TRANSACTION ISOLATION LEVEL (Transact-SQL)
https://docs.microsoft.com/en-us/sql/t-sql/statements/set-transaction-isolation-level-transact-sql?view=sql-server-ver15

Understanding isolation levels
https://docs.microsoft.com/en-us/sql/connect/jdbc/understanding-isolation-levels?view=sql-server-ver15

Snapshot Isolation in SQL Server
https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql/snapshot-isolation-in-sql-server
*/