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
        public static Random rnd = new Random();

        public Client() 
        {
            Accounts = new ObservableCollection<Account>();
        }

        public Client(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            Accounts = new ObservableCollection<Account>();

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

        /// <summary>
        /// Метод создает случайные счета клиентов
        /// </summary>
        /// <param name="client"></param>
        public void AddRandomAccounts()
        {
            var CountAcc = rnd.Next(3, 7);
            for (int i = 1; i <= CountAcc; i++)
            {
                var rndAccType = rnd.Next(2);
                switch (rndAccType)
                {
                    case 0:
                        this.AddAccount(new Account(i.ToString(), rnd.Next(5) * 1000));
                        break;
                    case 1:
                        this.AddAccount(new DepositAccount(i.ToString(), rnd.Next(1, 5) * 1000, Convert.ToBoolean(rnd.Next(1)), rnd.Next(1, 13), rnd.Next(1, 13)));
                        break;
                    default:
                        break;
                }
            }
        }

        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public int Id { get => id; set => id = value; }
        public bool GoodCreditHistory { get => goodCreditHistory; set => goodCreditHistory = value; }
        public ObservableCollection<Account> Accounts { get => accounts; set => accounts = value; }
        public double TotalBalance { get => Accounts.Sum(x => x.Balance); }
        public int MaxInterestRate { get => GoodCreditHistory? 13 :12; }
        public Department Department { get; set; }
        public int DepartmentId { get; set; }
    }
}
