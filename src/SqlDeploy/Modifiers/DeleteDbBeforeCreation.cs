using System;
using System.Linq;
using SqlDeploy.Executer;

namespace SqlDeploy.Modifiers
{
    internal class DeleteDbBeforeCreation: IModifier
    {
        public void Modify(SqlProject project)
        {
            var dbName = project.GetDatabaseName();

            if (dbName == null)
            {
                throw new Exception("DbName is not defined");
            }

            var sqlScript = SqlCommands.GetDeleteDb(dbName);

            foreach (var sqlBatch in sqlScript.Batches.Reverse())
            {
                project.Database.Batches.Insert(0, sqlBatch);
            }
        }
    }
}
