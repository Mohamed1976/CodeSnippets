using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Threading;
using System.IO;
using System.Xml;
using System.Collections;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Net;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Runtime.Serialization.Formatters.Binary;
using System.Globalization;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace _70_483_USING_NET_FRAMEWORK.Exercises
{
    class GeneralExercises
    {
        private Task<List<string>> TestIfWebSite(string url)
        {
            const string pattern = @"(http|https)://(www\.)?([^\.]+)\.com";

            return Task.Run<List<string>>(() =>
            {
                MatchCollection matches = Regex.Matches(url, pattern, RegexOptions.Compiled);
                List<string> list = (from Match match in matches select match.Value).ToList();
                return list;
            });
        }

        private List<string> GetValidUrls(string urls)
        {
            const string pattern = @"(http|https)://(www\.)?([^\.]+)\.com";
            var regex = new Regex(pattern);
            var matches = regex.Matches(urls);

            var validUrls = new List<string>();

            foreach(Match match in matches)
            {
                if(match.Success)
                {
                    validUrls.Add(match.Value);
                }
            }

            return validUrls;
        }

        private List<string> GetValidUrlsV1(string urls)
        {
            const string pattern = @"(http|https)://(www\.)?([^\.]+)\.com";
            var regex = new Regex(pattern);
            var matches = regex.Matches(urls);

            return (from Match match in matches where match.Success select match.Value).ToList();
        }


        private Task ViewMetaData(string filePath)
        {
            return Task.Factory.StartNew(() =>
            {
                byte[] content = System.IO.File.ReadAllBytes(filePath);
                //Loads an assembly into the reflection-only context, where it can be examined but not executed.
                Assembly assembly = Assembly.ReflectionOnlyLoad(content);
                foreach (Type type in assembly.GetTypes())
                    Console.WriteLine(type);
            }, TaskCreationOptions.DenyChildAttach);
        }

        private long StopWatch(CancellationToken ct)
        {
            long ticks = 0;
            while (!ct.IsCancellationRequested)
                ticks++;

            return ticks;
        }

        private void EvaluateLoan()
        {
            Console.WriteLine("EvaluateLoan() executed");
        }

        private void ProcessLoan()
        {
            Console.WriteLine("ProcessLoan() executed");
        }

        private void FundLoan()
        {
            Console.WriteLine("FundLoan() executed");
        }

        private X509Certificate2Collection LoadCertificate(string searchValue)
        {
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            //Load only certificates for which the subject exactly matches
            //the searchvalue parameter value. 
            X509Certificate2Collection cert = 
                store.Certificates.Find(X509FindType.FindBySubjectDistinguishedName, searchValue, false);
            return cert;
        }

        public void ViewMetaDataV2(string filePath)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                byte[] content = new byte[fileStream.Length];
                int length = fileStream.Read(content, 0, content.Length);
                Console.WriteLine("Bytes read: {0}", length);
                Assembly assembly = Assembly.ReflectionOnlyLoad(content);
                Console.WriteLine(assembly.FullName);
                foreach (Type type in assembly.GetTypes())
                    Console.WriteLine(type.Name);
            }
        }

        public void LongExec(int sleep, int maxSleep)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Thread.Sleep(sleep);
            stopwatch.Stop();
            if (stopwatch.ElapsedMilliseconds > maxSleep)
                throw new Exception(string.Format("Execution took to long: {0}", stopwatch.ElapsedMilliseconds));
        }

        private void MethodA()
        {
            int count = 0;
            for(; ; )
            {
                Console.WriteLine("MethodA: " + count++);
                Thread.Sleep(400);
                if (count == 3)
                    break;
            }
        }

        private void MethodB()
        {
            int count = 0;
            for (; ; )
            {
                Console.WriteLine("MethodB: " + count++);
                Thread.Sleep(400);
                if (count == 3)
                    break;
            }
        }

        private void PrintCount(int count)
        {
            Console.WriteLine("Dictionary count: {0}", count);
        }

        public string GenerateHash(string filename, string hashAlgorithm)
        {
            HashAlgorithm hashAlgorithm1;

            try
            {
                hashAlgorithm1 = HashAlgorithm.Create(hashAlgorithm);
                byte[] content = File.ReadAllBytes(filename);
                hashAlgorithm1.ComputeHash(content);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            return BitConverter.ToString(hashAlgorithm1.Hash).Replace("-", "");
        }

        public byte[] ComputeHash(FileStream fileStream, HashAlgorithm hashAlgorithm, byte[] buffer)
        {
            while (true)
            {
                int bytesRead = fileStream.Read(buffer, 0, buffer.Length);
                if (bytesRead == buffer.Length)
                {
                    hashAlgorithm.TransformBlock(buffer, 0, bytesRead, buffer, 0);
                }
                else
                {
                    hashAlgorithm.TransformFinalBlock(buffer, 0, bytesRead);
                    return hashAlgorithm.Hash;
                }
            }
        }

        [Flags]
        public enum Permissions
        {
            Users = 1,
            Supervisors = 2,
            Managers = 4,
            Administrators = 8
        }

        private bool ValidateCustomer(int customerId)
        {
            Thread.Sleep(1000);
            if(customerId % 2 == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<int>GetValidCustomers()
        {
            int[] customers = new int[] { 12,45,67,89,88,66,80 };

            List<int> validCustomers = (from c in customers.AsParallel()
                                        where ValidateCustomer(c)
                                        select c).ToList();
            return validCustomers;
        }

        // Create an array of four cultures.                                 
        private CultureInfo[] cultures =
        {
                CultureInfo.CreateSpecificCulture("de-DE"),
                CultureInfo.CreateSpecificCulture("en-US"),
                CultureInfo.CreateSpecificCulture("es-ES"),
                CultureInfo.CreateSpecificCulture("fr-FR"),
                CultureInfo.CreateSpecificCulture("nl-NL")
        };

        //Return string that includes the players name and number of coins
        //Display number of coins without leading zero if the number is [1-1000] 
        //Display 0 if number of coins is 0
        public string FormatCoins(string name, int coins)
        {
            return string.Format("Player {0}, collected {1} coins", name, coins.ToString("###0"));
        }

        public void Execute(object obj)
        {
            string str = obj as string;

            if (str == null)
            {
                Console.WriteLine("Wrong type.");
                return;
            }

            Console.WriteLine(str);
        }

        public void ProcessFile(Guid dataFileId, string url)
        {
            WebRequest webRequest = WebRequest.Create(url);
            //Enable authentication
            //webRequest.Credentials = CredentialCache.DefaultCredentials; 
            using (WebResponse webResponse = webRequest.GetResponse())
            {
                using (StreamReader streamReader = new StreamReader(webResponse.GetResponseStream()))
                {
                    using (StreamWriter streamWriter = new StreamWriter(@".\"+ dataFileId + ".dat"))
                    {
                        streamWriter.Write(streamReader.ReadToEnd());
                    }
                }
            }
        }

        private static Dictionary<int, WeakReference> _data = new Dictionary<int, WeakReference>(); 

        private void ExtractFile(string sourceFile, string headerFile, string bodyFile)
        {
            //First 20 bytes of file written to header file.  
            //The rest of the sourcefile is written to the bodyFile 
            using (FileStream sourceStream = File.OpenRead(sourceFile))
            {
                using (FileStream headerStream = File.OpenWrite(headerFile))
                {
                    using (FileStream bodyStream = File.OpenWrite(bodyFile))
                    {
                        Trace.WriteLine("SourceStream length: " + sourceStream.Length);
                        byte[] headerContent = new byte[20];
                        byte[] bodyContent = new byte[sourceStream.Length - 20];
                        int count = sourceStream.Read(headerContent, 0, headerContent.Length);
                        Trace.WriteLine("Header read (bytes): " + count);
                        headerStream.Write(headerContent, 0, headerContent.Length);
                        Trace.WriteLine("Header successfully written.");

                        Trace.WriteLine("sourceStream position: " + sourceStream.Position);
                        sourceStream.Seek(20, SeekOrigin.Begin);
                        Trace.WriteLine("sourceStream position after Seek: " + sourceStream.Position);

                        count = sourceStream.Read(bodyContent, 0, bodyContent.Length);
                        Trace.WriteLine("Body read (bytes): " + count);
                        bodyStream.Write(bodyContent, 0, bodyContent.Length);
                        Trace.WriteLine("Body successfully written.");
                    }
                }
            }
        }

        private void PrintType(object obj)
        {
            switch(obj)
            {
                case int intValue:
                    Console.WriteLine("int: {0}", intValue);
                    break;
                case double doubleValue:
                    Console.WriteLine("double: {0:F2}", doubleValue);
                    break;
                case decimal decimalValue:
                    Console.WriteLine("decimal: {0:N2}", decimalValue);
                    break;
                case float floatValue:
                    Console.WriteLine("float: {0:F2}", floatValue);
                    break;
                default:
                    Console.WriteLine("Unknown type.");
                    break;
            }
        }

        private T CastObject<[Programmer(programmer:"Mike")] T>(object obj) where T : class
        {
            // 'null' check comparison
            Console.WriteLine($"'is' constant pattern 'null' check result : { obj is null }");
            Console.WriteLine($"object.ReferenceEquals 'null' check result : { object.ReferenceEquals(obj, null) }");
            Console.WriteLine($"Equality operator (==) 'null' check result : { obj == null }");

            return obj is T ? (T)obj : null;
        }

        private bool ValidateActor(Actor actor)
        {
            Random random = new Random(7);
            int value = random.Next(1, 5);
            Thread.Sleep(value * 100);
            if(actor.City == "Seattle")
            {
                if(actor.ID == 2)
                    Thread.Sleep(2000);

                Thread.Sleep(400);
                return true;
            }
            else
            {
                return false;
            }
        }

        public int Calc(int i, int j)
        {
            if(i == 0)
            {
                return j;
            }
            else
            {
                return Calc(i - 1, i + j);
            }
        }

        public int Calc(int n)
        {
            if (n == 0)
            {
                return 0;
            }
            else if(n == 1)
            {
                return 1;
            }
            else
            {
                return Calc(n - 1) + Calc(n - 2);
            }
        }

        public delegate void AddBookCallback(int i);
        
        //private 
        
        
        public async Task Run()
        {
            //--------------------------------------------------------------------------------------------------------
            // Download Json response from website
            //
            // http://json2csharp.com/  generate c# classes from json
            //--------------------------------------------------------------------------------------------------------
            const string keyToken = "6f1aa5a06770929433faa4911e4334a1";
            string city = "Amsterdam";

            try
            {
                string weatherUrl = @"https://api.openweathermap.org/data/2.5/weather?q="+city+
                    "&units=metric&appid="+ keyToken;
                Console.WriteLine(weatherUrl);
                WebClient client = new WebClient();
                string json = client.DownloadString(weatherUrl);
                Console.WriteLine(json);
                RootObject weather = JsonConvert.DeserializeObject<RootObject>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in WebClient.UploadValues(): {ex.Message}");
                throw;
            }
            
            //--------------------------------------------------------------------------------------------------------
            // Static class, virtual class
            //--------------------------------------------------------------------------------------------------------
            double weightInKilos = 80;
            double weightInPounds = Conversions.KilosToPounds(weightInKilos);
            Console.WriteLine("weightInPounds: {0:N2}", weightInPounds);
            DrinksMachine drinksMachine = new DrinksMachine("John");
            drinksMachine.MakeCappuccino();
            drinksMachine.MakeEspresso();
            
            //--------------------------------------------------------------------------------------------------------
            // How many stars will be printed by the following code; 
            //
            //a) 1
            //b) 2
            //c) 3
            //d) 4
            //e) Code Throws an error 
            //--------------------------------------------------------------------------------------------------------
            int loops = 7;

            while(loops > 0)
            {
                loops -= 3;
                Console.Write("*");
                if(loops <= 2)
                {
                    break;
                }
                else
                {
                    Console.Write("*");
                }
            };
            Console.WriteLine();

            Collection<NetflixWatcher> watchers = new Collection<NetflixWatcher>();
            watchers.Add(new NetflixWatcher("John", "Deer",
                new Collection<Movie>()
                {
                    new Movie(1,"The Stand",new DateTime(2008,4,12),"Thriller",4.50m),
                    new Movie(2,"It",new DateTime(2009,6,11),"Horror",5.50m),
                    new Movie(3,"The Usual Suspects",new DateTime(2010,4,12),"Thriller",6.50m),
                    new Movie(4,"Contagion",new DateTime(2011,3,15),"Thriller",7.50m),
                }));

            watchers.Add(new NetflixWatcher("Deborah", "Brooks",
                new Collection<Movie>()
                {
                    new Movie(5,"Pulp Fiction",new DateTime(2012,5,15),"Thriller",7.50m),
                    new Movie(6,"Interstellar",new DateTime(2013,6,17),"Horror",5.50m),
                    new Movie(7,"The Matrix",new DateTime(2014,7,16),"Thriller",6.50m),
                }));

            foreach (NetflixWatcher watcher in watchers)
                Console.WriteLine(watcher);
            
            //NetflixWatcher that watched a movie with a ReleaseDate of 2010
            List<NetflixWatcher> selectedWatchers = (from watcher in watchers
                                                     where watcher.Movies.Any(m => m.ReleaseDate.Year == 2010)
                                                     select watcher).ToList();

            foreach (NetflixWatcher watcher in selectedWatchers)
                Console.WriteLine(watcher);

            //NetflixWatcher that watched a movie with a ReleaseDate of 2010
            List<NetflixWatcher> selectedWatchers1 = watchers.Where(x => x.Movies
            .Any(y => y.ReleaseDate.Year == 2010)).ToList();

            foreach (NetflixWatcher watcher in selectedWatchers1)
                Console.WriteLine(watcher);

            //--------------------------------------------------------------------------------------------------------
            // What will be the output of the code below
            //
            //a) 13
            //b) 7
            //c) infinite loop
            //d) 17
            //--------------------------------------------------------------------------------------------------------
            Console.WriteLine("Calc: {0}", Calc(4, 7));

            //--------------------------------------------------------------------------------------------------------
            // What will be the output of the code below
            //
            //a) 0 1 2 3
            //b) An exception will be thrown
            //c) 0 1 1 2 3
            //d) 0 1 1 2
            //--------------------------------------------------------------------------------------------------------
            for (int i = 0; i < 4; i++)
                Console.Write(Calc(i) + " ");

            //try
            //{
            //    //Assembly.LoadFile exception: The module was expected to contain an assembly manifest. (Exception from HRESULT: 0x80131018)
            //    Assembly assembly3 = Assembly.LoadFile(@"C:\Program Files (x86)\VCE Exam Simulator Demo\libeay32.dll");
            //    foreach (Type type1 in assembly3.GetTypes())
            //        Console.WriteLine(type1);
            //}
            //catch(Exception ex)
            //{
            //    Console.WriteLine("Assembly.LoadFile exception: {0}", ex.Message);
            //}

            ConcurrentDictionary<string, int> ages = new ConcurrentDictionary<string, int>();
            if (ages.TryAdd("Rob", 21))
                Console.WriteLine("Rob added successfully.");
            Console.WriteLine("Rob's age: {0}", ages["Rob"]);
            // Set Rob's age to 22 if it is 21
            if (ages.TryUpdate("Rob", 22, 21))
                Console.WriteLine("Age updated successfully");
            Console.WriteLine("Rob's new age: {0}", ages["Rob"]);

            int newVal = ages.AddOrUpdate("Rob", 1, (key, value) =>
            {
                Console.WriteLine("Key: {0}, Value: {1}", key, value);
                return value + 1;
            });

            Console.WriteLine("Rob's age updated to: {0}", newVal);
            Console.WriteLine("Rob's new age: {0}", ages["Rob"]);

            if (Attribute.IsDefined(typeof(ProcessManagement), typeof(ProgrammerAttribute)))
            {
                Attribute attribute = Attribute.GetCustomAttribute(typeof(ProcessManagement), typeof(ProgrammerAttribute));
                ProgrammerAttribute programmer = (ProgrammerAttribute)attribute;
                Console.WriteLine("Programmer: {0}", programmer.Programmer);
            }
            
            List<MyPoint> points = new List<MyPoint>()
            {
                new MyPoint(100, 100),
                new MyPoint(200, 200),
                new MyPoint(300, 300)
            };
            
            //You can use while loop to delete objects
            while(points.Count != 0)
            {
                points.RemoveAt(0);
            }

            Console.WriteLine("Points.Count: {0}", points.Count);
            
            CancellationTokenSource cancellationTokenSource1 = new CancellationTokenSource();
            ProcessManagement processManagement = new ProcessManagement();
            processManagement.DegreeOfParallelisme = 10;
            processManagement.SpawnTasks(cancellationTokenSource1.Token);
            Thread.Sleep(10000);
            cancellationTokenSource1.Cancel();
            Console.WriteLine("All tasks are canceled.");
            
            //--------------------------------------------------------------------------------------------------------
            // Linq join      
            //--------------------------------------------------------------------------------------------------------
            Kid magnus = new Kid { FirstName = "Magnus", LastName = "Hedlund" };
            Kid terry = new Kid { FirstName = "Terry", LastName = "Adams" };
            Kid charlotte = new Kid { FirstName = "Charlotte", LastName = "Weiss" };
            Kid arlene = new Kid { FirstName = "Arlene", LastName = "Huff" };
            Kid rui = new Kid { FirstName = "Rui", LastName = "Raposo" };

            TherapyAnimal barley = new TherapyAnimal { Name = "Barley", Owner = terry };
            TherapyAnimal boots = new TherapyAnimal { Name = "Boots", Owner = terry };
            TherapyAnimal whiskers = new TherapyAnimal { Name = "Whiskers", Owner = charlotte };
            TherapyAnimal bluemoon = new TherapyAnimal { Name = "Blue Moon", Owner = rui };
            TherapyAnimal daisy = new TherapyAnimal { Name = "Daisy", Owner = magnus };

            // Create two lists.
            List<Kid> kids = new List<Kid> { magnus, terry, charlotte, arlene, rui };
            List<TherapyAnimal> pets = new List<TherapyAnimal> { barley, boots, whiskers, bluemoon, daisy };

            var animalAndOwner = from aKid in kids
                                 join aPet in pets on aKid equals aPet.Owner
                                 select new { FullName = aKid.FirstName + ", " + aKid.LastName, PetName = aPet.Name };

            foreach (var owner in animalAndOwner)
                Console.WriteLine("FullName: " + owner.FullName + ", Pet: " + owner.PetName);
            
            //--------------------------------------------------------------------------------------------------------
            // Linq group by      
            //--------------------------------------------------------------------------------------------------------
            Company[] companies = new Company[]
            {
                new Company() { ID=1, Name="Accounting", Manager="User1", BuildingID=15 },
                new Company() { ID=2, Name="Sales", Manager="User2", BuildingID=3 },
                new Company() { ID=3, Name="IT", Manager="User3", BuildingID=15 },
                new Company() { ID=4, Name="Marketing", Manager="User4", BuildingID=3 },
            };

            var groups = from company in companies
                         group company by company.BuildingID
                         into dp
                         select new { BuildingID = dp.Key, Companies = dp };

            foreach (var group in groups)
            {
                Console.WriteLine("\tKey: {0}", group.BuildingID);
                foreach (Company company in group.Companies)
                {
                    Console.WriteLine("\t\t{0}", company);
                }
            }
            
            //--------------------------------------------------------------------------------------------------------
            // Parallel LINQ      
            //--------------------------------------------------------------------------------------------------------
            //Language - Integrated Query, or LINQ, is used to perform queries on items of
            //data in C# programs. Parallel Language-Integrated Query (PLINQ) can be used
            //to allow elements of a query to execute in parallel.
            //The AsParallel method examines the query to determine if using a
            //parallel version would speed it up.If it is decided that executing elements of the
            //query in parallel would improve performance, the query is broken down into a
            //number of processes and each is run concurrently. If the AsParallel method
            //can’t decide whether parallelization would improve performance the query is not
            //executed in parallel.
            Actor[] actors = new Actor[] 
            {
                new Actor { ID=1, Name = "Alan", City = "Hull" },
                new Actor { ID=2, Name = "Beryl", City = "Seattle" },
                new Actor { ID=3, Name = "Charles", City = "London" },
                new Actor { ID=4, Name = "David", City = "Seattle" },
                new Actor { ID=5, Name = "Eddy", City = "Paris" },
                new Actor { ID=6, Name = "Fred", City = "Berlin" },
                new Actor { ID=7, Name = "Gordon", City = "Hull" },
                new Actor { ID=8, Name = "Henry", City = "Seattle" },
                new Actor { ID=9, Name = "Isaac", City = "Seattle" },
                new Actor { ID=10, Name = "James", City = "London" },
                new Actor { ID=11, Name = "Wolff", City = "Seattle" }
            };

            ParallelQuery<Actor> selectedActors = from actor in actors.AsParallel()
                                                where ValidateActor(actor)
                                                select actor;

            foreach (Actor selectedActor in selectedActors)
                Console.WriteLine("{0:f}",selectedActor);
            //ID: 8, Name: Henry, City: Seattle
            //ID: 9, Name: Isaac, City: Seattle
            //ID: 11, Name: Wolff, City: Seattle
            //ID: 2, Name: Beryl, City: Seattle
            //ID: 4, Name: David, City: Seattle

            //This call of AsParallel requests that the query be parallelized whether
            //performance is improved or not, with the request that the query be executed on a
            //maximum of four processors. A non-parallel query produces output data that has the same order 
            //as the input data.A parallel query, however, may process data in a different order from the input data.
            ParallelQuery<Actor> selectedActors2 = from actor in actors
                                                     .AsParallel()
                                                     .WithDegreeOfParallelism(4)
                                                     .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                                                 where ValidateActor(actor)
                                                 select actor;

            Console.WriteLine("\nForceParallelism");
            foreach (Actor selectedActor in selectedActors2)
                Console.WriteLine("{0:f}", selectedActor);

            //If it is important that the order of the original data be preserved, the
            //AsOrdered method can be used to request this from the query
            //The AsOrdered method doesn’t prevent the parallelization of the query,
            //instead it organizes the output so that it is in the same order as the original data.
            //This can slow down the query.
            ParallelQuery<Actor> selectedActors3 = from actor in actors
                                                     .AsParallel()
                                                     .AsOrdered() //Keep same ordering as in original array  
                                                     .WithDegreeOfParallelism(4)
                                                     .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                                                 where ValidateActor(actor)
                                                 select actor;

            Console.WriteLine("\nAsOrdered()");
            foreach (Actor selectedActor in selectedActors3)
                Console.WriteLine("{0:f}", selectedActor);
            //AsOrdered()
            //ID: 2, Name: Beryl, City: Seattle
            //ID: 4, Name: David, City: Seattle
            //ID: 8, Name: Henry, City: Seattle
            //ID: 9, Name: Isaac, City: Seattle
            //ID: 11, Name: Wolff, City: Seattle

            //Another issue that can arise is that the parallel nature of a query may remove
            //ordering of a complex query. The AsSequential method can be used to
            //identify parts of a query that must be sequentially executed
            //AsSequential executes the query in order whereas AsOrdered returns a
            //sorted result but does not necessarily run the query in order.
            ParallelQuery<Actor> selectedActors4 = from actor in actors
                                                     .AsParallel()                                         
                                                     .WithDegreeOfParallelism(4)
                                                     .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                                                 where ValidateActor(actor)
                                                 orderby actor.ID descending
                                                 select actor;

            Console.WriteLine("\norderby actor.ID descending");
            foreach (Actor selectedActor in selectedActors4)
                Console.WriteLine("{0:f}", selectedActor);

            //The query in below retrieves the names of the first four people who live
            //in Seattle.The query requests that the result be ordered by person name, and this
            //ordering is preserved by the use of AsSequential before the Take, which
            //removes the four people.If the Take is executed in parallel it can disrupt the
            //ordering of the result.
            IEnumerable<Actor> selectedActors5 = (from actor in actors
                                                     .AsParallel()
                                                     .WithDegreeOfParallelism(4)
                                                     .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                                                 where ValidateActor(actor)
                                                 orderby actor.ID descending
                                                 select actor).AsSequential().Skip(3).Take(2);
            
            Console.WriteLine("\nAsSequential().Skip(3).Take(2)");
            foreach (Actor selectedActor in selectedActors5)
                Console.WriteLine("{0:f}", selectedActor);

            Console.WriteLine("\nIterating query elements using ForAll");
            //The ForAll method can be used to iterate through all of the elements in a
            //query.It differs from the foreach C# construction in that the iteration takes
            //place in parallel and will start before the query is complete
            //The parallel nature of the execution of ForAll means that the order of the
            //printed output above will not reflect the ordering of the input data.
            //IEnumerable<Actor> 
            ParallelQuery<Actor> selectedActors6 = from actor in actors
                                                       .AsParallel()
                                                       .AsOrdered() //Keep same ordering as in original array  
                                                       .WithDegreeOfParallelism(4)
                                                       .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                                                   where ValidateActor(actor)
                                                   select actor;

            selectedActors6.ForAll(actor => Console.WriteLine("{0:f}", actor));
            
            //--------------------------------------------------------------------------------------------------------
            // Using is operator      
            //--------------------------------------------------------------------------------------------------------
            Reporter reporter1 = new Reporter();
            BaseReporter baseReporter = CastObject<BaseReporter>(reporter1);
            baseReporter.LogCompleted();

            PrintType((double)34.78);
            PrintType((float)45.78);
            PrintType((decimal)67.78);
            PrintType((int)90);

            var message = "Hello goodfellas.";
            Console.WriteLine(message.Replace("Hello","Welcome"));

            Reporter reporter = new Reporter();
            reporter.Log("My message.");
            reporter.LogCompleted();

            ((BaseReporter)reporter).Log("My message.");
            ((BaseReporter)reporter).LogCompleted();
            
            try
            {
                ExtractFile("b3a6ce18-ced2-4dfc-b44d-627933c02f9a.dat", "header.dat", "body.dat");
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                throw;
            }
            
            //Sometimes you have to work with large objects that require a lot of time to create. 
            //For example, a list of objects that has to be retrieved from a database.It would be 
            //nice if you can just keep the items in memory; however, that increases the memory 
            //load of your application, and maybe the list won’t be needed any more.But if garbage 
            //collection hasn’t occurred yet, it would be nice if you could just reuse the list you created.
            //This is where the type WeakReference can be used.A WeakReference, as the name suggests, 
            //doesn’t hold a real reference to an item on the heap, so that it can’t be garbage collected. 
            //But when garbage collection hasn’t occurred yet, you can still access the item through the WeakReference.
            _data.Add(1, new WeakReference(new Device() { ID = 5, DeviceType = "Device1" }, false));
            _data.Add(2, new WeakReference(new Device() { ID = 7, DeviceType = "Device2" }, false));
            WeakReference weakReference = _data[1];
            if(weakReference.IsAlive)
            {
                Device device = weakReference.Target as Device;
                Console.WriteLine(device.ID +", "+ device.DeviceType);
            }
            
            ParameterizedThreadStart parameterizedThreadStart = new ParameterizedThreadStart(Execute);
            Thread thread = new Thread(parameterizedThreadStart);
            thread.Start("Message to thread method");
            thread.Join();
            Console.WriteLine("Threads finished.");

            try
            {
                ProcessFile(Guid.NewGuid(), @"https://www.microsoft.com/nl-nl/");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            
            //Some simple exercises
            int[] intArray = new int[] { 1,2,3,4,5 };
            //After adjusting values print: 1,3,6,10,15  

            //Correctly updates values
            //int sum = 0; 
            //for(int i = 0; i < intArray.Length;)
            //{
            //    sum += intArray[i];
            //    intArray[i++] = sum;
            //}

            //Correctly updates values
            for (int i = 1; i < intArray.Length; i++)
            {
                intArray[i] += intArray[i - 1];
            }

            foreach(int val in intArray)
            {
                Console.WriteLine(val);
            }

            Assembly assembly2 = Assembly.Load("MathLibrary, Version=1.1.0.0, Culture=neutral, PublicKeyToken=d03acf30f3169ca7");
            Console.WriteLine(assembly2.Location);
            Type[] types2 = assembly2.GetTypes();
            foreach (Type type1 in types2)
                Console.WriteLine(type1);
            
            string fullNameAss = typeof(MathLibrary.Calculator).Assembly.FullName;
            Console.WriteLine(fullNameAss);

            string sentence2 = "This is a random string.";
            int index1 = sentence2.LastIndexOf("is");//5
            string result1 = sentence2.Substring(0, index1);
            int index2 = sentence2.IndexOf("random");//10
            result1 += sentence2.Substring(index2);
            Console.WriteLine(result1);
            //This random string.
            
            //Instances of the StringReader class implement the TextReader interface.
            //It is a convenient way of getting string input into a program that would like to
            //read its input from a stream.
            string dataString = "Rob Miles\n21\nNext line";
            StringReader stringReader = new StringReader(dataString);
            Console.WriteLine(stringReader.ReadLine());
            Console.WriteLine(stringReader.ReadLine());
            
            //Example of string interning
            string s1 = "hello";
            string s2 = "hello";
            if (ReferenceEquals(s1, s2)) //String interning only happens when the program is compiled.
                Console.WriteLine("s1 and s2 reference same object, string interning (compiletime).");

            //Note (StreamWriter, StreamReader, StringWriter and StringReader) inherit from TextReader and TextWriter     
            StringWriter writer = new StringWriter();
            writer.WriteLine("FirstLine");
            writer.WriteLine();
            writer.WriteLine("SecondLine");
            writer.Close();
            Console.WriteLine(writer);
            
            //--------------------------------------------------------------------------------------------------------
            // Formatting DateTime, double, float, decimal, int     
            //--------------------------------------------------------------------------------------------------------
            Console.WriteLine(FormatCoins("John", 0));
            Console.WriteLine(FormatCoins("John", 1));
            Console.WriteLine(FormatCoins("John", 10));
            Console.WriteLine(FormatCoins("John", 100));
            Console.WriteLine(FormatCoins("David", 999));
            Console.WriteLine(FormatCoins("David", 1000));
            
            // Display the name of the current thread culture.
            Console.WriteLine("CurrentCulture is {0}.", CultureInfo.CurrentCulture.Name); //CurrentCulture is en-US
            Console.WriteLine("{0:G}", DateTime.Now); //3/29/2020 4:18:34 PM
            string myDate = "3/29/2020 4:30:34 PM";
            DateTime dateTime = DateTime.MinValue;
            bool valid = DateTime.TryParse(myDate,
                CultureInfo.CurrentCulture,
                DateTimeStyles.AssumeLocal | DateTimeStyles.AdjustToUniversal, out dateTime);
            
            if (valid)
            {
                Console.WriteLine("Valid date: {0}", dateTime);
            }
            else
            {
                Console.WriteLine("Invalid date");
            }

            decimal value2 = 123.456m;
            Console.WriteLine("{0:C}", value2); //123.46
            Console.WriteLine("{0:C2}", value2); //123.46
            Console.WriteLine("{0:C3}", value2); //123.456
            int value3 = 1234;
            Console.WriteLine("{0:D}", value3); //1234
            Console.WriteLine("{0:D5}", value3); //01234

            double value4 = 1052.1329112756d;
            Console.WriteLine("{0:E}", value4);
            Console.WriteLine("{0:G2}", value4); //standard general with 2 meaningful digits"
            Console.WriteLine("{0:G3}", value4); //standard general with 3 meaningful digits"
            Console.WriteLine("{0:G5}", value4); //standard general with 5 meaningful digits"
            Console.WriteLine("{0:G4}", 123.4546); //standard general with 4 meaningful digits"
            Console.WriteLine("{0:P}", 1); //standard general with 4 meaningful digits"
            Console.WriteLine("{0:P3}", 0.3334353); //standard general with 4 meaningful digits"
            Console.WriteLine("{0:P2}", 0.3334353); //standard general with 4 meaningful digits"
            Console.WriteLine("{0:X4}", 127); //standard general with 4 meaningful digits"
            Console.WriteLine("{0:X4}", 255); //standard general with 4 meaningful digits"
            Console.WriteLine("{0:X2}", 127); //standard general with 4 meaningful digits"
            Console.WriteLine("{0:X2}", 255); //standard general with 4 meaningful digits"

            float value1 = 247.5678f;
            Console.WriteLine(value1.ToString("00.00")); //247.57
            Console.WriteLine(value1.ToString("C2")); //247.57
            Console.WriteLine(value1.ToString("N2"));//247.57
            Console.WriteLine(value1.ToString("###.##"));//247.57
            Console.WriteLine(String.Format("{0:###.#}", value1)); //247.6
            Console.WriteLine(String.Format("{0:###}", value1)); //248
            Console.WriteLine(String.Format("{0:###}", 00045m)); //45
            Console.WriteLine(String.Format("{0:000.00}", 00045m)); //045.00
            Console.WriteLine("{0:F2}", 12f);    // 12.00 - two decimal places
            Console.WriteLine("{0:F0}", 12.3f);  // 12 - ommiting fractions

            //The type of a real literal is determined by its suffix as follows:
            //The literal without suffix or with the d or D suffix is of type double
            //The literal with the f or F suffix is of type float
            //The literal with the m or M suffix is of type decimal
            //There is only one implicit conversion between floating-point numeric types: 
            //from float to double. However, you can convert any floating-point type to any 
            //other floating-point type with the explicit cast. 
            //Format with two decimal places
            Console.WriteLine(String.Format("{0:0.00}", 123.4567m)); //123.46 (Is rounded)
            Console.WriteLine(String.Format("{0:0.00}", 123.4m)); //123.40
            Console.WriteLine(String.Format("{0:0.00}", 123.0m)); //123.00

            //Format with max two decimal places 
            Console.WriteLine(String.Format("{0:0.##}", 123.4567m)); //123.46 (Is rounded)
            Console.WriteLine(String.Format("{0:0.##}", 123.4m));    //123.4
            Console.WriteLine(String.Format("{0:0.##}", 123.0m));    //123
            Console.WriteLine(String.Format("{0:0.##}", 123));       //123

            // at least two digits before decimal point
            Console.WriteLine(String.Format("{0:00.00}", 123.4567m)); //123.46 (Is rounded)
            Console.WriteLine(String.Format("{0:00.00}", 23.4567m));  //23.46
            Console.WriteLine(String.Format("{0:00.00}", 3.4567m));   //03.46
            Console.WriteLine(String.Format("{0:00.00}", -3.4567));   //-03.46  
            
            // Create an array of all supported standard date and time format specifiers.
            string[] formats = {"d", "D", "f", "F", "g", "G", "m", "o", "r", "s", "t", "T", "u", "U", "Y"};

            //d -> Represents the day of the month as a number from 1 through 31.
            //dd -> Represents the day of the month as a number from 01 through 31.
            //ddd-> Represents the abbreviated name of the day (Mon, Tues, Wed, etc).
            //dddd-> Represents the full name of the day (Monday, Tuesday, etc).
            //h-> 12-hour clock hour (e.g. 4).
            //hh-> 12-hour clock, with a leading 0 (e.g. 06)
            //t-> Abbreviated AM / PM (e.g. A or P)
            //tt-> AM / PM (e.g. AM or PM
            //H-> 24-hour clock hour (e.g. 15)
            //HH-> 24-hour clock hour, with a leading 0 (e.g. 22)
            //m-> Minutes
            //mm-> Minutes with a leading zero
            //M-> Month number(eg.3)
            //MM-> Month number with leading zero(eg.04)
            //MMM-> Abbreviated Month Name (e.g. Dec)
            //MMMM-> Full month name (e.g. December)

            String[] formats2 =
            {
                "MM/dd/yyyy",                   //05/29/2015
                "dddd, dd MMMM yyyy",           //Friday, 29 May 2015
                "HH:mm",                        //05:50 HH 24-hour clock hour, with a leading 0 (e.g. 22)
                "HH:mm:ss",                     //05:50:06
                "dddd, dd MMMM yyyy HH:mm:ss",  //Friday, 29 May 2015 05:50:06
                "MM/dd/yyyy HH:mm",             //05/29/2015 05:50
                "MM/dd/yyyy hh:mm tt",          //05/29/2015 05:50 AM
                "MM/dd/yyyy H:mm",              //05/29/2015 5:50
                "MM/dd/yyyy h:mm tt",           //05/29/2015 5:50 AM
                "MM/dd/yyyy HH:mm:ss",          //05/29/2015 05:50:06
                "MMMM dd",                      //May 29
                "hh:mm tt",                     //05:50 AM
                "H:mm",                         //5:50
                "h:mm tt",                      //5:50 AM
                "HH:mm:ss",                     //05:50:06
                "yyyy MMMM",                    //2020 March
                "dddd, MMMM dd yyyy",           //Friday, May 05 2020
                "ddd, MMM d \"'\"yy",           //mon Jan 29 '20
                "dddd, MMMM dd",                //Thursday, August 17
                "M/yy",                         //11/20
                "dd-MM-yy"                      //20-04-20      
            };

            // Define date to be displayed.
            DateTime dateToDisplay = DateTime.Now;
            
            foreach (string formatSpecifier in formats2)
            {
                foreach (CultureInfo culture in cultures)
                    Console.WriteLine("{0} Format Specifier {1, 10} Culture {2, 40}",
                                      formatSpecifier, culture.Name,
                                      dateToDisplay.ToString(formatSpecifier, culture));
                Console.WriteLine();
            }
            
            // Iterate each standard format specifier.
            foreach (string formatSpecifier in formats)
            {
                foreach (CultureInfo culture in cultures)
                    Console.WriteLine("{0} Format Specifier {1, 10} Culture {2, 40}",
                                      formatSpecifier, culture.Name,
                                      dateToDisplay.ToString(formatSpecifier, culture));
                Console.WriteLine();
            }
            
            //--------------------------------------------------------------------------------------------------------
            // IComparable interface  
            //--------------------------------------------------------------------------------------------------------
            List<Device> devices = new List<Device>()
            {
                new Device() { ID = 5, DeviceType = "Device1" },
                new Device() { ID = 6, DeviceType = "Device2" },
                new Device() { ID = 3, DeviceType = "Device3" },
                new Device() { ID = 4, DeviceType = "Device4" },
            };

            try
            {
                IFormatter formatter = new BinaryFormatter();
                using (MemoryStream memoryStream3 = new MemoryStream())
                {
                    using (StreamReader streamReader = new StreamReader(memoryStream3))
                    {
                        formatter.Serialize(memoryStream3, devices);
                        memoryStream3.Position = 0;

                        List<Device> devices1 = formatter.Deserialize(memoryStream3) as List<Device>;
                        foreach (Device device in devices1)
                            Console.WriteLine("Device,  ID: {0}, DeviceType: {1}", device.ID, device.DeviceType);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            
            Console.WriteLine(devices.Count);
            devices.Sort();
            Console.WriteLine(devices[0].DeviceType);
            Console.WriteLine(devices[0].CompareTo(devices[0]));
            
            //--------------------------------------------------------------------------------------------------------
            // XElement
            //--------------------------------------------------------------------------------------------------------
            List<Contact> contactsDB = new List<Contact>()
            {
                new Contact("James", "Brand"),
                new Contact("John", "Wolf"),
                new Contact("Robert", "Turing"),

            };

            List<XElement> contactsFromDB = (from contact in contactsDB
                                             select new XElement("contact", 
                                                 new XAttribute("contactId", contact.ContactId),
                                                 new XElement("firstName", contact.FirstName),
                                                 new XElement("lastName", contact.LastName))).ToList();

            XElement contactsXml = new XElement("contacts", contactsFromDB);

            XNamespace ew = "ContactList";
            XElement root = new XElement(ew + "Root", contactsXml);
            Console.WriteLine(root+"\n");
            /*<Root xmlns="ContactList">
              <contacts xmlns="">
                <contact contactId="b31ed5bb-d44e-4d60-8ab6-f60acbee44d9">
                  <firstName>James</firstName>
                  <lastName>Brand</lastName>
                </contact>
                <contact contactId="daf9f1e0-3cc9-4b45-88a3-c6a1f046660f">
                  <firstName>John</firstName>
                  <lastName>Wolf</lastName>
                </contact>
                <contact contactId="7c67713f-128b-4069-970f-96bd871b16b5">
                  <firstName>Robert</firstName>
                  <lastName>Turing</lastName>
                </contact>
              </contacts>
            </Root>*/

            XElement contacts = new XElement("Contacts",
                new XElement("Contact",
                    new XElement("Name", "Patrick Hines"),
                    new XElement("Phone", "206-555-0144"),
                    new XElement("Address",
                        new XElement("Street1", "123 Main St"),
                        new XElement("City", "Mercer Island"),
                        new XElement("Postal", "68042"),
                        new XElement("Salary", 324.50))),
                new XElement("Contact",
                    new XElement("Name", "Patrick Hines"),
                    new XElement("Phone", "206-555-0144"),
                    new XElement("Address",
                        new XElement("Street1", "_123 Main St"),
                        new XElement("City", "_Mercer Island"),
                        new XElement("Postal", "_68042"),
                        new XElement("Salary", 324.50))));

            Console.WriteLine(contacts.ToString());

            IEnumerable<XElement> selectedContacts = from contact in contacts.Descendants("Address") select contact;

            foreach(XElement element in selectedContacts)
            {
                Console.WriteLine(element.Element("Street1").Value);
                Console.WriteLine(element.Element("City").Value);
                Console.WriteLine(element.Element("Postal").Value);
                Console.WriteLine(element.Element("Salary").Value);
            }

            /*<Contacts>  
              <Contact>  
                <Name>Patrick Hines</Name>  
                <Phone>206-555-0144</Phone>  
                <Address>  
                  <Street1>123 Main St</Street1>  
                  <City>Mercer Island</City>  
                  <State>WA</State>  
                  <Postal>68042</Postal>  
                </Address>  
              </Contact>  
            </Contacts>*/
            
            XElement address = new XElement("Address",
                new XElement("Street1", "123 Main St"),
                new XElement("City", "Mercer Island"),
                new XElement("State", "WA"),
                new XElement("Postal", "68042"));

            Console.WriteLine(address.ToString());

            /*<Address>  
              <Street1>123 Main St</Street1>  
              <City>Mercer Island</City>  
              <State>WA</State>  
              <Postal>68042</Postal>  
            </Address> */

            XElement MusicTracks = new XElement("MusicTracks",
                new List<XElement>
                {
                    new XElement("MusicTrack", new XElement("Artist", "Rob Miles"), new XElement("Title", "My Way")),
                    new XElement("MusicTrack", new XElement("Artist", "Immy Brown"), new XElement("Title", "Her Way"))
                });

                /*
                <MusicTracks>
                  <MusicTrack>
                    <Artist>Rob Miles</Artist>
                    <Title>My Way</Title>
                  </MusicTrack>
                  <MusicTrack>
                    <Artist>Immy Brown</Artist>
                    <Title>Her Way</Title>
                  </MusicTrack>
                </MusicTracks>
                */

            Console.WriteLine(MusicTracks.ToString());
            
            foreach (int i in GetValidCustomers())
                Console.WriteLine(i);
            
            Friend friend = new Friend() { FirstName = "John" }; //, LastName = "Connor" };

            DataContractSerializer dataContractSerializer2 = new DataContractSerializer(friend.GetType());
            using (MemoryStream memoryStream2 = new MemoryStream())
            {
                using (StreamReader streamReader2 = new StreamReader(memoryStream2))
                {
                    dataContractSerializer2.WriteObject(memoryStream2, friend);
                    memoryStream2.Position = 0;
                    Console.WriteLine(streamReader2.ReadToEnd());
                }
            }
                
            Permissions UserGroup = Permissions.Supervisors;
            Console.WriteLine("UserGroup < Permissions.Administrators: {0}", UserGroup < Permissions.Administrators);

            Permissions permissions = Permissions.Managers | Permissions.Administrators;

            if ((permissions & Permissions.Managers) == Permissions.Managers)
                Console.WriteLine("Managers flag is set");

            if ((permissions & Permissions.Administrators) == Permissions.Administrators)
                Console.WriteLine("Administrators flag is set");

            //Reset the Administrators flag 
            permissions &= ~Permissions.Administrators;

            Console.WriteLine("Reset Administrators\n");

            if ((permissions & Permissions.Managers) == Permissions.Managers)
                Console.WriteLine("Managers flag is set");

            if ((permissions & Permissions.Administrators) == Permissions.Administrators)
                Console.WriteLine("Administrators flag is set");

            Console.WriteLine("Administrators Set again\n");

            permissions |= Permissions.Administrators;

            if ((permissions & Permissions.Managers) == Permissions.Managers)
                Console.WriteLine("Managers flag is set");

            if ((permissions & Permissions.Administrators) == Permissions.Administrators)
                Console.WriteLine("Administrators flag is set");

            Identity identity = new Identity(67, "John");
            Identity identity1 = new Identity(72, "David");
            Identity identity2 = new Identity(67, "John");

            Console.WriteLine("identity == identity: {0}", identity.Equals(identity));
            Console.WriteLine("identity == identity1: {0}", identity.Equals(identity1));
            Console.WriteLine("identity == identity2: {0}", identity.Equals(identity2));
            
            //CBE35DC4F5F4980645C3656E9E1146076B1294445C9704DE68B4971B93946942 (HASH VALUE)
            const string pdfFile = @"C:\Users\moham\Desktop\Notes\exercises.pdf";
            string hash = GenerateHash(pdfFile, "SHA256");
            Console.WriteLine(hash);
            using (FileStream fileStream = new FileStream(pdfFile, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[4096];
                HashAlgorithm hashAlgorithm = HashAlgorithm.Create("SHA256");
                byte[] content = ComputeHash(fileStream, hashAlgorithm, buffer);
                Console.WriteLine(BitConverter.ToString(content).Replace("-", ""));
            }
            
            Poker poker = new Poker();
            poker.Add("Player1", 20, (count) =>
            {
                Console.WriteLine("Dictionary count: {0}", count);
            });

            poker.Add("Player2", 34, delegate (int count)
            {
                Console.WriteLine("Dictionary count: {0}", count);
            });

            poker.Add("Player3", 65, PrintCount);

            Console.WriteLine("Player2 count: {0}", poker["Player2"]);
            poker["Player2"] = 79;
            Console.WriteLine("Player2 count: {0}", poker["Player2"]);
            
            AddBookCallback addBookCallback = (i) =>
            {
                Console.WriteLine("AddBookCallback: {0}",i);
            };

            AddBookCallback addBookCallback1 = delegate (int i)
            {
                Console.WriteLine("addBookCallback1: {0}", i);
            };

            addBookCallback(3);
            addBookCallback1(1);

            Place place = new Place() { Label = "Amsterdam", PlaceDirection = Direction.North };
            //Serialize as XML
            DataContractSerializer dataContractSerializer1 = new DataContractSerializer(typeof(Place));
            MemoryStream memoryStream1 = new MemoryStream();
            StreamReader streamReader1 = new StreamReader(memoryStream1);
            dataContractSerializer1.WriteObject(memoryStream1, place);
            memoryStream1.Position = 0;
            Console.WriteLine(streamReader1.ReadToEnd());
            
            Task[] tasks = new Task[]
            {
                Task.Factory.StartNew(()=>MethodA()),
                Task.Factory.StartNew(()=>MethodB())
            };

            await Task.WhenAll(tasks);
            Console.WriteLine("Tasks are finished.");
            
            //The outer level CodeCompileUnit instance is created first. This serves as a container for
            //CodeNamespace objects that can be added to it.A CodeNameSpace can
            //contain a number of CodeTypeDeclarations and a class is a kind of CodeTypeDeclaration.
            CodeCompileUnit compileUnit = new CodeCompileUnit();
            // Create a namespace to hold the types we are going to create
            CodeNamespace personnelNameSpace = new CodeNamespace("Personnel");
            // Import the system namespace
            personnelNameSpace.Imports.Add(new CodeNamespaceImport("System"));
            // Create a Person class
            CodeTypeDeclaration personClass = new CodeTypeDeclaration("Person");
            personClass.IsClass = true;
            personClass.TypeAttributes = System.Reflection.TypeAttributes.Public;

            // Create a field to hold the name of a person
            CodeMemberField nameField = new CodeMemberField(typeof(string), "name");
            nameField.Attributes = MemberAttributes.Private;
            // Add the name field to the Person class
            personClass.Members.Add(nameField);

            // Add the Person class to personnelNamespace
            personnelNameSpace.Types.Add(personClass);

            // Add the namespace to the document
            compileUnit.Namespaces.Add(personnelNameSpace);
            //Once the CodeDOM object has been created you can create a
            //CodeDomProvider to parse the code document and produce the program code
            //from it.The code here shows how this works.It sends the program code to a
            //string and then displays the string.

            // Now we need to send our document somewhere
            // Create a provider to parse the document
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            // Give the provider somewhere to send the parsed output
            StringWriter s = new StringWriter();
            // Set some options for the parse - we can use the defaults
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            // Generate the C# source from the CodeDOM
            provider.GenerateCodeFromCompileUnit(compileUnit, s, options);
            // Close the output stream
            s.Close();
            // Print the C# output
            Console.WriteLine(s.ToString());
            
            //--------------------------------------------------------------------------------------------------------
            // XElement, XDocument
            //--------------------------------------------------------------------------------------------------------
            string XMLText =
            "<MusicTracks> " +
                "<MusicTrack> " +
                    "<Artist>Rob Miles</Artist> " +
                    "<Title>My Way</Title> " +
                    "<Length>150</Length>" +
                "</MusicTrack>" +
                "<MusicTrack>" +
                    "<Artist>Immy Brown</Artist> " +
                    "<Title>Her Way</Title> " +
                    "<Length>200</Length>" +
                "</MusicTrack>" +
            "</MusicTracks>";

            //The XMLDocument class has been
            //superseded in later versions of.NET(version 3.5 onwards) by the XDocument
            //class, which allows the use of LINQ queries to parse XML files.
            //A program can create an XDocument instance that represents the earlier
            //document by using the Parse method provided by the XDocument class as shown here.
            XDocument musicTracksDocument = XDocument.Parse(XMLText);

            //The format of LINQ queries is slightly different when working with XML.
            //This is because the source of the query is a filtered set of XML entries from the
            //source document. The query selects all the
            //“MusicTrack” elements from the source document.The result of the query is an
            //enumeration of XElement items that have been extracted from the document.
            //The XElement class is a development of the XMLElement class that includes
            //XML behaviors.The program uses a foreach construction to work through the
            //collection of XElement results, extracting the required text values.
            IEnumerable<XElement> selectedTracks =
                from track in musicTracksDocument.Descendants("MusicTrack") select track;

            int songLength = default(int);
            foreach (XElement item in selectedTracks)
            {
                if (!int.TryParse(item.Element("Length").Value, out songLength))
                    songLength = -1;

                Console.WriteLine("Artist:{0} Title:{1} Length:{2}",
                    item.Element("Artist").FirstNode,
                    item.Element("Title").FirstNode,
                    songLength);
            }
            
            try
            {
                LongExec(2000, 1000);
            }
            catch(ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                using (XmlWriterTraceListener xmlWriterTraceListener = new XmlWriterTraceListener(@".\Exceptions.xml"))
                {
                    xmlWriterTraceListener.TraceEvent(new TraceEventCache(),
                        "My application",
                        TraceEventType.Error,
                        ex.HResult,
                        ex.Message);
                }
            }

            //Reflection
            ViewMetaDataV2(@"C:\Users\moham\source\repos\CodeSnippets\CodeSnippets\MathLibrary\bin\Debug\MathLibrary.dll");            
            //--------------------------------------------------------------------------------------------------------
            // Your computer contains a list of certification authorities and also provides a certificate store.
            //--------------------------------------------------------------------------------------------------------
            X509Certificate2Collection certs = LoadCertificate("CN=localhost");
            foreach (X509Certificate2 certificate in certs)
            {
                Console.WriteLine("FriendlyName: {0}, Issuer: {1}, Subject: {2}, Start: {3}, End: {4}",
                    certificate.FriendlyName,
                    certificate.Issuer,
                    certificate.Subject,
                    certificate.NotBefore,
                    certificate.NotAfter);
            }

            X509Store store = new X509Store(StoreLocation.LocalMachine);//StoreLocation.CurrentUser); //StoreLocation.LocalMachine| 
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            foreach (X509Certificate2 certificate in store.Certificates)
            {
                Console.WriteLine("FriendlyName: {0}, Issuer: {1}, Subject: {2}, Start: {3}, End: {4}", 
                    certificate.FriendlyName,
                    certificate.Issuer,
                    certificate.Subject,
                    certificate.NotBefore,
                    certificate.NotAfter);
            }
            
            //--------------------------------------------------------------------------------------------------------
            // Write to Application EventLog when Source does not exits
            //--------------------------------------------------------------------------------------------------------
            EventLog eventLog1 = new EventLog();
            eventLog1.Source = "DoesNotExist";
            eventLog1.WriteEntry("Message 27-03-2020", EventLogEntryType.Information);
            Trace.WriteLine("Finished writing to EventLog.");
            
            //--------------------------------------------------------------------------------------------------------
            // An application serializes and deserializes XML from streams. The XML streams are in the following format:
            //<Name xmlns="http://www.contoso.com/2012/06">
            //  < LastName > Jones </ LastName >
            //  < FirstName > David </ FirstName >
            //</ Name >
            //
            //The application reads the XML streams by using a DataContractSerializer object that is declared by
            //the following code segment:
            //var ser = new DataContractSerializer(typeof(Name));
            // You need to ensure that the application preserves the element ordering as provided in the XML stream.
            //--------------------------------------------------------------------------------------------------------
            FullName fullName = new FullName() { FirstName = "Jones", LastName = "David", OrderDate=DateTime.Now  };
            DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(FullName));
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (StreamReader streamReader = new StreamReader(memoryStream))
                {
                    dataContractSerializer.WriteObject(memoryStream, fullName);
                    memoryStream.Position = 0;
                    string xml = streamReader.ReadToEnd();
                    Console.WriteLine(xml);
                }
            }

            //--------------------------------------------------------------------------------------------------------
            //You are developing an application that includes methods named EvaluateLoan, ProcessLoan, and
            //FundLoan.The application defines build configurations named TRIAL, BASIC, and ADVANCED.
            //You have the following requirements:
            //• The TRIAL build configuration must run only the EvaluateLoan() method.
            //• The BASIC build configuration must run all three methods.
            //• The ADVANCED build configuration must run only the EvaluateLoan() and ProcessLoan() methods.
            //You need to meet the requirements.
            //--------------------------------------------------------------------------------------------------------
#if (TRIAL)
            EvaluateLoan();
#elif BASIC
            EvaluateLoan();
            ProcessLoan();
            FundLoan();
#else //ADVANCED
            EvaluateLoan();
            ProcessLoan();
#endif

            //--------------------------------------------------------------------------------------------------------
            // Use WebClient to download picture 
            //--------------------------------------------------------------------------------------------------------
            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFile(
                    new Uri(@"https://upload.wikimedia.org/wikipedia/commons/e/ef/Great_tit_side-on.jpg"),
                    "bird.jpg");
                Trace.TraceInformation("Picture has been downloaded.");
            }

            //--------------------------------------------------------------------------------------------------------
            // Task example 
            //--------------------------------------------------------------------------------------------------------
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            Task<long> task = Task.Factory.StartNew<long>(() =>
            {
                return StopWatch(cancellationTokenSource.Token);
            }, cancellationTokenSource.Token);

            Console.WriteLine("Press any key to stop StopWatch.");
            Console.ReadLine();
            //if (Console.KeyAvailable)
            //{
            cancellationTokenSource.Cancel();
            long ticks = await task;
            Console.WriteLine("Timer stopped at: {0}", ticks);
            //}
            
            //--------------------------------------------------------------------------------------------------------
            // Json serializer 
            //--------------------------------------------------------------------------------------------------------
            Pet pet = new Pet() { FirstName = "Donald", LastName = "Duck", Values = new int[] { 0, 1, 2 } };
            //There are three Json Serializers: JavaScriptSerializer, JsonConvert, DataContractJsonSerializer 
            string petJson = JsonConvert.SerializeObject(pet);
            Console.WriteLine(petJson);
            Pet pet1 = JsonConvert.DeserializeObject<Pet>(petJson);
            Console.WriteLine(pet1.FirstName +", "+ pet1.LastName+", "+ pet1.Values[2]);

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            petJson = javaScriptSerializer.Serialize(pet);
            Console.WriteLine(petJson);
            pet1 = javaScriptSerializer.Deserialize<Pet>(petJson);
            Console.WriteLine(pet1.FirstName + ", " + pet1.LastName + ", " + pet1.Values[2]);

            ArrayList arrayList = new ArrayList();
            int var1 = 10;
            int var2;
            arrayList.Add(var1);
            var2 = (int)arrayList[0];
            Console.WriteLine(var2);
            //We can also use Convert.ToInt32
            var2 = Convert.ToInt32(arrayList[0]);
            Console.WriteLine(var2);

            const string reallyLongString = "AlfaBetaGammaDelta";
            StringBuilder sb = new StringBuilder(reallyLongString);
            //     The zero-based index position of the value parameter from the start of the current
            //     instance if that string is found, or -1 if it is not. If value is System.String.Empty,
            //     the return value is startIndex.
            if(sb.ToString().IndexOf("Gamma") >= 0)
            {
                Console.WriteLine("String found.");
            }
            else
            {
                Console.WriteLine("String NOT found.");
            }
            
            //SortedList Class is comparable with Dictionary
            //https://docs.microsoft.com/en-us/dotnet/api/system.collections.sortedlist?view=netframework-4.8
            //Represents a collection of key/value pairs that are sorted by the keys and are accessible by key and by index.
            //The following code example shows how to create and initialize a SortedList object and how to print out its keys and values.
            // Creates and initializes a new SortedList. It is currently recommended to use Strong typed SortedList.  
            SortedList<string, string> mySL = new SortedList<string, string>();
            mySL.Add("Third", "!");
            mySL.Add("Second", "World");
            mySL.Add("First", "Hello");
            Console.WriteLine("SortedList count: {0}", mySL.Count);
            Console.WriteLine("Second item: {0}", mySL["Second"]);
            Console.WriteLine("  Keys and Values:");
            foreach (KeyValuePair<string, string> item in mySL)
                Console.WriteLine("Key: {0}, Value: {1}", item.Key, item.Value);
            
            string urls1 = "https://www.beurs.comhttps://edition.cnn.comhttps://www.bbc.com/";
            List<string> list2 = GetValidUrls(urls1);
            foreach (string url in list2)
                Console.WriteLine(url);

            Console.WriteLine();

            list2 = GetValidUrlsV1(urls1);
            foreach (string url in list2)
                Console.WriteLine(url);

            //--------------------------------------------------------------------------------------------------------
            // Implementing explicit and implicit interfaces  
            //--------------------------------------------------------------------------------------------------------
            TempControl tempControl = new TempControl();
            tempControl.Temp();
            ((ICelsius)tempControl).Temp();
            ((IFahrenheit)tempControl).Temp();

            Product product = new Product() { CategoryID = 2, Name = "Banana" };
            string name = product.GetType().GetProperties().First(info => info.Name == "Name")
                .GetValue(product).ToString();
            Console.WriteLine(name);
            
            //--------------------------------------------------------------------------------------------------------
            // Linq example
            //--------------------------------------------------------------------------------------------------------
            List<string> vehicles = new List<string> { "Car", "Boat", "Bike", "Airplane" };

            IEnumerable<string> selectedVehicles = from vehicle in vehicles
                                                   where vehicle.StartsWith("B")
                                                   orderby vehicle ascending
                                                   select vehicle;

            foreach (string vehicle in selectedVehicles)
                Console.WriteLine(vehicle);

            Console.WriteLine();
            //Linq uses deffered execution
            vehicles.Add("Bandana");

            foreach (string vehicle in selectedVehicles)
                Console.WriteLine(vehicle);

            //LinkedList<T> Class, Represents a doubly linked list.
            //https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.linkedlist-1?view=netframework-4.8
            LinkedList<string> sentence = new LinkedList<string>();
            sentence.AddLast("the");
            sentence.AddLast("fox");
            sentence.AddLast("jumps");
            sentence.AddLast("over");
            sentence.AddLast("the");
            sentence.AddLast("dog");
            // Add the word 'today' to the beginning of the linked list.
            sentence.AddFirst("today");
            foreach (string word in sentence)
                Console.WriteLine(word);
                        
            //An assembly with a strong name consists of the simple text name of the assembly, 
            //its version number, and culture information. It also contains a public key and a digital signature.
            //By enabling the signing of the assembly, you can let Visual Studio generate a new key file, 
            //which is then added to your project and is used in the compilation step to strongly sign 
            //your assembly. A strong-named assembly can reference only other assemblies that are also 
            //strongly named.This is to avoid security flaws where a depending assembly could be changed 
            //to influence the behavior of a strong-named assembly.
            //Within an organization, it’s important to secure the private key.If all employees have access
            //to the private key, someone might leak or steal the key.They are then able to distribute
            //assemblies that look legitimate.But without access to the private key, developers can’t sign
            //the assembly and use it while building the application.
            //To avoid this problem, you can use a feature called delayed or partial signing.When using
            //delayed signing, you use only the public key to sign an assembly and you delay using the private
            //key until the project is ready for deployment.
            //Assemblies that are local to an application are called private assemblies. 
            //You can easily deploy an application that depends on private assemblies 
            //by copying it to a new location.
            //When referencing a shared assembly from your project, you can add a reference to the 
            //file located in the GAC or to a local copy of it.When Visual Studio detects that there 
            //is a GAC version of the DLL you are referencing, it will add a reference to the GAC, 
            //not to the local version.

            //The output from this program gives the fully qualified name from the strongly
            //named MusicStorage assembly as shown below.Note that assemblies
            //without a strong-name have a PublicKeyToken value of null.
            string assemblyName = typeof(MathLibrary.Calculator).Assembly.FullName;
            //MathLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
            //After signing
            //MathLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d03acf30f3169ca7
            Console.WriteLine(assemblyName);

            string assemblyName2 = typeof(MusicStorage.MusicTrack).Assembly.FullName;
            //MusicStorage, Version=1.2.3.5, Culture=neutral, PublicKeyToken=d03acf30f3169ca7
            Console.WriteLine(assemblyName2);

            //The exe references libraries
            //.assembly extern MathLibrary
            //{
            //  .publickeytoken = (D0 3A CF 30 F3 16 9C A7 )                         // .:.0....
            //  .ver 1:0:0:0
            //}
            //.assembly extern MusicStorage
            //{
            //  .publickeytoken = (D0 3A CF 30 F3 16 9C A7 )                         // .:.0....
            //  .ver 1:2:3:5
            //}

            //--------------------------------------------------------------------------------------------------------
            // Once the assembly is loaded, the code must be able to read the assembly metadata, 
            // but the code must be denied access from executing code from the assembly.
            // https://docs.microsoft.com/en-us/dotnet/framework/app-domains/how-to-load-assemblies-into-an-application-domain
            //--------------------------------------------------------------------------------------------------------
            await ViewMetaData(@"C:\Users\moham\source\repos\CodeSnippets\CodeSnippets\MathLibrary\bin\Debug\MathLibrary.dll");
                        
            //--------------------------------------------------------------------------------------------------------
            // You need to access the assembly found in the file named MathLibrary.dll.
            // Assembly.LoadFile - Loads the contents of an assembly file on the specified path.
            // http://msdn.microsoft.com/en-us/library/b61s44e8.aspx
            //--------------------------------------------------------------------------------------------------------
            Assembly assembly1 = Assembly.LoadFile(@"C:\Users\moham\source\repos\CodeSnippets\CodeSnippets\MathLibrary\bin\Debug\MathLibrary.dll");
            Type[] types = assembly1.GetTypes();
            foreach(Type t in types)
            {
                Console.WriteLine(t);
            }

            Type type = types[0];
            object calculator = Activator.CreateInstance(type);
            object result  = type.InvokeMember("Add", 
                BindingFlags.Default | BindingFlags.InvokeMethod, 
                null, calculator, new object[] { 7, 9 });
            Console.WriteLine("7 + 9 = {0}", (int)result);

            return;
            if (!EventLog.SourceExists("MyAppSource"))
            {
                EventLog.CreateEventSource("MyAppSource", "Application");
                Trace.WriteLine("MyAppSource created.");
                Thread.Sleep(1000);
            }

            if(EventLog.SourceExists("MyAppSource"))
            {
                EventLog eventLog = new EventLog();
                eventLog.Source = "MyAppSource";
                eventLog.WriteEntry("Message to logfile 23-03-2020.");

                //EventLog eventLog2 = new EventLog("Application");
                EventLogEntryCollection entryCollection = eventLog.Entries;
                int count = entryCollection.Count;

                for(int i = (count - 5); i < count; i++ )
                {
                    Console.WriteLine("Message: {0}, Time: {1}", 
                        entryCollection[i].Message,
                        entryCollection[i].TimeWritten);
                }
            }

            class2 class2 = new class2();
            ((INewInterface)class2).Method1();

            class1 @class = new class1();
            INewInterface newInterface = @class;
            newInterface.Method1();

            Connection connection = Connection.Create("Alfa");
            Console.WriteLine(connection);

            ConnectionV2 connectionV2 = new ConnectionV2("Beta");
            Console.WriteLine(connectionV2);

            string urls = "https://www.beurs.comhttps://edition.cnn.comhttps://www.bbc.com/";
            List<string> list = await TestIfWebSite(urls);
            foreach (string url in list)
                Console.WriteLine(url);

            //List<string> animals = new List<string>
            //{
            //    "Dog", "Cat", "Cheetah", "Giraffe", "Chicken", "Duck"
            //};

            string[] animals = new string[]
            {
                "Dog", "Cat", "Cheetah", "Giraffe", "Chicken", "Duck"
            };

            TabDelimitedFormatter<string> tabDelimitedFormatter =
                new TabDelimitedFormatter<string>();

            IEnumerator<string> enumerator = animals.ToList().GetEnumerator();
            string str = tabDelimitedFormatter.GetOutput(enumerator, 0);
            Console.WriteLine(str);
#if DEBUG
            Console.WriteLine("DEBUG");
#elif SYSTEM64BIT
            Console.WriteLine("SYSTEM64BIT");
#elif (SYSTEM32BIT)
            Console.WriteLine("SYSTEM32BIT");
#endif
            //--------------------------------------------------------------------------------------------------------
            // Example of performance counter
            //--------------------------------------------------------------------------------------------------------
            const string categoryName = "Contose";

            if (!PerformanceCounterCategory.Exists(categoryName))
            {
                CounterCreationDataCollection counterCreationDataCollection = new
                     CounterCreationDataCollection()
                {
                    new CounterCreationData("IOT connections/Sec.",
                        "The number of Iot connections.",
                        PerformanceCounterType.RateOfCountsPerSecond64)
                };

                PerformanceCounterCategory.Create(categoryName,
                    "Contose demo example",
                    PerformanceCounterCategoryType.SingleInstance,
                    counterCreationDataCollection);
                Trace.TraceInformation("PerformanceCounter created.");
                System.Threading.Thread.Sleep(1000);
            }

            if (PerformanceCounterCategory.Exists(categoryName))
            {
                PerformanceCounter performanceCounter =
                    new PerformanceCounter(categoryName, "IOT connections/Sec.", false);

                long counter = 0;
                for (; ; )
                {
                    counter++;
                    performanceCounter.Increment();
                    System.Threading.Thread.Sleep(50);
                    if (counter % 20 == 0)
                        Console.WriteLine("Speed: {0}", performanceCounter.NextValue());

                    if (Console.KeyAvailable)
                        break;
                }
            }

            //When compiling your programs, you have the option of creating an extra file with the extension.pdb.
            //This file is called a program database(PDB) file, which is an extra data source that annotates your 
            //application’s code with additional information that can be useful during debugging.
            //You can construct the compiler to create a PDB file by specifying the / debug:full 
            //or / debug:pdbonly switches. When you specify the full flag, a PDB file is created, 
            //and the specific assembly has debug information.With the pdbonly flag, the generated 
            //assembly is not modified, and only the PDB file is generated.The latter option is recommended 
            //when you are doing a release build.
            //DEBUG mode, the VS compiler uses / debug:full option  
            //Release mode , the VS compiler uses debug:pdbonly 
            //A.NET PDB file contains two pieces of information:
            //=> Source file names and their lines
            //=> Local variable names
            Trace.WriteLine("Trace before, CalcMethod()");
            Debug.WriteLine("Before, CalcMethod()");
            CalcMethod();
            Debug.WriteLine("After, CalcMethod()");

            //As you can see, the debugger knows that you are currently in the Main method of your application. 
            //All other code, however, is seen as External Code.
            //Microsoft has helpfully published its PDB files to its Symbol Server, which is a way to expose the PDB 
            //files of applications to the debugger so it can easily find the files.The Symbol Server also helps the 
            //debugger handle the different versions of PDB files so that it knows how to find the matching version 
            //for each build. If you want to use the Microsoft Symbol Server, you first need to turn off the Enable 
            //Just My Code option(you can find this option in Tools → Options → Debugging → General).Tell the debugger 
            //where to find the Microsoft symbol files.You can do this in the same Options section by selecting Symbols 
            //and then selecting the Microsoft Symbol Servers option. When you now start debugging, the debugger 
            //will download the PDB files from the Microsoft Symbol Server. If you look at the Modules window, 
            //you will see that all the modules have their symbols loaded.You will also see that the Call Stack 
            //window shows a lot more information than it did previously

            //When building your own projects, it’s important to set up a Symbol Server for your internal
            //use.The easiest way to do this is to use Team Foundation Server(TFS) to manage your source
            //code and builds.TFS has an option to publish all the PDB files from your builds to a shared
            //location, which can then act as a Symbol Server for Visual Studio, enabling you to debug all
            //previous versions of an application without having the source code around.
            try
            {
                int value = 10;
                Trace.Assert(value > 9);
            }
            catch (Exception ex)
            {
                Console.WriteLine("##" + ex.Message);
            }

            //Use DebuggerStepThrough to prevent debugger stepping
            //You can use the[DebuggerStepThrough] attribute to mark a method or
            //class so that the debugger will not debug each statement in turn when single
            //stepping through the code in that method or class. This is also useful when a
            //program contains programmatically generated elements that you don’t want to
            //step through.The attribute is used before the declaration of the element:
            //Note that if you apply the attribute to a class, it will mean that the debugger
            //will not step through any of the methods in the class. Note also that the program
            //will not hit any breakpoints that are set in elements marked with this attribute.
            UpdateV1();
            Update();

            //Another use for the
            //#line directive is to hide code statements from the
            //debugger. This can be very useful if your source code contains programmatically
            //generated elements. You don’t want to have to step through all the
            //programmatically generated statements when debugging, because you are only
            //interested in the code that you have written yourself.
            //In the code next the program will step past the WriteLine statement
            //because it is in a sequence of statements after a
            //#line hidden statement. The
            //stepping will resume after the
            //#line default statement, which resumes line
            //numbering and file name reporting.
            Console.WriteLine("Programming start.");
#line hidden
            // The debugger will not step through these statements
            Console.WriteLine("You haven't seen me");
#line default
            Console.WriteLine("Programming Ending.");

            //Another attribute that can be useful when debugging is DebuggerDisplayAttribute. 
            //By default, the debugger in Visual Studio calls ToString on each object that you 
            //want to inspect for a value. For simple objects, such as ints or strings, this is 
            //no problem because they have an overload for ToString that displays their value. 
            //But for types that don’t override ToString, the default implementation shows the 
            //name of the type, which is not useful when debugging. Of course, you can start 
            //overriding all ToString methods and give them an implementation that’s useful for 
            //debugging purposes, but that implementation will also show up in your release build.
            //As an alternative, you can use the DebuggerDisplayAttribute found in the System.Diagnostics 
            //namespace. This attribute is used by the Visual Studio debugger to display an object. 
            //Listing 3-43 shows an example.            
            try
            {
                Exploder();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

            Animal animal = new Animal() { FirstName = "Donald", LastName = "Duck" };
            Assembly assembly = LoadAssembly<int>();
            Console.WriteLine(assembly.FullName);
            Logline("Logline, log message.");

#line 200 "OtherFileName"
            int a; // line 200
#line default
            int b; // line 4
#line hidden
            int c; // hidden
            int d; // line 7

            Logline("Logline, log message.");

#if !DEBUG && LOGGING
#error "You can activate LOGGING only in DEBUG mode"            
#endif

#if (DEBUG)
            Console.WriteLine("Debug Mode");
#else
            Console.WriteLine("Release Mode");
#endif
        }

        public Assembly LoadAssembly<T>()
        {
#if !WINRT
            Assembly assembly = typeof(T).Assembly;
#else
            Assembly assembly = typeof(T).GetTypeInfo().Assembly;
#endif
            return assembly;
        }

        [Conditional("DEBUG")]
        [Obsolete("This method is obsolete. Call NewMethod instead.", false)]
        public void Logline(string str)
        {
            Console.WriteLine(str);
        }

        static void Exploder()
        {
#line 1 "kapow.ninja"
            throw new Exception("Bang");
#line default
        }

        [DebuggerStepThrough]
        public void Update()
        {
            Console.WriteLine("public void Update()");
        }

        public void UpdateV1()
        {
            Console.WriteLine("public void UpdateV1()");
        }

        private void CalcMethod()
        {
            double dval1 = 0.0;
            double dval2 = 0.0;
            Trace.Assert(dval2 > 1);
            Console.WriteLine(dval1 / dval2);

            try
            {
                checked
                {
                    int val = int.MaxValue;
                    int val2 = val + 10;
                    Console.WriteLine(val2);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Catcher CalcMethod() " + ex.Message);
            }
        }
    }

    [DebuggerDisplay("{FirstName}, {LastName}")]
    public class Animal
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        //public override string ToString()
        //{
        //    return FirstName + ", " + LastName;
        //}
    }

    public interface IOutputFormatter<T>
    {
        string GetOutput(IEnumerator<T> iterator, int recordSize);
    }

    public class TabDelimitedFormatter<T> : IOutputFormatter<T>
    {
        Func<int, char> suffix = (line) => line % 2 == 0 ? '\n' : '\t';

        public string GetOutput(IEnumerator<T> iterator, int recordSize)
        {
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 1; iterator.MoveNext(); i++)
            {
                stringBuilder.Append(iterator.Current);
                stringBuilder.Append(suffix(i));
            }

            return stringBuilder.ToString();
        }
    }

    public class Connection
    {
        protected Connection(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public static Connection Create(string name)
        {
            return new Connection(name);
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class ConnectionV2 : Connection
    {
        public ConnectionV2(string name) : base(name) 
        {
        }
    }

    public interface INewInterface
    {
        void Method1();
    }

    public class class2 : INewInterface
    {
        void INewInterface.Method1()
        {
            Console.WriteLine("INewInterface.Method1()");
        }
    }

    public class class1 : class2
    {
    }

    interface IFahrenheit
    {
        double Temp();
    }

    interface ICelsius
    {
        double Temp();
    }

    public partial class TempControl : ICelsius, IFahrenheit
    {
        //Implicit ICelsius interface implementation
        public double Temp()
        {
            Console.WriteLine("Temperature in Celsius.");
            return 1;
        }

        //Explicit IFahrenheit interface implementation
        double IFahrenheit.Temp()
        {
            Console.WriteLine("Temperature in Fahrenheit.");
            return 1;
        }
    }

    public class Pet
    {
        public int[] Values { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    [DataContract(Namespace="http://www.contoso.com/2012/06")]
    public class FullName
    {  
        [DataMember(Order = 10)]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public DateTime OrderDate { get; set; }
    }

    public enum Direction
    {
        North,
        South,
        East,
        West
    }

    [DataContract]
    public class Place
    {
        [DataMember]
        public string Label { get; set; }
        [DataMember]
        public Direction PlaceDirection { get; set; }
    }

    public delegate void GetCountDelegate(int count);
    public class Poker
    {
        private Dictionary<string, int> dictionary = new Dictionary<string, int>();

        public void Add(string name, int score, GetCountDelegate callback)
        {
            if(!dictionary.ContainsKey(name))
                dictionary.Add(name, score);

            callback(dictionary.Count);
        }

        public int this[string name]
        {
            get
            {
                return dictionary[name];
            }
            set
            {
                dictionary[name] = value;
            }
        }
    }

    public class Identity : IEquatable<Identity>
    {
        public Identity(int id, string name)
        {
            ID = id;
            Name = name;
        }

        public int ID { get; private set; }

        public string Name { get; private set; }

        public bool Equals(Identity other)
        {
            bool equals = false;

            if (other == null)
            {
                equals = false;
                Trace.TraceInformation("Equals(Identity other): other == null");
            }
            else if(ReferenceEquals(this, other))
            {
                equals = true;
                Trace.TraceInformation("Equals(Identity other): ReferenceEquals(this, other)");
            }
            else if(this.ID != other.ID)
            {
                equals = false;
                Trace.TraceInformation("Equals(Identity other): this.ID != other.ID");
            }
            else if(!this.Name.Equals(other.Name,StringComparison.Ordinal))
            {
                equals = false;
                Trace.TraceInformation("Equals(Identity other): !this.Name.Equals(other.Name,StringComparison.Ordinal)");
            }
            else
            {
                equals = true;
                Trace.TraceInformation("Equals(Identity other): Else { Equals == true}");
            }

            return equals;
        }
    }

    [DataContract(Namespace = "MyBestFriend", Name = "MyOnlyFriend")]
    public class Friend
    {
        public Friend()
        {

        }

        private string _firstName;

        [DataMember(Order=3)]
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        private string _lastName;

        [DataMember(EmitDefaultValue =false)]
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }
    }

    public class Contact
    {
        public Contact(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            ContactId = Guid.NewGuid();
        }

        public Guid ContactId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
    }

    [Serializable]
    public class Device : IComparable<Device>
    {
        public int ID { get; set; }
        public string DeviceType { get; set; }

        public int CompareTo(Device other)
        {
            return ID.CompareTo(other.ID);
        }
    }

    public class Phone : IEquatable<Phone>
    {
        public int ID { get; set; }
        public string PhoneType { get; set; }

        public bool Equals(Phone other)
        {
            if(other == null)
                return false;

            if (ID != other.ID)
                return false;

            if(!PhoneType.Equals(other.PhoneType, StringComparison.OrdinalIgnoreCase))
                return false;

            return true;
        }
    }

    public abstract class BaseReporter
    {
        public virtual void Log(string message)
        {
            Console.WriteLine("Base: " + message);
        }

        public void LogCompleted()
        {
            Console.WriteLine("Base: completed.");
        }
    }

    public class Reporter : BaseReporter
    {
        public override void Log(string message)
        {
            Console.WriteLine("Derrived: " + message);
        }

        //Hidding base class method
        public new void LogCompleted()
        {
            Console.WriteLine("Derrived: completed.");
        }
    }

    public class Actor : IFormattable
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string City { get; set; }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            string content = default;

            if (format.Equals("F", StringComparison.OrdinalIgnoreCase))
                content = string.Format("ID: {0}, Name: {1}, City: {2}", ID, Name, City); 

            return content;
        }
    }

    public class Company
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Manager { get; set; }
        public int BuildingID { get; set; }

        public override string ToString()
        {
            return "ID: " + ID + ", Name: " + Name + ", Manager: " + Manager + ", BuildingID: " + BuildingID;
        }
    }

    class Kid
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    class TherapyAnimal
    {
        public string Name { get; set; }
        public Kid Owner { get; set; }
    }

    [Programmer(programmer:"John")]
    public class ProcessManagement
    {
        public ProcessManagement()
        {
        }

        [Conditional("VERBOSE"), Conditional("TERSE")]
        private void PrintSettings()
        {
            Console.WriteLine("DegreeOfParallelisme: {0}", DegreeOfParallelisme);
        }

        private int _degreeOfParallelisme;
        private int NumberOfTasks = 0;
        public int DegreeOfParallelisme
        {
            get { return _degreeOfParallelisme; }
            //Allow only positive values
            set 
            {
                if(value > 0)
                {
                    if(value > 20)
                    {
                        _degreeOfParallelisme = 20;
                    }
                    else
                    {
                        _degreeOfParallelisme = value;
                    }                    
                }
                else
                {
                    throw new Exception("DegreeOfParallelisme must be larger than 0.");
                }                 
            }
        }
        
        public void SpawnTasks(CancellationToken cts)
        {
            while(DegreeOfParallelisme != NumberOfTasks)
            {
                Task.Factory.StartNew(() =>
                {
                    int counter = 0; 
                    while(!cts.IsCancellationRequested)
                    {
                        Console.WriteLine("Thread ID: {0}, Counter: {1}", Thread.CurrentThread.ManagedThreadId, counter++);
                        Thread.Sleep(400);
                    }

                    Console.WriteLine("FINSIHED: Thread ID: {0}", Thread.CurrentThread.ManagedThreadId);
                }, cts);

                NumberOfTasks++;
            }
        }
    }

    public struct MyPoint
    {
        public MyPoint(int x, int y)
        {
            XValue = x;
            YValue = y;
        }

        public int XValue { get; private set; }
        public int YValue { get; private set; }
    }

    //[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    //[AttributeUsage(AttributeTargets.Class)]
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.GenericParameter| AttributeTargets.Method, AllowMultiple =true)]
    class ProgrammerAttribute : Attribute
    {
        private string programmerValue;
        public ProgrammerAttribute(string programmer)
        {
            programmerValue = programmer;
        }
        public string Programmer
        {
            get
            {
                return programmerValue;
            }
        }
    }

    public class NetflixWatcher
    {
        public NetflixWatcher(string firstName, string lastName, Collection<Movie> movies)
        {
            FirstName = firstName;
            LastName = lastName;
            Movies = movies;
        }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public Collection<Movie> Movies { get; private set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(FirstName + ", " + LastName);
            foreach (Movie movie in Movies)
                sb.AppendLine("\t"+ movie);
            
            return sb.ToString();
        }
    }

    public class Movie
    {
        public Movie(int id, string title, DateTime releaseDate, string genre, decimal price)
        {
            ID = id;
            Title = title;
            ReleaseDate = releaseDate;
            Genre = genre;
            Price = price;
        }

        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; }
        public decimal Price { get; set; }

        public override string ToString()
        {
            return string.Format("ID: {0}, Title: {1}, ReleaseDate: {2}, Genre: {3}, Price: {4:N2}", 
                ID, Title, ReleaseDate.ToString("dd-MM-yyyy"), Genre, Price);
        }
    }

    public static class Conversions
    {
        public static double PoundsToKilos(double pounds)
        {
            // Convert argument from pounds to kilograms
            double kilos = pounds * 0.4536;
            return kilos;
        }
        public static double KilosToPounds(double kilos)
        {
            // Convert argument from kilograms to pounds
            double pounds = kilos * 2.205;
            return pounds;
        }
    }

    public partial class DrinksMachine
    {
        public DrinksMachine(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public void MakeCappuccino()
        {
            Console.WriteLine("Cappuccino for: {0}",Name);
            // Method logic goes here.
        }
    }

    public partial class DrinksMachine
    {

        public void MakeEspresso()
        {
            Console.WriteLine("Espresso for: {0}", Name);
            // Method logic goes here.
        }
    }

    //-----------------------------------------------------------------------------------
    //Deserialization of 
    // https://api.openweathermap.org/data/2.5/weather?q=Amsterdam&units=metric&appid=6f1aa5a06770929433faa4911e4334a1
    //Created classes with http://json2csharp.com/#
    //-----------------------------------------------------------------------------------
    public class Coord
    {
        public double lon { get; set; }
        public double lat { get; set; }
    }

    public class Weather
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }

    public class Main
    {
        public double temp { get; set; }
        public double feels_like { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }
        public int pressure { get; set; }
        public int humidity { get; set; }
    }

    public class Wind
    {
        public double speed { get; set; }
        public int deg { get; set; }
    }

    public class Clouds
    {
        public int all { get; set; }
    }

    public class Sys
    {
        public int type { get; set; }
        public int id { get; set; }
        public string country { get; set; }
        public int sunrise { get; set; }
        public int sunset { get; set; }
    }

    public class RootObject
    {
        public Coord coord { get; set; }
        public List<Weather> weather { get; set; }
        public string @base { get; set; }
        public Main main { get; set; }
        public int visibility { get; set; }
        public Wind wind { get; set; }
        public Clouds clouds { get; set; }
        public int dt { get; set; }
        public Sys sys { get; set; }
        public int timezone { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int cod { get; set; }
    }

    //-----------------------------------------------------------------------------------
}