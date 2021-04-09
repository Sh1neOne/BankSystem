using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem
{
    class LogsTransactions 
    {
        private DateTime dateTransaction;
        private Account accountFrom;
        private Account accountTo;
        private double sumTransaction;
        private static List<LogsTransactions> listTransaction = new List<LogsTransactions>();
        public LogsTransactions()
        {

        }

        public LogsTransactions(Account accountFrom, Account accountTo, double sumTrans)
        {         
            AccountFrom = accountFrom;
            AccountTo = accountTo;
            SumTransaction = sumTrans;
            DateTransaction = DateTime.Now;
        }

        public static string PrintLogs()
        {
            StringBuilder logsSB = new StringBuilder();
            foreach (var log in listTransaction)
            {
                logsSB.AppendLine($"Перевод {log.DateTransaction.ToString()}: {log.AccountFrom.Name} -> {log.AccountTo.Name} на сумму {log.SumTransaction}");
            }
            return logsSB.ToString();
        }

        public static void AddTransaction(Account accountFrom, Account accountTo, double sumTrans)
        {
            listTransaction.Add(new LogsTransactions(accountFrom, accountTo, sumTrans));
        }

       
  

        public Account AccountFrom { get => accountFrom; set => accountFrom = value; }
        public Account AccountTo { get => accountTo; set => accountTo = value; }
        public double SumTransaction { get => sumTransaction; set => sumTransaction = value; }
        public static List<LogsTransactions> ListTransaction { get => listTransaction; set => listTransaction = value; }
        public DateTime DateTransaction { get => dateTransaction; set => dateTransaction = value; }
    }
}
