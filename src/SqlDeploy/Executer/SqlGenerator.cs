using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace SqlDeploy.Executer
{
    public class SqlGenerator
    {
        private readonly SqlScriptGenerator _generator = new Sql140ScriptGenerator(new SqlScriptGeneratorOptions());

        public string GetSql(TSqlBatch sqlBatch)
        {
            _generator.GenerateScript(sqlBatch, out string sql);
            return sql;
        }
    }
}
