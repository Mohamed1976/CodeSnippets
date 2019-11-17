using System;
using System.Collections.Generic;
using System.Text;

namespace AbstractClassesNamespace
{
    public abstract class Employee
    {
        public Employee(string employeeId, string firstName, string lastName)
        {
            id = employeeId;
            fname = firstName;
            lname = lastName;
        }

        //Default constructor
        public Employee()
        {

        }

        //We can have fields and properties in the Abstract class
        protected String id;
        protected String lname;
        protected String fname;

        //Abstract properties 
        public abstract String ID
        {
            get;
            set;
        }

        public abstract String FirstName
        {
            get;
            set;
        }

        public abstract String LastName
        {
            get;
            set;
        }

        public String Add()
        {
            return "Employee " + id + " " +
                      lname + " " + fname +
                      " added";
        }
        //completed methods


        public String Delete()
        {
            return "Employee " + id + " " +
                      lname + " " + fname +
                      " deleted";
        }
        //completed methods


        public String Search()
        {
            return "Employee " + id + " " +
                      lname + " " + fname +
                      " found";
        }

        public abstract String CalculateWage();
    }


    public class FulltimeEmployee : Employee
    {
        //We must call the constructor of the base class
        public FulltimeEmployee(string employeeId, string firstName, string lastName) : base(employeeId, firstName, lastName)
        {
        }

        //Base() is a reference to the parent class constructor.
        public FulltimeEmployee() : base()
        {
        }

        //By inheriting from abstract class we must implement the abstract members.
        //public abstract String ID
        //public abstract String FirstName
        //public abstract String LastName
        public override String ID
        {
            get

            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        public override String FirstName
        {
            get

            {
                return fname;
            }
            set
            {
                fname = value;
            }
        }

        public override String LastName
        {
            get

            {
                return lname;
            }
            set
            {
                lname = value;
            }
        }

        //Base class method is hidden and overridden 
        public new String Search()
        {
            return base.Search();
        }

        public override String CalculateWage()
        {
            return "Full time employee " +
                  base.fname + " is calculated " +
                  "from the FulltimeEmployee";
        }
    }

    //In many cases, however, the base class is not expected to provide an implementation.Instead, 
    //the base class is an abstract class that declares abstract methods; it serves as a template that 
    //defines the members that each derived class must implement.
    //Below you can see the Shape class, and several shapes derived from it.
    //For example, each closed two-dimensional geometric shape includes two properties: area, the inner 
    //extent of the shape; and perimeter, or the distance along the edges of the shape.The way in which 
    //these properties are calculated, however, depends completely on the specific shape. The formula for 
    //calculating the perimeter (or circumference) of a circle, for example, is different from that of a triangle. 
    //The Shape class is an abstract class with abstract methods.That indicates derived classes share the same functionality, 
    //but those derived classes implement that functionality differently.
    public abstract class Shape
    {
        public abstract double Area { get; }

        public abstract double Perimeter { get; }

        public override string ToString()
        {
            return "Shape type: " + GetType().Name;
        }

        //Static members, note you could also call the Area method on the derived class.  
        public static double GetArea(Shape shape)
        {
            return shape.Area;
        }
        public static double GetPerimeter(Shape shape)
        {
            return shape.Perimeter;
        }
    }

    public class Square : Shape
    {
        public Square(double length)
        {
            Side = length;
        }

        public double Side { get; }

        public override double Area => Math.Pow(Side, 2);

        public override double Perimeter => Side * 4;

        public double Diagonal => Math.Round(Math.Sqrt(2) * Side, 2);
    }

    public class Rectangle : Shape
    {
        public Rectangle(double length, double width)
        {
            Length = length;
            Width = width;
        }

        public double Length { get; }

        public double Width { get; }

        public override double Area => Length * Width;

        public override double Perimeter => 2 * Length + 2 * Width;

        public bool IsSquare() => Length == Width;

        public double Diagonal => Math.Round(Math.Sqrt(Math.Pow(Length, 2) + Math.Pow(Width, 2)), 2);
    }

    public class Circle : Shape
    {
        public Circle(double radius)
        {
            Radius = radius;
        }

        public override double Area => Math.Round(Math.PI * Math.Pow(Radius, 2), 2);

        public override double Perimeter => Math.Round(Math.PI * 2 * Radius, 2);

        // Define a circumference, since it's the more familiar term.
        public double Circumference => Perimeter;

        public double Radius { get; }

        public double Diameter => Radius * 2;

        public override string ToString()
        {
            return "Override string from Circle class, calling base, " + base.ToString();
        }
    }

    //Private members are visible only in derived classes that are nested in their base class. Otherwise, they are 
    //not visible in derived classes. In the following example, A.B is a nested class that derives from A, and C derives 
    //from A. The private A.value field is visible in A.B. However, if you remove the comments from the C.GetValue method 
    //and attempt to compile the example, it produces compiler error CS0122: "'A.value' is inaccessible due to its protection level."
    public class A
    {
        private int value = 10;

        public class B : A
        {
            public int GetValue()
            {
                return this.value;
            }
        }
    }

    //If you remove comment, compiler error will occure because value is private member of base class 
    public class C : A
    {
        //    public int GetValue()
        //    {
        //        return this.value;
        //    }
    }

    public class SimpleClass {}

    public enum PublicationType { Misc, Book, Magazine, Article };

    public abstract class Publication
    {
        private bool published = false;
        private DateTime datePublished;
        private int totalPages;

        public Publication(string title, string publisher, PublicationType type)
        {
            if (publisher == null)
                throw new ArgumentNullException("The publisher cannot be null.");
            else if (String.IsNullOrWhiteSpace(publisher))
                throw new ArgumentException("The publisher cannot consist only of white space.");
            Publisher = publisher;

            if (title == null)
                throw new ArgumentNullException("The title cannot be null.");
            else if (String.IsNullOrWhiteSpace(title))
                throw new ArgumentException("The title cannot consist only of white space.");
            Title = title;

            Type = type;
        }

        public string Publisher { get; }

        public string Title { get; }

        public PublicationType Type { get; }

        public string CopyrightName { get; private set; }

        public int CopyrightDate { get; private set; }

        public int Pages
        {
            get { return totalPages; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("The number of pages cannot be zero or negative.");
                totalPages = value;
            }
        }

        public string GetPublicationDate()
        {
            if (!published)
                return "NYP";
            else
                return datePublished.ToString("d");
        }

        public void Publish(DateTime datePublished)
        {
            published = true;
            this.datePublished = datePublished;
        }

        public void Copyright(string copyrightName, int copyrightDate)
        {
            if (copyrightName == null)
                throw new ArgumentNullException("The name of the copyright holder cannot be null.");
            else if (String.IsNullOrWhiteSpace(copyrightName))
                throw new ArgumentException("The name of the copyright holder cannot consist only of white space.");
            CopyrightName = copyrightName;

            int currentYear = DateTime.Now.Year;
            if (copyrightDate < currentYear - 10 || copyrightDate > currentYear + 2)
                throw new ArgumentOutOfRangeException($"The copyright year must be between {currentYear - 10} and {currentYear + 1}");
            CopyrightDate = copyrightDate;
        }

        public override string ToString() => Title;
    }

    public sealed class Book : Publication
    {
        public Book(string title, string author, string publisher) : this(title, String.Empty, author, publisher)
        { }

        //Constructor of the base class 
        public Book(string title, string isbn, string author, string publisher) : base(title, publisher, PublicationType.Book)
        {
            // isbn argument must be a 10- or 13-character numeric string without "-" characters.
            // We could also determine whether the ISBN is valid by comparing its checksum digit 
            // with a computed checksum.
            //
            if (!String.IsNullOrEmpty(isbn))
            {
                // Determine if ISBN length is correct.
                if (!(isbn.Length == 10 | isbn.Length == 13))
                    throw new ArgumentException("The ISBN must be a 10- or 13-character numeric string.");
                ulong nISBN = 0;
                if (!UInt64.TryParse(isbn, out nISBN))
                    throw new ArgumentException("The ISBN can consist of numeric characters only.");
            }
            ISBN = isbn;

            Author = author;
        }

        public string ISBN { get; }

        public string Author { get; }

        public Decimal Price { get; private set; }

        // A three-digit ISO currency symbol.
        public string Currency { get; private set; }


        // Returns the old price, and sets a new price.
        public Decimal SetPrice(Decimal price, string currency)
        {
            if (price < 0)
                throw new ArgumentOutOfRangeException("The price cannot be negative.");
            Decimal oldValue = Price;
            Price = price;

            if (currency.Length != 3)
                throw new ArgumentException("The ISO currency symbol is a 3-character string.");
            Currency = currency;

            return oldValue;
        }

        public override bool Equals(object obj)
        {
            Book book = obj as Book;
            if (book == null)
                return false;
            else
                return ISBN == book.ISBN;
        }

        public override int GetHashCode() => ISBN.GetHashCode();

        public override string ToString() => $"{(String.IsNullOrEmpty(Author) ? "" : Author + ", ")}{Title}";
    }
}
