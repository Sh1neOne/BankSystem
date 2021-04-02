using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem
{
    public static class Extensions
    {
        public static void DelAccount(this Account a, Client client)
        {
            client.DeleteAccount(a);
        }
    }
}
