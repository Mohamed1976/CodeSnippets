using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreRazorPages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetCoreRazorPages.Pages.Forms
{
    public class AddAddressModel : PageModel
    {
        [BindProperty] //Not get property
        public Customer SelectedCustomer { get; set; }

        public void OnGet()
        {
        }

        //IActionResult is typical output from post
        //You perform a certain kind of action, go to other page, site or refresh page and clear input form
        public IActionResult OnPost()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }

            return RedirectToPage("/Index", new { City = SelectedCustomer.City }); //using annonymous
        }   
    }
}