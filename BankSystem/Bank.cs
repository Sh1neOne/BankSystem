using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem
{
    class Bank
    {

        //private ObservableCollection<Departament<Client>> listDepartaments;
        private Departament<VIPClient> vipDepartament;
        private Departament<StandartClient> standartDepartament;
        private Departament<CompanyClient> companyDepartament;
        private List<Client> allClients;
        private ArrayList departamentList;
       
        public Bank()
        {
            AllClients = new List<Client>();
            for (int i = 0; i < 5; i++)
            {
                AllClients.Add(new VIPClient($"ВИП Имя_{i}", $"Фамилия_{i}"));
                AllClients.Add(new StandartClient($"Стандарт Имя_{i}", $"Фамилия_{i}"));
                AllClients.Add(new CompanyClient($"Имя_{i}", $"Фамилия_{i}"));
            }

            VipDepartament = new Departament<VIPClient>("Отдел работы с VIP клиентами");
            VipDepartament.CreateClientCol(this);
            StandartDepartament = new Departament<StandartClient>("Отдел работы с обычными клиентами");
            StandartDepartament.CreateClientCol(this);
            CompanyDepartament = new Departament<CompanyClient>("Отдел работы с юридическими лицами");
            CompanyDepartament.CreateClientCol(this);
            DepartamentList = new ArrayList();
            DepartamentList.Add(VipDepartament);
            DepartamentList.Add(StandartDepartament);
            DepartamentList.Add(CompanyDepartament);


            //listDepartaments = new ObservableCollection<Departament>
            //{
            //    vipDepartamnent,
            //    standartDepartamnent,
            //    companyDepartamnent
            //};

        }

        
   
        //internal List<Departament<Client>> ListDepartaments { get => listDepartaments; set => listDepartaments = value; }
        public List<Client> AllClients { get => allClients; set => allClients = value; }
        public ArrayList DepartamentList { get => departamentList; set => departamentList = value; }
        internal Departament<VIPClient> VipDepartament { get => vipDepartament; set => vipDepartament = value; }
        internal Departament<StandartClient> StandartDepartament { get => standartDepartament; set => standartDepartament = value; }
        internal Departament<CompanyClient> CompanyDepartament { get => companyDepartament; set => companyDepartament = value; }
    }
}
