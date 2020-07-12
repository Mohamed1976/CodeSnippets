using EFCoreDBDemo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace EFCoreDBDemo.Exercises
{
    public class LinqExamples
    {
        private readonly AdventureWorksLT2017Context dbContext = null;

        public LinqExamples()
        {
            dbContext = new AdventureWorksLT2017Context();
        }

        public void Run()
        {
            //ShowCustomers();
            //Q1();
            //Q2();
            //Q3();
            //Q4();
            Q5();
        }

        //The Select Operator
        //Could you list the names of Products with a listprice greater than $3000?
        public void Q5()
        {
            IQueryable<string> products
                = dbContext.Product
                .Where(p => p.ListPrice > 3000)
                .Select(p => p.Name);

            foreach(string name in products)
            {
                Console.WriteLine(name);
            }
        }

        //The Include Extension Method
        //As we know, there are two critical LINQ operators, Select and SelectMany in this group. But before jumping into the learning of this two
        //operators, let's take a look at the Include method - another very important extension method of IQueryable<T>.
        //Usually, an one-to-many relationship is expressed as a foreign key in the relational database. For example, the relationship between ProductCategory and
        //product is one-to-many, and this relationship reflects the truth that there can be many products in a category. 
        //In the AdventureWorks database, there is a foreign key constraint between the product table and the 
        //ProductCategory table. The value of the ProductCategoryID column of the product table is constrained 
        //by the ID column of the ProductCategory table, and the ID column is the primary key of the 
        //ProductCategory table.This foreign key constraint prevents the product from having a nonexistent 
        //ProductCategory ID, also describes the one-to-many relationship between a ProductCategory and its products. 
        //When generating domain models with the dotnet ef dbcontext scaffold command, the foreign keys between tables are mapped to
        //navigation properties of the domain models. For example, the ProductCategory class has the public ICollection<Product> Product { get; set; }
        //property which represents its products. Since a ProductCategory can have multiple products, the Product property is a collection.
        //While the Products class has a non-collection property public ProductCategory ProductCategory { get; set; }, 
        //which means a Product can only belong to one ProductCategory.
        //When querying the database, the data of the navigation property won't be loaded automatically.
        public void Q4()
        {
            var categories = dbContext.ProductCategory;

            //The output indicates that the Product collection property of ProductCategory objects are not loaded.
            foreach (ProductCategory productCategory in categories)
            {
                Console.WriteLine($"Category Name: {productCategory.Name}, Nr of Product: {productCategory.Product.Count}");
            }

            //That means the code Include(nameof(ProductCategory.Product)) forced the query to load the data for 
            //the navigation property. The nameof helps us get the property name "Product".
            var _categories = dbContext.ProductCategory.Include(nameof(ProductCategory.Product));

            foreach (ProductCategory productCategory in _categories)
            {
                Console.WriteLine($"{productCategory.Name.PadRight(16)} {productCategory.Product.Count}");
            }
        }

        //The Contains Operator
        //If the FirstName, MiddleName,LastName and EmailAddress of two customers are identical, 
        //we consider them to be the same customer. Given a customer, its FirstName = Maxwell, MiddleName=J., 
        //LastName=Amland and EmailAddress=maxwell0@adventure-works.com, could you find out whether this 
        //customer is in the database?
        public void Q3()
        {
            Customer customer = new Customer() 
            { 
                FirstName= "Maxwell", 
                MiddleName= "J.",
                LastName = "Amland",
                EmailAddress = "maxwell0@adventure-works.com"
            };

            //Note you need to use the ToList() method, else you get exception 
            //Theoretically, the ToList is unnecessary. Depending on the time you are taking this course, 
            //if you remove the .ToList() and run the application; you will get a System.NotSupportedException exception,
            //which indicates that the EntityFrameworkCore.SQLServer hasn't fully implemented all of the LINQ operators yet. 
            //The ToList operator helps us query out the Customer and store them in a List<Customer> collection; 
            //then we can leverage the standard LINQ operators.
            bool found = dbContext.Customer.ToList()
                .Contains(customer, new CustomerEqualityComparer());

            Console.WriteLine($"Customer found using contains: {found}");

            //Using Any operator, we can implement the same operation:
            found = dbContext.Customer
                .Any(c => c.FirstName.Equals(customer.FirstName) && 
                c.MiddleName.Equals(customer.MiddleName) &&
                c.LastName.Equals(customer.LastName) &&
                c.EmailAddress.Equals(customer.EmailAddress));

            Console.WriteLine($"Customer found using any: {found}");
        }

        //Could you find out whether all products have a ListPrice that is less then $5000.
        public void Q2()
        {
            bool valid = dbContext.Product.All(p => p.ListPrice < 5000);
            //True means that there are no products that have a larger ListPrice than $5000.  
            Console.WriteLine($"All ListPrices are below $5000: {valid}");

            //Is there any product data stored in the database?
            //Is there any product with a ListPrice that is greater than $3000?
            bool anyRecord = dbContext.Product.Any();
            bool anyProduct = dbContext.Product.Any(p => p.ListPrice > 3000);
            bool anyProduct2 = dbContext.Product.Any(p => p.ListPrice > 4000);
            bool anyProduct3 = dbContext.Product.Any(p => p.ListPrice > 5000);

            Console.WriteLine($"Is there any product data stored in the database?: {anyRecord}");
            Console.WriteLine($"Is there any product with a ListPrice that is greater than $3000?: {anyProduct}");
            Console.WriteLine($"Is there any product with a ListPrice that is greater than $4000?: {anyProduct2}");
            Console.WriteLine($"Is there any product with a ListPrice that is greater than $5000?: {anyProduct3}");
            //Don't forget that the Any operator can be used to test whether a data source is empty.
        }

        //Could you find out the products with a ListPrice greater then or equal to $2000?
        public void Q1()
        {
            IQueryable<Product> products = dbContext.Product.Where(x => x.ListPrice >= 2000);

            Console.WriteLine($"Number of products: {products.Count()}");
            foreach (Product p in products)
            {
                Console.WriteLine($"Name: {p.Name}, ListPrice: {p.ListPrice}");
            }

            //The equivalent code in query expression (SQL-like) syntax is:
            IQueryable<Product> _products = from p in dbContext.Product
                                            where p.ListPrice >= 2000
                                            select p;
            Console.WriteLine("\n\n");

            Console.WriteLine($"Number of products: {_products.Count()}");
            foreach (Product p in _products)
            {
                Console.WriteLine($"Name: {p.Name}, ListPrice: {p.ListPrice}");
            }
        }

        public void ShowCustomers()
        {
            IEnumerable<Customer> customers = dbContext.Customer.AsEnumerable();
            var products = dbContext.Product.ToList();

            Console.WriteLine("Customers");
            foreach (Customer customer in customers)
            {
                Console.WriteLine($"\tFirstName: {customer.FirstName}, LastName: {customer.LastName}");
            }

            Console.WriteLine("\nProducts");

            foreach (var product in products)
            {
                System.Console.WriteLine($"\tName: {product.Name}, ListPrice: {product.ListPrice}");
            }
        }
    }

    public class CustomerEqualityComparer : IEqualityComparer<Customer>
    {
        public bool Equals([AllowNull] Customer x, [AllowNull] Customer y)
        {
            if (x == null || y == null)
                return false;

            return x.FirstName.Equals(y.FirstName) &&
                x.MiddleName.Equals(y.MiddleName) &&
                x.LastName.Equals(y.LastName) &&
                x.EmailAddress.Equals(y.EmailAddress);
        }

        public int GetHashCode([DisallowNull] Customer obj)
        {
            return obj.GetHashCode();
        }
    }
}