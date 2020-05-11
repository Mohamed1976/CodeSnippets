using System.Web;
using System.Web.Mvc;

namespace ASPNETWebApp
{
    //There are three primary ways to apply attributes.The first is on the action itself. Decorating
    //an action ensures that the requirements within the filter are met by the context that the
    //action is handling.The attribute can also be put on a class level, or controller.Putting the
    //attribute at the class level ensures that all actions in the controller act as if they have been
    //decorated with the attribute.The last place that you can assign a filter is through global filters,
    //which apply to all actions within the system.A default HandleErrorAttribute, for example,
    //is generally a good idea in an application. Some applications might need everything to happen
    //over a Secure Sockets Layer (SSL), so you apply RequireHttpsAttribute globally.To add a
    //filter to the global filters list, insert a line in the App_Start/FilterConfig.cs RegisterGlobalFilters
    //method: for example filters.Add(new RequireHttpsAttribute());
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
