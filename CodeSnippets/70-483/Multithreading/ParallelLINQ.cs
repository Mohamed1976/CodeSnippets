using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace _70_483.Multithreading
{
    public class ParallelLINQ
    {
        private Person[] people = new Person[] 
        {
            new Person { Name = "Alan", City = "Hull" },
            new Person { Name = "Beryl", City = "Seattle" },
            new Person { Name = "Charles", City = "London" },
            new Person { Name = "David", City = "Seattle" },
            new Person { Name = "Eddy", City = "Paris" },
            new Person { Name = "Fred", City = "Berlin" },
            new Person { Name = "Gordon", City = "Hull" },
            new Person { Name = "Henry", City = "Seattle" },
            new Person { Name = "Isaac", City = "Seattle" },
            new Person { Name = "James", City = "London" }            
        };

        public ParallelLINQ()
        {
        }

        public void Run()
        {
            ExecuteQuery1();
            ExecuteQuery2();
            ExecuteQuery3();
            ExecuteQuery4();
            ExecuteQuery5();
        }

        //The AsParallel method examines the query to determine if using a parallel version would speed it up.
        //If it is decided that executing elements of the query in parallel would improve performance, 
        //the query is broken down into a number of processes and each is run concurrently. If the AsParallel method
        //can’t decide whether parallelization would improve performance the query is not executed in parallel.
        //If you really want to use AsParallel you should design the behavior with this in mind, otherwise performance 
        //may not be improved and it is possible that you might get the wrong outputs.
        private void ExecuteQuery1()
        {
            var persons = from person in people.AsParallel() 
                         where person.City == "Seattle" 
                         select person;

            foreach(Person p in persons)
            {
                Console.WriteLine($"Name: {p.Name}, City: {p.City} ");
            }
        }

        //This call of AsParallel requests that the query be parallelized whether performance is improved or not, 
        //with the request that the query be executed on a maximum of four processors. A non-parallel query produces 
        //output data that has the same order as the input data. A parallel query, however, may process data in a 
        //different order from the input data. If it is important that the order of the original data be preserved, the
        //AsOrdered method can be used to request this from the quer. The AsOrdered method doesn’t prevent the parallelization 
        //of the query, instead it organizes the output so that it is in the same order as the original data. 
        //This can slow down the query. Another issue that can arise is that the parallel nature of a query may remove
        //ordering of a complex query. The AsSequential method can be used to identify parts of a query that must be 
        //sequentially executed. AsSequential executes the query in order whereas AsOrdered returns a
        //sorted result but does not necessarily run the query in order.
        private void ExecuteQuery2()
        {
            var persons = from person in people
                          .AsParallel()
                          /* AsOrdered(), Preserve the order of the original data. */
                          .AsOrdered()
                          /* Degree Of Parallelism, for example a maximum of four processors. */
                          .WithDegreeOfParallelism(4)
                          /* WithExecutionMode forces that the query is executed parallel. */
                          .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                          where person.City == "Seattle"
                          select person;

            foreach (Person p in persons)
            {
                Console.WriteLine($"Name: {p.Name}, City: {p.City} ");
            }
        }

        //The query in Listing 1-8 retrieves the names of the first four people who live in Seattle.
        //The query requests that the result be ordered by person name, and this ordering is preserved 
        //by the use of AsSequential before the Take, which removes the four people.If the Take is executed 
        //in parallel it can disrupt the ordering of the result.
        private void ExecuteQuery3()
        {
            var objects = (from person in people
                           .AsParallel()
                          where person.City == "Seattle"
                          orderby (person.Name)
                          /* Create anonymous class. */
                          select new
                          {
                              Name = person.Name
                          }).AsSequential().Take(4);

            foreach (var obj in objects)
            {
                Console.WriteLine($"Name: {obj.Name}");
            }
        }

        //The ForAll method can be used to iterate through all of the elements in a query. It differs from the 
        //foreach C# construction in that the iteration takes place in parallel and will start before the query is complete.
        //The parallel nature of the execution of ForAll means that the order of the printed output will not reflect 
        //the ordering of the input data.
        private void ExecuteQuery4()
        {
            var result = from person in people
                         .AsParallel()
                         where person.City == "Seattle"
                         select person;

            result.ForAll(person => Console.WriteLine($"Name: {person.Name}, City: {person.City}"));
        }

        //It is possible that elements of a query may throw exceptions. This CheckCity method throws an exception when the 
        //city name is empty. Using this method in a PLINQ query will cause exceptions to be thrown when empty city names 
        //are encountered in the data. The code uses the CheckCity method in a query. This will
        //cause exceptions to be thrown when empty city names are encountered during the query.
        //If any queries generate exceptions an AgregateException will be thrown when the query is complete.
        //This contains a list, InnnerExceptions, of the exceptions that were thrown during the query.
        //Note that the outer catch of AggregateException does catch any exceptions thrown by the CheckCity method.
        private void ExecuteQuery5()
        {
            try
            {
                var result = from person in people
                             .AsParallel()
                             where CheckCity(person.City)
                             select person;
                
                result.ForAll(person => Console.WriteLine(person.Name));
            }
            catch (AggregateException e)
            {
                Console.WriteLine(e.InnerExceptions.Count + " exceptions.");
                foreach(Exception ex in e.InnerExceptions)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public static bool CheckCity(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException(name);
            return name == "Seattle";
        }

        class Person
        {
            public string Name { get; set; }
            public string City { get; set; }
        }
    }
}
