using DataLibrary.ContosoUniversity.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLibrary.ContosoUniversity.Models
{
    public class OfficeAssignment : BaseEntity
    {
        //[Key]
        public int InstructorID { get; set; }
        [StringLength(50)]
        [Display(Name = "Office Location")]
        public string Location { get; set; }

        [ForeignKey(nameof(InstructorID))]
        [DisplayName("Instructor")]
        public Instructor Instructor { get; set; }
    }
}
