using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace _70_483.ConsumeData
{
    public class ConsumeDataExamples
    {
        public ConsumeDataExamples()
        {

        }

        //The DbConnection class that represents a connection to a database is an abstract class 
        //that describes the behaviors of the connection. The SqlConnection class is a child of //
        //the DbConnection class and represents the implementation of a connection to an SQL database.
        private async Task<string> DumpDatabase(string connectionString)
        {
            //The connection string contains a number of items expressed as name - value
            //pairs.For a server on a remote machine the connection string will contain the
            //address of the server, the port on which the server is listening and a
            //username / password pair that can authenticate the connection.
            //Note that is important that the connection string is not “hard wired” into program source code.
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand sqlCommand = new SqlCommand("SELECT *FROM [AdventureWorksLT2017].[SalesLT].[Customer]", connection);
                //The SqlReader provides methods that can be used to move through the
                //results returned by the query.Note that it is only possible to move forward
                //through the results. The Read method moves the reader on to the next entry in
                //the results. The Read method returns False when there are no more results.
                //The individual items in the element can be accessed using their name.
                SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

                StringBuilder databaseList = new StringBuilder();
                while (reader.Read())
                {
                    string firstName = reader["FirstName"].ToString();
                    string lastName = reader["LastName"].ToString();
                    string companyName = reader["CompanyName"].ToString();
                    databaseList.AppendLine(string.Format("FirstName: {0}, LastName: {1}, CompanyName: {2}",
                        firstName, lastName, companyName));
                    //Console.WriteLine("FirstName: {0}, LastName: {1}, CompanyName: {2}",
                    //    firstName, lastName, companyName);
                }

                return databaseList.ToString();

                //SELECT *FROM [AdventureWorksLT2017].[SalesLT].[Customer] WHERE FirstName='Rob'
                //A program can use the SQL UPDATE command to update the contents of an
                //entry in the database.
                //UPDATE [AdventureWorksLT2017].[SalesLT].[Customer] SET FirstName='Moo' Where ID = '2020'
                //When the update is performed it is possible to determine how many elements are updated.
                //int result = updateCommand.ExecuteNonQuery();, returns number of rows effected by update 
            }
        }

        //Adding appsettings.json to a .NET Core console app
        //https://blog.bitscry.com/2017/05/30/appsettings-json-in-net-core-console-app/
        //All that’s required is to add the following NuGet packages and an appsettings.json file.
        //Microsoft.Extensions.Configuration
        //Microsoft.Extensions.Configuration.FileExtensions
        //Microsoft.Extensions.Configuration.Json
        //The appsettings.json files “Copy to Output Directory” property should also be set to 
        //“Copy if newer” so that the application is able to access it when published.
        private string GetConnectionString()
        {
            //This solution has a setting file appsettings.Development.json that contains custom settings for use
            //during development.If you add an appsettings.Production.json file
            //to the solution, you can create settings information that will be used when the
            //program is running on a production server.
            //The setting information in a solution can contain the descriptions of
            //environments that are to be used for development and production deployments of
            //an application. The environments used for application deployment set out the
            //database connection string and any other options that you would like to
            //customize.For example, the development environment can use a local database
            //server and the production environment can use a distant server.
            //The setting information to be used is when a server is started and determined
            //by an environment variable on the computer that is tested when the program starts running.

            //In older ASP.NET applications the SQL settings are held in the web.config
            //file, which is part of the solution. Developers then use XML transformations to
            //override settings in the file to allow different SQL servers to be selected.

            string settingsFile = $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"}.json";
            Debug.WriteLine(settingsFile);

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(settingsFile, optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            string connectionString = configuration.GetConnectionString("AdventureworksContext");
            Debug.WriteLine(connectionString);
            return connectionString;
        }

        //A malicious user can use SQL injection to take control of a database.
        //For this reason, you should never construct SQL commands directly from user
        //input. You must use parameterized SQL statements instead.
        //The SQL command contains markers that identify the points in
        //the query where text is to be inserted.The program then adds the parameter
        //values that correspond to the marker points.The SQL server now knows exactly
        //where each element starts and ends, making SQL injection impossible.
        private async Task DumpDatabaseUsingFirstName(string connectionString, string firstNameSelected)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand sqlCommand = new 
                    SqlCommand("SELECT *FROM [AdventureWorksLT2017].[SalesLT].[Customer] " +
                    "WHERE FirstName=@FirstName", connection);
                sqlCommand.Parameters.AddWithValue("@FirstName", firstNameSelected);
                SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

                while (reader.Read())
                {
                    string firstName = reader["FirstName"].ToString();
                    string lastName = reader["LastName"].ToString();
                    string companyName = reader["CompanyName"].ToString();
                    Console.WriteLine("FirstName: {0}, LastName: {1}, CompanyName: {2}",
                        firstName, lastName, companyName);
                }
            }

            //string SqlCommandText = "UPDATE MusicTrack SET Artist=@artist WHERE Title=@searchTitle";
            //SqlCommand command = new SqlCommand(SqlCommandText, connection);
            //command.Parameters.AddWithValue("@artist", newArtist);
            //command.Parameters.AddWithValue("@searchTitle", searchTitle);
        }

        const string XMLDocument = "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
            "<MusicTrack xmlns:xsi=\"http://www.w3.org/2001/XMLSchemainstance\" " +
            "xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" +
            "<Artist>Rob Miles</Artist>" +
            "<Title>My Way</Title>" +
            "<Length>150</Length>" +
            "</MusicTrack>";

        public async Task Run()
        {
            //XMLDocument creates a Document Object Model (DOM)
            //in memory from which data can be extracted.Another advantage of a DOM is
            //that a program can change elements in the DOM and then write out a modified
            //copy of the document incorporating the changes.
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(XMLDocument);
            XmlElement rootElement = xmlDocument.DocumentElement;
            Console.WriteLine("RootElement: {0}", rootElement.Name);
            string artist = rootElement["Artist"].FirstChild.Value;
            string title = rootElement["Title"].FirstChild.Value;
            int length = Convert.ToInt32(rootElement["Length"].FirstChild.Value);
            Console.WriteLine("Artist:{0} Title:{1} Length:{2}", artist, title, length);

            //The XML language is slightly more expressive than JSON, but
            //XML documents are larger than equivalent JSON documents and this, along
            //with the ease of use of JSON, has led to JSON replacing XML.
            //One way to consume XML documents is to work through each element in
            //turn.The System.XML namespace contains a set of classes for working with
            //XML documents.One of these classes is the XMLTextReader class. An
            //instance of the XMLTextReader class will work through a stream of text and
            //decode each XML element in turn.
            //The example below creates a StringReader stream that is used to construct an XMLTextReader.The
            //program then iterates through each of the XML elements that are read from the
            //stream, printing out the element information.
            using (StringReader stringReader = new StringReader(XMLDocument))
            {
                using (XmlTextReader reader = new XmlTextReader(stringReader))
                {
                    //Whitespace characters are also shown as elements
                    while (reader.Read())
                    {
                        string description = string.Format("Type: {0}, Name:{1}, Value: {2}",
                            reader.NodeType.ToString(),
                            reader.Name,
                            reader.Value);
                        Console.WriteLine(description);
                    }
                }
            }
            
            //Read data from database, you need to install System.Data.SqlClient
            //AdventureWorks sample databases can be downloaded here: https://github.com/microsoft/sql-server-samples/releases 
            string ConnectionString = GetConnectionString();

            try
            {
                await DumpDatabaseUsingFirstName(ConnectionString, "Jessie");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occured while calling DumpDatabaseUsingFirstName(): {0}.", ex.Message);
            }
            
            try
            {
                string results = await DumpDatabase(ConnectionString);
                Console.WriteLine(results);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occured while calling DumpDatabase(): {0}.",ex.Message);
            }

            //Consume JSON and XML data
            //JSON (JavaScript Object Notation) is a means by which applications can exchange data. A JSON
            //document is a plain text file that contains a structured collection of name and value pairs.
            //(Newtonsoft.Json library.).
            //The website http://json2csharp.com. which will accept a web address that returns
            //a JSON document and then automatically generates a C# class as described in the document.
            //Wrap webclient call in try/catch 
            try
            {
                ImageOfDay imageOfDay = await GetImageOfDay();
                if (imageOfDay.media_type == "image")
                {
                    Console.WriteLine(imageOfDay);
                }
                else
                {
                    Console.WriteLine("It is not an image today");
                }                            
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error occured while calling GetImageOfDay(): {0}.", ex.Message); 
            }
        }

        private const string ImageOfDayUrl = "https://api.nasa.gov/planetary/apod?api_key=DEMO_KEY";

        private async Task<ImageOfDay> GetImageOfDay()
        {
            Debug.WriteLine("Entering: private async Task<ImageOfDay> GetImageOfDay(string imageURL)");
            Debug.Indent();
            WebClient webClient = new WebClient();
            string url = ImageOfDayUrl + "&date=" + DateTime.Now.ToString("yyyy-MM-dd");
            Debug.WriteLine("Download url: " + url);
            string JSONContent = await webClient.DownloadStringTaskAsync(url);
            Debug.WriteLine("JSON response: " + JSONContent);
            ImageOfDay imageOfDay = JsonConvert.DeserializeObject<ImageOfDay>(JSONContent);
            Debug.Unindent();
            Debug.WriteLine("Exiting: private async Task<ImageOfDay> GetImageOfDay(string imageURL)");
            return imageOfDay;
        }
    }
}
