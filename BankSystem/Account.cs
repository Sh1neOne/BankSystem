﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem
{
    public class Account : INotifyPropertyChanged
    {
        protected string name;
        private double balance;

        public static event Action<Account,Account,double> CommitTransaction;

        public Action<string, Account> BalanceChanged;
   
        public Account(string name, double balance = 0)
        {
            Name = name;
            Balance = balance;
        }
        /// <summary>
        /// Метод осуществляет перевод денег му счетами
        /// </summary>
        /// <param name="accountFrom"></param>
        /// <param name="accountTo"></param>
        /// <param name="sum"></param>
        public void BalanceTransferTo(Account accountTo, double sum)
        {
            this.Balance -= sum;
            accountTo.Balance += sum;
            CommitTransaction?.Invoke(this, accountTo, sum);

        } 

        /// <summary>
        /// Метод возвращает информацию о счете
        /// </summary>
        /// <returns></returns>
        public virtual string InformationAccount()
        {
            return $"Счет: {Name}\nБаланс: {Balance}";
        }

        
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public virtual string Name { get =>$"Стандартный счет {name}"; set => name = value; }
        public double Balance { get => balance; 
            set
            {
                balance = value;
                BalanceChanged?.Invoke(value.ToString(), this);
                OnPropertyChanged();           
            }
        }
    }
}
