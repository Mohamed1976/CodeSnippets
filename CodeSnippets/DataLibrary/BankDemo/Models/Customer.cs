using DataLibrary.BankDemo.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataLibrary.BankDemo.Models
{
    public class Customer : BaseEntity
    {
        [StringLength(250)]
        public string FirstName { get; set; }
        [StringLength(250)]
        public string LastName { get; set; }
        public List<Address> Addresses { get; set; } = new List<Address>();
        public List<Email> EmailAddresses { get; set; } = new List<Email>();
        public List<Account> Accounts { get; set; } = new List<Account>();

        [NotMapped]
        public string FullName 
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

        public int AddAddress()
        {
            //Addresses.Add(new Address());
            return 0;
        }
    }
}
