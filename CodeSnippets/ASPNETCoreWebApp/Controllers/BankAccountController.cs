using ASPNETCoreWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCoreWebApp.Controllers
{
    public class BankAccountController : Controller
    {
        private List<BankAccount> accounts = new List<BankAccount>()
        {
            new BankAccount() { AccountName="AccountName1", AccountNumber="23", Balance=2000 },
            new BankAccount() { AccountName="AccountName2", AccountNumber="27", Balance=3000 }
        };

        [HttpGet]
        public IActionResult GetAccounts()
        {
            return View(accounts);
        }

        [HttpGet]
        public IActionResult EditAccount(string maskedAccountNum)
        {
            Debug.WriteLine("EditAccount(string maskedAccountNum): " + maskedAccountNum);
            return View(nameof(GetAccounts), accounts);
        }
    }
}
