using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem
{
     class Departament<T>
        where T: Client
 
    {
        private ObservableCollection<T> clients;
        private string name;

        public Departament(string name)
        {
            Clients = new ObservableCollection<T>();
            Name = name;
        }

        public void AddClient(T client)
        {
            Clients.Add(client);
        }

        public void CreateClientCol(Bank bank)
        {
            var a = bank.AllClients.FindAll(x => x.GetType() == typeof(T));
            foreach (T item in a)
            {
                Clients.Add(item);
            }
        }

        public ObservableCollection<T> Clients { get => clients; set => clients = value; }
        public string Name { get => name; set => name = value; }

    }
}
