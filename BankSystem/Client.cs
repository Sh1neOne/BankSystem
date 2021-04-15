using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BankSystem
{
    public class Client : INotifyPropertyChanged
    {
        private string firstName;
        private string lastName;
        private int id;
        private ObservableCollection<Account> accounts;
        private bool goodCreditHistory;
        public event PropertyChangedEventHandler PropertyChanged;
        public static event Action<Account, int> accountSaveInDb;
        public static event Action<Account> accountDeleteInDb;
        public static event Action<Client> clientUpdateInDb;

        public Client() : this("", "", 0)
        {
            Id = -1;
        }

        public Client(string firstName, string lastName, int id)
        {
            FirstName = firstName;
            LastName = lastName;
            Accounts = new ObservableCollection<Account>();
            Id = id;
        }
        /// <summary>
        /// Метод вызывает диалог редактирования клиента
        /// </summary>
        public void EditClientDialog()
        {
            var dc = new DialogClient(this);
            dc.ShowDialog();
            clientUpdateInDb?.Invoke(this);
        }
        /// <summary>
        /// Метод вызывает диалог добавления клиента
        /// </summary>
        public void AddAccountDialog()
        {
            AccountDialog ad = new AccountDialog(this);
            if (ad.ShowDialog() == true)
            {
                AddAccount(ad.Account);
                OnPropertyChanged("TotalBalance");
            }
        }

        /// <summary>
        /// Метод добавляющий счет клиенту
        /// </summary>
        /// <param name="account"></param>
        public void AddAccount(Account account)
        {
            Accounts.Add(account);
            account.BalanceChanged += Account_BalanceChanged;
            accountSaveInDb?.Invoke(account, this.Id);
        }

        /// <summary>
        /// Метод для обработки оповещения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Account_BalanceChanged(object sender, Account.AccountEventArgs e)
        {
            var acc = sender as Account;
            MessageBox.Show($"Клиент:{this.LastName}! Изменился баланс на счете {acc.Name}. Текущий баланс {e.msg}");
    
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        /// <summary>
        /// Метод удаляет счет у клиента
        /// </summary>
        /// <param name="acc"></param>
        public void DeleteAccount(Account acc)
        {
            Accounts.Remove(acc);
            OnPropertyChanged("TotalBalance");
            accountDeleteInDb?.Invoke(acc);
        }

        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public int Id { get => id; set => id = value; }
        public bool GoodCreditHistory { get => goodCreditHistory; set => goodCreditHistory = value; }
        public ObservableCollection<Account> Accounts { get => accounts; set => accounts = value; }
        public double TotalBalance { get => Accounts.Sum(x => x.Balance); }
        public int MaxInterestRate { get => GoodCreditHistory? Bank.interestRate + 1 : Bank.interestRate; }
    }
}
