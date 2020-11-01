using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreDBDemo.Exercises
{
    //You use the HttpClient object to process order entries. The API throws SocketException errors 
    //when the Web App experiences a high volume of concurrent users.
    //Use the static modifier to declare the HttpClient object.
    //This is because if the HttpClient is disposed, new sockets will be allocated for each request, 
    //exhausting the pool of sockets if a large number of requests
    //are run in a short period of time (throwing the described exception). 
    //Reusing the same HttpClient instance will make sure that sockets are reused whenever possible.

    //free example restful web services
    //A List of REST Web-Services to try out for free
    //https://medium.com/@anoopm6/a-list-of-rest-web-services-to-try-out-for-free-63a641ba7dab
    //HttpClient: reuses recently resolved DNS lookups
    //Use IHttpClientFactory to implement resilient HTTP requests
    //https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests
    public class GeneralExercises
    {
        //If the class that wraps the external resource is shareable and thread-safe, create a shared 
        //singleton instance or a pool of reusable instances of the class.
        //https://docs.microsoft.com/en-us/azure/architecture/antipatterns/improper-instantiation/
        private static readonly HttpClient _httpClient;
        static GeneralExercises()
        {
            _httpClient = new HttpClient();
        }

        public GeneralExercises()
        {
        }

        public async Task<int> Run()
        {

            return 1;
            string content = await 
                GetMessage(new Uri("http://www.herongyang.com/Service/Hello_REST.php"));
            Console.WriteLine(content);

            content = await
                GetMessage(new Uri("https://restcountries.eu/rest/v2/alpha/col"));
            Console.WriteLine(content + "\n\n");

            content = await GetMessageV2(new Uri("https://restcountries.eu/rest/v2/alpha/col"));
            Console.WriteLine(content);

            return 1;
        }

        private Task<string> GetMessageV2(Uri uri)
        {
            Task<string> content = _httpClient.GetStringAsync(uri);
            ShowMsg();
            return content;
        }

        private void ShowMsg()
        {
            for(int i = 0; i < 3; i++)
            {
                Task.Delay(500).Wait();
                Console.WriteLine(DateTime.Now.ToString());
            }                
        }

        /*
        https://stackoverflow.com/questions/35722586/header-parameters-accept-and-content-type-in-a-rest-context
        - Accept header is used by HTTP clients to tell the server which type of content they expect/prefer as response.
        - Content-type can be used both by clients and servers to identify the format of the data in their request (client) 
        or response (server) and, therefore, help the other part interpret correctly the information.
        Since we are the calling client in this case we need to use the accept header to inform the server we want JSON.
        */
        private async Task<string> GetMessage(Uri uri)
        {
            //HttpClient httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Accept","application/json");
            //You need to add a header to ensure that data is returned as XML.
            //httpClient.DefaultRequestHeaders.Add("Accept", "application/xml");
            string content = await _httpClient.GetStringAsync(uri);
            return content;
        }
    }
}
