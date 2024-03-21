using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Entities
{
    public enum PrivilegeType
    {
        REGULAR,
        GOLD,
        PREMIUM
    }
    public interface IAccount
    {
        string GetAccType();
        bool Open();
        bool Close();
    }
    [Table("Accounts")]
    public abstract class Account : IAccount
    {
        // Instance variables
        protected int accNo;
        protected string name;
        protected string pin;
        protected bool active;
        protected DateTime dateOfOpening;
        protected double balance;
        protected PrivilegeType privilegeType;

        // Getters and Setters for instance variables
        [Key]
        [Column(TypeName = "int")]
        public int AccNo { get => accNo; set => accNo = value; }
        public string Name { get => name; set => name = value; }
        public string Pin { get => pin; set => pin = value; }
        public bool Active { get => active; set => active = value; }
        [DataType(DataType.Date)]
        public DateTime DateOfOpening { get => dateOfOpening; set => dateOfOpening = value; }
        public double Balance { get => balance; set => balance = value; }
        public PrivilegeType PrivilegeType { get => privilegeType; set => privilegeType = value; }

        public abstract string GetAccType();
        public bool Open()
        {
            active = true;
            return active;
        }
        public bool Close()
        {
            active = false;
            return active;
        }
    }

    // Derived class Savings
    public class Savings : Account
    {
        public override string GetAccType()
        {
            return "Savings";
        }
    }
    public class Current : Account
    {
        public override string GetAccType()
        {
            return "Current";
        }
    }
    [Table("Transactions")]
    public class Transaction
    {
        [Key]
        [Column(Order = 0)]
        public int TransID { get; set; }
        [Key]
        [Column(Order = 1, TypeName = "int")]
        public int AccNo { get; set; }
        public string TransType { get; set; }
        public DateTime TransDate { get; set; }
        public double Amount { get; set; }  
    }
    public class AccountFactory
    {
        public Account AccountCreator(string accountType)
        {
            string className = ConfigurationManager.AppSettings[accountType];
            Type theType = Type.GetType(className);
            return (Account)Activator.CreateInstance(theType);
        }
    }
}
