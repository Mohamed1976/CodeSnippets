using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using CalculatorService;
using PersonLookupService;

namespace _70_483.Exercises
{
    public class GeneralExercises2
    {
        public GeneralExercises2()
        {
        }

        public async Task Run()
        {
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

            return;
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
