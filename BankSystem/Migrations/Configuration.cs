namespace BankSystem.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BankSystem.BankContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BankSystem.BankContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            //context.Departments.AddOrUpdate(x =>x.Id,
            //    new Department("����� ������ � VIP ���������") { Id = 1},
            //    new Department("����� ������ � �������� ���������") { Id = 2 },
            //    new Department("����� ������ � ������������ ������") { Id = 3 });
        }
    }
}
