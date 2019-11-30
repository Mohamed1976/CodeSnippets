using System;
using System.Collections.Generic;
using System.Text;

namespace CodeSnippets.Implementing_Value_Equality
{
    // represents a basic employee
    // with value equality 
    // semantics
    public class Customer :
    // implementing this interface tells the .NET
    // framework classes that we can compare based on 
    // value equality.
    IEquatable<Customer>
    {
        public Customer(int id, string name, string title, DateTime birthday)
        {
            Id = id;
            Name = name;
            Title = title;
            Birthday = birthday;
        }

        public int Id { get; }
        public string Name { get; }
        public string Title { get; }
        public DateTime Birthday { get; }

        // implementation of 
        // IEqualityComparer<Employee2>.Equals()
        public bool Equals(Customer customer)
        {
            // short circuit if rhs and this
            // refer to the same memory location
            // (reference equality)
            if (ReferenceEquals(customer, this))
                return true;
            // short circuit for nulls
            if (ReferenceEquals(customer, null))
                return false;
            // compare each of the fields
            return Id == customer.Id &&
                0 == string.Compare(Name, customer.Name) &&
                0 == string.Compare(Title, customer.Title) &&
                Birthday == customer.Birthday;
        }

        // basic .NET value equality support
        public override bool Equals(object obj)
            => Equals(obj as Customer);

        // gets the hashcode based on the value
        // of Employee2. The hashcodes MUST be
        // the same for any Employee2 that
        // equals another Employee2!
        public override int GetHashCode()
        {
            // go through each of the fields,
            // getting the hashcode, taking
            // care to check for null strings
            // we XOR the hashcodes together
            // to get a result
            var result = Id.GetHashCode();
            if (null != Name)
                result ^= Name.GetHashCode();
            if (null != Title)
                result ^= Title.GetHashCode();
            result ^= Birthday.GetHashCode();
            return result;
        }

        // enable == support in C#
        public static bool operator ==(Customer customerX, Customer customerY)
        {
            // short circuit for reference equality
            if (ReferenceEquals(customerX, customerY))
                return true;
            // short circuit for null
            if (ReferenceEquals(customerX, null) || ReferenceEquals(customerY, null))
                return false;
            return customerX.Equals(customerY);
        }

        // enable != support in C#
        public static bool operator !=(Customer customerX, Customer customerY)
        {
            //Instead of defining !=, you can call the == operator and inverse the value.  
            return !(customerX == customerY);
            // essentially the reverse of ==
            //if (ReferenceEquals(customerX, customerY))
            //    return false;
            //if (ReferenceEquals(customerX, null) || ReferenceEquals(customerY, null))
            //    return true;
            //return !customerX.Equals(customerY);
        }
    }
}
