using System.IO;
using JetBrains.Annotations;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace SqlDeploy.Readers
{
    public class SqlFileReader
    {
        [NotNull]
        public TSqlScript Read([NotNull] string fileName)
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
    }
}
