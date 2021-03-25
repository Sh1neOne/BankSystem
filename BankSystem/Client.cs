using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem
{
    public class Client: IClient<Client> 
    {
        private string firstName;
        private string lastName;
        private string id;
        private ObservableCollection<Account> accounts;
        private bool goodCreditHistory;

        public Client()
        {

        }

        public Client(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            Accounts = new ObservableCollection<Account>();
            Id = "";
        }


        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string Id { get => id; set => id = String.IsNullOrEmpty(value)?Guid.NewGuid().ToString().Substring(0,5):value; }
        public bool GoodCreditHistory { get => goodCreditHistory; set => goodCreditHistory = value; }
        internal ObservableCollection<Account> Accounts { get => accounts; set => accounts = value; }
    }
}
