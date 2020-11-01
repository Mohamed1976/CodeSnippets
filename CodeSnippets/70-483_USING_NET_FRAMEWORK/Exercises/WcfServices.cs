using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace _70_483_USING_NET_FRAMEWORK.Exercises
{
    public class WcfServices
    {
        [ServiceContract]
        public interface ICalculator
        {
            [OperationContract]
            long Add(long x, long y);

            [OperationContract]
            long Subtract(long x, long y);

            [OperationContract]
            long Multiply(long x, long y);

            [OperationContract]
            long Divide(long x, long y);

            [OperationContract]
            long Mod(long x, long y);

            [OperationContract]
            string Echo(string input);
        }

        public class CalculatorService : ICalculator
        {
            public long Add(long x, long y)
            {
                return x + y;
            }

            public long Divide(long x, long y)
            {
                throw new NotImplementedException();
            }

            public long Mod(long x, long y)
            {
                throw new NotImplementedException();
            }

            public long Multiply(long x, long y)
            {
                throw new NotImplementedException();
            }

            public long Subtract(long x, long y)
            {
                throw new NotImplementedException();
            }

            public string Echo(string input)
            {
                return input;
            }
        }

        // Implement the IExtensibleDataObject interface
        // to store the extra data for future versions.
        [DataContract(Name = "Person", Namespace = "http://moccasoft.wcf.documentation")]
        public class PersonV1 : IExtensibleDataObject
        {
            [DataMember]
            public string firstName;
            [DataMember]
            public string lastName;
            [DataMember]
            public string Message;
            //[DataMember]
            //public XmlNode[] Blob;

            #region IExtensibleDataObject Members
            private ExtensionDataObject data = null;
            // To implement the IExtensibleDataObject interface,
            // you must implement the ExtensionData property. The property
            // holds data from future versions of the class for backward
            // compatibility.
            public ExtensionDataObject ExtensionData
            {
                get
                {
                    return this.data;
                }
                set
                {
                    this.data = value;
                }
            }
            #endregion
        }

        // The second version of the class adds a new field. The field's
        // data is stored in the ExtensionDataObject field of
        // the first version (Person). You must also set the Name property
        // of the DataContractAttribute to match the first version.
        // If necessary, also set the Namespace property so that the
        // name of the contracts is the same.
        [DataContract(Name = "Person", Namespace = "http://moccasoft.wcf.documentation")]
        public class PersonV2 : IExtensibleDataObject
        {
            [DataMember]
            public string firstName;
            [DataMember]
            public string lastName;
            [DataMember(Name = "Message")]
            public string Note;
            //[DataMember]
            //public XmlNode[] Blob;

            // Best practice: add an Order number to new members.
            [DataMember(Order = 4, EmitDefaultValue = true)]
            public int ID;

            #region IExtensibleDataObject Members
            private ExtensionDataObject data = null;

            public ExtensionDataObject ExtensionData
            {
                get
                {
                    return this.data;
                }
                set
                {
                    this.data = value;
                }
            }
            #endregion
        }

        /*
        https://docs.microsoft.com/en-us/dotnet/framework/wcf/samples/ws-transaction-flow
        */
        [ServiceContract(Namespace = "http://Moccasoft.ServiceModel.Samples")]
        public interface IMath
        {
            [OperationContract]
            //An Add operation request must include a flowed transaction.
            [TransactionFlow(TransactionFlowOption.Mandatory)]
            double Add(double n1, double n2);

            [OperationContract]
            //A Subtract operation request may include a flowed transaction.
            [TransactionFlow(TransactionFlowOption.Allowed)]
            double Subtract(double n1, double n2);

            //A Multiply operation request must not include a flowed transaction 
            //through the explicit NotAllowed setting.            
            [OperationContract]
            [TransactionFlow(TransactionFlowOption.NotAllowed)]
            double Multiply(double n1, double n2);

            //A Divide operation request must not include a flowed transaction through 
            //the omission of a TransactionFlow attribute.
            [OperationContract]
            double Divide(double n1, double n2);
        }

        // Service class that implements the service contract.  
        [ServiceBehavior(TransactionIsolationLevel = System.Transactions.IsolationLevel.Serializable,
            TransactionTimeout = "00:00:30")] // Operations must have 30 seconds timeouts.  
        public class MathService : IMath
        {
            //Transactions must flow from the client to the server.
            //For the class that implements the ICalculator interface, all of the methods 
            //are attributed with TransactionScopeRequired property set to true. This setting 
            //declares that all actions taken within the method occur within the scope of a 
            //transaction. In this case, the actions taken include recording to the log database. 
            //If the operation request includes a flowed transaction then the actions occur within 
            //the scope of the incoming transaction or a new transaction scope is automatically generated.
            [OperationBehavior(TransactionScopeRequired = true)]
            public double Add(double n1, double n2)
            {
                RecordToLog(String.Format(CultureInfo.CurrentCulture, "Adding {0} to {1}", n1, n2));
                return n1 + n2;
            }

            [OperationBehavior(TransactionScopeRequired = true)]
            public double Subtract(double n1, double n2)
            {
                RecordToLog(String.Format(CultureInfo.CurrentCulture, "Subtracting {0} from {1}", n2, n1));
                return n1 - n2;
            }

            [OperationBehavior(TransactionScopeRequired = true)]
            public double Multiply(double n1, double n2)
            {
                RecordToLog(String.Format(CultureInfo.CurrentCulture, "Multiplying {0} by {1}", n1, n2));
                return n1 * n2;
            }

            [OperationBehavior(TransactionScopeRequired = true)]
            public double Divide(double n1, double n2)
            {
                RecordToLog(String.Format(CultureInfo.CurrentCulture, "Dividing {0} by {1}", n1, n2));
                return n1 / n2;
            }

            private void RecordToLog(string logMsg)
            {
                Console.WriteLine("RecordToLog: " + logMsg);
            }
        }
    }
}