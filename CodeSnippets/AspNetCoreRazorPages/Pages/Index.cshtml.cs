using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace AspNetCoreRazorPages.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }


        private string _firstName = default; 
        //If you put argument in URL, that is called a get 
        [BindProperty(SupportsGet =true)]
        public string FirstName 
        {
            get
            {
                return _firstName;
            } 
            set
            {
                Trace.WriteLine("FirstName is set: " + value);
                _firstName = value;
            }
        }

        [BindProperty(SupportsGet = true)]
        public string City { get; set; }

        //Is called when you request the page
        public void OnGet()
        {
            if(string.IsNullOrWhiteSpace(FirstName))
            {
                FirstName = "User";
            }

            if(string.IsNullOrWhiteSpace(City))
            {
                City = "The Web";
            }
        }

        //Is called when you post something to this page
        //You capture information arguments to the page
        //public void OnPost()
        //{
        //}
    }
}
