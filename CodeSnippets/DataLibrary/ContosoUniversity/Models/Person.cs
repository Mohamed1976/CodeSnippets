using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataLibrary.ContosoUniversity.Models
{
    //Changed from inheritance to Owned class 
    //public abstract class Person : BaseEntity
    [Owned]
    public class Person
    {
        //public int ID { get; set; }

        [Required]
        [StringLength(50)]
        [Column("LastName")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        [Column("FirstName")]
        [Display(Name = "First Name")]
        public string FirstMidName { get; set; }

        //[Column(TypeName = "varchar(250)")]
        [Column("EmailAddress")]
        [StringLength(250, ErrorMessage = "EmailAddress cannot be longer than 250 characters.")]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Column("DateOfBirth")]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        //https://stackoverflow.com/questions/9/in-c-how-do-i-calculate-someones-age-based-on-a-datetime-type-birthday
        [NotMapped]
        public int Age
        {
            get
            {
                // Save today's date.
                var today = DateTime.Today;

                // Calculate the age.
                var age = today.Year - DateOfBirth.Year;

                // Go back to the year the person was born in case of a leap year
                if (DateOfBirth.Date > today.AddYears(-age)) age--;

                return age;
            }
        }

        [NotMapped]
        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return LastName + ", " + FirstMidName;
            }
        }
    }
}
