using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Entities
{
    public class Policy
    {
        public double MinimumBalance { get; set; }
        public double InterestRate { get; set; }
    }
    public class PolicyFactory
    {
        public Policy CreatePolicy(string accType, string privilege)
        {
            Policy p = new Policy();
            string savingsRegularValue = ConfigurationManager.AppSettings[$"{accType}-{privilege}"];
            if (savingsRegularValue != null)
            {
                // Split the value using the pipe separator
                string[] values = savingsRegularValue.Split('|');
                string amount = "0";
                string roi = "0";
                if (values.Length == 2)
                {
                    amount = values[0];
                    roi = values[1];
                }
                p.InterestRate = double.Parse(roi);
                p.MinimumBalance = double.Parse(amount);
                return p;
            }
            return null;
        }
    }
}
