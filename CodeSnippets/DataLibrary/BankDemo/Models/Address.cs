using DataLibrary.BankDemo.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLibrary.BankDemo.Models
{
    public class Address : BaseEntity
    {
        [StringLength(250)]
        public string StreetAddress { get; set; }
        [StringLength(100)]
        public string City { get; set; }
        [StringLength(100)]
        public string State { get; set; }
        [StringLength(20)]
        public string ZipCode { get; set; }
        [StringLength(100)]
        public string Country { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }
    }
}
