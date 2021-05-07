using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem
{
    public class BankDbContext : DbContext
    {
        public BankDbContext() : base("DbConnection")
        {
            Database.SetInitializer<BankDbContext>(new Initializer());
        }
        public DbSet<DepartmentModel> Departments { get; set; } 
        public DbSet<AccountModel> Accounts { get; set; }
        public DbSet<ClientModel> Clients { get; set; }

    }



    /// <summary>
    /// Заполнение таблицы по умолчанию
    /// </summary>
    public class Initializer : DropCreateDatabaseIfModelChanges<BankDbContext>
    {
        protected override void Seed(BankDbContext context)
        {
            DepartmentModel vipDepartment = new DepartmentModel("Отдел работы с VIP клиентами") { Id = 1 };
            DepartmentModel standartDepartment = new DepartmentModel("Отдел работы с обычными клиентами") { Id = 2 };
            DepartmentModel companyDepartment = new DepartmentModel("Отдел работы с юридическими лицами") { Id = 3 };
            var Deps = new List<DepartmentModel> {
               vipDepartment,
               standartDepartment,
               companyDepartment
               };

            context.Departments.AddRange(Deps);
            context.SaveChanges();
            for (int i = 1; i <= 5; i++)
            {
                var vipCl = new VIPClientModel($"ВИП Имя_{i}", $"Фамилия_{i}");
                vipDepartment.AddClient(vipCl);
                vipCl.AddRandomAccounts();

                var stmdCl = new StandartClientModel($"Стандарт Имя_{i}", $"Фамилия_{i}");
                standartDepartment.AddClient(stmdCl);
                stmdCl.AddRandomAccounts();

                var cmpCl = new CompanyClientModel($"Компания Имя_{i}", $"Фамилия_{i}");
                companyDepartment.AddClient(cmpCl);
                cmpCl.AddRandomAccounts();
            }
            context.SaveChanges();
            base.Seed(context);
        }
            
            
        }
   
    
    }

