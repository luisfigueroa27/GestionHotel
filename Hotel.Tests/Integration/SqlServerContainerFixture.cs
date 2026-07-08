using Microsoft.Data.SqlClient;
using Testcontainers.MsSql;

namespace Hotel.Tests.Integration;

[TestClass]
public class SqlServerContainerFixture
{
    public static MsSqlContainer Container = null!;

    [AssemblyInitialize]
    public static async Task Init(TestContext _)
    {
        Container = new MsSqlBuilder()
            .Build();

        await Container.StartAsync();

        // Conectar a master
        var builder =
            new SqlConnectionStringBuilder(
                Container.GetConnectionString());

        builder.InitialCatalog = "master";

        await using var connection =
            new SqlConnection(builder.ConnectionString);

        await connection.OpenAsync();

        // Crear HotelTests si no existe
        var command =
            connection.CreateCommand();

        command.CommandText = @"
IF DB_ID('HotelTests') IS NULL
BEGIN
    CREATE DATABASE HotelTests;
END
";

        await command.ExecuteNonQueryAsync();

        // Crear el esquema de EF Core
        using var context =
            TestDbFactory.Create();

        await context.Database.EnsureCreatedAsync();
    }

    [AssemblyCleanup]
    public static async Task Cleanup()
    {
        if (Container != null)
            await Container.DisposeAsync();
    }
}