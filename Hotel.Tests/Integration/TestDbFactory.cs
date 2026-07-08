using Hotel.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Tests.Integration;

public static class TestDbFactory
{
    public static HotelDbContext Create()
    {
        var builder =
            new SqlConnectionStringBuilder(
                SqlServerContainerFixture.Container
                    .GetConnectionString());

        // Cambiamos la base master por HotelTests
        builder.InitialCatalog = "HotelTests";

        var options =
            new DbContextOptionsBuilder<HotelDbContext>()
                .UseSqlServer(builder.ConnectionString)
                .Options;

        return new HotelDbContext(options);
    }
}