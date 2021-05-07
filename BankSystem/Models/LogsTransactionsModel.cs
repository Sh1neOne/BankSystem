using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem
{
    class LogsTransactionsModel
    {
        private DateTime dateTransaction;
        private AccountModel accountFrom;
        private AccountModel accountTo;
        private double sumTransaction;
        private static List<LogsTransactionsModel> listTransaction = new List<LogsTransactionsModel>();
        public LogsTransactionsModel()
        {

        }

        public LogsTransactionsModel(AccountModel accountFrom, AccountModel accountTo, double sumTrans)
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

        public static void AddTransaction(AccountModel accountFrom, AccountModel accountTo, double sumTrans)
        {
            listTransaction.Add(new LogsTransactionsModel(accountFrom, accountTo, sumTrans));
        }

        public static Task GenerateTaskTransaction(AccountModel accountFrom, AccountModel accountTo)
        {

            Task task = new Task(() =>
            {
                for (int i = 0; i < 10_000_000; i++)
                {
                    LogsTransactionsModel.AddTransaction(accountFrom, accountTo, 1);
                }
            });
            return task;
        }

        public static Task SaveToJson(string path, string filename)
        {
            return Task.Factory.StartNew(() =>
            {
                int j = 0;
                var tempList = new List<LogsTransactionsModel>() { LogsTransactionsModel.ListTransaction[0] };
                for (int i = 1; i < LogsTransactionsModel.ListTransaction.Count; i++)
                {

                    tempList.Add(LogsTransactionsModel.ListTransaction[i]);
                    if (i % 1_000_000 == 0)
                    {
                        using (var sw = new StreamWriter($"{path + filename + j++.ToString()}.json"))
                        {
                            sw.WriteAsync(JsonConvert.SerializeObject(tempList));
                        }
                        tempList.Clear();
                    }
                }
                File.WriteAllText($"{path + filename + j++.ToString()}.json", JsonConvert.SerializeObject(tempList));
            }
            );
        }

        public static ParallelLoopResult LoadTransactionFromJSON(string[] files)
        {
            return Parallel.ForEach(files, file =>
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
        }


        public AccountModel AccountFrom { get => accountFrom; set => accountFrom = value; }
        public AccountModel AccountTo { get => accountTo; set => accountTo = value; }
        public double SumTransaction { get => sumTransaction; set => sumTransaction = value; }
        public static List<LogsTransactionsModel> ListTransaction { get => listTransaction; set => listTransaction = value; }
        public DateTime DateTransaction { get => dateTransaction; set => dateTransaction = value; }
    }
}
