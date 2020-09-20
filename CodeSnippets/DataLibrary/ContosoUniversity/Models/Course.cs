using DataLibrary.ContosoUniversity.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataLibrary.ContosoUniversity.Models
{
    [Table("Course", Schema = "Dbo")]
    public class Course : BaseEntity
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        //[Display(Name = "Number")]
        //public int CourseID { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Title { get; set; }

        [Range(0, 5)]
        public int Credits { get; set; }

        public int DepartmentID { get; set; }

        [ForeignKey(nameof(DepartmentID))]
        [DisplayName("Department")]
        public Department DepartmentNavigation { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<CourseAssignment> CourseAssignments { get; set; }
    }
}
