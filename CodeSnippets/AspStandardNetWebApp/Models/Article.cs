using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspStandardNetWebApp.Models
{
    public class Article
    {
        public int ID { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Title { get; set; }
        [RegularExpression(@"A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}")]
        public string AuthorEmail { get; set;}
        [DataType(DataType.Date)]
        [Range(300, 3000)]
        public int NumberOfAuthors { get; set; }
        [Required]
        public DateTime CreateDate { get; set; }
        [Required]
        public string Description { get; set; }
        [Range(1, 250)]
        [DataType(DataType.Currency)]
        [Required]
        public decimal Price { get; set; }
    }
}