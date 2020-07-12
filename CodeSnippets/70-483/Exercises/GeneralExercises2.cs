using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using CalculatorService;
using PersonLookupService;
using System.Security.Cryptography;
using DataLibrary.Data;
using System.Linq;

namespace _70_483.Exercises
{
    public class GeneralExercises2
    {
        public GeneralExercises2()
        {
        }

        public string FileHash(string filename, string hashAlgorithm)
        {
            HashAlgorithm hashAlgorithm1 = HashAlgorithm.Create(hashAlgorithm);
            byte[] content = File.ReadAllBytes(filename);
            byte[] hash = hashAlgorithm1.ComputeHash(content);
            return BitConverter.ToString(hash).Replace("-", "");
        }

        // https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.getfiles?view=netcore-3.1
        // Compares all files in the sourceDirectory, 
        // Checks if file is duplicate, if so moves it to the targetDirectory
        public void ProcessDirectory(string[] sourceDirectories, string targetDirectory)
        {
            const string hashAlgorithm = "SHA256";
            Dictionary<string, string> files = new Dictionary<string, string>();

            foreach (string sourceDirectory in sourceDirectories)
            {
                // Make a reference to a directory.
                DirectoryInfo di = new DirectoryInfo(sourceDirectory);
                FileInfo[] fileEntries = di.GetFiles();
                // Process the list of files found in the directory.
                foreach (FileInfo fileName in fileEntries)
                {
                    string hash = FileHash(fileName.FullName, hashAlgorithm);
                    if (files.ContainsKey(hash))
                    { //Duplicate file move to targetDirectory  
                        FileInfo sourceFile = new FileInfo(files[hash]);
                        Console.WriteLine($"Duplicate file found:");
                        Console.WriteLine($"\tSource: {sourceFile.Name}, size: {sourceFile.Length}");
                        Console.WriteLine($"\tDuplicate: {fileName.Name}, size: {fileName.Length}");
                        string destination = Path.Combine(targetDirectory, fileName.Name);
                        //Check if file already exists
                        if(File.Exists(destination)) //Create unique name
                        {
                            destination = Path.Combine(targetDirectory, Path.GetRandomFileName() + "_" + fileName.Name);
                        }
                        Console.WriteLine($"\tMoved to: {destination}");                        
                        fileName.MoveTo(destination);
                    }
                    else // Unique file, add hash and path to dictionary
                    {
                        files.Add(hash, fileName.FullName);
                    }
                }
            }
        }
        
        private readonly string[] sourceDirectory =
        {
            @"C:\Users\moham\Desktop\70-486\Exercises",
            @"C:\Users\moham\Desktop\70-486\Duplicate1",
            @"C:\Users\moham\Desktop\70-486\Duplicates",
        };

        private const string targetDirectory = @"C:\Users\moham\Desktop\70-486\Remove";

        public async Task Run()
        {
            Console.WriteLine("public async Task Run() @Work.");
            //Create a .Net standard library to access database data
            //DataLibrary can then be used in multiple projects 
            //https://rajbos.github.io/blog/2020/04/23/EntityFramework-Core-NET-Standard-Migrations
            //https://stackoverflow.com/questions/52517607/how-to-install-entity-framework-of-standard-library
            AdventureWorksContext adventureWorksContext = new AdventureWorksContext();
            var customers = adventureWorksContext.Customer.ToList();

            foreach (var customer in customers)
            {
                Console.WriteLine($"\tFirstName: {customer.FirstName}, LastName: {customer.LastName}");
            }

            return;
            //--------------------------------------------------------------------------------------------------
            // Check for duplicate files in folder, if so move them to the remove directory  
            // Check if files have the same content is done using hash function
            //--------------------------------------------------------------------------------------------------
            ProcessDirectory(sourceDirectory, targetDirectory);
            Console.WriteLine("Finished processing files.");

            //--------------------------------------------------------------------------------------------------
            // Consuming ASMX Web Services in ASP.NET Core
            // https://github.com/dotnet/wcf/blob/master/release-notes/WCF-Web-Service-Reference-notes.md
            // https://thecodebuzz.com/consuming-asmx-web-services-in-asp-net-core/
            // http://www.dneonline.com/calculator.asmx?WSDL
            //--------------------------------------------------------------------------------------------------
            CalculatorSoapClient client = new CalculatorSoapClient(CalculatorSoapClient.EndpointConfiguration.CalculatorSoap);
            int result = await client.AddAsync(5, 8);
            Console.WriteLine($"5 + 8 = {result}");

            result = await client.MultiplyAsync(5, 8);
            Console.WriteLine($"5 * 8 = {result}");

            //https://www.crcind.com/csp/samples/SOAP.Demo.CLS?WSDL=1
            SOAPDemoSoapClient client1 = new SOAPDemoSoapClient(SOAPDemoSoapClient.EndpointConfiguration.SOAPDemoSoap);
            PersonLookupService.Person person1 = await client1.FindPersonAsync("1");
            Console.WriteLine($"Name: {person1.Name}");
            Console.WriteLine($"City: {person1.Home.City}");
            Console.WriteLine($"Street: {person1.Home.Street}");
            
            List<Teacher> persons = new List<Teacher>()
            {
                new Teacher("David"),
                new Teacher("Smith"),
                new Teacher("Wolf"),
                new Teacher("John"),
            };

            //https://blog.elmah.io/export-data-to-excel-with-asp-net-core/
            //Create/export data as Excel file (XLSX, XML based file)
            //nuget packages, closedxml(IMO), EPPlus and documentformat.openxml from microsoft
            using (XLWorkbook workbook = new XLWorkbook())
            {
                //Create CSV
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Id,Username");

                IXLWorksheet worksheet = workbook.Worksheets.Add("Users");
                int currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Id";
                worksheet.Cell(currentRow, 2).Value = "Username";

                foreach (Teacher person in persons)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = person.ID;
                    worksheet.Cell(currentRow, 2).Value = person.Username;
                    sb.AppendLine(person.ID+","+ person.Username);
                }

                using (FileStream stream = new FileStream("users.xlsx", FileMode.OpenOrCreate, FileAccess.Write))
                {
                    workbook.SaveAs(stream);
                }

                using (FileStream stream2 = new FileStream("users.csv",FileMode.OpenOrCreate, FileAccess.Write))
                {
                    byte[] content = Encoding.UTF8.GetBytes(sb.ToString());
                    stream2.Write(content, 0, content.Length);
                }
            }

            Console.WriteLine("Finished writing Excel data.");

            //In case you return IActionResult you need to set contenttype
            //return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "users.csv");

            //return File(
            //    content,
            //    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            //    "users.xlsx");
        }
    }

    public interface ITeacher
    {
        int ID { get; }
        string Username { get; }
    }

    public struct Teacher : ITeacher
    {
        private static int _id;

        //static constructor
        static Teacher()
        {
            Console.WriteLine("\"static Person()\" is called when first instance is created.");
            _id = 0;
        }

        public Teacher(string username)
        {
            ID = _id++;
            Username = username;
        }

        public int ID { get; private set; }
        public string Username { get; private set; }
    }
}
