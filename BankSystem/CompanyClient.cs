using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem
{
    class CompanyClient : Client
    {
        public CompanyClient() : base()
        {
            
        }

        public CompanyClient(string firstName, string lastName) : base(firstName, lastName)
        {
        }
    }
}
