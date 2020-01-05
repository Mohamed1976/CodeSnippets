using System;
using System.Collections.Generic;
using System.Text;

namespace _70_483.EventsAndCallbacks
{
    class DelegateExampleMethods
    {
        public DelegateExampleMethods(string name)
        {
            Name = name;
        }

        private string name;

        public string Name
        {
            get { return name; }
            private set { name = value; }
        }

        public void Method1(string message) 
        {
            Console.WriteLine($"Method1(string message) message{message}, Name[{Name}]");
        }
        public void Method2(string message) 
        {
            Console.WriteLine($"Method2(string message) message{message}, Name[{Name}]");
        }
    }
}
