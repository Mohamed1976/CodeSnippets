using DataLibrary.ContosoUniversity.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataLibrary.ContosoUniversity.Models
{
    public class CourseAssignment //: BaseEntity
    {
        public int InstructorID { get; set; }
        public int CourseID { get; set; }

        [ForeignKey(nameof(InstructorID))]
        [DisplayName("Instructor")]
        public Instructor Instructor { get; set; }

        [ForeignKey(nameof(CourseID))]
        [DisplayName("Course")]
        public Course Course { get; set; }
    }
}
