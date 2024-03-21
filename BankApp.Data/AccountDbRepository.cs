using BankApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Data
{
    public class AccountDbRepository
    {
        BankAppDbContext db = new BankAppDbContext();
        public void DeleteAccountbyId(int transid)
        {
            Account c = db.Accounts.Find(transid);
            db.Accounts.Remove(c);
            db.SaveChanges();
        }

        public List<Account> GetAllAccounts()
        {
            var Accounts = (from c in db.Accounts
                            select c).ToList();
            return Accounts;
        }

        public Account GetAccountbyAccNo(int AccountNo)
        {
            Account c = db.Accounts.Find(AccountNo);
            return c;
        }

        public void Save(Account Account)
        {
            db.Accounts.Add(Account);
            db.SaveChanges();
        }
        public void UpdateBalance(int AccountNo, double balance)
        {
            Account c = db.Accounts.Find(AccountNo);
            c.Balance = balance;
            db.SaveChanges();
        }
    }
}
