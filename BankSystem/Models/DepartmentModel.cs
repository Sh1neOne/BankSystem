using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem
{
    public class DepartmentModel

    {
        private ObservableCollection<ClientModel> clients;
        public static Action<ClientModel, int> addClientInDb;
        public static Action<ClientModel> deleteClientInDb; 

        private string name;
        private int id;


        public DepartmentModel(string name)
        {
            Clients = new ObservableCollection<ClientModel>();
            Name = name;
        }

        public DepartmentModel()
        {
            Clients = new ObservableCollection<ClientModel>();
        }

        /// <summary>
        /// Метод добавляет клиента
        /// </summary>
        /// <param name="client"></param>
        public void AddClient(ClientModel client)
        {
            Clients.Add(client);
            addClientInDb?.Invoke(client, this.Id);
        }

     

        /// <summary>
        /// Метод удаляет клиента
        /// </summary>
        /// <param name="client"></param>
        public void DeleteClient(ClientModel client)
        {
            Clients.Remove(client);
            deleteClientInDb?.Invoke(client);
        }

 

        public ObservableCollection<ClientModel> Clients { get => clients; set => clients = value; }
        public string Name { get => name; set => name = value; }
        public int Id { get => id; set => id = value; }
    }
}
