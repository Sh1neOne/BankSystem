using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystem.DataBase;

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
        public static int interestRate = 12;
        Random rnd = new Random();

        /// <summary>
        /// Конструктор инициализирует начальные данные
        /// </summary>
        public Bank()
        {
            SqlConnectionStringBuilder connectStrBuilder = new SqlConnectionStringBuilder
            {
                DataSource = @"(localdb)\MSSQLLocalDB",
                InitialCatalog = "BankSystemDB",
                IntegratedSecurity = true
            };

            VipDepartament = new Departament<VIPClient>("Отдел работы с VIP клиентами", 3);
            StandartDepartament = new Departament<StandartClient>("Отдел работы с обычными клиентами", 1);
            CompanyDepartament = new Departament<CompanyClient>("Отдел работы с юридическими лицами", 2);
          
            var DepSql = new BankSQL(connectStrBuilder);
            DepSql.ReadDataInDB(this);
            Account.BalanceChagedInDb += DepSql.UpdateAccountInDB;
            Account.CommitTransaction += LogsTransactions.AddTransaction;
            Client.accountSaveInDb += DepSql.AddAccountInDB;
            Client.accountDeleteInDb += DepSql.DeleteAccountInDb;
            Client.clientUpdateInDb += DepSql.UpdateClientInDB;
            Departament<CompanyClient>.addClientInDb += DepSql.AddClientInDB;
            Departament<VIPClient>.addClientInDb += DepSql.AddClientInDB;
            Departament<StandartClient>.addClientInDb += DepSql.AddClientInDB;
            Departament<CompanyClient>.deleteClientInDb += DepSql.DeleteClientInDB;
            Departament<VIPClient>.deleteClientInDb += DepSql.DeleteClientInDB;
            Departament<StandartClient>.deleteClientInDb += DepSql.DeleteClientInDB;
        }          
      
        /// <summary>
        /// Генерация случайных данных
        /// </summary>
        public void GenerateData()
        {
            DepartamentList = new ArrayList
            {
                VipDepartament,
                StandartDepartament,
                CompanyDepartament
            };
            for (int i = 1; i <= 5; i++)
            {
                var vipCl = new VIPClient($"ВИП Имя_{i}", $"Фамилия_{i}", i + 1);
                VipDepartament.AddClient(vipCl);
                AddRandomAccounts(vipCl);

                var stmdCl = new StandartClient($"Стандарт Имя_{i}", $"Фамилия_{i}", i + 2);
                StandartDepartament.AddClient(stmdCl);
                AddRandomAccounts(stmdCl);

                var cmpCl = new CompanyClient($"Компания Имя_{i}", $"Фамилия_{i}", i + 3);
                CompanyDepartament.AddClient(cmpCl);
                AddRandomAccounts(cmpCl);

            }
        }

        /// <summary>
        /// Метод создает случайные счета клиентов
        /// </summary>
        /// <param name="client"></param>
        public void AddRandomAccounts(Client client)
        {
            var CountAcc = rnd.Next(3, 7);
            for (int i = 1; i <= CountAcc; i++)
            {
                var rndAccType = rnd.Next(2);
                switch (rndAccType)
                {
                    case 0:
                        client.AddAccount(new Account(i.ToString(), rnd.Next(5) * 1000));                        
                        break;
                    case 1:
                        client.AddAccount(new DepositAccount(i.ToString(), rnd.Next(1, 5) * 1000, Convert.ToBoolean(rnd.Next(1)), rnd.Next(1, 13), rnd.Next(1,13)));
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Коллекция всех клиентов банков
        /// </summary>
        public List<Client> AllClients { get => allClients; set => allClients = value; }
        /// <summary>
        /// Коллекция департаментов 
        /// </summary>
        public ArrayList DepartamentList { get => departamentList; set => departamentList = value; }
        /// <summary>
        /// Департамент по работе с вип клиентами
        /// </summary>
        internal Departament<VIPClient> VipDepartament { get => vipDepartament; set => vipDepartament = value; }
        /// <summary>
        /// Департамент по работе со стандартными клиентами
        /// </summary>
        internal Departament<StandartClient> StandartDepartament { get => standartDepartament; set => standartDepartament = value; }
        /// <summary>
        /// Департамент по работе с компаниями
        /// </summary>
        internal Departament<CompanyClient> CompanyDepartament { get => companyDepartament; set => companyDepartament = value; }
    }
}
