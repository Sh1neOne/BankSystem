using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankLibrary;

namespace BankSystem
{
    class DepositAccountModel : AccountModel
    {
        private bool capitalization;
        private int mountCount;
        private int interestRate;

        public DepositAccountModel()
        {
        }

        public DepositAccountModel(string name, int balance = 0, int id = -1) : base(name, balance, id)
        {
        }
        public DepositAccountModel(string name, int balance, bool capitalization, int interestRate, int mountCount, int id = -1) : this(name, balance)
        {
            Capitalization = capitalization;
            InterestRate = interestRate;
            MountCount = mountCount;
            Id = id;
        }

        public override string Name { get => base.name; set => base.Name = value; }
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
                return CalculateDeposit.CalculateWithoutCapitalization(balanceDeposit, mountCount, interestRate);
            }
            else
            {
                return CalculateDeposit.CalculateWithCapitalization(balanceDeposit, mountCount, interestRate);    
            }
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
