using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Models
{
    class NullClientModel : ClientModel
    {
        public NullClientModel()
        {
            FirstName = "Не определен";
            LastName = "Не определен";
        }
    }
}
