using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCoreWebApp.Helpers
{
    //https://docs.microsoft.com/en-us/aspnet/core/web-api/action-return-types?view=aspnetcore-3.1
    //https://docs.microsoft.com/en-us/aspnet/core/web-api/advanced/formatting?view=aspnetcore-3.1
    public class ExcelResult : IActionResult
    {
        public string Path { get; set; }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            var request = context.HttpContext.Request;

            string acceptTypes = request.Headers["Accept"];
            var canProcess = acceptTypes.Contains("text/html");

            if(canProcess)
            {
                response.Headers.Add("Content-Disposition", "attachment; filename=myfile.csv");
                response.ContentType = "text/html";
                //We can read content from wwwroot "~/Documents/myfile.txt"
                await response.WriteAsync("A message written to the Stream.");
                //response.StatusCode = StatusCodes.Status200OK;
            }

            ////Could be improved 
            ////bool hasAcceptHeader = HttpContext.Request.Headers.ContainsKey("Accept");
            //var acceptHeaderValue = HttpContext.Request.Headers["Accept"].ToString();
            //if (canProcess)
            //    ViewData["accept"] = acceptHeaderValue + ", canProcess";
            //else
            //    ViewData["accept"] = acceptHeaderValue;
            ////HttpContext.Response.Headers.Add("Content-Disposition", "attachment; filename=myfile.csv");
            ////await HttpContext.Response.WriteAsync("A message written to the Stream.");
            ////HttpContext.Response.ContentType = "text/html";
            //var canProcess = request.
            //throw new NotImplementedException();
        }
    }
}
