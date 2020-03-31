using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace _70_483_USING_NET_FRAMEWORK.Exercises
{
    public class ExamNotes002
    {
        private void MaximumTermReached(object sender, MaxLeaseReachedEventArgs args)
        {
            Console.WriteLine("#3, {0}, {1}", sender.ToString(), args);
        }

        public string GenerateHash(string filename, string hashAlgorithm)
        {
            HashAlgorithm hashAlgorithm1 = HashAlgorithm.Create(hashAlgorithm);
            byte[] content = File.ReadAllBytes(filename);
            byte[] hash = hashAlgorithm1.ComputeHash(content);
            return BitConverter.ToString(hash).Replace("-", "");
        }

        delegate int AddOperation(int x, int y);

        public bool StringCompare(string string1, string string2, string string3)
        {
            StringBuilder stringBuilder = new StringBuilder(string1);
            stringBuilder.Append(string2);
            bool result = stringBuilder.ToString().Equals(string3, StringComparison.CurrentCultureIgnoreCase);
            return result;
        }

        private bool Method()
        {
            Console.WriteLine("Method() is called.");
            return true;
        }

        public void Run()
        {
            //-----------------------------------------------------------------------------------
            // Using EventHandler<EventArgs>
            //-----------------------------------------------------------------------------------
            Subscriber subscriber = new Subscriber();
            subscriber.Subscribe();
            subscriber.RaiseAlert();

            //-----------------------------------------------------------------------------------
            // Cast from value type to reference type and back
            //-----------------------------------------------------------------------------------
            float degrees = 20.45f;
            object degreesRef = degrees;
            int result = (int)(float)degreesRef;
            Console.WriteLine(result);

            //-----------------------------------------------------------------------------------
            // You are developing an application that includes a class named Customer.
            //The application will output the Customer class as a structured XML document by using the
            //following code segment:
            //
            //  <? xml version = "1.0" ?>
            //  < Prospect  xmlns:xsi = "http://www.w3.org/2001/XMLSchema-instance" xmlns: 
            //              xsd = "http://www.w3.org/2001/XMLSchema" ProspectId = "2cd70354-9fac-4041-99f6-1d37a2fa817d" 
            //              xmlns = "http://prospect" >
            //      < FullName > David Jones </ FullName >
            //      < DateOfBirth > 1976 - 07 - 04T00: 00:00 </ DateOfBirth >
            //  </ Prospect >
            //
            // You need to ensure that the Customer class will serialize to XML.
            // XmlSerialization see Xml attributes in class 
            //-----------------------------------------------------------------------------------
            Customer customer = new Customer() 
            { 
                Name = "David Jones", 
                DateOfBirth = new DateTime(1976, 7, 4), 
                Id = Guid.NewGuid(), 
                Tin = 1 
            };

            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(customer.GetType());
                using (MemoryStream memoryStream1 = new MemoryStream())
                {
                    using (StreamReader streamReader1 = new StreamReader(memoryStream1))
                    {
                        xmlSerializer.Serialize(memoryStream1, customer);
                        memoryStream1.Position = 0;
                        Console.WriteLine(streamReader1.ReadToEnd());
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            } 

            //-----------------------------------------------------------------------------------
            // Linq query
            //-----------------------------------------------------------------------------------
            decimal[] loanAmounts = new decimal[] { 303m, 1000m, 7654m, 3453m, 1200m, 400m, 22m};

            IEnumerable<decimal> loanAmounts1 = from value in loanAmounts
                                                where value % 2 == 0
                                                orderby value ascending
                                                select value;

            foreach (decimal value in loanAmounts1)
                Console.WriteLine(value);

            //-----------------------------------------------------------------------------------
            // An application serializes and deserializes XML from streams.
            // The application reads the XML streams by using a DataContractSerializer object 
            // that is declared by the following code segment: var ser = new DataContractSerializer(typeof(Name));
            // You need to ensure that the application preserves the element ordering as provided in the XML stream.
            // 1) LastName
            // 2) FirstName
            //-----------------------------------------------------------------------------------
            List<Dog> list2 = new List<Dog>()
            {
                new Dog() { FirstName="John", LastName="Kong" },
                new Dog() { FirstName="Ivan", LastName="Gupta" }
            };

            DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(List<Dog>));
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (StreamReader streamReader = new StreamReader(memoryStream))
                {
                    dataContractSerializer.WriteObject(memoryStream, list2);
                    memoryStream.Position = 0;
                    Console.WriteLine(streamReader.ReadToEnd());
                    //Deserialize
                    memoryStream.Position = 0;
                    List<Dog> dogs = dataContractSerializer.ReadObject(memoryStream) as List<Dog>;
                    foreach (Dog dog in dogs)
                        Console.WriteLine(dog.FirstName + ", " + dog.LastName);
                }
            }

            //-----------------------------------------------------------------------------------
            // Linq query
            //-----------------------------------------------------------------------------------
            DateTime[] dates = new DateTime[]
            {
                DateTime.Now.AddDays(-1),
                DateTime.Now.AddDays(-4),
                DateTime.Now.AddDays(-3),
                DateTime.Now.AddDays(-2)
            };

            DateTime dates1 = dates.Where(x => x.Year == DateTime.Now.Year)
                .OrderByDescending(x => x).FirstOrDefault(); // Or .First();

            Console.WriteLine(dates1);
            
            //-----------------------------------------------------------------------------------
            // IEnumerator
            //-----------------------------------------------------------------------------------
            string sentence = "The fox just ran the marathon.";
            IEnumerator<char> enumerator = sentence.GetEnumerator();
            while(enumerator.MoveNext())
            {
                Console.WriteLine(enumerator.Current);
            }
            
            //-----------------------------------------------------------------------------------
            // The short circuiting behavior of the and operator
            //-----------------------------------------------------------------------------------
            object obj = null;
            bool _ = obj == null || Method(); //Method is only called if obj != null  
            //List<string> list1 = null;
            //list1 ??= new List<string>(); c# 8.0 
            int? val1 = 4;
            int val2 = 3;
            int val3 = val1 ?? val2;
            Console.WriteLine(val3);

            //-----------------------------------------------------------------------------------
            // You are creating a method by using C#. The method will accept three strings as parameters.
            // The parameters are named string1, string2, and string3. The parameter values range from 5,000 to 15,000 characters.
            // You need to ensure that StringCompare only returns true if string1 concatenated to string2 is equal to string3. 
            // The comparison must be case-insensitive. The solution must ensure that StringCompare executes as quickly as possible.
            // https://docs.microsoft.com/en-us/dotnet/csharp/how-to/compare-strings
            //-----------------------------------------------------------------------------------
            Console.WriteLine("Alfa + Beta == AlfaBeta: {0}", StringCompare("Alfa", "Beta", "AlfaBeta"));

            //-----------------------------------------------------------------------------------
            // Delegate example
            //-----------------------------------------------------------------------------------
            AddOperation addOperation = delegate (int x, int y)
            {
                return x + y;
            };

            Console.WriteLine("66 + 99 = {0}", addOperation(66,99));
            
            //-----------------------------------------------------------------------------------
            // You are developing a C# application. The application includes a class named Rate.
            // You define a collection of rates named rateCollection by using the following code segment: 
            // Collection<Rate> rateCollection = new Collection<Rate>() ;
            // The application receives an XML file that contains rate information.
            // You need to parse the XML file and populate the rateCollection collection with Rate objects.
            //
            // Notes
            // The element name is rate not Ratesheet.
            // The Xmlreader readToFollowing reads until the named element is found.
            // The following example gets the value of the first attribute. reader.ReadToFollowing("book"); reader.MoveToFirstAttribute();
            // string genre = reader.Value; Console.WriteLine("The genre value: " + genre);
            // The following example displays all attributes on the current node.
            // if (reader.HasAttributes) 
            // {
            //      Console.WriteLine("Attributes of <" + reader.Name + ">"); while (reader.MoveToNextAttribute()) { Console.WriteLine(" {0}={1}", reader.Name, reader.Value);
            // }
            // Move the reader back to the element node. reader.MoveToElement(); 
            // The XmlReader.MoveToElement method moves to the element that contains the current attribute node.
            // Reference: XmlReader Methods
            // https://msdn.microsoft.com/en-us/library/System.Xml.XmlReader_methods(v=vs.110).aspx
            // https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreader?redirectedfrom=MSDN&view=netframework-4.8#methods
            //-----------------------------------------------------------------------------------
            List<Rate> rateCollection = new List<Rate>();
            using (FileStream fileStream = new FileStream(@".\Rates.xml", FileMode.Open, FileAccess.Read))
            {
                byte[] content = new byte[fileStream.Length];
                fileStream.Read(content, 0, content.Length);
                Console.WriteLine(Encoding.UTF8.GetString(content));
                fileStream.Position = 0;
                using (XmlReader xmlReader = XmlReader.Create(fileStream))
                {
                    while(xmlReader.ReadToFollowing("rate"))
                    {
                        Rate rate = new Rate(); 
                        xmlReader.MoveToFirstAttribute();
                        rate.Category = xmlReader.Value;
                        xmlReader.MoveToNextAttribute();
                        DateTime rateDate; 
                        if(DateTime.TryParse(xmlReader.Value, out rateDate))
                        {
                            rate.Date = rateDate;
                        }

                        xmlReader.ReadToFollowing("value");
                        decimal val;
                        if(decimal.TryParse(xmlReader.ReadElementContentAsString(), out val))
                        {
                            rate.Value = val; 
                        }

                        rateCollection.Add(rate);
                    }
                }
            }

            foreach (Rate rate1 in rateCollection)
                Console.WriteLine(rate1);

            //-----------------------------------------------------------------------------------
            // Simple loop exercises
            //-----------------------------------------------------------------------------------
            int[] intArray = new int[] { 1, 2, 3, 4, 5 };
            //The second foreach needs to display 1,3,6,10,15

            //Correct displays 1,3,6,10,15
            int sum = 0;
            for(int i = 0; i < intArray.Length;)
            {
                sum += intArray[i];
                intArray[i++] = sum;
            }

            foreach (int i in intArray)
                Console.WriteLine("Correct: " + i);

            intArray = new int[] { 1, 2, 3, 4, 5 };

            //WRONG displays 2,4,6,8,10
            for(int i = 0; i < intArray.Length; i++)
            {
                intArray[i] += intArray[i];
            }

            foreach (int i in intArray)
                Console.WriteLine("Wrong: " + i);

            intArray = new int[] { 1, 2, 3, 4, 5 };

            for(int i = 1; i < intArray.Length; i++)
            {
                intArray[i] += intArray[i -1];
            }

            foreach (int i in intArray)
                Console.WriteLine("Correct: " + i);

            //-----------------------------------------------------------------------------------
            // Activator.CreateInstance
            //-----------------------------------------------------------------------------------
            Type type = typeof(Product);
            Product product2 = (Product)Activator.CreateInstance(type, new object[] { "Cherry", 1 });
            Console.WriteLine(product2);

            //-----------------------------------------------------------------------------------
            // Linq query
            //-----------------------------------------------------------------------------------
            List<Product> products = new List<Product>()
            {
                new Product() { Name="Strawberry", CategoryID=1 },
                new Product() { Name="Banana", CategoryID=1 }
            };

            if (products is List<Product>)
            {
                Console.WriteLine("products is List<Product>");
            }
            else
            {
                Console.WriteLine("products is NOT List<Product>");
            }

            //We could also use ToList()
            List<Product> selectedProducts = (from product in products
                                             where product.Name.StartsWith("b", StringComparison.OrdinalIgnoreCase)
                                             select product).ToList();

            foreach (Product p in selectedProducts)
                Console.WriteLine(p.Name +", "+p.CategoryID);
            
            //-----------------------------------------------------------------------------------
            // Abstract class and overriding methods
            //-----------------------------------------------------------------------------------
            BaseLogger logger = new Logger("Initialization message.");
            logger.Log("Log started.");
            logger.Log("Base: Log continuing");
            logger.LogCompleted();
           
            using (XmlWriterTraceListener xmlWriterTraceListener = new XmlWriterTraceListener(@"./_XmlWriter.xml"))
            {
                xmlWriterTraceListener.TraceEvent(new TraceEventCache(), "Error that occured", TraceEventType.Information, 11);
            }

            //We can also use the EventLog
            EventLog eventLog = new EventLog();
            eventLog.Source = "Application";
            eventLog.WriteEntry("My error written to event log.", EventLogEntryType.Warning);
            
            //-----------------------------------------------------------------------------------
            // Calculate HASH value of file content 
            //-----------------------------------------------------------------------------------
            //CBE35DC4F5F4980645C3656E9E1146076B1294445C9704DE68B4971B93946942
            const string pdfFile = @"C:\Users\moham\Desktop\Notes\exercises.pdf";
            string hash = GenerateHash(pdfFile, "SHA256");
            Console.WriteLine(hash);

            //-----------------------------------------------------------------------------------
            //You need to declare a delegate for a method that accepts an integer as a parameter, and then returns an integer.
            //-----------------------------------------------------------------------------------
            Func<int, int, int> addMethod = (x, y) => x + y;
            Console.WriteLine("7 + 17 = {0}", addMethod(7, 17));

            //-----------------------------------------------------------------------------------
            // List<int>
            //-----------------------------------------------------------------------------------
            List<int> numbers = new List<int>() { 100, 95, 80, 75, 95 };
            int[] results = (from number in numbers where number > 80 select number).ToArray();
            foreach (int i in results)
                Console.WriteLine("i: {0}", i);
            
            //-----------------------------------------------------------------------------------
            // Using Trace with Listeners
            //-----------------------------------------------------------------------------------
            EventLogTraceListener eventLogTraceListener = new EventLogTraceListener("Demo Processing log");
            Trace.Listeners.Add(eventLogTraceListener);
            TraceSwitch traceSwitch = new TraceSwitch("MyTraceSwitch", "MyTraceSwitch description");
            traceSwitch.Level = TraceLevel.Warning;
            if(traceSwitch.TraceError)
            {
                Trace.WriteLine("My error message.");
                Trace.TraceInformation("Information message.");
                Trace.TraceError("Error message.");
            }
            
            //using(StreamWriter fileStream = new StreamWriter("Console.txt"))
            //{
            //    Console.SetOut(fileStream);
            //    Console.WriteLine("Console.WriteLine first line 1");
            //    Console.WriteLine("Console.WriteLine first line 2");
            //    Console.WriteLine("Console.WriteLine first line 3");
            //}

            //Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));
            //Console.WriteLine("Console.WriteLine first line 1");
            //Console.WriteLine("Console.WriteLine first line 2");
            //Console.WriteLine("Console.WriteLine first line 3");

            //-----------------------------------------------------------------------------------
            // Using Trace with Listners defined in config file
            //-----------------------------------------------------------------------------------
            Trace.WriteLine("This is a trace message.");
            Trace.TraceInformation("This is a trace Information message.");
            Trace.TraceWarning("This is a trace Warning message.");
            Trace.TraceError("This is a Error message.");

            //-----------------------------------------------------------------------------------
            // Example of Eventhandler and custom EventArgs 
            //-----------------------------------------------------------------------------------
            Lease lease = new Lease() { Term = 2 };
            lease.OnMaximumTermReached += delegate (object sender, MaxLeaseReachedEventArgs args)
            {
                Console.WriteLine("#1, {0}, {1}", sender.ToString(), args);
            };

            //We can also use Lambda expression
            lease.OnMaximumTermReached += (object sender, MaxLeaseReachedEventArgs args) =>
            {
                Console.WriteLine("#2, {0}, {1}", sender.ToString(), args);
            };

            //Reference local function
            lease.OnMaximumTermReached += MaximumTermReached;

            lease.OnMaximumTermReached += delegate
            {
                Console.WriteLine("#4, delegate without arguments.");
            };

            lease.Term = 7;
            
            //-----------------------------------------------------------------------------------
            //You are creating a console application by using C#.
            //You need to access the application assembly.
            //-----------------------------------------------------------------------------------
            Assembly currentAssem = Assembly.GetExecutingAssembly();
            // Display the name of the assembly.
            Console.WriteLine("Fullname: {0}", currentAssem.FullName);
            // Get the location of the assembly using the file: protocol.
            Console.WriteLine("CodeBase: {0}", currentAssem.CodeBase);
        }
    }

    public delegate void MaximumTermReachedHandler (object sender, MaxLeaseReachedEventArgs args);

    public class Lease
    {
        public event MaximumTermReachedHandler OnMaximumTermReached 
            = delegate { Console.WriteLine("Default handler: OnMaximumTermReached called.");  };            
            //We can also define delegate with arguments 
            //= delegate (object sender, MaxLeaseReachedEventArgs args) { Console.WriteLine("OnMaximumTermReached called."); };
        private int _term;
        private const int MaximumTerm = 5;
        private const decimal Rate = 0.034m;

        public int Term 
        {
            get
            {
                return _term;
            }
            
            set
            {
                if(value < MaximumTerm)
                {
                    _term = value;
                }
                else
                {
                    if(OnMaximumTermReached != null)
                    {
                        OnMaximumTermReached(this, new MaxLeaseReachedEventArgs(_term, MaximumTerm, Rate));
                    }
                }
            }
        }

        public override string ToString()
        {
            return "Lease: " + Term + ", "+ MaximumTerm +", "+ Rate.ToString("N3");
        }
    }

    public class MaxLeaseReachedEventArgs : EventArgs
    {
        public MaxLeaseReachedEventArgs(int term, int maxTerm, decimal rate)
        {
            Term = term;
            MaxTerm = maxTerm;
            Rate = rate;
        }

        public int Term { get; private set; }
        public int MaxTerm { get; private set; }
        public decimal Rate { get; private set; }

        public override string ToString()
        {
            return "EventArgs: " + Term + ", " + MaxTerm + ", " + Rate.ToString("N3");
        }
    }

    //The abstract keyword enables you to create classes and class members that are incomplete and 
    //must be implemented in a derived class. An abstract class cannot be instantiated. The purpose 
    //of an abstract class is to provide a common definition of a base class that multiple derived classes can share.
    public abstract class BaseLogger
    {
        public BaseLogger(string message)
        {
            Message = message;
        }

        protected virtual string Message { get; set; }

        public virtual void Print()
        {
            Console.WriteLine("BaseLogger, Msg: {0}", Message);
        }

        public virtual void Log(string message)
        {
            Console.WriteLine("Base: " + Message);
        }

        public void LogCompleted()
        {
            Console.WriteLine("Completed");
        }
    }

    public class Logger : BaseLogger
    {
        public Logger(string message) : base(message)
        {
            Message = message;
        }

        protected override string Message { get; set; }

        public override void Log(string message)
        {
            Console.WriteLine(message);
        }

        public new void LogCompleted()
        {
            Console.WriteLine("Finished");
        }
    }

    public class Product
    {
        public Product()
        {

        }
        public Product(string name, int categoryID)
        {
            Name = name;
            CategoryID = categoryID;
        }

        public string Name { get; set; }
        public int CategoryID { get; set; }

        public override string ToString()
        {
            return Name + ", " + CategoryID;
        }
    }

    public class Rate
    {
        public string Category { get; set; }
        public DateTime Date { get; set; }
        public decimal Value { get; set; }

        public override string ToString()
        {
            return Category+ ", " + Date +", "+ Value;
        }
    }

    [DataContract(Namespace ="http://www.nu.nl")]
    public class Dog
    {
        [DataMember(Name= "SureName", Order = 2)]
        public string FirstName { get; set; }

        [DataMember(Name = "MothersName", Order = 1)]
        public string LastName { get; set; }
    }

    [XmlRoot("Prospect", Namespace = "http://prospect")]
    public class Customer
    {
        [XmlAttribute("ProspectId")]
        public Guid Id { get; set; }

        [XmlElement("FullName", Order = 1)]
        public string Name { get; set; }

        [XmlElement(Order = 2)]
        public DateTime DateOfBirth { get; set; }
        
        [XmlIgnore]
        public int Tin { get; set; }
    }

    public class Alert
    {
        public event EventHandler<EventArgs> SendMessage;

        public void RaiseAlert()
        {
            if(SendMessage != null)
            {
                SendMessage(this, new EventArgs());
            }
        }
    }

    public class Subscriber
    {
        private Alert alert = null;

        public Subscriber()
        {
            alert = new Alert();
        }

        public void Subscribe()
        {
            alert.SendMessage += (sender, arg) => Console.WriteLine("First Line.");
            alert.SendMessage += (sender, arg) => Console.WriteLine("First Second.");
            alert.SendMessage += (sender, arg) => Console.WriteLine("First Third.");
            alert.SendMessage += (sender, arg) => Console.WriteLine("First Fourth.");
        }

        public void RaiseAlert()
        {
            alert?.RaiseAlert();
        }
    }
}
