using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject
{
    [TestClass]
    public class ContosoUnitTest
    {
        protected string _name;
        protected float _expenses;
        protected float _income;
        protected float _payment;
        protected float _balance;

        public ContosoUnitTest()
        {
            _name = "Contoso";
            _expenses = 0;
            _income = 100;
            _payment = 100;
            _balance = -1;
        }

        public ContosoUnitTest(string name, float expenses, float income, float payment, float balance)
        {
            _name = name;
            _expenses = expenses;
            _income = income;
            _payment = payment;
            _balance = balance;
        }

        [TestMethod]
        public void CheckName()
        {   
            Assert.IsNotNull(_name, "CheckName failed unit test");
        }

        [TestMethod]
        public void DebRatio()
        {
            //Assert.AreSame(_income, _payment, "DebRatio failed unit test");
            Assert.AreEqual(_income, _payment, "DebRatio failed unit test");
        }

        [TestMethod]
        public void CheckBalance()
        {
            Assert.IsTrue(_balance >= 0.0f, "Check balance failed unit test.");
        }
    }
}
