using BankApp.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Data
{
    public class AccountManager
    {
        BankAppDbContext db = new BankAppDbContext();
        AccountDbRepository AccountDb = new AccountDbRepository();
        TransactionLog TransactionDb = new TransactionLog();
        Dictionary<PrivilegeType, double> privilegeAmounts = new Dictionary<PrivilegeType, double>
        {
            { PrivilegeType.REGULAR, 100000.0 },
            { PrivilegeType.GOLD, 200000.0 },
            { PrivilegeType.PREMIUM, 300000.0 }
        };
        public IAccount CreateAccount(string name, string pin, double balance, PrivilegeType privilege, string accountType)
        {
            AccountFactory factory = new AccountFactory();
            Account account = factory.AccountCreator(accountType);
            

            //account.AccNo = IDGenerator.GenerateAccNo();
            account.Name = name;
            account.Pin = pin;
            account.Balance = balance;
            account.PrivilegeType = privilege;
            account.DateOfOpening = DateTime.Today;

            PolicyFactory policyfactory = new PolicyFactory();
            Policy p = policyfactory.CreatePolicy(account.GetAccType(), account.PrivilegeType.ToString());

            if (account.Balance < p.MinimumBalance)
                throw new NoMinimumBalanceException("No minimum balance");

            account.Open();
            AccountDb.Save(account);

            return account;
        }
        public bool Withdraw(Account fromAccount, double amount, string pin)
        {
            if (fromAccount.Pin != pin)
                throw new InvalidPinException("Invalid PIN");
            if (amount > fromAccount.Balance)
                throw new InsufficientbalanceException("Not enough balance in the account");
            if (fromAccount.Active == false)
                throw new InactiveAccountException("Account not active");

            PolicyFactory factory = new PolicyFactory();
            Policy p = factory.CreatePolicy(fromAccount.GetAccType(), fromAccount.PrivilegeType.ToString());

            if (fromAccount.Balance - amount < p.MinimumBalance)
                throw new NoMinimumBalanceException("No minimum balance");

            double totalAmountToday = db.Transactions
                                     .Where(t => t.AccNo == fromAccount.AccNo && t.TransDate == DateTime.Today)
                                     .Sum(t => (double?)t.Amount) ?? 0.0;

            if (totalAmountToday > privilegeAmounts[fromAccount.PrivilegeType])
                throw new TransactionAmountExceededException("Todays amount exceeded");

            fromAccount.Balance = fromAccount.Balance - amount;

            AccountDb.UpdateBalance(fromAccount.AccNo, fromAccount.Balance);
            IDGenerator generate = new IDGenerator();
            int transid = generate.GenerateTransId();
            TransactionDb.LogTransaction(new Transaction { AccNo = fromAccount.AccNo, Amount = amount, TransDate = DateTime.Today, TransType = "Withdraw", TransID = transid });

            StreamWriter writer = new StreamWriter("IdGeneratorFile.txt", false);
            writer.WriteLine(transid);
            writer.Close();
            return true;
        }
        public bool Deposit(Account fromAccount, double amount, string pin)
        {
            if (fromAccount.Pin != pin)
                throw new InvalidPinException("Invalid PIN");
            if (fromAccount.Active == false)
                throw new InactiveAccountException("Account not active");

            fromAccount.Balance = fromAccount.Balance + amount;

            AccountDb.UpdateBalance(fromAccount.AccNo, fromAccount.Balance);
            IDGenerator generate = new IDGenerator();
            int transid = generate.GenerateTransId();
            TransactionDb.LogTransaction(new Transaction { AccNo = fromAccount.AccNo, Amount = amount, TransDate = DateTime.Today, TransType = "Deposit", TransID = transid});
            StreamWriter writer = new StreamWriter("IdGeneratorFile.txt", false);
            writer.WriteLine(transid);
            writer.Close();
            return true;
        }
        public bool TransferFunds(Account fromAcc,  Account toAcc, double amt, string pin, string pan = null, string aadhaarno = null)
        {
            if (fromAcc.Active == false || toAcc.Active == false)
                throw new InactiveAccountException("Account is inactive");
            if (fromAcc.Pin != pin)
                throw new InvalidPinException("Invalid PIN");
            if (amt > fromAcc.Balance)
                throw new InsufficientbalanceException("Not enough balance in the account");
            PolicyFactory factory = new PolicyFactory();
            Policy p = factory.CreatePolicy(fromAcc.GetAccType(), fromAcc.PrivilegeType.ToString());

            if (fromAcc.Balance - amt < p.MinimumBalance)
                throw new NoMinimumBalanceException("No minimum balance");


            double totalAmountToday = db.Transactions
                                    .Where(t => t.AccNo == fromAcc.AccNo && t.TransDate == DateTime.Today)
                                    .Sum(t => (double?)t.Amount) ?? 0.0;

            if (totalAmountToday > privilegeAmounts[fromAcc.PrivilegeType])
                throw new TransactionAmountExceededException("Amount exceeded");

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    fromAcc.Balance = fromAcc.Balance - amt;
                    AccountDb.UpdateBalance(fromAcc.AccNo, fromAcc.Balance);
                    toAcc.Balance = toAcc.Balance + amt;
                    AccountDb.UpdateBalance(toAcc.AccNo, toAcc.Balance);
                    //update table
                    IDGenerator generate = new IDGenerator();
                    int transid = generate.GenerateTransId();
                    TransactionDb.LogTransaction(new Transaction { AccNo = fromAcc.AccNo, Amount = amt, TransDate = DateTime.Today, TransType = "Withdraw", TransID = transid });
                    TransactionDb.LogTransaction(new Transaction { AccNo = toAcc.AccNo, Amount = amt, TransDate = DateTime.Today, TransType = "Deposit" , TransID = transid });
                    StreamWriter writer = new StreamWriter("IdGeneratorFile.txt", false);
                    writer.WriteLine(transid);
                    writer.Close();
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
            return true;
        }
    }
    public class IDGenerator
    {
        
        public int transid { get; set; }

        public IDGenerator()
        {
            StreamReader reader = new StreamReader("IdGeneratorFile.txt");
            string line = reader.ReadLine();
            if (line != null)
                transid = int.Parse(line);
            else
                transid = 0;
            reader.Close();
        }
        public int GenerateTransId()
        {
            return ++transid;
        }
    }

}
