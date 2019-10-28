using System;
using System.Collections.Generic;
using System.Text;

namespace ExtensionMethodsExamples.Domains
{
    public class Person
    {
        public Person()
        {
        }

        private string firstname;

        public string Firstname
        {
            get { return firstname; }
            set { firstname = value; }
        }

        private string lastname;

        public string Lastname
        {
            get { return lastname; }
            set { lastname = value; }
        }
    }
}
