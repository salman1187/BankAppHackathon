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


            //IAccount acc1 = mgr.CreateAccount("joji", "1234", 1000000, PrivilegeType.PREMIUM, "Savings");
            //IAccount acc2 = mgr.CreateAccount("abc", "2425", 2300000, PrivilegeType.GOLD, "Current");


            Account acc1 = repo.GetAccountbyAccNo(1);
            Account acc2 = repo.GetAccountbyAccNo(2);
            //mgr.Withdraw(acc1, 99, "1234");
            //mgr.Deposit(acc2, 101, "2425");
            //mgr.TransferFunds(acc2, acc1, 100, "2425");



            ExternalAccount externalAccount = new ExternalAccount { AccNo = 1, BankCode = "HCL101", BankName = "HCLBank" };
            mgr.ExternalTransfer(acc1, externalAccount, "1234", 500);

        }
    }
}
