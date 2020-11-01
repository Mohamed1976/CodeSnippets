using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Discovery;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml;
using static _70_483_USING_NET_FRAMEWORK.Exercises.WcfServices;

namespace _70_483_USING_NET_FRAMEWORK.Exercises
{
    public class WcfExamples
    {
        public void Run()
        {
            //TransactionExample();

            return;
            //HostAndRunServices();
            //HostAndRunServicesV2();
            //HostAndRunServicesV3();

            /*
            https://docs.microsoft.com/en-us/dotnet/api/system.runtime.serialization.extensiondataobject?view=netcore-3.1
            https://docs.microsoft.com/en-us/dotnet/api/system.runtime.serialization.iextensibledataobject?view=netframework-4.8
            https://docs.microsoft.com/en-us/dotnet/framework/wcf/feature-details/forward-compatible-data-contracts
            https://docs.microsoft.com/en-us/dotnet/api/system.runtime.serialization.datacontractattribute?view=netframework-4.8
            */
            try
            {
                WriteVersion1("Version1ToFileDataContractExample.xml");
                ReadVersion2("Version1ToFileDataContractExample.xml");                
                WriteVersion2("DataContractExample.xml");
                WriteToVersion1("DataContractExample.xml");
                ReadVersion2("PersonV1_DataContractExample.xml");
            }
            catch (SerializationException exc)
            {
                Console.WriteLine("{0}: {1}", exc.Message, exc.StackTrace);
            }
            finally { }
        }

        //https://docs.microsoft.com/en-us/dotnet/framework/wcf/feature-details/how-to-create-a-transactional-service
        //https://docs.microsoft.com/en-us/dotnet/framework/wcf/samples/ws-transaction-flow
        //https://www.c-sharpcorner.com/UploadFile/e70b61/transaction-in-wcf/
        //https://www.codeproject.com/Articles/13523/Transactional-Web-Services-with-WS-AtomicTransacti
        private void TransactionExample()
        {
            try
            {
                Console.WriteLine("Opening TransactionExample().");
                const string url = "http://localhost:8733/MathService";
                Type type = typeof(MathService);

                using (ServiceHost host = new ServiceHost(type, new Uri(url)))
                {
                    host.AddServiceEndpoint(typeof(IMath), new WSHttpBinding() { TransactionFlow = true }, "");
                    //ServiceMetadataBehavior behavior = new ServiceMetadataBehavior();
                    //behavior.HttpsGetEnabled = true;
                    //host.Description.Behaviors.Add(behavior);
                    //host.Description.Behaviors.Add(new ServiceDiscoveryBehavior());
                    //host.AddServiceEndpoint(new UdpDiscoveryEndpoint());
                    //Start the Service
                    host.Open();
                    Console.WriteLine("Service is running.");

                    //Creating a client that will connect to the WCF service 
                    ChannelFactory<IMath> factory =
                    new ChannelFactory<IMath>(new WSHttpBinding() { TransactionFlow = true }, new EndpointAddress(url));
                    IMath proxy = factory.CreateChannel();
                    Console.WriteLine("Calling WSHttpBinding IMath service.");

                    double value1 = default;
                    double value2 = default;
                    double result = default;

                    // Start a transaction scope  
                    using (TransactionScope tx =
                                new TransactionScope(TransactionScopeOption.RequiresNew))
                    {
                        Console.WriteLine("Starting transaction");

                        // Call the Add service operation  
                        //  - generatedClient will flow the required active transaction  
                        value1 = 100.00D;
                        value2 = 15.99D;
                        result = proxy.Add(value1, value2);
                        Console.WriteLine("  Add({0},{1}) = {2}", value1, value2, result);

                        // Call the Subtract service operation  
                        //  - generatedClient will flow the allowed active transaction  
                        value1 = 145.00D;
                        value2 = 76.54D;
                        result = proxy.Subtract(value1, value2);
                        Console.WriteLine("  Subtract({0},{1}) = {2}", value1, value2, result);

                        // Start a transaction scope that suppresses the current transaction  
                        using (TransactionScope txSuppress =
                                    new TransactionScope(TransactionScopeOption.Suppress))
                        {
                            // Call the Subtract service operation  
                            //  - the active transaction is suppressed from the generatedClient  
                            //    and no transaction will flow  
                            value1 = 21.05D;
                            value2 = 42.16D;
                            result = proxy.Subtract(value1, value2);
                            Console.WriteLine("  Subtract({0},{1}) = {2}", value1, value2, result);

                            // Complete the suppressed scope  
                            txSuppress.Complete();
                        }

                        // Call the Multiply service operation  
                        // - generatedClient will not flow the active transaction  
                        value1 = 9.00D;
                        value2 = 81.25D;
                        result = proxy.Multiply(value1, value2);
                        Console.WriteLine("  Multiply({0},{1}) = {2}", value1, value2, result);

                        // Call the Divide service operation.  
                        // - generatedClient will not flow the active transaction  
                        value1 = 22.00D;
                        value2 = 7.00D;
                        result = proxy.Divide(value1, value2);
                        Console.WriteLine("  Divide({0},{1}) = {2}", value1, value2, result);

                        // Complete the transaction scope  
                        Console.WriteLine("  Completing transaction");
                        tx.Complete();
                    }

                    host.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void WriteVersion1(string path)
        {
            PersonV1 p1 = new PersonV1();
            p1.firstName = "Michael";
            p1.lastName = "Jobs";
            p1.Message = "Serialization of PersonV1 to file.";

            // Create the serializer using the version 1 type.
            DataContractSerializer ser = new DataContractSerializer(typeof(PersonV1));

            FileStream fs = new FileStream(path, FileMode.Create);
            ser.WriteObject(fs, p1);
            fs.Close();
        }

        // Create an instance of the version 2.0 class. It has
        // extra data (ID field) that version 1.0 does
        // not understand.
        static void WriteVersion2(string path)
        {
            Console.WriteLine("Creating a version 2 object");
            PersonV2 p2 = new PersonV2();
            p2.firstName = "Elizabeth";
            p2.lastName = "Middleton";
            p2.Note = "Serialization example using IExtensibleDataObject.";
            p2.ID = 2020;

            Console.WriteLine("Object data includes an ID");
            Console.WriteLine("\t firstName: {0}", p2.firstName);
            Console.WriteLine("\t lastName: {0}", p2.lastName);
            Console.WriteLine("\t Note: {0}", p2.Note);
            Console.WriteLine("\t ID: {0} \n", p2.ID);
            // Create an instance of the DataContractSerializer.
            DataContractSerializer ser =
                new DataContractSerializer(typeof(PersonV2));

            Console.WriteLine("Serializing the v2 object to file: {0}. \n\n", path);
            FileStream fs = new FileStream(path, FileMode.Create);
            ser.WriteObject(fs, p2);
            fs.Close();
        }

        // Deserialize version 2 data to a version 1 object.
        static void WriteToVersion1(string path)
        {
            // Create the serializer using the version 1 type.
            DataContractSerializer ser =
                new DataContractSerializer(typeof(PersonV1));

            FileStream fs = new FileStream(path, FileMode.Open);
            XmlDictionaryReader reader =
               XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());
            Console.WriteLine("Deserializing v2 data to v1 object. \n\n");

            PersonV1 p1 = (PersonV1)ser.ReadObject(reader, false);

            Console.WriteLine("V1 data has only a firstName, lastName, Message fields.");
            Console.WriteLine("But the v2 data (ID) is stored in the ExtensionData property. \n\n");
            Console.WriteLine("\t firstName: {0}, lastName: {1},  Message: {2}\n", 
                p1.firstName, 
                p1.lastName,
                p1.Message);

            fs.Close();

            // Change data in the object.
            p1.firstName = "John";
            Console.WriteLine("Changed the Name value to 'John' ");
            Console.Write("and reserializing the object to version 2 \n\n");
            // Reserialize the object.
            fs = new FileStream("PersonV1_" + path, FileMode.Create);
            ser.WriteObject(fs, p1);
            fs.Close();
        }

        // Deserialize a version 2.0 object.
        public static void ReadVersion2(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open);
            DataContractSerializer ser = new DataContractSerializer(typeof(PersonV2));

            Console.WriteLine("Deserializing new data to version 2 \n\n");
            PersonV2 p2 = (PersonV2)ser.ReadObject(fs);
            fs.Close();

            Console.WriteLine("ReadVersion2(string path)");
            Console.WriteLine("\t firstName: {0}, lastName: {1},  Message: {2}, ID: {3}\n",
                p2.firstName,
                p2.lastName,
                p2.Note,
                p2.ID);
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
