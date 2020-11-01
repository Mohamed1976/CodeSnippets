using DataLibrary.ContosoUniversity.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLibrary.ContosoUniversity.Models
{
    public class Student : BaseEntity //Person
    {
        public Person PersonalInformation { get; set; } = new Person();

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Enrollment Date")]
        public DateTime EnrollmentDate { get; set; }

        //public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

        [NotMapped]
        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return PersonalInformation.FullName;
            }
        }

        public override string ToString()
        {
            return $"FullName: {PersonalInformation.FullName}, " +
                $"Enrollment Date: {EnrollmentDate.ToShortDateString()}, " +
                $"EmailAddress:{PersonalInformation.EmailAddress}, " +
                $"Date Of Birth: {PersonalInformation.DateOfBirth.ToShortDateString()}, " +
                $"Age: {PersonalInformation.Age}";
        }
    }
}
