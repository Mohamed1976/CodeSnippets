using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASPNETWebApp.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;


//https://stackoverflow.com/questions/2639960/how-come-you-cannot-catch-code-contract-exceptions/6952770
//https://stackoverflow.com/questions/933613/how-do-i-use-assert-to-verify-that-an-exception-has-been-thrown
//https://stackoverflow.com/questions/21996781/is-there-any-way-how-to-test-contracts-set-using-code-contracts
//https://docs.microsoft.com/en-us/previous-versions/ms379625(v=vs.80)?redirectedfrom=MSDN#vstsunittesting_topic5
namespace UnitTestProject
{
    [TestClass]
    public class BankAccountManagerUnitTest
    {
        const string ContractExceptionName =
            "System.Diagnostics.Contracts.__ContractsRuntime.ContractException";

        private BankAccountManager _bankAccountManager = null;

        public BankAccountManagerUnitTest()
        {
            _bankAccountManager = new BankAccountManager();
        }

        //Logging testunits to stdout
        //https://stackoverflow.com/questions/11209639/can-i-write-into-console-in-a-unit-test-if-yes-why-the-console-window-is-not-o/11209805
        [TestMethod]
        public void ValidateName()
        {
            Console.WriteLine("Entering ValidateName().");
            _bankAccountManager.AddCustomer("Contoso", 100, 1000, 100, 1);
            Console.WriteLine($"_bankAccountManager._name: {_bankAccountManager._name}");
            Assert.AreEqual("Contoso", _bankAccountManager._name);
            Console.WriteLine("Exiting ValidateName().");

            //bool IsContractException = true;
            //try
            //{
            //    _bankAccountManager.AddCustomer(null,100,1000,100,1);
            //    Assert.Fail("Expected contract failure");
            //}
            //catch (Exception e)
            //{
            //    Debug.WriteLine("Exception e in NullName: " + e.GetType().FullName); 
            //    if (e.GetType().FullName != ContractExceptionName)
            //    {
            //        IsContractException = false;
            //        //throw;
            //    }
            //    Assert.IsTrue(IsContractException, e.GetType().FullName);
            //}            
        }

        [TestMethod]
        public void ValidateRating()
        {
            _bankAccountManager.AddCustomer("Contoso", 1000, 500, 500, 0);
            Assert.AreEqual("Contoso", _bankAccountManager._name);
            Assert.AreEqual(_bankAccountManager._rating, 0.0f);

        }

        [TestMethod]
        public void ValidateBalance()
        {
            _bankAccountManager.AddCustomer("Contoso", 100, 100, 1000, -1);
            Assert.AreEqual("Contoso", _bankAccountManager._name);
            Assert.AreEqual(_bankAccountManager._balance, 0.0f);
        }

        [TestMethod]
        public void AccountBalanceTest()
        {
            double currentBalance = 175.05;
            double transactionAmount = 76.03;
            double finalBalance = 251.08;
            double result = 0.00;
            result = _bankAccountManager.AccountBalance(currentBalance, transactionAmount);
            Assert.AreEqual(result, finalBalance);
        }
    }
}