using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLibrary
{
    public class CalculateDeposit
    {
        public static double CalculateWithCapitalization(double balanceDeposit, int mountCount, int interestRate)
        {
            for (int i = 0; i < mountCount; i++)
            {
                balanceDeposit += balanceDeposit * interestRate / 100 / 12;
            }

            return balanceDeposit;
        }

        public static double CalculateWithoutCapitalization(double balanceDeposit, int mountCount, int interestRate)
        {
            balanceDeposit += (balanceDeposit * interestRate / 100) * mountCount / 12;
            return balanceDeposit;
        }
    }
}
