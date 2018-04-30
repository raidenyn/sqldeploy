using System;
using SqlDeploy.Readers;

namespace SqlDeploy
{
    class Program
    {
        static void Main(string[] args)
        {
            var reader = new SqlProjectReader(@"D:\projects\privates\webapiboilerplate\db\WebApiBoilerplate.Database\WebApiBoilerplate.Database.sqlproj");

            reader.ReadProject();
        }
    }
}
