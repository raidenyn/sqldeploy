using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace SqlDeploy.SqlGenerator
{
    public class DatabaseGenerator
    {
        private readonly SqlProject _sqlProject;

        public DatabaseGenerator(SqlProject sqlProject)
        {
            _sqlProject = sqlProject;
        }

        public string GetSql()
        {
            var generator = new Sql140ScriptGenerator(new SqlScriptGeneratorOptions());

            generator.GenerateScript(_sqlProject.Objects, out string sql);

            return sql;
        }
    }
}
