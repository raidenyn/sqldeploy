using System;
using System.IO;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace SqlDeploy.Readers
{
    public class SqlFileReader
    {
        public TSqlScript Read(string fileName)
        {
            var parser = new TSql140Parser(initialQuotedIdentifiers: true, engineType: SqlEngineType.All);

            using (var file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var reader = new StreamReader(file))
                {
                    var script = (TSqlScript) parser.Parse(reader, out var errors);
                    if (errors.Count != 0)
                    {
                        // throw new Exception(String.Join(Environment.NewLine, errors));
                    }

                    return script;
                }
            }
        }
    }
}
