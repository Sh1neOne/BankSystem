using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem
{
    class VIPClientModel : ClientModel
    {
        public VIPClientModel() : base()
        {
        }

        public VIPClientModel(string firstName, string lastName) : base(firstName, lastName)
        {
        }
    }
}
