using DataLibrary.BankDemo.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataLibrary.BankDemo.Models
{
    public enum TransactionType
    {
        Withdrawal = 0, Deposit
    }

    public class Transaction : BaseEntity
    {
        //[Display(Name = "Transaction Amount")]
        //[DataType(DataType.Currency)]
        //[DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        //warn: Microsoft.EntityFrameworkCore.Model.Validation[30000]
        //No type was specified for the decimal column 'Balance' on entity 
        //type 'Account'. This will cause values to be silently truncated if 
        //they do not fit in the default precision and scale.Explicitly specify 
        //the SQL server column type that can accommodate all the values using 'HasColumnType()'.
        //http://jameschambers.com/2019/06/No-Type-Was-Specified-for-the-Decimal-Column/
        //[Column(TypeName = "decimal(18,2)")]
        [Column(TypeName = "decimal(18, 2)")]
        [DataType(DataType.Currency)]
        [Range(0, 1000000)]
        public decimal Amount { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }
        [StringLength(250, MinimumLength = 3)]
        public string Description { get; set; }

        public TransactionType Type { get; set; }

        public Transaction()
        {
        }

        public Transaction(TransactionType type, decimal amount, 
            DateTime creationDate, string description)
        {
            Type = type;
            Amount = amount;
            CreationDate = creationDate;
            Description = description;
        }

        public override string ToString()
        {
            return $"#Transaction, Creation Date: {CreationDate}, Type: {Type}, " +
                $"Description: {Description}, Amount: {Amount}";
        }
    }
}
