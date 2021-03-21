using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem
{
    class Account
    {
        private string name;
        private int balance;

        public Account(string name, int balance = 0)
        {
            Name = name;
            Balance = balance;
        }
        public void BalanceTransfer(Account accountFrom, Account accountTo, int sum)
        {
            accountFrom.Balance -= sum;
            accountTo.Balance += sum;
        }

        public string Name { get => name; set => name = value; }
        public int Balance { get => balance; set => balance = value; }
    }
}
