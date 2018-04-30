using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using SqlDeploy.Executer;

namespace SqlDeploy
{
    internal class SqlDatabase: ISqlDatabase
    {
        private readonly SqlExecuter _sqlExecuter;
        private readonly Identifier _database;
        
        public string ConnectionString { get; }

        public SqlDatabase([NotNull] SqlExecuter sqlExecuter, Identifier database)
        {
            _sqlExecuter = sqlExecuter ?? throw new ArgumentNullException(nameof(sqlExecuter));
            _database = database;

            var builder = new SqlConnectionStringBuilder(sqlExecuter.ConnectionString)
            {
                InitialCatalog = database.Value
            };

            ConnectionString = builder.ToString();
        }

        public Task DeleteAsync()
        {
            return _sqlExecuter.ExecuteSqlScriptAsync(SqlCommands.GetDeleteDb(_database));
        }

        public void Dispose()
        {
            _sqlExecuter.Dispose();
        }
    }
}
