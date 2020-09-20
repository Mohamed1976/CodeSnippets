using DataLibrary.BankDemo.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLibrary.BankDemo.Models
{
	public class Email : BaseEntity
	{
		[StringLength(250)]
		public string EmailAddress { get; set; }
		[DataType(DataType.Date)]
		public DateTime CreationDate { get; set; }
	}
}
