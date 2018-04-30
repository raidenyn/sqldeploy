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

        private readonly SqlGenerator Generator = new SqlGenerator();

        public string ConnectionString { get; }

        public SqlExecuter([NotNull] string connectionString)
            :this(new SqlConnection(connectionString))
        { }

        public SqlExecuter([NotNull] SqlConnection sqlConnection)
        {
            _connection = sqlConnection ?? throw new ArgumentNullException(nameof(sqlConnection));
            _sqlCmd = new SqlCmd(_connection);
            ConnectionString = _connection.ConnectionString;
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

                await ExecuteSqlAsync(project.Database).ConfigureAwait(false);
                await ExecuteSqlAsync(project.Objects).ConfigureAwait(false);

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

        public async Task ExecuteSqlScriptAsync([NotNull] TSqlScript sqlScript)
        {
            if (sqlScript == null) throw new ArgumentNullException(nameof(sqlScript));
            await _connection.OpenAsync().ConfigureAwait(false);

            try
            {
                await ExecuteSqlAsync(sqlScript).ConfigureAwait(false);
            }
            finally
            {
                _connection.Close();
            }
        }

        private async Task ExecuteSqlAsync([NotNull] TSqlScript sqlScript)
        {
            foreach (var batch in sqlScript.Batches)
            {
                await ExecuteSqlAsync(batch).ConfigureAwait(false);
            }
        }

        private Task ExecuteSqlAsync(TSqlBatch sqlBatch)
        {
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = Generator.GetSql(sqlBatch);

                return command.ExecuteNonQueryAsync();
            }
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
