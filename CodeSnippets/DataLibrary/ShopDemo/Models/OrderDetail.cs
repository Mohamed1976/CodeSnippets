using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataLibrary.ShopDemo.Models.Base;

namespace DataLibrary.ShopDemo.Models
{
    [Table("OrderDetails", Schema = "Store")]
    public class OrderDetail : BaseEntity
    {
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required, DataType(DataType.Currency), Display(Name = "Unit Cost")]
        public decimal UnitCost { get; set; }
        [DataType(DataType.Currency), Display(Name = "Total")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal? LineItemTotal { get; set; }
        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; }
    }
}
