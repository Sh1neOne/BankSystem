
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
        //LogsTransactions logs;
        public MainWindow()
        {
            InitializeComponent();
            Task<Bank> tBank = new Task<Bank>(() =>
            {
                return new Bank();
            }
            );
            
            tBank.Start();
            
            this.DataContext = tBank.Result.DepartamentList;
            AccountsListBox.DataContext = this.DataContext;

            this.IsEnabled = false;
            tBank.ContinueWith(_ =>
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        if (tBank.Result.DepartamentList.Count != 0)
                        {
                            this.IsEnabled = true;
                        }
                       
                    }
                    );
                }
            );

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
            MessageBox.Show(selAccount?.InformationAccount() ?? "Выберите счет");
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

            Task task = new Task(() =>
            {
                for (int i = 0; i < 10_000_000; i++)
                {
                    LogsTransactions.AddTransaction(accountFrom, accountTo, 1);
                }
            });
            task.Start();
            task.ContinueWith(_ => MessageBox.Show("Транзакции созданы"));

        }

        private void saveTransactionToJSON_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    var path = dialog.SelectedPath + "\\";
                    string filename = "log";
                    var t = Task.Factory.StartNew(() =>
                    {
                        int j = 0;
                        var tempList = new List<LogsTransactions>() { LogsTransactions.ListTransaction[0] };
                        for (int i = 1; i < LogsTransactions.ListTransaction.Count; i++)
                        {

                            tempList.Add(LogsTransactions.ListTransaction[i]);
                            if (i % 1_000_000 == 0)
                            {
                                using (var sw = new StreamWriter($"{path + filename + j++.ToString()}.json"))
                                {
                                    sw.Write(JsonConvert.SerializeObject(tempList));
                                }
                                tempList.Clear();
                            }
                        }
                        File.WriteAllText($"{path + filename + j++.ToString()}.json", JsonConvert.SerializeObject(tempList));
                    }
                    );
                    t.ContinueWith(_ => MessageBox.Show("Сохранение выполнено"));
                }
            }

        }

        private void loadTransactionFromJSON_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    var path = dialog.SelectedPath + "\\";
                    var files = Directory.GetFiles(path);

                    var par = Parallel.ForEach(files, file =>
                     {
                         using (var reader = new StreamReader(file))
                         {
                             using (var jsonReader = new JsonTextReader(reader))
                             {
                                 var serializer = new JsonSerializer();
                                 LogsTransactions.ListTransaction.AddRange(serializer.Deserialize<List<LogsTransactions>>(jsonReader));
                             }
                         }
                     }
                    );
                    if (par.IsCompleted)
                    {
                        MessageBox.Show("Загрузка завершена");
                    }
                }
            }
        }
    }
}
