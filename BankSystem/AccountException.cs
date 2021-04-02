using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem
{
    class AccountException : Exception
    {
        public string Value { get; set; }
        public AccountException(string message, string val) : base(message)
        {
            Value = val;
        }
    }
}
