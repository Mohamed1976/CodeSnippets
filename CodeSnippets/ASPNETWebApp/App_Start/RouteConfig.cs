using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

//https://stackoverflow.com/questions/10668105/routing-the-current-request-for-action-is-ambiguous-between-the-following
//You can only have a maximum of 2 action methods with the same name on a controller, and in order to do that, 
//1 must be[HttpPost], and the other must be[HttpGet]. Since both of your methods are GET, you should either 
//rename one of the action methods or move it to a different controller. Though your 2 Browse methods are 
//valid C# overloads, the MVC action method selector can't figure out which method to invoke. 
//It will try to match a route to the method (or vice versa), and this algoritm is not strongly-typed.
//You can accomplish what you want using custom routes pointing to different action methods:

//https://www.danylkoweb.com/Blog/aspnet-mvc-routing-examples-JQ
//https://moz.com/blog/15-seo-best-practices-for-structuring-urls
//Routing is one of the fundamental concepts for ASP.NET MVC web applications.
//When designing URLS take into account (How people see your site, How search engines see your site):
//➤ A domain name that is easy to remember and easy to spell
//➤ Short URLs
//➤ Easy-to-type URLs
//➤ URLs that refl ect the site structure
//➤ URLs that are hackable to allow users to move to higher levels of the information 
//   architecture hacking off the end of the URL
//➤ Persistent URLs, which don’t change

namespace ASPNETWebApp
{
    public class RouteConfig
    {
        //One important point,"always place the more specific routes before the more general routes" 
        //since the order in which we register the routes is respected by the routing engine.
        //So if we place the default route before the route that we have registered above our 
        //action method will never get called.Avoiding this mistake can save us a lot of debugging effort later on.
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //https://stackoverflow.com/questions/47887384/routeprefix-does-not-work-asp-net-mvc
            //Needed for ProductController 
            routes.MapMvcAttributeRoutes();

            const string DefaultName = "United Colors Of Benetton";

            //You need to update the routes to ensure that a Article is always displayed on the Article page.
            //So if you choose localhost/article, you would see the default productName 
            routes.MapRoute(
                name: "Article",
                url: "Article/{action}/{productName}",
                defaults: new { controller = "Article", action = "Show", productName = DefaultName });

            //The GetBagel action is the only action that should be accessed via a URL pattern. 
            //Routes to the other actions in the controller must be suppressed.
            routes.MapRoute(
                name: "Bagels",
                url: "Bagel/GetBagel/{bagelName}",
                defaults: new { controller = "Bagel", action = "GetBagel" }
            );

            routes.IgnoreRoute("Bagel/{*pathInfo}");

            //An Ignore should always be added to the route collection before any routes are identified, 
            //or it is possible your Ignore will not be reached because the route handler has already 
            //matched the URL and made the call to the action.However, because Ignores are generally 
            //added to the collection first, it is easy to unintentionally cut off access to parts 
            //of your application because the request matches the Ignore item before it matches 
            //the appropriate route.

            //The use of Ignore can be a flexible addition to your site predictability and security.Assume
            //that you have user documentation in a directory. You do not want to activate NTFS file system
            //(NTFS) permissions on that directory to limit access, but would rather serve users through an
            //action method that returns a FileContentResult so you can log which user wants which document.
            //By putting an Ignore on all PDF files, or in the directory that holds those files, you can
            //restrict access to the files.

            //Consider the following example, which tells the routing handler to ignore all 
            //direct requests for pages with either.htm or.html extensions in any directory:
            //routes.Ignore("{*allhtml}", new { allhtml = @".*\.htm(/.*)?});

            //The route with the pattern { resource}.axd /{ *pathInfo} is included to 
            //prevent requests for the Web resource files such as WebResource.axd or 
            //ScriptResource.axd from being passed to a controller.
            //The favicon route is to prevent the favicon to be mapped to a route.
            //routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            //You can programmatically define ASP.NET MVC applications to ignore certain routes with URL
            //patterns.The IgnoreRoute method adds a special route or routes and instructs the routing
            //engine to ignore the requested URL that matches the supplied pattern. Because the route
            //handler parses through routes in order, comparing them with the URL, ensure that the routes
            //you want to ignore have been added before the routes you want to identify because the
            //routing handler stops after it finds a route that fits the URL.
            //For example we can temporary ignore alle requests to the Blog controller. 
            //This will only work to block the Blog/Index page if visiting the page as localhost/Blog. 
            //Visiting the page as localhost/Blog/Index or visiting any other 
            //Blog view will still be allowed. To block all URLs controlled by the 
            //EmployeeController, you can use routes.IgnoreRoute("Blog /{*pathInfo}");
            ///routes.IgnoreRoute("Blog/{*pathInfo}");

            //TODO deny direct access to Documents folder  
            //Note documents can be downloaded directly using the file path in the url as shown below.
            //Using IgnoreRoute we can prohibit this ??
            //https://localhost:44396/Documents/0672329980_CH02.pdf
            //https://localhost:44396/Documents/programming-microsoft-asp.net-mvc-dino-esposito.pdf
            //routes.IgnoreRoute("Documents/{*pathInfo}");
            //routes.Ignore("{*allpdf}", new { allpdf = @".*\.pdf(/.*)?" });

            //The controller action would look like this:
            //public ActionResult Index(int year, int month, int day, string title)
            //https://localhost:44396/Blog/2020/04/10/Computable
            routes.MapRoute(
                name: "BlogArchive",
                url: "Blog/{year}/{month}/{day}/{title}",
                defaults: new { controller = "Blog", action = "List" }, //, month = "1", day = "1" },
                constraints: new { year = @"\d{2}|\d{4}", month = @"\d{1,2}", 
                    day = @"\d{1,2}", title = @"[a-zA-Z0-9]+" }
                );

            //We can pass title and id in the following format:            
            //https://localhost:44396/Blog/aspnet-mvc-routing-examples-JQ
            //https://localhost:44396/Blog/JQ , is allowed because title is optional 
            //Because this is the get action of the Index page, we can also specify the URL as:  
            //https://localhost:44396/Blog/?title=aspnet-mvc-routing-examples&id=JQ
            //The controller action would look like this:
            //public ActionResult Index(string title, string id)
            routes.MapRoute(
                name: "BlogPage",
                url: "Blog/{title}-{id}",
                defaults: new { controller = "Blog", action = "Index", 
                    title = UrlParameter.Optional } //, id = UrlParameter.Optional}
                );

            //https://localhost:44396/Blog/aspnet-mvc-routing-examples/JQ
            routes.MapRoute(
                name: "BlogPageDetails",
                url: "Blog/{title}/{id}", 
                defaults: new { controller = "Blog", action = "ListDetails" }
                //, title = UrlParameter.Optional, id = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "ASPNETWebApp.Controllers" }
            );
        }
    }
}
