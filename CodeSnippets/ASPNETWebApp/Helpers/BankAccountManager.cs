using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;

namespace ASPNETWebApp.Helpers
{
    public class BankAccountManager
    {
        public string _name;
        public float _rating;
        public float _balance;

        public void AddCustomer(string name, float expenses, float income, float payment, float balance)
        {
            Contract.Requires(name != null, "Name is required when AddCustomer is called.");
            _name = name;
            _rating = DebtRatio(expenses, income, payment);
            _balance = CheckBalance(balance);
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

        public double AccountBalance(double currentBalance, double transactionAmount)
        {
            double finalBalance = 0.00;
            finalBalance = currentBalance + transactionAmount;
            return finalBalance;
        }
    }
}