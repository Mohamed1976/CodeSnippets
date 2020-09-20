using DataLibrary.BankDemo.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLibrary.BankDemo.Models
{
    public enum AccountType
    {
        Savings, Checking
    }

    public class Account : BaseEntity
    {
        public Account()
        {
        }

        public Account(string number, decimal balance)
        {
            Number = number;
            Balance = balance;
            CreationDate = DateTime.Now;
        }

        [StringLength(20, MinimumLength = 20)]
        public string Number { get; set; }
        public AccountType Type { get; set; }
        //[Display(Name = "Account Balance")]
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
        public decimal Balance { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();

        public void MakeDeposit(decimal amount, DateTime date, string description)
        {
            Balance += amount;
            Transactions.Add(new Transaction(TransactionType.Deposit, amount, date, description));
        }

        public void MakeWithdrawal(decimal amount, DateTime date, string description)
        {
            Balance -= amount;
            Transactions.Add(new Transaction(TransactionType.Withdrawal, amount, date, description));
        }

        public override string ToString()
        {
            string account = $"#Account, Number: {Number}, Type: {Type}, " +
                $"Balance: {Balance}, Creation Date{CreationDate}\n";

            foreach(Transaction transaction in Transactions)
            {
                account += $"\t{transaction}\n";
            }

            return account;
        }

    }
}
