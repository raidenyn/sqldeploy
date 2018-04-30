# SqlDeploy

A little project for creation new databases in MS SQL Server from SQL Project files under .NET Core and Docker environment.

[![Build status](https://ci.appveyor.com/api/projects/status/oi9nqy36rgj5b9se/branch/master?svg=true)](https://ci.appveyor.com/project/raidenyn/sqldeploy/branch/master)

Unfortunately we cannot use Microsoft SSDT under Linux environment to create/migrate database version. But sometimes we want to run our tests from Docker and automatic database creation is really important detail for this. This library allows you to create and remove database easily.

### NuGet installation
``` cmd
dotnet add package SqlDeploy
```

### Limitations
* The lib can create database only. It cannot migrate or update exists databases.
* Creation script doesn't sort your DB objects by relations nad creates objects one by one as they are defined in *.sqlproj file. So you have to sort you objects manually to prevent errors bad relations order.

### Usage sample

``` cs
// Create a deployer
var deployer = new SqlProjectDeployer("path/to/your.sqlproj");

// Create a new db
var database = await deployer.RecreateToAsync(
    new SqlConnection("<connection string to MS SQL>"), 
    databaseName: "NewDBName");

// You can work with the new DB here

// Delete the database after tests
await database.DeleteAsync();
```

