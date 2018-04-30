using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace SqlDeploy.Modifiers
{
    internal class SetRecoveryMode: IModifier
    {
        public void Modify(SqlProject project)
        {
            project.Database.Batches.Add(new TSqlBatch
            {
                Statements =
                {
                    new AlterDatabaseSetStatement
                    {
                        DatabaseName = project.GetDatabaseName(),
                        Options =
                        {
                            new RecoveryDatabaseOption
                            {
                                Value = RecoveryDatabaseOptionKind.Simple
                            }
                        }
                    }
                }
            });
        }
    }
}
