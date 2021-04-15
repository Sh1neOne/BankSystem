using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem
{
    class StandartClient : Client
    {
        public StandartClient() : base()
        {
        }

        public StandartClient(string firstName, string lastName, int id) : base(firstName, lastName, id)
        {
        }
    }
}
