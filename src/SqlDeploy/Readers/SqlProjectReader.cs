using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.Build.Construction;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace SqlDeploy.Readers
{
    internal class SqlProjectReader
    {
        private readonly string _basePath;
        private readonly ProjectRootElement _project;
        private readonly SqlReader _sqlReader = new SqlReader();

        public SqlProjectReader([NotNull] string projectPath)
        {
            if (projectPath == null) throw new ArgumentNullException(nameof(projectPath));

            _project = ProjectRootElement.Open(projectPath);

            if (_project == null)
            {
                throw new Exception($"Project '{projectPath}' is not found.");
            }

            _basePath = Path.GetFullPath(Path.GetDirectoryName(projectPath));
        }

        public SqlProject ReadProject(string databaseName)
        {
            return new SqlProject
            {
                Database = RecreateDatabase(databaseName),
                Objects = ReadItems("Build"),
                PostDeployFilePath = GetFiles("PostDeploy").SingleOrDefault(),
                PreDeployFilePath = GetFiles("PreDeploy").SingleOrDefault(),
            };
        }

        [NotNull, ItemNotNull]
        private IEnumerable<string> GetFiles([NotNull] string itemType)
        {
            var buildItems = _project.Items.Where(item => item.ItemType == itemType);

            return buildItems.Select(item => Path.Combine(_basePath, item.Include.Replace("\\", "/")));
        }

        [NotNull]
        private TSqlScript ReadItems([NotNull] string itemType)
        {
            var tableFiles = GetFiles(itemType);

            var scripts = tableFiles.Select(path => _sqlReader.ReadFromFile(path));

            var resultScript = new TSqlScript();

            foreach (var sqlScript in scripts)
            {
                foreach (var scriptBatch in sqlScript.Batches)
                {
                    resultScript.Batches.Add(scriptBatch);
                }
            }

            return resultScript;
        }

        [NotNull]
        private TSqlScript RecreateDatabase([NotNull] string dbName)
        {
            var dbIdentifier = new Identifier
            {
                Value = dbName,
                QuoteType = QuoteType.SquareBracket
            };

            return new TSqlScript
            {
                Batches =
                {
                    new TSqlBatch
                    {
                        Statements =
                        {
                            new CreateDatabaseStatement
                            {
                                DatabaseName = dbIdentifier,
                            },
                        }
                    },
                    new TSqlBatch
                    {
                        Statements =
                        {
                            new UseStatement
                            {
                                DatabaseName = dbIdentifier,
                            }
                        }
                    }
                }
            };
        }
    }
}
