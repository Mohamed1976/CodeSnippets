using EFCoreDBDemo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

//public static class Queryable contains extension methods such as Count() that can perform db operations 
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
            //Q5();
            //Q6();
            //Q7();
            //Q8();
            //Q9();
            //Q10();
            //Q11();
            //Q12();
            //Q13();
            //Q14();
            //Q15();
            //Q16();
            //Q17();
            //Q18();
            //Q19();
            //Q20();
            //Q21();
            //Q22();
            //Q23();
            //Q24();
            //Q25();
            //Q26();
            //Q27();
            //Q28();
            //Q29();
            //DisplayPopulations();
            //PlinqExceptions();
            //PlinqQueryCancellation();
            //LinqExercise01();
            //LinqExercise02();
            //LinqExercise03();
        }

        /*Given int[] source = { 1, 1, 2, 3, 3, 4, 4 };, select all correct LINQ queries 
         * can remove the duplicate values
         * 
         * var result = source.ToList(); 
         * var result = source.ToHashSet();
         * var result = source.Distinct(); 
         * var result = new HashSet<int>(source); */
        private void LinqExercise03()
        {
            int[] source = { 1, 1, 2, 3, 3, 4, 4 };
            var result = source.Distinct();

            //How to generate a list of numbers from 99 to 0?
            //var nums = Enumerable.Range(0, 99).Reverse(); 
            //var nums = Enumerable.Range(0, 99).OrderByDescending(n => n); 
            //var nums = Enumerable.Range(0, 100).Reverse(); 
            //var nums = Enumerable.Range(0, 100).OrderByDescending(n => n);
            var nums = Enumerable.Range(0, 100).Reverse().ToArray();
            var _nums = Enumerable.Range(0, 100).OrderByDescending(n => n).ToArray();
        
            for(int i = 0; i < _nums.Length; i++)
            {
                Console.WriteLine($"{nums[i]} <==> {_nums[i]}");
            }
        }

        /*
         * Given var list = new List<string>{ 1000 strings }
         * };, select the LINQ query equivalent to var result = 
         * list.Where(s => s.StartsWith("A") && s.Length > 3).Select(s => s.Length);
         * 
         * var result = select s from list where s[0] == 'A' && s.Length > 3;
         * var result = from s in list where s[0] == 'A' and s.Length > 3 select s.Length;
         * var result = select s.Length from s in list where s[0] == 'A' && s.Length > 3; 
         * var result = from s in list where s[0] == 'A' && s.Length > 3 select s.Length;         
         */
        private void LinqExercise02()
        {
            var list = new List<string>
            {
                "Alfa",
                "Beta",
                "Gamma"
            };

            var results = from s in list 
                         where s[0] == 'A' && s.Length > 3 
                         select s.Length;

            foreach(int result in results)
            {
                Console.WriteLine($"Length(\"Alfa\"): {result}");
            }            
        }

        /* Given a list of Student instances (as below) and a total of 10 score sections as 
         * (0 to 9, 10 to 19 ... 90 to 99), with the score of 100 as a section on its own, 
         * select ALL correct LINQ queries that group the students by the score sections
         * var groups = students.GroupBy(s => s.Score / 10); 
         * var groups = students.ToLookup(s => s.Score / 10);
         * var groups = from s in students group s by s.Score; 
         * var groups = from s in students group s by s.Score into g select g;  */
        class Student
        {
            public int Score { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        private void LinqExercise01()
        {
            var students = new List<Student>
            {
                new Student() { FirstName = "Alfa", LastName = "_Alfa", Score=7 },
                new Student() { FirstName = "Beta", LastName = "_Beta", Score = 16 },
                new Student() { FirstName = "Gamma", LastName = "_Gamma", Score = 23 },
                new Student() { FirstName = "Delta", LastName = "_Delta", Score = 35 },
                new Student() { FirstName = "Zetta", LastName = "_Zetta", Score = 36 },
            };

            var groups = students.GroupBy(s => (int)(s.Score / 10));
            var lookup = students.ToLookup(s => (int)(s.Score / 10));

            foreach (var group in groups)
            {
                Console.WriteLine($"Key {group.Key}");
                foreach (Student s in group)
                {
                    Console.WriteLine("\tFirstName: {0}, Score: {1}", s.FirstName, s.Score);
                }
            }

            Console.WriteLine("\n\n");
            // Iterate through each IGrouping in the Lookup and output the contents.
            foreach (IGrouping<int, Student> group in lookup)
            {
                // Print the key value of the IGrouping.
                Console.WriteLine($"Key: {group.Key}");
                // Iterate through each value in the IGrouping and print its value.
                foreach (Student s in group)
                {
                    Console.WriteLine("\t#FirstName: {0}, Score: {1}", s.FirstName, s.Score);
                }
            }
        }

        /* Query Cancellation in PLINQ
        Compared with standard LINQ, one of the advantages of PLINQ is that a PLINQ query can 
        be canceled manually. The most classic scenario of query cancellations is the timeout. 
        Sometimes, when a query takes too long, we consider it a bad query and cancel it immediately. 
        Another scenario is when we allow the user to cancel a long-running query by clicking a button 
        on the UI or pressing a key on the keyboard.

        Since standard LINQ is not cancelable, to make the cancellation happen on a standard LINQ, 
        we have to wrap the standard LINQ in a task (an asynchronous method) and cancel the task. 
        To learn more about Asynchronous programming, check Microsoft course Asynchronous Programming 
        in C# and .Net Core. On the other hand, PLINQ supports cancellation out of box. 

        Actually the cancellation mechanism of PLINQ is based on the exception handling. 
        There is a centralized CancellationTokenSource object referenced by the variable cts. 
        The code of manual cancellation and the code of timeout cancellation call cts.Cancel() 
        to cancel the PLINQ query. When creating the PLINQ query, we called WithCancellation(cts.Token) 
        to enable the cancellation. The code cts.Token.ThrowIfCancellationRequested() indicates that 
        when the cts.Cancel() is called, the cancellation token will throw an exception. The type of 
        the exception thrown by the cancellation token is OperationCanceledException. The exception 
        throwing will happen in each thread of the PLINQ query, which causes the whole PLINQ query to 
        stop. Since it has to wait for the stopping of all threads, there will be a delay between the 
        cancellation command is fired and when the query is actually fully stopped. */
        private void PlinqQueryCancellation()
        {
            var source = Enumerable.Range(0, int.MaxValue);
            var cts = new CancellationTokenSource();

            // Manual cancellation
            Task.Factory.StartNew(() => 
            {
                if (Console.ReadKey().KeyChar == 'c')
                {
                    cts.Cancel();
                }
            });

            // Timeout cancellation
            var timer = new System.Timers.Timer(1000); // 1 second
            int counter = 0;
            timer.Elapsed += (sender, e) => 
            {
                Console.WriteLine($"{++counter} seconds elapsed ...");
                if (counter == 5)
                {
                    cts.Cancel();
                    timer.Stop();
                }
            };

            Task.Factory.StartNew(() =>
            {
                timer.Start();
            });

            // Long-run PLINQ query
            try
            {
                source.AsParallel().WithCancellation(cts.Token).ForAll((n) => 
                {
                    Console.WriteLine($"Processing: {n.ToString().PadLeft(6, '0')}");
                    cts.Token.ThrowIfCancellationRequested();
                    //if(cts.Token.IsCancellationRequested) { }
                    Task.Delay(500).Wait();
                });
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("The PLINQ query is canceled.");
            }
            finally
            {
                cts.Dispose();
            }
        }

        /* When there is an exception, a standard LINQ query will fail immediately.
        But if we convert the standard LINQ query to a PLINQ query, the code catch (DivideByZeroException e) 
        can no longer catch the exception. That's because the type of the exception thrown by PLINQ is 
        AggregateException. One thing we need to pay attention to is before throwing the AggregateException 
        exception; the PLINQ query waits for all threads to finish. That means, when there is an exception 
        thrown by a thread, the PLINQ query may not fail immediately. */
        private void PlinqExceptions()
        {

            var cts = new CancellationTokenSource(5000);
            var source = Enumerable.Range(0, 20).ToList();
            try
            {
                var result = source.AsParallel()
                    .WithCancellation(cts.Token)
                    .Select(n => Calc(n)).ToList();
                foreach (var item in result)
                {
                    Console.Write($"{item}|");
                }
            }
            catch (AggregateException e)
            {
                foreach (var item in e.InnerExceptions)
                {
                    if (item is DivideByZeroException)
                    {
                        Console.WriteLine($"Zero cannot be a divisor: {item.Message}");
                    } // test for other exceptions
                    else
                    {
                        Console.WriteLine($"Exception: {item.Message}");
                    }
                }
            }
        }

        private int Calc(int val)
        {
            Console.WriteLine($"Calc received: {val}");
            return 10 / val;
        }

        //We call the AspNetCoreWebApi.exe
        private void DisplayPopulations()
        {
            string[] contents = new string[]
            {
                "Africa",
                "Asia",
                "Europe",
                "America",
                //"Utopia",
                //"Atlantis"
            };

            Console.WriteLine("Please enter \'r\' to read webservice.");
            Console.ReadKey();
            //if (Console.ReadKey().KeyChar == 'r')
            //{
            //var populations = contents
            try
            {
                var populations = contents.AsParallel()
                    .Select(c => new
                    {
                        Continent = c,
                        Population = GetPopulation(c).Result
                    });

                foreach (var p in populations)
                {
                    Console.WriteLine($"#{p.Continent} {p.Population}");
                }

            }
            catch (AggregateException ex)
            {
                foreach(Exception e in ex.InnerExceptions)
                {
                    Console.WriteLine($"Exception: {e.Message}");
                }
                //throw;
            }
            //Console.WriteLine("Connecting to webservice.");
            //Console.WriteLine($"Population Africa: {GetPopulation("Africa").Result}");
            //Console.WriteLine($"Population Asia: {GetPopulation("Asia").Result}");
            //}
        }

        private async Task<long> GetPopulation(string continent)
        {
            long val = -1;

            //try
            //{
            string url = $"https://localhost:5001/api/Commands/GetCountries/{continent}";
            Console.WriteLine($"url: {url}");
            //Uri uri = new Uri($"https://localhost:5001/api/Commands/GetCountries/{continent}");
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(new Uri(url));
            //Check result of response
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"content: {content}");
                bool result = long.TryParse(content, out val);
                Console.WriteLine($"result: {result}, val: {val}");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new NotImplementedException($"NotFound: continent{continent}");
                //Console.WriteLine($"NotFound: continent{continent}");
            }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Exception: {ex.Message}");
            //    val = -1;
            //    //throw;
            //}

            return val;
        }

        /*LINQ to Objects */
        private void Q29()
        {
            //LINQ to Text File
            //When using a file as the data source, we have to load all of the data in the file to the memory to 
            //generate a source collection. That means, when dealing with a big file, LINQ might not be a 
            //high-performance choice. But for small files, such as configuration files, using LINQ will be  very convenient.
            //var lines = File.ReadAllLines(@"C:\Projects\male.csv");
            //var names = lines.Select(l => l.Split(",")[1]);
            //var result = from n in names
            //             group n by n[0] into g
            //             orderby g.Key
            //             select $"{g.Key},{g.Count()}";
            //File.WriteAllLines(@"C:\Projects\count.csv", result);

            string[] lines =
            {
                "1,Michael",
                "2,Christopher",
                "3,Matthew",
                "4,Joshua",
                "5,Jacob",
                "6,Andrew",
                "7,Mohamed"
            };

            var _names = lines.Select(str => str.Split(',')[1]);
            var _result = from n in _names
                         group n by n[0] into g
                         orderby g.Key descending
                         select $"{g.Key},{g.Count()}";

            foreach (var n in _result)
            {
                Console.WriteLine(n);
            }

            //LINQ is not a good choice to use when working with such a big file. You can think of it that way, 
            //LINQ is not created for this scenario so choosing to use LINQ with big files is not a good development 
            //practice. When dealing with large size files, switch back to using file streams. 
            //The code below shows how to do that using the same example above:
            var dict = new Dictionary<char, int>();
            foreach(string line in lines)
            {
                var name = line.Split(",")[1];
                if (!dict.ContainsKey(name[0]))
                {
                    dict[name[0]] = 0;
                }
                dict[name[0]]++;
            }

            var kvps = dict.OrderByDescending(kvp => kvp.Key);
            foreach (var kvp in kvps)
            {
                Console.WriteLine($"#{kvp.Key},{kvp.Value}");
            }

            /*var reader = new StreamReader(File.OpenRead(@"C:\Projects\male.csv"));
            var writer = new StreamWriter(File.OpenWrite(@"C:\Projects\count.csv"));
            var dict = new Dictionary<char, int>();
            while (!reader.EndOfStream) {
                var line = reader.ReadLine();
                var name = line.Split(",")[1];
                if (!dict.ContainsKey(name[0])) {
                    dict[name[0]] = 0;
                }
                dict[name[0]]++;
            }

            var kvps = dict.OrderBy(kvp => kvp.Key);
            foreach (var kvp in kvps) {
                writer.WriteLine($"{kvp.Key},{kvp.Value}");
            }

            writer.Flush();
            writer.Close(); */

            var nameString = "Tim,Ella,Tom,Gary,Gerry,Andrew,Marwa,Bre'Ana,Li";
            var names = nameString.Split(",");
            var lookup = names.ToLookup(n => n[0], n => n);
            //The source data is a long string which contains many names separated by comma. 
            //The first step towards LINQ query is split the long string to a string array. 
            //After we get the string array, we call the ToLookup standard LINQ operator.
            Console.WriteLine(string.Join(",", lookup['G']));

            //The class declaration of String is:
            //public sealed class String : IEnumerable<char>, IEnumerable, IComparable, IComparable<String>, IConvertible, IEquatable<String>, ICloneable {/*...*/}
            //That means, even without calling the ToCharArray method of a string, we still can treat a string as a char collection since the String class implements 
            //the IEnumerable<char> interface.
            var result = from c in nameString
                         group c by Char.ToUpper(c) into g
                         orderby g.Count() descending //g.Key
                         select new { Char = g.Key, Count = g.Count() };

            //As we see in the code, this time we don't need conversion at all. We just use a string 
            //object as a char collection directly. By the way, if we want to sort the output by the 
            //appearance frequency of the characters and use the character as a secondary order element, 
            //we just need to change the orderby g.Key to orderby g.Count() descending, g.Key. The output 
            //will change to:
            foreach (var item in result)
            {
                if (item.Char < 'A' || item.Char > 'Z') continue;
                Console.WriteLine($"{item.Char}: {item.Count}");
            }
        }

        /*  Querying the File System with LINQ
         *  I want to count the files in the C:\Program Files folder by the file type (extension name), 
         *  and get the top 10 file types and their file count, how to write this code?
         *  In this piece of code, we called AsParallel to leverage the power of PLINQ. 
         *  Also, we mixed the standard LINQ syntax and the query expression syntax to get the result. 
         *  To keep the code readable, we separated the step of getting files and the step of getting 
         *  result into two queries.
        */
        private void Q28()
        {
            Console.WriteLine("Querying the File System.");
            var files = new DirectoryInfo(@"C:\Users\moham\Desktop\Todo")
                .GetFiles("*", SearchOption.AllDirectories).AsParallel();

            var result = (from f in files
                          group f by f.Extension.ToLower() into g
                          orderby g.Count() descending
                          select g)
                          .Take(10)
                          .Select(g => $"{(g.Key == string.Empty ? "N/A" : g.Key)}:\t{g.Count()}");

            foreach (var item in result)
            {
                System.Console.WriteLine(item);
            }
        }

        /* Handling Exceptions in a PLINQ Query
        When there is an exception, a standard LINQ query will fail immediately. For example:
        Since there is a 0 in the source collection, the lambda expression n => 10 / n will throw 
        a DivideByZeroException when the ToList is called (the foreach won't get chance to execute at all). 
        Therefore, the output is: Zero cannot be a divisor.
        But if we convert the standard LINQ query to a PLINQ query, the code catch (DivideByZeroException e)
        can no longer catch the exception. That's because the type of the exception thrown by PLINQ is 
        AggregateException. Therefore, the code should be rewritten as:
        And, the output is: Zero cannot be a divisor.
        One thing we need to pay attention to is before throwing the AggregateException exception; the PLINQ 
        query waits for all threads to finish. That means, when there is an exception thrown by a thread, the PLINQ query may not fail immediately.
        */
        public void Q27()
        {
            var source = Enumerable.Range(0, 20).ToList();
            try
            {
                var result = source.Select(n => 10 / n).ToList();
                foreach (var item in result)
                {
                    Console.Write($"{item}|");
                }
            }
            catch (DivideByZeroException e)
            {
                Console.WriteLine("Zero cannot be a divisor.");
            }

            try
            {
                var result = source.AsParallel().Select(n => 10 / n).ToList();
                foreach (var item in result)
                {
                    Console.Write($"{item}|");
                }
            }
            catch (AggregateException e)
            {
                foreach (var item in e.InnerExceptions)
                {
                    if (item is DivideByZeroException)
                    {
                        Console.WriteLine("Zero cannot be a divisor.");
                    } // test for other exceptions
                }
            }
        }

        /* Both the Parallel.ForEach and AsParallel accept a Action<TSource> type parameter which 
         * represents the processing logic for each element. Please note, the Action<TSource> delegate 
         * won't return any value. Actually, the PLINQ version code will be translated to the TPL version 
         * eventually. The TPL will create an appropriate number of threads based on how many CPU cores 
         * your computer has and distribute the threads to the CPU cores to fully leverage the CPU power. 
         * As the side effect, we can't control the order of the element processing, even call AsOrdered 
         * after the AsParallel. Notice the use of the System.Threading.Tasks library.
        */
        public void Q26()
        {
            var source = Enumerable.Range(0, 20).ToList();

            // TPL version
            Parallel.ForEach(source, (item) => {
                Console.Write($"{item.ToString().PadLeft(2, '0')}|");
            });

            Console.WriteLine();
            Console.WriteLine("======================");

            // PLINQ version
            source.AsParallel().ForAll((item) => {
                Console.Write($"{item.ToString().PadLeft(2, '0')}|");
            });
        }

        /* In PLINQ, the goal is to maximize performance while maintaining correctness. 
         * A query should run as fast as possible but still produce the correct results. 
         * In some cases, correctness requires the order of the source sequence to be preserved; 
         * however, ordering can be computationally expensive. Therefore, by default, PLINQ does 
         * not preserve the order of the source sequence. To override the default behavior, you 
         * can turn on order-preservation by using the AsOrdered operator on the source sequence. 
         * You can then turn off order preservation later in the query by using the AsUnordered 
         * operator. With both methods, the query is processed based on the heuristics that determine 
         * whether to execute the query as parallel or as sequential.
        */
        public void Q25()
        {
            int[] source = Enumerable.Range(0, 20).ToArray();
            var query1 = source.AsParallel().AsOrdered()
                .Where(n => n % 2 == 1).Select(n => -n);
            var query2 = source.AsParallel().AsOrdered()
                .Where(n => n % 2 == 1).AsUnordered().Select(n => -n);
            /* The output of the code example is:
            -1, -3, -5, -7, -9, -11, -13, -15, -17, -19
            -1, -3, -7, -9, -13, -15, -17, -19, -5, -11
            Since the PLINQ query query1 called the AsOrdered operator at the beginning of the parallel 
            query pipeline, the original order of the elements is preserved. But for the PLINQ query query2, 
            the original order was only preserved for the Where operator, then the order-preservation was 
            turned off by calling AsUnordered. That's why the output of the final result of query2 is not 
            in the original order. Please note that there are some exceptions related to the order-preservation 
            of PLINQ operators:
            Some of the PLINQ operators don't have order at all since their results are single values. For example, All, Any, Max, Min.
            Some of the PLINQ operators have the order-preservation turned on by default (you even cannot turn it off). For example, the Range, Reverse.
            */
            Console.WriteLine(string.Join(", ", query1));
            Console.WriteLine(string.Join(", ", query2));
        }

        //If we go through the declaration of the class System.Linq.ParallelEnumerable, we can find 
        //that there is another suite of LINQ operators, such as Where, Select, All, Any, Max, Min, etc. 
        //They are the PLINQ operators. For most of these operators, the first parameter is this 
        //ParallelQuery<TSource> source, which indicates that these operators are extension methods 
        //attached to the ParallelQuery<T> type. In other words, we have to get a ParallelQuery<T> 
        //type object as the source first, then execute the PLINQ queries on it.
        //To get a ParallelQuery<T> type object, we just need to call the following extension method:
        //public static ParallelQuery<TSource> AsParallel<TSource>(this IEnumerable<TSource> source);
        //As we see, the AsParallel method is an extension method attached to any implementation of IEnumerable<T>. 
        //In other words, the AsParallel can convert a common collection object to a parallel-queriable object.
        public void Q24()
        {
            int[] source = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var query1 = source
                .Where(n => n % 2 == 1).Select(n => -n);
            var query2 = source.AsParallel()
                .Where(n => n % 2 == 1).Select(n => -n);

            /* Both the query1 and query2 have the same query pipeline .Where(n => n % 2 == 1).
             * Select(n => -n), but the query2 is a PLINQ query which executes the query pipeline 
             * on multiple threads then merge the results of each thread to the final result. 
             * To maximize the performance of the parallel execution, PLINQ ignores the original 
             * order of the elements by default, unless you ask the PLINQ to keep the original order. 
             * That's why the output of the query2 is the same as query1 but in a mixed order -5, -7, -9, -1, -3. 
             */
            Console.WriteLine(string.Join(", ", query1));
            Console.WriteLine(string.Join(", ", query2));
        }

        //Select ALL correct queries that can get the product name with its ProductCategory name
        public void Q23()
        {
            using (AdventureWorksLT2017Context _dbContext = new AdventureWorksLT2017Context())
            {
                var products = from p in _dbContext.Product 
                               join c in _dbContext.ProductCategory on p.ProductCategoryId equals c.ProductCategoryId 
                               orderby p.Name
                               select new { Product = p.Name, Category = c.Name };

                var products2 = _dbContext.Product
                    .Join(_dbContext.ProductCategory, p => p.ProductCategoryId, c => c.ProductCategoryId, 
                    (p, c) => new { Product = p.Name, Category = c.Name })
                    .OrderBy(p => p.Product);

                Console.WriteLine($"@@products count: {products.Count()}");
                Console.WriteLine($"@@products2 count: {products2.Count()}");

                var productList = products.ToList();
                var productList2 = products2.ToList();

                for(int i = 0; i < productList.Count(); i++)
                {
                    Console.WriteLine($"{i}: {productList[i].Product} {productList[i].Category} " +
                        $"<==> {productList2[i].Product} {productList2[i].Category}");
                }

                //foreach(var p in products)
                //{
                //    Console.WriteLine($"{p.Product} {p.Category}");
                //}

                //foreach (var p in products2)
                //{
                //    Console.WriteLine($"{p.Product} {p.Category}");
                //}
            }
        }

        //Select ALL correct queries that can get Customer FirstName sorted alphabetically
        public void Q22()
        {
            using (AdventureWorksLT2017Context _dbContext = new AdventureWorksLT2017Context())
            {
                var names = _dbContext.Customer
                    .OrderBy(c => c.FirstName)
                    .Select(c => c.FirstName)
                    .Distinct();

                Console.WriteLine($"{names.Count()}");
                foreach (string c in names)
                {
                    Console.WriteLine($"{c}");
                }

                names = _dbContext.Customer
                    .Select(c => c.FirstName)
                    .Distinct()
                    .OrderBy(p => p);

                Console.WriteLine($"\n\n{names.Count()}");
                foreach (string c in names)
                {
                    Console.WriteLine($"{c}");
                }

                names = (from c in _dbContext.Customer
                        orderby c.FirstName
                        select c.FirstName).Distinct();

                Console.WriteLine($"\n\n{names.Count()}");
                foreach (string c in names)
                {
                    Console.WriteLine($"{c}");
                }

                names = from n in (from c in _dbContext.Customer select c.FirstName).Distinct() 
                        orderby n 
                        select n;

                Console.WriteLine($"\n\n#{names.Count()}");
                foreach (string c in names)
                {
                    Console.WriteLine($"{c}");
                }
            }
        }

        //If we show n customers on one page, how to get the customers to be shown on the m th page?
        public void Q21()
        {
            IList<Customer> customers = GetCustomers(1);
            foreach(Customer c in customers)
            {
                Console.WriteLine($"#{c.CustomerId} {c.FirstName} {c.LastName}");
            }

            customers = GetCustomers(2);
            foreach (Customer c in customers)
            {
                Console.WriteLine($"##{c.CustomerId} {c.FirstName} {c.LastName}");
            }
        }

        private IList<Customer> GetCustomers(int m)
        {
            const int n = 10;

            using (AdventureWorksLT2017Context _dbContext = new AdventureWorksLT2017Context())
            {
                IList<Customer> customers = _dbContext.Customer
                    .OrderBy(p => p.CustomerId)
                    .Skip(n * (m - 1))
                    .Take(n)
                    .ToList();

                return customers;
            }
        }

        //How to get the products with ListPrice higher than $2000 and weight higher than 1000g?
        public void Q20()
        {
            using (AdventureWorksLT2017Context _dbContext = new AdventureWorksLT2017Context())
            {
                var products = _dbContext.Product.Where(p => p.ListPrice > 2000 && p.Weight > 1000);
                Console.WriteLine($"#{products.Count()}");
                products = _dbContext.Product.Where(p => p.ListPrice > 2000)
                    .Intersect(_dbContext.Product.Where(p => p.Weight > 1000));
                Console.WriteLine($"##{products.Count()}");

                foreach(Product p in products)
                {
                    Console.WriteLine($"{p.Name} {p.Weight} {p.ListPrice}");
                }
            }
        }

        //How to get the FirstName of all Customers?
        public void Q19()
        {
            using (AdventureWorksLT2017Context _dbContext = new AdventureWorksLT2017Context())
            {
                var names = _dbContext.Customer
                    .OrderBy(c => c.FirstName)
                    .Select(c => c.FirstName);

                foreach (var c in names)
                {
                    Console.WriteLine($"{c}");
                }

                Console.WriteLine($"{names.Count()}");
            }
        }

        //Select ALL LINQ queries that can find out the products with a ListPrice higher than $3000?
        public void Q18()
        {
            using (AdventureWorksLT2017Context _dbContext = new AdventureWorksLT2017Context())
            {
                var products = _dbContext.Product.Where(p => p.ListPrice > 3000);
                Console.WriteLine($"products count: {products.Count()}");
                products = from p in _dbContext.Product
                           where p.ListPrice > 3000
                           select p;
                Console.WriteLine($"products count: {products.Count()}");

                foreach(Product p in products)
                {
                    Console.WriteLine($"{p.Name} {p.ListPrice}");
                }
            }
        }

        //Select ALL LINQ queries that can tell us whether there is a Customer with FirstName "Donna"
        public void Q17()
        {
            using (AdventureWorksLT2017Context _dbContext = new AdventureWorksLT2017Context())
            {
                var found = _dbContext.Customer.Any(c => c.FirstName == "Donna");
                Console.WriteLine($"Customer \"Donna\" found: {found}");
                found = _dbContext.Customer.Where(c => c.FirstName == "Donna").Any();
                Console.WriteLine($"Customer \"Donna\" found: {found}");
                found = _dbContext.Customer.Count(c => c.FirstName == "Donna") > 0;
                Console.WriteLine($"Customer \"Donna\" found: {found}");
            }
        }

        /* The difference between JOIN and GROUPJOIN.
         * Each element of the first collection appears in the result set of a group join regardless of whether 
         * correlated elements are found in the second collection. In the case where no correlated elements are 
         * found, the sequence of correlated elements for that element is empty. The result selector therefore 
         * has access to every element of the first collection. This differs from the result selector in a non-group 
         * join, which cannot access elements from the first collection that have no match in the second collection.
        */
        //https://docs.microsoft.com/en-us/dotnet/csharp/linq/perform-grouped-joins
        //https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.groupjoin?view=netcore-3.1
        public void Q16()
        {
            using (AdventureWorksLT2017Context _dbContext = new AdventureWorksLT2017Context())
            {
                var categories = _dbContext.ProductCategory.ToList();
                var products = _dbContext.Product.ToList();

                // GroupJoin
                var result = from category in categories
                             join product in products
                             on category.ProductCategoryId equals product.ProductCategoryId into g
                             orderby category.Name
                             select new { Category = category.Name, ProductCount = g.Count() };

                foreach (var r in result)
                {
                    Console.WriteLine($"{r.Category}\t\t {r.ProductCount}");
                }

                var ProductCategoriesOrdered = _dbContext.ProductCategory.OrderBy(c => c.Name).ToList();
                var productsOrdered = _dbContext.Product.OrderBy(c => c.Name).ToList();

                Console.WriteLine("\n\n");
                //The GroupJoin Operator
                //Given two queries, one query's result is a collection of ProductCategories sorted by name, 
                //and the second query's result is a collection of products
                //sorted by name. Could you find out how many products belong to each Category?
                var _result = ProductCategoriesOrdered
                    .GroupJoin(productsOrdered,
                    c => c.ProductCategoryId,
                    p => p.ProductCategoryId,
                    (category, grp) => new { Category = category.Name, ProductCount = grp.Count() });

                foreach (var r in _result)
                {
                    Console.WriteLine($"{r.Category}\t {r.ProductCount}");
                }

                Console.WriteLine("\n\n");
                // Join then GroupBy
                //You can see that the code of direct GroupJoin is more readable than the code of Join then GroupBy.
                var __result = from category in categories
                               join product in products
                               on category.ProductCategoryId equals product.ProductCategoryId
                               orderby category.Name
                               group product by category.Name into g
                               select new { Category = g.Key, ProductCount = g.Count() };

                foreach (var r in __result)
                {
                    Console.WriteLine($"@{r.Category}\t\t {r.ProductCount}");
                }
            }
        }

        //Join Operations
        //Given two queries, one query's result is a collection of ProductCategory sorted by name, and the 
        //second query's result is a collection of products sorted by name.
        //Could you join them together and print out the product name with the ProductCategory name?
        //Please note: since both the Join operator and the Include extension method will be translated to 
        //JOIN operation on the database side, sometimes they are interchangeable.
        public void Q15()
        {
            using (AdventureWorksLT2017Context _dbContext = new AdventureWorksLT2017Context())
            {
                var ProductCategories = _dbContext.ProductCategory.OrderBy(c => c.Name);
                var products = _dbContext.Product.OrderBy(c => c.Name);

                var result = ProductCategories.Join(products,
                    c => c.ProductCategoryId,
                    p => p.ProductCategoryId,
                    (category, prod) => new { Category = category.Name, Product = prod.Name });

                foreach (var r in result)
                {
                    Console.WriteLine($"{r.Category}\t {r.Product}");
                }

                Console.WriteLine("\n\n");
                //The equivalent query in query expression syntax is:
                var _result = from category in ProductCategories
                              join prod in products
                              on category.ProductCategoryId equals prod.ProductCategoryId
                              select new { Category = category.Name, Product = prod.Name };

                foreach (var r in _result)
                {
                    Console.WriteLine($"{r.Category}\t {r.Product}");
                }
            }
        }

        //The GroupBy Operator
        //Could you find the top 3 most expensive products of each ProductCategory?
        public void Q14()
        {
            using (AdventureWorksLT2017Context _dbContext = new AdventureWorksLT2017Context())
            {
                var products = _dbContext.Product
                    .Include(nameof(Product.ProductCategory))
                    .OrderByDescending(p => p.ListPrice).AsEnumerable();

                var topThree = products.GroupBy(p => p.ProductCategory.Name, p => p.Name + ", " + p.ListPrice)
                    .Select(g => $"{g.Key}:\t{string.Join(",", g.Take(3))}\n");

                foreach(var prod in topThree)
                {
                    Console.WriteLine($"{prod}");
                }

                //Comment/Explanation:
                //The equivalent query in query expression syntax is:
                var _topThree = from p in _dbContext.Product
                                .Include(nameof(Product.ProductCategory)).AsEnumerable() //Retrieves list from db, client side execution of group by
                                orderby p.ListPrice descending
                                group p.Name by p.ProductCategory.Name into g
                                select $"{g.Key}\t{string.Join(",", g.Take(3))}";

                Console.WriteLine("\n\n");
                foreach (var prod in _topThree)
                {
                    Console.WriteLine($"{prod}");
                }
            }
        }

        //Aggregation Operations
        //The Max, Min, Count and Sum Operator
        //Could you find out:
        //the heaviest product
        //the lightest product
        //how many products in ProductCategory "Road Bikes"
        //the total weight of the products in ProductCategory "Road Bikes"
        public void Q13()
        {
            using (AdventureWorksLT2017Context _dbContext = new AdventureWorksLT2017Context())
            {
                decimal? heaviestProduct = _dbContext.Product.Max(p => p.Weight);
                decimal? lightest = _dbContext.Product.Min(p => p.Weight);

                IQueryable<Product> products = _dbContext.Product.Include(nameof(Product.ProductCategory));
                
                IQueryable<Product> _products = _dbContext.Product
                    .Include(nameof(Product.ProductCategory))
                    .Where(p => p.ProductCategory.Name == "Road Bikes");

                int productCount = products.Count(p => p.ProductCategory.Name == "Road Bikes");
                int _productCount = products
                    .Where(p => p.ProductCategory.Name == "Road Bikes")
                    .Count();
                decimal? totalWeight = products
                    .Where(p => p.ProductCategory.Name == "Road Bikes")
                    .Sum(p => p.Weight);

                Console.WriteLine($"The heaviest product: {heaviestProduct}");
                Console.WriteLine($"The lightest product: {lightest}");
                Console.WriteLine($"Product Count in \"Road Bikes\": {productCount}");
                Console.WriteLine($"Product Count in \"Road Bikes\": {_productCount}");
                Console.WriteLine($"Total Weight products in \"Road Bikes\": {totalWeight}");
                Console.WriteLine();
                Console.WriteLine($"The heaviest product: {_products.Max(p => p.Weight)}");
                Console.WriteLine($"The lightest product: {_products.Min(p => p.Weight)}");
                Console.WriteLine($"Product Count in \"Road Bikes\": {_products.Count()}");
                Console.WriteLine($"Total Weight products in \"Road Bikes\": {_products.Sum(p => p.Weight)}");
            }
        }

        //Converting Data Types
        //The ToArray, ToList, ToDictionary, and ToLookup Operator
        //Retrieve all products and store them in an array?
        //Retrieve all products and store them in a list?
        //Generate a dictionary for looking up the ListPrice of products?
        //Generate a Lookup<K,V> object for looking up product names by ProductCategory name?
        public void Q12()
        {
            using (AdventureWorksLT2017Context _dbContext = new AdventureWorksLT2017Context())
            {
                Product[] productsArray = _dbContext.Product.ToArray();
                List<Product> productsList = _dbContext.Product.ToList();
                Dictionary<string,decimal> productsDictionary = _dbContext.Product.ToDictionary(p => p.Name, p => p.ListPrice);
                ILookup<string,string> productsLookup = _dbContext.Product
                    .Include(nameof(Product.ProductCategory))
                    .ToLookup(p => p.ProductCategory.Name, p => p.Name);

                foreach (KeyValuePair<string, decimal> product in productsDictionary)
                {
                    Console.WriteLine($"{product.Key} {product.Value}");
                }

                Console.WriteLine("\n\n");
                // Iterate through each IGrouping in the Lookup and output the contents.
                //https://docs.microsoft.com/en-us/dotnet/api/system.linq.lookup-2.getenumerator?view=netcore-3.1
                foreach (IGrouping<string, string> productCategory in productsLookup)
                {
                    // Print the key value of the IGrouping.
                    Console.WriteLine(productCategory.Key);
                    // Iterate through each value in the IGrouping and print its value.
                    foreach (string ProductName in productCategory)
                        Console.WriteLine("    {0}", ProductName);
                }

                //Simple Join, to display for example all products in "Road Bikes" productCategory
                Console.WriteLine(string.Join(",", productsLookup["Road Bikes"]));

                Console.WriteLine("\n\n");
                Console.WriteLine(productsArray.Length);
                Console.WriteLine(productsList.Count);
            }
        }

        //The Skip and Take Operator
        //Given the products sorted by their List Price in descending order, could you find out the:
        //The first five products with the highest List Price  
        //Products in the 21st to 25th place
        //Cheapest five products
        public void Q11()
        {
            using (AdventureWorksLT2017Context _dbContext = new AdventureWorksLT2017Context())
            {
                IQueryable<Product> products = _dbContext.Product.OrderByDescending(p => p.ListPrice);

                List<Product> firstFiveProducts = products.Take(5).ToList();
                List<Product> Products21To25 = products.Skip(20).Take(5).ToList();
                //You may notice that the ToList operator is called in the code var smallest5 = 
                //sorted.ToList().TakeLast(5);. That's because the TakeLast
                //operator for IQueryable<T> is not implemented yet in the EF Core library. 
                //The operator TakeWhile and SkipWhile are also not implemented yet.
                //List<Product> ProductsLast5 = products.TakeLast(5).ToList();
                List<Product> ProductsLast5 = products.ToList().TakeLast(5).ToList();

                Console.WriteLine("[Highest 5]");
                foreach (var c in firstFiveProducts)
                {
                    Console.WriteLine($"{c.Name} {c.ListPrice}");
                }

                Console.WriteLine("===================");
                Console.WriteLine("[Highest 21 to 25]");
                foreach (var c in Products21To25)
                {
                    Console.WriteLine($"{c.Name} {c.ListPrice}");
                }

                Console.WriteLine("===================");
                Console.WriteLine("[Lowest 5]");
                foreach (var c in ProductsLast5)
                {
                    Console.WriteLine($"{c.Name} {c.ListPrice}");
                }
            }
        }

        //Single will throw an error if more than one record is obtained or when zero records found. 
        //If replacing all Single operators with SingleOrDefault
        //SingleOrDefault operator won't throw exception but return a null when the target value is not found. 
        //The SingleOrDefault operator will still throw exception when there are more than one target values are found.
        //Note: Since the main purpose of using the Single operator is to check if there is duplicates or missing on values that are supposed to be unique.
        //With that in mind, the Single operator should always be used in a try...catch block.
        public void Q10()
        {
            using (AdventureWorksLT2017Context _dbContext = new AdventureWorksLT2017Context())
            {
                try
                {
                    var product = _dbContext.Product.Single(p => p.Name == "Road-150 Red, 62");
                    Console.WriteLine($"Product found: {product.Name}, {product.ListPrice}");

                }
                catch (Exception)
                {
                    Console.WriteLine($"There maybe zero or more than one \"Road - 150 Red, 62\" product.");
                    //throw;
                }

                try
                {
                    _dbContext.Product.Single(p => p.Name == "Does not exist");
                }
                catch (Exception)
                {
                    Console.WriteLine($"There maybe zero or more than one \"Does not exist\" product.");
                    //throw;
                }
            }
        }


        //Single Element Operations
        //The First, Last, and ElementAt Operator
        //Could you find out the product that has:
        //the highest ListPrice
        //The lowest  ListPrice
        //The third highest ListPrice 
        public void Q9()
        {
            using (AdventureWorksLT2017Context _dbContext = new AdventureWorksLT2017Context())
            {
                IQueryable<Product> sortedProducts = _dbContext.Product.OrderByDescending(p => p.ListPrice);

                var ProdWithHighestListPrice = sortedProducts.First();
                var ProdWithLowestListPrice = sortedProducts.Last();
                //var ProdWithThirdHighestListPrice = sortedProducts.ElementAt(2);

                Console.WriteLine($"Highest ListPrice: {ProdWithHighestListPrice.Name}, " +
                    $"{ProdWithHighestListPrice.ListPrice}");

                Console.WriteLine($"Lowest ListPrice: {ProdWithLowestListPrice.Name}, " +
                    $"{ProdWithLowestListPrice.ListPrice}");

                //Unfortunately, the ElementAt operator for IQueryable<T> is not implement by the EF Core library yet. 
                //To use the ElementAt operator, we have to get the sorted list of products 
                var sortedProductsList = _dbContext.Product.OrderByDescending(p => p.ListPrice).ToList();
                var ProdWithThirdHighestListPrice = sortedProductsList.ElementAt(2);

                Console.WriteLine($"Third Highest ListPrice: {ProdWithThirdHighestListPrice.Name}, " +
                    $"{ProdWithThirdHighestListPrice.ListPrice}");

                //var ProdWithHighestListPrice = _dbContext.Product
                //    .OrderByDescending(p => p.ListPrice).First();

                //var ProdWithLowestListPrice = _dbContext.Product
                //    .OrderByDescending(p => p.ListPrice).Last();

                //var ProdWithThirdHighestListPrice = _dbContext.Product
                //    .OrderByDescending(p => p.ListPrice).ElementAt(2);
            }
        }

        //A sorting operation orders the elements of a sequence based on one or more attributes.
        //This group of operators includes:
        //OrderBy - Sorts values in ascending order.
        //ThenBy - Performs a secondary sort in ascending order.
        //OrderByDescending - Sorts values in descending order.
        //ThenByDescending - Performs a secondary sort in descending order.
        //Reverse - Reverses the order of the elements in a collection.
        //Could you sort all products by its ProductCategory name , 
        //and then sort the products in the same ProductCategory by ListPrice in the descending order? 
        //Could you also write this query in query expression syntax?
        public void Q8()
        {
            using (AdventureWorksLT2017Context _dbContext = new AdventureWorksLT2017Context())
            { 
                var sortedProducts = _dbContext.Product
                    .Include(nameof(Product.ProductCategory))
                    .OrderBy(p => p.ProductCategory.Name)
                    .ThenByDescending(p => p.ListPrice);

                foreach(var prod in sortedProducts)
                {
                    Console.WriteLine($"Group: {prod.ProductCategory.Name}, Price: {prod.ListPrice}, Product: {prod.Name}");
                }

                //The equivalent query in query expression syntax is:
                var _sortedProducts = from p in _dbContext.Product.Include(nameof(Product.ProductCategory))
                                      orderby p.ProductCategory.Name, p.ListPrice descending
                                      //orderby p.ProductCategory.Name ascending
                                      //orderby p.ListPrice descending
                                      select p;

                Console.WriteLine("\n\n");
                foreach (var prod in _sortedProducts)
                {
                    Console.WriteLine($"Group: {prod.ProductCategory.Name}, Price: {prod.ListPrice}, Product: {prod.Name}");
                }
            }
        }

        //Set Operations and Concatenation Operations
        //There are four operators: Distinct, Intersect, Except, and Union in this group.
        //The Intersect, Except, and Union Operator
        public void Q7()
        {
            //Given two collections of products:
            var highestPricedProducts = dbContext.Product
                .OrderByDescending(p => p.ListPrice)
                .Take(10);

            var highestWeight = dbContext.Product
                .OrderByDescending(p => p.Weight)
                .Take(10);

            //The variable highestPricedProducts references the top 10 products with the highest price. 
            //The variable highestWeight references the top 10 products with the highest weight. 
            //Could you find:
            //Products that either have a high price or have a high weight.
            //Products that have both high price and high weight.
            //Products that have a high price but not a high weight.
            var r1 = highestPricedProducts.Union(highestWeight);
            var r2 = highestPricedProducts.Intersect(highestWeight);
            var r3 = highestPricedProducts.Except(highestWeight);

            Console.WriteLine("[Products that either have a high price or have a high weight.]");
            foreach (var r in r1.OrderByDescending(p => p.Name))
            {
                Console.WriteLine(r.Name);
            }

            Console.WriteLine("========================");
            Console.WriteLine("[Products that have both high price and high weight.]");
            foreach (var r in r2)
            {
                Console.WriteLine(r.Name);
            }

            Console.WriteLine("========================");
            Console.WriteLine("[Products that have a high price but not a high weight.]");
            foreach (var r in r3)
            {
                Console.WriteLine(r.Name);
            }

            //Why Union not Concat?
            //Similar to the standard LINQ query, the Concat operator will just simply attach a collection to another, and it won't remove the duplicate data in
            //the result collection. In other words, if we use the Concat operator, the code:
            //We can see there are many duplicate product names in the output.
            var r4 = highestPricedProducts.Concat(highestWeight);
            Console.WriteLine("========================");
            Console.WriteLine("[Products that either have a high price or have a high weight.]");
            foreach (var r in r4.OrderByDescending(p => p.Name))
            {
                Console.WriteLine(r.Name);
            }

            //Could you find out the ProductCategories that have products in them?
            IQueryable<ProductCategory> productCategories = dbContext.Product
                .Include(nameof(Product.ProductCategory))
                .Select(p => p.ProductCategory)
                .Distinct()
                .OrderByDescending(c => c.Name);
            
            Console.WriteLine("========================");
            Console.WriteLine($"[ProductCategories that have products in them ({productCategories.Count()}).]");
            foreach (var r in productCategories)
            {
                Console.WriteLine(r.Name);
            }

            //The equivalent query is:
            //Note: As you may notice, the equivalent code is clearer and simpler. For this reason, in a real-world project we should consider the equivalent
            //query first. This example is designed to demonstrate the Distinct operator, but as we know, there's rarely duplicate data in a database
            //anyways.
            var _productCategories = dbContext.ProductCategory
                .Include(nameof(ProductCategory.Product))
                .Where(p => p.Product.Any())
                .OrderByDescending(p => p.Name);

            Console.WriteLine("========================");
            Console.WriteLine($"[ProductCategories that have products in them ({_productCategories.Count()}).]");
            foreach (var r in _productCategories)
            {
                Console.WriteLine(r.Name);
            }
        }

        //The SelectMany Operator
        //Could you list the name of products in the category Touring Frames and Wheels?
        //The query SelectMany(c => c.Product) flattens the Product collections of the selected two ProductCategory.
        public void Q6()
        {
            IQueryable<string> productNames = dbContext.ProductCategory
                .Include(nameof(ProductCategory.Product))
                .Where(c => c.Name == "Touring Frames" || c.Name == "Wheels")
                .SelectMany(c => c.Product)
                .Select(p => p.Name);

            Console.WriteLine($"Record count: {productNames.Count()}");
            foreach (string productName in productNames)
            {
                Console.WriteLine($"Product name: {productName}");
            }

            //The equivalent query which uses Select operator only is:
            //The difference between these two versions, after translated to SQL statement, 
            //is that the SelectMany version is joining the product table to the
            //ProductCategory table, while the Select-only version is joining the ProductCategory table to the Product table.
            //Note: As we see in the example, the equivalent Select-only version query is much simpler and straightforward. And, when querying the
            //database, we rarely see the SelectMany operator, that's because by picking a proper entity as the source data, we can write a simple Select
            //query and avoid from writing a crappy SelectMany query.
            IQueryable<string> _productNames = dbContext.Product
                .Include(p => p.ProductCategory)
                .Where(p => p.ProductCategory.Name == "Touring Frames" || p.ProductCategory.Name == "Wheels")
                .Select(p => p.Name);

            Console.WriteLine($"\n\nRecord count: {_productNames.Count()}");
            foreach (string productName in _productNames)
            {
                Console.WriteLine($"Product name: {productName}");
            }
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
            //Include means using eager loading
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