using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCoreWebApp.Helpers
{
    public class BankAccountManager
    {
        protected string _name;
        protected float _rating;
        protected float _balance;

        public void AddCustomer(string name, float expenses, float income, float payment, float balance)
        {
            Contract.Requires(name != null);
            _name = name;
            _rating = DebtRatio(expenses, income, payment);
        }

        public string Name
        {
            get
            {
                return _name;
            }

            private set
            {
                _name = value;
            }
        }
        public float DebtRatio(float expenses, float income, float payment)
        {
            float net = income - payment;
            Contract.Assert(net != 0);
            return expenses / net;
        }

        public float CheckBalance(float balance)
        {
            Contract.Ensures(balance >= 0.0f);
            if (balance < 0.0f) balance = 0.0f;
            return balance;
        }
    }
}
