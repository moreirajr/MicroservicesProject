using Dapper;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Contas.Infra.Data.Dapper
{
    public class ContasContextDb : IDisposable
    {
        private readonly IDbConnection _dbConnection;
        private IDbTransaction _currentTransaction;
        private readonly string _connectionString;

        public ContasContextDb(string connection)
        {
            _dbConnection = string.IsNullOrEmpty(connection) ? throw new ArgumentNullException() : new SqlConnection(connection);
            _connectionString = connection;

            CreateDatabaseIfNotExists();
            CreateTablesIfNotExists();
        }

        private string ChangeConnectionStringDatabase(string database) => _connectionString.Replace("MicroservicesProjectDB_Conta", database);

        private void CreateDatabaseIfNotExists()
        {
            string sql = @"
                        IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'MicroservicesProjectDB_Conta')
                        BEGIN
                            CREATE DATABASE MicroservicesProjectDB_Conta
                        END";

            string conn = ChangeConnectionStringDatabase("master");
            using (var masterConnection = new SqlConnection(conn))
            {
                masterConnection.Open();
                masterConnection.Execute(sql);
                masterConnection.Close();
            }
        }

        private void CreateTablesIfNotExists()
        {
            DapperDatabaseHelper.CreateTablesIfNotExists(_dbConnection);
        }

        public IDbConnection Connection => _dbConnection;

        public IDbTransaction GetCurrentTransaction() => _currentTransaction;
        public bool HasActiveTransaction => _currentTransaction != null;

        public IDbTransaction BeginTransaction()
        {
            if (_currentTransaction != null) return null;

            _currentTransaction = _dbConnection.BeginTransaction();

            return _currentTransaction;
        }

        public void CommitTransaction(IDbTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction is not current");

            try
            {
                transaction.Commit();
            }
            catch
            {
                RollBackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollBackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void Dispose()
        {
            _dbConnection.Dispose();
        }
    }
}