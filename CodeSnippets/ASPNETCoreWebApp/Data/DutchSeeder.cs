using ASPNETCoreWebApp.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCoreWebApp.Data
{
    public class DutchSeeder
    {
        private readonly DutchContext _ctx;
        private readonly IWebHostEnvironment _hosting;

        public DutchSeeder(DutchContext ctx, IWebHostEnvironment hosting)
        {
            this._ctx = ctx;
            this._hosting = hosting;
        }

        public void Seed()
        {
            _ctx.Database.EnsureCreated();

            //Check if Database already contains data
            if (!_ctx.Products.Any())
            {
                // Need to create sample data
                var filepath = Path.Combine(_hosting.ContentRootPath, "Data/SeedData/art.json");
                var json = File.ReadAllText(filepath);
                var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);
                Trace.TraceInformation("Seeding data DutchSeeder");
                Trace.TraceInformation($"Total records read: {products.Count()}");
                _ctx.Products.AddRange(products);

                //SqlException: Cannot insert explicit value for identity column in table 
                //'Orders' when IDENTITY_INSERT is set to OFF.
                //Order order = new Order()
                //{
                //    Id = 1,
                //    OrderDate = DateTime.Now,
                //    OrderNumber = "BL34",
                //    Items = new List<OrderItem>()
                //    {
                //        new OrderItem()
                //        {
                //            Product = products.First(),
                //            Quantity = 5,
                //            UnitPrice = products.First().Price
                //        }
                //    }
                //};

                //_ctx.Orders.Add(order);

                //var order = _ctx.Orders.Where(o => o.Id == 1).FirstOrDefault();
                //if (order != null)
                //{
                //    order.Items = new List<OrderItem>()
                //    {
                //        new OrderItem()
                //        {
                //          Product = products.First(),
                //          Quantity = 5,
                //          UnitPrice = products.First().Price
                //        }
                //    };
                //}

                _ctx.SaveChanges();

            }
        }
    }
}
