using BankApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Data
{
    public class TransactionLog
    {
        BankAppDbContext db = new BankAppDbContext();
        public List<Transaction> GetAllTransactions()
        {
            var Transactions = (from c in db.Transactions
                                select c).ToList();
            if (Transactions.Count == 0)
                throw new TransactionNotFoundException("No Transactions");

            return Transactions;
        }

        public List<Transaction> GetTransactionsbyAccNo(int AccountNo)
        {
            var Transactions = (from c in db.Transactions
                                where c.AccNo == AccountNo  
                                select c).ToList();
            if (Transactions.Count == 0)
                throw new TransactionNotFoundException("No Transactions");

            return Transactions;
        }

        public List<Transaction> GetTransactionsbyAccNoAndTransType(int AccountNo, string transtype)
        {
            var Transactions = (from c in db.Transactions
                                where c.AccNo == AccountNo && c.TransType == transtype
                                select c).ToList();
            if (Transactions.Count == 0)
                throw new TransactionNotFoundException("No Transactions");

            return Transactions;
        }
        

        public void LogTransaction(Transaction t)
        {
            db.Transactions.Add(t);
            db.SaveChanges();
        }
    }
}
