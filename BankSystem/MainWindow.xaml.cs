
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

        }

        private void AddClient_Click(object sender, RoutedEventArgs e)
        {
            var t = DepartamentsList.SelectedItem;
            if (t != null)
            {
                if (t is Departament<VIPClient> p)
                {
                    p.AddClientDialog(new VIPClient("",""));
                }
                else if (t is Departament<StandartClient> s)
                {
                    s.AddClientDialog(new StandartClient("", ""));
                }
                else if (t is Departament<CompanyClient> c)
                {
                    c.AddClientDialog(new CompanyClient("", ""));
                }
            }

        }

  


        private void DepartamentsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ContentClients.Content = (DepartamentsList.SelectedItem as Departament<VIPClient>).Clients;
        }

     
    }
}
