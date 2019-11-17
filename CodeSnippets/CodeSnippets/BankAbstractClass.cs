using System;
using System.Collections.Generic;
using System.Text;

namespace BankNamespace
{
    // Abstract class for all bank customers
    public abstract class BankAbstractClass
    {
        // Declare the GetCustomerType() default method here
        public abstract int GetCustomerType();

        // Implement the GetCustomerTypeName() default method here
        public abstract string GetCustomerTypeName();

        // Implement the GetCountryRegion() default method here
        public virtual string GetCountryRegion()
        {
            return "public virtual string GetCountryRegion() UK/EU";
        }
    }

    //In the class below, since the region remains the same, which is "UK/EU", so we did not override 
    //the method, GetCountryRegion(), for which we have used the virtual keyword.But we had to override 
    //the methods, GetCustomerType() and GetCustomerTypeName(), since the implementation of these two methods 
    //will always vary with each derived class.
    public class UKEUBankCustomer : BankAbstractClass
    {
        private string CustomerName;

        public UKEUBankCustomer(string CustomerName)
        {
            this.CustomerName = CustomerName;
        }

        // Get Customer Name
        public string GetName()
        {
            return CustomerName;
        }

        // Using override to return the already
        //implemented method, GetCustomerType() of Base class, which is CustomerAbstractClass
        public override int GetCustomerType()
        {
            return 1;
        }

        // Using override to return the already
        // implemented method, GetCustomerTypeName() of Base class, which is CustomerAbstractClass
        public override string GetCustomerTypeName()
        {
            return "UK/EU Citizen";
        }

        /*
        // If not required implementing
        public override int GetCustomerType()
        {
            throw new NotImplementedException();
        }
 
        // If not required implementing
        public override string GetCustomerTypeName()
        {
            throw new NotImplementedException();
        }
        */
    }

    public class NonUKEUBankCustomer : BankAbstractClass
    {
        private string CustomerName;

        public NonUKEUBankCustomer(string CustomerName)
        {
            this.CustomerName = CustomerName;
        }

        // Get Customer Name
        public string GetName()
        {
            return CustomerName;
        }

        // Using override to return the already
        //implemented method, GetCustomerType() of Base class, which is CustomerAbstractClass
        public override int GetCustomerType()
        {
            return 2;
        }

        // Using override to return the already
        //implemented method, GetCustomerTypeName() of Base class, which is CustomerAbstractClass
        public override string GetCustomerTypeName()
        {
            return "Non UK/EU Citizen";
        }

        //Notice, in this one, we implement the method GetCountryRegion() method but we did not override 
        //the original implementation from the base class. We could have easily overriden the method of 
        //the base class easily but the reason to use the new keyword is to show that even if we do not 
        //override a virtual method of the base class but we can still implement as new method/property 
        //in the derived class with the same name without any issue.
        /*         
        public new string GetCountryRegion()
        {
            return "Non UK/EU - Rest of the world";
        }         
        */

        // Implement the GetCountryRegion() default method here
        public override string GetCountryRegion()
        {
            return "Non UK/EU - British Island";
        }
    }
}
