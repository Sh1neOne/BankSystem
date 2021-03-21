using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem
{
    class VIPClient : Client, IClient<Client>
    {
        public VIPClient(string firstName, string lastName) : base(firstName, lastName)
        {
        }
    }
}
