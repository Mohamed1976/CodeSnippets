using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCoreWebApp.Models
{
    public class LogModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime RunDate { get; set; }

        [Required]
        [Range(0.01,1000.00)]
        public double Distance { get; set; }

        [Required]
        public TimeSpan Time { get; set; }

        public string ShortDate
        {
            get 
            { 
                return RunDate.ToLocalTime().ToShortDateString();             
            }
        }
    }
}
