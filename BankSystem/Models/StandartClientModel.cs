using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem
{
    class StandartClientModel : ClientModel
    {
        public StandartClientModel()
        {
        }

        public StandartClientModel(string firstName, string lastName) : base(firstName, lastName)
        {
        }
    }
}
