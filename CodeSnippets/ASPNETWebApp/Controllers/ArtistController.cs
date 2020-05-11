using ASPNETWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASPNETWebApp.Controllers
{
    public class ArtistController : Controller
    {
        public ActionResult Index()
        {
            //Using viewbag you can pass arguments to the view, which are then renderd in the view    
            ViewBag.Message = "Choose your favorite Artists.";

            //Strongly typed views allow you to set a model type for a view. This allows you to pass a model 
            //object from the controller to the view that’s strongly typed on both ends, so you get the 
            //benefi t of IntelliSense, compiler checking, and so on.In the Controller method, you 
            //can specify the model via an overload of the View method whereby you pass in the model instance:            
            return View(artists);
        }

        private List<Artist> artists = new List<Artist>()
        {
            new Artist("firstName1", "lastName1", new DateTime(1960,11,12)),
            new Artist("firstName2", "lastName2", new DateTime(1961,10,13)),
            new Artist("firstName3", "lastName3", new DateTime(1962,9,14)),
            new Artist("firstName4", "lastName4", new DateTime(1963,8,15)),
            new Artist("firstName5", "lastName5", new DateTime(1964,7,16)),
            new Artist("firstName6", "lastName6", new DateTime(1965,6,17)),
        };
    }
}