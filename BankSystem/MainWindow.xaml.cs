
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BankSystem
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>\
    public partial class MainWindow : Window
    {
        Bank bank;
        //LogsTransactions logs;
        public MainWindow()
        {
            InitializeComponent();
            bank = new Bank();
            //logs = new LogsTransactions();
           
            this.DataContext = bank.DepartamentList;
            AccountsListBox.DataContext = this.DataContext;
        }
        /// <summary>
        /// Обработчик нажатия кнопки добавить клиента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddClient_Click(object sender, RoutedEventArgs e)
        {
            var t = DepartamentsList.SelectedItem;
            MethodInfo mi = t.GetType().GetMethod("AddClientDialog");
            object[] args = { };
            mi.Invoke(t, args); 
        }

        /// <summary>
        /// Обработчик нажатия кнопки редактирования клиента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditClient_Click(object sender, RoutedEventArgs e)
        {
            var c = ClientListView.SelectedItem as Client;
            if (c != null) { c.EditClientDialog(); }
        }
        /// <summary>
        /// Обработчик нажатия кнопки удаления клиента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteClient_Click(object sender, RoutedEventArgs e)
        {
            var SelDep = DepartamentsList.SelectedItem;
            var SelClient = ClientListView.SelectedItem as Client;
            MethodInfo mi = SelDep.GetType().GetMethod("DeleteClient");
            object[] args = { SelClient };
            mi.Invoke(SelDep, args);     
        }

        /// <summary>
        /// Обработчик нажатия кнопки информации о клиенте
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InformationClient_Click(object sender, RoutedEventArgs e)
        {
            var selAccount = AccountsListBox.SelectedItem as Account;
            MessageBox.Show(selAccount?.InformationAccount()??"Выберите счет");
         
        }

        /// <summary>
        /// Обработчик нажатия кнопки добавления счета
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddAccount_Click(object sender, RoutedEventArgs e)
        {
            var SelClient = ClientListView.SelectedItem as Client;
            SelClient.AddAccountDialog();
        }

        /// <summary>
        /// Обработчик нажатия кнопки удаления счета
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteAccount_Click(object sender, RoutedEventArgs e)
        {
            var selClient = ClientListView.SelectedItem as Client;
            var selAccount = AccountsListBox.SelectedItem as Account;
            selAccount.DelAccount(selClient);

        }

        /// <summary>
        /// Обработчик нажатия кнопки выбора счета с которого нужно перевести деньги
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void accountFromButton_Click(object sender, RoutedEventArgs e)
        {
            var selAccount = AccountsListBox.SelectedItem as Account;
            accountFromListBox.ItemsSource = new List<Account> { selAccount };
        }

        /// <summary>
        /// Обработчик нажатия кнопки выбора счета на который нужно перевести деньги
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void accountToButton_Click(object sender, RoutedEventArgs e)
        {
            var selAccount = AccountsListBox.SelectedItem as Account;
            accountToListBox.ItemsSource = new List<Account> { selAccount };
        }

        /// <summary>
        /// Обработчик нажатия кнопки перевода денег
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void transfer_Click(object sender, RoutedEventArgs e)
        {
            if (accountFromListBox.Items.Count < 1 || accountToListBox.Items.Count < 1)
            {
                return;
            }

            Account accountFrom = accountFromListBox.Items[0] as Account;
            Account accountTo = accountToListBox.Items[0] as Account;
            if (accountFrom == null || accountTo == null)
            {
                return;
            }
            if (!Int32.TryParse(sumTextBox.Text, out int sum))
            {
                MessageBox.Show("Введите корректную сумму!");                
                return;
            }
            accountFrom.BalanceTransferTo(accountTo, sum);
            ClientListView.Items.Refresh();
            MessageBox.Show("Перевод выполнен");
        }

        private void logsTransactions_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(LogsTransactions.PrintLogs());
        }

        private void generateTransaction_Click(object sender, RoutedEventArgs e)
        {

            if (accountFromListBox.Items.Count < 1 || accountToListBox.Items.Count < 1)
            {
                MessageBox.Show("Укажите счета");
                return;
            }

            Account accountFrom = accountFromListBox.Items[0] as Account;
            Account accountTo = accountToListBox.Items[0] as Account;

            Task task = new Task(()=>
            {
                for (int i = 0; i < 10_000_000; i++)
                {
                    LogsTransactions.AddTransaction(accountFrom, accountTo, 1);
                }
            });
            task.Start();
            //task.Wait();
            //ParallelLoopResult result = Parallel.For(0, 500, _ => 
            //{
            //    LogsTransactions.AddTransaction(accountFrom, accountTo, 1);
            //    LogsTransactions.AddTransaction(accountFrom, accountTo, -1);


            if(task.IsCompleted)
            {
                MessageBox.Show("Транзакции созданы");
            }

        }

        private void saveTransactionToJSON_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = "organisation";
            dlg.DefaultExt = ".json";
            dlg.Filter = "json (.json)|*.json";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;
                Task.Factory.StartNew(()=> File.WriteAllText(filename, JsonConvert.SerializeObject(LogsTransactions.ListTransaction)));              
            }
        }
    }
}
