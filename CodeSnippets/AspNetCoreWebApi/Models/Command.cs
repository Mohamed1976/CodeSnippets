using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// .NET Core 3.1 MVC REST API - Full Course
/// https://www.youtube.com/watch?v=fmvcAzHpsk8&t=3061s
/// </summary>
namespace AspNetCoreWebApi.Models
{
    public class Command
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(250)]
        public string HowTo { get; set; }

        [Required]
        public string Line { get; set; }

        [Required]
        public string Platform { get; set; }
    }
}
