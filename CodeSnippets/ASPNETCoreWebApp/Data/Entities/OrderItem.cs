
//Has a relationship to Product and Order 
//This relationship will be modeled in the database as foreign keys as shown below 

//CREATE TABLE[dbo].[OrderItem]
//(
//   [Id] INT             IDENTITY(1, 1) NOT NULL,
//[ProductId] INT NULL,
//[Quantity]  INT NOT NULL,
//    [UnitPrice] DECIMAL(18, 2) NOT NULL,
//   [OrderId]   INT NULL,
//   CONSTRAINT[PK_OrderItem] PRIMARY KEY CLUSTERED([Id] ASC),
//    CONSTRAINT[FK_OrderItem_Orders_OrderId] FOREIGN KEY([OrderId]) REFERENCES[dbo].[Orders] ([Id]),
//    CONSTRAINT[FK_OrderItem_Products_ProductId] FOREIGN KEY([ProductId]) REFERENCES[dbo].[Products] ([Id])
//);
//GO
//CREATE NONCLUSTERED INDEX[IX_OrderItem_OrderId]
//    ON[dbo].[OrderItem] ([OrderId] ASC);
//GO
//CREATE NONCLUSTERED INDEX[IX_OrderItem_ProductId]
//    ON[dbo].[OrderItem] ([ProductId] ASC);

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPNETCoreWebApp.Data.Entities
{
  public class OrderItem
  {
    public int Id { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    [DataType(DataType.Currency)]
    [Range(1, 100)]
    public decimal UnitPrice { get; set; }
    public Order Order { get; set; }
  }
}