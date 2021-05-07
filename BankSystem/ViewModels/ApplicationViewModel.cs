using BankSystem.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BankSystem.ViewModels
{
    public class ApplicationViewModel : BaseViewModel
    {
        private ObservableCollection<DepartmentModel> departments;
        private DepartmentModel selectedDepartment;
        private ClientModel selectedClient;
        private AccountModel selectedAccount;
        private ObservableCollection<AccountModel> accountFrom;
        private ObservableCollection<AccountModel> accountTo;
        private string sumTransfer;
        public ICommand InformationAccountCommand { get; }
        public ICommand AddClientCommand { get; }
        public ICommand EditClientCommand { get; }
        public ICommand DeleteClientCommand { get; }
        public ICommand AddAccountCommand { get; }
        public ICommand DeleteAccountCommand { get; }
        public ICommand SelectTransferAccountFromCommand { get; }
        public ICommand SelectTransferAccountToCommand { get; }
        public ICommand TransferSumCommand { get; }
        public ICommand ShowTransactionCommand { get; }
        public ICommand GenerateTransaction { get; }
        public ICommand SaveTransactionToJSONCommand { get; }
        public ICommand LoadTransacionFromJSONCommand { get; }
        
        private readonly BankDbContext bankDbContext;

        public ApplicationViewModel()
        {
            var bank = new BankModel();
            bankDbContext = bank.BankDbContext;
            accountFrom = new ObservableCollection<AccountModel>();
            accountTo = new ObservableCollection<AccountModel>();

            Departments = new ObservableCollection<DepartmentModel>(bank.Departments);
            InformationAccountCommand = new DelegateCommand(OnInformationCommadAccountExecuted, OnInformationAccountCanExecute);
            AddClientCommand = new DelegateCommand(OnAddClientCommandExecuted, OnAddClientCanExecute);
            EditClientCommand = new DelegateCommand(OnEditClientCommandExecuted, OnEditClientCanExecute);
            DeleteClientCommand = new DelegateCommand(OnDeleteClientCommandExecuted, OnDeleteClientCanExecute);

            AddAccountCommand = new DelegateCommand(OnAddAccountCommandExecuted, OnAddClietnCanExecute);
            DeleteAccountCommand = new DelegateCommand(OnDeleteAccountCommandExecuted, OnDeleteAccountCanExecute);
            SelectTransferAccountFromCommand = new DelegateCommand(OnSelectTransferAccountFromExecuted, OnSelectTransferAccountFromCanExecute);
            SelectTransferAccountToCommand = new DelegateCommand(OnSelectTransferAccountToExecuted, OnSelectTransferAccountToCanExecute);
            TransferSumCommand = new DelegateCommand(OnTransferSumToExecuted, OnTransferSumCanExecute);

            ShowTransactionCommand = new DelegateCommand(OnShowTransactionCommandExecuted);
            GenerateTransaction = new DelegateCommand(OnGenerateTransactionCommandExecuted, OnGenerateTransactionCommandCanExecute);

            SaveTransactionToJSONCommand = new DelegateCommand(OnSavetransactionInJSONCommandExecuted);
            LoadTransacionFromJSONCommand = new DelegateCommand(OnLoadTransactionFromJSONCommandExecuted);
        }

        private void OnLoadTransactionFromJSONCommandExecuted(object obj)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    var path = dialog.SelectedPath + "\\";
                    var files = Directory.GetFiles(path);

                    ParallelLoopResult par = LogsTransactionsModel.LoadTransactionFromJSON(files);

                    if (par.IsCompleted)
                    {
                        MessageBox.Show("Загрузка завершена");
                    }
                }
            }
        }

        private void OnSavetransactionInJSONCommandExecuted(object obj)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    var path = dialog.SelectedPath + "\\";
                    string filename = "log";
                    Task t = LogsTransactionsModel.SaveToJson(path, filename);
                    t.ContinueWith(_ => MessageBox.Show("Сохранение выполнено"));
                }
            }
        }

        private void OnGenerateTransactionCommandExecuted(object obj)
        {
            var t = LogsTransactionsModel.GenerateTaskTransaction(AccountFrom[0], AccountTo[0]);
            t.Start();            
            t.ContinueWith(_ => MessageBox.Show("Транзакции созданы"));

        }

        private bool OnGenerateTransactionCommandCanExecute(object arg)
        {
            if (AccountFrom.Count == 0 || AccountTo.Count == 0)
            {
                return false;
            }
            return true;
        }

       


        #region Команды клиента
        /// <summary>
        /// Команда на добавление клиента
        /// </summary>
        /// <param name="obj"></param>
        private void OnAddClientCommandExecuted(object obj)
        {          
            DialogClient dc = new DialogClient(SelectedDepartment);
            if (dc.ShowDialog() == true)
            {
                SelectedDepartment.AddClient(dc.ContextClient);
            }
        }
        private bool OnAddClientCanExecute(object arg)
        {
            return SelectedDepartment != null;
        }
        /// <summary>
        /// Команда редактирования клиента
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private void OnEditClientCommandExecuted(object obj)
        {
            var dc = new DialogClient(SelectedClient);
            dc.ShowDialog();
        }
        private bool OnEditClientCanExecute(object arg) => SelectedClient != null;
        /// <summary>
        /// Команда удаления клиента
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private void OnDeleteClientCommandExecuted(object obj)
        {
            SelectedDepartment.DeleteClient(SelectedClient);
        }
        private bool OnDeleteClientCanExecute(object arg) => SelectedClient != null;
        #endregion
        #region Команды счета
        /// <summary>
        /// Команда добавления счета
        /// </summary>
        /// <param name="obj"></param>
        private void OnAddAccountCommandExecuted(object obj)
        {
            AccountDialog ad = new AccountDialog(SelectedClient);
            if (ad.ShowDialog() == true)
            {
                SelectedClient.AddAccount(ad.Account);
            }
        }
        private bool OnAddClietnCanExecute(object arg) => SelectedClient != null;
        /// <summary>
        /// Команда удаления счета
        /// </summary>
        /// <param name="obj"></param>
        private void OnDeleteAccountCommandExecuted(object obj)
        {
            SelectedClient.DeleteAccount(SelectedAccount);
        }
        private bool OnDeleteAccountCanExecute(object arg) => SelectedAccount != null;
        /// <summary>
        /// Команда информации о счете
        /// </summary>
        /// <param name="obj"></param>
        private void OnInformationCommadAccountExecuted(object obj) =>
             MessageBox.Show(SelectedAccount?.InformationAccount() ?? "Выберите счет");
        private bool OnInformationAccountCanExecute(object arg) => SelectedAccount != null;
        /// <summary>
        /// Команда выбора счетов для перевода 
        /// </summary>
        /// <param name="obj"></param>
        private void OnSelectTransferAccountFromExecuted(object obj)
        {
            AccountFrom = new ObservableCollection<AccountModel>() { SelectedAccount };
        }
        private void OnSelectTransferAccountToExecuted(object obj)
        {
            AccountTo = new ObservableCollection<AccountModel>() { SelectedAccount };
        }
        private bool OnSelectTransferAccountFromCanExecute(object arg) 
        {
            if (SelectedAccount == null)
            {
                return false;
            }
            if (AccountTo.Count != 0 && AccountTo[0] == SelectedAccount)
            {
                return false;
            }
            return true;           
        }
        private bool OnSelectTransferAccountToCanExecute(object arg)
        {
            if (SelectedAccount == null)
            {
                return false;
            }
            if (AccountFrom.Count != 0 && AccountFrom[0] == SelectedAccount)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Команда выполняет перевод с счета на счет 
        /// </summary>
        /// <param name="obj"></param>
        private void OnTransferSumToExecuted(object obj)
        {
            if (!Int32.TryParse(SumTransfer, out int sum))
            {
                MessageBox.Show("Введите корректную сумму!");
                SumTransfer = "";
                return;
            }
            if (sum > AccountFrom[0].Balance)
            {
                MessageBox.Show("Сумма перевода не может быть больше суммы баланса на счете!");
                return;
            }
            AccountFrom[0].BalanceTransferTo(AccountTo[0], sum);
            SumTransfer = "";
            MessageBox.Show("Перевод выполнен");
        }
        private bool OnTransferSumCanExecute(object arg)
        {
            if (AccountFrom.Count == 0 || AccountTo.Count == 0)
            {
                return false;
            }
            return true;
        }
        #endregion

        #region Остальные команды
        /// <summary>
        /// Команда показывает транзакции
        /// </summary>
        /// <param name="obj"></param>
        private void OnShowTransactionCommandExecuted(object obj)
        {
            MessageBox.Show(LogsTransactionsModel.PrintLogs());
        }

        
        #endregion

        public ObservableCollection<DepartmentModel> Departments { get => departments; set => departments = value; }
        public DepartmentModel SelectedDepartment { get => selectedDepartment; set => Set<DepartmentModel>(ref selectedDepartment, value); }
        public AccountModel SelectedAccount { get => selectedAccount; set => Set<AccountModel>(ref selectedAccount, value); }
        public ClientModel SelectedClient { get => selectedClient; set => Set<ClientModel>(ref selectedClient, value); }

        public ObservableCollection<AccountModel> AccountFrom { get => accountFrom; set => Set<ObservableCollection<AccountModel>>(ref accountFrom, value); }
        public ObservableCollection<AccountModel> AccountTo { get => accountTo; set => Set<ObservableCollection<AccountModel>>(ref accountTo, value); }
        public string SumTransfer { get => sumTransfer; set => Set(ref sumTransfer, value); }
    }
}
