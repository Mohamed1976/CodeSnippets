using ASPNETCoreWebApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCoreWebApp.Filters
{
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute//, IExceptionFilter
    {
        //private readonly IHostingEnvironment _hostingEnvironment;
        //private readonly IModelMetadataProvider _modelMetadataProvider;

        //public CustomExceptionFilterAttribute(IHostingEnvironment hostingEnvironment,
        //    IModelMetadataProvider modelMetadataProvider)
        //{
        //    _hostingEnvironment = hostingEnvironment;
        //    _modelMetadataProvider = modelMetadataProvider;
        //}

        public override void OnException(ExceptionContext context)
        {
            Debug.WriteLine("Entering CustomExceptionFilterAttribute, OnException(ExceptionContext context)");
            Debug.WriteLine(context.ToString());

            var result = new ViewResult { ViewName = "CustomError" };
            var modelMetadata = new EmptyModelMetadataProvider();
            result.ViewData = new ViewDataDictionary( modelMetadata, context.ModelState);
            result.ViewData.Add("HandleException", context.Exception);
            result.ViewData.Add("Message", "Message from CustomExceptionFilterAttribute");

            context.Result = result;
            context.ExceptionHandled = true;
            base.OnException(context);

            //var result = new ViewResult()
            //{
            //    ViewName = "CustomError", 
            //};
            //context.Result = result;
        }
    }
}
