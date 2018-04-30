using System;
using System.IO;
using System.Linq;
using Microsoft.Build.Construction;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace SqlDeploy.Readers
{
    public class SqlProjectReader
    {
        private readonly string _basePath;
        private readonly ProjectRootElement _project;
        private readonly SqlFileReader _sqlFileReader = new SqlFileReader();

        public SqlProjectReader(string projectPath)
        {
            if (projectPath == null) throw new ArgumentNullException(nameof(projectPath));

            _project = ProjectRootElement.Open(projectPath);

            if (_project == null)
            {
                throw new Exception($"Project '{projectPath}' is not found.");
            }

            _basePath = Path.GetDirectoryName(projectPath);
        }

        public SqlProject ReadProject()
        {
            return new SqlProject
            {
                Objects = ReadItems("Build"),
                PostDeploy = ReadItems("PostDeploy"),
                PreDeploy = ReadItems("PreDeploy")
            };
        }

        private TSqlScript ReadItems(string itemType)
        {
            var buildItems = _project.Items.Where(item => item.ItemType == itemType);

            var tableFiles = buildItems.Select(item => Path.Combine(_basePath, item.Include));

            var scripts = tableFiles.Select(path => _sqlFileReader.Read(path));

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
    }
}
