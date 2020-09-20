using DataLibrary.BankDemo.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace DataLibrary.BankDemo.Data
{
    public static class DbInitializer
    {
        public static void Initialize(BankContext context)
        {
            try
            {
                // Create the TransactionScope to execute the commands, guaranteeing
                // that both commands can commit or roll back as a single unit of work.
                using (TransactionScope scope = new TransactionScope())
                {
                    Console.WriteLine("Start seeding the Bank database...");
                    //Console.WriteLine("Deleting ContosoUniversity database...");
                    //context.Database.EnsureDeleted();
                    //Console.WriteLine("Creating ContosoUniversity database...");
                    //context.Database.EnsureCreated();

                    var customers = new List<Customer>
                    {
                        new Customer()
                        {
                            FirstName = "Mickey",
                            LastName = "Mouse",
                            EmailAddresses = new List<Email>
                            {
                                new Email()
                                {
                                    CreationDate = DateTime.Now,
                                    EmailAddress = "Mickey.Mouse@gmail.com"
                                },
                                new Email()
                                {
                                    CreationDate = DateTime.Now,
                                    EmailAddress = "Mickey.Mouse@hotmail.com"
                                },
                                new Email()
                                {
                                    CreationDate = DateTime.Now,
                                    EmailAddress = "Mickey.Mouse@Yahoo.com"
                                },
                            },
                            Addresses = new List<Address>
                            {
                                new Address()
                                {
                                    Country="Canada",
                                    State="Georgia",
                                    City="Amsterdam",
                                    StreetAddress="Mainstreet 12",
                                    ZipCode="4567-HJ-90",
                                    CreationDate = DateTime.Now,
                                }
                            },
                            Accounts = new List<Account>
                            {
                                new Account("ABCDEFGHIJ0123456789", 5900)
                                {
                                    Type = AccountType.Savings,
                                    Transactions = new List<Models.Transaction>
                                    {
                                        new Models.Transaction(TransactionType.Deposit,500,
                                        DateTime.Now,"Added money to Savings account"),
                                        new Models.Transaction(TransactionType.Withdrawal,34,
                                        DateTime.Now,"Paid game"),
                                        new Models.Transaction(TransactionType.Deposit,800,
                                        DateTime.Now,"Added lottery money won."),
                                    }
                                }
                            }
                        }
                    };

                    context.Customers.AddRange(customers);
                    context.SaveChanges();

                    // The Complete method commits the transaction. If an exception has been thrown,
                    // Complete is not  called and the transaction is rolled back.
                    scope.Complete();
                    Console.WriteLine("Finished seeding the Bank database...");
                } //Using TrasactionScope
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error encountered in DBInitialize, Exception: {ex.Message}");
                Console.WriteLine($"Error encountered in DBInitialize, InnerException: {ex.InnerException.Message}");
                //throw;
            }
        }
    }
}
