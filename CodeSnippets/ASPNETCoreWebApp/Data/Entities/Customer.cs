using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCoreWebApp.Data.Entities
{
    public class Customer
    {
        public string Avatar { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public int Id { get; set; }
        public string LastName { get; set; }
    }
}
