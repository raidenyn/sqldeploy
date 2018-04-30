using JetBrains.Annotations;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace SqlDeploy
{
    internal class SqlProject
    {
        [NotNull]
        public TSqlScript Database { get; set; }

        [NotNull]
        public TSqlScript Objects { get; set; }

        [CanBeNull]
        public string PreDeployFilePath { get; set; }

        [CanBeNull]
        public string PostDeployFilePath { get; set; }
    }
}
