using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem
{
    class Departament<T>
       where T : Client, new()

    {
        private ObservableCollection<T> clients;
        public static Action<Client, int> addClientInDb;
        public static Action<Client> deleteClientInDb;

        private string name;
        private int id;

        public Departament(string name, int id)
        {
            Clients = new ObservableCollection<T>();
            Name = name;
            Id = id;
        }

        public void AddClient(T client)
        {
            Clients.Add(client);
            addClientInDb?.Invoke(client, this.Id);
        }
        /// <summary>
        /// Метод возвращает коллекцию клиентов определенного типа из общего списка
        /// </summary>
        /// <param name="bank"></param>
        public void CreateClientCol(Bank bank)
        {
            var a = bank.AllClients.FindAll(x => x.GetType() == typeof(T));
            foreach (T item in a)
            {
                Clients.Add(item);
            }
        }
        /// <summary>
        /// Метод вызывает диалог добавления нововго клиента
        /// </summary>
        public void AddClientDialog()
        {
            T newClient = new T();
       
            DialogClient dc = new DialogClient(newClient as T);
            if (dc.ShowDialog() == true)
            {
                this.AddClient(dc.ContextClient as T);
            }
        }

        /// <summary>
        /// Метод удаляет клиента
        /// </summary>
        /// <param name="client"></param>
        public void DeleteClient(T client)
        {
            Clients.Remove(client);
            deleteClientInDb?.Invoke(client);
        }

 

        public ObservableCollection<T> Clients { get => clients; set => clients = value; }
        public string Name { get => name; set => name = value; }
        public int Id { get => id; set => id = value; }
    }
}
