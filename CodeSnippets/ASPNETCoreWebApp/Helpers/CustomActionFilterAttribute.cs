using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

//The action filter is one of the more commonly customized filters.The IActionFilter interface supports 
//two methods: OnActionExecuting, which is called prior to the action being called; and OnActionExecuted, 
//which is called after the execution completes.Standardized logging that captures information about action 
//methods that are called is a good example of a custom action filter that might useful in an ASP.NET MVC 
//application. Some filters might implement both methods to catch the context on its way into the action 
//and on the way out, whereas others might need to implement code only in one and let the other flow through 
//to the base filter.

//The result filter enables you to manipulate the results before and after the action results are executed.
//The IResultFilter supports two methods: OnResultExecuting, which is called before an action result is executed; 
//and the OnResultExecuted, which is called upon completion of the action result’s execution.This filter lets 
//you do special work in the view or as an extension to the previous logging example, logging which views are 
//rendered and how long it took for them to complete processing.

//Action filters provide one of the most robust customization points in ASP.NET MVC.Being
//able to apply the same behavior to multiple actions through attribution allows a high degree
//of flexibility.You should be familiar with the ActionFilterAttribute class and its various methods.

//The last attribute to discuss is the ActionFilterAttribute.It isn’t a true attribute; it is the
//abstract class upon which action filters are based.This class enables the creation of custom
//action filters or any kind of class that you want to be able to act as an attribute on an action.
//The four primary methods available for override in a customized action filter are the following,
//in order of execution:
//■■ OnActionExecuting Called before the action is called.It gives you the opportunity
//to look at information within the HttpContext and make decisions about whether the
//process should continue to be processed.
//■■ OnActionExecuted Enables you look at the results of an action and determine
//whether something needs to happen at that point.
//■■ OnResultExecuting Called before the action result from the action is processed.
//■■ OnResultExecuted Called after the action result is processed but before the output
//is loaded into the response stream.

//Action filters enable you to mark the attribute as allowed to be run only once or can be
//run multiple times.The InitializeSimpleMembershipAttribute is a good example of a filter that
//should be run only once. It initializes the database to ensure that the application can reach
//the database and that the database schema is correct.You can mark a custom filter to be run
//only once through the AllowMultiple parameter in the AttributeUsage attribute on the filter
//class: AllowMultiple = false.

namespace ASPNETCoreWebApp.Helpers
{
    //https://stackoverflow.com/questions/53867875/net-core-action-filter-is-not-a-an-attribute-class
    //https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/filters?view=aspnetcore-3.1#action-filters
    [AttributeUsage(AttributeTargets.Class| AttributeTargets.Method, AllowMultiple =false, Inherited =false)]
    //The attributes are based on the System.Web.Mvc.FilterAttribute class, and the basic set of attributes 
    //enables a developer to put some business logic around the flow of the application.

    //Action An action filter implements the System.Web.Mvc.IActionFilter and enables
    //the developer to wrap the execution of the action method.It also enables the system
    //to perform an additional workaround, providing extra information into the action
    //method; or it can inspect the information coming out of the action and also cancel an
    //action methods execution.

    //Result A result filter implements the System.Web.Mvc.IResultFilter and is a wrapper
    //around an action result.It enables the developer to do extra processing of the results
    //from an action method.

    public class CustomActionFilterAttribute : ActionFilterAttribute, IActionFilter, IResultFilter//, IActionFilter, ActionFilterAttribute already implements this interface
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            // Do something before the action executes.
            Debug.WriteLine("CustomActionAttribute.OnActionExecuted(), " + MethodBase.GetCurrentMethod() + ", "+ context.HttpContext.Request.Path);
            base.OnActionExecuted(context);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Do something before the action executes.
            Debug.WriteLine("CustomActionAttribute.OnActionExecuting(), " + MethodBase.GetCurrentMethod() + ", " + context.HttpContext.Request.Path);
            base.OnActionExecuting(context);
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            Debug.WriteLine("CustomActionAttribute.OnResultExecuting(), " + 
                MethodBase.GetCurrentMethod() + ", " + context.HttpContext.Request.Path);
            //context.HttpContext.Response.Headers.Add(_settings.Title,
            //                                         new string[] { _settings.Name });
            base.OnResultExecuting(context);
        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            Debug.WriteLine("CustomActionAttribute.OnResultExecuted(), " +
                MethodBase.GetCurrentMethod() + ", " + context.HttpContext.Request.Path);
            // Can't add to headers here because response has started.
            //_logger.LogInformation("AddHeaderResultServiceFilter.OnResultExecuted");
            base.OnResultExecuted(context);
        }
    }
}
