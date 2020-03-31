//#undef DEBUG
//#define LOG_DATA

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
using System.Globalization;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

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

        private static decimal CalculateInterest(decimal loanAmount,int loanTerm, decimal loanRate)
        {
            //You need to ensure that the debugger breaks execution within the CalculateInterest()
            //method when the loanAmount variable is less than or equal to zero in all builds of the application.
            Trace.Assert(loanAmount > 0, "Loan amount in CalculateInterest() needs to be larger then 0.");
            //Debug.Assert(loanAmount > 0, "Loan amount in CalculateInterest() needs to be larger then 0.");
            decimal interestAmount = loanAmount * loanRate * loanTerm;
//#if LOG_DATA 
            //Note you could also decorate the LogLine() method with a Conditional attribute [Conditional("Debug")],
            //the code is then only executed when DEBUG is defined, note that the method code is included in the release version of the application.
            LogLine("Interest amount : ", interestAmount.ToString("c")); //Format as currency
//#endif
            return interestAmount;
        }

        [Conditional("LOG_DATA")]
        public static void LogLine(string msg, string details)
        {
            Console.WriteLine("Log: {0} - {1}", msg, details);
        }

        public async Task GetData(WebResponse response)
        {
            using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
            {
                string content = await streamReader.ReadToEndAsync();
                Console.WriteLine(content);
            }            
        }

        private int scoreValue = 0;
        private object lockObject = new object();

        public void UpdateScore(int score)
        {
            lock(lockObject)
            {
                scoreValue = score;
            }
        }

        public static bool GetValidInteger(ref int value)
        {
            int number;
            bool success = false;
            Console.Write("Please enter a integer and press enter: >");
            string input = Console.ReadLine();
            Console.WriteLine("Read: {0}", input);

            if(int.TryParse(input, out number))
            {
                value = number;
                success = true;
            }

            return success;
        }

        //[Flags]
        [System.FlagsAttribute]
        public enum Group
        {
            Users = 1,
            Supervisors = 2,
            Managers = 4,
            Administrators = 8
        }

        private void DoWork(object obj)
        {
            //Description Explanation: As - The as operator is like a cast operation. However, 
            //if the conversion isn't possible, as returns null instead of raising an exception. 
            //http://msdn.microsoft.com/en-us/library/cscsdfbt(v=vs.110).aspx
            IPerson person = obj as IPerson;

            if(person != null)
            {
                Console.WriteLine("FirstName: {0}, LastName: {1}",
                    person.FirstName, person.LastName);
            }
        }

        public static Name ConvertToName(string json)
        {
            Name name = JsonConvert.DeserializeObject<Name>(json);
            return name;
        }


        public bool DictionarySerialize<TKey, TValue>(Dictionary<TKey, TValue> dictionary, out string json)
        {
            bool success = false;
            string _json = default;

            try
            {
                _json = JsonConvert.SerializeObject(dictionary);                
                success = true;
            }
            catch(Exception Ex)
            {
                Trace.WriteLine("An error occured in DictionarySerialize: {0}", Ex.Message);
            }

            json = success ? _json : default(string);
            return success;
        }


        public bool DictionaryTryParse<TKey, TValue>(string json, out Dictionary<TKey, TValue> dictionary)
        {
            bool success = false;
            Dictionary<TKey, TValue> _dictionary = default;

            try
            {
                _dictionary = JsonConvert.DeserializeObject<Dictionary<TKey, TValue>>(json);
                success = true;
            }
            catch (Exception ex)
            {
                Trace.WriteLine("An error occured in DictionaryTryParse: {0}", ex.Message);
            }

            dictionary = success ? _dictionary : default(Dictionary<TKey, TValue>);
            return success;
        }

        private int[] FilterArray(int[] customerIds, int customerIdToRemove)
        {
            int[] filteredCustomerIds = customerIds.Where(value => value != customerIdToRemove).Distinct()
                .OrderByDescending(x => x).Select(x => x).ToArray();

            //You can also remove the Select clause  
            //int[] filteredCustomerIds = customerIds.Where(value => value != customerIdToRemove).Distinct()
            //    .OrderByDescending(x => x).ToArray();

            return filteredCustomerIds;
        }

        public string GetResponse(char letter)
        {
            string response;

            switch(letter)
            {
                case 'a':
                    response = "animal";
                    break;
                case 'm':
                    response = "mineral";
                    break;
                default :
                    response = "invalid choice";
                    break;
            }

            return response;
        }

        private bool IsNull(object obj)
        {
            if (obj == null)
                return true;
            else
                return false;

        }

        public void FetchData()
        {
            try
            {
                //Will be caught by catch(Exception ex) handler 
                //throw new Exception("Exception in FetchData.");

                //Will be caught by catch (AdventureWorksException ex) handler
                //throw new AdventureWorksException("Exception in FetchData.");

                //Will also be caught by catch (AdventureWorksException ex) handler
                //throw new AdventureWorksDbException("Exception in FetchData.");

                //Will also be caught by catch (AdventureWorksValidationException ex) handler
                throw new AdventureWorksValidationException("Exception in FetchData.");
            }
            catch (AdventureWorksValidationException ex)
            {
                Logger.Log(ex);
            }
            //Note AdventureWorksDbException will be caught by this handler
            //because AdventureWorksDbException is derived from AdventureWorksException;  
            catch (AdventureWorksException ex)
            {
                Logger.Log(ex);
            }
            catch(Exception ex)
            {
                Logger.Log(ex);
            }
            finally
            {
                Console.WriteLine("Always executed.");
            }
        }

        //The EmployeeType property value must meet the following requirements:
        //✑ The value must be accessed only by code within the Employee class or within a
        //class derived from the Employee class.
        //✑ The value must be modified only by code within the Employee class.
        //You need to ensure that the implementation of the EmployeeType property meets the requirements
        protected string EmployeeType
        {
            get;
            private set;
        }

        private Task<string> GetFirstLine(WebResponse response)
        {
            StreamReader streamReader = new StreamReader(response.GetResponseStream());
            Task<string> firstLine = streamReader.ReadLineAsync();
            return firstLine;
        }

        private List<string> departments = new List<string>()
        {
            "Accounting", "Marketing", "Sales", "Manufacturing", "Information", "System", "Training"
        };

        public bool GetMatches(List<string> list, string searchTerm)
        {
            bool exists = list.Exists(delegate (string listItem)
            {
                return listItem.Equals(searchTerm);
            });

            return exists;
        }

        //We can also use Lambda expression
        public bool GetMatchesV1(List<string> list, string searchTerm)
        {
            bool exists = list.Exists((listItem) => listItem.Equals(searchTerm));
            return exists;
        }

        private void Delay(int delay)
        {
            Thread.Sleep(delay*1000);
        }

        private void LongRunningTask(string msg, int delay)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Delay(delay);
            stopwatch.Stop();

            if(stopwatch.Elapsed.Seconds > 2)
            {
                string exception = string.Format("Execution took too long: {0}, {1} (ms)",
                    msg,
                    stopwatch.Elapsed.TotalMilliseconds);
                throw new Exception(exception);
            }
        }

        public int Add(int x, int y)
        {
            return x + y;
        }

        public int Add(int x, int y, int z)
        {
            int result = default;

            try
            {
                result = x + y + z;
            }
            //Most specific Exceptions first
            catch (DivideByZeroException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch(ArithmeticException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch(ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine("Finally always  executed");
            }
            return result;
        }

        //WebRequest.Create Method(Uri) Initializes a new WebRequest instance for the specified 
        //URI scheme. * Example: 1. To request data from a host server Create a WebRequest instance 
        //by calling Create with the URI of the resource. C# WebRequest request = 
        //WebRequest.Create("http://www.contoso.com/"); 2. Set any property values that you need 
        //in the WebRequest. For example, to enable authentication, set the Credentials property 
        //to an instance of the NetworkCredential class. C# request.Credentials = 
        //CredentialCache.DefaultCredentials; 3. To send the request to the server, call GetResponse. 
        //The actual type of the returned WebResponse object is determined by the scheme of the 
        //requested URI. C# WebResponse response = request.GetResponse(); 4. To get the stream 
        //containing response data sent by the server, use the GetResponseStream method of the 
        //WebResponse. C# Stream dataStream = response.GetResponseStream ();
        public void ProcessFile(Uri uri, string filePath)
        {
            WebRequest webRequest = WebRequest.Create(uri);
            //webRequest.Credentials = CredentialCache.DefaultCredentials;
            using (WebResponse webResponse = webRequest.GetResponse())
            {
                using (StreamReader streamReader = new StreamReader(webResponse.GetResponseStream()))
                {
                    using (StreamWriter streamWriter = new StreamWriter(filePath))
                    {
                        streamWriter.Write(streamReader.ReadToEnd());
                    }
                }
            }
        }

        int[] arrayContent;
        private int[] IntPaging(int page)
        {
            const int pageSize = 10;

            if (page == 0)
                arrayContent = Enumerable.Range(0, 100).ToArray();

            int[] result = arrayContent.Skip(pageSize * page).Take(pageSize).ToArray();

            return result;
        }

        public async Task ProcessWrite()
        {
            string buff = "The fox jumped over the lazy dog.";
            string path = @".\temp0304.txt";
            await WriteTextAsync(path, buff);
        }

        //Description Explanation: await sourceStream.WriteAsync(encodedText, 0, encodedText.Length); 
        //The following example has the statement await sourceStream.WriteAsync(encodedText, 0, 
        //encodedText.Length);, which is a contraction of the following two statements: 
        //Task theTask = sourceStream.WriteAsync(encodedText, 0, encodedText.Length); await theTask; 
        //Example: The following example writes text to a file.At each await statement, the method 
        //immediately exits. When the file I/O is complete, the method resumes at the statement that 
        //follows the await statement.Note that the async modifier is in the definition of methods 
        //that use the await statement. public async void ProcessWrite() { string filePath = @"temp2.txt"; 
        //string text = "Hello World\r\n"; await WriteTextAsync(filePath, text); }
        //private async Task WriteTextAsync(string filePath, string text) { byte[] 
        //encodedText = Encoding.Unicode.GetBytes(text); using (FileStream sourceStream = 
        //new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.None, 
        //bufferSize: 4096, useAsync: true)) { await sourceStream.WriteAsync(encodedText, 0, encodedText.Length); }; }
        //Reference: Using Async for File Access(C# and Visual Basic) 
        //https://msdn.microsoft.com/en-us/library/jj155757.aspx
        public async Task WriteTextAsync(string path, string buff)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(buff);
            using (FileStream fileStream = new FileStream(path, FileMode.Create, 
                FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync:true))
            {
                await fileStream.WriteAsync(buffer, 0, buffer.Length);
            }
        }

        public async Task Run()
        {
            //-----------------------------------------------------------------------------------
            // Example of explicit interface 
            //-----------------------------------------------------------------------------------
            class2 _class2 = new class2();
            //_class2.Method1();
            //Method1 is not visible in _class2 instance because of explicit interface implementation   
            INewInterface newInterface = new class1();
            newInterface.Method1();

            List<string> fruits = new List<string>()
            { 
                "apple", "passionfruit", "banana", "mango", "orange", "blueberry", "grape", "strawberry" 
            }; 
            
            IEnumerable<string> query = fruits.Where(fruit => fruit.Length < 6); 
            
            foreach (string fruit in query)
                Console.WriteLine(fruit);
            
            //-----------------------------------------------------------------------------------
            // Simple Linq example
            //You need to retrieve all of the numbers from the items variable that are greater than 80.
            //-----------------------------------------------------------------------------------
            List<int> list3 = new List<int>()
            {
                100, 95, 80, 75, 95 
            };

            var selectedValues = from value in list3 where value > 80 orderby value ascending select value;
            foreach (int value in selectedValues)
                Console.WriteLine(value);

            //-----------------------------------------------------------------------------------
            // Asyn write method, FileStream
            //-----------------------------------------------------------------------------------
            await ProcessWrite();
            
            //-----------------------------------------------------------------------------------
            // Paging example
            //-----------------------------------------------------------------------------------
            foreach (int val in IntPaging(0))
                Console.WriteLine(val);

            foreach (int val in IntPaging(1))
                Console.WriteLine(val);
            
            //-----------------------------------------------------------------------------------
            // Save webpage to file.
            //You are implementing a method named ProcessFile that retrieves data files from web
            //servers and FTP servers.The ProcessFile() method has the following method signature:
            //Public void ProcessFile(Guid dataFileld, string dataFileUri)
            //Each time the ProcessFile() method is called, it must retrieve a unique data file and then
            //save the data file to disk.
            //-----------------------------------------------------------------------------------
            ProcessFile(new Uri(@"https://www.microsoft.com/nl-nl/"), @".\download.txt");

            //-----------------------------------------------------------------------------------
            // Example of method overloading
            //Member overloading means creating two or more members on the same type that differ 
            //only in the number or type of parameters but have the same name. Overloading is one 
            //of the most important techniques for improving usability, productivity, and readability 
            //of reusable libraries. Overloading on the number of parameters makes it possible to 
            //provide simpler versions of constructors and methods. Overloading on the parameter 
            //type makes it possible to use the same member name for members performing identical 
            //operations on a selected set of different types.
            //-----------------------------------------------------------------------------------
            Console.WriteLine("{0}",Add(4, 7));
            Console.WriteLine("{0}", Add(4, 7, 9));

            //-----------------------------------------------------------------------------------
            // Using class indexers
            //-----------------------------------------------------------------------------------
            Scorecard scorecard = new Scorecard();
            scorecard.Add("Player1", 10);
            scorecard.Add("Player2", 20);
            int expectedScore = 20;
            int actualScore = scorecard["Player2"];
            Trace.Assert(actualScore == expectedScore);
            Console.WriteLine("actualScore == expectedScore: {0}", actualScore == expectedScore);

            //-----------------------------------------------------------------------------------
            // Using reflection to call Method
            //-----------------------------------------------------------------------------------
            Calculator calculator = new Calculator();
            int result2 = calculator.DoOperation("AddNumber", 5, 7);
            Console.WriteLine("Calculator result: {0}", result2);

            //-----------------------------------------------------------------------------------
            // using XmlWriterTraceListner
            // XmlWriterTraceListener Directs tracing or debugging output as XML-encoded data to a 
            // TextWriter or to a Stream, such as a FileStream.
            //-----------------------------------------------------------------------------------
            try
            {
                LongRunningTask("Using XmlWriterTraceListener.", 3);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                XmlWriterTraceListener xmlWriterTraceListener = new XmlWriterTraceListener(@".\ErrorLog.txt");
                xmlWriterTraceListener.WriteLine(ex.Message);
                xmlWriterTraceListener.Flush();
                xmlWriterTraceListener.Close();
            }

            //-----------------------------------------------------------------------------------
            // Implementing the IComparable/IComparable<T> interface
            // You are developing a class named Temperature.
            // You need to ensure that collections of Temperature objects are sortable.
            //-----------------------------------------------------------------------------------
            List<Temperature> temperatures = new List<Temperature>()
            {
                new Temperature(75.89),
                new Temperature(35.89),
                new Temperature(5.89),
                new Temperature(13.89),
            };

            foreach(Temperature temperature in temperatures)
            {
                Console.WriteLine("Unsorted Temperature: {0}", temperature.Fahrenheit);
            }

            temperatures.Sort();

            foreach (Temperature temperature in temperatures)
            {
                Console.WriteLine("Sorted Temperature: {0}", temperature.Fahrenheit);
            }
            
            //-----------------------------------------------------------------------------------
            // Using Linq queries
            //-----------------------------------------------------------------------------------
            Console.WriteLine("\"Manufacturing\" Exists: {0}", GetMatches(departments, "Manufacturing"));
            Console.WriteLine("\"Manufacturing\" Exists: {0}", GetMatchesV1(departments, "Manufacturing"));
            Console.WriteLine("\"Manufacturinge\" Exists: {0}", GetMatches(departments, "Manufacturinge"));
            Console.WriteLine("\"Manufacturinge\" Exists: {0}", GetMatchesV1(departments, "Manufacturinge"));
            
            //-----------------------------------------------------------------------------------
            // Using StreamReader derived from abstract class TextReader 
            //-----------------------------------------------------------------------------------
            LogFileViewer logFileViewer = new LogFileViewer(@"C:\Users\moham\Desktop\Notes\Notes001.txt");
            logFileViewer.View();

            //-----------------------------------------------------------------------------------
            // Example of casting
            //-----------------------------------------------------------------------------------
            ArrayList arrayList2 = new ArrayList();
            arrayList2.Add(2020);
            int val4 = Convert.ToInt32(arrayList2[0]);
            Console.WriteLine("Convert.ToInt32(arrayList2[0]): " + val4);
            float val5 = 34.89f;
            ArrayList arrayList3 = new ArrayList();
            arrayList3.Add(val5);
            Console.WriteLine("(int)(float)arrayList3[0]: " + (int)(float)arrayList3[0]);
            
            //-----------------------------------------------------------------------------------
            // Example on how to implement IEquatable 
            //-----------------------------------------------------------------------------------
            Employee employee = new Employee(12, "John", new DateTime(1980, 7, 4));
            Employee employee1 = new Employee(12, "John", new DateTime(2000, 3, 9));
            if(employee.Equals(employee1))
            {
                Console.WriteLine("Employees are equal.");
            }
            
            //-----------------------------------------------------------------------------------
            // Use WebRequest (Abstract class) to get WebResponse and pass it to parser method
            //-----------------------------------------------------------------------------------
            WebRequest webRequest1 = WebRequest.Create(new Uri("https://www.microsoft.com/nl-nl/"));
            WebResponse webResponse1 = webRequest1.GetResponse();
            string firstLine = await GetFirstLine(webResponse1);
            Console.WriteLine(firstLine);
            
            //Trigger Trace.Assert()
            //CalculateInterest(0m, 4, 0.02m);

            //-----------------------------------------------------------------------------------
            // Example of custom exceptions and exception handling.  
            // You are developing an application that implements a set of custom exception types.
            //The application includes a function named DoWork that throws.NET Framework
            //exceptions and custom exceptions. 
            //The application must meet the following requirements:
            //✑ When AdventureWorksValidationException exceptions are caught, log the
            //information by using the static void Log(AdventureWorksValidationException ex) method.
            //✑ When AdventureWorksDbException or other AdventureWorksException
            //exceptions are caught, log the information by using the static void I oq(
            //AdventureWorksException ex) method.
            // Note: Derived classes can be casted to base class, therefore a base class instance handler 
            // can also handle derived class instances, as illustrated by this example.  
            //-----------------------------------------------------------------------------------
            FetchData();
            
            //-----------------------------------------------------------------------------------
            // Example You need to evaluate whether an object is null.
            //-----------------------------------------------------------------------------------
            object obj3 = null;
            Console.WriteLine(IsNull(obj3));
            
            //-----------------------------------------------------------------------------------
            // Example of using event EventHandler and delegates, lambda expression
            //-----------------------------------------------------------------------------------
            Users users = new Users();
            //Using lambda expression
            users.OnUserAdded += (sender, args) =>
            {
                Console.WriteLine("Lambda User Added, FirstName: {0}, LastName: {1}, TimeCreated: {2}", 
                    args.User.FirstName,
                    args.User.LastName,
                    args.TimeCreated);
            };

            users.OnUserAdded += delegate (object sender, UserAddedEventArgs args)
            {
                Console.WriteLine("Delegate User Added, FirstName: {0}, LastName: {1}, TimeCreated: {2}",
                    args.User.FirstName,
                    args.User.LastName,
                    args.TimeCreated);
            };

            users.AddUser(new User("John", "Deer"));
            
            //-----------------------------------------------------------------------------------
            // Example of Extension method
            //-----------------------------------------------------------------------------------
            Console.WriteLine("IsEmail: {0}", "Test message".IsEmail());
                        
            //-----------------------------------------------------------------------------------
            // Example of StringBuilder
            // Description Explanation: A String object concatenation operation always creates a 
            // new object from the existing string and the new data.A StringBuilder object maintains 
            // a buffer to accommodate the concatenation of new data.New data is appended to the 
            // buffer if room is available; otherwise, a new, larger buffer is allocated, data 
            // from the original buffer is copied to the new buffer, and the new data is then 
            // appended to the new buffer.The performance of a concatenation operation for a 
            // String or StringBuilder object depends on the frequency of memory allocations.
            // A String concatenation operation always allocates memory, whereas a StringBuilder 
            // concatenation operation allocates memory only if the StringBuilder object buffer 
            // is too small to accommodate the new data.Use the String class if you are concatenating 
            // a fixed number of String objects.In that case, the compiler may even combine individual 
            // concatenation operations into a single operation.Use a StringBuilder object if you are 
            // concatenating an arbitrary number of strings; for example, if you're using a loop to 
            // concatenate a random number of strings of user input. 
            // http://msdn.microsoft.com/en-us/library/system.text.stringbuilder(v=vs.110).aspx
            //-----------------------------------------------------------------------------------
            IEnumerable<string> animals2 = new string[]
            {
                "Bird", "Dog", "Cat", "Fox", "Wolf", "Lyon", "Leopard", "Giraffe", "Bull"
            };
            
            TabDelimitedFormatter tabDelimitedFormatter = new TabDelimitedFormatter();
            string output = tabDelimitedFormatter.GetOutput(animals2.GetEnumerator(), 0);
            Console.WriteLine(output);

            string str2 = "Line1 ";
            str2 += "Line2 "; //you can concatenate strings 
            str2 = str2 + "Line3 ";
            Console.WriteLine($"string += operator: {str2}");

            string str1;
            str1 = string.Concat("Line1 ", "Line2 ");
            str1 = string.Concat(str1, "Line3 ");
            Console.WriteLine($"string.Concat() operator: {str1}");

            //-----------------------------------------------------------------------------------
            // Example of Lock
            //-----------------------------------------------------------------------------------
            Inventory inventory = WareHouse.Inventory;
            inventory.ProductName = "Goat cheese";
            Console.WriteLine(inventory.ProductName);
            Inventory inventory1 = WareHouse.Inventory; //Returns the same instance as above
            Console.WriteLine(inventory1.ProductName);
            
            //-----------------------------------------------------------------------------------
            // Example of delegate and event 
            //-----------------------------------------------------------------------------------
            Lease lease = new Lease(4);
            lease.OnMaxTermExceeded += (sender, args) =>
            {
                Console.WriteLine("Callback: {0}", args.Message);
            };
            lease.Term = 6;
            
            //-----------------------------------------------------------------------------------
            // Example of switch statement
            //-----------------------------------------------------------------------------------
            Console.WriteLine(GetResponse('m'));

            //-----------------------------------------------------------------------------------
            // Example of delegate callback
            //-----------------------------------------------------------------------------------
            BookTracker bookTracker = new BookTracker();
            bookTracker.AddBook("The magicians", delegate (int value)
            {
                Console.WriteLine("Total books in collection: {0}", value);
            });

            bookTracker.AddBook("The dream birds", (value) =>
            {
                Console.WriteLine("Total books in collection: {0}", value);
            });

            bookTracker.Print();

            //You use the Task.Run() method to launch a long-running data processing operation.The
            //data processing operation often fails in times of heavy network congestion.
            //If the data processing operation fails, a second operation must clean up any results of the
            //first operation. You need to ensure that the second operation is invoked only if the data processing
            //operation throws an unhandled exception. What should you do?
            //Description Explanation: Task.ContinueWith - Creates a continuation that executes 
            //asynchronously when the target Task completes.The returned Task will not be scheduled 
            //for execution until the current task has completed, whether it completes due to running 
            //to completion successfully, faulting due to an unhandled exception, or exiting out early 
            //due to being canceled. http://msdn.microsoft.com/en-us/library/dd270696.aspx

            //Commented out because you need to press continue in compiler to continue execution
            //Task longTask = Task.Run(() =>
            //{
            //    for(int counter = 0; counter < 1000; counter++)
            //    {
            //        Thread.Sleep(250);
            //        if (counter == 3)
            //            throw new ArgumentOutOfRangeException();
            //    }
            //});

            //await longTask.ContinueWith((Task) =>
            //{
            //    Console.WriteLine("An exception has occured in the task: {0}", Task.Exception.Message);
            //}, TaskContinuationOptions.OnlyOnFaulted);

            //-----------------------------------------------------------------------------------
            //You are developing an application by using C#. The application includes an array of
            //decimal values named loanAmounts. You are developing a LINQ query to return the values from the array.
            //The query must return decimal values that are evenly divisible by two.The values must be
            //sorted from the lowest value to the highest value.
            //You need to ensure that the query correctly returns the decimal values.
            //-----------------------------------------------------------------------------------
            decimal[] loanAmounts = new decimal[]
            {
                303m, 1000m, 520m, 200m, 501.51m, 603m, 1200m, 400m, 22m 
            };

            decimal[] loanAmountsResult = (from loan in loanAmounts
                                           where loan % 2 == 0
                                           orderby loan ascending
                                           select loan).ToArray();
            foreach(decimal loan in loanAmountsResult)
            {
                Console.WriteLine("{0:N0}", loan);
            }

            //-----------------------------------------------------------------------------------
            //You are developing an application.The application calls a method that returns an array of
            //integers named customerIds.You define an integer variable named customerIdToRemove
            //and assign a value to it.You declare an array named filteredCustomerIds.
            //You have the following requirements.
            //✑ Remove duplicate integers from the customerIds array.
            //✑ Sort the array in order from the highest value to the lowest value.
            //✑ Remove the integer value stored in the customerIdToRemove variable from the
            //customerIds array. You need to create a LINQ query to meet the requirements.
            //-----------------------------------------------------------------------------------
            int[] customerIds = new int[]
            {
                12,13,12,15,15,15,16,1,5,6,2,9,2020
            };

            int[] filteredCustomerIds = FilterArray(customerIds, 15);
            foreach(int id in filteredCustomerIds)
            {
                Console.WriteLine(id);
            }
            
            //-----------------------------------------------------------------------------------
            // Example of serializing/deserializing dictionary and error handling   
            //-----------------------------------------------------------------------------------
            Dictionary<string, int> animals = new Dictionary<string, int>()
            {
                { "Duck",5 },
                { "Fox", 7 }
            };
            animals.Add("Bird", 3);
            string jsonCollection;

            if(DictionarySerialize<string, int>(animals, out jsonCollection))
            {
                Console.WriteLine("{0}", jsonCollection);
            }

            //Deserializing dictionary from json string 
            Dictionary<string, int> animalsFromJson = null;
            if (DictionaryTryParse<string, int>(jsonCollection, out animalsFromJson))
            {
                foreach(KeyValuePair<string,int> pair in animalsFromJson)
                {
                    Console.WriteLine("{0}, {1}", pair.Value, pair.Key);
                }
            }

            //-----------------------------------------------------------------------------------
            //You are developing an application by using C#. The application will output the text string
            //"First Line" followed by the text string "Second Line".
            //You need to ensure that an empty line separates the text strings.
            //Which four code segments should you use in sequence? (To answer, move the appropriate
            //code segments to the answer area and arrange them in the correct order.)
            //-----------------------------------------------------------------------------------
            StringBuilder stringBuilder = new StringBuilder("First Line");
            stringBuilder.AppendLine();
            stringBuilder.Append("Second Line");
            Console.WriteLine(stringBuilder.ToString());

            //-----------------------------------------------------------------------------------
            // Example of implementing IEnumerable interface
            //-----------------------------------------------------------------------------------
            Loan[] loans = new Loan[]
            {
                new Loan(100,1,0.05m),
                new Loan(500,2,0.04m),
                new Loan(1000,3,0.03m),
                new Loan(1500,4,0.025m)
            };

            LoanCollection loanCollection = new LoanCollection(loans);
            foreach(Loan loan in loanCollection)
            {
                Console.WriteLine("{0}", loan);
            }

            //Note because LoanCollection stores references to the Loan objects in the loans array.
            //We can mutate Loan amount and the change will be reflected in the collection.
            loans[0].UpdateLoanAmount(2020);
            foreach (Loan loan in loanCollection)
            {
                Console.WriteLine("{0}", loan);
            }
            
            //-----------------------------------------------------------------------------------
            // Example of implementing IValidateableObject interface
            //-----------------------------------------------------------------------------------
            Product product = new Product()
            {
                CategoryId = 3,
                Id = 4,
                Name = "Butter"
            };

            var validationErrors = product.Validate(null);
            if(!validationErrors.Any())
            {
                Console.WriteLine("No errors encounterd.");
                Console.WriteLine("{0}", product);
            }

            Product product1 = new Product()
            {
                CategoryId = 3,
            };

            validationErrors = product1.Validate(null);
            if (validationErrors.Any())
            {
                Console.WriteLine("Errors encounterd.");
                foreach(var validationResult in validationErrors)
                {
                    Console.WriteLine("{0}", validationResult);
                }
            }
            
            //-----------------------------------------------------------------------------------
            // Casting using as
            //-----------------------------------------------------------------------------------
            Name name2 = new Name
            {
                FirstName = "John",
                LastName = "Jones",
                Values = new int[] { 0, 1, 2 }
            };

            DoWork(name2);
            DoWork(new Animal());
            
            //-----------------------------------------------------------------------------------
            // Serialization/Deserialization using Json
            //-----------------------------------------------------------------------------------
            Name name = new Name
            {
                FirstName = "John",
                LastName = "Jones",
                Values = new int[] { 0, 1, 2 }
            };
            string json = JsonConvert.SerializeObject(name);
            Console.WriteLine("Json: {0}", json);
            Name name1 = ConvertToName(json);
            Console.WriteLine("FirstName: {0}, LastName: {1}, Value: {2}", 
                name1.FirstName, name1.LastName, name1.Values[2]);

            //-----------------------------------------------------------------------------------
            // Using flag enums 
            //-----------------------------------------------------------------------------------
            Group userGroup = default;
            Console.WriteLine("UserGroup: {0}", userGroup);
            userGroup |= Group.Users;
            Console.WriteLine("UserGroup: {0}", userGroup);
            //Check if Users flag is set 
            if ((userGroup & Group.Users) == Group.Users)
            {
                Console.WriteLine("User flag is set, if((userGroup & Group.Users) == Group.Users).");
            }
            if (userGroup == Group.Users)
            {
                Console.WriteLine("User flag is set, if(userGroup == Group.Users).");
            }

            userGroup |= Group.Supervisors;
            Console.WriteLine("UserGroup: {0}", userGroup);
            userGroup |= Group.Managers;
            Console.WriteLine("UserGroup: {0}", userGroup);
            userGroup |= Group.Administrators;
            Console.WriteLine("UserGroup: {0}", userGroup);
            //Example of resetting a flag
            userGroup &= ~Group.Administrators;
            Console.WriteLine("UserGroup after resetting Group.Administrators flag: {0}", userGroup);
            userGroup &= ~Group.Managers;
            Console.WriteLine("UserGroup after resetting Group.Managers flag: {0}", userGroup);
            userGroup &= ~Group.Supervisors;
            Console.WriteLine("UserGroup after resetting Group.Supervisors flag: {0}", userGroup);
            userGroup &= ~Group.Users;
            Console.WriteLine("UserGroup after resetting Group.Users flag: {0}", userGroup);

            Group userGroup1 = Group.Users | Group.Supervisors | Group.Managers | Group.Administrators;
            Console.WriteLine("All flags set UserGroup: {0}", userGroup1);

            Group userGroup2 = default;
            userGroup2 |= Group.Supervisors;
            if(userGroup2 < Group.Administrators)
            {
                Console.WriteLine("Group.Supervisors < Group.Administrators");
            }
            
            //Open a shared file
            //const string Filename = "sharedFile.txt";
            //var fs = File.Open(Filename, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);
            
            //-----------------------------------------------------------------------------------
            // Read int value from console input 
            //-----------------------------------------------------------------------------------
            //bool bValidInteger;
            //int value1 = default;
            //do
            //{
            //    bValidInteger = GetValidInteger(ref value1);
            //}
            //while (!bValidInteger);
            //Console.Write("Successfully read value: {0}", value1);

            //-----------------------------------------------------------------------------------
            // Use WebRequest to download page, advantage of WebRequest allows advanced configuration       
            // WebRequest is an abstract class and creates HttpWebRequest, HttpsWebRequest, ftpWebRequest object  
            //-----------------------------------------------------------------------------------
            WebRequest webRequest = WebRequest.Create(new Uri("https://www.microsoft.com/nl-nl/"));
            WebResponse webResponse = webRequest.GetResponse();
            await GetData(webResponse);

            //-----------------------------------------------------------------------------------
            // Parse local DateTime string to DateTime object.
            //3/1/2020 6:04:04 PM
            //-----------------------------------------------------------------------------------
            Console.WriteLine("DateTime.Now: {0}", DateTime.Now.ToString());
            const string inputDate = "3/1/2020 6:04:04";
            DateTime validatedDate = default;

            //Converted to universal time ==> (local time == GMT + 1) 
            bool validDate = DateTime.TryParse(inputDate,
                CultureInfo.CurrentCulture,
                DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeLocal,
                out validatedDate);

            if (validDate)
            {
                Console.WriteLine("Valid Date: {0}", validatedDate.ToString());
            }

            //-----------------------------------------------------------------------------------
            // Conditionally include method, only included when debug mode 
            //-----------------------------------------------------------------------------------
            CalculateInterest(16325.62m, 2, 0.07m);
            Console.WriteLine("{0:0.00}", CalculateInterest(16325.62m, 2, 0.07m));

            //-----------------------------------------------------------------------------------
            // Debug Assert example
            //https://stackoverflow.com/questions/6951335/using-string-format-to-show-decimal-up-to-2-places-or-simple-integer/6951366
            //-----------------------------------------------------------------------------------
            Console.WriteLine("{0:0.00}", CalculateInterest(16325.62m, 2, 0.07m));
            //Console.WriteLine("{0:0.00}", CalculateInterest(0m, 2, 0.07m)); Activates Debug.Assert

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

    public interface IPerson
    {
        string FirstName { get; set; }
        string LastName { get; set; }
    }

    public class Name : IPerson
    {
        public int[] Values { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    //You implement System.ComponentModel.DataAnnotations.IValidateableObject interface to
    //provide a way to validate the Product object.
    //The Product object has the following requirements:
    //✑ The Id property must have a value greater than zero.
    //✑ The Name property must have a value other than empty or null.
    //You need to validate the Product object. Which code segment should you use?
    public class Product : IValidatableObject
    {
        public Product()
        {

        }

        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public bool IsValid { get; set; }

        public override string ToString()
        {
            return Name +", "+ CategoryId.ToString() +", "+ Id.ToString();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(Id <= 0)
            {
                yield return new ValidationResult("The Id property must have a value greater than zero."); 
            }

            if (string.IsNullOrEmpty(Name))
            {
                yield return new ValidationResult("The Name property must have a value other than empty or null.");
            }
        }
    }


    public class Loan : IFormattable
    {
        public Loan(decimal loanAmount, int loanTerm, decimal loanRate)
        {
            LoanAmount = loanAmount;
            LoanTerm = loanTerm;
            LoanRate = loanRate;
        }

        public void UpdateLoanAmount(decimal newLoanAmount)
        {
            LoanAmount = newLoanAmount;
        }

        public decimal LoanAmount { get; private set; }
        public int LoanTerm { get; private set; }
        public decimal LoanRate { get; private set; }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            //https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings
            return string.Format("Loan Amount: {0:C2}, Loan Term: {1} years, Rate: {2:P2}", LoanAmount, LoanTerm, LoanRate);
        }
    }

    public class LoanCollection : IEnumerable
    {
        private Loan[] _loans = default;
        public LoanCollection(Loan[] loans)
        {
            if (loans == null || !loans.Any())
                throw new ArgumentOutOfRangeException("Loan array passed is empty or null.");

            _loans = new Loan[loans.Length];
            //Copy all array references  
            loans.CopyTo(_loans, 0);
        }

        public IEnumerator GetEnumerator()
        {
            return _loans?.GetEnumerator();
        }
    }

    public class Book
    {
        public string Title { get; set; }
        public string ISBN { get; set; }
    }

    public delegate void AddBookCallBack(int i);
    public class BookTracker
    {
        private List<Book> books = new List<Book>();

        public void AddBook(string title, AddBookCallBack callBack)
        {
            books.Add(new Book() { Title = title });
            callBack(books.Count);
        }
  
        public void Print()
        {
            foreach(Book book in books)
                Console.WriteLine("{0}", book.Title);
        }
    }

    public class Lease
    {
        public Lease(int term)
        {
            _term = term;
        }

        private int _term;
        private const int MaximumTerm = 5;
        private const decimal Rate = 0.034m;

        //Eventhandler that is called when Lease Exceeds MaxTerm
        public event LeaseExceedsMaxTerm OnMaxTermExceeded;

        public int Term
        {   
            get { return _term; }
            set 
            {
                if (value <= MaximumTerm)
                    _term = value;
                else
                    //We can also use simplified invocation
                    //OnMaxTermExceeded?.Invoke(this, new EventArgs());
                    if (OnMaxTermExceeded != null)
                    {
                        OnMaxTermExceeded(this, new LeaseEventArgs("Max Term Is Exceeded."));
                    }
            }
        }
    }

    public class LeaseEventArgs : EventArgs
    {
        public LeaseEventArgs(string message)
        {
            Message = message;
        }

        public string Message { get; private set; }
    }

    public delegate void LeaseExceedsMaxTerm(object sender, LeaseEventArgs args);

    public class Inventory
    {
        public string ProductName { get; set; }
        public int Count { get; set; }
    }

    public class WareHouse
    {
        private static Inventory _inventory = null;
        private static object _lock = new object();

        public static Inventory Inventory
        {
            get 
            { 
                if(_inventory == null)
                {
                    lock(_lock)
                    {
                        if(_inventory == null)
                        {
                            _inventory = new Inventory();
                        }
                    }
                }

                return _inventory; 
            }
        }
    }

    public interface IOutputFormatter<T>
    {
        string GetOutput(IEnumerator<T> iterator, int recordSize);
    }

    public class TabDelimitedFormatter : IOutputFormatter<string>
    {
        readonly Func<int, char> suffix = (row) => row % 2 == 0 ? '\n' : '\t'; 
        public string GetOutput(IEnumerator<string> iterator, int recordSize)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for(int row = 1; iterator.MoveNext(); row++)
            {
                stringBuilder.Append(iterator.Current);
                stringBuilder.Append(suffix(row));
            }

            return stringBuilder.ToString();
        }
    }

    public static class ExtensionMethod
    {
        public static bool IsEmail(this string str)
        {
            //const string regex = "^$";
            //Regex regex1 = new Regex(regex);
            //bool bValid = regex1.IsMatch(regex);
            return true;
        }
    }

    public class UserAddedEventArgs : EventArgs
    {
        public UserAddedEventArgs(User user, DateTime timeCreated)
        {
            User = user;
            TimeCreated = timeCreated;
        }

        public DateTime TimeCreated { get; private set; }

        public User User { get; private set; }
    }

    public class User
    {
        //Copy constructor
        public User(User user)
        {
            LastName = user.LastName;
            FirstName = user.FirstName;
        }

        public User(string lastName, string firstName)
        {
            LastName = lastName;
            FirstName = firstName;
        }

        public string LastName { get; private set; }
        public string FirstName { get; private set; }
    }

    public class Users
    {
        private List<User> users = new List<User>();
        public event EventHandler<UserAddedEventArgs> OnUserAdded; 

        public void AddUser(User user)
        {
            users.Add(user);
            OnUserAdded?.Invoke(this, new UserAddedEventArgs(new User(user), DateTime.Now));
        }
    }

    public class Logger
    {
        public Logger()
        {
        }

        public static void Log(Exception ex)
        {
            Console.WriteLine("Class Logger Exception, Type: {0}, Message: {1}", ex.GetType(), ex.Message);
        }

        public static void Log(AdventureWorksException ex)
        {
            Console.WriteLine("Class Logger AdventureWorksException, Type: {0}, Message: {1}", ex.GetType(), ex.Message);
        }

        public static void Log(AdventureWorksValidationException ex)
        {
            Console.WriteLine("Class Logger AdventureWorksValidationException, Type: {0}, Message: {1}", ex.GetType(), ex.Message);
        }
    }


    //Custom exception classes
    public class AdventureWorksException : Exception
    {
        public AdventureWorksException(string message) : base(message)
        {
            Message = message;
        }

        public override string Message { get; }
    }

    public class AdventureWorksDbException : AdventureWorksException
    {
        public AdventureWorksDbException(string message) : base(message)
        {
            Message = message;
        }

        public override string Message { get; }
    }

    public class AdventureWorksValidationException : AdventureWorksDbException
    {
        public AdventureWorksValidationException(string message) : base(message)
        {
            Message = message;
        }

        public override string Message { get; }
    }

    public class Employee : IEquatable<Employee>
    {
        public Employee(int id, string name, DateTime birthDay)
        {
            ID = id;
            Name = name;
            BirthDay = birthDay;
        }

        public int ID { get; private set; }

        public string Name { get; private set; }

        public DateTime BirthDay { get; private set; }

        public bool Equals([AllowNull] Employee other)
        {
            Console.WriteLine("Employee Equals([AllowNull] Employee other) @Work");

            if (other == null)
                return false;

            //Check reference equality
            //if(ReferenceEquals(this, other))
            //    return true;

            if(ID != other.ID)
                return false;

            if(!Name.Equals(other.Name, StringComparison.Ordinal))
                return false;

            return true;
        }
    }

    public class LogFileViewer
    {
        public LogFileViewer(string filePath)
        {
            FilePath = filePath;
        }

        protected string FilePath { get; private set; }

        public void View()
        {
            string line;

            try
            {
                using (StreamReader streamReader = new StreamReader(FilePath))
                {

                    while ((line = streamReader.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                    }
                }

            }
            catch(FileNotFoundException ex)
            {
                Console.WriteLine("FileNotFoundException: " + ex.Message);
            }
            catch (Exception ex)
            {

                Console.WriteLine("Genral Exception: " + ex.Message);
            }
        }
    }

    public class Temperature : IComparable
    {
        public double Fahrenheit { get; private set; }

        public Temperature(double fahrenheit)
        {
            Fahrenheit = fahrenheit;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("Unable to compare null object.");

            //Return null if unable to cast to specified type
            Temperature otherTemperature = obj as Temperature;
            if (otherTemperature == null)
                throw new ArgumentException("Not Temperature type");

            return Fahrenheit.CompareTo(otherTemperature.Fahrenheit);
        }
    }

    public class Calculator
    {
        public int AddNumber(int x, int y)
        {
            return x + y;
        }

        public int SubNumber(int x, int y)
        {
            return x - y;
        }

        public int DoOperation(string operation, int x, int y)
        {
            //Calculator calculator = new Calculator();
            //Type type = calculator.GetType();
            //MethodInfo methodInfo = type.GetMethod(operation);
            //object[] prm = new object[] { x, y };
            //object result = methodInfo.Invoke(calculator, prm);
            //return (int)result;

            //We can also reference this instance.
            MethodInfo methodInfo = this.GetType().GetMethod(operation);
            object[] prm = new object[] { x, y };
            object result = methodInfo.Invoke(this, prm);
            return (int)result;
        }
    }

    public class Scorecard
    {
        private Dictionary<string, int> pairs = new Dictionary<string, int>();

        public void Add(string name, int score)
        {
            pairs.Add(name, score);
        }

        public int this[string key]
        {
            get { return pairs[key]; }
        }
    }

    public class class1 : class2
    {
    }

    public class class2 : INewInterface
    {
        void INewInterface.Method1()
        {
            Console.WriteLine("void INewInterface.Method1()");
        }
    }

    public interface INewInterface
    {
        void Method1();
    }

}