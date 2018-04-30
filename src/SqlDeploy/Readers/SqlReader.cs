using System.IO;
using JetBrains.Annotations;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace SqlDeploy.Readers
{
    public class SqlReader
    {
        [NotNull]
        public TSqlScript ReadFromFile([NotNull] string fileName)
        {
            var parser = new TSql140Parser(initialQuotedIdentifiers: true, engineType: SqlEngineType.All);

            using (var file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var reader = new StreamReader(file))
                {
                    var script = (TSqlScript) parser.Parse(reader, out var errors);
                    return script;
                }
            }
        }

        [NotNull]
        public TSqlScript ReadFromString([NotNull] string sql)
        {
            var parser = new TSql140Parser(initialQuotedIdentifiers: true, engineType: SqlEngineType.All);

            using (var reader = new StringReader(sql))
            {
                var script = (TSqlScript)parser.Parse(reader, out var errors);
                return script;
            }
        }
    }
}
