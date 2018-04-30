using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SqlDeploy.Executer;
using SqlDeploy.Readers;

namespace SqlDeploy
{
    public class SqlProjectDeployer
    {
        private readonly SqlProjectReader _sqlProjectReader;
        private readonly Modifiers.Modifiers _modifiers = new Modifiers.Modifiers();

        public SqlProjectDeployer([NotNull] string sqlProjectFilePath)
        {
            _sqlProjectReader = new SqlProjectReader(sqlProjectFilePath);
        }

        public async Task<ISqlDatabase> RecreateToAsync([NotNull] SqlConnection connection, [NotNull] string databaseName)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (databaseName == null) throw new ArgumentNullException(nameof(databaseName));

            var sqlProject = _sqlProjectReader.ReadProject(databaseName);

            _modifiers.Modify(sqlProject);

            var executer = new SqlExecuter(connection);
            await executer.ExecuteAsync(sqlProject).ConfigureAwait(false);

            return new SqlDatabase(executer, sqlProject.GetDatabaseName());
        }
    }
}
