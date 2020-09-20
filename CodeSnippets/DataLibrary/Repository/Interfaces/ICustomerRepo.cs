using DataLibrary.BankDemo.Models;
using DataLibrary.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLibrary.Repository.Interfaces
{
    public interface ICustomerRepo : IRepo<Customer>
    {
        IEnumerable<Customer> Search(string searchString);
    }
}
