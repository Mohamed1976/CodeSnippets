//#undef DEBUG

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Dynamic;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace _70_483.Exercises
{
    public class _70_483_ExamExercises
    {
        public _70_483_ExamExercises()
        {

        }

        /// <summary>
        /// Resizes an array.
        /// </summary>
        /// <param name="source">Source array of type T that needs to be resized.</param>
        /// <param name="newSize">The requested new array size.</param>
        /// <returns>void.</returns>
        private void ResizeArray<T>(ref T[] source, int newSize)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (source.Length > newSize)
                throw new ArgumentOutOfRangeException("Source array is larger than requested array, choose right size.");

            T[] buffer = new T[newSize];
            source.CopyTo(buffer, 0);
            source = buffer; //Array is passed by refrence so we can assign it a new array/memory address  
        }

        private void calculate(float price)
        {
            object boxedValue = price;
            int unboxedValue = (int)((float)boxedValue + 0.5);
            Console.WriteLine("Price: {0}", unboxedValue);
        }

        //TODO create working example, create account on imgur 
        //https://imgur.com/, upload images using WebClient.UploadValuesTaskAsync()
        //https://apidocs.imgur.com, and https://www.postman.com/ shows how api works
        private Task<Byte[]> UploadImage(MemoryStream image, string imageName = null)
        {
            const string url = @"https://api.imgur.com/3/upload.json";
            const string clientId = "68a0074c7783fd4";
            string Authorization = "Authorization: Client-ID " + clientId;
            Task<byte[]> result = null;

            var imgBase64 = "MG9S5ujZM46pWpe";//Convert.ToBase64String(image.GetBuffer());, Auhorization exception
            var nvc = new NameValueCollection();
            nvc.Add("image", imgBase64);
            if (!string.IsNullOrEmpty(imageName))
                nvc.Add("name", imgBase64);

            using (WebClient client = new WebClient())
            {
                client.Headers.Add(Authorization);
                result = client.UploadValuesTaskAsync(url, nvc);
            }

            return result;
        }

        public static Task<byte[]> SendMessage(string url, int intA, int intB)
        {
            var client = new WebClient();
            var nvc = new NameValueCollection() { { "a", intA.ToString() }, { "b", intB.ToString() } };
            return client.UploadValuesTaskAsync(new Uri(url), nvc);
        }

        private string GetImagePath()
        {
            string imagePath = default;
#if (DEBUG)
            imagePath = @"c:\TempFolder\Images\";
            Console.WriteLine("Entering debug mode");
#else
            imagePath = @"c:\DevFolder\Images\";
            Console.WriteLine("Entering release mode");
#endif
            return imagePath;
        }

        private void SendMessage(dynamic msg)
        {
            Console.WriteLine("{0}", msg.From);
            Console.WriteLine("{0}", msg.To);
            Console.WriteLine("{0}", msg.Content);
        }

        //https://stackoverflow.com/questions/3809401/what-is-a-good-regular-expression-to-match-a-url
        private List<string> IsWebsite(string url)
        {
            const string pattern = @"https://(www\.)?([^\.])+\.com";
            List<string> results = new List<string>();

            MatchCollection matches = Regex.Matches(url, pattern);

            foreach (Match match in matches)
                results.Add(match.Value);

            return results;
        }

        private int cents = 0;
        private int dollars = 0;

        public void Deposit(int dollars, int cents)
        {
            checked
            {
                int totalCents = cents + this.cents;
                int extraDollars = totalCents / 100;
                this.cents = totalCents - 100 * extraDollars;
                this.dollars += dollars + extraDollars;
            }
        }

        private void DisplayTemerature(DateTime date, double temp)
        {
            string output;
            output = string.Format("Temperature at {0:h:mm tt} on {0:MM/dd/yyyy} is {1:N2} degrees celsius.", date, temp);
            Console.WriteLine("{0}", output);
        }

        public async Task Run()
        {
            //-----------------------------------------------------------------------------------
            // Example using generic method
            //-----------------------------------------------------------------------------------
            Bird bird = new Bird("Donald Duck");
            Animal animal = new Animal("The Jackal");
            Save<Animal>(animal);
            Save<Bird>(bird);
            //Ant ant = new Ant();
            //Save<Ant>(ant); Compile error because Ant is not derived from Animal 

            //-----------------------------------------------------------------------------------
            // Example on how to retrieve data from database
            //-----------------------------------------------------------------------------------
            IEnumerable<Customer> customers = await Customer.GetCustomers(GetConnectionString());
            foreach (Customer customer in customers)
            {
                Console.WriteLine("{0}, {1}", customer.CompanyName, customer.FullName);
            }

            //-----------------------------------------------------------------------------------
            // Example of a Queue 
            //-----------------------------------------------------------------------------------
            Queue<string> numbers = new Queue<string>();
            numbers.Enqueue("one");
            numbers.Enqueue("two");
            numbers.Enqueue("three");
            numbers.Enqueue("four");
            numbers.Enqueue("five");

            // A queue can be enumerated without disturbing its contents.
            foreach (string number in numbers)
            {
                Console.WriteLine(number);
            }

            Console.WriteLine("\nDequeuing '{0}'", numbers.Dequeue());
            Console.WriteLine("Peek at next item to dequeue: {0}", numbers.Peek());
            Console.WriteLine("Dequeuing '{0}'", numbers.Dequeue());
            //We can make  a copy of the queue to array. 
            string[] numbers1 = numbers.ToArray();
            for (int index = 0; index < numbers1.Length; index++)
            {
                Console.WriteLine("[{0}]: {1}", index, numbers1[index]);
            }

            //-----------------------------------------------------------------------------------
            // Formatting DateTime/double values  
            //-----------------------------------------------------------------------------------
            double temp = 23.45;
            DateTime date = new DateTime(2013, 4, 21, 14, 0, 0);
            DisplayTemerature(date, temp);

            //-----------------------------------------------------------------------------------
            //The checked keyword is used to explicitly enable overflow checking for integral-type 
            //arithmetic operations and conversions. By default, an expression that contains only 
            //constant values causes a compiler error if the expression produces a value that is 
            //outside the range of the destination type. If the expression contains one or more 
            //non-constant values, the compiler does not detect the overflow.
            //https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/checked
            //-----------------------------------------------------------------------------------
            try
            {
                Deposit(1000, 1010);
                Console.WriteLine("Dollars: {0}, cents: {1}", dollars, cents);
                Deposit(int.MaxValue, 10);
            }
            catch (System.OverflowException e)
            {
                // The following line displays information about the error.
                Console.WriteLine("OverflowException CHECKED and CAUGHT:  " + e.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected exception: {0}", ex.Message);
            }

            try
            {
                // The following example causes compiler error CS0220 because 2147483647
                // is the maximum value for integers. 
                // int i1 = 2147483647 + 10;
                // The following example, which includes variable ten, does not cause
                // a compiler error.
                int ten = 10;
                int i2 = int.MaxValue + ten; //2147483647 + ten;

                // By default, the overflow in the previous statement also does
                // not cause a run-time exception. The following line displays 
                // -2,147,483,639 as the sum of 2,147,483,647 and 10.
                Console.WriteLine(i2);

                // The following line raises an exception because it is checked.
                int i3 = checked(int.MaxValue + ten);
                Console.WriteLine(i3);

                // Note we can also use a checked block.
                checked
                {
                    int i4 = checked(int.MaxValue + ten);
                    Console.WriteLine(i4);
                }
            }
            catch (System.OverflowException e)
            {
                // The following line displays information about the error.
                Console.WriteLine("OverflowException CHECKED and CAUGHT:  " + e.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected exception: {0}", ex.Message);
            }

            //-----------------------------------------------------------------------------------
            // Example of delegate
            //-----------------------------------------------------------------------------------
            Func<string, string> MathSqrt = (value) =>
            {
                int digit = Convert.ToInt32(value);
                double result = Math.Sqrt(digit);
                return result.ToString();
            };

            Console.WriteLine("{0}", MathSqrt("120"));

            //-----------------------------------------------------------------------------------
            // Using RegEx to validate and split data 
            //-----------------------------------------------------------------------------------
            List<string> results = IsWebsite("https://www.google.com/https://www.nu.com/https://www.microsoft.com/");
            foreach (string result in results)
                Console.WriteLine("{0}", result);

            //-----------------------------------------------------------------------------------
            // Using ExpandoObject and anonymous type 
            //-----------------------------------------------------------------------------------
            dynamic obj = new ExpandoObject();
            obj.From = "John Morris";
            obj.To = "Jane Smith";
            obj.Content = "Message to you.";
            SendMessage(obj);
            //anonymous type
            var obj1 = new { From = "John Morris", To = "Jane Smith", Content = "Message to you." };
            SendMessage(obj1);
            dynamic obj2 = new { From = "John Morris", To = "Jane Smith", Content = "Message to you." };
            SendMessage(obj2);

            //-----------------------------------------------------------------------------------
            // Casting from object to int
            //-----------------------------------------------------------------------------------
            ArrayList arrayList1 = new ArrayList();
            int var1 = 10, var2;
            arrayList1.Add(var1);
            var2 = Convert.ToInt32(arrayList1[0]);
            Console.WriteLine("{0}", var2);

            //-----------------------------------------------------------------------------------
            // Debug/Trace logging.
            //-----------------------------------------------------------------------------------
            Debug.WriteLine("Trace data...");
            Trace.WriteLine("Trace data...");

            //-----------------------------------------------------------------------------------
            // The debug and release versions of the application will display different logo images.
            //-----------------------------------------------------------------------------------
            Console.WriteLine("GetImagePath(): {0}", GetImagePath());

            //-----------------------------------------------------------------------------------
            // You can also upload data to website using WebClient
            //-----------------------------------------------------------------------------------
            //byte[] byteArray = Encoding.UTF8.GetBytes("MG9S5ujZM46pWpe");
            //MemoryStream stream = new MemoryStream(byteArray); //Simulated Image
            //try
            //{
            //    byte[] response = await UploadImage(stream, "MyImage");
            //    var json = Encoding.UTF8.GetString(response);
            //    Console.WriteLine("Web Response: {0}", json);
            //}
            //catch(Exception ex)
            //{
            //    Console.WriteLine($"There was an exception calling UploadImage: {ex.ToString()}");
            //}

            try
            {
                byte[] response = await SendMessage("http://microsoft.com", 2, 3);
                Console.WriteLine("Web Response: {0}", Encoding.UTF8.GetString(response));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was an exception calling SendMessage: {ex.ToString()}");
            }

            //Simple example of NameValueCollection  
            int intA = 10;
            int intB = 29;
            var nvc = new NameValueCollection()
            {
                { "a",  intA.ToString() },
                { "b",  intB.ToString() },
            };
            nvc["c"] = "2020"; //You can also use indexing
            nvc.Add("d", "2050");

            Console.WriteLine("{0}", nvc.Get("a"));
            Console.WriteLine("{0}", nvc.Get("b"));
            Console.WriteLine("{0}", nvc.Get("c"));

            //-----------------------------------------------------------------------------------
            // Example shows how to box and unbox a value.
            //-----------------------------------------------------------------------------------
            calculate((float)5.9);

            //-----------------------------------------------------------------------------------
            // Could you use a List to store every type of data in my program?
            // Yes if you define the list as a collection of objects similar to ArrayList
            // All types in C# derive from object, hence all types can be stored in an object list.
            //-----------------------------------------------------------------------------------
            List<object> list = new List<object>();
            list.Add("The brown fox");
            list.Add(2020);
            list.Add(new ArrayList() { "Dog", "Wolf" });
            list.Add(new int[] { 2000, 2020, 2030, 2050 });
            foreach (object item in list)
            {
                Console.WriteLine("{0}", item.GetType());
                if (item.GetType() == typeof(string))
                {
                    string str = (string)item;
                    Console.WriteLine("String: {0}", str);
                }

                if (item.GetType() == typeof(int))
                {
                    int number = (int)item;
                    Console.WriteLine("Int: {0}", number);
                }

                if (item.GetType() == typeof(ArrayList))
                {
                    ArrayList arrayList = (ArrayList)item;
                    Console.WriteLine("ArrayList: {0}, {1}", (string)arrayList[0], (string)arrayList[1]);
                }

                if (item.GetType() == typeof(int[]))
                {
                    int[] arr = (int[])item;
                    Console.WriteLine("int[]: {0}, {1}, {2}, {3}", arr[0], arr[1], arr[2], arr[3]);
                }
            }

            //-----------------------------------------------------------------------------------
            // How does a Dictionary actually work?
            // It probably uses a list or array of keyvaluepair in an indexed class, as illustrated in the code below.
            //-----------------------------------------------------------------------------------
            SimpleDictionary<int, string> simpleDictionary = new SimpleDictionary<int, string>();
            simpleDictionary.Add(1, "Bird");
            simpleDictionary.Add(2, "Cat");
            simpleDictionary.Add(3, "Dog");
            simpleDictionary.Add(4, "Cheetah");
            simpleDictionary.Print();
            string fromDictionary = simpleDictionary[3];
            simpleDictionary[3] = "Tasmanian devil";
            simpleDictionary.Print();
            Console.WriteLine("Get from dictionary: {0}", fromDictionary);
            simpleDictionary.Add(5, "Giraffe");
            simpleDictionary.Print();
            simpleDictionary.Remove(2);
            simpleDictionary.Print();

            //-----------------------------------------------------------------------------------
            // The array is one element too small. How do you add a new element on the end of the array?
            // 
            //-----------------------------------------------------------------------------------
            int[] intArray = { 1, 2, 3 }; //The same as int[] intArray = new int [] { 1, 2, 3 };
            Console.WriteLine("Length: {0}, {1}, {2}, {3}",
                intArray.Length, intArray[0], intArray[1], intArray[2]);
            ResizeArray<int>(ref intArray, intArray.Length + 1);
            Console.WriteLine("Length: {0}, {1}, {2}, {3}, {4}",
                intArray.Length, intArray[0], intArray[1], intArray[2], intArray[3]);

            //-----------------------------------------------------------------------------------
            // An example of a two dimensional array is shown below (4 rows x 5 columns).
            // 
            //-----------------------------------------------------------------------------------
            int[,] matrix = new int[,] //Note declaration could also be [4,5] 
            {
                { 0, 45, 4, 12, 16 },
                { 7, 34, 7, 11, 19 },
                { 54, 87, 32, 68, 123 },
                { 45, 32, 19, 23, 45 }
            };

            int rows = matrix.GetLength(0);
            int columns = matrix.GetLength(1);

            for (int rowCount = 0; rowCount < rows; rowCount++)
            {
                for (int columnCount = 0; columnCount < columns; columnCount++)
                {
                    Console.Write("{0}{1}", matrix[rowCount, columnCount],
                        columnCount == (columns - 1) ? "\n" : " ");
                }
            }

            //-----------------------------------------------------------------------------------
            // Can you create an array of arrays?
            // Yes, arrays of arrays are called jagged arrays.
            //-----------------------------------------------------------------------------------
            string[][] wordList = new string[][]
            {
                new string[] { "Dog", "Cat", "Bird"},
                new string[] { "House", "Street" },
                new string[] { "skyscraper" }
            };

            for (int index = 0; index < wordList.Length; index++)
            {
                for (int position = 0; position < wordList[index].Length; position++)
                {
                    Console.Write("{0}{1}", wordList[index][position],
                        position == (wordList[index].Length - 1) ? "\n" : " ");
                }
            }
        }

        private string GetConnectionString()
        {
            //This solution has a setting file appsettings.Development.json that contains custom settings for use
            //during development.If you add an appsettings.Production.json file
            //to the solution, you can create settings information that will be used when the
            //program is running on a production server.
            //The setting information in a solution can contain the descriptions of
            //environments that are to be used for development and production deployments of
            //an application. The environments used for application deployment set out the
            //database connection string and any other options that you would like to
            //customize.For example, the development environment can use a local database
            //server and the production environment can use a distant server.
            //The setting information to be used is when a server is started and determined
            //by an environment variable on the computer that is tested when the program starts running.

            //In older ASP.NET applications the SQL settings are held in the web.config
            //file, which is part of the solution. Developers then use XML transformations to
            //override settings in the file to allow different SQL servers to be selected.

            string settingsFile = $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"}.json";
            Debug.WriteLine(settingsFile);

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(settingsFile, optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            string connectionString = configuration.GetConnectionString("AdventureworksContext");
            Debug.WriteLine(connectionString);
            return connectionString;
        }

        public static void Save<T>(T animal) where T : Animal, new()
        {
            Console.WriteLine("Type: {0}, Name: {1}", animal.GetType(), animal.Name);
        }
    }

    public class Customer
    {
        public Customer(string companyName, string fullName)
        {
            CompanyName = companyName;
            FullName = fullName;
        }

        public string CompanyName { get; private set; }
        public string FullName { get; private set; } // FirstName + " " + LastName

        const string sqlSelectCustomers = "SELECT* FROM[AdventureWorksLT2017].[SalesLT].[Customer]";

        public static async Task<IEnumerable<Customer>> GetCustomers(string sqlConnectionString)
        {
            List<Customer> customers = new List<Customer>();
            using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(sqlSelectCustomers, sqlConnection);
                sqlConnection.Open();
                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                while (sqlDataReader.Read())
                {
                    string CompanyName = sqlDataReader["CompanyName"].ToString();
                    string FullName = sqlDataReader["FirstName"].ToString() + " " +
                        sqlDataReader["LastName"].ToString();

                    customers.Add(new Customer(companyName: CompanyName, fullName: FullName));
                }
            }

            return customers;
        }
    }

    public class Animal
    {
        public Animal()
        {
        }

        public Animal(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public virtual void Speak()
        {
            Console.WriteLine("I am an animal, my name is: {0}.", Name);
        }
    }

    public class Bird : Animal
    {
        public Bird()
        {

        }

        public Bird(string name) : base(name) 
        {
        }

        public override void Speak()
        {
            Console.WriteLine("I am an bird, my name is: {0}.", Name);
        }
    }

    public class Ant
    {
        public Ant()
        {
        }

        public void Speak()
        {
            Console.WriteLine("I am an ant, always at work.");
        }
    }
}