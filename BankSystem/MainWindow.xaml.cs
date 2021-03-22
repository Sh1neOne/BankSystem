
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        public MainWindow()
        {
            InitializeComponent();
            Bank bank = new Bank();
            DepartamentsList.ItemsSource = bank.DepartamentList;
            //bank.AllClients.Add(new VIPClient("123", "123"));
            //var a = new ObservableCollection<VIPClient>();
            //bank.CreateClientCol<VIPClient>(a);

        }

        private void AddClient_Click(object sender, RoutedEventArgs e)
        {
            var t = DepartamentsList.SelectedItem;
 
            if (t is Departament<VIPClient> p)
            {
                p.AddClient(new VIPClient("123", "123"));
            }
            
        }
    }
}
