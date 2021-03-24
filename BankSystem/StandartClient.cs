using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem
{
    class StandartClient : Client,IClient<Client>
    {
        public StandartClient(string firstName, string lastName) : base(firstName, lastName)
        {
        }
    }
}
