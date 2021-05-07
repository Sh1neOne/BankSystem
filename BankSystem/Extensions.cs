using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem
{
    public static class Extensions
    {
        public static void DelAccount(this AccountModel a, ClientModel client)
        {
            client.DeleteAccount(a);
        }
    }
}
