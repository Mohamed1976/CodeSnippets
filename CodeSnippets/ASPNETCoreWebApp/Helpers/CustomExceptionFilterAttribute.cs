using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

//The exception filter implements the IExceptionFilter interface that has a single method, OnException, 
//which is called when an unhandled exception is thrown within the action method.In keeping with the 
//logging example, it is another good example of when a custom filter that manages logging the exception is appropriate.

namespace ASPNETCoreWebApp.Helpers
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited =false)]
    //Exception An exception filter implements the System.Web.Mvc.IExceptionFilter and
    //is run when there is an unhandled exception thrown in the processing of an action
    //method.It covers the whole lifetime of the action, from the initial authorization filters
    //through the result filter.
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute, IExceptionFilter
    {
        public override void OnException(ExceptionContext context)
        {
            // Do something before the action executes.
            Debug.WriteLine("CustomExceptionFilterAttribute.OnException(), " 
                + MethodBase.GetCurrentMethod() + ", " + context.HttpContext.Request.Path
                +" Exception message: " + context.Exception.Message);
            base.OnException(context);
        }
    }
}
