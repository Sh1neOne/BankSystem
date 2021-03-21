using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem
{
    interface IClient<out T>
        where T: Client
    {     
 
    }
}


