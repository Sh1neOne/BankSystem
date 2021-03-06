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
using System.Windows.Shapes;

namespace BankSystem
{
    /// <summary>
    /// Логика взаимодействия для DialogClient.xaml
    /// </summary>
    public partial class DialogClient : Window

    {
        private Client contextClient;
        public DialogClient(Client contextCl)
        {
            ContextClient = contextCl;
            this.DataContext = ContextClient;
            InitializeComponent();
        }

        public Client ContextClient { get => contextClient; set => contextClient = value; }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            ContextClient.GoodCreditHistory = goodCreditHistoryCheckBox.IsEnabled;
        }
    }
}
