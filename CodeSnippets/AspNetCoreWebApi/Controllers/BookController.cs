using AspNetCoreWebApi.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace AspNetCoreWebApi.Controllers
{
    [Route("[controller]s")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly ILogger<BookController> logger;

        public BookController(ILogger<BookController> logger)
        {
            this.logger = logger;
        }

        /* Request URL https://localhost:5001/Books
         * 
         * curl -X POST "https://localhost:5001/Books" -H "accept: text/plain" -H "Content-Type: application/json" -d "{\"title\":\"Lord of the Rings\"}"
         * 
         * Response body:
         * {
              "version": "1.1",
              "content": null,
              "statusCode": 201,
              "reasonPhrase": "Created",
              "headers": [],
              "trailingHeaders": [],
              "requestMessage": null,
              "isSuccessStatusCode": true
            }

            Response headers:
            content-length: 157 
            content-type: application/json; charset=utf-8 
            date: Fri11 Sep 2020 12:38:45 GMT 
            server: Kestrel 
            status: 200
        */
        [HttpPost]
        public HttpResponseMessage Post([FromBody] BookDto book)
        {
            logger.LogInformation($"Post([FromBody] BookDto book): {book.Title}");

            var response = new HttpResponseMessage(HttpStatusCode.Created);
            return response;
        }
        
        /*
        https://localhost:5001/Books/GetObject
        curl -X GET "https://localhost:5001/Books/GetObject" -H "accept: * / *"

        {
            "name": "Fabrikam",
            "vendorNumber": 9823,
            "items": [
                "Apples",
                "Oranges"
            ]
        }

        content-length: 68 
        content-type: application/json; charset=utf-8 
        date: Fri11 Sep 2020 13:55:31 GMT
        server: Kestrel
        status: 200 
        */
        [HttpGet]
        [Route("GetObject")]
        public object get()
        {
            var obj = new
            {
                Name = "Fabrikam",
                VendorNumber = 9823,
                Items = new List<string>() 
                {
                    "Apples", 
                    "Oranges"
                }
            };

            return obj;
        }
    }
}
