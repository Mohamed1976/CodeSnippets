using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPNETWebApp.Models
{
    public class Employee
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Position { get; set; }
        public string Office { get; set; }
        public DateTime HireDate { get; set; }
        public decimal Salary { get; set; }
    }
}