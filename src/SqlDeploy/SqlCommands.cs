using JetBrains.Annotations;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace SqlDeploy
{
    internal static class SqlCommands
    {
        [NotNull]
        public static TSqlScript GetDeleteDb([NotNull] Identifier dbIdentifier)
        {
            return new TSqlScript
            {
                Batches =
                {
                    new TSqlBatch
                    {
                        Statements =
                        {
                            new UseStatement
                            {
                                DatabaseName = new Identifier
                                {
                                    Value = "master"
                                }
                            }
                        }
                    },
                    new TSqlBatch
                    {
                        Statements =
                        {
                            new IfStatement
                            {
                                Predicate = new BooleanIsNullExpression
                                {
                                    IsNot = true,
                                    Expression = new FunctionCall
                                    {
                                        FunctionName = new Identifier{Value = "DB_ID"},
                                        Parameters = { new StringLiteral
                                        {
                                            Value = dbIdentifier.Value,
                                            IsNational = true,
                                        }}
                                    }
                                },
                                ThenStatement = new BeginEndBlockStatement
                                {
                                    StatementList = new StatementList
                                    {
                                        Statements =
                                        {
                                            new AlterDatabaseSetStatement
                                            {
                                                DatabaseName = dbIdentifier,
                                                Options =
                                                {
                                                    new DatabaseOption
                                                    {
                                                        OptionKind = DatabaseOptionKind.SingleUser
                                                    }
                                                },
                                                Termination = new AlterDatabaseTermination
                                                {
                                                    ImmediateRollback = true
                                                },
                                            },
                                            new DropDatabaseStatement
                                            {
                                                Databases = {
                                                    dbIdentifier
                                                },
                                            },
                                        }
                                    }
                                }
                            },
                            
                        }
                    },
                }
            };
        }
    }
}
