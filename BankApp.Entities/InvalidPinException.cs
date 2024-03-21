using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Entities
{
    public class InvalidPinException : ApplicationException
    {
        public InvalidPinException(string msg) : base(msg) { }
    }
    public class InsufficientbalanceException : ApplicationException
    {
        public InsufficientbalanceException(string msg) : base(msg) { }
    }
    public class InactiveAccountException : ApplicationException
    {
        public InactiveAccountException(string msg) : base(msg) { }
    }
    public class TransactionAmountExceededException : ApplicationException
    {
        public TransactionAmountExceededException(string msg) : base(msg) { }
    }
    public class TransactionNotFoundException : ApplicationException
    {
        public TransactionNotFoundException(string msg) : base(msg) { }
    }
    public class NoMinimumBalanceException : ApplicationException
    {
        public NoMinimumBalanceException(string msg) : base(msg) { }
    }
}
