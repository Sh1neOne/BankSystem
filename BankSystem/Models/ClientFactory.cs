using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Models
{
    public class ClientFactory
    {
        public static ClientModel CreateClient(DepartmentModel depClient, string FirstName, string LastName)
        {

            if (depClient.Name == "Отдел работы с VIP клиентами") return new CompanyClientModel(FirstName, LastName);
            else if (depClient.Name == "Отдел работы с обычными клиентами") return new VIPClientModel(FirstName, LastName);
            else if (depClient.Name == "Отдел работы с юридическими лицами") return new StandartClientModel(FirstName, LastName);
            else return new NullClientModel();
        }
    }
}
