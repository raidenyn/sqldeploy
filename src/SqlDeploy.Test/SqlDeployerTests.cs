using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SqlDeploy.Test
{
    public class SqlDeployerTests
    {
        public Task<ISqlDatabase> CreateDeployDb1()
        {
            var deployer = new SqlProjectDeployer("db-1/DB1.sqlproj");
            
            return deployer.RecreateToAsync(new SqlConnection(SqlServerConnectionString.Current), databaseName: "DB1");
        }

        [Test]
        public async Task Db1ExistsTest()
        {
            using (var database = await CreateDeployDb1())
            {
                using (var queryConnection = new SqlConnection(database.ConnectionString))
                {
                    await queryConnection.OpenAsync();

                    var queryCommand = queryConnection.CreateCommand();

                    queryCommand.CommandText = "select top(1) hi from IdentityInfo";

                    var result = await queryCommand.ExecuteScalarAsync();

                    Assert.AreEqual(1000, result);
                }
            }
        }

        [Test]
        public async Task Db1RemovedTest()
        {
            using (var database = await CreateDeployDb1())
            {
                Assert.IsTrue(DatabaseIsExists());

                await database.DeleteAsync();

                Assert.IsFalse(DatabaseIsExists());
            }
        }

        private bool DatabaseIsExists()
        {
            using (var queryConnection = new SqlConnection(SqlServerConnectionString.Current))
            {
                queryConnection.Open();

                var queryCommand = queryConnection.CreateCommand();

                queryCommand.CommandText =
                    "select DB_ID(N'DB1')";

                var result = queryCommand.ExecuteScalar();

                return result != DBNull.Value;
            }
        }
    }
}
