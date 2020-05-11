using ASPNETCoreWebApp.Data;
using ASPNETCoreWebApp.Data.Entities;
using ASPNETCoreWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

//Use PostMan
//https://localhost:5001/api/products


namespace ASPNETCoreWebApp.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {
        private readonly IDutchRepository _repository;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IDutchRepository repository, ILogger<ProductsController> logger)
        {
            this._repository = repository;
            this._logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<IEnumerable<Product>> Get()
        {
            try
            {
                Trace.WriteLine($"ProductsController.Get()");
                return Ok(_repository.GetAllProducts());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get products: {ex}");
                return BadRequest("Failed to get products");
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            Trace.WriteLine($"ProductsController.Get(int id): {id}");
            var product = _repository.GetAllProducts().FirstOrDefault();
            return Ok(product);
        }

        [HttpPost]
        public IActionResult Post([FromBody]Product product)
        {
            Trace.WriteLine($"{product.Title}, {product.Category}");
            return Ok();
        }
    }
}