using DataLibrary.CarDealerships.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLibrary.CarDealerships.Models
{
    [Table("CreditRisks", Schema = "Dbo")]
    public partial class CreditRisk : BaseEntity
    {
        [Required]
        public int CustomerId { get; set; }

        public Person PersonalInformation { get; set; } = new Person();

        [ForeignKey(nameof(CustomerId))]
        [InverseProperty(nameof(Customer.CreditRisks))]
        public Customer CustomerNavigation { get; set; }
    }
}
