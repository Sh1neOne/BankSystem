
using System;
using System.Collections.Generic;
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
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Bank bank = new Bank();
            //bank.VipDepartamnent.AddClient(new VIPClient("123", "123"));
            DepartamentsList.ItemsSource = bank.ListDepartaments;
            //VIPDepartament.DataContext = bank.VipDepartamnent;
            //StandartDepartament.DataContext = bank.StandartDepartamnent;
            //CompanyDepartament.DataContext = bank.CompanyDepartamnent;
            var dep = new Departament<VIPClient>("123");
            dep.AddClient(new Client())

        }

        private void AddClient_Click(object sender, RoutedEventArgs e)
        {
        
        }
    }
}
