using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem
{
    public class Department

    {
        private ObservableCollection<Client> clients;
        public static Action<Client, int> addClientInDb;
        public static Action<Client> deleteClientInDb;

        private string name;
        private int id;


        public Department(string name)
        {
            Clients = new ObservableCollection<Client>();
            Name = name;
        }

        public Department()
        {
            Clients = new ObservableCollection<Client>();
        }

        public void AddClient(Client client)
        {
            Clients.Add(client);
            addClientInDb?.Invoke(client, this.Id);
        }

        /// <summary>
        /// Метод вызывает диалог добавления нововго клиента
        /// </summary>
        public void AddClientDialog()
        {
            Client newClient = new Client();
       
            DialogClient dc = new DialogClient(newClient);
            if (dc.ShowDialog() == true)
            {
                this.AddClient(dc.ContextClient as Client);                
            }
        }

        /// <summary>
        /// Метод удаляет клиента
        /// </summary>
        /// <param name="client"></param>
        public void DeleteClient(Client client)
        {
            Clients.Remove(client);
            deleteClientInDb?.Invoke(client);
        }

 

        public ObservableCollection<Client> Clients { get => clients; set => clients = value; }
        public string Name { get => name; set => name = value; }
        public int Id { get => id; set => id = value; }
    }
}
