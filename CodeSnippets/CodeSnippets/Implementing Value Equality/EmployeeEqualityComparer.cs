using System;
using System.Collections.Generic;
using System.Text;

namespace CodeSnippets.Implementing_Value_Equality
{
    // a class for comparing two employees for equality
    // this class is used by the framework in classes like
    // Dictionary<TKey,TValue> to do key comparisons.
    public class EmployeeEqualityComparer : IEqualityComparer<Employee>
    {
        // static singleton field
        public static readonly EmployeeEqualityComparer Default = new EmployeeEqualityComparer();

        // compare two employee instances for equality
        public bool Equals(Employee employeeX, Employee employeeY)
        {
            // Always check this first to avoid unnecessary work
            // If two objects reference same memory, therefore they are equal 
            if (ReferenceEquals(employeeX, employeeY)) return true;
            // short circuit for nulls
            if (ReferenceEquals(employeeX, null) || ReferenceEquals(employeeY, null))
                return false;
            //compare each of the fields
            return employeeX.Id == employeeY.Id &&
                0 == string.Compare(employeeX.FirstName, employeeY.FirstName) &&
                0 == string.Compare(employeeX.LastName, employeeY.LastName) &&
                employeeX.Birthday == employeeY.Birthday;

            //bool areEqual = employeeX.Id == employeeY.Id &&
            //    0 == string.Compare(employeeX.FirstName, employeeY.FirstName) &&
            //    0 == string.Compare(employeeX.LastName, employeeY.LastName) &&
            //    employeeX.Birthday == employeeY.Birthday;

            //return areEqual;
        }

        // gets the hashcode for the employee
        // this value must be the same as long
        // as the fields are the same.
        public int GetHashCode(Employee employee)
        {
            // short circuit for null
            if (null == employee) return 0;
            // get the hashcode for each field
            // taking care to check for nulls
            // we XOR the hashcodes for the 
            // result
            var result = employee.Id.GetHashCode();
            if (null != employee.FirstName)
                result ^= employee.FirstName.GetHashCode();
            if (null != employee.LastName)
                result ^= employee.LastName.GetHashCode();
            result ^= employee.Birthday.GetHashCode();
            return result;
        }

        //TODO find a good standard procedure to generate GetHashCode(), is exclusive OR operator ^ a good way?? 
        //https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/boolean-logical-operators
        //Logical exclusive OR operator ^
        //Console.WriteLine(true ^ true);    // output: False
        //Console.WriteLine(true ^ false);   // output: True
        //Console.WriteLine(false ^ true);   // output: True
        //Console.WriteLine(false ^ false);  // output: False

    }
}
