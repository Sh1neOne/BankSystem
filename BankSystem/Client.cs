using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem
{
    class Client
    {
        private string firstName;
        private string lastName;
        private string id;

        public Client(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            Id = "";
        }

        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string Id { get => id; set => id = String.IsNullOrEmpty(value)?Guid.NewGuid().ToString().Substring(0,5):value; }
    }
}
