using System;
using System.Collections.Generic;
using System.Text;

namespace ExtensionMethodsExamples.Domains
{
    public static class PersonExtensions
    {
        public static Person Fill(this Person person)
        {
            person.Firstname = "myFirstname";
            person.Lastname = "myLastname";
            return person;
        }

        public static void Print(this Person person)
        {
            Console.WriteLine(person.Firstname);
            Console.WriteLine(person.Lastname);
        }
    }
}
