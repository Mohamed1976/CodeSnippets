using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Discovery;
using System.Text;
using System.Threading.Tasks;
using static _70_483_USING_NET_FRAMEWORK.Exercises.WcfServices;

namespace _70_483_USING_NET_FRAMEWORK.Exercises
{
    public class WcfExamples
    {
        public void Run()
        {
            //HostAndRunServices();
            //HostAndRunServicesV2();
            ///HostAndRunServicesV3();
        }

        private void HostAndRunServicesV3()
        {
            Console.WriteLine("Opening Host.");
            const string url = "http://localhost:8733/StockTicker";
            Type type = typeof(CalculatorService);

            using (ServiceHost host = new ServiceHost(type, new Uri(url)))
            {
                host.AddServiceEndpoint(typeof(ICalculator), new WSHttpBinding(), "");
                host.Description.Behaviors.Add(new ServiceDiscoveryBehavior());
                host.AddServiceEndpoint(new UdpDiscoveryEndpoint());
                //Start the Service
                host.Open();

                //Creating a client that will connect to the WCF service 
                ChannelFactory<ICalculator> factory =
                    new ChannelFactory<ICalculator>(new WSHttpBinding(), new EndpointAddress(url));
                ICalculator proxy = factory.CreateChannel();

                Console.WriteLine("Calling HTTP ICalculator service.");
                Console.WriteLine(proxy.Add(200, 750));
                Console.WriteLine(proxy.Echo("Hello world"));

                host.Close();
            }
        }

        //http://wcftutorial.net/wcf-self-hosting.aspx
        //https://www.oreilly.com/library/view/learning-wcf/9780596101626/ch04s02.html
        //https://www.codemag.com/Article/0701041/Hosting-WCF-Services
        //https://docs.microsoft.com/en-us/dotnet/framework/wcf/samples/custom-service-host
        private void HostAndRunServicesV2()
        {
            Console.WriteLine("Opening Host.");
            const string url = "http://localhost:8090/MyService/SimpleCalculator";
            //const string tcpUrl = "net.tcp://localhost:8090/MyService/SimpleCalculator";
            //Uri tcp = new Uri(tcpUrl);
            //Create a URI to serve as the base address
            Uri httpUrl = new Uri(url);
            //Create ServiceHost
            ServiceHost host = new ServiceHost(typeof(CalculatorService), httpUrl);
            //You can register multiple base addresses separated by commas, but address should not use same transport schema.
            //ServiceHost host = new ServiceHost(typeof(CalculatorService), httpUrl, tcp);

            //Add a service endpoint
            host.AddServiceEndpoint(typeof(ICalculator), new WSHttpBinding(), "");

            //A TCP error (10013: An attempt was made to access a socket in a way forbidden by its access permissions) occurred while listening on IP Endpoint=0.0.0.0:8090.
            //host.AddServiceEndpoint(typeof(ICalculator), new NetTcpBinding(), "");

            //Enable metadata exchange
            ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
            smb.HttpGetEnabled = true;
            host.Description.Behaviors.Add(smb);
            //Start the Service
            host.Open();
            Console.WriteLine("Host opened.");

            //Creating a client that will connect to the WCF service 
            ChannelFactory<ICalculator> factory = 
                new ChannelFactory<ICalculator>(new WSHttpBinding(), new EndpointAddress(url));
            ICalculator proxy = factory.CreateChannel();

            Console.WriteLine("Calling HTTP ICalculator service.");
            Console.WriteLine(proxy.Add(123, 456));
            Console.WriteLine(proxy.Echo("Hello world"));

            //Error when using NetTcpBinding
            //You have tried to create a channel to a service that does not support .Net Framing. 
            //It is possible that you are encountering an HTTP endpoint.
            //ChannelFactory<ICalculator> factory2 =
            //    new ChannelFactory<ICalculator>(new NetTcpBinding(), new EndpointAddress(tcpUrl));
            //ICalculator proxy2 = factory2.CreateChannel();

            //Console.WriteLine("Calling TCP ICalculator service.");
            //Console.WriteLine(proxy2.Add(299, 118));
            //Console.WriteLine(proxy2.Echo("Another day @work"));

            // Close the ServiceHost.
            host.Close();
        }

        //Run VS as Administrator or
        //netsh http add urlacl url=http://+:8000/ServiceModelSamples/Service user=mylocaluser
        //https://www.codeproject.com/Questions/310360/HTTP-could-not-register-URL-http-plus-8000-HelloWC
        //static readonly string ServiceBaseAddress = "http://" + Environment.MachineName + ":8000/Service";
        //https://stackoverflow.com/questions/8727293/http-could-not-register-url-http-8000-hellowcf-your-process-does-not-have
        //https://docs.microsoft.com/en-us/dotnet/framework/wcf/feature-details/configuring-http-and-https?redirectedfrom=MSDN
        static readonly string ServiceBaseAddress = "http://localhost:8733/ServiceModelSamples/service";

        //https://docs.microsoft.com/en-us/dotnet/framework/wcf/feature-details/calling-a-rest-style-service-from-a-wcf-service
        //https://docs.microsoft.com/en-us/dotnet/framework/wcf/feature-details/how-to-host-a-wcf-service-in-a-managed-windows-service
        //https://docs.microsoft.com/en-us/dotnet/api/system.servicemodel.servicehost?redirectedfrom=MSDN&view=netframework-4.8
        private void HostAndRunServices()
        {
            using (ServiceHost serviceHost = new ServiceHost(typeof(CalculatorService), new Uri(ServiceBaseAddress)))
            {
                try
                {
                    serviceHost.AddServiceEndpoint(typeof(ICalculator), new BasicHttpBinding(), "");
                    serviceHost.Open();

                    Console.WriteLine("Host opened.");

                    //Creating a client that will connect to the WCF service 
                    ChannelFactory<ICalculator> factory = new ChannelFactory<ICalculator>(new BasicHttpBinding(), new EndpointAddress(ServiceBaseAddress));
                    ICalculator proxy = factory.CreateChannel();

                    Console.WriteLine("Calling ICalculator service.");
                    Console.WriteLine(proxy.Add(123, 456));
                    Console.WriteLine(proxy.Echo("Hello world"));

                    // Close the ServiceHost.
                    serviceHost.Close();
                }
                catch (TimeoutException timeProblem)
                {
                    Console.WriteLine(timeProblem.Message);
                }
                catch (CommunicationException commProblem)
                {
                    Console.WriteLine(commProblem.Message);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            //Console.WriteLine("Starting WCF service, using selfhosting.");

            //ServiceHost normalHost = new ServiceHost(typeof(CalculatorService), new Uri(ServiceBaseAddress));
            //normalHost.AddServiceEndpoint(typeof(ICalculator), new BasicHttpBinding(), "");
            //normalHost.Open();

            //Console.WriteLine("Host opened.");

            ////Creating a client that will connect to the WCF service 
            //ChannelFactory<ICalculator> factory = new ChannelFactory<ICalculator>(new BasicHttpBinding(), new EndpointAddress(ServiceBaseAddress));
            //ICalculator proxy = factory.CreateChannel();

            //Console.WriteLine("Calling ICalculator service.");
            //Console.WriteLine(proxy.Add(123, 456));
            //Console.WriteLine(proxy.Echo("Hello world"));
        }
    }
}
