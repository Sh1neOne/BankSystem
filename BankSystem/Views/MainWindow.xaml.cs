
using BankSystem.ViewModels;
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
        BankDbContext bankContext;
        //LogsTransactions logs;
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new ApplicationViewModel();
            //bankContext = new BankContext();
            //this.DataContext = bankContext.Departments.Include("Clients.Accounts").ToList();
            //AccountsListBox.DataContext = this.DataContext;

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
                                 LogsTransactionsModel.ListTransaction.AddRange(serializer.Deserialize<List<LogsTransactionsModel>>(jsonReader));
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
