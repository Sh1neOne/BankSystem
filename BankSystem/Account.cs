using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem
{
    public class Account : INotifyPropertyChanged
    {
        protected string name;
        private double balance;

        public event EventHandler<AccountEventArgs> BalanceChanged;
        public static event Action<Account,Account,double> CommitTransaction;

        //public event Action<string, Account> BalanceChanged;
   
        public Account(string name, double balance = 0)
        {
            Name = name;
            Balance = balance;
        }
        /// <summary>
        /// Метод осуществляет перевод денег му счетами
        /// </summary>
        /// <param name="accountFrom"></param>
        /// <param name="accountTo"></param>
        /// <param name="sum"></param>
        public void BalanceTransferTo(Account accountTo, double sum)
        {
            this.Balance -= sum;
            accountTo.Balance += sum;
            CommitTransaction?.Invoke(this, accountTo, sum);

        } 

        /// <summary>
        /// Метод возвращает информацию о счете
        /// </summary>
        /// <returns></returns>
        public virtual string InformationAccount()
        {
            return $"Счет: {Name}\nБаланс: {Balance}";
        }

        
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public class AccountEventArgs : EventArgs
        {
            public readonly string msg;
            public AccountEventArgs(string messsage)
            {
                msg = messsage;
            }
        }

        /// <summary>
        /// Перегрузка оператора +
        /// </summary>
        /// <param name="ac1"></param>
        /// <param name="ac2"></param>
        /// <returns></returns>
        public static Account operator +(Account ac1, Account ac2)
        {
            return new Account($"{ac1.Name} {ac2.Name}", ac1.Balance + ac2.Balance);
        }

        public virtual string Name { get =>$"Стандартный счет {name}"; set => name = value; }
        public double Balance { get => balance; 
            set
            {
                if (value < 0)
                {
                    throw new AccountException("Баланс не может быть отрицательным", value.ToString());
                }
                balance = value;
                BalanceChanged?.Invoke(this, new AccountEventArgs(value.ToString()));
                OnPropertyChanged();           
            }
        }
    }
}
