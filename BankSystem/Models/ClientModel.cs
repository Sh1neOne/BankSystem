using BankSystem.Models;
using BankSystem.ViewModels;
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
    public class ClientModel : BaseViewModel
    {
        private string firstName;
        private string lastName;
        private int id;    
        private ObservableCollection<AccountModel> accounts;
        private bool goodCreditHistory;
        public static event Action<AccountModel, int> AccountSaveInDb;
        public static event Action<AccountModel> AccountDeleteInDb;
        public static event Action<ClientModel> ClientUpdateInDb;
        public static Random rnd = new Random();

        public ClientModel() 
        {
            Accounts = new ObservableCollection<AccountModel>();
        }

        public ClientModel(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            Accounts = new ObservableCollection<AccountModel>();

        }

        public void EditClient()
        {
            ClientUpdateInDb?.Invoke(this);
        }


        /// <summary>
        /// Метод добавляющий счет клиенту
        /// </summary>
        /// <param name="account"></param>
        public void AddAccount(AccountModel account)
        {
            Accounts.Add(account);
            account.BalanceChanged += Account_BalanceChanged;
            AccountSaveInDb?.Invoke(account, this.Id);
            OnPropertyChanged("TotalBalance");
        }

        /// <summary>
        /// Метод для обработки оповещения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Account_BalanceChanged(object sender, AccountModel.AccountEventArgs e)
        {
            var acc = sender as AccountModel;
            MessageBox.Show($"Клиент:{this.LastName}! Изменился баланс на счете {acc.Name}. Текущий баланс {e.msg}");  
        }

       
        /// <summary>
        /// Метод удаляет счет у клиента
        /// </summary>
        /// <param name="acc"></param>
        public void DeleteAccount(AccountModel acc)
        {
            Accounts.Remove(acc);
            OnPropertyChanged("TotalBalance");
            AccountDeleteInDb?.Invoke(acc);
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
                        this.AddAccount(new AccountModel(i.ToString(), rnd.Next(5) * 1000));
                        break;
                    case 1:
                        this.AddAccount(new DepositAccountModel(i.ToString(), rnd.Next(1, 5) * 1000, Convert.ToBoolean(rnd.Next(1)), rnd.Next(1, 13), rnd.Next(1, 13)));
                        break;
                    default:
                        break;
                }
            }
        }

        public string FirstName 
        { 
            get => firstName;
            set 
            { 
                firstName = value;
                OnPropertyChanged();
            }
        }
        public string LastName
        {
            get => lastName;
            set
            {
                lastName = value;
                OnPropertyChanged();
            }
        }
        public int Id { get => id; set => id = value; }
        public bool GoodCreditHistory { get => goodCreditHistory; set => goodCreditHistory = value; }
        public ObservableCollection<AccountModel> Accounts { get => accounts; set => accounts = value; }
        public double TotalBalance { get => Accounts.Sum(x => x.Balance); }
        public int MaxInterestRate { get => GoodCreditHistory? 13 :12; }
        public DepartmentModel Department { get; set; }
        public int DepartmentId { get; set; }
    }
}
