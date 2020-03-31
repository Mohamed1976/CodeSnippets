using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Collections;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Diagnostics;

namespace _70_483.Exercises
{
    public class GeneralExercises1
    {
        public GeneralExercises1()
        {
        }

        private int DoSomeWork(int taskNum, CancellationToken ct)
        {
            /* IF you want to use System.Windows.Threading.Dispatcher to update WPF UI thread
                You need to add this to project file.
                <Project Sdk="Microsoft.NET.Sdk.WindowsDesktop">
                    <PropertyGroup>
                        <TargetFramework>netcoreapp3.1</TargetFramework>
                        <UseWPF>true</UseWPF>
                    </PropertyGroup>
                </Project> */
            //Action action = () => lblStatus = taskNum;
            //Dispatcher.BeginInvoke(action)

            // Was cancellation already requested?
            if (ct.IsCancellationRequested)
            {
                Console.WriteLine("Task {0} was cancelled before it got started.", taskNum);
                ct.ThrowIfCancellationRequested();
            }

            for (int i = 0; i <= 1000000; i++)
            {
                for(int J = 0; J <= 1000; J++) { }
            }

            return 1;
        }

        private Task<string>GetData(Uri uri)
        {
            WebRequest webRequest = WebRequest.Create(uri);
            WebResponse webResponse = webRequest.GetResponse();
            StreamReader streamReader = new StreamReader(webResponse.GetResponseStream());
            return streamReader.ReadToEndAsync();
        }
            
        private bool CheckName(string name)
        {
            Thread.Sleep(500);
            return name.Length > 3;
        }

        private Task<List<string>> GetWebSiteUrls(string urls)
        {
            const string matchPattern = @"(http|https)://(www\.)?([^\.])+\.(com|nl)";

            return Task.Factory.StartNew(() =>
            {
                MatchCollection matches = Regex.Matches(urls, matchPattern, RegexOptions.Compiled);
                //throw new Exception("Self Made disaster");
                //We can also use LINQ to populate List<string>, as shown below 
                List<string> listOfUrls = (from Match match in matches select match.Value).ToList<string>();
                return listOfUrls;

                //Use foreach loop to fill list 
                //List<string> listOfUrls = new List<string>();
                //foreach (Match match in matches)
                //    listOfUrls.Add(match.Value);
                //return listOfUrls;

                //The code below is not possible, you cant cast an enumerator to List<string>  
                //Unable to cast object of type 'Enumerator' to type 'System.Collections.Generic.List`1[System.String]'.
                //List<string> listOfUrls = (List<string>)matches.GetEnumerator();
                //return listOfUrls;
            });
        }

        private async Task<string> ReadFile(string path)
        {
            const int bufferSize = 4096;
            Encoding encoding = Encoding.UTF8;
            byte[] buffer = new byte[bufferSize];
            int bytesRead = default(int);
            StringBuilder sb = new StringBuilder();

            //Note FileMode.Open throws an exception if file was not found
            using (FileStream fileStream = new FileStream(path, FileMode.Open,
                FileAccess.Read, FileShare.ReadWrite, bufferSize: bufferSize))
            {
                do
                {
                    bytesRead = await fileStream.ReadAsync(buffer, 0, buffer.Length);
                    sb.Append(encoding.GetString(buffer));
                } while (bytesRead == bufferSize);
            }

            return sb.ToString();
        }

        private async Task<string> ReadFileV2(string path)
        {
            string content;

            //Note FileMode.Open throws an exception if file was not found
            using (FileStream fileStream = new FileStream(path, FileMode.Open,
                FileAccess.Read, FileShare.ReadWrite))
            {
                byte[] buffer = new byte[fileStream.Length];
                int bytesRead = await fileStream.ReadAsync(buffer, 0, buffer.Length);
                content = Encoding.UTF8.GetString(buffer);
                Console.WriteLine("Bytes Read: {0}, File Size {1}", bytesRead, fileStream.Length);
            }

            return content;
        }

        private async Task GetWebContent(Uri uri)
        {
            HttpClient httpClient = new HttpClient();
            Task<string> task = httpClient.GetStringAsync(uri);
            Console.WriteLine("Downloading from: {0}", uri);
            string content = await task;
            Console.WriteLine("Downloading complete length: {0}", content.Length);
        }

        private bool CheckNameLength(string name)
        {
            Thread.Sleep(500);
            return name.Length > 5;
        }

        object _lock = new object();
        private void ValidateDate()
        {
            List<string> list = new List<string>() { "Li", "Christopher", "Joe", "David", "Pete", "Elizabeth" };
            List<string> filterdList = new List<string>();

            Parallel.ForEach(list, delegate (string name)
            {
                if(CheckNameLength(name))
                {
                    lock(_lock)
                    {
                        filterdList.Add(name);
                    }
                }
            });

            foreach(string str in filterdList)
            {
                Console.WriteLine(str);
            }
        }

        private List<Category> GetCategories()
        {
            return new List<Category>()
            {
                new Category() { ID = 1, Name="Food" },
                new Category() { ID = 2, Name="Beverage" },
                new Category() { ID = 3, Name="Condiment" }
            };
        }

        private List<FoodProduct> GetProducts()
        {
            return new List<FoodProduct>()
            {
                new FoodProduct() { CategoryID = 1, Name="Pizza" },
                new FoodProduct() { CategoryID = 1, Name="Pasta" },
                new FoodProduct() { CategoryID = 2, Name="Soda" },
                new FoodProduct() { CategoryID = 2, Name="Water" },
                new FoodProduct() { CategoryID = 3, Name="Ketchup" },
                new FoodProduct() { CategoryID = 3, Name="Mustard" },
                new FoodProduct() { CategoryID = 3, Name="Mayonaise" }
            };
        }

        private void ProcessProducts()
        {
            //First create products and categories
            List<FoodProduct> foodProducts = GetProducts();
            List<Category> categories = GetCategories();

            //Perform join between products and categories so we can display Product and corresponding category   
            var joinResult = from product in foodProducts
                             join category in categories on product.CategoryID equals category.ID
                             select new { product.Name, Category = category.Name }; //annonymous type

            foreach (var result in joinResult)
                Console.WriteLine("Product: {0}, Category: {1}", result.Name, result.Category);

            //Simple group by 
            var groupByResult = from product in foodProducts
                                group product by product.CategoryID
                                into productCountPerGroup
                                select new { CategoryID = productCountPerGroup.Key, ProductCount = productCountPerGroup.Count() };

            //Display groups
            foreach (var group in groupByResult)
                Console.WriteLine("CategoryID: {0}, Nr of Product: {1}", group.CategoryID, group.ProductCount);

            //Note we can also iterate through a group, as shown below
            //Each product.CategoryID is stored with a collection of FoodProducts 
            IEnumerable<IGrouping<int, FoodProduct>> groupByResult2 = from product in foodProducts
                                                                      group product by product.CategoryID
                                                                      into productCountPerGroup
                                                                      select productCountPerGroup;

            foreach(IGrouping<int, FoodProduct> group in groupByResult2)
            {
                Console.WriteLine("CategoryID: {0}", group.Key);
                foreach(FoodProduct foodProduct in group)
                {
                    Console.WriteLine("\tName: {0}, CategoryID: {1}", foodProduct.Name, foodProduct.CategoryID);
                }
            }

            var groupByResult3 = from product in foodProducts
                                 group product by product.CategoryID
                                 into groupedProducts
                                 select new { CategoryID = groupedProducts.Key, Products = groupedProducts };

            foreach(var group in groupByResult3)
            {
                Console.WriteLine("CategoryID: {0}", group.CategoryID);
                foreach(var product in group.Products)
                {
                    Console.WriteLine("\tName: {0}, CategoryID: {1}", product.Name, product.CategoryID);
                }
            }

            //Combining LINQ Join and Group By  
            var groupedProducts1 = from category1 in categories
                                   join product1 in foodProducts on category1.ID equals product1.CategoryID
                                   group product1 by category1.Name
                                   into group1
                                   orderby group1.Key descending
                                   select new { CategoryName = group1.Key, Products = group1 };
            
            foreach(var group in groupedProducts1)
            {
                Console.WriteLine("Category: {0}", group.CategoryName);
                foreach (var product in group.Products)
                {
                    Console.WriteLine("\tName: {0}, CategoryID: {1}", product.Name, product.CategoryID);
                }
            }
        }

        object _lock1 = new object();


        private int _progressCounter = 0;
        protected int ProgressCounter 
        {
            get 
            {
                lock (_lock1)
                {
                    return _progressCounter;
                }
            }
            private set
            {
                lock(_lock1)
                {
                    _progressCounter = value;
                }
            } 
        }

        private void Method1Async()
        {
            Random random = new Random(13);
            
            for(int i = 0; i < 10; i++)
            {
                Thread.Sleep(random.Next(100, 500));
                Console.WriteLine("Thread 1, value: {0}", ProgressCounter++);
            }            
        }

        private object _locker = new object();

        private bool IsOdd(int value)
        {
            Thread.Sleep(500);
            return ((value % 2) != 0);
        }

        //Whether you’re doing async work or not, accepting a CancellationToken as a parameter to your method is a great 
        //pattern for allowing your caller to express lost interest in the result.
        //When canceling leave your method/application in a valid state, cleanup if necessary 

        private string LongRunningProcess(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            for(int i = 0; i < 20; i++)
            {
                Thread.Sleep(100);

                //If you throw ThrowIfCancellationRequested, Task Status will be canceled
                cancellationToken.ThrowIfCancellationRequested();                
                //If you use Token below, Task will return RanToCompletion
                //if (cancellationToken.IsCancellationRequested)
                //{
                //break;
                //}
            }

            if (id == 1)
                throw new ArgumentException("Wrong answer.");

            return "LongRunningProcess has successfully finished";
        }

        private bool ValidatePrice(string value)
        {
            const string regexPattern = @"^\d+(\.\d\d)?$";

            Regex regex = new Regex(regexPattern);            
            if(regex.IsMatch(value))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        string[] _list = new string[] { "Alfa", "Beta", "Gamma", "Delta", "Zetta", "Sigma", "Ksi" }; 

        private void GetPagedInfo(int page, out string[] info)
        {
            const int pageSize = 5;
            info = _list.Skip((page - 1) * pageSize).Take(pageSize).ToArray();
        }

        public bool StringCompare(string string1, string string2, string string3)
        {
            StringBuilder stringBuilder = new StringBuilder(string1);
            stringBuilder.Append(string2);
            bool isEqual = stringBuilder.ToString().Equals(string3, StringComparison.CurrentCultureIgnoreCase);
            return isEqual;
        }

        public bool StringCompareV2(string string1, string string2, string string3)
        {
            string strings = string1 + string2;
            bool isEqual = strings.ToUpper() == string3.ToUpper();
            return isEqual;
        }

        private delegate int AddNumbers(int x, int y);

        public async Task Run()
        {
            //-----------------------------------------------------------------------------------
            // You need to ensure that the Customers class can be initialized by using the following code. {item1, item2 }
            // Create extension method
            //-----------------------------------------------------------------------------------
            WordDictionaries wordDictionaries = new WordDictionaries()
            {
                new WordDictionary() { FileName = "Alfa", Size = 2020 },
                new WordDictionary() { FileName = "Beta", Size = 2050 },
                new WordDictionary() { FileName = "Gamma", Size = 1970 },
                new WordDictionary() { FileName = "Zetta", Size = 1776 }
            };

            //wordDictionaries.AddDictionary(new WordDictionary() { FileName = "Alfa", Size = 2020 });
            //wordDictionaries.AddDictionary(new WordDictionary() { FileName = "Beta", Size = 2010 });
            IEnumerator enumerator = wordDictionaries.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Console.WriteLine(((WordDictionary)enumerator.Current).FileName + ", " + ((WordDictionary)enumerator.Current).Size);
            }

            //-----------------------------------------------------------------------------------
            // Create an anonymous method.
            //-----------------------------------------------------------------------------------
            AddNumbers add = delegate (int x, int y)
            {
                return x + y;
            };

            //-----------------------------------------------------------------------------------
            //You need to ensure that StringCompare only returns true if string1 concatenated to string2 is
            //equal to string3.The comparison must be case-insensitive.The solution must ensure that
            //executes as quickly as possible.
            //-----------------------------------------------------------------------------------
            Console.WriteLine("Alfa + Beta == AlfaBeta: {0}", StringCompare("Alfa", "Beta", "AlfaBeta"));
            Console.WriteLine("Alfa + Beta == AlfaBeta: {0}", StringCompareV2("Alfa", "Beta", "AlfaBeta"));

            //-----------------------------------------------------------------------------------
            // You need to write code that will display value1, and then value2 in the console.
            //-----------------------------------------------------------------------------------
            string settings = "value1;value2";
            foreach (string val in settings.Split(';')) // you can use string in stead of char notation Split(";"))
                Console.WriteLine(val);
            
            //-----------------------------------------------------------------------------------
            // You have an application that uses paging. Each page displays five items from a list.
            // You need to display the second page.
            //-----------------------------------------------------------------------------------
            string[] info = null;
            GetPagedInfo(1, out info);
            Debug.Assert(info != null);
            foreach (string _str in info)
                Console.WriteLine(_str);

            Console.WriteLine();
            GetPagedInfo(2, out info);
            foreach (string _str in info)
                Console.WriteLine(_str);

            //-----------------------------------------------------------------------------------
            // Operator examples
            //-----------------------------------------------------------------------------------
            int a = 1;
            int b = 2;
            Console.WriteLine("a == --b && a == b++: {0}", a == --b && a == b++); //True
            Console.WriteLine("a == --b || a == b++: {0}", a == --b || a == b++); //True, second part of || is not executed therefore b == 1 
            Console.WriteLine("a == --b && b == a++: {0}", a == --b && b == a++); //False
            
            //-----------------------------------------------------------------------------------
            // Array exercise resulting in following output: 1, 3, 6, 10, 15  
            //-----------------------------------------------------------------------------------
            int[] arr = new int[] {1,2,3,4,5 };
            //int sum = 0;
            //for(int i = 0; i < arr.Length;)
            //{
            //    sum += arr[i];
            //    arr[i++] = sum;
            //}

            for(int i = 1; i < arr.Length; i++)
            {
                arr[i] += arr[i - 1];
            }

            foreach(int val in arr)
            {
                Console.WriteLine(val);
            }
            
            //-----------------------------------------------------------------------------------
            // RegEx must ensure that prices are positive and have two decimal places.
            //-----------------------------------------------------------------------------------
            Console.WriteLine("2.77 is valid: {0}", ValidatePrice("2.77"));
            Console.WriteLine("-2.77 is valid: {0}", ValidatePrice("-2.77"));
            Console.WriteLine("2.7 is valid: {0}", ValidatePrice("2.7"));
            Console.WriteLine("2 is valid: {0}", ValidatePrice("2"));
            Console.WriteLine("2.779 is valid: {0}", ValidatePrice("2.779"));
            Console.WriteLine("456 is valid: {0}", ValidatePrice("456"));
            
            //-----------------------------------------------------------------------------------
            // Hashtable
            // Represents a collection of key/value pairs that are organized based on the hash code of the key.
            // Microsoft:
            // We don't recommend that you use the Hashtable class for new development. Instead, 
            // we recommend that you use the generic Dictionary<TKey,TValue> class.
            //-----------------------------------------------------------------------------------
            Hashtable openWith = new Hashtable();
            // Add some elements to the hash table. There are no 
            // duplicate keys, but some of the values are duplicates.
            openWith.Add("txt", "notepad.exe");
            openWith.Add("bmp", "paint.exe");
            openWith.Add("dib", "paint.exe");
            openWith.Add("rtf", "wordpad.exe");
            Console.WriteLine("For key = \"rtf\", value = {0}.", openWith["rtf"]);
            openWith["rtf"] = "winword.exe";
            Console.WriteLine("For key = \"rtf\", value = {0}.", openWith["rtf"]);

            //-----------------------------------------------------------------------------------
            // Thread/ThreadPool, Tasks/Task.Factory examples
            //-----------------------------------------------------------------------------------
            int ID = 3;
            CancellationTokenSource cancellationTokenSource1 = new CancellationTokenSource();
            CancellationToken cancellationToken1 = cancellationTokenSource1.Token;
            //Task<string> task4 = Task.Run(() =>
            //{
            //    return LongRunningProcess(ID, cancellationToken1);
            //});

            //Task[] continuationTasks = new Task[3];

            //continuationTasks[0] = task4.ContinueWith((i) =>
            //{
            //    Console.WriteLine("Task Canceled");
            //}, TaskContinuationOptions.OnlyOnCanceled);

            //continuationTasks[1] = task4.ContinueWith((i) =>
            //{
            //    Console.WriteLine("Task Faulted");
            //}, TaskContinuationOptions.OnlyOnFaulted);

            //continuationTasks[2] = task4.ContinueWith((i) =>
            //{
            //    Console.WriteLine("Task Completed");
            //}, TaskContinuationOptions.OnlyOnRanToCompletion);

            //await Task.WhenAll(continuationTasks);
            //Console.WriteLine("All continuation tasks completed.");
            
            Task<string> task3 = Task.Run<string>(() =>
            {
                return LongRunningProcess(ID, cancellationToken1);
            }, cancellationToken1);

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            //Task.Run(() =>
            //{
            //    Thread.Sleep(50);
            //    cancellationTokenSource1.Cancel();
            //});
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            await task3.ContinueWith((task) =>
            {
                if(task.Status == TaskStatus.RanToCompletion)
                {
                    Console.WriteLine("Task completed successfully, result: {0}", task.Result);
                }
                else if(task.Status == TaskStatus.Faulted)
                {
                    Console.WriteLine("Task has faulted.");
                }
                else if (task.Status == TaskStatus.Canceled)
                {
                    Console.WriteLine("Task has been canceled.");
                }
            });

            ThreadPool.QueueUserWorkItem((state) =>
            {
                Thread.Sleep(500);
                Console.WriteLine("From ThreadPool.QueueUserWorkItem: {0}", state as string);
            }, "Message in a bottle");

            Task<string>[] tasks3 = new Task<string>[]
            {
                Task.Factory.StartNew<string>(() =>
                {
                    Thread.Sleep(1000);
                    return "Hi from thread 1";
                }, TaskCreationOptions.DenyChildAttach), //DenyChildAttach is option defult used by Task.Run  
                Task.Factory.StartNew<string>(()=>
                {
                    Thread.Sleep(1000);
                    return "Hi from thread 2";
                })
            };

            string[] taskResults = await Task.WhenAll(tasks3);
            foreach (string taskResult in taskResults)
                Console.WriteLine(taskResult);

            Task<int>[] tasks = new Task<int>[]
            {
                Task.Run<int>(()=> { Thread.Sleep(1000); return 1; }),
                Task.Run<int>(()=> { Thread.Sleep(500); return 2; }),
                Task.Run<int>(()=> { Thread.Sleep(750); return 3; }),
                Task.Run<int>(()=> { Thread.Sleep(800); return 4; })
            };

            while (tasks.Length > 0)
            {
                //Returns: The index of the completed task in the tasks array argument, or -1 if the timeout occurred.
                int index = Task.WaitAny(tasks);
                Console.WriteLine("Another one bites the dust, index: {0}, Value: {1}", index, tasks[index].Result);
                //Instead of copying array elements to array elements, just convert to list and back
                List<Task<int>> tasks1 = tasks.ToList<Task<int>>();
                tasks1.RemoveAt(index);
                tasks = tasks1.ToArray();
            }

            Console.WriteLine("All tasks are finished");

            //await Task.WhenAll(tasks);
            //foreach (Task<int> taskResult in tasks)
            //    Console.WriteLine("Task result: {0}", taskResult.Result);

            int[] intArray = new int[] { 11, 13, 67, 45, 99, 86, 65, 77, 89,50,80 };
            List<int> oddNumbers = new List<int>();
            //Use For loop to iterate only over the first six elements in array 0 to 5 (6 is excluded) 
            Parallel.For(0, 6, (index) =>
            {
                  if (IsOdd(intArray[index]))
                      lock(_locker)
                      {
                          oddNumbers.Add(intArray[index]);
                      }
            });

            Console.WriteLine("Odd Number Count : {0}", oddNumbers.Count);
            for (int i = 0; i < oddNumbers.Count; i++)
            {
                Console.WriteLine("Odd number: {0}", oddNumbers[i]);
            }

            oddNumbers.Clear();
            Parallel.ForEach(intArray, (value) =>
             {
                 if (IsOdd(value))
                     lock (_locker)
                         oddNumbers.Add(value);
             });

            Console.WriteLine("Odd Number Count : {0}", oddNumbers.Count);
            for (int i = 0; i < oddNumbers.Count; i++)
            {
                Console.WriteLine("Odd number: {0}", oddNumbers[i]);
            }

            Thread thread = new Thread(new ThreadStart(Method1Async));
            //Using lambda expression
            Thread thread1 = new Thread(new ThreadStart(() =>
            {
                Random random = new Random(13);

                for (int i = 0; i < 10; i++)
                {
                    Thread.Sleep(random.Next(100, 500));
                    Console.WriteLine("Thread 2, value: {0}", ProgressCounter++);
                }
            }));

            //Using delegate
            Thread thread2 = new Thread(new ThreadStart(delegate()
            {
                Random random = new Random(13);

                for (int i = 0; i < 10; i++)
                {
                    Thread.Sleep(random.Next(100, 500));
                    Console.WriteLine("Thread 3, value: {0}", ProgressCounter++);
                }
            }));

            thread1.Start();
            thread2.Start();
            thread.Start();
            thread1.Join();
            thread2.Join();
            thread.Join();
            Console.WriteLine("All threads are finished");

            //-----------------------------------------------------------------------------------
            // LINQ Join Group By example
            //-----------------------------------------------------------------------------------
            ProcessProducts();
            
            //-----------------------------------------------------------------------------------
            // DataContractSerializer, methods in class to be serialized is decorated with DataMember attribute 
            // class itself is decorated with DataContract attribute
            //[DataMember(Name="GivenName", IsRequired = true)], property will be serialized with specified name  
            //-----------------------------------------------------------------------------------
            Politician politician = new Politician() { FirstName = "Bernie", LastName="Sanders" };
            DataContractJsonSerializer dataContractJsonSerializer = 
                new DataContractJsonSerializer(typeof(Politician));

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (StreamReader streamReader = new StreamReader(memoryStream))
                {
                    dataContractJsonSerializer.WriteObject(memoryStream, politician);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    //memoryStream.Position = 0; simillar as seek                
                    string serializedObj = streamReader.ReadToEnd();
                    Console.WriteLine(serializedObj);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    //Deserialize object
                    Politician politician1 = (Politician)dataContractJsonSerializer.ReadObject(memoryStream);
                    Console.WriteLine("Deserialized: {0}, {1}", politician1.FirstName, politician1.LastName);
                }
            }
            
            //-----------------------------------------------------------------------------------
            // Simple PLINQ example
            //-----------------------------------------------------------------------------------
            List<string> list3 = new List<string>() { "Li", "Christopher", "Joe", "David", "Pete", "Elizabeth" };
            List<string> list4 = list3.AsParallel().Where(name => CheckNameLength(name)).ToList();
            foreach (string item in list4)
            {
                Console.WriteLine(item);
            }

            //-----------------------------------------------------------------------------------
            // Example of using, Parallel.For, Parallel.ForEach Parallel.Invoke 
            //-----------------------------------------------------------------------------------
            ValidateDate();

            //-----------------------------------------------------------------------------------
            // Example of using httpClient
            //-----------------------------------------------------------------------------------
            await GetWebContent(new Uri(@"https://www.microsoft.com/nl-nl/"));

            //-----------------------------------------------------------------------------------
            // Example of linq deferred execution
            //-----------------------------------------------------------------------------------
            List<int> list2 = new List<int>() { 1, 2, 3, 4 };
            IEnumerable<int> filterdList2 = list2.Where(value => value > 3);

            foreach (int val in filterdList2)
                Console.WriteLine("FilterdList deferred execution, BEFORE ADD: {0}", val);

            list2.Add(5);
            //5 is included in the list because of deferred execution  
            foreach (int val in filterdList2)
                Console.WriteLine("FilterdList deferred execution, AFTER ADD: {0}",val);

            //If you want that filterdList not have deferred execution
            //you need to convert linq execution to a list/array
            //Aggregates are directly executed and not deffered
            IEnumerable<int> filterdList3 = list2.Where(value => value > 3).ToList();
            
            foreach (int val in filterdList3)
                Console.WriteLine("FilterdList WITHOUT deferred execution, BEFORE ADD: {0}", val);

            list2.Add(6);
            foreach (int val in filterdList3)
                Console.WriteLine("FilterdList WITHOUT deferred execution, AFTER  ADD: {0}", val);
            
            //-----------------------------------------------------------------------------------
            // Example of overflow exceptions
            //-----------------------------------------------------------------------------------
            try
            {
                double valDouble1 = 0.0;
                double valDouble2 = 0.0;
                double resultDivision = valDouble1 / valDouble2;
                Console.WriteLine("Division: {0}", resultDivision); //Results in NaN
                //Example of overflow exception
                //checked
                //{
                //    int ResultInt = int.MaxValue + 10;
                //}                
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine("DivideByZeroException exception: {0}", ex.Message);
            }
            catch (ArithmeticException ex)
            {
                Console.WriteLine("ArithmeticException exception: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("General error in division: {0}", ex.Message);
            }
            
            //-----------------------------------------------------------------------------------
            // Example of reading textfile using fixed buffer
            //-----------------------------------------------------------------------------------
            try
            {
                string content = await ReadFileV2(@"C:\Users\moham\Desktop\Notes\Notes001.txt");
                //Example below performs multiple reads using a buffer
                //string content = await ReadFile(@"C:\Users\moham\Desktop\Notes\Notes001.txt");
                Console.WriteLine(content);
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("FileNotFoundException in ReadFile(path): {0}", ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while calling ReadFile(path): {0}", ex.Message);
            }
            
            //-----------------------------------------------------------------------------------
            // Simple example that illustrates how to remove items from list 
            //-----------------------------------------------------------------------------------
            string str = "This is a random sentence.";
            //            01234567890123456789
            int val1 = str.IndexOf("random"); //Should be 10
            int val2 = str.LastIndexOf("is"); //Should be 5
            string str1 = str.Substring(0, val2); // "This "
            string str2 = str.Substring(val1); // "random sentence."
            Console.WriteLine(str1 + str2); //"This random sentence."

            //-----------------------------------------------------------------------------------
            // Simple example that illustrates how to remove items from list 
            //-----------------------------------------------------------------------------------
            List<string> simpleList = new List<string>() { "Alfa", "Beta", "Gamma", "Delta" };
            //simpleList.Clear(); //removes all items 
            //Note simpleList.Count is updated when items are removed, it does not have fixed size such as array
            //Count represents numer of items in collection 

            //Code below is Wrong, because you cannot remove items
            //in a foreach loop, the enumerator will then cause an error.
            //Furthermore you cannot modify value types using foreach loop
            //If you use reference types you can modify them in foreach loop 
            //foreach (string item in simpleList)
            //    simpleList.Remove(item);

            simpleList.Remove("Gamma");

            //Example below also removes all items from list
            //while (simpleList.Count != 0)
            //    simpleList.RemoveAt(0);

            //Example below also removes all items from list
            //int total = simpleList.Count;
            //for (int i = 0; i < total; i++)
            //    simpleList.RemoveAt(0);

            Console.WriteLine("List size: {0}: ",simpleList.Count);
            foreach (string item in simpleList)
                Console.WriteLine(item);
            
            //-----------------------------------------------------------------------------------
            // Example of Activator.CreateInstance Method
            // Creates an instance of the specified type using the constructor that best 
            // matches the specified parameters.
            //https://docs.microsoft.com/en-us/dotnet/api/system.activator.createinstance?view=netframework-4.8
            //-----------------------------------------------------------------------------------
            Person p1  = Activator.CreateInstance<Person>();
            Type type = typeof(Person);
            Person p2 = (Person)Activator.CreateInstance(type); //Returns object and needs to be casted
            Person p3 = (Person)Activator.CreateInstance(type, new object[] { "Pete", "Buttigieg" });
            Console.WriteLine(p1);
            Console.WriteLine(p2);
            Console.WriteLine(p3);
            
            //-----------------------------------------------------------------------------------
            // Retrieve list of strings that match regex pattern
            //-----------------------------------------------------------------------------------
            string urls = "https://www.google.comhttps://stackoverflow.comhttps://twitter.com/https://www.nu.nl";

            try
            {
                List<string> urlList = await GetWebSiteUrls(urls);
                foreach (string url in urlList)
                    Console.WriteLine(url);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while calling GetWebSiteUrls(urls): {0}", ex.Message);
            }

            //-----------------------------------------------------------------------------------
            // In a test exam it was illustrated that this was possible var2 = ((List<int>) array1) [0]; 
            // in which array1 is an ArrayList filled with integers, this is not possible (see code below). 
            //-----------------------------------------------------------------------------------
            ArrayList arrayList = new ArrayList() { 2020, 2030, 2050 };
            int result1 = Convert.ToInt32(arrayList[1]); //Can also convert objects to Int32, if the right type 
            Console.WriteLine(result1);
            //var var1 = ((List<int>)arrayList)[0]; //Compiler error 
            //var var1 = (int[])arrayList.ToArray(); Compiler error Cannot convert object array into int array
            
            //-----------------------------------------------------------------------------------
            // Using Generic class, type must be Reference (class) type and have a parameterless constructor 
            // Method is also generic type, can only be called by types that support the ICloneable interface 
            //-----------------------------------------------------------------------------------
            Person person1 = new Person("Joe", "Biden");
            Person person3 = (Person)person1.Clone();
            Console.WriteLine("Person1: {0}", person1);
            Console.WriteLine("Person2: {0}", person3);
            //ObjectParser and CloneMethod illustrate generics and restrictions that you can define on them  
            ObjectParser<object> objectParser = new ObjectParser<object>();
            Person person = new Person("John", "Deer");
            Person person2 = objectParser.CloneMethod<Person>(person);
            Console.WriteLine("Person1: {0}", person);
            Console.WriteLine("Person2: {0}", person2);

            //-----------------------------------------------------------------------------------
            //Perform Linq query parallel 
            //-----------------------------------------------------------------------------------
            List<string> list = new List<string>()
            {
                "Chris", "John", "Kennedy", "Li"
            };

            //Before you can use Linq methods, you need to reference "using System.Linq";  
            List<string> filterdList1 = list.AsParallel().Where((item) => CheckName(item)).ToList();
            foreach (string name in filterdList1)
                Console.WriteLine(name);

            //-----------------------------------------------------------------------------------
            // Example of using Parallel
            //Parallel.invoke, for, foreach is the preferred way to perform CPU intensive tasks.
            //If your code performs IO, Database IO or Web IO than you should use async await instead of parallel.   
            //-----------------------------------------------------------------------------------
            List<string> filterdList = new List<string>();
            Parallel.ForEach(list, (name) =>
            {
                if (CheckName(name))
                    filterdList.Add(name);
            });

            foreach (string name in filterdList)
                Console.WriteLine(name);
            
            //-----------------------------------------------------------------------------------
            // Example of async and await
            //-----------------------------------------------------------------------------------            
            string result = await GetData(new Uri(@"https://www.microsoft.com/nl-nl/"));
            Console.WriteLine("Webpage size: {0}", result.Length);
            
            //-----------------------------------------------------------------------------------
            // Example of using Tasks
            //-----------------------------------------------------------------------------------            
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken ct = cancellationTokenSource.Token;
            Task<int> task = Task.Run<int>(() =>
            {
                int result = DoSomeWork(1, ct);
                return result;
            });

            await task;
            Console.WriteLine("Task.Run result: {0}", task.Result);

            Task<int> task2 = Task.Factory.StartNew(() =>
            {
                int result = DoSomeWork(1, ct);
                return result;
            });

            await task2;
            Console.WriteLine("Task.Factory.StartNew result: {0}", task.Result);
        }
    }

    public class ObjectParser<T> where T: class, new()
    {
        public ObjectParser()
        {
        }

        public T2 CloneMethod<T2>(T2 t2) where T2: ICloneable 
        {
            //Note we can also create T type because it is a reference type and has 
            //a parameterless constructor
            T obj = new T();

            //T2 implements IClonable interface 
            T2 clone = (T2)t2.Clone();
            return clone;
        }
    }

    public class Person : ICloneable
    {
        public Person()
        {
            if (((new Random(6)).Next(0, 100)) % 2 == 0)
            {
                FirstName = "John";
            }
            else
            {
                FirstName = "Jane";
            }
            LastName = "Doe";
            Id = Guid.NewGuid();
        }

        public Person(string lastName, string firstName)
        {
            LastName = lastName;
            FirstName = firstName;
            Id = Guid.NewGuid();
        }

        //Copy constructor
        public Person(Person person)
        {
            LastName = person.LastName;
            FirstName = person.FirstName;
            Id = Guid.NewGuid();
        }

        public string LastName { get; private set; }

        public string FirstName { get; private set; }

        public Guid Id { get; private set; }

        public object Clone()
        {
            return new Person(this);
        }

        public override string ToString()
        {
            return FirstName + ", " + LastName + ", " + Id.ToString();
        }
    }

    [DataContract()]
    public class Politician
    {
        [DataMember(Name="GivenName", IsRequired = true)]
        public string FirstName { get; set; }

        [DataMember(Name = "SurName", IsRequired = true)]
        public string LastName { get; set; }
    }

    public class Category
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class FoodProduct
    {
        public int CategoryID { get; set; }
        public string Name { get; set; }
    }

    public class CustomException : Exception
    {
        public CustomException(string msg) : base(msg)
        {
        }
    }

    public class WordDictionary
    {
        public string FileName { get; set; }
        public int Size { get; set; }
    }

    public class WordDictionaries : IEnumerable<WordDictionary>
    {
        private List<WordDictionary> wordDictionaries = new List<WordDictionary>();

        public void AddDictionary(WordDictionary wordDictionary)
        {
            wordDictionaries.Add(wordDictionary);
        }

        //Alternatively we can use extension method that defines the Add method, as shown below after this class WordDictionariesExtension
        //public void Add(WordDictionary wordDictionary)
        //{
        //    wordDictionaries.Add(wordDictionary);
        //}

        public IEnumerator<WordDictionary> GetEnumerator()
        {
            return wordDictionaries.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public static class WordDictionariesExtension
    {
        public static void Add(this WordDictionaries wordDictionaries, WordDictionary wordDictionary)
        {
            wordDictionaries.AddDictionary(wordDictionary);
        }
    }
}
