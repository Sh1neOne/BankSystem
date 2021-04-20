using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem
{
    public class BankContext : DbContext
    {
        public BankContext() : base("DbConnection")
        {
            Database.SetInitializer<BankContext>(new Initializer());
        }
        public DbSet<Department> Departments { get; set; } 
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Client> Clients { get; set; }

    }

    /// <summary>
    /// Заполнение таблицы по умолчанию
    /// </summary>
    public class Initializer : DropCreateDatabaseIfModelChanges<BankContext>
    {
        protected override void Seed(BankContext context)
        {
            Department vipDepartment = new Department("Отдел работы с VIP клиентами") { Id = 1 };
            Department standartDepartment = new Department("Отдел работы с обычными клиентами") { Id = 2 };
            Department companyDepartment = new Department("Отдел работы с юридическими лицами") { Id = 3 };
            var Deps = new List<Department> {
               vipDepartment,
               standartDepartment,
               companyDepartment
               };

            context.Departments.AddRange(Deps);
            context.SaveChanges();
            for (int i = 1; i <= 5; i++)
            {
                var vipCl = new VIPClient($"ВИП Имя_{i}", $"Фамилия_{i}");
                vipDepartment.AddClient(vipCl);
                vipCl.AddRandomAccounts();

                var stmdCl = new StandartClient($"Стандарт Имя_{i}", $"Фамилия_{i}");
                standartDepartment.AddClient(stmdCl);
                stmdCl.AddRandomAccounts();

                var cmpCl = new CompanyClient($"Компания Имя_{i}", $"Фамилия_{i}");
                companyDepartment.AddClient(cmpCl);
                cmpCl.AddRandomAccounts();
            }
            context.SaveChanges();
            base.Seed(context);
        }
            
            
        }
    }

