using BankApp.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BankApp.Data
{
    public class ExternalTransferService
    {
        public void Start(ExternalAccount toAcc, double amt)
        {
            using (var db = new BankAppDbContext())
            {
                var allOpenTrans = db.Transactions.Where(t => t.Status == TransactionStatus.OPEN).ToList();
                if (allOpenTrans.Any())
                {
                    ExternalTransferFactory exfactory = new ExternalTransferFactory();
                    IExternalBankContract toBank = exfactory.ExternalTransferCreator(toAcc.BankCode);
                    //HCLBankTransfer h = new HCLBankTransfer();
                    foreach (var t in allOpenTrans)
                    {
                        if(toBank.DepositMoney(toAcc, amt) == true)
                            t.Status = TransactionStatus.CLOSED;

                        Console.WriteLine("Executed");
                        
                    }
                    db.SaveChanges();
                }
            }
        }
    }
}
