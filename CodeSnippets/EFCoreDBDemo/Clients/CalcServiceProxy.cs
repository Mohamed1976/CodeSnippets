﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreDBDemo.Clients
{
    //------------------------------------------------------------------------------
    // <auto-generated>
    //     This code was generated by a tool.
    //     Runtime Version:4.0.30319.42000
    //
    //     Changes to this file may cause incorrect behavior and will be lost if
    //     the code is regenerated.
    // </auto-generated>
    //------------------------------------------------------------------------------

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName = "ICalcService")]
    public interface ICalcService
    {

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ICalcService/Add", ReplyAction = "http://tempuri.org/ICalcService/AddResponse")]
        double Add(double n1, double n2);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ICalcService/Add", ReplyAction = "http://tempuri.org/ICalcService/AddResponse")]
        System.Threading.Tasks.Task<double> AddAsync(double n1, double n2);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ICalcService/Subtract", ReplyAction = "http://tempuri.org/ICalcService/SubtractResponse")]
        double Subtract(double n1, double n2);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ICalcService/Subtract", ReplyAction = "http://tempuri.org/ICalcService/SubtractResponse")]
        System.Threading.Tasks.Task<double> SubtractAsync(double n1, double n2);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ICalcService/Multiply", ReplyAction = "http://tempuri.org/ICalcService/MultiplyResponse")]
        double Multiply(double n1, double n2);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ICalcService/Multiply", ReplyAction = "http://tempuri.org/ICalcService/MultiplyResponse")]
        System.Threading.Tasks.Task<double> MultiplyAsync(double n1, double n2);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ICalcService/Divide", ReplyAction = "http://tempuri.org/ICalcService/DivideResponse")]
        double Divide(double n1, double n2);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ICalcService/Divide", ReplyAction = "http://tempuri.org/ICalcService/DivideResponse")]
        System.Threading.Tasks.Task<double> DivideAsync(double n1, double n2);
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ICalcServiceChannel : ICalcService, System.ServiceModel.IClientChannel
    {
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CalcServiceClient : System.ServiceModel.ClientBase<ICalcService>, ICalcService
    {

        public CalcServiceClient()
        {
        }

        public CalcServiceClient(string endpointConfigurationName) :
                base(endpointConfigurationName)
        {
        }

        public CalcServiceClient(string endpointConfigurationName, string remoteAddress) :
                base(endpointConfigurationName, remoteAddress)
        {
        }

        public CalcServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
                base(endpointConfigurationName, remoteAddress)
        {
        }

        public CalcServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
                base(binding, remoteAddress)
        {
        }

        public double Add(double n1, double n2)
        {
            return base.Channel.Add(n1, n2);
        }

        public System.Threading.Tasks.Task<double> AddAsync(double n1, double n2)
        {
            return base.Channel.AddAsync(n1, n2);
        }

        public double Subtract(double n1, double n2)
        {
            return base.Channel.Subtract(n1, n2);
        }

        public System.Threading.Tasks.Task<double> SubtractAsync(double n1, double n2)
        {
            return base.Channel.SubtractAsync(n1, n2);
        }

        public double Multiply(double n1, double n2)
        {
            return base.Channel.Multiply(n1, n2);
        }

        public System.Threading.Tasks.Task<double> MultiplyAsync(double n1, double n2)
        {
            return base.Channel.MultiplyAsync(n1, n2);
        }

        public double Divide(double n1, double n2)
        {
            return base.Channel.Divide(n1, n2);
        }

        public System.Threading.Tasks.Task<double> DivideAsync(double n1, double n2)
        {
            return base.Channel.DivideAsync(n1, n2);
        }
    }
}
