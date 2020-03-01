using System;
using System.Collections.Generic;
using System.Text;

namespace _70_483.OOP
{
    class ClassHierarchyExamples
    {
        public ClassHierarchyExamples()
        {
                
        }

        public static void ShowPublicationInfo(Publication pub)
        {
            string pubDate = pub.GetPublicationDate();
            Console.WriteLine($"{pub.Title}, " + 
                $"{(pubDate == "NYP" ? "Not Yet Published" : "published on " + pubDate):d} by {pub.Publisher}");
        }

        interface IAlien
        {
            void SetState(AlienState alienState);
        }

        //Unless specified otherwise, an enumerated type is based on the int type and
        //the enumerated values are numbered starting at 0. You can modify this by adding
        //extra information to the declaration of the enum.
        enum AlienState : byte
        {
            Sleeping = 0,
            Attacking,
            Destroyed
        };

        struct Alien : IAlien
        {
            public int X;
            public int Y;
            public int Lives;

            public Alien(int x, int y) : this(x, y, 3, AlienState.Sleeping)
            {
            }

            public Alien(int x, int y, int lives, AlienState alienState)
            {
                X = x;
                Y = y;
                Lives = lives;
                State = alienState;
            }

            public void SetState(AlienState alienState)
            {
                State = alienState;
            }

            public AlienState State { get; private set; }

            //The structure definition contains an override for the ToString
            //method.This is perfectly acceptable; although a structure cannot be used in a
            //class hierarchy, because it is possible to override methods from the parent type of struct.
            public override string ToString()
            {
                return string.Format("X: {0} Y: {1} Lives: {2} AlienState: {3}", X, Y, Lives, State);
            }
        }

        class Alienclass
        {
            public int X;
            public int Y;
            public int Lives;
            private static int Max_Lives = 99;

            //use the const or readonly keyword or make the variable constants 
            private readonly int myConst;
            private const int myConst2 = 20;

            //Constructors can perform validation of their parameters to ensure that any
            //objects that are created contain valid information.If the validation fails, the
            //constructor must throw an exception to prevent the creation of an invalid object.
            public Alienclass(int x, int y) : this(x,y,3)
            {
            }

            public Alienclass(int x, int y, int lives)
            {
                if (x < 0 || y < 0)
                    throw new ArgumentOutOfRangeException("X and Y should be larger than 0.");

                if(lives > Max_Lives)
                    throw new Exception("Invalid number of lives, exceeds limit");

                X = x;
                Y = y;
                Lives = lives;
                myConst = 10;
            }

            public override string ToString()
            {
                return string.Format("X: {0} Y: {1} Lives: {2}", X, Y, Lives);
            }
        }

        public class myStack<T> where T : class //Accepts only reference types as T
        {
            //A class can contain a static constructor method. This is called once before the
            //creation of the very first instance of the class.
            //When the program runs, the message is printed once, before the first myStack object is created.The
            //static constructor is not called when the second myStack object is created.
            //A static constructor is a good place to load resources and initialize values that
            //will be used by all instances of the class. This can include the values of static members of the class
            static myStack()
            {
                Console.WriteLine("Static myStack constructor running");
            }

            private int stackTop = 0;
            //Create array that can hold 100 references.
            private T[] stack = null;

            public myStack(int stackSize)
            {
                stack = new T[stackSize];
            }

            //A program can avoid code repetition by making one constructor call another
            //constructor by use of the keyword this.
            public myStack() : this(100)
            {
            }

            public void Push(T item)
            {
                if(stackTop == stack.Length)
                    throw new Exception("Stack full");

                //Store reference to object in array
                stack[stackTop] = item;
                stackTop++;
            }

            public T Pop()
            {
                if (stackTop == 0)
                    throw new Exception("Stack empty");
                stackTop--;
                return stack[stackTop];
            }

        }

        public static void ReadValue(
            int low, // lowest allowed value
            int high, // highest allowed value
            string prompt = "" // prompt for the user
            )
        {
            Console.WriteLine($"low: {low}, high: {high}, prompt{prompt}");
        }

        class MessageDisplay
        {
            public void DisplayMessage(string message)
            {
                Console.WriteLine(message);
            }
        }

        class Customer
        {
            //Note that there is a C# convention that private members of a class have identifiers that start with
            //an underscore(_) character.
            private string _nameValue;

            //Properties provide a powerful way to enforce encapsulation
            //You can provide “read only” properties by creating properties that only contain a get behavior.
            public string Name
            {
                get
                {
                    return _nameValue;
                }
                set
                {
                    if (string.IsNullOrEmpty(value))
                        throw new Exception("Invalid customer name");
                    _nameValue = value;
                }
            }
        }

        public enum TemperatureScale : byte
        {
            Kelvin = 0,
            Celsius,
            Fahrenheit
        }

        public class Thermometer
        {
            public Thermometer(double temperature, TemperatureScale temperatureScale = TemperatureScale.Celsius )
            {
                switch(temperatureScale)
                {
                    case TemperatureScale.Celsius:
                        CelsiusDegrees = temperature;
                        FahrenheitDegrees = 1.80 * temperature + 32;
                        Kelvin = temperature + 271.15;
                        break;
                    case TemperatureScale.Fahrenheit:
                        break;
                    case TemperatureScale.Kelvin:
                        break;
                    default:
                        throw new ArgumentException("Specified TemperatureScale is not supported.");
                }
            }

            public double Kelvin { get; private set; }

            public double CelsiusDegrees { get; private set; }

            public double FahrenheitDegrees { get; private set; }
        }

        public void PrintReport(IPrintable printable)
        {
            Console.WriteLine("IPrintable " + printable.GetTitle() + ", " + printable.GetPrintableText(20, 20));
        }

        public interface IAccount : IComparable<IAccount>
        {
            void PayInFunds(decimal amount);
            bool WithdrawFunds(decimal amount);
            decimal GetBalance();
        }

        //You can use nested classes.
        //BankAccount implements the IAccount interface (Explicit)
        public class BankAccount : IAccount//, IComparable<IAccount>
        {
            private decimal _accountBalance = 0;

            public BankAccount(decimal initialBalance)
            {
                _accountBalance = initialBalance;
            }

            public int CompareTo(IAccount account)
            {
                // if we are being compared with a null object we are definitely after it
                if (account == null) return 1;
                // use the balance value as the basis of the comparison
                return this._accountBalance.CompareTo(account.GetBalance());
            }

            decimal IAccount.GetBalance()
            {
                return _accountBalance;
            }

            void IAccount.PayInFunds(decimal amount)
            {
                _accountBalance += amount;
            }

            public virtual bool WithdrawFunds(decimal amount)
            {
                if(_accountBalance >= amount)
                {
                    _accountBalance -= amount;
                    return true;
                }
                else
                {
                    return false;
                }                
            }

            protected class Address
            {
                public string FirstLine;
                public string Postcode;
            }            
        }

        //Sealed means you cannot derive from BabyAccount 
        //Check can we remove the interface definitions,
        //When parent class implements certain
        //As I understand interfaces (and my experimentation has reinforced), there is no purpose to having both 
        //the parent and the child implement the same interface.
        public sealed class BabyAccount : BankAccount//, IAccount, IComparable<IAccount>
        {
            private string _guardian;
            private const decimal _withdrawLimit = 10;
            public BabyAccount(decimal initialBalance, string guardian) : base(initialBalance)
            {
                _guardian = guardian;
            }

            public override bool WithdrawFunds(decimal amount)
            {
                Console.WriteLine("BabyAccount bool WithdrawFunds(decimal amount)");
                //You cannot withdraw if amount exceeds _withdrawLimit 
                if (amount > _withdrawLimit)
                {
                    return false;
                }
                else
                {
                    return base.WithdrawFunds(amount);
                }
            }

            public string GetGuardian()
            {
                return _guardian;
            }

            //Replacement method is not able to use base to call the method
            //that it has overridden, because it has not overridden a method, it has replaced it.I
            //cannot think of a good reason for replacing a method, and I’m mentioning this
            //feature of C# because I feel you need to know about it; and not because youshould use it.
            /* public new decimal GetBalance()
            {
                return 1000000;
            } */
        }

        public sealed class OverdraftAccount : BankAccount //, IAccount
        {
            public OverdraftAccount(decimal initialBalance) : base(initialBalance)
            {
            }

            private const decimal overdraftLimit = 100;
            Address GuarantorAddress;
        }

        public void Run()
        {
            //IEnumerable
            //The string type supports enumeration, and so a program can call the GetEnumerator method on a
            //string instance to get an enumerator. The enumerator exposes the method
            //MoveNext(), which returns the value true if it was able to move onto
            //another item in the enumeration. The enumerator also exposes a property called
            //Current, which is a reference to the currently selected item in the enumerator.
            // Get an enumerator that can iterate through a string
            IEnumerator<char> stringEnumerator = "Hello world".GetEnumerator();
            while (stringEnumerator.MoveNext())
            {
                Console.WriteLine(stringEnumerator.Current);
            }

            //Foreach uses the enumerator  
            foreach (char ch in "Hello world")
                Console.Write(ch);

            //The IComparable interface is used by .NET to determine the ordering of objects when they are sorted.
            //the interface contains a single method, CompareTo, which compares this object with another.
            //The CompareTo method returns an integer. If the value returned is less than 0 it indicates that this
            //object should be placed before the one it is being compared with.If the value
            //returned is zero, it indicates that this object should be placed at the same position
            //as the one it is being compared with and if the value returned is greater than 0 it
            //means that this object should be placed after the one it is being compared with.
            List<IAccount> accounts = new List<IAccount>();
            Random rand = new Random(1);
            for (int i = 0; i < 20; i++)
            {
                //IAccount _account = new BankAccount(rand.Next(0, 10000));
                IAccount _account = new BabyAccount(rand.Next(0, 10000), "None");
                accounts.Add(_account);
            }

            // Sort the accounts
            accounts.Sort();

            // Display the sorted accounts
            foreach (IAccount __account in accounts)
            {
                Console.WriteLine(__account.GetBalance());
            }

            //A reference to a base class in a class hierarchy can refer to an instance of any of
            //the classes that inherits from that base class. In other words, a variable declared
            //as a reference to BankAccount objects can refer to a BabyAccount instance.
            //However, the reverse is not true. A variable declared as a reference to a
            //BabyAccount object cannot be made to refer to a BankAccount object.
            //This is because the BabyAccount class may have added extra behaviors to the
            //parent class (for example a method called GetParentName). A
            //BankAccount instance will not have that method.
            //However, I much prefer it if you manage references to objects in terms of the
            //interfaces than the type of the particular object. This is much more flexible, in
            //that you’re not restricted to a particular type of object when developing the code.
            //BankAccount bankAccount = new BankAccount(1000);
            //BabyAccount babyAccount2 = (BabyAccount)bankAccount;

            //Overriding methods in inheritance relation
            BabyAccount babyAccount = new BabyAccount(1000, "Mohamed Kalmoua");
            IAccount account = babyAccount;
            account.WithdrawFunds(10);
            Console.WriteLine("Balance: " + account.GetBalance().ToString("N2"));

            //When you use an interface in a program you should ensure that all the
            //implementations of any interface methods are explicit. This reduces the chances
            //of the interface methods being used in an incorrect context.
            //When a class implements an interface it must contain an implementation of all
            //methods that are defined in the interface. Sometimes a class may implement
            //multiple interfaces, in which case it must contain all the methods defined in all
            //the interfaces.This can lead to problems, in that two interfaces might contain a
            //method with the same name. You can resolve duplicate method signatures by using explicit implementation
            Report report = new Report();
            //Because interfaces are explicitly implemented, Interface methods are not visible in the report class
            //If we want access to interface members, we need to cast to the corresponding interface   
            Console.WriteLine(report.GetTitle());
            //You can use an explicit interface implementation to make methods implementing an interface 
            //only visible when the object is accessed via an interface reference.
            IPrintable printable = report;
            IDisplay display = report;
            Console.WriteLine(printable.GetTitle() + ", " + printable.GetPrintableText(20,20));
            Console.WriteLine(display.GetTitle());
            //You might make an IPrintable
            //interface that specifies methods used to print any object. This is a good idea,
            //because now a printer can be asked to print any item that is referred to by a
            //reference of IPrintable type.In other words, any object that implements the
            //methods in IPrintable can be printed.
            PrintReport(report);

            Thermometer thermometer = new Thermometer(30, TemperatureScale.Celsius);
            Console.WriteLine("Celsius: {0:N2}, Kelvin: {1:N2}, Fahrenheit: {2:N2}", 
                thermometer.CelsiusDegrees,
                thermometer.Kelvin,
                thermometer.FahrenheitDegrees);

            //Encapsulation
            Customer customer = new Customer { Name = "Mohamed" };
            Console.WriteLine("Customer name: {0}", customer.Name);

            //Using COM Component Object Model allows you to interact with other Components such as Excel   
            //The Component Object Model(COM) is a mechanism that allows software
            //components to interact. The model describes how to express an interface to
            //which other objects can connect.COM is interesting to programmers because a
            //great many resources you would like to use are exposed via COM interfaces.
            //The code inside a COM object runs as unmanaged code, having direct access
            //to the underlying system.
            //When a .NET application wants to interact with a COM object it has to perform the following:
            //1) Convert any parameters for the COM object into an appropriate format
            //2) Switch to unmanaged execution for the COM behavior
            //3) Invoke the COM behavior
            //4) Switch back to managed execution upon completion of the COM behavior
            //5) Convert any results of the COM request into the correct types of .NETobjects
            //The steps above are performed by a component called the Primary Interop Assembly(PIA)
            //that is supplied along with the COM object.The results returned by the PIA can
            //be managed as dynamic objects, so that the type of the values can be inferred
            //rather than having to be specified directly.As long as your program uses the
            //returned values in the correct way, the program will work correctly.You add a
            //Primary Interop Assembly to an application as you would any other assembly.
            //The C# code uses dynamic types to make the interaction with the Office
            //application very easy.There is no need to cast the various elements that the
            //program is interacting with, as they are exposed by the interop as dynamic types,
            //so conversion is performed automatically based on the inferred type of an assignment destination.
            //You can create applications that interact with different versions of Microsoft
            //Office by embedding the Primary Interop Assembly in the application. This is
            //achieved by setting the Embed Interop Types option of the assembly reference to True.
            //This removes the need for any interop assemblies on the machine running the application.

            //Primary Interop Assembly (PIA), You add a Primary Interop Assembly to an application as you would any other assembly.
            // Example shows how to interact with Excel COM object
            //Create the interop
            //var excelApp = new Microsoft.Office.Interop.Excel.Application
            // make the app visible
            //excelApp.Visible = true;
            // Add a new workbook
            //excelApp.Workbooks.Add();
            // Obtain the active sheet from the app
            // There is no need to cast this dynamic type
            //Microsoft.Office.Interop.Excel.Worksheet workSheet = excelApp.ActiveSheet
            // Write into two cells
            //workSheet.Cells[1, "A"] = "Hello";
            //workSheet.Cells[1, "B"] = "from C#";

            //The ExpandoObject class allows a program to dynamically add properties to an object.
            //The dynamic variable person is assigned to a new ExpandoObject
            //instance.The program then adds Name and Age properties to the person and then prints out these values.
            //A program can add ExpandoObject properties to an ExpandoObject to
            //create nested data structures. An ExpandoObject can also be queried using
            //LINQ and can exposes the IDictionary interface to allow its contents to be
            //queried and items to be removed.ExpandoObject is especially useful when
            //creating data structures from markup languages, for example when reading a JSON or XML document.
            dynamic person = new System.Dynamic.ExpandoObject();
            person.Name = "Mohamed";
            person.Age = 40;
            
            //Handle dynamic types
            //C# is a strongly typed language. This means that when the program is compiled
            //the compiler ensures that all actions that are performed are valid in the context of
            //the types that have been defined in the program. As an example, if a class does
            //not contain a method with a particular name, the C# compiler will refuse to
            //generate a call to that method.As a way of making sure that C# programs are
            //valid at the time that they are executed, strong typing works very well.
            //No strong typing information is avialable when using the following types:
            //Common Object Model (COM) interop, Document Object Model (DOM),
            //reflection, or when interworking with dynamic languages such as JavaScript.
            //The keyword dynamic is used to identify items for which the C# compiler should suspend static type checking.
            //The flexibility was added to make it easy to interact with other
            //languages and libraries written using the Component Object Model(COM).
            dynamic messageDisplay = new MessageDisplay();
            messageDisplay.DisplayMessage("Hello there.");
            //messageDisplay.DoesNotExist("Hello there."); //Although method does not exist, it compiles
            dynamic d = 99;
            d = d + 1;
            Console.WriteLine(d);
            d = "Hello";
            d = d + " Rob";
            Console.WriteLine(d);

            //Convert types with System.Convert
            //The System.Convert class provides a set of static methods that can be used
            //to perform type conversion between.NET types. The convert method will throw an exception if the 
            //string provided cannot be converted into an integer.
            int intA = Convert.ToInt32("2020");

            //You can define explicit and implicit operators in your class
            Miles miles = new Miles(1000);
            int nrOfMiles = (int)miles;
            Kilometers kilometers = miles;
            Console.WriteLine("Kilometers: {0}, Miles: {1}", kilometers, nrOfMiles);

            //Boxing and unboxing, int, float, double and structs are value types
            //From a computational point of view, value types such as int and float
            //have the advantage that the computer processor can manipulate value types
            //directly.Adding two int values together can be achieved by fetching the values
            //into the processor, performing the addition operation, and then storing the result.
            //It can be useful to treat value types as reference types, and the C# runtime
            //system provides a mechanism called boxing that will perform this conversion when required.
            int val = 2020;
            object obj = val; //Boxing (convert a value type to a reference type)
            int val2 = (int)obj; //unboxing (convert a reference type to a value type)
            //Each built-in C# value type(int, float etc) has a matching C# type called its interface type to
            //which it is converted when boxing is performed.The interface type for int is int32.
            //Boxing and unboxing values slows the program down, using boxing/unboxing in solution is a symptom of poor design.

            //Cast types
            //Note that casting cannot be used to convert between different types, string to int for example
            //C# program will not allow a programmer to perform a conversion between types that result in the loss of data.
            float floatX = 9.9f;
            //explicit conversion, narrowing, lost of data 
            int intX = (int)(floatX + 0.5); //Explicit cast is necessary because of data lost (int i will have value 9) 

            //Widening conversion no lost of data, implicite conversion, no casting needed.
            int intY = 2020;
            double doubleX = intY;

            //Casting is also used when converting references to objects that may be part of class hierarchies or expose interfaces.

            //Other types than integers can be used in indexed properties.
            //This is how the Dictionary collection is used to index on a particular type of key value.
            StringIndexedClass stringIndexedClass = new StringIndexedClass();
            stringIndexedClass["zero"] = 1976;
            stringIndexedClass["one"] = 2020;
            Console.WriteLine("stringIndexedClass[\"zero\"]: {0}, stringIndexedClass[\"one\"]: {1}",
                stringIndexedClass["zero"],
                stringIndexedClass["one"]);

            //A class can use the same indexing mechanism as used in arrays to provide indexed propertyvalues.
            //Indexed properties
            IndexedClass indexedClass = new IndexedClass();
            indexedClass[0] = 10;
            indexedClass[1] = 20;
            Console.WriteLine("indexedClass[0]: {0}, indexedClass[1]: {1}", indexedClass[0], indexedClass[1]);

            //Optional and named parameters
            ReadValue(high: 20, low: 5);

            //Example of constructor overloading
            DateTime d0 = new DateTime(ticks: 636679008000000000);
            DateTime d1 = new DateTime(year: 2018, month: 7, day: 23);
            Console.WriteLine(d0);
            Console.WriteLine(d1);

            //Using extension method.
            //Extension methods allow you to add behaviors to existing classes and use
            //them as if they were part of that class. They are very powerful.LINQ query
            //operations are added to types in C# programs by the use of extension methods.
            string text = "A rocket explorer called Wright, \nOnce travelled much faster than light, \nHe set out one day,\nIn a relative way,\nAnd returned on the previous night";
            Console.WriteLine("text.LineCount(): " +text.LineCount());
            
            //If an object only has a private constructor it cannot be
            //instantiated unless the object contains a public factory method that can be called
            //to create instances of the class.
            //When creating objects that are part of a class hierarchy, a programmer must
            //ensure that information required by the constructor of a parent object is passed
            //into a parent constructor.
            myStack<string> myStack = new myStack<string>();
            myStack.Push("Rob");
            myStack.Push("Mary");
            Console.WriteLine(myStack.Pop());
            Console.WriteLine(myStack.Pop());

            //Note that when a variable of type Alien is declared, the variable is now a
            //reference to an Alien, and initially the reference does not refer to anything.
            //When we created the alien swarm we had to explicitly set each element in the
            //array to refer to an Alien instance. Create an array of Alienclass references 
            //Note that declaring a class does not create any
            //instances of that class. An instance of a class is created when the new keyword is used.
            Alienclass[] aliens = new Alienclass[10];
            for (int i = 0; i < aliens.Length; i++)
                aliens[i] = new Alienclass(0, 0);

            Alienclass c = new Alienclass(100, 100);
            Console.WriteLine("Alien C {0}", c);
            Console.WriteLine("aliens [0] {0}", aliens[0]);

            //Structures can contain methods, data values, properties and can haveconstructors.
            //1) Structures support interfaces but you cannot use inheritance. 
            //2) Structure instances are generally created on the program stack
            //3) The constructor for a structure must initialize all the data members in the
            //   structure.Data members cannot be initialized in the structure.
            //4) It is not possible for a structure to have a parameterless constructor.
            //   However, as you shall see, it is possible for a structure to be created by
            //   calling a parameterless constructor on the structure type, in which case all
            //   the elements of the structure are set to the default values for that type
            //   (numeric elements are set to zero and strings are set to null).
            Alien alien = new Alien();
            alien.X = 50;
            alien.Y = 50;
            alien.Lives = 3;
            Console.WriteLine("alien: {0}", alien.ToString());

            Alien alienB = new Alien(100, 100);
            Console.WriteLine("alienB {0}", alienB.ToString());

            //calling a parameterless constructor on the structure type
            //the elements of the structure are set to the default values for that type
            Alien[] swarm = new Alien[100];
            Console.WriteLine("swarm [0] {0}", swarm[0].ToString());
            //A program can use casting (see the Cast Types section) to obtain the numeric
            //value that is held in an enum variable.
            Console.WriteLine("AlienState: {0} Value: {1}", alien.State, (byte)alien.State);
            
            //A good example of a value type is the DateTime structure provided by the .NET library.
            //When an assignment to a DateTime variable is performed, all of
            //the values that represent the date are copied into the destination variable.
            DateTime birthDay = new DateTime(1976, 7, 4);
            DateTime birthDay2 = birthDay;
            Console.WriteLine("{0} == {1}", birthDay.ToString("d"), birthDay2.ToString("d"));

            // A good example of a reference type is the Bitmap class from the
            // System.Drawing library in .NET.The Bitmap class is used to create
            //objects that hold all of the pixels that make up an image on the screen.Images
            //can contain millions of pixels.If a Bitmap is held as a value type, when one
            //Bitmap image is assigned to another, all of the pixels in the source Bitmap
            //must be copied from the source image into the destination.Because bitmaps are
            //managed by reference, an assignment simply makes the destination reference
            //refer to the same object as the source reference.

            //Immutability in types, The DateTime structure and strings are immutable types 
            
            //The following example uses objects derived from Shape. It instantiates an array of objects derived from 
            //Shape and calls the static methods of the Shape class, which wraps return Shape property values. 
            //The runtime retrieves values from the overridden properties of the derived types. The example also 
            //casts each Shape object in the array to its derived type and, if the cast succeeds, retrieves properties 
            //of that particular subclass of Shape.
            Shape[] shapes = { new Rectangle(10, 12), new Square(5), new Circle(3) };
            foreach (Shape shape in shapes)
            {
                Console.WriteLine($"{shape.ToString()}: area, {Shape.GetArea(shape)}; " + $"perimeter, {Shape.GetPerimeter(shape)}");
                Rectangle rect = shape as Rectangle;
                Square sq = shape as Square;
                if (rect != null)
                {
                    Console.WriteLine($"   Is Square: {rect.IsSquare()}, Diagonal: {rect.Diagonal}");
                }
                else if(sq != null)
                {
                    Console.WriteLine($"   Diagonal: {sq.Diagonal}");
                }
            }

            Book book = new Book("The Tempest", "0971655819", "Shakespeare, William", "Public Domain Press");
            ShowPublicationInfo(book);
            book.Publish(new DateTime(2016, 8, 18));
            ShowPublicationInfo(book);

            Book book2 = new Book("The Tempest", "0971655819", "Classic Works Press", "Shakespeare, William");
            Console.WriteLine($"{book.Title} and {book2.Title} are the same publication: " + $"{book.Equals(book2)}");

            //You use the abstract keyword to force derived classes to provide an implementation. You use the virtual keyword 
            //to allow derived classes to override a base class method. By default, methods defined in the base class are not overridable.
            //By default, a base class can be instantiated by calling its class constructor. You do not have to explicitly 
            //define a class constructor. If one is not present in the base class' source code, the C# compiler automatically 
            //provides a default (parameterless) constructor.
            //You can apply the sealed keyword to indicate that a class cannot serve as a base class for any additional classes.

            Automobile automobile = new Automobile("Packard", "Custom Eight", 1948);
            Console.WriteLine("Automobile: {0:G}", automobile);

            //https://docs.microsoft.com/en-us/dotnet/csharp/tutorials/inheritance
            //Inheritance is a feature of object-oriented programming languages that allows you to define a base class that provides 
            //specific functionality(data and behavior) and to define derived classes that either inherit or override that functionality.
            //Inheritance is one of the fundamental attributes of object-oriented programming. It allows you to define a child class 
            //that reuses (inherits), extends, or modifies the behavior of a parent class. The class whose members are inherited 
            //is called the base class. The class that inherits the members of the base class is called the derived class.
            //C# and .NET support single inheritance only. That is, a class can only inherit from a single class. However, 
            //inheritance is transitive, which allows you to define an inheritance hierarchy for a set of types. In other words, 
            //type D can inherit from type C, which inherits from type B, which inherits from the base class type A. 
            //Because inheritance is transitive, the members of type A are available to type D.
            //The following members are not inherited: Static constructors, Instance constructors, Finalizers
            //A member's accessibility affects its visibility for derived classes as follows:
            //1) Private members are visible only in derived classes that are nested in their base class. Otherwise, they are not visible in derived classes.
            //2) Protected members are visible only in derived classes, for example you can't instantiate object and access its protected members
            //3) Internal members are visible only in derived classes that are located in the same assembly as the base class. 
            //They are not visible in derived classes located in a different assembly from the base class
            ClassExampleA.ClassExampleB b = new ClassExampleA.ClassExampleB();
            b.DisplayValue();

            //4) Public members are visible in derived classes and are part of the derived class' public interface. 
            //Public inherited members can be called just as if they are defined in the derived class.
            ClassExampleD classExampleD = new ClassExampleD();
            classExampleD.ShowStatus();

            //Derived classes can also override inherited members by providing an alternate implementation.
            //In order to be able to override a member, the member in the base class must be marked with the virtual keyword.
            //By default, base class members are not marked as virtual and cannot be overridden.Attempting to override a 
            //non-virtual member, as the following example does, generates compiler error CS0506: "<member> cannot override 
            //inherited member <member> because it is not marked virtual, abstract, or override.
            ClassExampleF classExampleF = new ClassExampleF();
            classExampleF.DisplayInfo();

            //A class that declares an Abstract method must be declared abstract.
            //Note you can have an Abstract class without abstract methods, class can only be used as template
            //in derived classes and the abstract class cannot be instantiated. 
            //An abstract class without any abstract methods indicates that this class represents an abstract concept that 
            //is shared among several concrete classes
            //In some cases, a derived class must override the base class implementation. 
            //Base class members marked with the abstract keyword require that derived classes override them.
            ClassExampleH classExampleH = new ClassExampleH();
            classExampleH.MethodA(); //Abstract method is implemented in derived class, you must implement abstract methods in derived classes   
            classExampleH.MethodC(); //Method is implemented in abstract class

            //Inheritance applies only to classes and interfaces.Other type categories(structs, delegates, and enums) do not support 
            //inheritance. Struct can use interfaces as shown in example below.
            PersonStructure personStructure = new PersonStructure("Mohamed", DateTime.Now);
            Console.WriteLine("{0}, {1}", personStructure.Name, personStructure.BirthDate.ToString("d"));
        }

        private struct PersonStructure : IPerson
        {
            public PersonStructure(string name, DateTime birthDate)
            {
                Name = name;
                BirthDate = birthDate;
            }

            public string Name { get; private set; }
            public DateTime BirthDate { get; private set; }
        }

        private interface IPerson
        {
            string Name { get; }
            DateTime BirthDate { get; }
        }

    }

    //Extension methods provide a way in which behaviors can be added to a class without needing to extend the class
    //itself. You can think of the extension methods as being “bolted on” to an existing class.
    public static class MyExtensions
    {
        public static int LineCount(this String str)
        {
            return str.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;
            }
        }
    }
