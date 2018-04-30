using System.Linq;
using JetBrains.Annotations;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace SqlDeploy
{
    internal static class SqlProjectExtensions
    {
        [CanBeNull]
        public static Identifier GetDatabaseName([NotNull] this SqlProject project)
        {
            var createStatement =
                (from batch in project.Database.Batches
                    from statement in batch.Statements.OfType<CreateDatabaseStatement>()
                    select statement
                ).SingleOrDefault();

            return createStatement?.DatabaseName;
        }
    }
}
