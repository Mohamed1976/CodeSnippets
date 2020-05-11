using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using AspNetCoreRazorPages.Data;
using AspNetCoreRazorPages.Models;

namespace AspNetCoreRazorPages.Pages.Movies
{
    public class CreateModel : PageModel
    {
        private readonly AspNetCoreRazorPages.Data.AspNetCoreRazorPagesContext _context;

        public CreateModel(AspNetCoreRazorPages.Data.AspNetCoreRazorPagesContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Movie Movie { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            //If there are any model errors, the form is redisplayed, along with any form data posted. 
            //Most model errors can be caught on the client - side before the form is posted. 
            //An example of a model error is posting a value for the date field that cannot be converted to a date.
             
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Movie.Add(Movie);
            await _context.SaveChangesAsync();

            //If there are no model errors, the data is saved, and the browser is redirected to the Index page.
            return RedirectToPage("./Index");
        }
    }
}
