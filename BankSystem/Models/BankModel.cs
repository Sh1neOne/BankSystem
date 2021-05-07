using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Models
{
    public class BankModel
    {
        private List<DepartmentModel> departments;
        private BankDbContext bankDbContext;
        
        public BankModel()
        {
            bankDbContext = new BankDbContext();
            Departments = BankDbContext.Departments.Include("Clients.Accounts").ToList();
            DepartmentModel.addClientInDb = (client, id) => bankDbContext.SaveChanges();
            DepartmentModel.deleteClientInDb = (client) =>
                {
                    bankDbContext.Clients.Remove(client);
                    bankDbContext.SaveChanges();
                };
            ClientModel.ClientUpdateInDb += (client) => bankDbContext.SaveChanges();
            ClientModel.AccountSaveInDb += (client, id) => bankDbContext.SaveChanges();
            ClientModel.AccountDeleteInDb += (account) => bankDbContext.SaveChanges();
            AccountModel.BalanceChagedInDb += (account) => bankDbContext.SaveChanges();
            AccountModel.CommitTransaction +=(accountFrom, accountTo, sum) => LogsTransactionsModel.AddTransaction(accountFrom, accountTo, sum);
        }

        public List<DepartmentModel> Departments { get => departments; set => departments = value; }
        public BankDbContext BankDbContext { get => bankDbContext; }
    }
}
