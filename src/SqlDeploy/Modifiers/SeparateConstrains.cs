using System.Linq;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace SqlDeploy.Modifiers
{
    internal class SeparateConstrains: IModifier
    {
        public void Modify(SqlProject project)
        {
            foreach (var batch in project.Objects.Batches.ToList())
            {
                foreach (var statement in batch.Statements.OfType<CreateTableStatement>())
                {
                    var constrains = statement.Definition.TableConstraints;

                    if (constrains.Count == 0)
                    {
                        continue;
                    }

                    var alterTable =
                        new AlterTableAddTableElementStatement
                        {
                            SchemaObjectName = statement.SchemaObjectName,
                            Definition = new TableDefinition()
                        };

                    foreach (var constraint in constrains)
                    {
                        alterTable.Definition.TableConstraints.Add(constraint);
                    }

                    project.Objects.Batches.Add(new TSqlBatch
                    {
                        Statements = { alterTable }
                    });

                    statement.Definition.TableConstraints.Clear();
                }
            }
        }
    }
}
