using DataLibrary.BankDemo.Data;
using DataLibrary.BankDemo.Models;
using DataLibrary.Repository.Base;
using DataLibrary.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLibrary.Repository
{

    public class CustomerRepo : RepoBase<Customer>, ICustomerRepo
    {
        public CustomerRepo(BankContext context) : base(context)
        {
        }

        internal CustomerRepo(DbContextOptions<BankContext> options) : base(options)
        {
        }

        //Order by FullName
        //public override IEnumerable<Customer> GetAll() => base.GetAll(x => x.FullName).ToList();
        //Order by LastName
        public override IEnumerable<Customer> GetAll() => base.GetAll(x => x.LastName).ToList();

        public IEnumerable<Customer> Search(string searchString)
        {
            //Both IEmumerable and IQueryable are executes the query in deferred mechanism.
            IEnumerable<Customer> customers = Table
                .Where(c => EF.Functions.Like(c.FirstName, $"%{searchString}%")
                    || EF.Functions.Like(c.LastName, $"%{searchString}%"))
                .ToList();

            return customers;
        }
    }
}
