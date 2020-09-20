using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace WcfServiceLibrary
{
    [ServiceContract]
    public interface ISaleService
    {
        [OperationContract]
        bool InsertCustomer(Customer obj);

        [OperationContract, WebGet] //Check when to use WebGet, WCF as rest service 
        List<Customer> GetAllCustomer();

        [OperationContract]
        bool DeleteCustomer(int Cid);

        [OperationContract]
        bool UpdateCustomer(Customer obj);
    }

    [DataContract]
    public class Customer
    {
        [DataMember]
        public int CustomerID;
        [DataMember]
        public string CustomerName;
        [DataMember]
        public string Address;
        [DataMember]
        public string EmailId;
    }
}
