using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCoreWebApp.Models
{
    public class SampleViewModel
    {
		[Required]
		public string inputtext { get; set; }

		[Required]
		public string inputtext2 { get; set; }
	}
}
