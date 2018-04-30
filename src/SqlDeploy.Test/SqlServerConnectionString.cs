using System;

namespace SqlDeploy.Test
{
    public class SqlServerConnectionString
    {
        private const string DockerConnectionString = "Data Source=mssql;User ID=sa;Password=wiEPzF9pXnuVuejTN3p7;";

        private const string WindowConnectionString = "Data Source=localhost,14338;Persist Security Info=True;User ID=sa;Password=wiEPzF9pXnuVuejTN3p7;";


        private static readonly Lazy<string> LazyConnectionString = new Lazy<string>(() =>
        {
            var connectionString = Environment.GetEnvironmentVariable("SqlDeployConnectionString");
            if (connectionString != null)
            {
                return connectionString;
            }

            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                return DockerConnectionString;
            }

            return WindowConnectionString;
        });

        public static string Current => LazyConnectionString.Value;
    }
}
