using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem
{
    class Bank
    {

        private List<Departament<Client>> listDepartaments;
       
        public Bank()
        {
            var vipDepartamnent = new Departament<IClient<VIPClient>>("Отдел работы с VIP клиентами");
            var standartDepartamnent = new Departament<StandartClient>("Отдел работы с обычными клиентами");
            var companyDepartamnent = new Departament<CompanyClient>("Отдел работы с юридическими лицами");
            ListDepartaments = new List<Departament<Client>>
            {
                vipDepartamnent,
                standartDepartamnent,
                companyDepartamnent
            };

        }

   
        internal List<Departament<Client>> ListDepartaments { get => listDepartaments; set => listDepartaments = value; }

    }
}
