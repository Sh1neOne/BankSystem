using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem
{
    class VIPClient : Client
    {
        public VIPClient() : base()
        {
        }

        public VIPClient(string firstName, string lastName, int id) : base(firstName, lastName, id)
        {
        }
    }
}
