using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLibrary.ShopDemo.Models.Base
{
    public abstract class BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id", Order = 0)]
        public int Id { get; set; }

        [Timestamp]
        [Column("TimeStamp", Order = 1)]
        public byte[] TimeStamp { get; set; }

        //Used for global query filters
        [Column("IsDeleted", Order = 2)]
        public bool IsDeleted { get; set; }
    }
}
