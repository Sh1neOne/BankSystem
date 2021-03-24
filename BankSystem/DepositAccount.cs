using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem
{
    class DepositAccount : Account
    {
        private bool capitalization;
        private int mountCount;
        private int interestRate;
        public DepositAccount(string name, int balance = 0) : base(name, balance)
        {
        }
        public DepositAccount(string name, int balance, bool capitalization, int interestRate, int mountCount) : this(name, balance)
        {
            Capitalization = capitalization;
            InterestRate = interestRate;
            MountCount = mountCount;
        }

        public override string Name { get => $"Депозитный счет {base.name}"; set => base.Name = value; }
        public bool Capitalization { get => capitalization; set => capitalization = value; }
        public int InterestRate { get => interestRate; set => interestRate = value; }
        public int MountCount { get => mountCount; set => mountCount = value; }
        
        /// <summary>
        /// Метод возвращает сумму накопленного депозита
        /// </summary>
        /// <returns></returns>
        public double CalculateDepositSum()
        {
            double balanceDeposit = Balance;
            if (!Capitalization)
            {
                balanceDeposit += (balanceDeposit * interestRate / 100) * mountCount/12;
            }
            else
            {
                for (int i = 0; i < mountCount; i++)
                {
                    balanceDeposit += balanceDeposit * interestRate / 100 / 12;
                }
            }
            return balanceDeposit;
        }

        
        public override string InformationAccount()
        {

            return $"{base.InformationAccount()}\n" +
                   $"Капитализация: {Capitalization}\n" +
                   $"Процентная ставка: {InterestRate}\n" +
                   $"Количество месяцев: {MountCount}\n" +
                   $"Сумма накопленная на окончание срока депозита: {Convert.ToString(CalculateDepositSum())}";
                
          
        }
     
    }
}
