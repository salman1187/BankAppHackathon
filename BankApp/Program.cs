using BankApp.Data;
using BankApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            AccountManager mgr = new AccountManager();
            AccountDbRepository repo = new AccountDbRepository();
            BankAppDbContext db = new BankAppDbContext();

            //IAccount acc1 = mgr.CreateAccount("joji", "1234", 10000, PrivilegeType.PREMIUM, "Savings");


            //IAccount acc2 = mgr.CreateAccount("abc", "2425", 2300, PrivilegeType.GOLD, "Current");

            Account acc1 = repo.GetAccountbyAccNo(1);
            Account acc2 = repo.GetAccountbyAccNo(3);
            //mgr.Deposit(acc1, 100000, "1234");
            //mgr.Deposit(acc2, 100000, "2425");
            mgr.TransferFunds(acc2 , acc1, 100, "2425");
        }
    }
}
