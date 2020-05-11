using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

//Where the filters enable you to program your application to customize information 
//in and out of the controller process, there is also a way for you to create your 
//own controllers and the factory that manufactures them.Creating a custom controller 
//factory enables you to take control of creating your controllers.The most common reason 
//for doing this is to support Dependency Injection (DI) and Inversion of Control (IoC). 
//While many of the major IoC containers have several projects that provide a custom 
//controller factor for your application already available in NuGet, you might still 
//need to create your own to support custom IoC implementations or configurations.Another 
//reason to create a custom controller factory is if you need to pass in a service reference 
//or data repository information—basically, any time you need to create a controller in a way 
//that is not supported by the basic functionality. Creating a custom ControllerFactory class 
//requires that you implement System.Web.Mvc.IControllerFactory.This method has three methods: 
//CreateController, ReleaseController, and GetControllerSessionBehavior. The CreateController 
//method handles the actual control creation, so if you were creating a customized constructor, 
//this is where your code would participate.The ReleaseController method cleans the controller up.
//In some cases, you might be freeing up your IoC container; in other cases, you might be logging 
//out a service connection.The GetControllerSessionBehavior method enables you to define and control 
//how your controller works with session. After you create your own ControllerFactory class, 
//you need to register it for use.You can add the following code to the Global.asax Application_Start method:

//ControllerBuilder.Current.SetControllerFactory(
//typeof(MyCustomControllerFactory());

namespace ASPNETWebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
