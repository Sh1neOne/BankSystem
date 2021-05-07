using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem
{
    class CompanyClientModel : ClientModel
    {
        public CompanyClientModel() : base()
        {
            
        }

        public CompanyClientModel(string firstName, string lastName) : base(firstName, lastName)
        {
        }
    }
}
