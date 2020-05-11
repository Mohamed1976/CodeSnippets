using ASPNETCoreWebApp.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCoreWebApp.Components
{
    public class CategoryMenu : ViewComponent
    {
        private readonly IDutchRepository repository;

        public CategoryMenu(IDutchRepository repository)
        {
            this.repository = repository;
        }

        public IViewComponentResult Invoke()
        {
            //Get all Categories
            var categories = repository.GetAllProducts()
                .Select(p => p.Category)
                .Distinct()
                .OrderBy(x => x);
            return View(categories);
        }
    }
}
