using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BankSystem.DataBase
{
    class BankSQL
    {
        private string _connectionString;
        private SqlConnection _sqlConnection = null;

        public BankSQL(SqlConnectionStringBuilder connectStrBuilder)
        {
            _connectionString = connectStrBuilder.ConnectionString;
        }

        /// <summary>
        /// Открытие соединения с БД
        /// </summary>
        private void OpenConnection()
        {
            _sqlConnection = new SqlConnection { ConnectionString = _connectionString };
            _sqlConnection.Open();
        }

        /// <summary>
        /// Закрытие соединения с БД
        /// </summary>
        private void CloseConnection()
        {
            if (_sqlConnection?.State != ConnectionState.Closed)
            {
                _sqlConnection?.Close();
            }
        }

        /// <summary>
        /// Проверяет доступна ли база данныъ
        /// </summary>
        /// <returns></returns>
        public bool ConnectionAvailable()
        {
            bool connectAvailable = false;
            try
            {
                OpenConnection();
                if (_sqlConnection.State == ConnectionState.Open)
                {
                    connectAvailable = true;
                }
            }
            catch (SqlException e)
            {
                connectAvailable = false;
                MessageBox.Show(e.Message);
            }
            finally
            {
                CloseConnection();
            }
            return connectAvailable;
        }

        /// <summary>
        /// Читает данные из БД и создает объекты соответсвующие этим данным
        /// </summary>
        /// <param name="bank"></param>
        public void ReadDataInDB(Bank bank)
        {
            bank.DepartamentList = GetAllDepartments();
            foreach (var dep in bank.DepartamentList)
            {
                if (dep is Departament<StandartClient> s)
                {
                    var listStandartClient = GetDepartmentClient<StandartClient>(s.Id);
                    bank.StandartDepartament = s;
                    s.Clients = listStandartClient;
                    foreach (var cl in s.Clients)
                    {
                        cl.Accounts = GetAccounts(cl);
                    }
                }
                else if(dep is Departament<VIPClient> v)
                {
                    var listVipClient = GetDepartmentClient<VIPClient>(v.Id);
                    bank.VipDepartament = v;
                    v.Clients = listVipClient;
                    foreach (var cl in v.Clients)
                    {
                        cl.Accounts = GetAccounts(cl);
                    }
                }
                else if(dep is Departament<CompanyClient> c)
                {
                    var listCompanyClient = GetDepartmentClient<CompanyClient>(c.Id);
                    bank.CompanyDepartament = c;
                    c.Clients = listCompanyClient;
                    foreach (var cl in c.Clients)
                    {
                        cl.Accounts = GetAccounts(cl);
                    }
                }
            }
        }
  
        /// <summary>
        /// Получает все департаменты из БД и создает соответсвующие объекты
        /// </summary>
        /// <returns></returns>
        public ArrayList GetAllDepartments()
        {
            var ArrayDeps = new ArrayList();
            var sql = @"select * from Departments";
            try
            {
                OpenConnection();
                using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
                {
                    SqlDataReader r = command.ExecuteReader();
                    while (r.Read())
                    {
                        var depName = r["name"].ToString();
                        var depId = r.GetInt32(0);
                        var depType = r.GetInt32(2);
                        switch (depType)
                        {
                            case 1:
                                Departament<StandartClient> dep = new Departament<StandartClient>(depName, depId);
                                ArrayDeps.Add(dep);
                                break;
                            case 2:
                                Departament<CompanyClient> depCCl = new Departament<CompanyClient>(depName, depId);
                                ArrayDeps.Add(depCCl);
                                break;
                            default:
                                Departament<VIPClient> depVip = new Departament<VIPClient>(depName, depId);
                                ArrayDeps.Add(depVip);
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            finally
            {
                CloseConnection();
            }
            return ArrayDeps;
        }

        /// <summary>
        /// Получает клиентов по департаменту из БД и создает соответсвующие объекты
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="depId"></param>
        /// <returns></returns>
        public ObservableCollection<T> GetDepartmentClient<T>(int depId)
            where T : Client, new()
        {
            var clientList = new ObservableCollection<T>();

            try
            {
                OpenConnection();
                var sql = @"SELECT * from Clients WHERE departmentId = @id";
                using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
                {
                    var param = new SqlParameter()
                    {
                        ParameterName = "@id",
                        Value = depId,
                        SqlDbType = SqlDbType.Int
                    };
                    command.Parameters.Add(param);
                    SqlDataReader r = command.ExecuteReader();
                    while (r.Read())
                    {
                        var clietnId = r.GetInt32(0);
                        var firsName = r.GetString(1).Trim();
                        var lastName = r.GetString(2).Trim();
                        var goodCreditHistory = r.GetBoolean(3);
                        var clientType = r.GetInt32(4);
                        clientList.Add(new T() { FirstName = firsName, Id = clietnId, LastName = lastName });
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            finally
            {
                CloseConnection();
            }

            return clientList;
        }

        /// <summary>
        /// Получает счета клиентов из бд, и создает соответсвующие объекты
        /// </summary>
        /// <param name="cl"></param>
        /// <returns></returns>
        public ObservableCollection<Account> GetAccounts(Client cl)
        {
            var sql = $@"SELECT * FROM Accounts WHERE clientId = {cl.Id}";
            var accountsList = new ObservableCollection<Account>(); 
            try
            {
                OpenConnection();
                using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
                {
                    SqlDataReader r = command.ExecuteReader();
                    while (r.Read())
                    {
                        var id = r.GetInt32(0);
                        var clientId = r.GetInt32(1);
                        var name = r.GetString(2);
                        var balance = r.GetSqlMoney(3).ToInt32();
                        var accType = r.GetInt32(4);
                      
                        switch (accType)
                        {
                            case 1:
                                accountsList.Add(new Account(name, balance, id));
                                break;
                            default:
                                var capitalization = r.GetInt32(5);
                                var mountCount = r.GetInt32(6);
                                var interestRatte = r.GetInt32(7);
                                accountsList.Add(new DepositAccount(name, balance, Convert.ToBoolean(capitalization), interestRatte, mountCount, id));
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            finally
            {
                CloseConnection();
            }
            return accountsList;
        }

        #region Account
        /// <summary>
        /// Обновляет данные счета в БД
        /// </summary>
        /// <param name="acc"></param>
        public void UpdateAccountInDB(Account acc)
        {
            OpenConnection();
 
            var sql = "";
            if (acc.GetType() == typeof(Account))
            {
                sql = $@"UPDATE Accounts SET name = N'{acc.Name}',
                                             balance = N'{acc.Balance}'
                                             WHERE id = N'{acc.Id}'";
            }
            else
            {
                var DepAcc = (DepositAccount)acc;
                var capitaliz = DepAcc.Capitalization ? 1 : 0;
                sql = $@"UPDATE Accounts SET name = N'{DepAcc.Name}',
                                             balance = N'{DepAcc.Balance}',
                                             capitalization = N'{capitaliz}',
                                             mountCount = N'{DepAcc.MountCount}',
                                             interestRate = N'{DepAcc.InterestRate}'
                                             WHERE id = {DepAcc.Id}";
            }
            try
            {
                OpenConnection();
                using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
                {
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }
        /// <summary>
        /// Удаляет счет из БД
        /// </summary>
        /// <param name="acc"></param>
        public void DeleteAccountInDb(Account acc)
        {
            var sql = $"DELETE FROM Accounts WHERE id = {acc.Id}"; ;
            OpenConnection();
            try
            {
                using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
                {
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// Добавляет счет в БД
        /// </summary>
        /// <param name="acc"></param>
        /// <param name="clientId"></param>
        public void AddAccountInDB(Account acc, int clientId)
        {
            int accountType;
            string sql;
            if (typeof(Account) == acc.GetType())
            {
                accountType = 1;
                sql = $@"INSERT INTO Accounts (clientId,  name,  balance, accountType) 
                                     VALUES (N'{clientId}', N'{acc.Name}', N'{acc.Balance}', N'{accountType}');
                                     SET @id = @@IDENTITY;";                

            }
            else
            {
                accountType = 2;
                var depAcc = (DepositAccount)acc;
                sql = $@"INSERT INTO Accounts (clientId,  name,  balance, accountType, capitalization, mountCount, interestRate) 
                                     VALUES (N'{clientId}', N'{depAcc.Name}', N'{depAcc.Balance}', N'{accountType}', N'{Convert.ToInt32(depAcc.Capitalization)}', N'{depAcc.MountCount}', N'{depAcc.InterestRate}');
                                     SET @id = @@IDENTITY;";
            }

            try
            {
                OpenConnection();
                using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
                {
                    SqlParameter personId = new SqlParameter("@id", SqlDbType.Int);
                    personId.Direction = ParameterDirection.Output;
                    command.Parameters.Add(personId);
                    command.ExecuteNonQuery();
                    if (acc.Id == -1)
                    {
                        acc.Id = Convert.ToInt32(personId.Value);
                    }
                }
               

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            finally
            {
                CloseConnection();
            }

        
        }
        #endregion

        #region Client
        
        /// <summary>
        /// Добавляет клиента в БД
        /// </summary>
        /// <param name="client"></param>
        /// <param name="depId"></param>
        public void AddClientInDB(Client client, int depId)
        {
            int clientType;
            if (client.GetType() == typeof(CompanyClient))
            {
                clientType = 2;
            }
            else if (client.GetType() == typeof(StandartClient))
            {
                clientType = 1;
            }
            else
            {
                clientType = 3;
            }
            var sql = $@"INSERT INTO Clients (firstName,  lastName,  goodCreditHistory, clientType, departmentId) 
                                     VALUES (N'{client.FirstName}', N'{client.LastName}', N'{client.GoodCreditHistory}', N'{clientType}', N'{depId}');
                                     SET @id = @@IDENTITY;";
            try
            {
                OpenConnection();
                using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
                {
                    SqlParameter clientId = new SqlParameter("@id", SqlDbType.Int);
                    clientId.Direction = ParameterDirection.Output;
                    command.Parameters.Add(clientId);
                    command.ExecuteNonQuery();
                    if (client.Id == -1)
                    {
                        client.Id = Convert.ToInt32(clientId.Value);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }
        /// <summary>
        /// Обновляет клиента в БД
        /// </summary>
        /// <param name="client"></param>
        public void UpdateClientInDB(Client client)
        {
            var sql = $@"UPDATE Clients SET     
                               firstName = N'{client.FirstName}',  
                               lastName = N'{client.LastName}',  
                               goodCreditHistory =  N'{client.GoodCreditHistory}' 
                               WHERE id = {client.Id}";
            try
            {
                OpenConnection();
                using (SqlCommand command = new SqlCommand(sql, _sqlConnection)) 
                {
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }
        /// <summary>
        /// Удаляет клиента из БД
        /// </summary>
        /// <param name="client"></param>
        public void DeleteClientInDB(Client client)
        {
            var sql = $"DELETE FROM Clients WHERE id = {client.Id}"; ;
            OpenConnection();
            try
            {
                using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
                {
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }
        #endregion
    }
}
