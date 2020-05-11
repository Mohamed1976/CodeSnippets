using ASPNETCoreWebApp.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCoreWebApp.Controllers
{
    public class ShopController : Controller
    {
        private readonly IDutchRepository _repository;

        public ShopController(IDutchRepository repository)
        {
            this._repository = repository;
        }

        public IActionResult Index()
        {
            var result = _repository.GetAllProducts();

            return View(result);
        }
    }
}
