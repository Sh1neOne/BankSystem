
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
            DepartamentsList.DataContext = bank.DepartamentList;
            ClientListView.DataContext = bank.DepartamentList;
            //bank.AllClients.Add(new VIPClient("123", "123"));
            //var a = new ObservableCollection<VIPClient>();
            //bank.CreateClientCol<VIPClient>(a);

        }

        private void AddClient_Click(object sender, RoutedEventArgs e)
        {
            var t = DepartamentsList.SelectedItem;

            if (t != null)
            {
                Client newClient;
                Departament<Client> dep;

                if (t is Departament<VIPClient> p)
                {
                    newClient = new VIPClient("", "");
                    dep = t as Departament<VIPClient>;
                }
                else if (t is Departament<CompanyClient>)
                {
                    newClient = new CompanyClient("", "");
                    dep = t as Departament<CompanyClient>;
                }
                else
                {
                    newClient = new StandartClient("", "");
                    dep = t as Departament<StandartClient>;
                }


                DialogClient dc = new DialogClient(newClient);
                if (dc.ShowDialog() == true)
                {
                    dep.AddClient(dc.ContextClient as VIPClient);
                }

            }
        }



        private void DepartamentsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ContentClients.Content = (DepartamentsList.SelectedItem as Departament<VIPClient>).Clients;
        }
    }
}
