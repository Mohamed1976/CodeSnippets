using DataLibrary.BankDemo.Data;
using DataLibrary.BankDemo.Models;
using DataLibrary.Repository.Base;
using DataLibrary.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLibrary.Repository
{
    public class EmailRepo : RepoBase<Email>, IEmailRepo
    {
        public EmailRepo(BankContext context) : base(context)
        {
        }

        internal EmailRepo(DbContextOptions<BankContext> options) : base(options)
        {
        }
    }
}