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
    /// Логика взаимодействия для AccountDialog.xaml
    /// </summary>
    public partial class AccountDialog : Window
    {
        private Account account;
        /// <summary>
        /// Конструктор окна счета. Если у клиента хорошая кредитная история, добавляется 1 процент к максимальной процентной ставке депозита
        /// </summary>
        /// <param name="client"></param>
        public AccountDialog(Client client)
        {
            InitializeComponent();
            var typeAccounts = GetAccountDictonary();
            AccountType.ItemsSource = typeAccounts;
            AccountType.SelectedItem = typeAccounts.First();

            for (int i = 1; i <= client.MaxInterestRate; i++)
            {
                interestRateComboBox.Items.Add(i);
            }

        }

        /// <summary>
        /// Обработчик нажатия кнопки ОК
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            Type selectTypeAccount = ((KeyValuePair<String, Type>)(AccountType.SelectedItem)).Value;

            if (!Int32.TryParse(balanceTextBox.Text, out int balance))
            {
                MessageBox.Show("Баланс введен не корректно!");
                return;
            }

            if ((!Int32.TryParse(countMonthTextBox.Text, out int countMonth) || countMonth < 1) && (selectTypeAccount == typeof(DepositAccount)))
            {
                MessageBox.Show("Не верно указано количество месяцеы");
                return;
            }


            try
            {
                if (selectTypeAccount == typeof(DepositAccount))
                {
                    Account = new DepositAccount(nametextBox.Text,
                                                 balance,
                                                 Convert.ToBoolean(capitalizaztionCheckBox.IsChecked),
                                                 Convert.ToInt32(interestRateComboBox.Text),
                                                 countMonth);
                }
                else
                {
                    Account = new Account(nametextBox.Text,
                                          balance);
                }
                this.DialogResult = true;
            }
            catch (AccountException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        /// <summary>
        /// Словарь наименование и типов счетов
        /// </summary>
        /// <returns></returns>
        private Dictionary<String, Type> GetAccountDictonary()
        {
            Dictionary<String, Type> dict = new Dictionary<string, Type>();
            dict.Add("Депозитный счет", typeof(DepositAccount));
            dict.Add("Обычный счет", typeof(Account));
            return dict;
        }

        public Account Account { get => account; set => account = value; }

        private void AccountType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Type selectTypeAccount = ((KeyValuePair<String, Type>)(AccountType.SelectedItem)).Value;
            Visibility depVisible = selectTypeAccount == typeof(DepositAccount) ? Visibility.Visible : Visibility.Collapsed;
            depositBlockStackPanel.Visibility = depVisible;
        }
    }
}
