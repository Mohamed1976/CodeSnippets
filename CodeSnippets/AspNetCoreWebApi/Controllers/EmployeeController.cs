using AspNetCoreWebApi.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
//using System.Web.Http;

//IActionResult and ActionResult
//https://exceptionnotfound.net/asp-net-core-demystified-action-results/
//https://docs.microsoft.com/en-us/visualstudio/get-started/csharp/tutorial-aspnet-core-ef-step-04?view=vs-2019
namespace AspNetCoreWebApi.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        public EmployeeController(ILogger<EmployeeController> logger, IWebHostEnvironment environment)
        {
            Logger = logger;
            this.environment = environment;
        }

        public ILogger<EmployeeController> Logger { get; }

        /*  curl -X GET "https://localhost:5001/api/Employees" -H "accept: application/json"
        
            content-length: 144 
            content-type: application/json; charset=utf-8 
            date: Sun20 Sep 2020 21:54:42 GMT 
            server: Kestrel 
            status: 200
        
            [
              {
                "id": 1,
                "firstName": "Alfa",
                "lastName": "_Alfa"
              },
              {
                "id": 2,
                "firstName": "Beta",
                "lastName": "_Beta"
              },
              {
                "id": 3,
                "firstName": "Gamma",
                "lastName": "_Gamma"
              }
            ]
        */
        [HttpGet]
        public async Task<IEnumerable<EmployeeDto>> Get()
        {
            Logger.LogInformation("async Task<IEnumerable<EmployeeDto>>Get()");
            /*
            Nuget Microsoft.Extensions.Caching.Memory
            Nuget System.Runtime.Caching
            https://docs.microsoft.com/en-us/dotnet/api/system.runtime.caching.memorycache?redirectedfrom=MSDN&view=netframework-4.8#methods
            https://stackoverflow.com/questions/41505612/memory-cache-in-dotnet-core
            */
            List<EmployeeDto> employees = Cache.Get("EmployeesKey") as List<EmployeeDto>;
            if(employees == null)
            {
                Logger.LogInformation("Adding employees to cache.");
                employees = employeeList;
                Cache.Add(new CacheItem("EmployeesKey", employees), GetVendorPolicy());
                //Cache.Set(new CacheItem("EmployeesKey", employees), GetVendorPolicy());
            }
            else
            {
                Logger.LogInformation("Employees already present in cache.");
            }

            await Task.Delay(100);
            return employees;

            //return new List<EmployeeDto>()
            //{
            //    new EmployeeDto { FirstName = "Alfa", LastName = "_Alfa", id=1 },
            //    new EmployeeDto { FirstName = "Beta", LastName = "_Beta", id=2 },
            //    new EmployeeDto { FirstName = "Gamma", LastName = "_Gamma", id=3 },
            //};
        }

        /* curl -X GET "https://localhost:5001/api/Employees/1" -H "accept: text/plain"
            https://localhost:5001/api/Employees/1

             content-length: 46 
             content-type: application/json; charset=utf-8 
             date: Sun20 Sep 2020 21:57:53 GMT 
             server: Kestrel 
             status: 200 

            {
              "id": 1,
              "firstName": "Alfa",
              "lastName": "_Alfa"
            }
        */
        [HttpGet("{id}")]
        public async Task<EmployeeDto> Get([FromRoute] int id)
        {
            Logger.LogInformation($"async Task<EmployeeDto> Get([FromRoute]int id), id: {id}");
            await Task.Delay(100);

            return employeeList.Where(e => e.id == id).FirstOrDefault();
            //return new EmployeeDto { FirstName = "Alfa", LastName = "_Alfa", id = id };
        }

        private List<EmployeeDto> employeeList = new List<EmployeeDto>()
        {
            new EmployeeDto { FirstName = "Alfa_FirstName", LastName = "Alfa_LastName", id = 0 },
            new EmployeeDto { FirstName = "Beta_FirstName", LastName = "Beta_LastName", id = 1 },
            new EmployeeDto { FirstName = "Gamma_FirstName", LastName = "Gamma_LastName", id = 2 },
            new EmployeeDto { FirstName = "Delta_FirstName", LastName = "Delta_LastName", id = 3 },
            new EmployeeDto { FirstName = "Sigma_FirstName", LastName = "Sigma_LastName", id = 4 },
            new EmployeeDto { FirstName = "Zetta_FirstName", LastName = "Zetta_LastName", id = 5 },
            new EmployeeDto { FirstName = "Ksi_FirstName", LastName = "Ksi_LastName", id = 6 },
        };
        private readonly IWebHostEnvironment environment;

        //Install-Package System.Runtime.Caching
        private ObjectCache Cache 
        {
            get 
            { 
                return System.Runtime.Caching.MemoryCache.Default; 
            } 
        }

        private CacheItemPolicy GetVendorPolicy()
        {
            CacheItemPolicy policy = new CacheItemPolicy();
            //https://docs.microsoft.com/en-us/dotnet/api/system.runtime.caching.memorycache?view=dotnet-plat-ext-3.1
            //https://docs.microsoft.com/en-us/dotnet/api/system.runtime.caching.cacheitempolicy?view=dotnet-plat-ext-3.1
            //Monitor file changes
            policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(5.0); //DateTime.Now.AddMinutes(5);
            policy.ChangeMonitors
                .Add(new HostFileChangeMonitor(GetTriggerPaths()));
            return policy;
        }

        private List<string> GetTriggerPaths()
        {
            List<string> triggerPaths = new List<string>();
            // List<string> GetTriggerPaths(), path: C:\Users\moham\source\repos\CodeSnippets\CodeSnippets\AspNetCoreWebApi\wwwroot\Documents\VendorTrigger.txt
            string path = Path.Combine(environment.WebRootPath, "Documents","VendorTrigger.txt");
            Logger.LogInformation($"List<string> GetTriggerPaths(), path: {path}");
            triggerPaths.Add(path);
            return triggerPaths;
        }

        /* curl -X GET "https://localhost:5001/api/Employees/category/Alfa" -H "accept: text/plain"
           https://localhost:5001/api/Employees/category/Alfa

            {
              "id": 3,
              "firstName": "Gamma",
              "lastName": "_Gamma"
            }

            content-length: 48 
             content-type: application/json; charset=utf-8 
             date: Sun20 Sep 2020 22:00:26 GMT 
             server: Kestrel 
             status: 200 
        */
        [HttpGet("category/{category}")]
        public async Task<EmployeeDto> Get([FromRoute] string category)
        {
            Logger.LogInformation($"async Task<EmployeeDto> Get([FromRoute]string category), " +
                $"category: {category}");
            await Task.Delay(100);

            return new EmployeeDto { FirstName = "Gamma", LastName = "_Gamma", id = 3 };
        }

        /*
            curl -X POST "https://localhost:5001/api/Employees" -H "accept: * / *" -H "Content-Type: application/json" -d "{\"id\":0,\"firstName\":\"Moo\",\"lastName\":\"Kalmoua\"}"
            https://localhost:5001/api/Employees
        
            {
              "id": 3,
              "firstName": "Gamma",
              "lastName": "_Gamma"
            }

             content-length: 48 
             content-type: application/json; charset=utf-8 
             date: Sun20 Sep 2020 22:02:36 GMT
             location: http://example.org/myitem 
             server: Kestrel
             status: 201 
        */
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EmployeeDto employee)
        {
            Logger.LogInformation($"async Task<IActionResult> Post([FromBody]EmployeeDto employee), " +
                $"employee.FirstName: {employee.FirstName}, employee.LastName: {employee.LastName}");
            await Task.Delay(100);

            return Created("http://example.org/myitem",
                new EmployeeDto { FirstName = "Gamma", LastName = "_Gamma", id = 3 });//new { name = "testitem" });
        }

        /*  curl -X PUT "https://localhost:5001/api/Employees/20" -H "accept: * / *" -H "Content-Type: application/json" -d "{\"id\":20,\"firstName\":\"Jo\",\"lastName\":\"Boston\"}"
            https://localhost:5001/api/Employees/20
 
            content-length: 0 
            date: Sun20 Sep 2020 22:07:49 GMT 
            server: Kestrel 
            status: 200 
        */
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] EmployeeDto employee)
        {
            Logger.LogInformation($"async Task<IActionResult> Put([FromRoute]int id, [FromBody]EmployeeDto employee)" +
                $", id: {id}, employee.FirstName: {employee.FirstName}, employee.LastName: {employee.LastName}");
            await Task.Delay(100);

            return Ok();
        }

        /* curl -X DELETE "https://localhost:5001/api/Employees/1" -H "accept: * / *"
            https://localhost:5001/api/Employees/1

             date: Sun20 Sep 2020 22:10:44 GMT
             server: Kestrel
             status: 204 
        */
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            Logger.LogInformation($"async Task<IActionResult> Delete([FromRoute]int id), id: {id}");
            await Task.Delay(100);

            if (id == -1)
            {
                return NotFound();
            }
            else
            {
                return NoContent();
            }
        }

        /* 
        curl -X PUT "https://localhost:5001/api/Employees/Employee/1" -H "accept: * / *" -H "Content-Type: application/json" -d "{\"id\":0,\"firstName\":\"Ralph\",\"lastName\":\"Belleman\"}"
        
        https://localhost:5001/api/Employees/Employee/1
        
        content-length: 0 
        date: Mon21 Sep 2020 20:24:41 GMT 
        server: Kestrel 
        status: 200
        
         If id == 0 then exception 
         content-type: text/plain 
         date: Mon21 Sep 2020 20:27:04 GMT 
         server: Kestrel 
         status: 500 
        */
        [HttpPut("Employee/{id}")]
        public async Task PutEmployee([FromRoute] int id, [FromBody] EmployeeDto employee)
        {
            Logger.LogInformation($"async Task PutEmployee([FromRoute] int id, [FromBody] EmployeeDto employee)" +
                $", id: {id}, employee.FirstName: {employee.FirstName}, employee.LastName: {employee.LastName}");
            await Task.Delay(100);

            if (id < 1)
            {
                throw new System.Web.Http.HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.NotFound));
            }
        }

        /*
        curl -X DELETE "https://localhost:5001/api/Employees/Employee/1" -H "accept: text/plain"
        https://localhost:5001/api/Employees/Employee/1

        {
          "version": "1.1",
          "content": null,
          "statusCode": 204,
          "reasonPhrase": "No Content",
          "headers": [],
          "trailingHeaders": [],
          "requestMessage": null,
          "isSuccessStatusCode": true
        }

         content-length: 160 
         content-type: application/json; charset=utf-8 
         date: Mon21 Sep 2020 20:30:02 GMT 
         server: Kestrel 
         status: 200 

        */
        [HttpDelete("Employee/{id}")]
        public async Task<HttpResponseMessage> 
            DeleteEmployee([FromRoute] int id)
        {
            Logger.LogInformation($"DeleteEmployee([FromRoute] int id, [FromBody] EmployeeDto employee)" +
                $", id: {id}");
            await Task.Delay(100);

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        [HttpPost("ProcesssEmployee")]
        public async Task<HttpResponseMessage>
            ProcesssEmployee([FromBody] EmployeeDto employee)
        {
            Logger.LogInformation($"ProcesssEmployee([FromBody] EmployeeDto: {employee.FirstName} {employee.LastName}");
            Logger.LogInformation($"ContentType: {Request.ContentType}");
            await Task.Delay(100);

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);
            response.Headers.Add("X-Customer", "Moccasoft");

            //How to create a redirection URL
            //https://stackoverflow.com/questions/31617345/what-is-the-asp-net-core-mvc-equivalent-to-request-requesturi
            //var builder = new UriBuilder();
            //builder.Scheme = Request.Scheme;            
            //builder.Host = Request.Host.Value;
            //builder.Path = Request.Path;
            //builder.Query = Request.QueryString.ToUriComponent();
            //Logger.LogInformation($"builder.Uri: {builder.Uri}");
            //response.Headers.Location = builder.Uri;
            //var relativePath = "api/Employees/2";
            //response.Headers.Location = new Uri(relativePath);

            return response;
        }
    }
}