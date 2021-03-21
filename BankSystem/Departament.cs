using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem
{
    class Departament<T>: IClient<T>
        where T : Client, new()
    {
        private ObservableCollection<IClient<T>> clients;
        private string name;

        public Departament(string name)
        {
            Clients = new ObservableCollection<IClient<T>>();
            Name = name;
        }

        public void AddClient()
        {
            IClient<T> tclient = new T();
            Clients.Add(tclient);
        }


        public ObservableCollection<IClient<T>> Clients { get => clients; set => clients = value; }
        public string Name { get => name; set => name = value; }

    }
}
