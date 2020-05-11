using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCoreWebApp.Models
{
    //The use of strongly-typed model binding provides many advantages because it 
    //enables the ASP.NET MVC application to understand the model and to be able 
    //to apply this knowledge to the binding. You can use model attributes such 
    //as Required and DataType(DataType.Password), especially around validation.
    //The binder uses model attributes to render the HTML from the HTML helper 
    //with the appropriate JavaScript settings to enable client-side validation. 
    //It also enables the binder to recognize the model when the data is returned 
    //to the server and to determine whether the model is valid.The helper could 
    //not make this distinction on the client side without strong binding. 
    //Implementing strongly-typed binding, along with an appropriately attributed 
    //model and adding a few extra HTML helper tags on validation ensures that 
    //your UI is completely validated. A strongly-typed text box references the 
    //model directly, as follows:
    //@Html.DisplayNameFor(model => model.Make), displays the attribute [Display(Name = "Car Brand")] 
    //The <label> that the user sees rendered in a web browser is explicitly linked to the
    //Make field.
    public class Car
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Car Brand")]
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int Doors { get; set; }
        public string Colour { get; set; }
        [Range(1, 100000), DataType(DataType.Currency)] //, Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        //[DataType(DataType.Password)]
    }

    public interface ICarService
    {
        List<Car> GetAll();
    }
    public class CarService : ICarService
    {
        public List<Car> GetAll()
        {
            List<Car> cars = new List<Car>
            {
                new Car{Id = 1, Make="Audi",Model="R8",Year=2014,Doors=2,Colour="Red",Price=79995},
                new Car{Id = 2, Make="Aston Martin",Model="Rapide",Year=2010,Doors=2,Colour="Black",Price=54995},
                new Car{Id = 3, Make="Porsche",Model=" 911 991",Year=2016,Doors=2,Colour="White",Price=155000},
                new Car{Id = 4, Make="Mercedes-Benz",Model="GLE 63S",Year=2017,Doors=5,Colour="Blue",Price=83995},
                new Car{Id = 5, Make="BMW",Model="X6 M",Year=2016,Doors=5,Colour="Silver",Price=62995},
            };
            return cars;
        }
    }
}
