using ASPNETCoreWebApp;
using ASPNETCoreWebApp.Helpers;
using ASPNETCoreWebApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;



//https://xunit.github.io/docs/capturing-output.html
//https://stackoverflow.com/questions/4786884/how-to-write-output-from-a-unit-test
//https://docs.microsoft.com/en-us/visualstudio/test/run-unit-tests-with-test-explorer?view=vs-2019
//https://www.youtube.com/watch?v=QBiBZ8bsfcU
namespace XUnitTestAspNetCore
{
    public class ApiControllerTests
    {
        private BankAccountManager _bankAccountManager = null;
        private readonly ITestOutputHelper output;

        public ApiControllerTests(ITestOutputHelper output)
        {
            this.output = output;
            _bankAccountManager = new BankAccountManager();
        }

        [Fact]
        public void TestBankAccountManager()
        {
            output.WriteLine("Entering TestBankAccountManager().");
            Assert.NotNull(_bankAccountManager);
            _bankAccountManager.AddCustomer("Contoso", 100, 1000, 100, 1);
            output.WriteLine($"_bankAccountManager._name: {_bankAccountManager.Name}");
            Assert.Equal("Contoso", _bankAccountManager.Name);
            output.WriteLine("Exiting TestBankAccountManager().");
        }

        //How to create TestServer
        //You are developing an ASP.NET Core MVC web application. The application is configured to use a Startup class.
        //The status action must be tested on each check-in to source control.
        //https://www.meziantou.net/testing-an-asp-net-core-application-using-testserver.htm
        //https://andrewlock.net/converting-integration-tests-to-net-core-3/
        [Fact]
        public async Task TestPatientController()
        {
            string result = null;

            try
            {
                output.WriteLine("Entering TestPatientController().");
                var webHostBuilder = new WebHostBuilder()
                    .UseEnvironment("Test") // You can set the environment you want (development, staging, production)
                    .UseStartup<Startup>(); // Startup class of your web app project

                output.WriteLine("WebHostBuilder created");
                using (var server = new TestServer(webHostBuilder))
                {
                    using (var client = server.CreateClient())
                    {
                        result = await client.GetStringAsync("/api/Patients");
                        output.WriteLine($"Response: {result}");                        
                        //IEnumerable<Patient> Deserialize
                        //ActionResult<IEnumerable<Patient>>
                        //Assert.AreEqual("[\"value1\",\"value2\"]", result);
                    }
                }

                output.WriteLine("Exiting TestPatientController().");

            }
            catch (Exception ex)
            {
                output.WriteLine($"Exception in TestPatientController(): {ex.Message}");
                //throw;
            }

            Assert.NotNull(result);
        }
    }
}
