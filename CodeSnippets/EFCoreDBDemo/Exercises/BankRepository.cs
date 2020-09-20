using System;
using System.Collections.Generic;
using System.Text;
using Bogus;
using DataLibrary.BankDemo.Models;

namespace EFCoreDBDemo.Exercises
{
    /*
    Bogus (https://github.com/bchavez/Bogus), is a simple data generator for C#. 
    All you have to do is create rules for your data and generate it. Simple as that! 
    Then, you’ve got the data to use in your programs. It can be fixed (every time you 
    run your program, you have the same data) or variable (every time you get a different 
    set of data), and once you got it, you can serialize it to whichever data format you 
    want: json files, databases, xml or plain text files.
    https://blogs.msmvps.com/bsonnino/2019/04/24/creating-sample-data-for-c/
    https://www.nuget.org/packages/Bogus/
    */
    public class BankRepository
    {
        public IEnumerable<Customer> GetCustomers(int generateCount = 100)
        {
            Randomizer.Seed = new Random(123456);
            var emailgenerator = new Faker<Email>()
                .RuleFor(e => e.EmailAddress, f => f.Person.Email)
                .RuleFor(e => e.CreationDate, f => f.Date.Between(DateTime.Now.AddYears(-2), DateTime.Now));

            var addressgenerator = new Faker<Address>()
                .RuleFor(e => e.Country, f => f.Address.Country())
                .RuleFor(e => e.State, f => f.Address.State())
                .RuleFor(e => e.City, f => f.Address.City())
                .RuleFor(e => e.StreetAddress, f => f.Address.StreetAddress())
                .RuleFor(e => e.ZipCode, f => f.Address.ZipCode())
                .RuleFor(e => e.CreationDate, f => f.Date.Between(DateTime.Now.AddYears(-2), DateTime.Now));

            var transactionGenerator = new Faker<Transaction>()
                .RuleFor(t => t.Amount, f => f.Finance.Amount(0, 1000))
                .RuleFor(t => t.Type, f => f.PickRandom<DataLibrary.BankDemo.Models.TransactionType>())
                .RuleFor(t => t.Description, f => f.Rant.Review());

            var accountgenerator = new Faker<Account>()
                .RuleFor(a => a.Number, f => f.Phone.PhoneNumber("###-#####-#####-####"))
                .RuleFor(e => e.Balance, f => f.Finance.Amount(0, 10000))
                .RuleFor(e => e.Type, f => f.PickRandom<DataLibrary.BankDemo.Models.AccountType>())
                .RuleFor(c => c.Transactions, f => transactionGenerator.Generate(f.Random.Number(10)));

            var customerGenerator = new Faker<Customer>()
                .RuleFor(c => c.FirstName, f => f.Person.FirstName)
                .RuleFor(c => c.LastName, f => f.Person.LastName)
                .RuleFor(c => c.EmailAddresses, f => emailgenerator.Generate(f.Random.Number(1,4)))
                .RuleFor(c => c.Addresses, f => addressgenerator.Generate(f.Random.Number(1,3)))
                .RuleFor(c => c.Accounts, f => accountgenerator.Generate(f.Random.Number(1, 2)));

            return customerGenerator.Generate(generateCount);
        }
    }
}
