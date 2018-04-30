using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace SqlDeploy.Executer
{
    public class SqlExecuter
    {
        private readonly DbConnection _connection;

        public SqlExecuter(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public void Execute(string sql)
        {
            _connection.Open();

            try
            {
                using (var tran = _connection.BeginTransaction(IsolationLevel.Chaos))
                {
                    ExecuteSql(sql);
                    tran.Commit();
                }
            }
            finally
            {
                _connection.Close();
            }
        }

        private void ExecuteSql(string sql)
        {
            var command = _connection.CreateCommand();

            command.CommandText = sql;

            command.ExecuteNonQuery();
        }
    }
}
