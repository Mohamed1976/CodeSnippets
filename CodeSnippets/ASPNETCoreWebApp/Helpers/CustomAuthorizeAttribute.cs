using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//The AuthorizeAttribute gives you control over whether the user must be authenticated before
//being able to take the decorated action.It can be modified to check for authorization as well
//by checking to see whether the user has roles that are in an accepted list: Authorize(Roles =
//Admin, PowerUser).

//In many cases, the AuthorizeAttribute is applied globally so
//that all actions in all controllers require that the user be authorized, with the few that do not
//require authorization being marked with AllowAnonymous.

//Authorization filters, for example, enable you to do custom work around authentication and authorization.
//Suppose that your company was just purchased by another company.Users from the purchasing company need 
//to be able to access your application, but the user stores haven’t been merged.They offer you a token 
//service, where the purchasing company’s proxy server adds a token to the header of the HttpRequest. 
//You need to call a token service to verify that the token is still authorized. This functionality 
//can be done in multiple ways, but a customized AuthorizationAttribute class enables you to apply 
//the functionality on those actions or controllers as needed.The IAuthorizationFilter interface 
//implements a single method, OnAuthorization, which is called when authorization is required.


//https://www.craftedforeveryone.com/adding-your-own-custom-authorize-attribute-to-asp-net-core-2-2-and-above/
namespace ASPNETCoreWebApp.Helpers
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    //Authorization An authorization filter implements the System.Web.Mvc.IAuthorizationFilter
    //and makes a security-based evaluation about whether an action method should be executed, 
    //and it can perform custom authentication or other security needs and evaluations.
    public class CustomAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public CustomAuthorizeAttribute()
        {
        }

        public string Permissions { get; set; } //Permission string to get from controller

        public void OnAuthorization(AuthorizationFilterContext context)
        {

            //context.Result = new UnauthorizedResult();
            //context.Result
            //context.Result = new UnauthorizedResult();

            //context.Result = new RedirectResult("~/Account/Login");
            //filterContext.Result = new RedirectResult("~/Account/AccessDenied");
            ////throw new NotImplementedException();
            ////Validate if any permissions are passed when using attribute at controller or action level
            //if (string.IsNullOrEmpty(Permissions))
            //{
            //    //Validation cannot take place without any permissions so returning unauthorized
            //    context.Result = new UnauthorizedResult();
            //    return;
            //}

            ////The below line can be used if you are reading permissions from token
            ////var permissionsFromToken=context.HttpContext.User.Claims.Where(x=>x.Type=="Permissions").Select(x=>x.Value).ToList()

            ////Identity.Name will have windows logged in user id, in case of Windows Authentication
            ////Indentity.Name will have user name passed from token, in case of JWT Authenntication and having claim type "ClaimTypes.Name"
            //var userName = context.HttpContext.User.Identity.Name;
            //var assignedPermissionsForUser = MockData.UserPermissions.Where(x => x.Key == userName).Select(x => x.Value).ToList();

            //var requiredPermissions = Permissions.Split(","); //Multiple permissiosn can be received from controller, delimiter "," is used to get individual values
            //foreach (var x in requiredPermissions)
            //{
            //    if (assignedPermissionsForUser.Contains(x))
            //        return; //User Authorized. Wihtout setting any result value and just returning is sufficent for authorizing user
            //}

            //context.Result = new UnauthorizedResult();
            //return;
        }
    }
}
