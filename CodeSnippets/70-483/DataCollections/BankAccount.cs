using System;
using System.Collections.Generic;
using System.Text;

namespace _70_483.DataCollections
{
    public class BankAccount
    {
        public int AccountNo;
        public string Name;

        public override string ToString()
        {
            return AccountNo + ", " + Name;
        }
    }
}
