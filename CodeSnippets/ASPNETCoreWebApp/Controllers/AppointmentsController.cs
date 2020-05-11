using ASPNETCoreWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ASPNETCoreWebApp.Helpers;

namespace ASPNETCoreWebApp.Controllers
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
    //method: filters.Add(new RequireHttpsAttribute());

    //AuthorizeAttribute is another filter designed specifically to enable you to wrap security
    //around an action being taken without having to write any code as part of that action.The
    //AuthorizeAttribute gives you control over whether the user must be authenticated before
    //being able to take the decorated action.It can be modified to check for authorization as well
    //by checking to see whether the user has roles that are in an accepted list: Authorize(Roles = Admin, PowerUser).
    //The functionality provided by the AuthorizeAttribute is critical for any application that
    //needs to identify the user.Whether you have an e-commerce application tracking a buyer’s
    //behavior through the site to make recommendations on the next purchase or a bank that
    //wants to ensure that users are who they say they are, authentication is one of the cornerstones
    //of the modern Internet. In many cases, the AuthorizeAttribute is applied globally so
    //that all actions in all controllers require that the user be authorized, with the few that do not
    //require authorization being marked with AllowAnonymous.
    //When we place the Authorize attribute on the controller itself, the authorize attribute 
    //applies to all of the actions inside. The MVC framework will not allow a request to 
    //reach an action protected by this attribute unless the user passes an authorization check.
    //By default, if you use no other parameters, the only check the Authorize attribute will 
    //make is a check to ensure the user is logged in so we know their identity.
    //But you can use parameters to specify any fancy custom authorization policy that you like.
    //There is also an AllowAnonymous attribute. This attribute is useful when you want to use 
    //the Authorize attribute on a controller to protect all of the actions inside, but then 
    //there is this single action or one or two actions that you want to unprotect and 
    //allow anonymous users to reach that specific action.
    //[CustomAuthorize]
    public class AppointmentsController : Controller
    {
        private readonly ILogger<AppointmentsController> _logger;

        public AppointmentsController(ILogger<AppointmentsController> logger)
        {
            _logger = logger;
        }

        //Use the RequireHttpsAttribute on the controller actions that you want to ensure
        //are always accessed on a secure communications channel.This will redirect end
        //users to the HTTPS version of the URL when they attempt to access the page using
        //HTTP. The RequireHttpsAttribute ensures that all calls to the decorated controller or method have
        //gone through HTTPS to ensure secure transport.You typically use it whenever you manage
        //confidential or secure information, such as personal information, credit card purchases,
        //or screens that are expecting login names and passwords.If the call has not gone through
        //HTTPS, the application forces a resubmit over HTTPS.
        //To enable http://localhost:5001;, Go to your project properties -> 
        //debug tab and remove https://localhost:5001;
        [RequireHttps]
        [HttpGet]

        //[CustomAuthorize], authorization attribute
        
        //Action filters enables the creation of custom action filters or any kind of class 
        //that you want to be able to act as an attribute on an action.
        //Action filters: Run code immediately before and after an action method is called.
        //Can change the arguments passed into an action.
        //Can change the result returned from the action.
        //Are not supported in Razor Pages.
        //Implement either the IActionFilter or IAsyncActionFilter interface.
        //Their execution surrounds the execution of action methods.
        [CustomActionFilter]
        public ViewResult Index()
        {
            Debug.WriteLine("AppointmentsController, ViewResult Index()");
            //In general this is set in the view
            ViewData["Title"] = "Index page of Appointments.";

            List<Appointment> appointment = new List<Appointment>()
            {
                new Appointment() { Id ="1-00", Name="Dentist", AppointmentDate = new DateTime(2020,3,18,19,12,13) },
                new Appointment() { Id ="2-00", Name="Doctor", AppointmentDate =  new DateTime(2020,4,17,20,11,14) },
                new Appointment() { Id ="3-00", Name="Teacher", AppointmentDate = new DateTime(2020,5,16,21,10,15) },
                new Appointment() { Id ="4-00", Name="Holiday", AppointmentDate = new DateTime(2020,6,15,22,9,16) },
                new Appointment() { Id ="5-00", Name="Family", AppointmentDate =  new DateTime(2020,7,14,23,8,17) },
                new Appointment() { Id ="6-00", Name="Job", AppointmentDate =     new DateTime(2020,8,13,1, 7,18) },
            };

            return View(appointment);
        }

        [HttpGet]
        public ViewResult Create()
        {
            Debug.WriteLine("AppointmentsController, ViewResult Create()");
            return View();
        }

        //The ValidateAntiForgeryTokenAttribute helps protect your application against cross-site
        //request forgeries by ensuring that there is a shared, secret value between the form data in
        //a hidden field, a cookie, and on the server.It validates that the form is one that your server
        //posted, that it is the same browser session, and that it matches an expected value on the
        //server. You should also add a ValidateAntiForgeryToken attribute to the Contact
        //action method.The ValidateAntiForgeryToken attribute adds a security mechanism that prevents a type of
        //attack called cross-site request forgery.This is an attack where another site takes advantage of the fact that a
        //user is logged on to your web site and then tricks that user into submitting information that can expose data
        //about that user or modify information on your web site.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //Note method can return RedirectToRouteResult and ViewResult, both implement IActionResult
        //Therfore the method returns IActionResult

        //Todo which are the the alternatives for [AllowHtml] [ValidateInput] in core  
        //One of the risks of allowing users input is that they might insert potentially dangerous
        //information.The ValidateInputAttribute gives you control over the content coming back from
        //a post operation and ensures that there is no potentially dangerous content, such as <$ or
        //<! items, which could potentially lead to problems.You can select form fields that will not be
        //validated in the attribute by[ValidateInput(true, Exclude = “ArbitraryField”)] and on a model
        //property by decorating the model property with the AllowHtml attribute.You can also turn
        //validation completely off, if desired.If a form field fails the validation, the server returns the A
        //Potentially Dangerous Request.Form Value Was Detected From The Client message and does
        //not allow the request processing to continue.
        //[AllowHtml] attribute needs to decorate model property to have effect 
        //[ValidateInput]
        public IActionResult Create([Bind("Name,AppointmentDate")]Appointment appointment)
        {
            Debug.WriteLine("AppointmentsController, IActionResult Create(Bind(\"Name, AppointmentDate\")]Appointment appointment)");
            Debug.WriteLine($"appointment.Name: {appointment.Name}, " +
                $"AppointmentDate.AppointmentDate: {appointment.AppointmentDate}");
            if (ModelState.IsValid)
            {
                //appointment.Id = Guid.NewGuid().ToString();
                //_context.Add(appointment);
                //await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(appointment);
        }

        //The HandleErrorAttribute is an error management
        //tool that handles exceptions that occur within the action.By default, ASP.NET MVC 4 displays
        //the ~/Views/Shared/Error view when an error occurs in a decorated action.However, you can
        //also set the ExceptionType, View, and Master properties to call different views with different
        //master pages based on the type of exception.To perform customizations or overrides using
        //the HandleErrorAttribute, override the OnException method.Doing so gives your application
        //access to error information as well as some context about the error.

        //In asp.net core, you could use Exception Filters. 
        //Error handling exception filter could consolidate error handling.
        //Exception filters apply global policies to unhandled exceptions that occur before the response body has been written to.
        //Exception filters:
        //Implement IExceptionFilter or IAsyncExceptionFilter.
        //Can be used to implement common error handling policies.

        //Invokes exception filters: Exception filters contain code that is executed when
        //exceptions occur.If any of the exception filters return a result, execution of the
        //action will be short-circuited, and no other filters will be invoked.
        [CustomExceptionFilterAttribute]
        public IActionResult Details()
        {
            throw new NotImplementedException("Details Action is not implemented yet.");
        }

        public IActionResult CreatePerson()
        {
            ViewData["Title"] = "Index page of Appointments CreatePerson().";

            Trace.WriteLine("AppointmentsController.CreatePerson()");
            return View();
        }

        [HttpPost]
        public IActionResult DisplayPerson(string firstName, string lastName)
        {
            Trace.WriteLine($"AppointmentsController.DisplayPerson(string firstName, string lastName)" +
                $"firstName: {firstName}, lastName: {lastName}");

            ViewBag.Message = $"Welcome {firstName} {lastName}.";

            //You can also access FORM fields by referencing their names
            Trace.WriteLine($"HttpContext.Request.Form[\"FirstName\"]: {HttpContext.Request.Form["FirstName"]}" +
                $" HttpContext.Request.Form[\"LastName\"]: {HttpContext.Request.Form["LastName"]}");
            //HttpContext.Request.Form["UserName"] + ". You are " + HttpContext.Request.Form["UserAge"] + " years old!"); 

            return View();
        }

        [HttpPost]
        //The Bind attribute makes helpers available to the model binder so that
        //it has a better understanding of how the values should be mapped.The Bind attribute also
        //enables you to map a specific prefix. Prefix mapping is useful when parallel work between UI
        //design and development is under way. If a UI designer uses a different value for a variable
        //name (such as HomeAddress.Country) than the developer(such as user), the model binder doesn’t
        //recognize them as a match. The use of the Bind attribute enables the mapping to proceed without 
        //the UI designer or developer having to change the code:
        public IActionResult DisplayPersonFromModel([Bind(Prefix = "HomeAddress")]PersonAddress address)
        //public IActionResult DisplayPersonFromModel([Bind(Prefix = "HomeAddress")]string City, string Country), didnt receive the values in Post   
        {
            Trace.WriteLine($"AppointmentsController.DisplayPersonFromModel(string City, string Country)");
            Trace.WriteLine($"Received City: {address.City} Country: {address.Country}");
            string city = HttpContext.Request.Form["HomeAddress.City"];
            string country = HttpContext.Request.Form["HomeAddress.Country"];
            Trace.WriteLine($"HttpContext.Request.Form[\"HomeAddress.City\"]: {city }" +
                $" HttpContext.Request.Form[\"HomeAddress.Country\"]: {country}");

            ViewBag.Message = $"#Welcome {city} {country}.";

            return View(nameof(DisplayPerson));
        }

        //GET
        [HttpGet]
        public IActionResult SampleForm()
        {
            Trace.WriteLine("IActionResult AppointmentsController.SampleForm()");
            return View();
        }

        [HttpPost]
        public IActionResult SampleForm(SampleViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            return Content("Success");
        }

        [HttpGet]
        public IActionResult About(bool approved = false)
        {
            Trace.WriteLine($"IActionResult About(bool approved = false), approved: {approved}");
            ViewData["Message"] = "The About Form @WORK";
            //return View();
            return View(new WebsiteContext
            {
                Approved = approved,
                CopyrightYear = 2015,
                Version = new Version(1, 3, 3, 7),
                TagsToShow = 20
            });
        }
    }
}
