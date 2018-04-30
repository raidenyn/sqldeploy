using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using com.rusanu.DBUtil;
using JetBrains.Annotations;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace SqlDeploy.Executer
{
    class SqlExecuter: IDisposable
    {
        private readonly SqlConnection _connection;
        private readonly SqlCmd _sqlCmd;

        private readonly SqlGenerator _generator = new SqlGenerator();

        public string ConnectionString => _connection.ConnectionString;

        public SqlExecuter([NotNull] string connectionString)
            :this(new SqlConnection(connectionString))
        { }

        public SqlExecuter([NotNull] SqlConnection sqlConnection)
        {
            _connection = sqlConnection ?? throw new ArgumentNullException(nameof(sqlConnection));
            _sqlCmd = new SqlCmd(_connection);
        }

        public async Task ExecuteAsync([NotNull] SqlProject project)
        {
            if (project == null) throw new ArgumentNullException(nameof(project));
            
            await _connection.OpenAsync().ConfigureAwait(false);

            try
            {
                if (project.PreDeployFilePath != null)
                {
                    _sqlCmd.ExecuteFile(project.PreDeployFilePath);
                }

                await ExecuteSqlAsync(project.Database.Batches).ConfigureAwait(false);
                await ExecuteSqlAsync(project.Objects.Batches).ConfigureAwait(false);

                if (project.PostDeployFilePath != null)
                {
                    _sqlCmd.ExecuteFile(project.PostDeployFilePath);
                }
            }
            finally
            {
                _connection.Close();
            }
        }

        public async Task ExecuteSqlAsync([NotNull, ItemNotNull] IEnumerable<TSqlBatch> batches)
        {
            foreach (var batch in batches)
            {
                await ExecuteSqlAsync(batch).ConfigureAwait(false);
            }
        }

        public Task ExecuteSqlAsync(TSqlBatch sqlBatch)
        {
            var command = _connection.CreateCommand();

            command.CommandText = _generator.GetSql(sqlBatch);

            return command.ExecuteNonQueryAsync();
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
