using CalcServiceReference;
using EFCoreDBDemo.Clients;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreDBDemo.Exercises
{
    public class WCFExamples
    {
        public async Task Run()
        {
            //Console.WriteLine("Please press key to contact WCF.");
            //Console.ReadKey();
            //ChannelFactoryExample();

            //TODO, configuration files are not supported .net core
            //Console.WriteLine("Please press key to contact WCF.");
            //Console.ReadKey();
            //await CalcOperation();
        }

        //https://docs.microsoft.com/en-us/dotnet/framework/wcf/feature-details/how-to-use-the-channelfactory
        private void ChannelFactoryExample()
        {
            // Create a channel factory.
            BasicHttpBinding myBinding = new BasicHttpBinding();
            EndpointAddress myEndpoint = new EndpointAddress("http://localhost:8733/Design_Time_Addresses/WcfServiceLibrary/CalcService");
            ChannelFactory<ICalcService> myChannelFactory = new ChannelFactory<ICalcService>(myBinding, myEndpoint);
            // Create a channel.
            ICalcService wcfClient1 = myChannelFactory.CreateChannel();

            double s = wcfClient1.Add(3, 39);
            Console.WriteLine($"3 + 39 = {s.ToString()}");
            ((IClientChannel)wcfClient1).Close();

            // Create another channel.
            ICalcService wcfClient2 = myChannelFactory.CreateChannel();
            s = wcfClient2.Add(15, 27);
            Console.WriteLine($"15 + 27 = {s.ToString()}");
            ((IClientChannel)wcfClient2).Close();
            myChannelFactory.Close();
        }

        //Generating proxies by creating a service reference (Add Service Reference)
        private void CalcOperationService()
        {
            Service1Client client = new Service1Client();
        }

        /* Creating WCF client using svcutil.exe
        
        You need to install System.ServiceModel.Primitives nuget

        C:\temp>"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools\x64\svcutil.exe" /language:c# 
        /out:CalcServiceProxy.cs /config:app.config http://localhost:8733/Design_Time_Addresses/WcfServiceLibrary/CalcService

        Microsoft (R) Service Model Metadata Tool
        [Microsoft (R) Windows (R) Communication Foundation, Version 4.8.3928.0]
        Copyright (c) Microsoft Corporation.  All rights reserved.

        Attempting to download metadata from 'http://localhost:8733/Design_Time_Addresses/WcfServiceLibrary/CalcService' using WS-Metadata Exchange or DISCO.
        Generating files...
        C:\temp\CalcServiceProxy.cs
        C:\temp\app.config
        */

        /* The generated App.config svcutil.exe:
        Note if you add App.config to project, DBContext starts looking in the file and reports error 
        because it cannot find a specific section.  

        <?xml version="1.0" encoding="utf-8" ?>
        <configuration>
            <system.serviceModel>
                <bindings>
                    <basicHttpBinding>
                        <binding name="BasicHttpBinding_ICalcService" />
                    </basicHttpBinding>
                </bindings>
                <client>
                    <endpoint address="http://localhost:8733/Design_Time_Addresses/WcfServiceLibrary/CalcService/"
                        binding="basicHttpBinding" bindingConfiguration="BasicHttpBinding_ICalcService"
                        contract="ICalcService" name="BasicHttpBinding_ICalcService" />
                </client>
            </system.serviceModel>
        </configuration>
        */
        private async Task CalcOperation()
        {
            CalcServiceClient client = new CalcServiceClient();
            double result = client.Add(2.55, 6.67);
            Console.WriteLine($"2.55 + 6.67 = {result}");

            result = await client.AddAsync(6.78, 23.89);
            Console.WriteLine($"6.78 + 23.89 = {result}");
        }
    }
}
