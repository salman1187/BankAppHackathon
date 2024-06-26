﻿using BankApp.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Data
{
    public class BankAppDbContext : DbContext
    {
        //configure database
        public BankAppDbContext() : base("defaultConnection")
        {

        }
        //configure table
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<HCLBank> HCLBanks { get; set; }
        public DbSet<ICICIBank> ICICIBanks { get; set; }
        public DbSet<CITIBank> CITIBanks { get;set; }
    }
}
