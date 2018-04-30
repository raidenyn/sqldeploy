using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace SqlDeploy
{
    public class SqlProject
    {
        public TSqlScript Objects { get; set; }

        public TSqlScript PreDeploy { get; set; }

        public TSqlScript PostDeploy { get; set; }
    }
}
