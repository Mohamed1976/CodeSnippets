using DataLibrary.ContosoUniversity.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLibrary.ContosoUniversity.Models
{
    public class Instructor : BaseEntity //Person
    {

        public Person PersonalInformation { get; set; } = new Person();

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Hire Date")]
        public DateTime HireDate { get; set; }

        public ICollection<CourseAssignment> CourseAssignments { get; set; } = new List<CourseAssignment>();
        //public ICollection<CourseAssignment> CourseAssignments { get; set; }
        public OfficeAssignment OfficeAssignment { get; set; }

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
            return $"{PersonalInformation.FullName} {HireDate.ToShortDateString()}";
        }
    }
}
