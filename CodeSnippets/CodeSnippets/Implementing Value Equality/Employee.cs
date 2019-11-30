using System;
using System.Collections.Generic;
using System.Text;

namespace CodeSnippets.Implementing_Value_Equality
{
    public class Employee

    {
        //Parametrized constructor to initialize readonly properties.
        public Employee(int id, string firstName, string lastName, DateTime birthday)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Birthday = birthday;
        }

        public int Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        //Readonly property without set property, can only be set in constructor
        public DateTime Birthday { get; }
    }
}
