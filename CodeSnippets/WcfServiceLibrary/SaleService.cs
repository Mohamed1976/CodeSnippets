using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WcfServiceLibrary
{
    //https://www.codeproject.com/Tips/468354/WCF-Example-for-Inserting-Deleting-and-Displaying
    public class SaleService : ISaleService
    {
        private static List<Customer> cutomerList = new List<Customer>()
        {
            new Customer {CustomerID = 1, CustomerName="Sujeet", Address="Pune", EmailId="test@yahoo.com" },
            new Customer {CustomerID = 2, CustomerName="Rahul", Address="Pune", EmailId="test@yahoo.com" },
            new Customer {CustomerID = 3, CustomerName="Mayur", Address="Pune", EmailId="test@yahoo.com"},
            new Customer {CustomerID = 4, CustomerName="John", Address="MClane", EmailId="John@yahoo.com"}
        };

        public bool InsertCustomer(Customer obj)
        {
            cutomerList.Add(obj);
            return true;
        }

        public List<Customer> GetAllCustomer()
        {
            return cutomerList;
        }

        private const int min = 1;
        private const int max = 4;
        //There are two basic types of interceptors you should be familiar with: 
        //ChangeInterceptors and QueryInterceptors.As the names imply, they have 
        //distinct usages depending on what you are looking for. ChangeInterceptors 
        //are used for NON-Query operations; QueryInterceptors are used for Query operations.
        //ChangeInterceptors are used for all nonquery operations and have no return type
        //They must accept two parameters: Type and UpdateOperations
        //Similarly, you can tell by the behavior which is which and which is being asked for. 
        //DeleteTopics would be a ChangeInterceptor question; a GetAdvancedTopics method asking 
        //about filtering Topics entity would be a QueryInterceptor question.
        //A QueryInterceptor is triggered on the GET Requests on an EntitySet, 
        //which you can use to check authorization, or to filter out certain 
        //requests you don’t want people to see.
        //https://docs.microsoft.com/en-us/dotnet/api/system.data.services.queryinterceptorattribute?view=netframework-4.8
        //http://failedturing.blogspot.com/2017/05/microsoft-70-487-create-and-implement.html
        //https://docs.microsoft.com/en-us/dotnet/framework/data/wcf/interceptors-wcf-data-services
        //https://docs.microsoft.com/en-us/dotnet/framework/data/wcf/how-to-intercept-data-service-messages-wcf-data-services
        [QueryInterceptor("Customers")]
        public Expression<Func<Customer, bool>> OnQueryCustomers()
        {
            return c => c.CustomerID > min && c.CustomerID < max;
        }

        //The ChangeInterceptor responds to "UpdateOperations" that occur on an 
        //individual entity, which includes Add, Change, and Delete operations.  
        //In order for the change interceptor to trigger, write operations much be 
        //enabled to the entity (otherwise you can't change it... simple).
        [ChangeInterceptor("Customers")]
        public void Alarm(Customer customer, UpdateOperations operations)
        {
            if (operations == UpdateOperations.Delete)
            {
                throw new DataServiceException(400,
                    "Customer cannot be deleted! That's cruel!");
            }
            /*
                if ( operations == UpdateOperations.Add ||
                   operations == UpdateOperations.Change )
               {
                   if ( car.Make == "VW" )
                       throw new DataServiceException( 400,
                           "A car with make equals to VW, cannot be modified" );
               }
               else if ( operations == UpdateOperations.Delete )
               {
                   throw new DataServiceException( 400,
                       "Cars cannot be deleted" );
               }
            */
        }

        public bool DeleteCustomer(int Cid)
        {
            var item = cutomerList.First(x => x.CustomerID == Cid);

            cutomerList.Remove(item);
            return true;
        }

        public bool UpdateCustomer(Customer obj)
        {
            var list = cutomerList;
            cutomerList.Where(p => p.CustomerID ==
            obj.CustomerID).Update(p => p.CustomerName = obj.CustomerName);
            return true;
        }
    }

    public static class LinqUpdates
    {
        public static void Update<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
                action(item);
        }
    }
}
