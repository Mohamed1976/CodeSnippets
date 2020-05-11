using System.Web.Mvc;

//In asp.net mvc when we partition web applications into smaller units that are referred as areas.
//Areas provide a way to separate a large mvc web application into smaller functional groupings.
//https://www.codeproject.com/Articles/1139669/How-to-Create-an-Area-in-ASP-NET-MVC-Application

namespace ASPNETWebApp.Areas.Dashboard
{
    public class DashboardAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Dashboard";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                name: "Dashboard_default",
                url: "Dashboard/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "ASPNETWebApp.Areas.Dashboard.Controllers" }
            );
        }
    }
}